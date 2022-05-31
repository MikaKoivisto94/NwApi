using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NWApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace NWApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        //private readonly NorthwindContext db = new NorthwindContext();

        // Dependency Injection tyyli
        private readonly NorthwindContext db;

        public DocumentationController(NorthwindContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        [Route("{keycode}")]
        public ActionResult GetAll(string keycode)
        {
            var xxx = db.Documentations.Where(d => d.Keycode == keycode);
            return Ok(xxx);
        }
    }
}
