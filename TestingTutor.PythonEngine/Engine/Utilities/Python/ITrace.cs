namespace TestingTutor.PythonEngine.Engine.Utilities.Python
{
    public interface ITrace
    {
        int Run(string test, string coverDir, string outputFile, string workingdirectory);
    }
}
