using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Internal;
using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Boc.Commands;
using LaYumba.Functional;
using System.Collections.Generic;
using Examples;
using Boc.Chapter7.FunctionsEverywhere;

namespace Boc.Chapter7.Delegate
{
   public class ControllerActivator : IControllerActivator
   {
      internal ILoggerFactory loggerFactory { get; set; }
      DefaultControllerActivator defaultActivator;
      TypeActivatorCache typeActivatorCache;
      IConfigurationRoot configuration;

      public ControllerActivator(IConfigurationRoot configuration)
      {
         typeActivatorCache = new TypeActivatorCache();
         defaultActivator = new DefaultControllerActivator(typeActivatorCache);
         this.configuration = configuration;
      }

      public object Create(ControllerContext context)
      {
         var type = context.ActionDescriptor.ControllerTypeInfo;
         if (type.AsType().Equals(typeof(BookTransferController_FunctionDependencies)))
            return ConfigureTransferOnsController(context.HttpContext.RequestServices);

         return defaultActivator.Create(context);
      }

      BookTransferController_FunctionDependencies ConfigureTransferOnsController(IServiceProvider serviceProvider)
      {
         ConnectionString connString = configuration.GetSection("ConnectionString").Value;
         var save = Sql.TryExecute.Apply(connString).Apply(Sql.Queries.InsertTransferOn);

         var validate = Validation.DateNotPast(() => DateTime.UtcNow);

         // var logger = loggerFactory.CreateLogger<BookTransferController_FunctionDependencies>();
         return new BookTransferController_FunctionDependencies(validate, save);
      }

      public void Release(ControllerContext context, object controller)
      {
         var disposable = controller as IDisposable;
         if (disposable != null) disposable.Dispose();
      }
   }

   public class UseCaseFactory
   {
      ILoggerFactory loggerFactory;
      IConfigurationRoot configuration;

      public UseCaseFactory(IConfigurationRoot configuration
         , ILoggerFactory loggerFactory)
      {
         this.loggerFactory = loggerFactory;
         this.configuration = configuration;
      }

      //public Func<TransferOn, IActionResult> PersistTransferOn(ControllerContext context)
      //{
      //   // can get other dependencies from here...
      //   IServiceProvider serviceProvider = context.HttpContext.RequestServices;
      //}

      public Func<BookTransfer, IActionResult> PersistTransferOn()
      {
         // // persistence layer
         // ConnectionString connString = configuration.GetSection("ConnectionString").Value;
         // var persist = Sql.TryExecute
         //    .Apply(connString)
         //    .Apply(Sql.Queries.InsertTransferOn);

         // // service layer
         // var validators = ConfigureTransferOnValidators();
         // var handle = TransferOnHandler.Handle
         //    .Apply(validators)
         //    .Apply(persist);

         // // api layer
         // var logger = loggerFactory.CreateLogger<TransfersController_FunctionDependencies>();
         // return UseCases.TransferOn
         //    .Apply(logger)
         //    .Apply(handle);
         throw new NotImplementedException();
      }

      public void Release(ControllerContext context, object controller)
      {
         var disposable = controller as IDisposable;
         if (disposable != null) disposable.Dispose();
      }
   }
}
