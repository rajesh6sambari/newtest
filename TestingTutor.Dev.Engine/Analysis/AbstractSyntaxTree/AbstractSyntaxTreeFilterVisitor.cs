using System.Linq;
using TestingTutor.Dev.Engine.Utilities.Filter;

namespace TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree
{
    public class AbstractSyntaxTreeFilterVisitor : IAbstractSyntaxTreeVisitor
    {
        protected ILineFilter Filter;
        public AbstractSyntaxTreeFilterVisitor(ILineFilter filter)
        {
            Filter = filter;
        }

        public void Visit(AbstractSyntaxTreeNode node)
        {
            node.Value = Filter.Filter(node.Value);
        }
    }
}
