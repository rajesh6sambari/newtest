using System.IO;
using System.IO.Compression;

namespace TestingTutor.PythonEngine.Engine.Utilities.FileHandlers
{
    public class FileHandler : IFileHandler
    {
        public void UnzipByteArray(byte[] bytes, string current, string identifier)
        {
            var path = Path.Combine(current, "temp.zip");
            File.WriteAllBytes(path, bytes);
            ZipFile.ExtractToDirectory(path, identifier);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
