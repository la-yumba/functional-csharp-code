using Boc.Commands;
using LaYumba.Functional;
using System.Text.RegularExpressions;
using Boc.Domain;

namespace Boc.Chapter7.OOP
{
   public class BicCodeValidator : IValidator<MakeTransfer>
   {
      static readonly Regex regex = new Regex("^[A-Z]{6}[A-Z1-9]{5}$");

      public Validation<MakeTransfer> Validate(MakeTransfer request)
      {
         if (!regex.IsMatch(request.Bic.ToUpper()))
            return Errors.InvalidBic;
         return request;
      }
   }
}