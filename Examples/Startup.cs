using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Boc.Chapter7.Delegate;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using Boc.Commands;
using Microsoft.AspNetCore.Mvc;

using LaYumba.Functional;

namespace Boc
{
   public class ConnectionStrings
   {
      public string Default { get; set; }
   }

   public class Startup
   {
      public Startup(IHostingEnvironment env)
      {
         var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
             .AddEnvironmentVariables();
         Configuration = builder.Build();
      }

      string DefaultApiRoot => "localhost:8000/api";
      string GetApiRoot(IConfigurationRoot config)
         => config.Lookup("ApiRoot").GetOrElse("localhost");

      public IConfigurationRoot Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddMvc();
         services.AddLogging();

         // configuration
         services.AddOptions();
         services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

         // relevant for chapter 7
         var ctrlActivator = new ControllerActivator(Configuration);
         services.AddSingleton<IControllerActivator>(ctrlActivator);
         services.AddSingleton<ControllerActivator>(ctrlActivator);

         services.AddSwaggerGen();
         //services.ConfigureSwaggerGen(options =>
         //{
         //   options.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
         //   {
         //      Version = "v1",
         //      Title = "Examples",
         //      Description = "Examples for Functional Programming in C#",
         //   });
         //   //options.IncludeXmlComments(pathToDoc);
         //   options.DescribeAllEnumsAsStrings();
         //});
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ControllerActivator ctrlActivator)
      {
         ctrlActivator.loggerFactory = loggerFactory;
         
         loggerFactory.AddConsole(Configuration.GetSection("Logging"));
         loggerFactory.AddDebug();
         
         //var routeBuilder = new RouteBuilder(app);
         //routeBuilder.MapRoute("echo", (context) =>
         //{
         //   var body = new StreamReader(context.Request.Body).ReadToEnd();

         //   // context.GetRouteData().Values
         //   return context.Response.WriteAsync(body);
         //});

         //app.UseRouter(routeBuilder.Build());

         var useCases = new UseCaseFactory(Configuration, loggerFactory);

         // demonstrates how you can just have all your logic live in functions;
         // but this fails to provide many niceties you get when using a Controller
         app.Map("/api/transferOn", a => a.Run(async ctx =>
         {
            BookTransfer transfer = await Parse<BookTransfer>(ctx.Request.Body);
            IActionResult result = useCases.PersistTransferOn()(transfer);
            await WriteResponse(ctx.Response, result);
         }));

         app.UseMvcWithDefaultRoute();

         app.UseSwagger();
         app.UseSwaggerUi();
      }

      private Task WriteResponse(HttpResponse response, IActionResult result)
      {
         throw new NotImplementedException();
      }

      private Task<T> Parse<T>(Object body)
      {
         throw new NotImplementedException();
      }
   }
}
