using System.Threading.Tasks;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine
{
    public class JavaEngine : IEngine
    {
        private Submission _submission;
        public Task<FeedbackDto> Run(SubmissionDto submissionDto)
        {
            _submission = Submission.MapFrom(submissionDto);

            ModeEngineFactory.GetEngine(_submission).Run(out var feedbackDto);
            feedbackDto.StudentId = submissionDto.SubmitterId;
            feedbackDto.SubmissionId = submissionDto.SubmissionId;
            return Task.FromResult(feedbackDto);
        }
    }
}
