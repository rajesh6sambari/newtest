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
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Pages.Assignments;
using TestingTutor.UI.Utilities;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Portal
{
    public class CreateModel : AssignmentPageModel
    {
        [BindProperty, Required, DisplayName("Assignment Specification"), FileExtensions(fileExtensions: "pdf", ErrorMessage = "The file must be a PDF.")]
        public IFormFile AssignmentSpecificationUpload { get; set; }

        [BindProperty, Required, DisplayName("Assignment Reference Solution"), FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile ReferenceSolutionUpload { get; set; }

        [BindProperty, Required, DisplayName("Assignment Reference Test Cases Solution"), FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile ReferenceTestCasesSolutionsUpload { get; set; }

        public CreateModel(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (ViewData != null)
            {
                var institutionalId = Context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
                ViewData["AssignmentSpecificationId"] = new SelectList(Context.AssignmentSpecifications, "Id", "Id");
                var courses = Context.Courses.Where(c => c.InstitutionId.Equals(institutionalId)).ToList();
                courses = courses.Prepend(new Course()
                {
                    Id = 0,
                    CourseName = "Portal Only"
                }).ToList();
                ViewData["CourseId"] = new SelectList(courses, "Id", "CourseName");
                ViewData["LanguageId"] = new SelectList(Context.Languages, "Id", "Name");
                ViewData["TestingTypeOptionsId"] = new SelectList(Context.TestingTypeOptions, "Id", "Name");
                ViewData["FeedbackLevelOptionId"] = new SelectList(Context.FeedbackLevelOptions, "Id", "Name");
                ViewData["AssignmentVisibilityProtectionLevelId"] = new SelectList(Context.AssignmentVisibilityProtectionLevels, "Id", "Name");
                ViewData["DifficultiesId"] = new SelectList(Context.Difficulties, "Id", "Value");
                ViewData["TagsId"] = new SelectList(Context.Tags, "Id", "Name");
            }

            ApplicationModes = await GetApplicationModes(null);
            CoverageTypeOptions = await GetCoverageTypeOptions(null);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var institutionalId = Context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
                ViewData["AssignmentSpecificationId"] = new SelectList(Context.AssignmentSpecifications, "Id", "Id");
                var courses = Context.Courses.Where(c => c.InstitutionId.Equals(institutionalId)).ToList();
                courses = courses.Prepend(new Course()
                {
                    Id = 0,
                    CourseName = "Portal Only"
                }).ToList();
                ViewData["CourseId"] = new SelectList(courses, "Id", "CourseName");
                ViewData["LanguageId"] = new SelectList(Context.Languages, "Id", "Name");
                ViewData["TestingTypeOptionsId"] = new SelectList(Context.TestingTypeOptions, "Id", "Name");
                ViewData["FeedbackLevelOptionId"] = new SelectList(Context.FeedbackLevelOptions, "Id", "Name");
                ViewData["AssignmentVisibilityProtectionLevelId"] = new SelectList(Context.AssignmentVisibilityProtectionLevels, "Id", "Name");
                ViewData["DifficultiesId"] = new SelectList(Context.Difficulties, "Id", "Value");
                ViewData["TagsId"] = new SelectList(Context.Tags, "Id", "Name");

                return Page();
            }

            var file = FileHelpers.ProcessFormFile(AssignmentSpecificationUpload, ModelState);
            var assignmentSpecification = new AssignmentSpecification
            {
                FileName = file.FileName,
                FileBytes = file.FileBytes
            };

            Context.AssignmentSpecifications.Add(assignmentSpecification);
            await Context.SaveChangesAsync();

            file = FileHelpers.ProcessFormFile(ReferenceSolutionUpload, ModelState);
            var referenceSolution = new ReferenceSolution
            {
                FileName = file.FileName,
                FileBytes = file.FileBytes
            };

            Context.ReferenceSolutions.Add(referenceSolution);
            await Context.SaveChangesAsync();


            file = FileHelpers.ProcessFormFile(ReferenceTestCasesSolutionsUpload, ModelState);
            var referenceTestCasesSolutions = new ReferenceTestCasesSolutions
            {
                FileName = file.FileName,
                FileBytes = file.FileBytes

            };
            Context.ReferenceTestCasesSolutions.Add(referenceTestCasesSolutions);
            await Context.SaveChangesAsync();

            Assignment.AssignmentSpecification = assignmentSpecification;
            Assignment.ReferenceSolution = referenceSolution;
            Assignment.ReferenceTestCasesSolutions = referenceTestCasesSolutions;

            foreach (var mode in ApplicationModes)
            {
                Assignment.AssignmentApplicationModes.Add(new AssignmentApplicationMode
                {
                    IsChecked = mode.IsChecked,
                    ApplicationModeId = mode.Id
                });
            };

            foreach (var option in CoverageTypeOptions)
            {
                Assignment.AssignmentCoverageTypeOptions.Add(new AssignmentCoverageTypeOption
                {
                    IsChecked = option.IsChecked,
                    CoverageTypeOptionId = option.Id
                });
            };

            var user = await Context.Users.FirstAsync(u => u.UserName.Equals(HttpContext.User.Identity.Name));
            Assignment.InstitutionId = Context.Institutions
                .Single(i => i.ApplicationUsers.Any(a => a.UserName.Equals(user.UserName))).Id;

            Assignment.Instructors.Add(new InstructorAssignment()
            {
                Instructor = user,
            });

            if (Assignment.CourseId == 0)
            {
                Assignment.CourseId = null;
            }

            await Context.AddAssignment(Assignment);
            await Context.SaveChangesAsync();

            foreach (var tag in AddedTags)
            {
                if (await Context.Tags.FirstOrDefaultAsync(t => t.Name.Equals(tag)) == null)
                {
                    Context.Tags.Add(new Tag()
                    {
                        Name = tag
                    });
                }
            }

            await Context.SaveChangesAsync();


            foreach (var tag in Tags)
            {
                if (tag < 0)
                {
                    Assignment.Tags.Add(new AssignmentTag()
                    {
                        Tag = Context.Tags.First(t => t.Name.Equals(AddedTags[tag * -1 - 1]))
                    });
                }
                else
                {
                    Assignment.Tags.Add(new AssignmentTag()
                    {
                        Tag = Context.Tags.First(t => t.Id.Equals(tag))
                    });
                }
            }

            Context.Update(Assignment);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}