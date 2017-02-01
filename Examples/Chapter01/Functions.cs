using System;
using System.Collections.Generic;
using System.Linq;

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
      
      T Get(Guid id)
      {
         throw new NotImplementedException();
      }
   }
}
