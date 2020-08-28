using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Internal;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.CoverageXml;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;
using TestingTutor.JavaEngine.Utilities;

namespace TestingTutor.JavaEngine.Engine
{
    public class RawCoverager : IRawCoverager
    {
        private const string Command = "java";
        private const string JunitCommand = @"-javaagent:C:\TestingTutorTools\Jacoco\lib\jacocoagent.jar -cp c:\TestingTutorTools\junit-4.12.jar;C:\TestingTutorTools\hamcrest-all-1.3.jar; org.junit.runner.JUnitCore";
        private const string ReportCommand = @"-jar C:\TestingTutorTools\jacoco\lib\jacococli.jar report jacoco.exec";
        private const string ReportOutput = @"--xml";
        private const string ReportName = "Report.xml";
        private const string ClassPath = "--classfiles";
        private const string SourceFiles = "--sourcefiles";

        public void RawCoverage(string originalCodeDirectory, List<JavaTestClass> studentClasses, out IList<ClassCoverageDto> classCoverageDtos)
        {
            var testClasses = studentClasses.ToList();

            SetupJUnitCore(originalCodeDirectory, testClasses);
            GenerateReport(originalCodeDirectory, testClasses);
            classCoverageDtos = GetClassCoverageDtos(originalCodeDirectory);
        }

        public IList<ClassCoverageDto> GetClassCoverageDtos(string codeDirectory)
        {
            var classCoverageDtos = new List<ClassCoverageDto>();

            var report = GetReport(Path.Combine(codeDirectory, ReportName));

            foreach (var javaClass in report.Package.ReportPackageClasses)
            {
                if (!javaClass.SourceFilename.Contains("Test", StringComparison.OrdinalIgnoreCase))
                {
                    classCoverageDtos.Add(GetClassCoverageDto(report.Package.Name, javaClass, GetSourceFile(javaClass, report.Package.SourceFiles)));
                }

            }

            return classCoverageDtos;
        }

        private ClassCoverageDto GetClassCoverageDto(string container, ReportPackageClass javaClass, ReportPackageSourceFile sourceFile)
        {
            var classCoverageDto = new ClassCoverageDto()
            {
                Name = javaClass.Name,
                Container = container
            };

            if (javaClass.Method.Length > 0)
            {
                for (var index = 0; index < javaClass.Method.Length - 1; index++)
                {
                    var method = javaClass.Method[index];
                    var nextMethod = javaClass.Method[index + 1];
                    classCoverageDto.MethodCoveragesDto
                        .Add(GetMethodCoverage(method, sourceFile, nextMethod.Line));
                }
                classCoverageDto.MethodCoveragesDto.Add(GetMethodCoverage(javaClass.Method[javaClass.Method.Length - 1], sourceFile));
            }

            return classCoverageDto;
        }

        private MethodCoverageDto GetMethodCoverage(ReportPackageClassMethod method, ReportPackageSourceFile sourceFile)
        {
            var lines = sourceFile.Lines.Where(line => line.LineNumber >= method.Line).ToList();
            return CalculateMethodCoverage(method, lines);
        }

        private MethodCoverageDto GetMethodCoverage(ReportPackageClassMethod method, ReportPackageSourceFile sourceFile, int lineEnd)
        {
            var lines = sourceFile.Lines.Where(line => line.LineNumber >= method.Line && line.LineNumber < lineEnd).ToList();

            return CalculateMethodCoverage(method, lines);
        }

        private MethodCoverageDto CalculateMethodCoverage(ReportPackageClassMethod method, IList<ReportPackageSourceFileLine> lines)
        {
            var branches = lines.Where(line => line.MissedBranches > 0 || line.CoveredBranches > 0).ToList();
            return new MethodCoverageDto()
            {
                Name = method.Name,
                LinesMissed = lines.Count(line => line.MissingInstructions > 0),
                LinesCovered = lines.Count(line => line.MissingInstructions == 0),
                BranchesCovered = branches.Count(line => line.MissedBranches == 0),
                BranchesMissed = branches.Count(line => line.MissedBranches > 0),
                ConditionsCovered = (branches.Count > 0) ? branches.Sum(line => line.CoveredBranches) : 0,
                ConditionsMissed = (branches.Count > 0) ? branches.Sum(line => line.MissedBranches) : 0,
            };
        }


        private ReportPackageSourceFile GetSourceFile(ReportPackageClass javaClass, ReportPackageSourceFile[] packageSourceFiles)
        {
            try
            {
                return packageSourceFiles.First(file => file.Name == javaClass.SourceFilename);
            }
            catch (Exception e)
            {
                throw new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForRawCoveragerSoureFile(javaClass)
                };
            }
        }

        public Report GetReport(string fileUri)
        {
            var text = File.ReadAllText(fileUri);

            var xRoot = new XmlRootAttribute()
            {
                ElementName = "report",
                IsNullable = true
            };

            var xmlSerializer = new XmlSerializer(typeof(Report), xRoot);
            var buffer = Encoding.UTF8.GetBytes(text);
            using (var stream = new MemoryStream(buffer))
            {
                return (Report)xmlSerializer.Deserialize(stream);
            }
        }


        public void GenerateReport(string workingDirectory, IList<JavaTestClass> javaClasses)
        {
            var process = new EngineProcess(Command, GetCommonOptionsForReport(javaClasses), workingDirectory);
            try
            {
                process.Run();
            }
            catch (Exception e)
            {
                var exception =  new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForRawCoveragerReport(process, e, javaClasses)
                };
                process.Stop();
                throw exception;
            }
            process.Stop();
        }

        private string GetCommonOptionsForReport(IList<JavaTestClass> javaClasses)
            => $"{ReportCommand} {ReportOutput} {ReportName} {ClassPath} {GetPackageDirectoryList(javaClasses)} {SourceFiles} {GetPackageDirectoryList(javaClasses)}";

        private string GetPackageDirectoryList(IList<JavaTestClass> javaClasses)
        {
            return javaClasses.Select(javaClass => javaClass.PackageDirectory).Distinct().ToArray().Join(" ");
        }

        public void SetupJUnitCore(string workingDirectory, IList<JavaTestClass> javaClasses)
        {
            var process = new EngineProcess(Command, GetCommonOptionsForJunitCore(javaClasses), workingDirectory);
            try
            {
                process.Run();
            }
            catch (Exception e)
            {
                var exception = new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForRawCoveragerJunitCore(process, e, javaClasses)
                };
                process.Stop();
                throw exception;
            }
            process.Stop();
        }

        public string GetCommonOptionsForJunitCore(IList<JavaTestClass> javaClass)
            => $"{JunitCommand} {GetPackageNames(javaClass)}";

        public string GetPackageNames(IList<JavaTestClass> javaClasses)
            => javaClasses.Select(javaClass => $"{javaClass.Package}.{javaClass.Name}").Distinct().ToArray().Join(" ");

    }
}
