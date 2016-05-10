using Boc.Commands;

namespace Boc.Services
{
   public interface IPublisher
   {
      void Publish(Command command);
   }
}