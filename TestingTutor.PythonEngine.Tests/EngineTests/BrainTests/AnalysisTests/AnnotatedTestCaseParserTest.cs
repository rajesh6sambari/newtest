using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers;

namespace TestingTutor.PythonEngine.Tests.EngineTests.BrainTests.AnalysisTests
{
    [TestClass]
    public class AnnotatedTestCaseParserTest
    {
        protected AnnotatedTestParser Parser;
        protected string CurrentDirectory;


        [TestInitialize]
        public void Init()
        {
            Parser = new AnnotatedTestParser();
            CurrentDirectory = Directory.GetCurrentDirectory();
        }

        [TestMethod]
        public void FindsAnnotatedTest()
        {
            // Arrange
            var xml = Path.Combine(CurrentDirectory, "file.xml");
            File.WriteAllText(xml,
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><testsuite errors=\"0\" failures=\"0\" name=\"pytest\" skips=\"0\" tests=\"1\" time=\"0.226\"><testcase classname=\"test_file\" file=\"test_file.py\" line=\"0\" name=\"test_add\" time=\"0.003987789154052734\"><properties><property name=\"EquivalanceClass\" value=\"FUNCTION CALL\"/><property name=\"Concepts\" value=\"func,equals\"/></properties></testcase></testsuite>");

            // Act
            var annotated = Parser.GatherAnnotatedTest(xml);

            // Assert
            Assert.AreEqual("FUNCTION CALL", annotated.EquivalanceClass);
            Assert.IsTrue(annotated.Concepts.OrderBy(o => o).SequenceEqual(
                (new List<string>() {"func", "equals"}).OrderBy(o => o)));
        }

        [TestCleanup]
        public void Clean()
        {
            
        }
    }
}
