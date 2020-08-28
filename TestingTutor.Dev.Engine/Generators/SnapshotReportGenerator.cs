using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;
using TestingTutor.Dev.Engine.Data;
using TestingTutor.Dev.Engine.Utilities;

namespace TestingTutor.Dev.Engine.Generators
{
    public class SnapshotReportGenerator : ISnapshotReportGenerator
    {
        protected IAbstractSyntaxTreeGenerator AbstractSyntaxTreeGenerator;
        protected IAbstractSyntaxTreeClassExtractor ClassExtractor;
        protected IRepository<SnapshotReport, int> SnapshotReports;
        protected ISnapshotMethodGenerator MethodGenerator;
        protected IUnitTestGenerator UnitTestGenerator;
        public SnapshotReportGenerator(IAbstractSyntaxTreeGenerator abstractSyntaxTreeGenerator, IAbstractSyntaxTreeClassExtractor classExtractor,
            IRepository<SnapshotReport, int> snapshotReports, ISnapshotMethodGenerator methodGenerator, IUnitTestGenerator unitTestGenerator)
        {
            AbstractSyntaxTreeGenerator = abstractSyntaxTreeGenerator;
            ClassExtractor = classExtractor;
            SnapshotReports = snapshotReports;
            MethodGenerator = methodGenerator;
            UnitTestGenerator = unitTestGenerator;
        }

        public async Task<SnapshotReport> Generate(SubmissionData data, string snapshot, DevAssignment assignment, AbstractSyntaxTreeNode solutionNode)
        {
            try
            {
                var report = await GenerateImpl(data, snapshot, assignment, solutionNode);
                return report;
            }
            catch (EngineReportExceptionData exception)
            {
                return new SnapshotFailureReport()
                {
                    Report = $"Error Type: {exception.Type}.\n{exception.Message}",
                };
            }
        }

        public async Task<SnapshotReport> GenerateImpl(SubmissionData data, string snapshot, DevAssignment assignment,
            AbstractSyntaxTreeNode solutionNode)
        {
            var studentNode = GetStudentSnapshot(data, snapshot, assignment);

            var snapshotMethods = assignment
                .Solution
                .MethodDeclarations
                .Select(methodDeclaration =>
                    MethodGenerator.Generate(studentNode, solutionNode, methodDeclaration))
                .ToList();

            var unitTests = await UnitTestGenerator.GenerateResults(data, snapshot,
                assignment, snapshotMethods);

            var report = new SnapshotSuccessReport()
            {
                SnapshotMethods = snapshotMethods,
                UnitTestResults = unitTests,
            };

            await SnapshotReports.Add(report);

            return report;
        }


        public AbstractSyntaxTreeNode GetStudentSnapshot(SubmissionData data, string snapshot, DevAssignment assignment)
        {
            var root = AbstractSyntaxTreeGenerator.CreateFromFile(data, data.SnapshotSourceFileFullPath(snapshot, assignment.Filename));
            var classNode = ClassExtractor.Extract(root, assignment.Solution.Name);
            return classNode;
        }


    }
}
