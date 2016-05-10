using System;
using System.Collections.Generic;
using System.Linq;
using Examples.Domain;

namespace Examples.Chapter0.Introduction
{
   class Names
   {
      Comparison<string> caseInsensitive = (x, y) => x.ToUpper().CompareTo(y.ToUpper());

      public void Sort(List<string> names) => names.Sort(caseInsensitive);
   }

   class Names_Lambda
   {
      public void Sort(List<string> names)
         => names.Sort((x, y) => x.ToUpper().CompareTo(y.ToUpper()));
   }

   public class Lambda_Closure
   {
      private List<Employee> employees;

      public IEnumerable<Employee> FindByName(string name)
         => employees.Where(e => e.LastName.StartsWith(name));
   }

   class Cache<T> where T : class
   {
      public T Get(Guid id, Func<T> onMiss) 
         => Get(id) ?? onMiss();

      public T Get_MoreReailstically(Guid id, Func<T> onMiss)
      {
         T result = Get(id);
         if (result == null)
         {
            result = onMiss();
            Add(id, result);
         }
         return result;
      }

      private void Add(Guid id, T result) 
      {
         throw new NotImplementedException();
      }

      T Get(Guid id)
      {
         throw new NotImplementedException();
      }
   }
}
