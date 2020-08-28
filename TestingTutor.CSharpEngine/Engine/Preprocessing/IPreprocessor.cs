using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.EngineModels;
using TestingTutor.CSharpEngine.Models;

namespace TestingTutor.CSharpEngine.Engine.Preprocessing
{
    public interface IPreprocessor
    {
        bool Preprocessing(SubmissionDto submissionDTO, string directory, out EngineWorkingDirectories workingDirectories,
            out FeedbackDto feedback);
    }
}
