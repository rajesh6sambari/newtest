namespace TestingTutor.JavaEngine.Models
{
    public class LineCoverage
    {
        public int LineNumber { get; set; }
        public int MissedInstructions { get; set; }
        public int CoveredInstructions { get; set; }
        public int MissedBranches { get; set; }
        public int CoveredBranches { get; set; }
    }
}
