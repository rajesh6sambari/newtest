using System;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.Dev.Engine.Data
{
    public class EngineAssignmentExceptionData : Exception
    {
        public PreAssignmentReport Report { get; set; }
    }
}
