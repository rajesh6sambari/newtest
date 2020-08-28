using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Utilities;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface IJavaCompiler
    {
        void Compile(string workingDirectory, IList<string> sourceFiles);
    }

    public class JavaCompiler2 : IJavaCompiler
    {
        private const string Command = "javac.exe";
        private const string Extension = ".java";
        private const string ClassPathOption = "-classpath";
        private const string ClassPath = @"c:\TestingTutorTools\junit-4.12.jar;c:\TestingTutorTools\hamcrest-core-1.3.jar;C:\TestingTutorTools\TestingTutorAnnotation.jar;C:\TestingTutorTools\xstream\lib\xstream-1.4.11.1.jar;";

        public void Compile(string workingDirectory, IList<string> sourceFiles)
        {
            var process = new EngineProcess(Command, GetCommandOptions(sourceFiles), workingDirectory);
            RunProcess(process);
            VerifyCompilation(process, sourceFiles);
            process.Stop();
        }

        private void VerifyCompilation(EngineProcess process, IList<string> sourceFiles)
        {
            if (!VerifyCompile(sourceFiles))
            {
                var exception = new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory.GenerateReportForCompileVerification(process, sourceFiles),
                };
                process.Stop();
                throw exception;
            }
        }

        private void RunProcess(EngineProcess process)
        {
            try
            {
                process.Run();
            }
            catch (Exception e)
            {
                var exception =  new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForCompileProcess(process, e)
                };
                process.Stop();
                throw exception;
            }
        }

        private static string GetCommandOptions(IEnumerable<string> files)
        {
            var builder = new StringBuilder();

            files.ToList().ForEach(file => builder.Append($" {file} "));
            builder.Append($" {ClassPathOption} {ClassPath}");

            return builder.ToString();
        }

        private static bool VerifyCompile(IEnumerable<string> sourceFiles)
        {
            var classFiles = new List<string>();
            sourceFiles.ToList().ForEach(file => classFiles.Add(file.Replace(".java", ".class")));
           
            return JavaUtilities.VerifyFilesExist(classFiles);
        }
    }
}
