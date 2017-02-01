using System;
using System.Threading;

namespace LaYumba.Functional
{
   public sealed class Atom<T>
      where T : class
   {
      private volatile T value;
      public T Value => value;

      public Atom(T value)
      {
         this.value = value;
      }

      public T Swap(Func<T, T> update)
      {
         T original, updated;
         original = value;
         updated = update(original);

         if (original != Interlocked.CompareExchange(ref value, updated, original)) // try once
         {
            var spinner = new SpinWait(); // if we fail, go into a spin wait, spin, and try again until succeed
            do
            {
               spinner.SpinOnce();
               original = value;
               updated = update(original);
            }
            while (original != 
               Interlocked.CompareExchange(ref value, updated, original));
         }
         return updated;
      }

      T Swap_SimplerButLessEfficient(Func<T, T> update)
      {
         T original, updated;
         do
         {
            original = value;
            updated = update(original);
         }
         while (original != 
            Interlocked.CompareExchange(ref value, updated, original));

         return updated;
      }
   }
}
