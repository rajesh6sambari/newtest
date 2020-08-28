using System.Diagnostics;

namespace TestingTutor.PythonEngine.Engine.Utilities.Python
{
    public interface IPytestCov
    {
        int Run(string arguments, string output, string workingDirectory);
    }
}
