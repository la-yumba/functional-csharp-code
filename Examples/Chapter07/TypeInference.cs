using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;

using Name = System.String;
using Greeting = System.String;
using PersonalizedGreeting = System.String;

namespace Examples.Chapter7
{
   public class TypeInference_Method
   {
      // 1. method
      PersonalizedGreeting GreeterMethod(Greeting gr, Name name)
         => $"{gr}, {name}";

      // the below does NOT compile!
      //Func<Name, Greeting> __GreetWith(Greeting greeting)
      //   => GreeterMethod.Apply(greeting);

      // the lines below compiles, but oh my!
      Func<Name, PersonalizedGreeting> GreetWith_1(Greeting greeting)
         => FuncExt.Apply<Greeting, Name, PersonalizedGreeting>(GreeterMethod, greeting);

      Func<Name, PersonalizedGreeting> _GreetWith_2(Greeting greeting)
         => new Func<Greeting, Name, PersonalizedGreeting>(GreeterMethod)
            .Apply(greeting);
   }
   
   public class TypeInference_Delegate
   {
      string separator = "! ";

      // 1. field
      Func<Greeting, Name, PersonalizedGreeting> GreeterField 
         = (gr, name) => $"{gr}, {name}";

      // 2. property
      Func<Greeting, Name, PersonalizedGreeting> GreeterProperty 
         => (gr, name) => $"{gr}{separator}{name}";

      // 3. factory
      Func<Greeting, T, PersonalizedGreeting> GreeterFactory<T>()
         => (gr, t) => $"{gr}, {t}";

      Func<Name, PersonalizedGreeting> CreateGreetingWith(Greeting greeting)
      {
         // 1. field
         return GreeterField.Apply(greeting);

         // 2. property
         return GreeterProperty.Apply(greeting);

         // 3. factory
         return GreeterFactory<Name>().Apply(greeting);
      }
      
   }
}

