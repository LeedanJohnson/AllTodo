using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllTodo.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;

namespace AllTodo.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            IDateTimeProvider datetimeprovider = new SimpleDateTimeProvider();
            IUserService userservice = new VolatileUserService(datetimeprovider);
            ITodoService todoservice = new VolatileTodoService(userservice);


            services.AddSingleton<IDateTimeProvider>(datetimeprovider);
            services.AddSingleton<IUserService>(userservice);
            services.AddSingleton<ITodoService>(todoservice);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRewriter(new RewriteOptions().AddRedirectToHttps(301, 44343));

            app.UseMvc();
        }
    }
}
