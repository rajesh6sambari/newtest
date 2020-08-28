using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.JavaEngine.Utilities
{
    public class EngineProcess
    {
        protected Process Process;
        protected int WaitForExit;
        protected bool ReadStandardOut;
        protected bool ReadStandardError;
        public string Arguments { get; }
        public string Command { get; }

        public EngineProcess(string command, string arguments, string workingDirectory, int waitForExit = 100000)
        {
            Command = command;
            Arguments = arguments;
            Process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                }
            };
            ReadStandardOut = false;
            ReadStandardError = false;
            WaitForExit = waitForExit;
        }

        public int Run()
        {
            Process.Start();
            if (!Process.WaitForExit(WaitForExit))
            {
                Process.Kill();
                return -1;
            }
            var exitCode = Process.ExitCode;
            return exitCode;
        }

        public void Stop()
        {
            if (!ReadStandardError)
            {
                using (var stream = StandardError)
                {
                    stream.ReadToEnd();
                }
            }
            if (!ReadStandardOut)
            {
                using (var stream = StandardOutput)
                {
                    stream.ReadToEnd();
                }
            }
            Process.Close();
            Process.Dispose();
        }

        public StreamReader StandardError
        {
            get
            {
                ReadStandardError = true;
                return Process.StandardError;
            }
        }

        public StreamReader StandardOutput
        {
            get
            {
                ReadStandardOut = true;
                return Process.StandardOutput;
            }
        }


    }
}
