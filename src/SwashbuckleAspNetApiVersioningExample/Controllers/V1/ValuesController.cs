using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SwashbuckleAspNetApiVersioningExample.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "version1", "version1" };
        }
    }
}
