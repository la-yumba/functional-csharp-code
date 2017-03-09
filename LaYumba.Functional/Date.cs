using System;

namespace LaYumba.Functional
{
   using static F;

   public static class Date
   {
      public static Option<DateTime> Parse(string s)
         => DateTime.TryParse(s, out DateTime d) ? Some(d) : None;
   }
}
