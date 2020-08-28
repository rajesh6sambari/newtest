using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.UI.Data;

namespace TestingTutor.UI.Utilities
{
    public interface IAppDbContextFactory 
    {
        ApplicationDbContext Create();
    }
}