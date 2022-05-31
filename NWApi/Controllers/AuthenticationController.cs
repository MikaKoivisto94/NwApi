using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWApi.Models;
using NWApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Credentials tunnukset)
        {
            var loggedUser = _authenticateService.Authenticate(tunnukset.Username, tunnukset.Password);

            if (loggedUser == null)
                return BadRequest(new { message = "Käyttäjätunnus tai salasana on virheellinen." });

            return Ok(loggedUser); // Palautus front endiin (sis. vain loggedUser luokan mukaiset kentät)
        }

    }
}
