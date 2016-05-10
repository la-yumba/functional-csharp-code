using NUnit.Framework;

namespace LaYumba.Functional.Tests.Option
{
   [TestFixture]
   public class Members
   {
      [TestCase("Hello", ExpectedResult = true)]
      [TestCase(null, ExpectedResult = false)]
      public bool ValidOption_IsSome(string s)
         => F.Some(s).IsSome;

      [TestCase("John", ExpectedResult = "hello, John")]
      [TestCase(null, ExpectedResult = "sorry, who?")]
      public string MatchCallsAppropriateFunc(string name)
         => F.Some(name).Match(
            Some: n => $"hello, {n}",
            None: () => "sorry, who?");

      [TestCase(ExpectedResult = true)]
      public bool NoneField_IsNone() => Option<string>.None.IsNone;
   }
}