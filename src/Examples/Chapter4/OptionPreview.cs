using System;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace Examples.Chapter1
{
   /// <summary>
   /// Never again check for nulls
   /// </summary>
   public class OptionPreview
   {
      Map<CountryCode, Region> shippingRegionsByCountryCode;

      public Option<Region> GetDefaultShippingRegion(Customer customer)
         => Some(customer)
            .Bind(c => c.Addresses)
            .Find(a => a.IsPrimary)
            .Map(c => c.CountryCode)
            .Bind(shippingRegionsByCountryCode.Lookup);
   }


   public class ImperativeVersion
   {
      IDictionary<string, Region> shippingRegionsByCountryCode;

      public Region GetDefaultShippingRegion(Customer customer)
      {
         if (customer == null)
         {
            throw new ArgumentNullException(nameof(customer));
         }

         foreach (var address in customer.Addresses)
         {
            if (address.IsPrimary && address.CountryCode != null)
            {
               Region region;
               if (shippingRegionsByCountryCode.TryGetValue(address.CountryCode, out region))
               {
                  return region;
               }
            }
         }
         return null;
      }
   }

   public class Region
   {
   }

   public class CountryCode
   {
      private readonly string code;

      public CountryCode(string code)
      {
         this.code = code;
      }

      public static implicit operator string (CountryCode self) => self.ToString();
      public static implicit operator CountryCode(string code) => new CountryCode(code);
   }


   public class Map<T, T1> : Dictionary<T, T1>
   {
   }
   public class Customer
   {
      public IEnumerable<Address> Addresses;
   }

   public class Address

   {
      public bool IsPrimary;
      public CountryCode CountryCode;
   }

   public static class Ext
   {
      public static Option<T> Find<T>(this IEnumerable<T> list, Predicate<T> pred)
         => list.FirstOrDefault(new Func<T, bool>(pred));

      public static Option<T> Lookup<K, T>(this IDictionary<K, T> map, K key)
         => map.ContainsKey(key) ? Some(map[key]) : None;
   }
}