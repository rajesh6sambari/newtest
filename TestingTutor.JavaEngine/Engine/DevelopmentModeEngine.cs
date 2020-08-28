using System;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine
{
    public class DevelopmentModeEngine : IModeEngine
    {
        private readonly IDevelopmentMode _mode;

        public DevelopmentModeEngine(IDevelopmentMode mode) => _mode = mode;

        public void Run(out FeedbackDto feedbackDto)
        {
            try
            {
                _mode.Phase0Preparation();
                _mode.Phase1ExecutionOfReferenceTestCases();
                _mode.Phase2ExecutionOfStudentTestCases(out feedbackDto);
            }
            catch (EngineExceptionDto engineException)
            {
                feedbackDto = new FeedbackDto()
                {
                    EngineExceptionDto = engineException,
                };
            }
            _mode.Phase3Cleanup();
        }
    }
}