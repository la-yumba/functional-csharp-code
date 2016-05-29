using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LaYumba.Functional;

namespace Examples.Parallel
{
    using static Console;

    public class SearchPatternNaive
    {
        internal static void _main()
        {

            do
            {
                var files = new[]
                {
                    @"C:\functional-cs\manuscript\0 Preface.md",
                    @"C:\functional-cs\manuscript\1 Introduction.md",
                    @"C:\functional-cs\manuscript\2 Option.md",
                    @"C:\functional-cs\manuscript\3 Workflows.md",
                    @"C:\functional-cs\manuscript\4 Bind.md",
                    @"C:\functional-cs\manuscript\5 Either.md",
                    @"C:\functional-cs\manuscript\6 Partial.md",
                    @"C:\functional-cs\manuscript\7 Apply.md",
                };
                Stopwatch watch;
                int matches;

                watch = Stopwatch.StartNew();
                matches = CountMatches_Sequential("map", files);
                watch.Stop();
                WriteLine($"sequential: {matches}, {watch.ElapsedTicks}");

                watch = Stopwatch.StartNew();
                matches = CountMatches_Naive("map", files);
                watch.Stop();
                WriteLine($"parallel: {matches}, {watch.ElapsedTicks}");

                watch = Stopwatch.StartNew();
                matches = CountMatches_Better("map", files);
                watch.Stop();
                WriteLine($"lock-free: {matches}, {watch.ElapsedTicks}");

                watch = Stopwatch.StartNew();
                matches = CountMatches_Better("map", files);
                watch.Stop();
                WriteLine($"parallel-ext: {matches}, {watch.ElapsedTicks}");
            }
            while (ReadKey().KeyChar == ' ');
        }

        static int CountMatches_Sequential(string pattern, string[] files)
        {
            int hits = 0;
            foreach (var file in files)
            {
                var body = File.ReadAllText(file);
                Match m = new Regex(pattern).Match(body);
                while (m.Success)
                {
                    hits++;
                    m = m.NextMatch();
                }
            }
            return hits;
        }

        static int CountMatches_Naive(string pattern, string[] files)
        {
            int hits = 0;
            var tasks = new List<Task>();
            var lockObj = new object();

            foreach (var file in files)
            {
                Task task = Task.Factory.StartNew(fileObj =>
                {
                    string __file = (string)fileObj;
                    var body = File.ReadAllText(__file);
                    Match m = new Regex(pattern).Match(body);
                    while (m.Success)
                    {
                        lock (lockObj) { hits++; }
                        m = m.NextMatch();
                    }
                }
                , file);
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            return hits;
        }

        static int CountMatches_Better(string pattern, string[] files)
        {
            var tasks = files.Map(async f => FindOccurrences(pattern)(f)).ToArray();
            Task.WaitAll(tasks);
            return tasks.Sum(t => t.Result);
        }

        static int CountMatches_ParallelExt(string pattern, string[] files)
        {
            return files
                .AsParallel()
                .Map(FindOccurrences(pattern))
                .Sum();
        }

        static int CountMatches_Plinq(string pattern, string[] files) 
            => (from file in files.AsParallel()
                select FindOccurrences(pattern)(file)).Sum();

        private static Func<string, int> FindOccurrences(string pattern) => file =>
        {
            int hits = 0;
            var body = File.ReadAllText((string)file);
            Match m = new Regex(pattern).Match(body);
            while (m.Success)
            {
                hits++;
                m = m.NextMatch();
            }
            return hits;
        };
    }
}
