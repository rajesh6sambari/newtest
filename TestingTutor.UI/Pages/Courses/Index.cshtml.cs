using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Courses
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get;set; }

        public async Task OnGetAsync()
        {
            var institutionId = _context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
            Course = await _context.Courses.Where(c => c.InstitutionId.Equals(institutionId))
                .Include(c => c.Term)
                .Include(c => c.Institution)
                .ToListAsync();
        }
    }
}
