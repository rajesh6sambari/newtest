using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;

namespace TestingTutor.Dev.Engine.Analysis
{
    public interface IAbstractSyntaxTreeMetricCreator
    {
        AbstractSyntaxTreeMetric Create(AbstractSyntaxTreeNode expected, AbstractSyntaxTreeNode actual);
    }
}
