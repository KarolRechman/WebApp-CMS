using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cit_eTrike.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cit_eTrike.Pages
{
    public class PoliciesPageModel : PageModel
    {
        private readonly DataAccess dataAcesss = new DataAccess();
        /// <summary>
        /// Used only to retrieve string
        /// </summary>
        public eTrikeCategory eTrikeCategory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void OnGet(int id)
        {
            eTrikeCategory = dataAcesss.GetDataPolicy(id);
        }
    }
}