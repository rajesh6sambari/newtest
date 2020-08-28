using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.DevAssignments
{
    //[Authorize(Policy = "InstructorAndHigherPolicy")]

    public class DevAssignmentPageModel :PageModel
    {
        protected readonly ApplicationDbContext Context;

        protected DevAssignmentPageModel(ApplicationDbContext context)
        {
            Context = context;
        }
    }
}
