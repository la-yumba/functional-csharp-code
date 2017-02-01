//#r "..\..\LaYumba.Functional\bin\Debug\netstandard1.6\LaYumba.Functional.dll"
#r "LaYumba.Functional\bin\Debug\netstandard1.6\LaYumba.Functional.dll"
#r "LaYumba.Functional.Data\bin\Debug\netstandard1.6\LaYumba.Functional.Data.dll"


using LaYumba.Functional;
using static LaYumba.Functional.F;
using static System.Console;

var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(d => d.ToString());

static Func<int, int, int> multiply = (i, j) => i * j;
static Func<int, int> @double = i => i * 2;
