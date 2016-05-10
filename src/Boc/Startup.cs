using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen;

namespace Boc
{
    public class Startup
    {   
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen();
            services.ConfigureSwaggerDocument(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Examples",
                    Description = "Examples for Functional Programming in C#",
                });
            });
            services.ConfigureSwaggerSchema(options =>
            {
                options.DescribeAllEnumsAsStrings = true;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseSwaggerGen();
            app.UseSwaggerUi();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
