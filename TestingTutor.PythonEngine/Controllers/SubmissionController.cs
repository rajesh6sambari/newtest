using Microsoft.AspNetCore.Mvc;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine;

namespace TestingTutor.PythonEngine.Controllers
{
    [Route("api/Submission")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        protected IEngine Engine;
        public SubmissionController(IEngine engine)
        {
            Engine = engine;
        }

        // POST: api/Submission
        [HttpPost]
        public void Post([FromBody] SubmissionDto value)
        {
            //if (value != null)
            //{
            //    var task = Engine.Run(value);
            //    if (!task.IsCompleted) task.Start();
            //}
        }
    }
}
