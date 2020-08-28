using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Pages.Assignments;
using TestingTutor.UI.Utilities;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Portal
{
    public class TemplateModel : AssignmentPageModel
    {
        [BindProperty, DisplayName("Assignment Specification"), FileExtensions(fileExtensions: "pdf", ErrorMessage = "The file must be a PDF.")]
        public IFormFile AssignmentSpecificationUpload { get; set; }

        [BindProperty, DisplayName("Assignment Reference Solution"), FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile ReferenceSolutionUpload { get; set; }

        [BindProperty, DisplayName("Assignment Reference Test Cases Solution"), FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile ReferenceTestCasesSolutionsUpload { get; set; }

        public TemplateModel(ApplicationDbContext context) : base(context)
        {
        }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Assignment = await Context.GetAssignmentById(id);

            if (Assignment == null)
            {
                return NotFound();
            }


            var institutionalId = Context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;

            if (Assignment.InstitutionId != institutionalId)
            {
                Assignment.InstitutionId = institutionalId;
                Assignment.CourseId = 0;
            }

            if (ViewData != null)
            {
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

            Tags = Assignment.Tags.Select(t => t.TagId).ToList();
            ApplicationModes = await GetApplicationModes(null);
            CoverageTypeOptions = await GetCoverageTypeOptions(null);

            foreach (var mode in ApplicationModes)
            {
                var assignment = Assignment.AssignmentApplicationModes.FirstOrDefault(a => a.ApplicationModeId.Equals(mode.Id));
                if (assignment != null)
                {
                    mode.IsChecked = assignment.IsChecked;
                }
            }

            foreach (var option in CoverageTypeOptions)
            {
                var assignment =
                    Assignment.AssignmentCoverageTypeOptions.FirstOrDefault(a =>
                        a.CoverageTypeOptionId.Equals(option.Id));
                if (assignment != null)
                {
                    option.IsChecked = assignment.IsChecked;
                }
            }

            Assignment.Id = 0;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
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

            var assignmentTemplate = await Context.GetAssignmentById(id);

            try
            {
                if (AssignmentSpecificationUpload != null)
                {
                    var file = FileHelpers.ProcessFormFile(AssignmentSpecificationUpload, ModelState);
                    var assignmentSpecification = new AssignmentSpecification
                    {
                        FileName = file.FileName,
                        FileBytes = file.FileBytes
                    };

                    Context.AssignmentSpecifications.Add(assignmentSpecification);
                    await Context.SaveChangesAsync();

                    Assignment.AssignmentSpecificationId = 0;
                    Assignment.AssignmentSpecification = assignmentSpecification;
                    await Context.SaveChangesAsync();
                }
                else
                {
                    var assignmentSpecification = new AssignmentSpecification
                    {
                        FileName = assignmentTemplate.AssignmentSpecification.FileName,
                        FileBytes = assignmentTemplate.AssignmentSpecification.FileBytes
                    };
                    Context.AssignmentSpecifications.Add(assignmentSpecification);
                    await Context.SaveChangesAsync();

                    Assignment.AssignmentSpecificationId = 0;
                    Assignment.AssignmentSpecification = assignmentSpecification;
                    await Context.SaveChangesAsync();
                }

                if (ReferenceSolutionUpload != null)
                {
                    var file = FileHelpers.ProcessFormFile(ReferenceSolutionUpload, ModelState);
                    var referenceSolution = new ReferenceSolution
                    {
                        FileName = file.FileName,
                        FileBytes = file.FileBytes
                    };

                    Context.ReferenceSolutions.Add(referenceSolution);
                    await Context.SaveChangesAsync();

                    Assignment.ReferenceSolutionId = 0;
                    Assignment.ReferenceSolution = referenceSolution;
                    await Context.SaveChangesAsync();
                }
                else
                {
                    var referenceSolution = new ReferenceSolution
                    {
                        FileName = assignmentTemplate.ReferenceSolution.FileName,
                        FileBytes = assignmentTemplate.ReferenceSolution.FileBytes
                    };

                    Context.ReferenceSolutions.Add(referenceSolution);
                    await Context.SaveChangesAsync();

                    Assignment.ReferenceSolutionId = 0;
                    Assignment.ReferenceSolution = referenceSolution;
                    await Context.SaveChangesAsync();
                }

                if (ReferenceTestCasesSolutionsUpload != null)
                {
                    var file = FileHelpers.ProcessFormFile(ReferenceTestCasesSolutionsUpload, ModelState);
                    var referenceTestCasesSolutions = new ReferenceTestCasesSolutions
                    {
                        FileName = file.FileName,
                        FileBytes = file.FileBytes

                    };

                    Context.ReferenceTestCasesSolutions.Add(referenceTestCasesSolutions);
                    await Context.SaveChangesAsync();

                    Assignment.ReferenceTestCasesSolutionsId = 0;
                    Assignment.ReferenceTestCasesSolutions = referenceTestCasesSolutions;
                    await Context.SaveChangesAsync();
                }
                else
                {
                    var referenceTestCasesSolutions = new ReferenceTestCasesSolutions
                    {
                        FileName = assignmentTemplate.ReferenceTestCasesSolutions.FileName,
                        FileBytes = assignmentTemplate.ReferenceTestCasesSolutions.FileBytes
                    };

                    Context.ReferenceTestCasesSolutions.Add(referenceTestCasesSolutions);
                    await Context.SaveChangesAsync();

                    Assignment.ReferenceTestCasesSolutionsId = 0;
                    Assignment.ReferenceTestCasesSolutions = referenceTestCasesSolutions;
                    await Context.SaveChangesAsync();
                }

                foreach (var mode in ApplicationModes)
                {
                    Assignment.AssignmentApplicationModes.Add(new AssignmentApplicationMode
                    {
                        IsChecked = mode.IsChecked,
                        ApplicationModeId = mode.Id,
                    });
                }

                foreach (var option in CoverageTypeOptions)
                {
                    Assignment.AssignmentCoverageTypeOptions.Add(new AssignmentCoverageTypeOption
                    {
                        IsChecked = option.IsChecked,
                        CoverageTypeOptionId = option.Id,
                    });
                }

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


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(Assignment.Id))
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

        private bool AssignmentExists(int id)
        {
            return Context.Assignments.Any(e => e.Id == id);
        }
    }
}