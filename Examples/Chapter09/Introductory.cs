using static System.Linq.Enumerable;
using static System.Console;
using System.Threading.Tasks;
using System;

namespace Examples.Chapter10.Data
{
   class Product
   {
      public int Inventory { get; private set; }

      public void ReplenishInventory(int units)
         => Inventory += units;

      public void ProcessSale(int units)
         => Inventory -= units;
   }

   class Product_
   {
      int inventory;

      public bool IsLowOnInventory { get; private set; }
      public int Inventory
      {
         get { return inventory; }
         private set
         {
            inventory = value;

            IsLowOnInventory = inventory <= 5;
         }
      }
   }

   public class LocalMutationIsOk
   {
      int Sum(int[] ints)
      {
         var result = 0;
         foreach (int i in ints) result += i;
         return result;
      }

      public static void _main()
      {
         var nums = Range(-10000, 20001).Reverse().ToList();
         Parallel.Invoke(
            () => WriteLine(nums.Sum()),
            () => { nums.Sort(); WriteLine(nums.Sum()); });
      }

      public static void __main()
      {
         var nums = Range(-10000, 20001).Reverse().ToList();

         Action task1 = () => WriteLine(nums.Sum());
         Action task2 = () => { nums.Sort(); WriteLine(nums.Sum()); };

         Parallel.Invoke(task1, task2);
      }

      public static void WithIEnumerable()
      {
         var nums = Range(-10000, 20001).Reverse();

         Action task1 = () => WriteLine(nums.Sum());
         Action task2 = () => { nums.OrderBy(x => x); WriteLine(nums.Sum()); };

         Parallel.Invoke(task1, task2);
      }
   }
}
