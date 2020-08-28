using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;

namespace TestingTutor.UI.Pages.Courses
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty] public Course Course { get; set; }

        [BindProperty] public List<UserViewModel> Instructors { get; set; }

        [BindProperty] public List<UserViewModel> Students { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Courses
                .Include(c => c.Term)
                .Include(c => c.Institution)
                .Include(c => c.Students)
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Course == null)
            {
                return NotFound();
            }

            Instructors = GetInstitutionalUsersInRole("Instructor").ToList();
            Students = GetInstitutionalUsersInRole("Student").ToList();

            ViewData["TermId"] = new SelectList(_context.Terms, "Id", "Name");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Course).State = EntityState.Modified;
            var institutionalId = _context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
            Course.InstitutionId = institutionalId;

            try
            {
                foreach (var student in Students)
                {
                    if (student.IsChecked && !IsUserAssignedToCourse(Course.Id, student.UserId, "Student"))
                    {
                        _context.StudentCourses.Add(new StudentCourse {CourseId = Course.Id, StudentId = student.UserId});
                    }
                    else if (!student.IsChecked && IsUserAssignedToCourse(Course.Id, student.UserId, "Student"))
                    {
                        var studentCourse = _context.StudentCourses.Single(course =>
                            course.StudentId.Equals(student.UserId) && course.CourseId.Equals(Course.Id));
                        _context.StudentCourses.Remove(studentCourse);
                    }
                }

                foreach (var instructor in Instructors)
                {
                    if (instructor.IsChecked && !IsUserAssignedToCourse(Course.Id, instructor.UserId, "Instructor"))
                    {
                        _context.InstructorCourses.Add(new InstructorCourse { CourseId = Course.Id, InstructorId = instructor.UserId});
                    }
                    else if (!instructor.IsChecked && IsUserAssignedToCourse(Course.Id, instructor.UserId, "Instructor"))
                    {
                        var instructorCourse = _context.InstructorCourses.Single(course =>
                            course.InstructorId.Equals(instructor.UserId) && course.CourseId.Equals(Course.Id));
                        _context.InstructorCourses.Remove(instructorCourse);
                    }

                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(Course.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }

        private IEnumerable<UserViewModel> GetInstitutionalUsersInRole(string userRole)
        { 
            var users = new List<UserViewModel>();

            var usersInInstitution = _context.Users
                .Where(user => user.InstitutionId.Equals(Course.InstitutionId)).AsNoTracking().ToList();

            foreach (var user in usersInInstitution)
            {
                var usersClaims =
                    _context.UserClaims.Where(userClaim => userClaim.UserId.Equals(user.Id)).AsNoTracking().ToList();

                if (usersClaims.Any(claim =>
                    claim.ClaimType.Equals(ClaimTypes.Role) && claim.ClaimValue.Equals(userRole)))
                {
                    users.Add(new UserViewModel
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        IsChecked = IsUserAssignedToCourse(Course.Id, user.Id, userRole)
                    });
                }
            }

            return users;
        }

        private bool IsUserAssignedToCourse(int courseId, string userName, string userRole)
        {
            var isUserAssignedToCourse = false;
            switch (userRole)
            {
                case "Student":
                    isUserAssignedToCourse = _context.StudentCourses.AsNoTracking().Any(course =>
                        course.CourseId.Equals(courseId) && course.StudentId.Equals(userName));
                    break;
                case "Instructor":
                    isUserAssignedToCourse = _context.InstructorCourses.AsNoTracking().Any(course =>
                        course.CourseId.Equals(courseId) && course.InstructorId.Equals(userName));
                    break;
            }

            return isUserAssignedToCourse;
        }
    }
}
