using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.EngineModels;
using TestingTutor.CSharpEngine.Models;

namespace TestingTutor.CSharpEngine.Engine.CoverageStat
{
    public interface ICoverageStats
    {
        void GetStats(SubmissionDto submission, ref EngineWorkingDirectories workingDirectories,
            ref FeedbackDto feedback);
    }
}
