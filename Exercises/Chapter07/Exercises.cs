using LaYumba.Functional;
using NUnit.Framework;
using System;

namespace Exercises.Chapter7
{
   static class Exercises
   {
      // 1. Partial application with a binary arithmethic function:
      // Write a function `Remainder`, that calculates the remainder of 
      // integer division(and works for negative input values!). 

      // Notice how the expected order of parameters is not the
      // one that is most likely to be required by partial application
      // (you are more likely to partially apply the divisor).

      // Write an `ApplyR` function, that gives the rightmost parameter to
      // a given binary function (try to write it without looking at the implementation for `Apply`).
      // Write the signature of `ApplyR` in arrow notation, both in curried and non-curried form

      // Use `ApplyR` to create a function that returns the
      // remainder of dividing any number by 5. 

      // Write an overload of `ApplyR` that gives the rightmost argument to a ternary function


      // 2. Let's move on to ternary functions. Define a class `PhoneNumber` with 3
      // fields: number type(home, mobile, ...), country code('it', 'uk', ...), and number.
      // `CountryCode` should be a custom type with implicit conversion to and from string.

      // Now define a ternary function that creates a new number, given values for these fields.
      // What's the signature of your factory function? 

      // Use partial application to create a binary function that creates a UK number, 
      // and then again to create a unary function that creates a UK mobile


      // 3. Functions everywhere. You may still have a feeling that objects are ultimately 
      // more powerful than functions. Surely, a logger object should expose methods 
      // for related operations such as Debug, Info, Error? 
      // To see that this is not necessarily so, challenge yourself to write 
      // a very simple logging mechanism without defining any classes or structs. 
      // You should still be able to inject a Log value into a consumer class/function, 
      // exposing operations like Debug, Info, and Error, like so:

      //static void ConsumeLog(Log log) 
      //   => log.Info("look! no objects!");

      enum Level { Debug, Info, Error }
   }
}
