using Xunit;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using System.Threading.Tasks;

namespace LaYumba.Functional.Tests
{
   public class TaskValidationTest
   {
      [Fact]
      public void SingleSelectClauseCompiles()
      {
         Task<int> t1 = 
            from i in Async(1)
            select i;

         Validation<int> v1 =
            from i in Valid(1)
            select i;

         Task<Validation<int>> tv1 =
            from i in Async(Valid(1))
            select i;
      }

      [Fact]
      public void TwoSelectClauseCompiles()
      {
         Task<int> t1 = 
            from i in Async(1)
            from j in Async(2)
            select i + j;

         Validation<int> v1 =
            from i in Valid(1)
            from j in Valid(2)
            select i + j;

         Task<Validation<int>> tv1 =
            from i in Async(Valid(1))
            from j in Async(Valid(2))
            select i + j;
      }

      [Fact]
      public void ThreeSelectClauseCompiles()
      {
         Task<int> t1 = 
            from i in Async(1)
            from j in Async(2)
            from k in Async(3)
            select i + j + k;

         Validation<int> v1 =
            from i in Valid(1)
            from j in Valid(2)
            from k in Valid(3)
            select i + j + k;

         Task<Validation<int>> tv1 =
            from i in Async(Valid(1)).Stack()
            from j in Async(Valid(2))
            from k in Async(Valid(3))
            select i + j + k;
      }

      [Fact]
      public void FourSelectClauseCompiles()
      {
         Task<int> t1 = 
            from i in Async(1)
            from j in Async(2)
            from k in Async(3)
            from m in Async(4)
            select i + j + k + m;

         Validation<int> v1 =
            from i in Valid(1)
            from j in Valid(2)
            from k in Valid(3)
            from m in Valid(4)
            select i + j + k + m;

         Task<Validation<int>> tv1 =
            from i in Async(Valid(1)).Stack()
            from j in Async(Valid(2))
            from k in Async(Valid(3))
            from m in Async(Valid(4))
            select i + j + k + m;
      }
   }
}
