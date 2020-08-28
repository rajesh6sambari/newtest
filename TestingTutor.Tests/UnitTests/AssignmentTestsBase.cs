using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestingTutor.UI.Data;
using TestingTutor.UI.Data.Models;

namespace TestingTutor.Tests.UnitTests
{
    public class AssignmentTestsBase
    {
        protected readonly Mock<ApplicationDbContext> MockAppDbContext;
        protected readonly IFormFile UploadReferenceAssignment;
        protected readonly IFormFile UploadReferenceTestCases;
        protected readonly IFormFile UploadReferenceTestCasesSolutions;
        protected readonly IList<Assignment> Assignments;

        public AssignmentTestsBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            MockAppDbContext = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            Assignments = ApplicationDbContext.GetSeedingAssignments();
            var expectedAssignment = Assignments.First(x => x.Id == 1);

            MockAppDbContext.Setup(
                db => db.GetAssignmentsAsync()).Returns(Task.FromResult(Assignments));
            MockAppDbContext.Setup(
                db => db.GetAssignmentById(It.IsAny<int>())).Returns(Task.FromResult(expectedAssignment));
            MockAppDbContext.Setup(
                db => db.GetCoursesAsync()).Returns(Task.FromResult(ApplicationDbContext.GetSeedingCourses()));
            MockAppDbContext.Setup(
                db => db.GetApplicationModesAsync()).Returns(Task.FromResult(ApplicationDbContext.GetSeedingApplicationModes()));
            MockAppDbContext.Setup(
                db => db.GetCoverageTypeOptionsAsync()).Returns(Task.FromResult(ApplicationDbContext.GetSeedingCoverageTypeOptions()));
            MockAppDbContext.Setup(
                    db => db.AddAssignment(It.IsAny<Assignment>()))
                .Returns((Assignment assignment) =>
                {
                    Assignments.Add(assignment);
                    return Task.FromResult(true);
                });

            //MockAppDbContext.Setup(
            //        db => db.Users.FirstAsync())
            //    .Returns(Task.FromResult(new ApplicationUser {UserName = "lucas.cordova@oit.edu"}));

            //MockAppDbContext.Setup(
            //    db => db.Institutions.Single())
            //    .Returns(
            //        new Institution
            //        {
            //            ApplicationUsers = new List<ApplicationUser>() {new ApplicationUser() {UserName = "Lucas.Cordova@oit.edu"}}
            //        });

            UploadReferenceAssignment = new FormFile(new MemoryStream(Enumerable.Repeat((byte)0x20, 2000).ToArray()), 0, 2000, "File", "File.pdf");
            UploadReferenceTestCases = new FormFile(new MemoryStream(Enumerable.Repeat((byte)0x20, 2000).ToArray()), 0, 2000, "File", "File.zip");
            UploadReferenceTestCasesSolutions = new FormFile(new MemoryStream(Enumerable.Repeat((byte)0x20, 2000).ToArray()), 0, 2000, "File", "File.zip");
        }


    }
}
