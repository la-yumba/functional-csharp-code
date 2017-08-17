namespace LaYumba.Functional
{
   using static F;
   
   public static class Enum
   { 
      public static Option<T> Parse<T>(this string s) where T : struct
         => System.Enum.TryParse(s, out T t) ? Some(t) : None ;
   }
}
