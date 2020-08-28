namespace TestingTutor.EngineModels
{
    public class SubmissionDto
    {
        public string SubmitterId { get; set; }
        public int SubmissionId { get; set; }
        public string ApplicationMode { get; set; }
        public byte[] ReferenceSolution { get; set; }
        public byte[] ReferenceTestSolution { get; set; }
        public string SolutionFolderName { get; set; }
        public byte[] AssignmentSolution { get; set; }
        public byte[] TestCaseSolution { get; set; }
    }
}
