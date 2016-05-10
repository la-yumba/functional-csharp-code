namespace Boc.Services
{
   public interface IHandler<T>
   {
      void Handle(T command);
   }
}