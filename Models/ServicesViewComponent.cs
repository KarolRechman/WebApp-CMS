using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cit_eTrike.Models
{
    public class ServicesViewComponent: ViewComponent
    {
        public DataAccess dataAccess = new DataAccess();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var locations = new List< eTrikieLocation>();

            locations = await dataAccess.GetLocations();

            return View("Services", locations);
        }
    }
}
