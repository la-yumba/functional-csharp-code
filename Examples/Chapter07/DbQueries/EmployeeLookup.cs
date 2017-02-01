using LaYumba.Functional;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples.Chapter7
{
   public static class EmployeeLookup
   {
      public static void Run()
      {
         ConnectionString conn = "my-database";

         SqlTemplate select = "SELECT * FROM EMPLOYEES"
            , sqlById = $"{select} WHERE ID = @Id"
            , sqlByName = $"{select} WHERE LASTNAME = @LastName";

         // queryEmployees : (SqlTemplate, object) → IEnumerable<Employee>
         var queryEmployees = conn.Query<Employee>();

         // queryById : object → IEnumerable<Employee>
         var queryById = queryEmployees.Apply(sqlById);

         // queryByLastName : object → IEnumerable<Employee>
         var queryByLastName = queryEmployees.Apply(sqlByName);

         // LookupEmployee : Guid → Option<Employee>
         Option<Employee> LookupEmployee(Guid id)
            => queryById(new { Id = id }).FirstOrDefault();

         // FindEmployeesByLastName : string → IEnumerable<Employee>
         IEnumerable<Employee> FindEmployeesByLastName(string lastName)
            => queryByLastName(new { LastName = lastName });
      }
   }
}
