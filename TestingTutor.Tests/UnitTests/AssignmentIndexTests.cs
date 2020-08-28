using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestingTutor.UI.Data;
using TestingTutor.UI.Data.Models;
using TestingTutor.UI.Pages.Assignments;
using Xunit;

namespace TestingTutor.Tests.UnitTests
{
    public class AssignmentIndexTests
    {
        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel_WithAListOfAssignments()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");

            var mockAppDbContext = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var expectedAssignments = ApplicationDbContext.GetSeedingAssignments();
            mockAppDbContext.Setup(
                db => db.GetAssignmentsAsync()).Returns(Task.FromResult(expectedAssignments));
            var pageModel = new IndexModel(mockAppDbContext.Object);

            // Act
            await pageModel.OnGetAsync();
            
            // Assert
            var actualAssignments = Assert.IsAssignableFrom<List<Assignment>>(pageModel.Assignment);
            Assert.Equal(
                expectedAssignments.Select(m => m.Id),
                actualAssignments.Select(m => m.Id));
        }

        [Fact]
        public async Task OnPostDownload_ShouldDownload_ZeroByteFile()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");

            var mockAppDbContext = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var expectedAssignments = ApplicationDbContext.GetSeedingAssignments();
            var expectedAssignment = expectedAssignments.First(x => x.Id == 1);
            mockAppDbContext.Setup(
                db => db.GetAssignmentsAsync()).Returns(Task.FromResult(expectedAssignments));
            mockAppDbContext.Setup(
                db => db.GetAssignmentById(It.IsAny<int>())).Returns(Task.FromResult(expectedAssignment));
            var pageModel = new IndexModel(mockAppDbContext.Object);

            // Act
            var fileStreamResult = await pageModel.OnPostDownload(1, "AssignmentSpecification");

            // Assert
            Assert.Equal(0, fileStreamResult.FileStream.Length);
        }
    }
}
