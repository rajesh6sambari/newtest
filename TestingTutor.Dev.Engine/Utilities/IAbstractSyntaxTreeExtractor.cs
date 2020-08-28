using System.IO;
using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;

namespace TestingTutor.Dev.Engine.Utilities
{
    public interface IAbstractSyntaxTreeExtractor
    {
        AbstractSyntaxTreeNode Extract(StreamReader reader);
    }
}
