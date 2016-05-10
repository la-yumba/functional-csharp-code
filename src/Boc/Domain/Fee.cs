namespace Boc.Domain
{
   public class Fee
   {
      public decimal Amount { get; }
      public string Description { get; }

      public static Fee ForOverdraft(decimal amount) 
         => new Fee(amount, "Overdraft fee");

      public Fee(decimal amount, string description)
      {
         Amount = amount;
         Description = description;
      }

      public static Fee Create(decimal amount) => new Fee(amount, null);
   }
}