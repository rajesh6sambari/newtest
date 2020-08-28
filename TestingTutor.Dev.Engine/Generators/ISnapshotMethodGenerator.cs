using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;

namespace TestingTutor.Dev.Engine.Generators
{
    public interface ISnapshotMethodGenerator
    {
        SnapshotMethod Generate(AbstractSyntaxTreeNode student, AbstractSyntaxTreeNode solution,
            MethodDeclaration method);
    }
}
