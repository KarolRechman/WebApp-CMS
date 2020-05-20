using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cit_eTrike.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cit_eTrike.Pages.CMS
{
    public class eTrikesEditModel : PageModel
    {
        private DataAccess dataAccess = new DataAccess();

        public List<Category> categories { get; set; }

        public async Task OnGetAsync()
        {
            categories = await dataAccess.GetAlleTrikesCategories();
        }
    }
}
