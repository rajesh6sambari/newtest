using System.Collections.Generic;
using TestingTutor.CSharpEngine.Engine.Analysis.Parser;
using System.IO;
using TestingTutor.CSharpEngine.Engine.Factory;
using System.Xml.Serialization;
using OpenCover.Framework.Model;

namespace TestingTutor.CSharpEngine.Engine.Analysis.Trace
{

    public class Tracer:ITracer
    {
        protected IEngineFactory Factory;

        public Tracer(IEngineFactory factory)
        {
            Factory = factory;
        }
        public TraceTests GetTestCoverage(string testsuit, string coverDir, 
            IList<IndividualTest> tests)
        {
            var traceTests = new TraceTests();
            var tracer = Factory.Trace();
            var outputFile = Path.Combine(coverDir, "OutputResults.xml");


            foreach (var currentFile in tests)
            {
                tracer.Run(currentFile.TestName, coverDir, outputFile, testsuit);
                traceTests.TestCoverages.Add(
                    new TestCoverage()
                    {
                        Test = currentFile,
                        Pass = GetTestPass(coverDir, currentFile.TestName),
                        CoverageSession = GetCoverageSession(currentFile, coverDir),
                    });
                Directory.Delete(coverDir, true);
                Directory.CreateDirectory(coverDir);
            }
            return traceTests;
        }

        public CoverageSession GetCoverageSession(IndividualTest test, string coverDir)
        {
            var testName = test.TestName.Split('.');

            XmlSerializer serializer = new XmlSerializer(typeof(CoverageSession));

            FileStream fs = new FileStream(coverDir + "\\" + testName[2] + ".xml", FileMode.Open);

            CoverageSession cs;

            cs = (CoverageSession)serializer.Deserialize(fs);

            fs.Close();

            return cs;
        }

        public bool GetTestPass(string workingDirectory, string testName)
        {
            var path = workingDirectory + "\\" + testName + ".txt";
            
            return (!System.IO.File.ReadAllText(path).Contains("Failed") ? true : false);
        }
    }
}

