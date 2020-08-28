using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;

namespace TestingTutor.Dev.Engine.Analysis
{
    public interface IBagOfWordsMetricCreator
    {
        BagOfWordsMetric Create(AbstractSyntaxTreeNode expected, AbstractSyntaxTreeNode actual);
    }
}
