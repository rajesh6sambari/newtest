using System.Collections.Generic;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface IJavaReflector
    {
        void Reflect(string codeDirectory, string reflectionDirectory, ref List<JavaTestClass> testClasses);
    }
}
