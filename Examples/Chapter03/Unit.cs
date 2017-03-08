using System.IO;
using System.Text;
using LaYumba.Functional;

namespace Examples.Chapter3
{
   using static F;

   public class UnitExample
   {
      static void _main()
      {
         // call the overload that takes a Func<T>
         var contents = Instrumentation.Time("reading from file.txt"
            , () => File.ReadAllText("file.txt"));

         // explicitly call with Func<Unit>
         Instrumentation.Time("reading from file.txt", () =>
         {
            File.AppendAllText("file.txt", "New content!", Encoding.UTF8);
            return Unit();
         });

         // call the overload that takes an Action
         Instrumentation.Time("reading from file.txt"
            , () => File.AppendAllText("file.txt", "New content!", Encoding.UTF8));
      }
   }
}
