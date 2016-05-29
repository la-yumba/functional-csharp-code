//using System;
//using System.IO;
//using LaYumba.Functional;

//namespace Examples.Chapter3
//{
//   using static F;

//   public class Unit
//   {
//      static void _main()
//      {
//         var fileContents = Using(new StreamReader("file.txt")
//            , reader => reader.ReadToEnd());

//         Using(new StreamWriter("file.txt")
//            , writer => writer.WriteLine("some more content"));

//         Using(new StreamWriter("file.txt")
//            , writer =>
//            {
//               writer.WriteLine("some more content");
//               return Unit();
//            });
//      }

//      // The version withoug duplication is in F
//      static void Using_WithDuplication<TDisp>(TDisp disposable
//         , Action<TDisp> act) where TDisp : IDisposable
//      {
//         using (disposable) act(disposable);
//      }
//   }
//}
