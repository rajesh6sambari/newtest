using System.Threading.Tasks;
using TestingTutor.Dev.Engine.Data;

namespace TestingTutor.Dev.Engine
{
    public interface IEmailService
    {
        Task Send(EmailData data);
    }
}
