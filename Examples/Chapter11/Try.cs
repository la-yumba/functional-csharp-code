using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Examples.Chapter11
{
   public class TryTests
   {
      Exceptional<Uri> Boilerplate_CreateUri(string uri)
      {
         try { return new Uri(uri); }
         catch (Exception ex) { return ex; }
      }

      Try<Uri> CreateUri(string uri) => () => new Uri(uri);

      Try<JObject> Parse(string s) => () => JObject.Parse(s);

      Try<Uri> ExtractUri(string json) =>
         from jObj in Parse(json)
         let uriStr = (string)jObj["Uri"]
         from uri in Try(() => new Uri(uriStr))
         select uri;

      [TestCase(@"{'Uri': 'http://github.com'}", ExpectedResult = "Ok")]
      [TestCase("{'Uri': 'rubbish'}", ExpectedResult = "Invalid URI: The format of the URI could not be determined.")]
      [TestCase("{}", ExpectedResult = "Value cannot be null.\r\nParameter name: uriString")]
      [TestCase("blah!", ExpectedResult = "Unexpected character encountered while parsing value: b. Path '', line 0, position 0.")]
      public string SuccessfulTry(string json)
         => ExtractUri(json).Run().Match(ex => ex.Message, _ => "Ok");
   }
}
