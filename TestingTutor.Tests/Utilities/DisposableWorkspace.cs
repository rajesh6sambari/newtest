using System;
using System.IO;

namespace TestingTutor.Tests.Utilities
{
    public class DisposableWorkspace : IDisposable
    {
        public DisposableWorkspace(string path)
        {
            Path = path;
            CreateWorkspace();
        }

        public string Path { get; }

        private void CreateWorkspace()
        {
            if (Directory.Exists(Path))
            {
                RemoveWorkspace();
            }
            Directory.CreateDirectory(Path);
        }
        public void Dispose()
        {
            RemoveWorkspace();
        }


        private void RemoveWorkspace()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, true);
            }
        }

    }
}
