using System.Collections.Generic;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface ITracerProducer
    {
        void Trace(string workingDirectory, ref List<JavaTestClass> javaClasses);
    }
}
