using System.Threading.Tasks;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Data;
using TestingTutor.Dev.Engine.Options;
using TestingTutor.Dev.Engine.Utilities;

namespace TestingTutor.Dev.Engine
{
    public interface IEngineRunner
    {
        Task<EmailData> RunSubmission(SubmissionData data);
        Task RunPreAssignment(PreAssignment assignment, DirectoryHandler directory);
        Task RunMarkovModel(DevAssignment assignment, MarkovModelOptions options, DirectoryHandler directory);
    }
}
