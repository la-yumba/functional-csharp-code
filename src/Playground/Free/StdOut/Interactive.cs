using LaYumba.Functional;

namespace Playground.Free
{
   using static FreeStdOutFactory;

   public static class Interactive
   {
      public static Free<Unit> AgeWorkflow =>
         from age in Ask("What's your age?")
         from _   in Tell($"Only {age}! That's young!")
         select _;

      //   Ask("What's your age?")

      //   More(
      //      Ask(
      //         Prompt: "What's your age?",
      //         Next: age => Done(
      //            Value: age
      //         )
      //      )
      //   )

      //   Bind

      //   age =>
      //   More(
      //      Tell(
      //         Message: $"Only {age}! That's young!",
      //         Result: Done(
      //            Value: Unit()
      //         )
      //      )
      //   )

      public static Free<Unit> NameWorkflow =>
         from first in Ask("What's your first name?")
         from last  in Ask("What's your last name?")
         from _     in Tell($"Hello {first} {last}!")
         select _;
   }
}
