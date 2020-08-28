using System.Net.Http;
using TestingTutor.EngineModels;

namespace TestingTutor.PythonEngine.Engine.Brain.Feedback
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
