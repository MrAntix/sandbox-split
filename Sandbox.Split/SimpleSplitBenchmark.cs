using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Antix.Testing;
using Xunit;

namespace Sandbox.Split
{
    public class SimpleSplitBenchmark
    {
        // ReSharper disable ImplicitlyCapturedClosure
        // ReSharper disable UnusedVariable
        const string Separator = ".";

        static string GetTestString()
        {
            return string.Join(Separator,
                               TestData.Text
                                       .WithLetters()
                                       .WithRange(10, 10).Build(1000));
        }

        [Fact]
        public void Go()
        {
            var testString = GetTestString();

            var regex = new Regex("\\.");
            var regexCompiled = new Regex("\\.", RegexOptions.Compiled);

            var results = new Collection<BenchmarkResult>();

            Run(() => testString.Split('.'), "string.split", results);
            Run(() => regex.Split(testString), "regex.split", results);
            Run(() => regexCompiled.Split(testString), "regex.split (compiled)", results);
            Run(() => SplitLoop(testString, '.'), "loop", results);
            Run(() => SplitLoopYield(testString, '.'), "loop (yield)", results);
            Run(() => SplitIndexOf(testString, '.'), "indexOf", results);
            Run(() => SplitIndexOfYield(testString, '.'), "indexOf (yield)", results);

            foreach (var result in results.OrderBy(r => r.Average))
            {
                Console.WriteLine(result);
            }
        }

        static IEnumerable<string> SplitLoop(string text, char separator)
        {
            var list = new List<string>();

            var index = 0;
            var lastIndex = 0;
            foreach (var c in text)
            {
                if (c == separator)
                {
                    list.Add(text.Substring(lastIndex, index - lastIndex));
                    lastIndex = index + 1;
                }

                index++;
            }
            list.Add(text.Substring(lastIndex));

            return list;
        }

        static IEnumerable<string> SplitLoopYield(string text, char separator)
        {
            var index = 0;
            var lastIndex = 0;
            foreach (var c in text)
            {
                if (c == separator)
                {
                    yield return text.Substring(lastIndex, index - lastIndex);
                    lastIndex = index + 1;
                }

                index++;
            }

            yield return text.Substring(lastIndex);
        }

        static IEnumerable<string> SplitIndexOf(string text, char separator)
        {
            var list = new List<string>();

            int index;
            var lastIndex = 0;
            while ((index = text.IndexOf(separator, lastIndex)) > -1)
            {
                list.Add(text.Substring(lastIndex, index - lastIndex));
                lastIndex = index + 1;
            }
            list.Add(text.Substring(lastIndex));

            return list;
        }

        static IEnumerable<string> SplitIndexOfYield(string text, char separator)
        {
            int index;
            var lastIndex = 0;
            while ((index = text.IndexOf(separator, lastIndex)) > -1)
            {
                yield return text.Substring(lastIndex, index - lastIndex);
                lastIndex = index + 1;
            }

            yield return text.Substring(lastIndex);
        }

        static void Run(Func<IEnumerable<string>> action, string name, ICollection<BenchmarkResult> results)
        {
            var result = Benchmark
                .Run(() =>
                    {
                        foreach (var _ in action())
                        {
                        }
                    }, name, 10000);

            results.Add(result);
        }
    }
}