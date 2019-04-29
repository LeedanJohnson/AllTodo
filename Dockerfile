FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY AllTodo.Server/AllTodo.Server.csproj AllTodo.Server/
RUN dotnet restore "AllTodo.Server/AllTodo.Server.csproj"
COPY . .
WORKDIR "/src/AllTodo.Server"
RUN dotnet build "AllTodo.Server.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AllTodo.Server.csproj" -c Release -o /app
# Stuff I don't understand
ENV certPassword 1234
# Use opnssl to generate a self signed certificate cert.pfx with password $env:certPassword
RUN openssl genrsa -des3 -passout pass:${certPassword} -out server.key 2048
RUN openssl rsa -passin pass:${certPassword} -in server.key -out server.key
RUN openssl req -sha256 -new -key server.key -out server.csr -subj '/CN=localhost'
RUN openssl x509 -req -sha256 -days 365 -in server.csr -signkey server.key -out server.crt
RUN openssl pkcs12 -export -out /app/cert.pfx -inkey server.key -in server.crt -certfile server.crt -passout pass:${certPassword}
# End: Stuff I don't understand (Lies, I don't understand any of it. I just REALLY don't understand this part)

FROM base AS final
EXPOSE 443
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AllTodo.Server.dll"]
