FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
ENV ASPNETCORE_URLS=https://+:443
WORKDIR /src
COPY AllTodo.Server/AllTodo.Server.csproj AllTodo.Server/
RUN dotnet restore "AllTodo.Server/AllTodo.Server.csproj"
COPY . .
WORKDIR "/src/AllTodo.Server"
RUN dotnet build "AllTodo.Server.csproj" -c Release -o /app

FROM build AS publish
ENV ASPNETCORE_URLS=https://+:443
RUN dotnet publish "AllTodo.Server.csproj" -c Release -o /app

FROM base AS final
ENV ASPNETCORE_URLS=https://+:443
EXPOSE 443
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AllTodo.Server.dll"]
