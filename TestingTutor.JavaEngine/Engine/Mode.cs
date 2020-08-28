using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;
using TestingTutor.JavaEngine.Utilities;

namespace TestingTutor.JavaEngine.Engine
{
    public abstract class Mode
    {
        protected readonly Submission Submission;
        protected readonly WorkingDirectories WorkingDirectories;
        protected CompilationUnit ReferenceCompilationUnit;
        protected CompilationUnit StudentCompilationUnit;
        protected List<JavaTestClass> ReferenceJavaTestClasses;
        protected List<JavaTestClass> StudentJavaTestClasses;

        protected Mode(Submission submission)
        {
            Submission = submission;
            WorkingDirectories = new WorkingDirectories(submissionId: submission.SubmissionId);
        }

        protected IList<string> GetFileUris(CompilationUnit compilationUnit)
        {
            var files = new List<string>();
            compilationUnit.Classes.ToList().ForEach(file => files.Add(file.FileUri));
            return files;
        }

        protected void Compile(string workingDirectory, IList<string> files) 
            => new JavaCompiler2().Compile(workingDirectory, files);

        protected void Reflect(string codeDirectory, string reflectionDirectory, ref List<JavaTestClass> classes) 
            => new JavaReflector().Reflect(codeDirectory, reflectionDirectory, ref classes);

        protected void RunJunitTests(List<JavaTestClass> testClasses, string instrumentedCodeDirectory, string traceDirectory, string referenceCode = null)
            => new JunitTestRunner().RunTests(testClasses, instrumentedCodeDirectory, traceDirectory, referenceCode);

        protected void RunTrace(string workingDirectory, ref List<JavaTestClass> testClasses) 
            => new TraceProducer().Trace(workingDirectory, ref testClasses);

        protected void Unpack(Dictionary<string, MemoryStream> unit1, Dictionary<string, MemoryStream> unit2, out CompilationUnit compilationUnit, string directory)
            => JavaUtilities.Unpack(unit1.Concat(unit2).ToDictionary(x => x.Key, y => y.Value), directory, out compilationUnit);

        protected void Extract(string traceDirectory, ref List<JavaTestClass> javaClasses)
            => new TraceExtractor().Extract(traceDirectory, ref javaClasses);

        protected void Analyze(List<JavaTestClass> referenceTests, List<JavaTestClass> studentTests, out FeedbackDto feedbackDto)
            => new TraceAnalysis().Analyze(referenceTests, studentTests, out feedbackDto);

        protected void RawCoverage(string studentTraceDirectory, List<JavaTestClass> studentClasses, out IList<ClassCoverageDto> classCoverageDtos)
            => new RawCoverager().RawCoverage(studentTraceDirectory, studentClasses, out classCoverageDtos);

    }
}
