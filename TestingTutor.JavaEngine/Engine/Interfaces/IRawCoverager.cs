using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface IRawCoverager
    {
        void RawCoverage(string originalCodeDirectory, List<JavaTestClass> studentClasses, out IList<ClassCoverageDto> classCoverageDtos);
    }
}
