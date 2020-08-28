using System.Collections.Generic;

namespace TestingTutor.JavaEngine.Models
{
    public class CompilationUnit
    {
        public void Add(JavaTestClass javaTestClass) => Classes.Add(javaTestClass);
        public IList<JavaTestClass> Classes { get; } = new List<JavaTestClass>();
        public IList<string> SourceFiles { get; } = new List<string>();
    }
}
