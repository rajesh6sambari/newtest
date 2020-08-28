using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.CSharpEngine.Engine.Analysis.ModuleHandlers;

namespace TestingTutor.CSharpEngine.Engine.Analysis.Trace
{
    public class ModuleCoverage
    {
        public ModuleName ModuleName { get; set; }
        public string[] Contents { get; set; }
    }
}
