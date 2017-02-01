namespace Examples.Purity
{
   public static class StringExt
   {
      public static string ToSentenceCase(this string s)
         => s.ToUpper()[0] + s.ToLower().Substring(1);
   }
}