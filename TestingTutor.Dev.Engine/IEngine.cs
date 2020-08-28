using System.Threading.Tasks;
using TestingTutor.Dev.Data.Dtos;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Options;

namespace TestingTutor.Dev.Engine
{
    public interface IEngine
    {
        Task RunSubmission(StudentSubmissionDto submission);
        Task RunPreAssignment(PreAssignment assignment);
        Task RunMarkovModel(DevAssignment assignment, MarkovModelOptions options);
    }
}
