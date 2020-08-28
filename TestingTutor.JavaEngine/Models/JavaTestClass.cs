using System.Collections.Generic;

namespace TestingTutor.JavaEngine.Models
{
    public class JavaTestClass
    {
        public string Name { get; set; }
        public string Package { get; set; }
        public string PackageDirectory { get; set; }
        public string FileUri { get; set; }
        public string ClassPath { get; set; }
        public IList<JavaTestMethod> Methods { get; } = new List<JavaTestMethod>();
        public void AddMethod(JavaTestMethod javaTestMethod) => Methods.Add(javaTestMethod);
    }
}
