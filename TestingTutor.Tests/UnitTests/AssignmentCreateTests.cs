using System.Linq;
using System.Threading.Tasks;
using TestingTutor.UI.Data;
using TestingTutor.UI.Data.Models;
using TestingTutor.UI.Pages.Assignments;
using Xunit;

namespace TestingTutor.Tests.UnitTests
{
    public class AssignmentCreateTests : AssignmentTestsBase
    {
        [Fact]
        public async Task OnPostAsync_PopulatesThePageModel_WithAnAssignment()
        {
            // Arrange
            var pageModel = new CreateModel(MockAppDbContext.Object)
            {
                Assignment = ApplicationDbContext.GetSeedingAssignments().First(a => a.Id == 1),
                AssignmentSpecificationUpload = UploadReferenceAssignment,
                ReferenceSolutionUpload = UploadReferenceTestCases,
                ReferenceTestCasesSolutionsUpload = UploadReferenceTestCasesSolutions
            };

            pageModel.Assignment.Id = 0;
            pageModel.Assignment.Name = "Newly created assignment";

            pageModel.ApplicationModes = await pageModel.GetApplicationModes(null);
            pageModel.CoverageTypeOptions = await pageModel.GetCoverageTypeOptions(null);


            // Act
            await pageModel.OnPostAsync();

            // Assert
            var assignments = await MockAppDbContext.Object.GetAssignmentsAsync();
            var actualAssignment = Assert.IsAssignableFrom<Assignment>(assignments.FirstOrDefault(a => a.Name == "Newly created assignment"));
            Assert.NotNull(actualAssignment);
        }
    }
}
