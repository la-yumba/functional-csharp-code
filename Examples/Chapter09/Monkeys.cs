using LaYumba.Functional;
using System;

namespace Examples.Chapter10.Data
{
   enum Ripeness { Green, Yellow, Brown }

   abstract class Reward
   {
      public abstract R Match<R>(Func<Ripeness, R> Banana, Func<R> Peanut);
   }
   class Peanut : Reward
   {
      public override R Match<R>(Func<Ripeness, R> Banana, Func<R> Peanut)
         => Peanut();
   }
   class Banana : Reward
   {
      public Ripeness Ripeness;
      public override R Match<R>(Func<Ripeness, R> Banana, Func<R> Peanut)
         => Banana(Ripeness);
   }
   class PatternMatching
   {
      void PatternMatchingExceptions()
      {
         try { SomeOperationThatCanFail(); }
         catch (OverflowException ex) { /* ... */ }
         catch (InvalidOperationException ex) { /* ... */ }
         catch (InvalidCastException ex) { /* ... */ }
      }

      void SomeOperationThatCanFail()
      {
         throw new NotImplementedException();
      }

      string DescribeChoiceOf_Dull(Reward reward)
      {
         Peanut peanut = reward as Peanut;
         if (peanut != null)
            return "It's a peanut";

         Banana banana = reward as Banana;
         if (banana != null)
            return $"It's a {banana.Ripeness} banana";

         throw new InvalidOperationException("Unknown reward");
      }

      string DescribeWithIs(Reward reward)
      {
         if (reward is Banana banana)
            return $"It's a {banana.Ripeness} banana";

         else if (reward is Peanut _)
            return "It's a peanut";

         return "It's a reward I don't know or care about";
      }

      string DescribeWithSwitch(Reward reward)
      {
         switch (reward)
         {
            case Banana banana:
               return $"It's a {banana.Ripeness} banana";
            case Peanut _:
               return "It's a peanut";
            default:
               return "It's a reward I don't know or care about";
         }
      }

      string DescribeChoiceOf(Reward reward)
         => reward.Match(
            Peanut: () => "It's a peanut",
            Banana: r => $"It's a {r} banana");

      string __DescribeChoiceOf(Reward reward)
         => new Pattern<string>
         {
            (Peanut _) => "It's a peanut",
            (Banana b) => $"It's a {b.Ripeness} banana"
         }
         .Match(reward);

      string _DescribeChoiceOf(Reward reward)
      {
         if (reward is Banana)
         {
            var banana = reward as Banana;
            return $"It's a {banana.Ripeness} banana";
         }
         else if (reward is Peanut)
         {
            return "It's a peanut";
         }
         else throw new UnknownRewardTypeException();
      }
   }
}
