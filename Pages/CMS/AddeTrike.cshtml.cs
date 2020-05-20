using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cit_eTrike.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cit_eTrike
{
    public class AddeTrikeModel : PageModel
    {
        private DataAccess dataAccess = new DataAccess();

        public dynamic eTrikeDesc { get; set; }
        public List<Category> categories { get; set; }

        public async Task OnGetAsync(int id = 0, string category = "")
        {
            eTrikeDesc = dataAccess.GeteTrikeDesc(id);

            categories = await dataAccess.GetAlleTrikesCategories();
            eTrikeDesc.Types = dataAccess.GetAllTypes();

            if (category != "")
            {
                ViewData["category"] = category;
            }
            else
            {
                ViewData["category"] = eTrikeDesc.Category.Trim();
            }
            if (id != 0)
            {
                ViewData["Colors"] = eTrikeDesc.Colors;
                ViewData["Batteries"] = eTrikeDesc.Batteries;
                ViewData["Chargers"] = eTrikeDesc.Chargers;
                //ViewData["Type"] = eTrikeDesc.Typ;
                ViewData["Title"] = "Edit eTrike";

                eTrikeDesc.Batteries = dataAccess.GeteTrikesBatteries(0);
                eTrikeDesc.Colors = dataAccess.GeteTrikeColors(0);
                eTrikeDesc.Chargers = dataAccess.GeteTrikeChargers(0);
                eTrikeDesc.Types = dataAccess.GetAllTypes();
            }
            else
            {
                ViewData["Colors"] = "";
                ViewData["Batteries"] = "";
                ViewData["Chargers"] = "";
                ViewData["Title"] = "Add eTrike";
            }
        }
    }
}