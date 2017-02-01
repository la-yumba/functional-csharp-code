namespace LaYumba.Functional
{
   using static F;
   
   public static class Enum
   { 
      public static Option<T> Parse<T>(this string s) where T : struct
      {
         T t;
         return System.Enum.TryParse(s, out t) ? Some(t) : None ;
      }
   }
}
