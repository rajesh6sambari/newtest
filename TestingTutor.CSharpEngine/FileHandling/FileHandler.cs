using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace TestingTutor.CSharpEngine.FileHandling
{
    public class FileHandler: IFileHandler
    {
        public void UnzipArray(byte[] bytes, string currentDirectory, string newLocation)
        {
            var path = Path.Combine(currentDirectory, "temp.zip");
            File.WriteAllBytes(path, bytes);
            ZipFile.ExtractToDirectory(path, newLocation);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
