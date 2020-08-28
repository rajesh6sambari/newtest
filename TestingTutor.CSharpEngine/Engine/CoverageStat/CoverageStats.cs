using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.EngineModels;
using TestingTutor.CSharpEngine.Models;
using TestingTutor.CSharpEngine.Engine.Factory;
using TestingTutor.CSharpEngine.OpenCover;
using System.IO;
using System.Xml.Serialization;
using OpenCover.Framework.Model;

namespace TestingTutor.CSharpEngine.Engine.CoverageStat
{
    public class CoverageStats : ICoverageStats
    {
        private readonly IEngineFactory _factory;

        public CoverageStats(IEngineFactory factory)
        {
            _factory = factory;
        }

        public void GetStats(SubmissionDto submission, ref EngineWorkingDirectories workingDirectories, ref FeedbackDto feedback)
        {
            

        }
    }
}
