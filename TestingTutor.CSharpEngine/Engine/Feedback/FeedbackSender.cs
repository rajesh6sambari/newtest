using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TestingTutor.EngineModels;

namespace TestingTutor.CSharpEngine.Engine.Feedback
{
    public class FeedbackSender : IFeedbackSender
    {
        public async void SendFeedback(FeedbackDto feedback)
        {
            using (var client = new HttpClient())
            {
                //var response = await client.PostAsJsonAsync("http://localhost:49829/api/Feedbacks", feedback);
                var response = await client.PostAsJsonAsync("https://localhost:44353/api/Feedback", feedback);
                //response.Start();
                //response.EnsureSuccessStatusCode();
            }
        }
    }
}
