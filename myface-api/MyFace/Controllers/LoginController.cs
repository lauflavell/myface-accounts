using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Services;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {

        private readonly IAuthService _authService;

        [HttpGet("")]
        public ActionResult login([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            
            }
        return auth;
        }
    }
}