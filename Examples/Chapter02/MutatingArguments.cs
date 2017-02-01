using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples.Chapter2
{
   class MutatingArguments
   {
      decimal RecomputeTotal_MutatingArguments(Order order, List<OrderLine> linesToDelete)
      {
         var result = 0m;
         foreach (var line in order.OrderLines)
            if (line.Quantity == 0) linesToDelete.Add(line);
            else result += line.Product.Price * line.Quantity;
         return result;
      }

      (decimal, IEnumerable<OrderLine>) RecomputeTotal_NoSideEffects(Order order)
         => ( order.OrderLines.Sum(l => l.Product.Price * l.Quantity)
            , order.OrderLines.Where(l => l.Quantity == 0));

      class Product
      {
         public decimal Price { get; }
      }

      class OrderLine
      {
         public Product Product { get; }
         public int Quantity { get; }
      }

      class Order
      {
         public IEnumerable<OrderLine> OrderLines { get; } 
      }
   }
}
