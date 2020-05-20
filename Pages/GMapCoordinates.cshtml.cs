using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cit_eTrike.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cit_eTrike.Pages
{
    public class GMapCoordinatesModel : PageModel
    {
        public DataAccess dataAccess = new DataAccess();
        public async Task<JsonResult> OnGetAsync()
        {
            var locations = new List<eTrikieLocation>();
            locations = await dataAccess.GetLocations();
            return new JsonResult(locations);
        }
    }
}