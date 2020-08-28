using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TestingTutor.JavaEngine.Engine.CoverageXml;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine
{
    public class TraceExtractor : ITraceExtractor
    {
        public void Extract(string traceDirectory, ref List<JavaTestClass> javaClasses)
        {
            foreach (var javaClass in javaClasses)
            {
                ExtractJavaTestClass(traceDirectory, javaClass);
            }
        }

        public void ExtractJavaTestClass(string traceDirectory, JavaTestClass javaTestClass)
        {
            foreach (var method in javaTestClass.Methods)
            {
                var report = GetReport(traceDirectory, javaTestClass, method);
                AddLineCoveragesFromReport(report, method);
            }
        }

        public void AddLineCoveragesFromReport(Report report, JavaTestMethod testMethod)
        {
            foreach (var sourceFile in GetTestFiles(report))
            {
                foreach (var line in sourceFile.Lines)
                {
                    testMethod.AddLineCoverage(new LineCoverage
                    {
                        LineNumber = line.LineNumber,
                        CoveredBranches = line.CoveredBranches,
                        MissedBranches = line.MissedBranches,
                        CoveredInstructions = line.CoveredInstructions,
                        MissedInstructions = line.MissingInstructions
                    });
                }
            }
        }

        public IEnumerable<ReportPackageSourceFile> GetTestFiles(Report report)
        {
            return report.Package.SourceFiles.Where(file => !file.Name.ToLower().Contains("test"));
        }

        public Report GetReport(string traceDirectory, JavaTestClass javaTestClass, JavaTestMethod testMethod)
        {
            var fileUri = Path.Combine(traceDirectory, $"{javaTestClass.Name}-{testMethod.Name}.xml");
            return GetReport(fileUri);
        }


        private static Report GetReport(string fileUri)
        {
            var text = File.ReadAllText(fileUri);

            var xRoot = new XmlRootAttribute()
            {
                ElementName = "report",
                IsNullable = true
            };

            var xmlSerializer = new XmlSerializer(typeof(Report), xRoot);
            var buffer = Encoding.UTF8.GetBytes(text);
            using (var stream = new MemoryStream(buffer))
            {
                return (Report) xmlSerializer.Deserialize(stream);
            }
        }
    }
}