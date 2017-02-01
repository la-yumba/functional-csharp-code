namespace LaYumba.Functional
{
   using static F;

   public static class Double
   {
      public static Option<double> Parse(string s)
      {
         double result;
         return double.TryParse(s, out result)
            ? Some(result) : None;
      }
   }
}
