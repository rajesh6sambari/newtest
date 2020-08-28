using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;

namespace TestingTutor.Dev.Engine.Utilities
{
    public interface IAbstractSyntaxTreeMethodExtractor
    {
        AbstractSyntaxTreeNode ExtractOrDefault(AbstractSyntaxTreeNode root, MethodDeclaration methodDeclaration);
    }
}
