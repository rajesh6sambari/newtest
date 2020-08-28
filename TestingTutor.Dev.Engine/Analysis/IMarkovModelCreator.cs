using System.Collections.Generic;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Options;
using TestingTutor.Dev.Engine.Utilities;

namespace TestingTutor.Dev.Engine.Analysis
{
    public interface IMarkovModelCreator
    {
        IList<MarkovModelState> Create(IList<Snapshot> snapshots, IList<IList<double>> distanceMatrix, int numberOfStates);
    }
}
