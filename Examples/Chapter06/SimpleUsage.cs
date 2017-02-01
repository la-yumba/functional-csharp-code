using LaYumba.Functional;
using static LaYumba.Functional.F;
using NUnit.Framework;
using static System.Math;

partial class Either_Example
{
   Either<string, double> Calc(double x, double y)
   {
      if (y == 0)
         return "y cannot be 0";

      if (x != 0 && Sign(x) != Sign(y))
         return "x / y cannot be negative";

      return Sqrt(x / y);
   }

   void UseMatch(double x, double y)
   {
      var message = Calc(x, y).Match(
         Right: z => $"Result: {z}",
         Left: err => $"Invalid input: {err}");
   }

   Either<Error, int> Run(double x, double y)
      => Calc(x, y)
         .Map(
            left: msg => Error(msg),
            right: d => d)
         .Bind(ToIntIfWhole);
      
   Either<Error, int> ToIntIfWhole(double d)
   {
      if ((int)d == d) return (int)d;
      return Error($"Expected a whole number but got {d}");
   }
}


partial class Either_Example
{
   [TestCase(1d, 0d, ExpectedResult = false, TestName = "When y is 0 Calc fails")]
   [TestCase(-90d, 10d, ExpectedResult = false, TestName = "When x/y < 0 Calc fails")]
   [TestCase(90d, 10d, ExpectedResult = true, TestName = "Otherwise Calc succeeds")]
   public bool TestCalc(double x, double y) => Calc(x, y).Match(_ => false, _ => true);
}
