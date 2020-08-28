using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;

namespace TestingTutor.Dev.Engine.Utilities
{
    public interface IAbstractSyntaxTreeClassExtractor
    {
        AbstractSyntaxTreeNode Extract(AbstractSyntaxTreeNode root, string name);
        AbstractSyntaxTreeNode ExtractOrDefault(AbstractSyntaxTreeNode root, string name);
    }
}
