namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers
{
    public class IndividualTest
    {
        public IndividualTest(string testName)
        {
            TestName = testName;
        }

        public string TestName { get; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return obj is IndividualTest test && test.TestName.Equals(TestName);
        }


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
