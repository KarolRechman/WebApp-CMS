using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Cit_eTrike.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Cit_eTrike.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class eTrikesController : ControllerBase
    {
        private DataAccess dataAccess = new DataAccess();

        // GET: api/eTrikes
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/eTrikes/5
        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(int id)
        {
            var eTrike = dataAccess.GeteTrikeDesc(id);
            //var json = JsonConvert.SerializeObject(eTrike);
            return JsonConvert.SerializeObject(eTrike);
        }

        // POST: api/eTrikes
        [HttpPost]
        public ActionResult Post([FromBody] dynamic eTrike)
        {
            var result = dataAccess.AddeTrike(eTrike, false);
            return Content(result, "application/json");
        }

        // PUT: api/eTrikes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] bool value)
        {
            dataAccess.SetAvailability(id, value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = dataAccess.DeleteeTrike(id);
            return Content(result, "application/json");
        }
    }
}
