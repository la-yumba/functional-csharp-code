namespace Boc.Domain.Validator
{
   static class AccountExt
   {
      public static bool CanCover(this Account account, Money amount)
         => amount <= account.Balance + account.Overdraft;
   }
}