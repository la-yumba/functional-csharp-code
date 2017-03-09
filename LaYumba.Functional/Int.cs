namespace LaYumba.Functional
{
   using static F;

   public static class Int
   {
      public static Option<int> Parse(string s)
         => int.TryParse(s, out int i) ? Some(i) : None;

      public static bool IsOdd(int i) => i % 2 == 1;

      public static bool IsEven(int i) => i % 2 == 0;
   }
}
