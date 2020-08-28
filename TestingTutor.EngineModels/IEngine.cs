using System.Threading.Tasks;

namespace TestingTutor.EngineModels
{
    public interface IEngine
    {
        Task<FeedbackDto> Run(SubmissionDto submissionDto);
    }
}
