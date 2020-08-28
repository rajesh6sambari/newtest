using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers
{
    public class TestCaseParser
    {
        public Regex Module = new Regex(@"[<]Module \'([\w\.]+(/[\w\.]+){0,})\'[>]");
        public Regex Class = new Regex(@"[<]Class \'([\w\.]+)\'[>]");
        public Regex Function = new Regex(@"[<]Function \'([\w\.]+)\'[>]");

        public IList<IndividualTest> GatherTests(IList<string> inputs)
        {
            IList<IndividualTest> tests = new List<IndividualTest>();

            var index = 0;

            while (index < inputs.Count)
            {
                if (Module.IsMatch(inputs[index]))
                {
                    var moduleName = Module.Matches(inputs[index])[0].Groups[1].Value;
                    index = CollectFunctionTest(moduleName, ++index, ref inputs, ref tests);
                }
                else
                {
                    index++;
                }
            }

            return tests;
        }

        public int CollectFunctionTest(string name, int index, ref IList<string> inputs, ref IList<IndividualTest> tests)
        {
            while (index < inputs.Count)
            {
                if (Module.IsMatch(inputs[index])) return index;

                if (Class.IsMatch(inputs[index]))
                {
                    var className = Class.Matches(inputs[index])[0].Groups[1].Value;
                    index = CollectClassTest($"{name}::{className}", ++index, ref inputs, ref tests);
                }
                else if (Function.IsMatch(inputs[index]))
                {
                    var functionName = Function.Matches(inputs[index])[0].Groups[1].Value;
                    tests.Add(
                        new IndividualTest($"{name}::{functionName}"));
                    index++;
                }
                else
                {
                    index++;
                }
            }
            return index;
        }

        public int CollectClassTest(string name, int index, ref IList<string> inputs, ref IList<IndividualTest> tests)
        {
            while (index < inputs.Count)
            {
                if (Module.IsMatch(inputs[index])) return index;

                if (Function.IsMatch(inputs[index]))
                {
                    if (Function.Match(inputs[index]).Index < 3) return index;

                    var functionName = Function.Matches(inputs[index])[0].Groups[1].Value;
                    tests.Add(
                        new IndividualTest($"{name}::{functionName}"));
                }
                index++;
            }

            return index;
        }

    }

}
