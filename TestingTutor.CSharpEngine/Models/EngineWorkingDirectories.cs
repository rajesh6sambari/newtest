using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.CSharpEngine.Models
{
    public class EngineWorkingDirectories
    {
        public string ParentDirectory { get; set; }
        public string StudentTestSuit { get; set; }
        public string ReferenceTestSuit { get; set; }
        public string Solution { get; set; }
        public string InstructorDllPath { get; set; }
        public string StudentDllPath { get; set; }
    }
}
