using System.Collections.Generic;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface ITraceAnalysis
    {
        void Analyze(List<JavaTestClass> referenceTests, List<JavaTestClass> studentTests, out FeedbackDto feedbackDto);
    }
}
