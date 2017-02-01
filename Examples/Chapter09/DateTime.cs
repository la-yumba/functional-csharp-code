using System;

namespace Examples.Chapter0.Introduction
{
   using static Console;

   class DateTime_Example
   {
      internal static void _main()
      {
         var momsBirthday = new DateTime(1966, 12, 13);
         momsBirthday.AddDays(-7);
         WriteLine(momsBirthday);

         var johnsBirthday = momsBirthday;
         johnsBirthday = johnsBirthday.AddDays(1);
         WriteLine("Johns: " + johnsBirthday.Date);
         WriteLine("moms: " + momsBirthday.Date);
      }
   }

}
