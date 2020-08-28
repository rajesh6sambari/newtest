namespace TestingTutor.PythonEngine.Engine.Utilities.Python
{
    public interface IPytest
    {
        int Run(string arguments, string output, string workingDirectory);
    }
}
