using System.Diagnostics;

namespace TestingTutor.PythonEngine.Engine.Utilities.Python
{
    public class PyTracer : ITrace
    {
        public string PytestFile { get; }

        public PyTracer(string pytestFile)
        {
            PytestFile = pytestFile;
        }

        public int Run(string test, string coverDir, string outputFile, string workingdirectory)
        {
            var process = new Process();
            var startinfo =
                new ProcessStartInfo()
                {
                    FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    Arguments = $"python -m trace --count -C {coverDir} {PytestFile} {test} > {outputFile}",
                    WorkingDirectory = workingdirectory
                };
            process.StartInfo = startinfo;
            process.Start();
            process.WaitForExit();
            var exitCode = process.ExitCode;
            process.Close();
            process.Dispose();
            return exitCode;
        }
    }
}
