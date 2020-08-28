using System;
using System.Collections.Generic;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;
using TestingTutor.JavaEngine.Utilities;

namespace TestingTutor.JavaEngine.Engine
{
    public class TraceProducer : ITracerProducer
    {
        private const string Command = "java.exe";
        private const string ClassFilesOption = "--classfiles";
        private const string SourceFilesOption = "--sourcefiles";
        private const string XmlOption = "--xml";
        private const string Subcommand = @"-jar C:\TestingTutorTools\jacoco\lib\jacococli.jar report";

        public void Trace(string workingDirectory, ref List<JavaTestClass> javaClasses)
        {
            foreach (var javaClass in javaClasses)
            {
                foreach (var method in javaClass.Methods)
                {
                    TraceJavaMethod(javaClass, method, workingDirectory);
                }
            }
        }

        public void TraceJavaMethod(JavaTestClass javaTestClass, JavaTestMethod testMethod, string workingDirectory)
        {
            var commandOptions = GetCommandOptions(javaTestClass.Name, testMethod.Name, javaTestClass.ClassPath);
            var process = new EngineProcess(Command, commandOptions, workingDirectory);
            try
            {
                process.Run();
            }
            catch (Exception e)
            {
                var exception =  new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForTraceJavaMethod(process, e, javaTestClass, testMethod)
                };
                process.Stop();
                throw exception;
            }
            process.Stop();
        }

        public string GetCommandOptions(string className, string methodName, string classPath)
        {
            return $"{Subcommand} {className}-{methodName}.exec {ClassFilesOption} {classPath} {SourceFilesOption} {classPath} {XmlOption} {className}-{methodName}.xml";
        }
    }
}