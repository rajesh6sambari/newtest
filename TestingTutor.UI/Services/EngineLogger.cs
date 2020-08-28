using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.Dev.Engine;
using TestingTutor.UI.Options;
using Microsoft.Extensions.Options;

namespace TestingTutor.UI.Services
{
    public class EngineLogger : IEngineLogger
    {
        public LoggerOptions Options;

        public EngineLogger(IOptions<LoggerOptions> options)
        {
            Options = options.Value;
        }

        public void Log(string message)
        {
            File.AppendAllText(Path.Combine(Options.RootDirectory, $"{DateTime.Today:yyyy_MMMM_dd}.txt"),
                message);
        }
    }
}
