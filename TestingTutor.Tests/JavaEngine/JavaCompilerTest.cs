using System.Linq;
using Xunit;
using Submission = TestingTutor.JavaEngine.Models.Submission;

namespace TestingTutor.Tests.JavaEngine
{
    public class JavaCompilerTest : TestBase
    {


        //[Fact]
        //public void Compile_ShouldCompileSingleCompilationUnit()
        //{
        //    var compiler = new JavaCompiler();
        //    var workingDirectories = new WorkingDirectories();
        //    var compilationUnit = Submissions.MapFrom(LearningModeSubmissionDto).ReferenceSolution;
        //    var result = compiler.Compile(workingDirectories.ReferenceCodeDirectory, compilationUnit);

        //    Assert.True(result);
        //}

        //[Fact]
        //public void Compile_ShouldCompileInstructorArtifacts()
        //{
        //    var compiler = new JavaCompiler();
        //    var submission = Submissions.MapFrom(LearningModeSubmissionDto);
        //    var workingDirectories = new WorkingDirectories();
        //    var compilationUnit = submission.ReferenceSolution;
        //    compilationUnit = compilationUnit.Concat(submission.ReferenceTestSolution).ToDictionary(x => x.Key, x => x.Value);
        //    var result = compiler.Compile(workingDirectories.ReferenceCodeDirectory, compilationUnit);

        //    Assert.True(result);
        //}

        //[Fact]
        //public void Compile_ShouldCompileInstructorAndStudentArtifacts_ForLearningMode()
        //{
        //    var compiler = new JavaCompiler();
        //    var submission = Submissions.MapFrom(LearningModeSubmissionDto);
        //    var workingDirectories = new WorkingDirectories();
        //    var compilationUnit = submission.ReferenceSolution;
        //    compilationUnit = compilationUnit.Concat(submission.ReferenceTestSolution).ToDictionary(x => x.Key, x => x.Value);
        //    var result = compiler.Compile(workingDirectories.ReferenceCodeDirectory, compilationUnit);
        //    Assert.True(result);

        //    compilationUnit = submission.ReferenceSolution;
        //    compilationUnit = compilationUnit.Concat(submission.TestCaseSolution).ToDictionary(x => x.Key, x => x.Value);
        //    result = compiler.Compile(workingDirectories.TestingCodeDirectory, compilationUnit);

        //    Assert.True(result);
        //}
    }
}
