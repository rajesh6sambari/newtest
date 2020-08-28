using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Assignments
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public abstract class AssignmentPageModel : PageModel
    {
        protected readonly TestingTutor.Dev.Data.DataAccess.ApplicationDbContext Context;

        protected AssignmentPageModel(ApplicationDbContext context)
        {
            Context = context;
        }

        [BindProperty] public Assignment Assignment { get; set; }
        [BindProperty] public IList<ApplicationModeViewModel> ApplicationModes { get; set; }
        [BindProperty] public IList<CoverageTypeOptionViewModel> CoverageTypeOptions { get; set; }
        [BindProperty] public IList<int> Tags { get; set; }
        [BindProperty] public IList<string> AddedTags { get; set; }

        

        public async Task<IList<ApplicationModeViewModel>> GetApplicationModes(int? id)
        {
            var assignment = await Context.GetAssignmentById(id);

            var applicationModeViewModels = new List<ApplicationModeViewModel>();

            if (assignment == null)
            {
                (await Context.GetApplicationModesAsync()).ToList()
                    .ForEach(mode => applicationModeViewModels.Add(new ApplicationModeViewModel { Id = mode.Id, Name = mode.Name}));

                return applicationModeViewModels;
            }

            assignment.AssignmentApplicationModes.ToList()
                .ForEach(mode => applicationModeViewModels.Add(
                    new ApplicationModeViewModel{Name = mode.ApplicationMode.Name, IsChecked = mode.IsChecked}));

            (await Context.GetApplicationModesAsync()).ToList()
                .ForEach(mode =>
                {
                    if (!applicationModeViewModels.Exists(x => mode.Name.Equals(x.Name)))
                    {
                        applicationModeViewModels.Add(new ApplicationModeViewModel { Name = mode.Name});
                    }
                });

            return applicationModeViewModels;
        }
        public async Task<IList<CoverageTypeOptionViewModel>> GetCoverageTypeOptions(int? id)
        {
            var assignment = await Context.GetAssignmentById(id);

            var coverageTypeOptionViewModels = new List<CoverageTypeOptionViewModel>();

            if (assignment == null)
            {
                (await Context.GetCoverageTypeOptionsAsync()).ToList()
                    .ForEach(mode => coverageTypeOptionViewModels.Add(new CoverageTypeOptionViewModel { Id = mode.Id, Name = mode.Name }));

                return coverageTypeOptionViewModels;
            }

            assignment.AssignmentCoverageTypeOptions.ToList()
                .ForEach(mode => coverageTypeOptionViewModels.Add(
                    new CoverageTypeOptionViewModel { Name = mode.CoverageTypeOption.Name, IsChecked = mode.IsChecked }));

            (await Context.GetCoverageTypeOptionsAsync()).ToList()
                .ForEach(mode =>
                {
                    if (!coverageTypeOptionViewModels.Exists(x => mode.Name.Equals(x.Name)))
                    {
                        coverageTypeOptionViewModels.Add(new CoverageTypeOptionViewModel { Name = mode.Name });
                    }
                });

            return coverageTypeOptionViewModels;
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public class FileExtensionsAttribute : ValidationAttribute
        {
            private List<string> AllowedExtensions { get; set; }

            public FileExtensionsAttribute(string fileExtensions)
            {
                AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            public override bool IsValid(object value)
            {
                if (value is IFormFile file)
                {
                    var fileName = file.FileName;

                    return AllowedExtensions.Any(y => fileName.EndsWith(y));
                }

                return true;
            }
        }
    }
}
