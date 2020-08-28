using System;
using System.Collections.Generic;
using System.Diagnostics;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Models;
using TestingTutor.JavaEngine.Utilities;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface IJunitTestRunner
    {
        void RunTests(IList<JavaTestClass> javaClasses, string originalCodeDirectory, string traceDirectory, string referenceCode = null);
    }

    public class JunitTestRunner : IJunitTestRunner
    {
        private const string Command = "java.exe";
        private const string Agent = @"-javaagent:C:\TestingTutorTools\jacoco\lib\jacocoagent.jar";
        private const string ClassPathOption = "-cp";
        private const string ClassPath = @"c:\TestingTutorTools\jacoco\lib\jacocoagent.jar;c:\TestingTutorTools\junit-4.12.jar;c:\TestingTutorTools\hamcrest-core-1.3.jar;C:\TestingTutorTools;";
        private const string Subcommand = @"SingleTestRunner";

        public void RunTests(IList<JavaTestClass> javaClasses, string originalCodeDirectory,
            string traceDirectory, string referenceCode = null)
        {
            foreach (var javaClass in javaClasses)
            {
                foreach (var method in javaClass.Methods)
                {
                    RunTestMethod(javaClass, method, originalCodeDirectory, traceDirectory, referenceCode);
                }
            }
        }

        public void RunTestMethod(JavaTestClass javaTestClass, JavaTestMethod javaTestMethod,
            string originalCodeDirectory,
            string traceDirectory, string referenceCode = null)
        {
            var packageClassAndMethod = $"{javaTestClass.Package}.{javaTestClass.Name}#{javaTestMethod.Name}";
            var commandOptions = GetCommandOptions(packageClassAndMethod, originalCodeDirectory, javaTestClass.ClassPath, javaTestClass.Name, javaTestMethod.Name, traceDirectory, referenceCode);

            var process = new EngineProcess(Command, commandOptions, originalCodeDirectory);
            try
            {
                var exitCode = process.Run();
                javaTestMethod.Passed = exitCode == 0;
            }
            catch (Exception e)
            {
                var exception = new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForJunitTestRunnerTestMethodProcess(process, e, javaTestClass, javaTestMethod)
                };
                process.Stop();
                throw exception;
            }
            process.Stop();
        }

        
        public string GetCommandOptions(string packageClassAndMethod, string originalCodeDirectory, string javaClassPath, string javaClassName, string methodName, string traceDirectory, string referenceCode = null)
            => $"{Agent}=destfile={traceDirectory}\\{javaClassName}-{methodName}.exec {ClassPathOption} {ClassPath}{originalCodeDirectory}; {Subcommand} {originalCodeDirectory} {packageClassAndMethod}";
    }
}
