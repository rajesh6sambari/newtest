using System.Collections.Generic;
using System.IO;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Utilities;

namespace TestingTutor.JavaEngine.Models
{
    public class Submission
    {
        public static Submission MapFrom(SubmissionDto submissionDto)
        {
            return new Submission
            {
                SubmitterId = submissionDto.SubmitterId,
                SubmissionId = submissionDto.SubmissionId,
                ApplicationMode = submissionDto.ApplicationMode,
                ReferenceSolution = ZipUtilities.UnZipToMemory(submissionDto.ReferenceSolution),
                ReferenceTestSolution = ZipUtilities.UnZipToMemory(submissionDto.ReferenceTestSolution),
                PackageName = submissionDto.SolutionFolderName,
                AssignmentSolution = ZipUtilities.UnZipToMemory(submissionDto.AssignmentSolution),
                TestCaseSolution = ZipUtilities.UnZipToMemory(submissionDto.TestCaseSolution)
            };
        }

        public string SubmitterId { get; private set; }
        public int SubmissionId { get; private set; }
        public string ApplicationMode { get; set; }
        public Dictionary<string, MemoryStream> ReferenceSolution { get; private set; }
        public Dictionary<string, MemoryStream> ReferenceTestSolution { get; private set; }
        public string PackageName { get; private set; }
        public Dictionary<string, MemoryStream> AssignmentSolution { get; private set; }
        public Dictionary<string, MemoryStream> TestCaseSolution { get; private set; }
    }
}
