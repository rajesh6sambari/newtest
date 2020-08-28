using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.EngineModels;

namespace TestingTutor.CSharpEngine.Engine.Feedback
{
    public interface IFeedbackSender
    {
        void SendFeedback(FeedbackDto feedback);
    }
}
