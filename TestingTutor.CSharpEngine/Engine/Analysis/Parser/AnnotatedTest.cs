using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.CSharpEngine.Engine.Analysis.ModuleHandlers;

namespace TestingTutor.CSharpEngine.Engine.Analysis.Parser
{
    public class AnnotatedTest
    {
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return obj is AnnotatedTest test && test.EquivalanceClass.Equals(EquivalanceClass) && test.IndividualTest.Equals(IndividualTest)
                && test.Concepts.OrderBy(o => o).SequenceEqual(Concepts.OrderBy(o => o));
        }

        protected bool Equals(AnnotatedTest other)
        {
            return string.Equals(EquivalanceClass, other.EquivalanceClass) && IndividualTest.Equals(other.IndividualTest)
                && Concepts.OrderBy(o => o).SequenceEqual(other.Concepts.OrderBy(o => o));
        }

        public string EquivalanceClass { get; set; } = "NONE";
        public IList<string>Concepts { get; set; } = new List<string>();
        public IndividualTest IndividualTest { get; set; } = new IndividualTest("None");
    }
}
