using System;

namespace Boc.Services
{
   public interface IDateTimeService
   {
      DateTime Now { get; }
      DateTime UtcNow { get; }
      DateTime Today { get; }
      bool IsToday(DateTime date);
   }

   public class DefaultDateTimeService : IDateTimeService
   {
      public DateTime Now => DateTime.Now;
      public DateTime UtcNow => DateTime.UtcNow;
      public DateTime Today => DateTime.Today;

      public bool IsToday(DateTime date)
      {
         return date.Date == Today;
      }
   }
}
