using System;

namespace Examples.Chapter2
{
   public static class ParseInt_FrameworkFunctions
   {
      public static int Parse1(string value)
      {
         try
         {
            return int.Parse(value);
         }
         catch (Exception)
         {
            return 0;
         }
      }

      public static int Parse2(string value)
      {
         int result;
         int.TryParse(value, out result);
         return result;
      }
   }
}