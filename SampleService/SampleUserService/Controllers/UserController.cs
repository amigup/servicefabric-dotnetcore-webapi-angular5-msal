using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleUserService.Models;

namespace SampleUserService.Controllers
{
    
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "BearerTokenAuthentication")]
        [HttpGet]
        public string Me()
        {
            return "Test User!";
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "BearerTokenAuthentication")]
        [HttpGet("~/claims")]
        public List<UserClaim> GetClaims()
        {
            List<UserClaim> claims = new List<UserClaim>();
            foreach(var claim in HttpContext.User.Claims)
            {
                claims.Add(new UserClaim() { ClaimType = claim.Type, ClaimValue = claim.Value });
            }

            return claims;
        }
    }
}