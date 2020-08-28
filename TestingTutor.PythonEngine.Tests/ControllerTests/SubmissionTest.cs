using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Controllers;
using TestingTutor.PythonEngine.Engine;

namespace TestingTutor.PythonEngine.Tests.ControllerTests
{
    [TestClass]
    public class SubmissionTest
    {
        protected SubmissionController Controller;

        private class MockEngine : IEngine
        {
            public static int Count = 0;

            public Task<FeedbackDto> Run(SubmissionDto submissionDto)
            {
                Count++;
                Task.Delay(1000);
                var feedbackDto = new FeedbackDto();
                return Task.FromResult(feedbackDto);
            }
        }

        [TestInitialize]
        public void Init()
        {
            Controller = new SubmissionController(new MockEngine());
        }

        [Timeout(100)]
        [TestMethod]
        public void MultipleTaskRun()
        {
            // Arrange
            MockEngine.Count = 0;

            // Act
            Controller.Post(new SubmissionDto());
            Controller.Post(new SubmissionDto());

            // Assert
            Assert.AreEqual(2, MockEngine.Count);
        }
    }
}
