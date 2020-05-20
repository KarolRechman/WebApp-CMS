using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cit_eTrike.Models
{
    public class AboutUsViewComponent : ViewComponent
    {
        public DataAccess dataAccess = new DataAccess();

        public async Task<IViewComponentResult> InvokeAsync(int IdSection)
        {
            string ViewName = "";

            switch (IdSection)
            {
                case 2:
                    ViewName = "AboutUs";
                    break;
                case 1:
                    ViewName = "IntroTitle";
                    break;
                case 4:
                    ViewName = "Contact";
                    break;
                default:
                    break;
            }

            var HTMLContent = await dataAccess.GetContent(IdSection);

            return View(ViewName, HTMLContent);
        }
    }
}
