using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Utilities;

namespace TestingTutor.UI.Pages.Assignments
{
    public class EditModel : AssignmentPageModel
    {
        [BindProperty, DisplayName("Assignment Specification"), FileExtensions(fileExtensions: "pdf", ErrorMessage = "The file must be a PDF.")]
        public IFormFile AssignmentSpecificationUpload { get; set; }

        [BindProperty, DisplayName("Assignment Reference Solution"), FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile ReferenceSolutionUpload { get; set; }

        [BindProperty, DisplayName("Assignment Reference Test Cases Solution"), FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile ReferenceTestCasesSolutionsUpload { get; set; }
        public EditModel(ApplicationDbContext context)
            : base(context)
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

            if (ViewData != null)
            {
                var institutionalId = Context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
                var courses = Context.Courses.Where(c => c.InstitutionId.Equals(institutionalId)).ToList();
                courses = courses.Prepend(new Course()
                {
                    Id = 0,
                    CourseName = "Portal Only"
                }).ToList();
                ViewData["CourseId"] = new SelectList(courses, "Id", "CourseName");
                ViewData["LanguageId"] = new SelectList(Context.Languages, "Id", "Name");
                ViewData["FeedbackLevelOptionId"] = new SelectList(Context.FeedbackLevelOptions, "Id", "Name");
                ViewData["CoverageTypeLevelOptionId"] = new SelectList(Context.CoverageTypeOptions, "Id", "Name");
                ViewData["TestingTypeOptionsId"] = new SelectList(Context.TestingTypeOptions, "Id", "Name");
                ViewData["AssignmentVisibilityProtectionLevelId"] = new SelectList(Context.AssignmentVisibilityProtectionLevels, "Id", "Name");
                ViewData["DifficultiesId"] = new SelectList(Context.Difficulties, "Id", "Value");
                ViewData["TagsId"] = new SelectList(Context.Tags, "Id", "Name");
            }

            Tags = Assignment.Tags.Select(t => t.TagId).ToList();
            ApplicationModes = await GetApplicationModes(Assignment.Id);
            CoverageTypeOptions = await GetCoverageTypeOptions(Assignment.Id);
 
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                var institutionalId = Context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
                var courses = Context.Courses.Where(c => c.InstitutionId.Equals(institutionalId)).ToList();
                courses = courses.Prepend(new Course()
                {
                    Id = 0,
                    CourseName = "Portal Only"
                }).ToList();
                ViewData["CourseId"] = new SelectList(courses, "Id", "CourseName");
                ViewData["LanguageId"] = new SelectList(Context.Languages, "Id", "Name");
                ViewData["FeedbackLevelOptionId"] = new SelectList(Context.FeedbackLevelOptions, "Id", "Name");
                ViewData["CoverageTypeLevelOptionId"] = new SelectList(Context.CoverageTypeOptions, "Id", "Name");
                ViewData["TestingTypeOptionsId"] = new SelectList(Context.TestingTypeOptions, "Id", "Name");
                ViewData["AssignmentVisibilityProtectionLevelId"] = new SelectList(Context.AssignmentVisibilityProtectionLevels, "Id", "Name");
                ViewData["DifficultiesId"] = new SelectList(Context.Difficulties, "Id", "Value");
                ViewData["TagsId"] = new SelectList(Context.Tags, "Id", "Name");
                return Page();
            }

            var assignmentToUpdate = await Context.GetAssignmentById(id);

            try
            {
                if (Assignment.CourseId == 0)
                {
                    Assignment.CourseId = null;
                }

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

                    assignmentToUpdate.AssignmentSpecificationId = 0;
                    assignmentToUpdate.AssignmentSpecification = assignmentSpecification;
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

                    assignmentToUpdate.ReferenceSolutionId = 0;
                    assignmentToUpdate.ReferenceSolution = referenceSolution;
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

                    assignmentToUpdate.ReferenceTestCasesSolutionsId = 0;
                    assignmentToUpdate.ReferenceTestCasesSolutions = referenceTestCasesSolutions;
                    await Context.SaveChangesAsync();
                }

                assignmentToUpdate.Name = Assignment.Name;
                assignmentToUpdate.TestCoverageLevel = Assignment.TestCoverageLevel;
                assignmentToUpdate.RedundantTestLevel = Assignment.RedundantTestLevel;
                assignmentToUpdate.LanguageId = Assignment.LanguageId;
                assignmentToUpdate.CourseId = Assignment.CourseId;
                assignmentToUpdate.FeedbackLevelOptionId = Assignment.FeedbackLevelOptionId;
                assignmentToUpdate.TestingTypeOptionId = Assignment.TestingTypeOptionId;
                assignmentToUpdate.AssignmentVisibilityProtectionLevelId =
                    Assignment.AssignmentVisibilityProtectionLevelId;

                ApplicationModes.ToList().ForEach(m =>
                {
                    if (assignmentToUpdate.AssignmentApplicationModes.ToList().Exists(x => x.ApplicationMode.Name.Equals(m.Name)))
                    {
                        assignmentToUpdate.AssignmentApplicationModes.First(x => x.ApplicationMode.Name.Equals(m.Name)).IsChecked =
                            m.IsChecked ? true : false;
                    }
                });

                CoverageTypeOptions.ToList().ForEach(o =>
                {
                    if (assignmentToUpdate.AssignmentCoverageTypeOptions.ToList().Exists(x => x.CoverageTypeOption.Name.Equals(o.Name)))
                    {
                        assignmentToUpdate.AssignmentCoverageTypeOptions.First(a => a.CoverageTypeOption.Name.Equals(o.Name)).IsChecked = o.IsChecked ? true : false;
                    }
                });
                

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

                assignmentToUpdate.Tags = new List<AssignmentTag>();
                foreach (var tag in Tags)
                {
                    if (tag < 0)
                    {
                        assignmentToUpdate.Tags.Add(new AssignmentTag()
                        {
                            Tag = Context.Tags.First(t => t.Name.Equals(AddedTags[tag * -1 - 1]))
                        });
                    }
                    else if (assignmentToUpdate.Tags.FirstOrDefault(t => t.TagId.Equals(tag)) == null)
                    {
                        assignmentToUpdate.Tags.Add(new AssignmentTag()
                        {
                            Tag = Context.Tags.First(t => t.Id.Equals(tag))
                        });
                    }
                }

                Context.Update(assignmentToUpdate);
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
