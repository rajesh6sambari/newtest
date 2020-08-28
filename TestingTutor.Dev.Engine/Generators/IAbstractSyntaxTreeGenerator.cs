using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;
using TestingTutor.Dev.Engine.Data;
using TestingTutor.Dev.Engine.Utilities;

namespace TestingTutor.Dev.Engine.Generators
{
    public interface IAbstractSyntaxTreeGenerator
    {
        AbstractSyntaxTreeNode CreateFromFile(SubmissionData data, string path);
        AbstractSyntaxTreeNode CreateFromFile(DirectoryHandler handler, string path);
        AbstractSyntaxTreeNode CreateOrDefaultFromFile(DirectoryHandler handler, string path);
    }
}
