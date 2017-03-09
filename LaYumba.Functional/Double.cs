namespace LaYumba.Functional
{
   using static F;

   public static class Double
   {
      public static Option<double> Parse(string s)
         => double.TryParse(s, out double d) ? Some(d) : None;
   }
}
