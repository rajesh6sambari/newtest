using System;
using System.IO;

namespace TestingTutor.JavaEngine.Models
{
    public class WorkingDirectories : IDisposable
    {
        private readonly string _path;
        private readonly string _rootDirectoryName;
        private const string Separator = @"\";
        private const string ReferenceFolderName = "Reference";
        private const string StudentFolderName = "Student";
        private const string TestingFolderName = "Testing";
        private const string CodeFolderName = "Code";
        private const string TraceFolderName = "Trace";
        private const string ReflectionFolderName = "Reflection";

        private const string OriginalFolderName = "Original";
        public WorkingDirectories(string path = null, int? submissionId = null)
        {
            _path = path ?? @"C:\Temp";
            _rootDirectoryName = submissionId.ToString() ?? Guid.NewGuid().ToString();
            ParentDirectory = Path.Combine(_path, _rootDirectoryName);
            CreateDirectories();
        }

        public string ParentDirectory { get; }
        public string ReferenceRootDirectory => Path.Combine(ParentDirectory, ReferenceFolderName) + Separator;
        public string ReferenceCodeDirectory => Path.Combine(ReferenceRootDirectory, CodeFolderName) + Separator;
        public string ReferenceTraceDirectory => Path.Combine(ReferenceRootDirectory, TraceFolderName) + Separator;
        public string ReferenceReflectionDirectory => Path.Combine(ReferenceRootDirectory, ReflectionFolderName) + Separator;
        public string StudentRootDirectory => Path.Combine(ParentDirectory, StudentFolderName) + Separator;
        public string StudentCodeDirectory => Path.Combine(StudentRootDirectory, CodeFolderName) + Separator;
        public string StudentTraceDirectory => Path.Combine(StudentRootDirectory, TraceFolderName) + Separator;
        public string StudentReflectionDirectory => Path.Combine(StudentRootDirectory, ReflectionFolderName) + Separator;
        public string TestingRootDirectory => Path.Combine(ParentDirectory, TestingFolderName) + Separator;
        public string TestingCodeDirectory => Path.Combine(TestingRootDirectory, CodeFolderName) + Separator;
        public string TestingTraceDirectory => Path.Combine(TestingRootDirectory, TraceFolderName) + Separator;
        public string TestingReflectionDirectory => Path.Combine(TestingRootDirectory, ReflectionFolderName) + Separator;
        public string ReferenceOriginalCodeDirectory => Path.Combine(ReferenceCodeDirectory, OriginalFolderName) + Separator;
        public string StudentOriginalCodeDirectory => Path.Combine(StudentCodeDirectory, OriginalFolderName) + Separator;
        public string TestingOriginalCodeDirectory => Path.Combine(TestingCodeDirectory, OriginalFolderName) + Separator;

        private void CreateDirectories()
        {
            var path = new DirectoryInfo(_path);
            path.CreateSubdirectory(_rootDirectoryName);

            var parentDirectory = new DirectoryInfo(ParentDirectory);
            parentDirectory.CreateSubdirectory(ReferenceFolderName);
            parentDirectory.CreateSubdirectory(TestingFolderName);

            var referenceRootDirectory = new DirectoryInfo(ReferenceRootDirectory);
            referenceRootDirectory.CreateSubdirectory(CodeFolderName);
            referenceRootDirectory.CreateSubdirectory(TraceFolderName);
            referenceRootDirectory.CreateSubdirectory(ReflectionFolderName);

            var studentRootDirectory = new DirectoryInfo(StudentRootDirectory);
            studentRootDirectory.CreateSubdirectory(CodeFolderName);
            studentRootDirectory.CreateSubdirectory(TraceFolderName);
            studentRootDirectory.CreateSubdirectory(ReflectionFolderName);

            var testingRootDirectory = new DirectoryInfo(TestingRootDirectory);
            testingRootDirectory.CreateSubdirectory(CodeFolderName);
            testingRootDirectory.CreateSubdirectory(TraceFolderName);
            testingRootDirectory.CreateSubdirectory(ReflectionFolderName);

        }

        public void Dispose()
        {
            
        }
    }
}
