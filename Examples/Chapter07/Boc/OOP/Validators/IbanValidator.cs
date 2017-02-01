using Boc.Commands;
using LaYumba.Functional;
using System.Text.RegularExpressions;
using Boc.Domain;

namespace Boc.Chapter7.OOP
{
   public class IbanValidator : IValidator<MakeTransfer>
   {
      private const string ERROR_MESSAGE = "The beneficiary's IBAN code is invalid";
      private readonly Regex regex = new Regex(
          "[A-Z]{2}[0-9]{2}[A-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}");

      public Validation<MakeTransfer> Validate(MakeTransfer request)
      {
         if (regex.IsMatch(request.Iban))
            return Errors.InvalidBic;
         return request;
      }
   }
}