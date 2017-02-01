namespace LaYumba.Functional
{
   using static F;

   public static class Int
   {
      public static Option<int> Parse(string s)
      {
         int result;
         return int.TryParse(s, out result)
            ? Some(result) : None;
      }

      public static bool IsOdd(int i) => i % 2 == 1;

      public static bool IsEven(int i) => i % 2 == 0;
   }
}
