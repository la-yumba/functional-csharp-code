using LaYumba.Functional;
using System;

namespace Boc
{
   public static class ErrorExt
   {
      public static Either<Error, R> ToEither<R>(this R value) => value;
      public static Either<Error, R> ToEither<R>(this Error err) => err;
   }

   public abstract class Error
   {
      public virtual string Message { get; }

      // specific error types

      public sealed class InvalidBicError : Error
      {
         public override string Message { get; } 
            = "The beneficiary's BIC/SWIFT code is invalid";
      }

      public static InvalidBicError InvalidBic() => new InvalidBicError();

      public sealed class TransferDateIsPastError : Error
      {
         public override string Message { get; }
            = "Transfer date cannot be in the past";
      }

      public static TransferDateIsPastError TransferDateIsPast()
         => new TransferDateIsPastError();
   }
}
