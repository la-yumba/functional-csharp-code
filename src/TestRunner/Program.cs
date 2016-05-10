using System;
using System.Reflection;
using NUnit.Common;
using NUnitLite;

namespace TestRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new AutoRun(typeof (LaYumba.Functional.Tests.Unit).GetTypeInfo().Assembly)
                .Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);

            new AutoRun(typeof(Boc.Startup).GetTypeInfo().Assembly)
                .Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
        }
    }
}
