using LaYumba.Functional;

namespace Exercises.Chapter4
{
   public class Person
   {
      public string FirstName { get; }
      public string LastName { get; }

      public decimal Earnings { get; set; }
      public Option<int> Age { get; set; }

      public Person() { }

      public Person(string firstName, string lastName)
      {
         FirstName = firstName;
         LastName = lastName;
      }
   }
}
