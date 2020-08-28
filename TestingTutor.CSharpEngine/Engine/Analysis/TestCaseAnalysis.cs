using System;
using System.Collections.Generic;
using System.IO;
using TestingTutor.CSharpEngine.Models;
using TestingTutor.EngineModels;
using TestingTutor.CSharpEngine.Engine.Analysis.Trace;
using TestingTutor.CSharpEngine.Engine.Analysis.Parser;
using TestingTutor.CSharpEngine.Engine.Factory;
using OpenCover.Framework.Model;

namespace TestingTutor.CSharpEngine.Engine.Analysis
{
    public class TestCaseAnalysis:ITestCaseAnalysis
    {
        private IList<AnnotatedTest> annotatedTests = new List<AnnotatedTest>();
        protected IEngineFactory EngineFactory;
        

        public TestCaseAnalysis(IEngineFactory engineFactory)
        {
            EngineFactory = engineFactory;
        }

        public void Run(SubmissionDto submissionDto, ref EngineWorkingDirectories workingDirectories, ref FeedbackDto feedback)
        {
            var instructorTestSuit = CollectTestCases(workingDirectories.ReferenceTestSuit,workingDirectories.Solution, ref workingDirectories);

            var studentTestSuit = CollectTestCases(workingDirectories.StudentTestSuit, workingDirectories.Solution, ref workingDirectories);

            var coverDirectory = Path.Combine(workingDirectories.ParentDirectory, "CoverDirectory");
            Directory.CreateDirectory(coverDirectory);

            var tracer = new Tracer(EngineFactory);

            var instructorTestCoverage = tracer.GetTestCoverage(workingDirectories.InstructorDllPath,
                coverDirectory, instructorTestSuit);
            var studentTestCoverage = tracer.GetTestCoverage(workingDirectories.StudentDllPath,
                coverDirectory, studentTestSuit);

            var comparator = EngineFactory.Comparator();
            
            comparator.Compare(ref annotatedTests, ref instructorTestCoverage, ref studentTestCoverage,
                ref feedback, workingDirectories);
        }

        public IList<IndividualTest> CollectTestCases(string testDirectory, string solution, ref EngineWorkingDirectories workingDirectories)
        {
            AnnotatedTestParser testParser = new AnnotatedTestParser(); 

            string path = GetDirectoryPaths(testDirectory, solution, ref  workingDirectories);

            var annotations = testParser.Setup(path, null, workingDirectories);

            List<IndividualTest> individualTests = new List<IndividualTest>();

            foreach(AnnotatedTest t in annotations)
            {
                individualTests.Add(t.IndividualTest);
            }

            annotatedTests = annotations;

            return individualTests;
        }

        public string GetDirectoryPaths(string testDirectory, string solution, ref EngineWorkingDirectories workingDirectories)
        {
            //Creates new solutin folder for combination of solution and test
            var newPath = Directory.CreateDirectory(testDirectory + "//CombineSolution");

            //Copy content to new folder
            DirectoryCopy(solution, newPath.FullName, true);
            var solutionPath = newPath.GetDirectories();

            string returnString = "";

            if (solutionPath[0] != null)
            {

                var path = Directory.GetDirectories(testDirectory);

                if (path[1] != null)
                {
                    DirectoryCopy(path[1], solutionPath[0].FullName, true);
                    var dirs = Directory.GetDirectories(solutionPath[0].FullName);

                    foreach(var d in dirs)
                    {
                        if (d.Contains("Test"))
                        {
                            returnString = d;
                        }
                    }
                
                }
                else
                {
                    throw new Exception("Directory not found!");
                }
            }
            else
            {
                throw new Exception("Directory not found!");
            } 

            if(testDirectory.Contains("Student"))
            {
                workingDirectories.StudentDllPath = returnString;
            }
            else
            {
                workingDirectories.InstructorDllPath = returnString;
            }

            return returnString;
        }

        private Exception Exception(string v)
        {
            throw new NotImplementedException();
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
                
            }
        }
    }
}

