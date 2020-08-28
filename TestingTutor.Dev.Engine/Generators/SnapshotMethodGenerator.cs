using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Analysis;
using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;
using TestingTutor.Dev.Engine.Utilities;
using TestingTutor.Dev.Engine.Utilities.Filter;

namespace TestingTutor.Dev.Engine.Generators
{
    public class SnapshotMethodGenerator : ISnapshotMethodGenerator
    {
        protected IAbstractSyntaxTreeMethodExtractor Extractor;
        protected ILineFilter Filter;
        protected IAbstractSyntaxTreeMetricCreator AbstractSyntaxTreeMetricCreator;
        protected IBagOfWordsMetricCreator BagOfWordsMetricCreator;

        public SnapshotMethodGenerator(IAbstractSyntaxTreeMethodExtractor extractor, ILineFilter filter, IAbstractSyntaxTreeMetricCreator abstractSyntaxTreeMetricCreator, IBagOfWordsMetricCreator bagOfWordsMetricCreator)
        {
            Extractor = extractor;
            Filter = filter;
            AbstractSyntaxTreeMetricCreator = abstractSyntaxTreeMetricCreator;
            BagOfWordsMetricCreator = bagOfWordsMetricCreator;
        }

        public SnapshotMethod Generate(AbstractSyntaxTreeNode studentNode, AbstractSyntaxTreeNode solutionNode, MethodDeclaration method)
        {
            var studentMethod = GetMethodOrDefault(studentNode, method);
            var solutionMethod = GetMethodOrDefault(solutionNode, method);

            if (studentMethod == null || solutionMethod == null)
                return new SnapshotMethod()
                {
                    Declared = false,
                    MethodDeclaration = method,
                };

            return new SnapshotMethod()
            {
                Declared = true,
                CodeAnalysisMetric = new CodeAnalysisMetric()
                {
                    AbstractSyntaxTreeMetric = AbstractSyntaxTreeMetricCreator.Create(solutionMethod, studentMethod),
                    BagOfWordsMetric = BagOfWordsMetricCreator.Create(solutionMethod, studentMethod),
                },
                MethodDeclaration = method
            };
        }

        public AbstractSyntaxTreeNode GetMethodOrDefault(AbstractSyntaxTreeNode node, MethodDeclaration method)
        {
            var methodNode = Extractor.ExtractOrDefault(node, method);
            if (methodNode == null) return null;
            var filter = new AbstractSyntaxTreeFilterVisitor(Filter);
            methodNode.PostOrder(filter);
            return methodNode;
        }

    }
}
