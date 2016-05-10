using LaYumba.Functional;
using System;

using static System.Console;
using static LaYumba.Functional.F;

namespace Examples.Chapter0.Introduction
{
   class Unit_Example
   {
      public Unit DoStuff() => new Action(InternalDoStuff).ToFunc()();

      private void InternalDoStuff() { /* side effects here */ }

      Action sayHelloAction = () => WriteLine("hello");

      Func<Unit> sayHelloFunction = () =>
      {
         WriteLine("hello");
         return Unit();
      };
   }
}
