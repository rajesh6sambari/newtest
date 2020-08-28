using System.Collections.Generic;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface ITraceExtractor
    {
        void Extract(string traceDirectory, ref List<JavaTestClass> javaClasses);
    }
}
