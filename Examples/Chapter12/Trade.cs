using LaYumba.Functional;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatefulComputations
{
   using static StatefulComputation<State>;
   using static F;
   using NUnit.Framework;

   struct Bid
   {
      public int Id { get; set; }
      public decimal PriceLimit { get; set; }
      public int Quantity { get; set; }

      public Bid Fill(int quantity) => new Bid
      {
         Quantity = this.Quantity - quantity,
         Id = this.Id,
         PriceLimit = this.PriceLimit,
      };
   }

   struct Offer
   {
      public int Id { get; set; }
      public decimal Price { get; set; }
      public int Quantity { get; set; }
   }

   struct Trade
   {
      public int BidId { get; set; }
      public int OfferId { get; set; }
      public int Quantity { get; set; }
   }

   struct State
   {
      public Bid Bid { get; set; }
      public IEnumerable<Offer> Offers { get; set; }

      public static implicit operator State((Bid, IEnumerable<Offer>) t)
         => new State
         {
            Bid = t.Item1,
            Offers = t.Item2,
         };
   }

   static class BidOfferMatcher
   {
      public static IEnumerable<Trade> Match(Bid bid, IEnumerable<Offer> offers)
      {
         var filteredOffers =
            from o in offers
            where o.Price <= bid.PriceLimit
            orderby o.Price select o;

         State state = (bid, filteredOffers);
         return GetTrades.Run(state);
      }
      
      static StatefulComputation<State, IEnumerable<Trade>> GetTrades =>
         from state in Get
         from trades in state.Bid.Quantity == 0
            ? Empty
            : state.Offers.Match(
               () => Empty,
               (bestOffer, otherOffers) => 
                  from result in Return(CreateTrade(state.Bid, bestOffer))
                  from _ in Put((result.FilledBid, otherOffers))
                  from trades in GetTrades // recursively call GetTrades with the new state
                  select List(result.Trade).Concat(trades))
         select trades;

      static (Bid FilledBid, Trade Trade) CreateTrade(Bid bid, Offer offer) 
      {
         var tradedQuantity = Math.Min(bid.Quantity, offer.Quantity);
         var trade = new Trade
         {
            BidId = bid.Id,
            OfferId = offer.Id,
            Quantity = tradedQuantity,
         };
         return (bid.Fill(tradedQuantity), trade);
      }

      static StatefulComputation<State, IEnumerable<Trade>> Empty = Return(Enumerable.Empty<Trade>());
   }

   public static class BidOfferMatcherTest
   {
      [Test] public static void EmptyTuplesAreEqual()
      {
         var a = new ValueTuple();
         var b = new ValueTuple();
         Assert.AreEqual(a, b);
      }

      [Test] public static void Test()
      {
         Bid bid = new Bid
         {
            Id = 123,
            PriceLimit = 100,
            Quantity = 50
         };

         Offer[] offers =
         {
            new Offer { Id = 1, Price = 98, Quantity = 25 },
            new Offer { Id = 2, Price = 99, Quantity = 20 },
            new Offer { Id = 3, Price = 100, Quantity = 20 },
            new Offer { Id = 4, Price = 101, Quantity = 10 },
         };

         Trade[] expected =
         {
            new Trade { BidId = 123, OfferId = 1, Quantity = 25 },
            new Trade { BidId = 123, OfferId = 2, Quantity = 20 },
            new Trade { BidId = 123, OfferId = 3, Quantity = 5 },
         };

         Trade[] actual = BidOfferMatcher.Match(bid, offers).ToArray();

         Assert.AreEqual(expected, actual);
      }
   }
}
