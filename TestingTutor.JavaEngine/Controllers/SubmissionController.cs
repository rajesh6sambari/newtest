using Microsoft.AspNetCore.Mvc;
using TestingTutor.EngineModels;

namespace TestingTutor.JavaEngine.Controllers
{
    [Route("api/Submission")]
    [ApiController]
    public class SubmissionController
    {
        // POST: api/Submission
        [HttpPost]
        public void Post([FromBody] SubmissionDto value)
        {
            //if (value == null) return;
            //var task = new Engine.JavaEngine().Run(value);
            //if (!task.IsCompleted) task.Start();
        }
    }
}
