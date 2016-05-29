using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LaYumba.Functional
{
   public class Pattern<R> : IEnumerable
   {
      IList<Delegate> funcs = new List<Delegate>();

      IEnumerator IEnumerable.GetEnumerator() => funcs.GetEnumerator();

      public void Add<T>(Func<T, R> func) => funcs.Add(func);

      public R Match(object value)
         => (R)funcs.First(InputArgMatchesTypeOf(value))
            .DynamicInvoke(new[] { value });

      static Func<Delegate, bool> InputArgMatchesTypeOf(object value)
         => func => value.GetType().FullName
            == func.GetType().GetGenericArguments()[0].FullName;
   }
}
