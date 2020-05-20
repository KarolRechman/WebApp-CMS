using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cit_eTrike.Pages
{
    public class CMSModel : PageModel
    {
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {        


            await HttpContext.SignOutAsync();
            return RedirectToPage("LoginPage");
        }


    }
}