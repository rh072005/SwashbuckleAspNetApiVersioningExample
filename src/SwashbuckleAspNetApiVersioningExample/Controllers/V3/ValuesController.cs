using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SwashbuckleAspNetApiVersioningExample.Controllers.V3
{
    [ApiVersion("3")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "version3", "version3" };
        }
    }
}
