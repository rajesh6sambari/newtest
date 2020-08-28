using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Utilities
{
    public static class JavaUtilities
    {
        public static Process GetProcess(string command, string arguments, string workingDirectory)
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory,
                }
            };
        }

        public static int RunProcess(Process process, out string output)
        {
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(100000);
            return process.ExitCode;
        }

        public static void Unpack(Dictionary<string, MemoryStream> source, string workingDirectory, out CompilationUnit unit)
        {
            unit = new CompilationUnit();
            foreach (var (key, value) in source)
            {
                if (!key.EndsWith(".java")) continue;

                var split = key.Split("/");
                var packageAndFile = string.Join("\\", split);
                var packageDirectory = Path.GetDirectoryName(packageAndFile);
                var fullyQualifiedPackageName = packageDirectory.Replace(@"\", ".");
                var fileName = Path.GetFileName(packageAndFile);
                var fullUri = Path.Combine(workingDirectory, packageAndFile);

                if (fileName.Contains("Test", StringComparison.CurrentCultureIgnoreCase))
                {
                    var javaFile = GetJavaTestClass(workingDirectory, fullUri, packageDirectory, fullyQualifiedPackageName, fileName);
                    unit.Add(javaFile);
                }

                unit.SourceFiles.Add(fullUri);

                CreateDirectoryPath(Path.GetDirectoryName(fullUri));
                File.WriteAllText(fullUri, Encoding.ASCII.GetString(value.ToArray()), Encoding.ASCII);

            }
        }
        private static void CreateDirectoryPath(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (IOException exception)
            {
                throw new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForUnpackCreateDirectoryPath(exception, path)
                };
            }
        }

        private static void WriteToDisk(string uri, MemoryStream stream)
        {
            try
            {
                File.WriteAllBytes(uri, stream.GetBuffer());
            }
            catch (Exception exception)
            {
                throw new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForUnpackWriteToDisk(exception, uri)
                };
            }
        }

        private static JavaTestClass GetJavaTestClass(string workingDirectory, string fullUri, string packageDirectory, string fullyQualifiedPackageName, string file)
        {
            var classPath = Path.Combine(workingDirectory, packageDirectory + @"\");
            return new JavaTestClass
            {
                Name = Path.GetFileNameWithoutExtension(file),
                FileUri = fullUri,
                PackageDirectory = packageDirectory,
                Package = fullyQualifiedPackageName,
                ClassPath = classPath
            };
        }

        public static bool VerifyFilesExist(IList<string> files)
        {
            return files.All(File.Exists);
        }
    }
}
