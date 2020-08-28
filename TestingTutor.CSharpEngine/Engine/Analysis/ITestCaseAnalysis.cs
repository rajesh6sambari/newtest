using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.CSharpEngine.Models;
using TestingTutor.EngineModels;

namespace TestingTutor.CSharpEngine.Engine.Analysis
{
    public interface ITestCaseAnalysis
    {
        void Run(SubmissionDto submissionDTO, ref EngineWorkingDirectories workingDirectories,
            ref FeedbackDto feedback);
    }
}
