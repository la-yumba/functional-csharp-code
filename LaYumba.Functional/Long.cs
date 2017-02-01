namespace LaYumba.Functional
{
   using static F;

   public static class Long
   {
      public static Option<long> Parse(string s)
      {
         long result;
         return long.TryParse(s, out result)
            ? Some(result) : None;
      }
   }
}
