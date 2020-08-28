using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Factory;
using TestingTutor.PythonEngine.Engine.Models;

namespace TestingTutor.PythonEngine.Engine.Brain.CoverageStat
{
    public class CoverageStats : ICoverageStats
    {
        private readonly IEngineFactory _factory;

        private readonly Regex _coverageHeaderRegex = new Regex("Name\\s+Stmts\\s+Miss\\s+Branch\\s+BrPart\\s+Cover");

        public CoverageStats(IEngineFactory factory)
        {
            _factory = factory;
        }

        public void GetStats(SubmissionDto submission, ref EngineWorkingDirectories workingDirectories, ref FeedbackDto feedback)
        {
            var pytestcov = _factory.PytestCov();

            var filepath = Path.Combine(workingDirectories.ParentDirectory, "stats");

            pytestcov.Run($"--cov-branch --cov={workingDirectories.Solution} .", filepath, workingDirectories.StudentTestSuit);

            var file = File.ReadAllLines(filepath);

            var found = false;

            foreach (var line in file)
            {
                if (found)
                {
                    if (!line.StartsWith('-') && !line.StartsWith('=') && !string.IsNullOrWhiteSpace(line))
                    {
                        var match = Regex.Match(line, @"(.+)(\s+)(\d+)(\s+)(\d+)(\s+)(\d+)(\s+)(\d+)(\s+)(\d+)(%)(\s*)");
                        if (match.Groups.Count == 14)
                        {
                            feedback.NumberOfStatements = int.Parse(match.Groups[3].Value);
                            feedback.NumberOfMissingStatements = int.Parse(match.Groups[5].Value);
                            feedback.NumberOfBranchesHit = int.Parse(match.Groups[7].Value);
                            feedback.CoveragePercentage = Convert.ToDouble(int.Parse(match.Groups[11].Value)) / 100.00f;
                        }

                    }
                }
                else
                {
                    if (_coverageHeaderRegex.IsMatch(line))
                    {
                        found = true;
                    }
                }
            }

        }
    }
}
