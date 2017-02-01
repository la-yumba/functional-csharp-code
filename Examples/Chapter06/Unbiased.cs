using LaYumba.Functional;
using System;

namespace Examples.Chapter5.Either.Unbiased
{
   interface IResearch
   {
      Either<Mooc, Book> GetStudyMaterial(string topic);
   }

   class Unbiased_Example
   {
      IResearch research;
      Func<Mooc, Knowledge> Watch;
      Func<Book, Knowledge> Read;

      void Start()
         => research
            .GetStudyMaterial("Functional programming")
            .Match(
               Left: Watch,
               Right: Read)
            .PutInPractice();
   }

   class Knowledge
   {
      internal void PutInPractice()
      {
         throw new NotImplementedException();
      }
   }
   class Book
   {
      private string v;

      public Book(string v)
      {
         this.v = v;
      }
   }
   class Mooc
   {
      private string v;

      public Mooc(string v)
      {
         this.v = v;
      }
   }
}
