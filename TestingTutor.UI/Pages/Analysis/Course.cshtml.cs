using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Analysis
{
    public class CourseModel : AnalysisPageModel
    {

        public CourseModel(ApplicationDbContext context) : base(context)
        {
        }

        public bool HasData = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Assignment = await Context.GetAssignmentById(id);

            if (Assignment == null) return NotFound();

            var instructors = Assignment.Instructors.ToList().Select(a => a.Instructor.Id).ToList();

            var submissions = GetSubmissions(instructors);
            HasData = submissions.Count > 0;
            if (HasData)
            {
                CourseCharts = GetCharts(submissions);
                CourseCharts = AddClassAverage(CourseCharts);
                Submitters = GetSubmitters(submissions);
            }
            return Page();
        }

        [HttpPost, ActionName("Download")]
        public async Task<FileStreamResult> OnPostDownload(int? id)
        {
            Assignment = await Context.GetAssignmentById(id);

            if (Assignment == null) return null;

            var instructors = Assignment.Instructors.ToList().Select(a => a.Instructor.Id).ToList();

            var submissions = GetSubmissions(instructors);
            HasData = submissions.Count > 0;
            if (HasData)
            {
                CourseCharts = GetCharts(submissions);
                CourseCharts = AddClassAverage(CourseCharts);
            }
            return GetCsv(CourseCharts);
        }


    }
}