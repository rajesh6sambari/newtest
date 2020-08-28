using System;
using System.IO;

namespace TestingTutor.PythonEngine.Engine.Utilities.Workspaces
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
                Directory.Delete(Path, true);
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
