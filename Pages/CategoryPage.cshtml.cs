using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cit_eTrike.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cit_eTrike.Pages
{
    public class CategoryPageModel : PageModel
    {
        private DataAccess dataAccess = new DataAccess();
        // private Helper helper = new Helper();

        public List<eTrikeCarousel> eTrikes { get; set; }

        public Category category { get; set; }

        public async Task OnGetAsync(int Id)
        {
            string domainName = HttpContext.Request.PathBase.Value.ToString();

            eTrikes = await dataAccess.GeteTrikes(Id,domainName);
            category = await dataAccess.GeteTrikesCategory(Id);

            ViewData["CategoryName"] = category.CategoryName.Trim();
            ViewData["CategoryDesc"] = category.CategoryDescription.Trim();
        }
    }
}