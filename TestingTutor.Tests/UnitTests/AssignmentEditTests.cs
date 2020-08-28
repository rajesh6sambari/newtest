using System.Linq;
using System.Threading.Tasks;
using TestingTutor.UI.Data;
using TestingTutor.UI.Data.Models;
using TestingTutor.UI.Pages.Assignments;
using Xunit;

namespace TestingTutor.Tests.UnitTests
{
    public class AssignmentEditTests : AssignmentTestsBase
    {
        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel_WithAnAssignment()
        {
            // Arrange
            var pageModel = new EditModel(MockAppDbContext.Object);

            // Act
            await pageModel.OnGetAsync(1);

            // Assert
            var actualAssignment = Assert.IsAssignableFrom<Assignment>(pageModel.Assignment);
            Assert.Equal(ApplicationDbContext.GetSeedingAssignments().First(a => a.Id == 1).Name, actualAssignment.Name);
        }

        [Fact]
        public async Task OnPostAsync_EditAnAssignment_ShouldSaveNewAssignmentName()
        {
            // Arrange
            var pageModel = new EditModel(MockAppDbContext.Object)
            {
                Assignment = ApplicationDbContext.GetSeedingAssignments().First(x => x.Id == 1)
            };
            pageModel.Assignment.Name = "New Assignment Name";
            pageModel.ApplicationModes = await pageModel.GetApplicationModes(1);
            pageModel.CoverageTypeOptions = await pageModel.GetCoverageTypeOptions(1);

            // Act
            await pageModel.OnPostAsync(1);

            // Assert
            Assert.Equal(pageModel.Assignment.Name, (await MockAppDbContext.Object.GetAssignmentById(1)).Name);
        }

        [Fact]
        public async Task OnPostAsync_EditAnAssignment_ShouldUpdateApplicationModeToAllNotChecked()
        {
            // Arrange
            var pageModel = new EditModel(MockAppDbContext.Object)
            {
                Assignment = ApplicationDbContext.GetSeedingAssignments().First(x => x.Id == 1),
            };

            pageModel.ApplicationModes = await pageModel.GetApplicationModes(1);
            pageModel.CoverageTypeOptions = await pageModel.GetCoverageTypeOptions(1);

            pageModel.ApplicationModes.ToList().ForEach(mode => mode.IsChecked = false);

            // Act 
            await pageModel.OnPostAsync(1);

            // Assert
            var updatedAssignment = await MockAppDbContext.Object.GetAssignmentById(1);
            updatedAssignment.AssignmentApplicationModes.ToList().ForEach(mode => Assert.False(mode.IsChecked));
        }

        [Fact]
        public async Task OnPostAsync_EditAnAssignment_ShouldUpdateCoverageTypesToAllNotChecked()
        {
            // Arrange
            var pageModel = new EditModel(MockAppDbContext.Object)
            {
                Assignment = ApplicationDbContext.GetSeedingAssignments().First(x => x.Id == 1),
            };

            pageModel.ApplicationModes = await pageModel.GetApplicationModes(1);
            pageModel.CoverageTypeOptions = await pageModel.GetCoverageTypeOptions(1);

            pageModel.CoverageTypeOptions.ToList().ForEach(type => type.IsChecked = false);

            // Act 
            await pageModel.OnPostAsync(1);

            // Assert
            var updatedAssignment = await MockAppDbContext.Object.GetAssignmentById(1);
            updatedAssignment.AssignmentCoverageTypeOptions.ToList().ForEach(type => Assert.False(type.IsChecked));
        }

        [Fact]
        public async Task OnPostAsync_EditAnAssignment_ShouldUpdateCoverageTypesAndApplicationModesToAllNotChecked()
        {
            // Arrange
            var pageModel = new EditModel(MockAppDbContext.Object)
            {
                Assignment = ApplicationDbContext.GetSeedingAssignments().First(x => x.Id == 1),
            };

            pageModel.ApplicationModes = await pageModel.GetApplicationModes(1);
            pageModel.CoverageTypeOptions = await pageModel.GetCoverageTypeOptions(1);

            pageModel.ApplicationModes.ToList().ForEach(mode => mode.IsChecked = false);
            pageModel.CoverageTypeOptions.ToList().ForEach(type => type.IsChecked = false);

            // Act 
            await pageModel.OnPostAsync(1);

            // Assert
            var updatedAssignment = await MockAppDbContext.Object.GetAssignmentById(1);
            updatedAssignment.AssignmentApplicationModes.ToList().ForEach(mode => Assert.False(mode.IsChecked));
            updatedAssignment.AssignmentCoverageTypeOptions.ToList().ForEach(type => Assert.False(type.IsChecked));
        }
    }
}
