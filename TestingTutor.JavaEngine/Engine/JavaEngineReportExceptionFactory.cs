using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using TestingTutor.JavaEngine.Engine.CoverageXml;
using TestingTutor.JavaEngine.Models;
using TestingTutor.JavaEngine.Utilities;

namespace TestingTutor.JavaEngine.Engine
{
    public static class JavaEngineReportExceptionFactory
    {
        public static string GenerateReportForCompileProcess(EngineProcess process, Exception e)
        {
            var compileError = GetErrorFromProcess(process);
            return $"Failure to compile.\n" +
                   $"{GetMessageFromException(e)}\n" +
                   $"Compile Error:\n{compileError}";
        }

        public static string GenerateReportForCompileVerification(EngineProcess process, IList<string> sourceFiles)
        {
            var compilerError = GetErrorFromProcess(process);
            var joinFiles = sourceFiles.Join(", ");
            return $"Failure to compile and verify.\nCommand args: {process.Arguments}\nSource Files: {joinFiles}\nCompile Error:\n{compilerError}";
        }

        private static string GetErrorFromProcess(EngineProcess process)
            => process.StandardError.ReadToEnd();

        public static string GenerateReportForReflectionProcess(EngineProcess p, Exception e, JavaTestClass javaTestClass)
        {
            var reflectionError = GetErrorFromProcess(p);
            var name = GetNameFromJavaClass(javaTestClass);
            return $"Failure to reflect.\n" +
                   $"Name of Java Class: {name}\n" +
                   $"{GetMessageFromException(e)}\n" +
                   $"Reflection Error: {reflectionError}";
        }

        public static string GenerateReportForReflectionMapping(Exception e, JavaTestClass javaTestClass)
        {
            var name = GetNameFromJavaClass(javaTestClass);
            return "Failure to reflex and map test methods.\n" +
                   $"Name of Java Class: {name}\n" +
                   $"{GetMessageFromException(e)}";
        }

        public static string GenerateReportForReflectionVerifyXml(int expected, string[] files)
        {
            var xmlFiles = files.Join(", ");
            return $"Failure to reflex and verify xml.\n" +
                   $"Expected number of xml: {expected}\n" +
                   ((files.Any()) ? $"Found files {files.Length}: {xmlFiles}." : "Found no xml files.");
        }

        public static string GenerateReportForJunitTestRunnerTestMethodProcess(EngineProcess p, Exception e,
            JavaTestClass javaTestClass, JavaTestMethod javaTestMethod)
        {
            var name = GetNameFromJavaMethod(javaTestClass, javaTestMethod);
            var junitTestError = GetErrorFromProcess(p);
            return "Failure to junit test runner for test testMethod.\n" +
                   $"Name of Java Method: {name}\n" +
                   $"{GetMessageFromException(e)}\n" +
                   $"Junit Test Error:\n{junitTestError}";
        }

        public static string GenerateReportForTraceJavaMethod(EngineProcess p, Exception e, JavaTestClass javaTestClass,
            JavaTestMethod javaTestMethod)
        {
            var name = GetNameFromJavaMethod(javaTestClass, javaTestMethod);
            var traceError = GetErrorFromProcess(p);
            return "Failure to run trace on java testMethod.\n" +
                   $"Name of Java Method: {name}\n" +
                   $"{GetMessageFromException(e)}\n" +
                   $"Trace Error:\n{traceError}";
        }

        public static string GenerateReportForUnpackCreateDirectoryPath(Exception e, string path)
        {
            return "Failure to unpack to directory.\n" +
                   $"{GetMessageFromException(e)}\n" +
                   $"Path: {path}";
        }

        public static string GenerateReportForUnpackWriteToDisk(Exception e, string path)
        {
            return "Failure to unpack to write to disk.\n" +
                   $"{GetMessageFromException(e)}\n" +
                   $"Path: {path}";
        }

        public static string GenerateReportForInstrumentation(EngineProcess p, Exception e)
        {
            var instrumentationError = GetErrorFromProcess(p);
            return "Failure to instrument.\n" +
                   $"{GetMessageFromException(e)}\n" +
                   $"Instrumentation Error:\n{instrumentationError}";
        }

        public static string GenerateReportForAnalysisLineCoverage(JavaTestMethod reference, JavaTestMethod student)
        {
            return "Failure to Trace Analysis.\n" +
                   "Line number counts between reference and student are not the same.\n" +
                   $"Method Name: {reference.Name}." +
                   $"Reference Line Coverage Count: {reference.LineCoverages.Count}." +
                   $"Student Line Coverage Count: {student.LineCoverages.Count}.";
        }

        private static string GetMessageFromException(Exception exception)
            => $"Exception Message: {exception.Message}\n" +
               $"Exception Source: {exception.Source}";

        private static string GetNameFromJavaClass(JavaTestClass javaTestClass)
            => $"{javaTestClass.Package}.{javaTestClass.Name}";

        private static string GetNameFromJavaMethod(JavaTestClass javaTestClass, JavaTestMethod javaTestMethod)
            => $"{GetNameFromJavaClass(javaTestClass)}#{javaTestMethod}";

        public static string GenerateReportForRawCoveragerJunitCore(EngineProcess process, Exception exception, IList<JavaTestClass> javaClasses)
        {
            var junitCoreProcess = GetErrorFromProcess(process);
            return $"Failure to run Raw Coverage JunitCore.\n" +
                   $"Name of Java Class:{GetNamesFromJavaClasses(javaClasses)}\n" +
                   $"{GetMessageFromException(exception)}\n" +
                   $"Junit Core Process:\n{junitCoreProcess}";
        }

        public static string GenerateReportForRawCoveragerReport(EngineProcess process, Exception exception, IList<JavaTestClass> javaClasses)
        {
            var reportProcess = GetErrorFromProcess(process);
            return $"Failure to run Raw Coverage Report.\n" +
                   $"Name of Java Class:{GetNamesFromJavaClasses(javaClasses)}\n" +
                   $"{GetMessageFromException(exception)}\n" +
                   $"Report Process:\n{reportProcess}";
        }

        private static string GetNamesFromJavaClasses(IList<JavaTestClass> javaClasses)
            => javaClasses.Select(GetNameFromJavaClass).Join(", ");

        public static string GenerateReportForRawCoveragerSoureFile(ReportPackageClass reportPackageClass)
        {
            return $"Failure to Find Source File in Raw Coverage Report.\n" +
                   $"Report Package Source File Looking For: {reportPackageClass.SourceFilename}\n" +
                   $"Report Package Name: {reportPackageClass.Name}";
        }

        public static string GenerateReportForUnzipByteArray(IOException ioException, string source, string destination)
        {
            return $"Failure to Unzip Byte Array.\n" +
                   $"{GetMessageFromException(ioException)}\n" +
                   $"Source: {source}\n" +
                   $"Destination: {destination}";
        }
    }
}
