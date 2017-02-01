using Microsoft.Extensions.Configuration;
using System;

using LaYumba.Functional;
using static LaYumba.Functional.F;
using System.Linq;
using NUnit.Framework;

namespace Boc
{
   static class ConfigurationExt
   {
      public static Option<T> Lookup<T>
         (this IConfigurationRoot config, params string[] path)
         => (T)Convert.ChangeType(config.GetSection(path).Value, typeof(T));

      public static Option<string> Lookup
         (this IConfigurationRoot config, params string[] path)
         => config.GetSection(path).Value;

      public static IConfigurationSection GetSection
         (this IConfigurationRoot config, params string[] path)
         => (IConfigurationSection)path.Aggregate(config as IConfiguration, (parent, name) => parent.GetSection(name));
   }

   static class ConfigurationExtTest
   {
      static IConfigurationRoot GetConfig()
      {
         var builder = new ConfigurationBuilder();
         builder.AddInMemoryCollection();
         var config = builder.Build();
         config["somekey"] = "somevalue";
         return config;
      }

      [Test]
      public static void WhenAValueIsAvailable_LookupReturnsSome() => Assert.AreEqual(
         actual: GetConfig().Lookup("somekey"),
         expected: Some("somevalue"));

      [Test]
      public static void WhenAValueIsNotAvailable_LookupReturnsNone() => Assert.AreEqual(
         actual: GetConfig().Lookup("_"),
         expected: None);
   }
}
