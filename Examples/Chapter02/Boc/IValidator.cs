namespace Boc.Services
{
   public interface IValidator<T>
   {
      bool IsValid(T t);
   }
}