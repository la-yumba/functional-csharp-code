namespace LaYumba.Functional
{
   using static F;

   public static class Long
   {
      public static Option<long> Parse(string s)
         => long.TryParse(s, out long l) ? Some(l) : None;
   }
}
