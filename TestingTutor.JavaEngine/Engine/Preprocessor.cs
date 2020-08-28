using System;
using System.Collections.Generic;
using System.IO;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine
{
    public class Preprocessor
    {
        private readonly Submission _submission;
        private readonly WorkingDirectories _workingDirectories;

        public Preprocessor(Submission submission, WorkingDirectories workingDirectories)
        {
            _submission = submission;
            _workingDirectories = workingDirectories;
        }

        private static bool CreateDirectoryPath(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception);
                return false;
            }

            return true;
        }
        
    }
}