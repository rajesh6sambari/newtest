using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestingTutor.CSharpEngine.Engine;
using TestingTutor.EngineModels;


namespace TestingTutor.CSharpEngine.Controllers
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
            if (value != null)
            {
                var task = Engine.Run(value);
                if (!task.IsCompleted) task.Start();
            }
        }
    }
}
