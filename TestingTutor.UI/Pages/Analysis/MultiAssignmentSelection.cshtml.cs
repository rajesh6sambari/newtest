using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Internal;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Analysis
{
    public class MultiAssignmentSelectionModel : PageModel
    {
        protected ApplicationDbContext Context;

        public MultiAssignmentSelectionModel(ApplicationDbContext context)
        {
            Context = context;
        }

        public class AssignmentCard
        {
            public int Index { get; set; }
            public Assignment Assignment { get; set; }
        }

        [BindProperty]
        public IList<int> Selected { get; set; } = new List<int>();

        public IList<AssignmentCard> SelectedAssignments { get; set; } = new List<AssignmentCard>();

        public IList<AssignmentCard> NonSelectedAssignments { get; set; } = new List<AssignmentCard>();

        public async Task<IActionResult> OnGetAsync()
        {
            await GetMembers();
            return Page();
        }

        public async Task<IActionResult> OnPostRightAsync(int right)
        {
            if (right >= 0 && right < Selected.Count - 1)
            {
                var temp = Selected[right];
                Selected[right] = Selected[right + 1];
                Selected[right + 1] = temp;
            }

            await GetMembers();
            return Page();
        }

        public async Task<IActionResult> OnPostLeftAsync(int left)
        {
            if (left >= 1 && left < Selected.Count)
            {
                var temp = Selected[left];
                Selected[left] = Selected[left - 1];
                Selected[left - 1] = temp;
            }
            await GetMembers();
            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync(int add)
        {
            Selected.Add(add);
            await GetMembers();
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int remove)
        {
            Selected.Remove(remove);
            await GetMembers();
            return Page();
        }

        public IActionResult OnPostPerform()
        {
            return RedirectToPage("/Analysis/MultiAnalysis", new
            {
                selected = $"{Selected.Select(x => $"{x}").Join(",")}"
            });
        }

        public async Task GetMembers()
        {
            var institutionalId = Context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;

            var assignments = (await Context.GetAssignmentsAsync()).Where(a => a.InstitutionId.Equals(institutionalId)).ToList();
            foreach (var index in Selected)
            {
                if (index >= 0 && index < assignments.Count)
                {
                    SelectedAssignments.Add(
                        new AssignmentCard()
                        {
                            Index = index,
                            Assignment = assignments[index]
                        });
                }
            }

            NonSelectedAssignments = assignments.Select((x, index) =>
                new AssignmentCard()
                {
                    Assignment = x,
                    Index = index
                }).Where(x => !Selected.Contains(x.Index)).ToList();

        }
    }
}