using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cit_eTrike.Models
{
    public class MenuViewComponent: ViewComponent
    {
        public DataAccess dataAccess = new DataAccess();

        public async Task<IViewComponentResult> InvokeAsync(int id, string pageRoute)
        {
            var menu = new Menu();
            string ViewName = "";

            switch (id)
            {
                case 2:
                    ViewName = "AdminMenu";
                    break;
                case 1:
                    ViewName = "Menu";
                    break;              
                default:
                    break;
            }

            menu = await dataAccess.GetMenu(id);
            menu.PageRoute = pageRoute;

            return View(ViewName, menu);
        }
    }
}
