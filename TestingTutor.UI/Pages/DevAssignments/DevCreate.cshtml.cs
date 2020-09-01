using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Constants;
using TestingTutor.UI.Pages.Assignments;
using TestingTutor.UI.Services;

namespace TestingTutor.UI.Pages.DevAssignments
{
    public class DevCreateModel : AssignmentPageModel
    {

        public DbSet<CourseClass> CourseClasses;
        public IEngineService EngineService;
        public DevCreateModel(ApplicationDbContext context)
           : base(context)
        {
            CourseClasses = context.Set<CourseClass>();
            //EngineService = engineService;
        }

        [BindProperty] public PreAssignment PreAssignment { get; set; } = new PreAssignment();

        [BindProperty, Required, DisplayName("Solution Files"),
         Dev.Ui.Annotations.FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile SolutionFiles { get; set; }

        [BindProperty, Required, DisplayName("Test Project Files"),
         Dev.Ui.Annotations.FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile TestProjectFiles { get; set; }

        public MethodDeclaration MethodDeclaration { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //var course = await CourseClasses.FindAsync(2);

            //if (course == null)
            //    return NotFound();
            ViewData["Types"] = new SelectList(MethodDeclarationConstants.AstTypes);
            return Page();
            
        }



        //public IActionResult OnGet()
        //{
        //    return Page();
        //}
    }
}