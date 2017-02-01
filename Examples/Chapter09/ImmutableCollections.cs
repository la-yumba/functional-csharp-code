using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Examples.Chapter0.Introduction
{
   public class Order_MutableList
   {
      public Guid CustomerId { get; }
      public DateTime CreatedAt { get; }
      public List<OrderLine> OrderLines { get; }

      public Order_MutableList(Guid customerId, DateTime createdAt
         , List<OrderLine> orderLines)
      {
         this.CustomerId = customerId;
         this.CreatedAt = createdAt;
         this.OrderLines = orderLines;
      }
   }

   public class Order
   {
      public Guid CustomerId { get; }
      public DateTime CreatedAt { get; }
      public ImmutableList<OrderLine> OrderLines { get; }

      public Order(Guid customerId, DateTime createdAt
         , IEnumerable<OrderLine> orderLines)
      {
         CustomerId = customerId;
         CreatedAt = createdAt;
         OrderLines = orderLines.ToImmutableList();
      }

      public Order AddOrderLine(OrderLine newLine)
         => new Order(CustomerId, CreatedAt, OrderLines.Add(newLine));
   }

   public class OrderLine
   {
   }

   class ImmutableCollections_Examples
   {
      public void ChangeMutableReadonlyList(Order order)
         => order.OrderLines.Clear();
   }
}
