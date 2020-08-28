using System.Collections.Generic;

namespace TestingTutor.EngineModels
{
    public class FeedbackDto
    { 
        public string StudentId { get; set; }
        public int SubmissionId { get; set; }
        public IList<InstructorTestDto> InstructorTests { get; set; } = new List<InstructorTestDto>();
        public EngineExceptionDto EngineExceptionDto { get; set; }
        public IList<ClassCoverageDto> ClassCoveragesDto { get; set; } = new List<ClassCoverageDto>();
    }   
}
