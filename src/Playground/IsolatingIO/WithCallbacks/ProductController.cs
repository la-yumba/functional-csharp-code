using System;
using System.Collections.Generic;
using System.Linq;
using Boc;
using Dapper;
using Examples.Chapter1.DbLogger;
using Examples.Domain;
using LaYumba.Functional;

namespace Examples.IsolatingIO.WithCallbacks
{
   internal class Product : IEntity
   {
      public Product(Guid id, decimal price)
      {
         Id = id;
         Price = price;
      }

      public Guid Id { get; }
      public decimal Price { get; }

      internal Product WithPrice(decimal newPrice)
         => new Product(this.Id, newPrice);
   }

   internal static class ProductExt
   {
      public static Either<Error, Product> UpdatePrice(this Product @this, decimal newPrice)
      {
         if (newPrice < @this.Price * 0.8m) return new PriceDecreaseTooGreat();
         if (@this.Price * 1.2m < newPrice) return new PriceIncreaseTooGreat();
         return @this.WithPrice(newPrice);
      }

      public class PriceIncreaseTooGreat : Error
      {
         public override string Message 
            => "Price cannot increase by more than 20%";
      }

      public class PriceDecreaseTooGreat : Error
      {
         public override string Message 
            => "Price cannot decrease by more than 20%";
      }
   }

   class ProductController
   {
      IRepository<Product> products;

      DbDriver db = new DbDriver("");

      public void UpdatePrice(UpdatePrice request)
      {
         ProductService.UpdatePrice(request)
            .Match((readArgs, callback) =>
            {
               var product = db.Read<Product>(readArgs).Single();
               var writeOpArgs = callback(product);
               db.Write(writeOpArgs);
            });
      }
   }

   internal static class ProductService
   {
      public static Tuple<DbOpArgs, Func<Product, DbOpArgs>> UpdatePrice(UpdatePrice request) 
         => F.Tuple(ReadProductSql(request.ProductId), ProductReadCallback(request));

      // specifies how to read the product from the DB
      static DbOpArgs ReadProductSql(Guid productId)
         => new DbOpArgs("SELECT * FROM [Products] WHERE [Id] = @Id", new { Id = productId });

      // specifies how to update the product
      static Func<Product, DbOpArgs> ProductReadCallback(UpdatePrice request)
         => product => WriteProductSql(product.WithPrice(request.Price));

      // specifies how to save the product to the DB
      static DbOpArgs WriteProductSql(Product product)
         => new DbOpArgs("UPDATE [Products] SET [Price] = @Price WHERE [Id] = @Id", product);
   }

   internal class DbDriver
   {
      string connString;

      public DbDriver(string connString)
      {
         this.connString = connString;
      }

      public IEnumerable<T> Read<T>(DbOpArgs args)
         => ConnectionExt.Connect(connString
            , conn => conn.Query<T>(args.Sql, args.Param));

      public void Write(DbOpArgs args)
         => ConnectionExt.Connect(connString
            , conn => conn.Execute(args.Sql, args.Param));
   }

   internal struct DbOpArgs
   {
      public DbOpArgs(string sql, object param)
      {
         Sql = sql;
         Param = param;
      }

      public string Sql { get; }
      public object Param { get; }
   }

   internal class UpdatePrice
   {
      public Guid ProductId { get; set; }
      public decimal Price { get; set; }
   }
}
