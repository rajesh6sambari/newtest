using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.UI.Data;
using TestingTutor.UI.Data.Models;
using Xunit;

namespace TestingTutor.Tests.UnitTests
{
    public class DataAccessLayerTests
    {
        [Fact]
        public async Task GetCoursesAsync_CoursesAreReturned()
        {
            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptions()))
            {
                var expectedCourses = ApplicationDbContext.GetSeedingCourses();
                await db.AddRangeAsync(expectedCourses);
                await db.SaveChangesAsync();

                var result = await db.GetCoursesAsync();

                var actualCourses = Assert.IsAssignableFrom<List<Course>>(result);
                Assert.Equal(expectedCourses.Select(x => x.CourseName), actualCourses.Select(x => x.CourseName));
            }
        }
    }
}
