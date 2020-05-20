using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cit_eTrike.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cit_eTrike.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        //private readonly DataAccess dataAcesss = new DataAccess();

        ///// <summary>
        ///// Retrieves directory path from "appsettings.json" file
        ///// </summary>
        ///// <returns>Current directory, path were files are stored</returns>
        //public string GetDirectory()
        //{
        //    var config = configuration();
        //    string directory = config.GetSection("StaticFiles").GetSection("StaticFilesFolder").Value;

        //    return directory;
        //}

        ///// <summary>
        ///// Builds configuration object, based on "appsettings.json" file
        ///// </summary>
        ///// <returns>Configuration object</returns>
        //public IConfigurationRoot configuration()
        //{
        //    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //    return builder.Build();
        //}

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}
        //public JsonResult OnGetFilter()
        //{
        //    var et = new eTrikeCarousel();
        //    et.CategoryName = "bl";
        //    return new JsonResult(et);
        //}
        //public List<eTrikeCarousel> eTrikes { get; set; }
        //public void OnGet()
        //{
        //    eTrikes = dataAcesss.GeteTrikes();
        //}
    }
}
