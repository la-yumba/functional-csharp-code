using LaYumba.Functional;

namespace Boc.Api
{
   public class ResultDto<T>
   {
      public bool Succeeded { get; }
      public bool Failed => !Succeeded;

      public T Data { get; }
      public Error Error { get; }

      internal ResultDto(T data) { Succeeded = true; Data = data; }
      internal ResultDto(Error error) { Error = error; }
   }

   public static class EitherExt
   {
      public static ResultDto<T> ToResult<T>(this Either<Error, T> @this)
         => @this.Match(
            Right: data => new ResultDto<T>(data),
            Left: error => new ResultDto<T>(error));
   }
}