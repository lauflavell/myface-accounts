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
        private readonly IUsersRepo _users;

        public LoginController(IUsersRepo users, IAuthService authService)
        {
            _users = users;
            _authService = authService;
        }

        [HttpGet("")]
        public ActionResult<LoginResponse> login([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;

            }
            var userCredentials = _authService.userCredentials(authorizationHeader);
            var username = userCredentials[0];
            var user = _users.GetUserForAuthentication(username);
            return new LoginResponse(user);
        }
    }
}