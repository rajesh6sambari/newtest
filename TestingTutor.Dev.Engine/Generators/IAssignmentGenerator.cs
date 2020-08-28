using System.Threading.Tasks;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Utilities;

namespace TestingTutor.Dev.Engine.Generators
{
    public interface IAssignmentGenerator
    {
        Task Generate(PreAssignment assignment, DirectoryHandler handler);
    }
}
