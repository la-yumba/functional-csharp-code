using Xunit;

namespace LaYumba.Functional.Tests
{
   public class PatternTest
   {
      class Super { }
      class SubA : Super { }
      class SubB : Super { }

      [Fact]
      public void ItMatchesTheCorrectType()
      {
         Super a = new SubA()
             , b = new SubB();

         var patt = new Pattern<string>
         {
            (SubA _) => "a",
            (SubB _) => "b",
         };

         Assert.Equal("a", patt.Match(a));
         Assert.Equal("b", patt.Match(b));
      }

      [Fact]
      public void ItCanHaveADefaultClause()
      {
         Super a = new SubA()
             , b = new SubB();

         var patt = new Pattern<string>
         {
            (SubA _) => "a",
            (SubB _) => "b",
         }
         .Default(() => "other");

         Assert.Equal("a", patt.Match(a));
         Assert.Equal("b", patt.Match(b));
         Assert.Equal("other", patt.Match("hello"));
      }

      [Fact]
      public void DefaultClauseCanTakeAValue()
      {
         var patt = new Pattern<string>
         {
            (SubA _) => "a",
            (SubB _) => "b",
         }
         .Default("other");
         
         Assert.Equal("other", patt.Match("hello"));
      }

      [Fact]
      public void FirstMatchingClauseWins()
      {
         Super a = new SubA()
             , b = new SubB();

         var patt = new Pattern<string>
         {
            (SubA _) => "a",
            (object _) => "other",
            (SubB _) => "b",
         };

         Assert.Equal("a", patt.Match(a));
         Assert.Equal("other", patt.Match(b));
      }

      [Fact]
      public void ItThrowsWhenNoMatchSpecified()
      {
         Super b = new SubB();

         var patt = new Pattern<string>
         {
            (SubA _) => "a",
         };

         Assert.Throws<NonExhaustivePattern>(() => patt.Match(b));
      }
   }
}
