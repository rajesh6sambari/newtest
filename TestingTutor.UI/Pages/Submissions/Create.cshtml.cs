using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestingTutor.EngineModels;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Pages.Assignments;
using TestingTutor.UI.Utilities;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Submissions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty] public int AssignmentId { get; set; }
        [BindProperty] public string AssignmentName { get; set; }
        public Assignment Assignment { get; set; }

        [BindProperty, DisplayName("Assignment Upload"),
         AssignmentPageModel.FileExtensionsAttribute(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile SubmitterSolutionUpload { get; set; }

        [BindProperty, Required, DisplayName("Test Cases Upload"),
         AssignmentPageModel.FileExtensionsAttribute(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile SubmitterTestCaseSolutionUpload { get; set; }

        [BindProperty] public Submission Submission { get; set; }

        public IActionResult OnGet(int? id)
        {
            AssignmentId = id.Value;

            Assignment = _context.Assignments.AsNoTracking()
                .Include(a => a.AssignmentApplicationModes)
                .Include(a => a.ReferenceSolution)
                .Include(a => a.ReferenceTestCasesSolutions)
                .Single(a => a.Id == id);
            AssignmentName = Assignment.Name;

            ViewData["ApplicationModes"] = new SelectList(GetApplicationModesToDisplay(), "Name");

            return Page();
        }

        private IList<string> GetApplicationModesToDisplay()
        {
            var applicationModes = Assignment.AssignmentApplicationModes
                .Join(_context.ApplicationModes,
                    assignmentAppModes => assignmentAppModes.ApplicationModeId,
                    appMode => appMode.Id,
                    (assignmentAppModes, appMode) => new
                    {
                        AssignmentApplicationMode = assignmentAppModes,
                        ApplicationMode = appMode
                    })
                .Select(assignmentAppModeAppMode => new
                {
                    assignmentAppModeAppMode.ApplicationMode,
                    assignmentAppModeAppMode.AssignmentApplicationMode
                }).ToList();

            var applicationModesDisplay = new List<string>();

            applicationModes.ForEach(mode =>
            {
                if (mode.AssignmentApplicationMode.IsChecked) applicationModesDisplay.Add(mode.ApplicationMode.Name);
            });

            return applicationModesDisplay;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Assignment = _context.Assignments.AsNoTracking()
                .Include(a => a.AssignmentApplicationModes)
                .Include(a => a.ReferenceSolution)
                .Include(a => a.ReferenceTestCasesSolutions)
                .Include(a => a.Language)
                .Single(a => a.Id == AssignmentId);

            if (!ModelState.IsValid)
            {
                ViewData["ApplicationModes"] = new SelectList(GetApplicationModesToDisplay(), "Id", "Name");
                return Page();
            }

            Submission.AssignmentId = AssignmentId;
            Submission.SubmitterId = _context.Users.AsNoTracking().Single(u => u.Email.Equals(User.Identity.Name)).Id;
            Submission.SubmissionDateTime = DateTime.Now;

            if (SubmitterSolutionUpload != null)
            {
                var submitterSolutionFile = FileHelpers.ProcessFormFile(SubmitterSolutionUpload, ModelState);
                Submission.SubmitterSolution = submitterSolutionFile.FileBytes;
            }

            var submitterTestCasesFile = FileHelpers.ProcessFormFile(SubmitterTestCaseSolutionUpload, ModelState);
            Submission.SubmitterTestCaseSolution = submitterTestCasesFile.FileBytes;

            _context.Submissions.Add(Submission);
            await _context.SaveChangesAsync();

            var submissionDto = MapSubmissionObjects();

            TransmitToEngine(submissionDto);

            return RedirectToPage("./Index");
        }

        private void TransmitToEngine(SubmissionDto submissionDto)
        {
           switch (Assignment.Language.Name)
            {
                case "Python":
                    //var pythonEngineResponse = client.SubmitEngineJobAsync("http://localhost:7591/api/Submissions", submissionDto);
                    throw new NotImplementedException();
                    //var pythonResult = new EngineSubmitter(new PythonEngine.Engine.PythonEngine(new EngineFactory(Directory.GetCurrentDirectory())),
                    //    HttpContext.RequestServices.GetService<IAppDbContextFactory>()).Submit(submissionDto);
                    break;

                case "Java":
                    //var javaEngineResponse = client.SubmitJavaEngineJobAsync("http://localhost:8080/JavaEngine/submit", submissionDto);
                    BackgroundJob.Enqueue<EngineSubmitter>(sub =>
                        sub.Submit(new JavaEngine.Engine.JavaEngine(), submissionDto, User.Identity.Name));
                    //_engineSubmitter.Submit(new JavaEngine.Engine.JavaEngine(), submissionDto);
                    break;

                case "CSharp":

                    break;
            }
        }

        private SubmissionDto MapSubmissionObjects()
        {
            return new SubmissionDto
            {
                SubmitterId = Submission.SubmitterId,
                SubmissionId = Submission.Id,
                ApplicationMode = Submission.ApplicationMode,
                ReferenceSolution = Assignment.ReferenceSolution.FileBytes,
                ReferenceTestSolution = Assignment.ReferenceTestCasesSolutions.FileBytes,
                AssignmentSolution = Submission.SubmitterSolution,
                TestCaseSolution = Submission.SubmitterTestCaseSolution,
            };
        }

 
    }
}