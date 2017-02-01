using System;

namespace Examples
{
   public static class TimeSpanExt
   {
      public static TimeSpan Seconds(this int @this)
         => TimeSpan.FromSeconds(@this);

      public static TimeSpan Minutes(this int @this)
         => TimeSpan.FromMinutes(@this);

      public static TimeSpan Days(this int @this)
         => TimeSpan.FromDays(@this);

      public static DateTime Ago(this TimeSpan @this)
         => DateTime.UtcNow - @this;
   }
}