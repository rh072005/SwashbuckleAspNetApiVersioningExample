using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SwashbuckleAspNetApiVersioningExample.Controllers.V2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "version2", "version2" };
        }
    }
}
