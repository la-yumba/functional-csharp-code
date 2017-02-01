using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;

class Interview_Example_Option
{
   Func<Candidate, bool> IsEligible;
   Func<Candidate, Option<Candidate>> Interview;

   Option<Candidate> FirstRound(Candidate c)
      => Some(c)
         .Where(IsEligible)
         .Bind(Interview);
}

class Interview_Example_Either
{
   Func<Candidate, bool> IsEligible;
   Func<Candidate, Either<Rejection, Candidate>> Interview;

   Either<Rejection, Candidate> CheckEligibility(Candidate c)
   {
      if (IsEligible(c)) return c;
      return new Rejection("Not eligible");
   }

   Either<Rejection, Candidate> FirstRound(Candidate c)
      => Right(c)
         .Bind(CheckEligibility)
         .Bind(Interview);
}

class Candidate { }
class Rejection
{
   private string reason;

   public Rejection(string reason)
   {
      this.reason = reason;
   }
}
