using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.CSharpEngine.Engine.Analysis.Parser
{
    public class IndividualTest
    {
        public IndividualTest(string testName)
        {
            TestName = testName;
        }

        public string TestName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return obj is IndividualTest test && test.TestName.Equals(TestName);
        }

        public List<int> LinesCovered { get; set; } = new List<int>();

        protected bool Equals(IndividualTest other)
        {
            return string.Equals(TestName, other.TestName);
        }

        public override int GetHashCode()
        {
            return (TestName != null ? TestName.GetHashCode() : 0);
        }
    }
}
