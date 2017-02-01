using Microsoft.AspNetCore.Mvc;
using System;
using Boc.Commands;
using Boc.Services;

namespace Boc.Chapter5.Imperative
{
   public class Chapter5_TransfersController : Controller
   {
      IValidator<MakeTransfer> validator;

      public void MakeTransfer([FromBody] MakeTransfer transfer)
      {
         if (validator.IsValid(transfer))
            Book(transfer);
      }

      void Book(MakeTransfer transfer) { throw new NotImplementedException(); }
   }
   
   public class Account
   {
      public decimal Balance { get; private set; }

      public Account(decimal balance) { Balance = balance; }

      public void Debit(decimal amount)
      {
         if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds");

         Balance -= amount;
      }
   }
}