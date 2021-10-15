using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Services;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepo _users;
        private readonly IAuthService _authService;

        public UsersController(IUsersRepo users, IAuthService authService)
        {
            _users = users;
            _authService = authService;
        }

        [HttpGet("")]
        public ActionResult<UserListResponse> Search([FromQuery] UserSearchRequest searchRequest,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            }

            var users = _users.Search(searchRequest);
            var userCount = _users.Count(searchRequest);
            return UserListResponse.Create(searchRequest, users, userCount);
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetById([FromRoute] int id,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            }

            var user = _users.GetById(id);
            return new UserResponse(user);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateUserRequest newUser,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _users.Create(newUser);

            var url = Url.Action("GetById", new { id = user.Id });
            var responseViewModel = new UserResponse(user);
            return Created(url, responseViewModel);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<UserResponse> Update([FromRoute] int id, [FromBody] UpdateUserRequest update,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _users.Update(id, update);
            return new UserResponse(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            }
            
            var isAdmin = _authService.isUserAdmin(authorizationHeader);
            if (!(isAdmin is null))
            {
                return isAdmin;
            }

            _users.Delete(id);
            return Ok();
        }
    }
}