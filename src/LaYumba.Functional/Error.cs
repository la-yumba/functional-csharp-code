namespace LaYumba.Functional
{
   public static class ErrorExt
   {
      public static Either<Error, R> ToEither<R>(this R value) => value;
      public static Either<Error, R> ToEither<R>(this Error err) => err;
   }

   public abstract class Error
   {
      public virtual string Message { get; }
   }
}
