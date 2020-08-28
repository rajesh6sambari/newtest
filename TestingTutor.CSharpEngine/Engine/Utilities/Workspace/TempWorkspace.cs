using System;
using System.IO;

namespace TestingTutor.CSharpEngine.Engine.Utilities.Workspace
{
    public class TempWorkspace : IWorkspace
    {
        protected string Path;

        public TempWorkspace(string path)
        {
            Path = path;
        }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                File.SetAttributes(Path, FileAttributes.Normal);
                //Directory.Delete(Path, true);
            }
        }

        public string CreateDirectory()
        {
            if (Directory.Exists(Path))
            {
                throw new ArgumentException("Path already exist");
            }

            Directory.CreateDirectory(Path);
            return Path;
        }
    }
}
