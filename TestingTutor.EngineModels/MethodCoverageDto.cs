namespace TestingTutor.EngineModels
{
    public class MethodCoverageDto
    {
        public string Name { get; set; }
        
        public int LinesCovered { get; set; }
        
        public int LinesMissed { get; set; }
        
        public int BranchesCovered { get; set; }
        
        public int BranchesMissed { get; set; }
        
        public int ConditionsCovered { get; set; }
        
        public int ConditionsMissed { get; set; }
    }
}