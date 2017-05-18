using System;
using System.Linq;
using LaYumba.Functional;
using Double = LaYumba.Functional.Double;

namespace Examples.Chapter8.Linq
{
   using static Console;
   using static F;

   class OptionExamples
   {
      static string Prompt(string msg)
      {
         WriteLine(msg);
         return ReadLine();
      }

      internal static void _main()
      {
         WriteLine("Enter a number, for which this program will calculate the square root:");
         var input = ReadLine();

         var result = from d in Double.Parse(input)
                      where 0 <= d
                      select Math.Sqrt(d);

         WriteLine(result.Match(
            Some: r => $"sqrt({input}) = {r}",
            None: () => "Please enter a valid, positive number"));

         ReadKey();
      }

      internal static void Hypothenuse()
      {
         string s1 = Prompt("First leg:")
              , s2 = Prompt("Second leg:");

         var result = from a in Double.Parse(s1)
                      let aa = a * a
                      from b in Double.Parse(s2)
                      let bb = b * b
                      select Math.Sqrt(aa + bb);

         WriteLine(result.Match(
            () => "Please enter two valid, positive numbers",
            (h) => $"The hypothenuse is {h}"));
      }

      internal static void HypothenuseFilter()
      {
         string s1 = Prompt("First leg:")
              , s2 = Prompt("Second leg:");

         var result = from a in Double.Parse(s1)
                      where a >= 0
                      let aa = a * a

                      from b in Double.Parse(s2)
                      where b >= 0
                      let bb = b * b

                      select Math.Sqrt(aa + bb);

         WriteLine(result.Match(
            () => "Please enter two valid, positive numbers",
            (h) => $"The hypothenuse is {h}"));
      }

      public static Option<bool> IsPalindromeDate(string s)
         => from date in Date.Parse(s)
            let forward = date.ToString("ddMMyyyy")
            let backward = forward.Reverse().ToString()
            select forward == backward;

      internal static void _main_1()
      {
         WriteLine("Enter first addend:");
         var s1 = ReadLine();

         WriteLine("Enter second addend:");
         var s2 = ReadLine();

         var result = from a in Int.Parse(s1)
                      from b in Int.Parse(s2)
                      select a + b;

         // alternatively, any of the following
         result = Int.Parse(s1).Bind(a => Int.Parse(s2).Map(b => a + b));
         result = Int.Parse(s1).SelectMany(a => Int.Parse(s2), (a, b) => a + b);
         result = Some(new Func<int, int, int>((a, b) => a + b))
            .Apply(Int.Parse(s1)).Apply(Int.Parse(s2));

         WriteLine(result.Match(
            Some: r => $"{s1} + {s2} = {r}",
            None: () => "Please enter 2 valid integers"));

         ReadKey();
      }

      internal static void _main_()
      {
         SimpleSelectExample();
         WriteLine();
         WriteLine(DoubleOf("12"));
         WriteLine(DoubleOf("not an int"));
         //         SimpleSelectManyExample();

         ReadKey();
      }

      public static void SimpleSelectExample()
      {
         var a =
            from x in Some(1)
            select x * 2;

         var b = from x in (Option<int>)None
                 select x * 2;

         var c =
            (from x in Some(1)
             select x * 2) == Some(1).Map(x => x * 2);

         WriteLine(a);
         WriteLine();
         WriteLine(b);
         WriteLine();
         WriteLine(c);
         WriteLine();
      }

      public static Option<int> DoubleOf(string s)
         => from i in Int.Parse(s)
            select i * 2;

      public static void SimpleSelectManyExample()
      {
         var a =
            from c in Range('a', 'c')
            from i in Range(1, 2)
            select (c, i);

         var b =
            Range('a', 'c')
               .SelectMany(c => Range(1, 2)
                  .Select(i => (c, i)));

         a.ForEach(t => WriteLine($"({t.Item1}, {t.Item2})"));
         WriteLine();
         b.ForEach(t => WriteLine($"({t.Item1}, {t.Item2})"));
         WriteLine();
      }
   }
}
