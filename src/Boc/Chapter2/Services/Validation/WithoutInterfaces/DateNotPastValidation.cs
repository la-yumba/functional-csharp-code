using System;
using Boc.Commands;

namespace Boc.Services.Validation.WithoutInterfaces
{
   public delegate DateTime Clock();

   public static class Default
   {
      public static Clock Clock = () => DateTime.UtcNow;
   }

   public class DateNotPastValidator : IValidator<Transfer>
   {
      public bool IsValid(Transfer request)
         => IsValid(request, Default.Clock);

      internal static bool IsValid(Transfer request, Clock clock)
         => clock().Date <= request.Date.Date;
   }
}