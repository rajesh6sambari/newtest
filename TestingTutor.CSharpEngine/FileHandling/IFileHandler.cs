using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.CSharpEngine.FileHandling
{
    public interface IFileHandler
    {
        void UnzipArray(byte[] bytes, string currentDirectory, string newLocation);
    }
}
