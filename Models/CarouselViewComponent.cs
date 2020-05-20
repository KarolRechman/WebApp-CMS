using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cit_eTrike.Models
{
    public class CarouselViewComponent: ViewComponent
    {
        public DataAccess dataAccess = new DataAccess();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var eTrikes = new List<eTrikeCarousel>();
            string domainName = HttpContext.Request.PathBase.Value.ToString();

            //eTrikes = await dataAccess.GeteTrikes(domainName);
            eTrikes = await dataAccess.GeteTrikesCategories(domainName);
    
            return View("Carousel", eTrikes);
        }
    }
}
