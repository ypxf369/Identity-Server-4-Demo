using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServerMVcClient.Controllers
{
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var username = User.Claims.First(i => i.Type == "email").Value;
            return Ok(username);
        }
    }
}