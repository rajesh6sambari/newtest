using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;

namespace TestingTutor.UI.Pages.Portal
{
    [Authorize]
    public class MyCourses : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyCourses(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<CourseViewModel> StudentCourses { get;set; } = new List<CourseViewModel>();
        public IList<CourseViewModel> InstructorCourses { get; set; } = new List<CourseViewModel>();

        public async Task OnGetAsync()
        {
            var user = _userManager.Users.AsNoTracking().Single(x => x.Email.Equals(User.Identity.Name));

            var publishedStudentCourses = await _context.StudentCourses.AsNoTracking()
                .Include(course => course.Student)
                .Include(course => course.Course)
                .ThenInclude(course => course.Term)
                .Where(course => course.StudentId.Equals(user.Id))
                .Join(_context.Courses.AsNoTracking(),
                    studentCourse => studentCourse.CourseId,
                    course => course.Id,
                    (studentCourse, course) => new
                    {
                        StudentCourse = studentCourse,
                        Course = course
                    })
                .Where(x => x.Course.IsPublished == true)
                .Select(studentCourseCourse => new
                {
                    studentCourseCourse.Course.Id,
                    studentCourseCourse.Course.CourseName,
                    studentCourseCourse.Course.Term
                })
                .ToListAsync();

            publishedStudentCourses.ForEach(studentCourse =>
            {
                StudentCourses.Add(new CourseViewModel
                {
                    CourseId = studentCourse.Id,
                    CourseName = studentCourse.CourseName,
                    TermName = studentCourse.Term.Name
                });
            });

            var instructorCourses = await _context.InstructorCourses.AsNoTracking()
                .Include(course => course.Instructor)
                .Include(course => course.Course)
                .ThenInclude(course => course.Term)
                .Where(course => course.InstructorId.Equals(user.Id))
                .Join(_context.Courses.AsNoTracking(),
                    instructorCourse => instructorCourse.CourseId,
                    course => course.Id,
                    (instructorCourse, course) => new
                    {
                        StudentCourse = instructorCourse,
                        Course = course
                    })
                .Select(instructorCourseCourse => new
                {
                    instructorCourseCourse.Course.Id,
                    instructorCourseCourse.Course.CourseName,
                    instructorCourseCourse.Course.Term,
                    instructorCourseCourse.Course.IsPublished
                })
                .ToListAsync();

            instructorCourses.ForEach(instructorCourse =>
            {
                InstructorCourses.Add(new CourseViewModel
                {
                    CourseId = instructorCourse.Id,
                    CourseName = instructorCourse.CourseName,
                    TermName = instructorCourse.Term.Name,
                    Status = instructorCourse.IsPublished ? "Published" : "Not Published"
                });
            });
        }
    }
}
