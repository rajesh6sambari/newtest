﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestingTutor.Pages
{
    public class ContactModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Contact page...";
        }
    }
}
