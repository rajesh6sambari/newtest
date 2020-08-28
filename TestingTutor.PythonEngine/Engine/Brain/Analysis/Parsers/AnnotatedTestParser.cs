using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers
{
    public class AnnotatedTestParser
    {
        public AnnotatedTest GatherAnnotatedTest(string file)
        {
            var doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(file));
            
            var node = doc["testsuite"];

            var testcase = node?["testcase"];

            var properties = testcase?["properties"];

            var prop = properties?["property"];

            var annotatedTest = new AnnotatedTest();

            var first = prop?.Attributes["name"];
            var firstValue = prop?.Attributes["value"];

            if (first != null && first.Value.Equals("EquivalanceClass"))
            {
                annotatedTest.EquivalanceClass = firstValue.Value;
            }
            else if (first != null && first.Value.Equals("Concepts"))
            {
                annotatedTest.Concepts = firstValue.Value.Split(',').ToList();
            }

            var second = prop?.NextSibling?.Attributes["name"];
            var secondValue = prop?.NextSibling?.Attributes["value"];

            if (second != null && second.Value.Equals("EquivalanceClass"))
            {
                annotatedTest.EquivalanceClass = secondValue.Value;
            }
            else if (second != null && second.Value.Equals("Concepts"))
            {
                annotatedTest.Concepts = secondValue.Value.Split(',').ToList();
            }

            return annotatedTest;
        }
    }
}
