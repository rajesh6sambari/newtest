using System.Collections.Generic;

namespace TestingTutor.JavaEngine.Models
{
    public class JavaTestMethod
    {
        public string Name { get; set; }
        public string EquivalenceClass { get; set; }
        public string[] LearningConcepts { get; set; }
        public IList<LineCoverage> LineCoverages { get; } = new List<LineCoverage>();
        public void AddLineCoverage(LineCoverage lineCoverage) => LineCoverages.Add(lineCoverage);
        public bool Passed { get; set; }
    }
}
