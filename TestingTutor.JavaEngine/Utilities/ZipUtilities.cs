using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine;

namespace TestingTutor.JavaEngine.Utilities
{
    public static class ZipUtilities
    {
        public static void UnzipByteArray(byte[] bytes, string source, string destination)
        {
            try
            {
                var path = Path.Combine(source, "temp.zip");
                File.WriteAllText(path, System.Text.Encoding.UTF8.GetString(bytes));
                ZipFile.ExtractToDirectory(path, destination);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (IOException exception)
            {
                throw new EngineExceptionDto
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForUnzipByteArray(exception, source, destination)
                };
            }
        }

        public static Dictionary<string, MemoryStream> UnZipToMemory(byte[] bytes)
        {
            if (bytes == null) return null;

            var data = new MemoryStream(bytes);
            var result = new Dictionary<string, MemoryStream>();
            var archive = new ZipArchive(data);

            foreach (var entry in archive.Entries)
            {
                var stream = new MemoryStream();
                entry.Open().CopyTo(stream);
                if (stream.Length == 0) continue;
                result.Add(entry.FullName, stream);
            }

            return result.Count > 0 ? result : null;
        }
    }
}
