using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.Portal
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private const int StepSize = 20;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Assignment> Assignments { get; set; }

        [FromRoute] public int Step { get; set; } = 0;

        [BindProperty, DisplayName("Difficulty")]
        public int DifficultyId { get; set; }

        [BindProperty, DisplayName("Language")]
        public int LanguageId { get; set; }

        [BindProperty, DisplayName("Tags")]
        public IList<int> Tags { get; set; }

        [BindProperty]
        public string Name { get; set; }

        public bool IsNext { get; set; }
        public bool IsPrevious { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var assignments = await GetAvaliableAssignments();

            IsNext = Step * StepSize < assignments.Count - StepSize;
            IsPrevious = Step > 0;

            Assignments = assignments.Skip(Step * StepSize).Take(StepSize).ToList();

            SetViewData();



            return Page();
        }

        public async Task<IActionResult> OnPostFilter()
        {
            var assignments = await GetFilterAvaliableAssignments();

            Step = 0;
            IsNext = 0 * StepSize < assignments.Count - StepSize;
            IsPrevious = false;

            Assignments = assignments.Take(StepSize).ToList();

            SetViewData();

            return Page();
        }

        public async Task<IActionResult> OnPostNext()
        {
            var assignments = await GetFilterAvaliableAssignments();

            IsNext = Step * StepSize < assignments.Count - StepSize;
            IsPrevious = Step > 0;

            Assignments = assignments.Skip(Step * StepSize).Take(StepSize).ToList();

            SetViewData();
            return Page();
        }

        public async Task<IActionResult> OnPostPrevious()
        {
            var assignments = await GetFilterAvaliableAssignments();
            IsNext = Step * StepSize < assignments.Count - StepSize;
            IsPrevious = Step > 0;
            Assignments = assignments.Skip(Step * StepSize).Take(StepSize).ToList();

            SetViewData();
            return Page();
        }

        public async Task<IList<Assignment>> GetFilterAvaliableAssignments()
        {
            return (await GetAvaliableAssignments()).Where(FilterAvaliableAssignmentsPredicate).ToList();
        }

        private bool FilterAvaliableAssignmentsPredicate(Assignment arg)
        {
            return DifficultyAssignmentPredicate(arg) &&
                LanguageAssignmentPredicate(arg) &&
                NameAssignmentPredicate(arg) &&
                TagAssignmentPredicate(arg);
        }

        private bool TagAssignmentPredicate(Assignment assignment)
        {
            return !Tags.Any() || Tags.All(tag => assignment.Tags.Any(t => t.TagId.Equals(tag)));
        }

        private bool NameAssignmentPredicate(Assignment assignment)
        {
            return string.IsNullOrWhiteSpace(Name) || assignment.Name.Contains(Name.Trim());
        }

        private bool LanguageAssignmentPredicate(Assignment assignment)
        {
            return LanguageId == 0 || assignment.LanguageId.Equals(LanguageId);
        }

        private bool DifficultyAssignmentPredicate(Assignment assignment)
        {
            return DifficultyId == 0 || assignment.DifficultyId.Equals(DifficultyId);
        }

        public void SetViewData()
        {
            if (ViewData != null)
            {
                var languages = _context.Languages.ToList();
                languages = languages.Prepend(new Language()
                {
                    Id = 0,
                    Name = "All"
                }).ToList();
                ViewData["LanguageId"] = new SelectList(languages, "Id", "Name");
                var difficulties = _context.Difficulties.ToList();
                difficulties = difficulties.Prepend(new Difficulty()
                {
                    Id = 0,
                    Value = "All"
                }).ToList();
                ViewData["DifficultiesId"] = new SelectList(difficulties, "Id", "Value");

                ViewData["TagsId"] = new SelectList(_context.Tags, "Id", "Name");
            }
        }

        [HttpPost, ActionName("Download")]
        public async Task<FileStreamResult> OnPostDownload(int id, string documentType)
        {
            var assignment = await _context.GetAssignmentById(id);
            var fileBytes = await _context.GetDocumentFileBytes(assignment.Id, documentType);
            var mimeType = string.Empty;

            switch (documentType)
            {
                case "AssignmentSpecification":
                    mimeType = "application/pdf";
                    break;
                case "ReferenceSolution":
                    mimeType = "application/zip";
                    break;
                case "ReferenceTestCasesSolutions":
                    mimeType = "application/zip";
                    break;
            }

            return new FileStreamResult(new MemoryStream(fileBytes), mimeType);
        }



        public async Task<IList<Assignment>> GetAvaliableAssignments()
        {
            return (await _context.GetAssignmentsAsync())
                .Where(AvaliableAssignmentsPredicate).ToList();
        }

        public bool AvaliableAssignmentsPredicate(Assignment assignment)
        {
            return 
                RepositoryAssignmentsPredicate(assignment) ||
                OrganizationOnlyAssignmentsPredicate(assignment) ||
                InstructorAssignmentsPredicate(assignment);
        }

        public bool InstructorAssignmentsPredicate(Assignment assignment)
        {
            var user = _context.Users.First(u => u.UserName.Equals(HttpContext.User.Identity.Name));
            return assignment.Instructors.ToList().FirstOrDefault(i => i.Instructor.Id.Equals(user.Id)) != null;
        }

        public bool OrganizationOnlyAssignmentsPredicate(Assignment assignment)
        {
            var user = _context.Users.First(u => u.UserName.Equals(HttpContext.User.Identity.Name));
            return assignment.AssignmentVisibilityProtectionLevel.Name.Equals("Organization-Only") &&
                   assignment.InstitutionId.Equals(user.InstitutionId);
        }

        public bool RepositoryAssignmentsPredicate(Assignment assignment)
        {
            return assignment.AssignmentVisibilityProtectionLevel.Name.Equals("Repository");
        }

    }
}