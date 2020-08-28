using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.ModuleHandlers;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers;
using TestingTutor.PythonEngine.Engine.Factory;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.Trace
{
    public class Tracer
    {
        protected IEngineFactory Factory;

        public Tracer(IEngineFactory factory)
        {
            Factory = factory;
        }

        public TraceTests GetTestCoverage(string testsuit,
            string coverDir,
            IList<IndividualTest> tests,
            IList<ModuleName> module)
        {
            var traceTests = new TraceTests();
            var tracer = Factory.Trace();
            var outputFile = Path.Combine(coverDir, "OutputResults.txt");

            foreach (var test in tests)
            {
                tracer.Run(test.TestName, coverDir, outputFile, testsuit);
                traceTests.TestCoverages.Add(
                    new TestCoverage()
                    {
                        Test = test,
                        Pass = Passed(outputFile),
                        ModuleCoverages = GetModuleCoverages(coverDir, module)
                    });

                Directory.Delete(coverDir, true);
                Directory.CreateDirectory(coverDir);
            }
            return traceTests;
        }

        public bool Passed(string outputfile)
        {
            var text = File.ReadAllText(outputfile);
            return !Regex.IsMatch(text, $"= FAILURES =");
        }

        public IList<ModuleCoverage> GetModuleCoverages(string directory, IList<ModuleName> modules)
        {
            var coverages = new List<ModuleCoverage>();

            var files = Directory.GetFiles(directory, "*.cover", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                foreach (var module in modules)
                {
                    if (Regex.IsMatch(Path.GetFileName(file), module.CoverFileName))
                    {
                        coverages.Add(new ModuleCoverage()
                        {
                            ModuleName = module,
                            Contents = File.ReadAllLines(file)
                        });
                    }
                }
            }
            return coverages;
        }
        
    }
}
