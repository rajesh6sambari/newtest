using System;

namespace TestingTutor.Dev.Engine.Data
{
    public class EngineReportExceptionData : Exception
    {
        public EngineReportExceptionData(string message) : base(message)
        {

        }
        public string Type { get; set; }
    }
}
