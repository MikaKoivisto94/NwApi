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
    public class UsersController : ControllerBase
    {
        //private readonly NorthwindContext db = new NorthwindContext();

        // Dependency Injection tyyli
        private readonly NorthwindContext db;

        public UsersController(NorthwindContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var users = db.Users;

            foreach (var user in users)
            {
                user.Password = null;
            }
            
            return Ok(users);
        }
        //Adding new user
        [HttpPost]
        public ActionResult PostCreateNew([FromBody] User u)
        {
            try
            {
                db.Users.Add(u);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä " + u.Username);
            }
            catch (Exception e)
            {
                return BadRequest("Lisääminen ei onnistunut. Lisätietoja: " + e);
            }
        }
    }
}
