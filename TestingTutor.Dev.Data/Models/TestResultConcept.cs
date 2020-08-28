namespace TestingTutor.Dev.Data.Models
{
    public class TestResultConcept
    {
        public int Id { get; set; }
        public int InstructorTestResultId { get; set; }
        public InstructorTestResult InstructorTestResult { get; set; }
        public int TestConceptId { get; set; }
        public TestConcept TestConcept { get; set; }
    }
}
