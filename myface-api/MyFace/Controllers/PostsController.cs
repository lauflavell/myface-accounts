using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Services;



namespace MyFace.Controllers
{
    [ApiController]
    [Route("/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _users;

        private readonly IAuthService _authService;

        public PostsController(IPostsRepo posts, IUsersRepo users, IAuthService authService)
        {
            _posts = posts;
            _users = users;
            _authService = authService;
        }

        [HttpGet("")]
        public ActionResult<PostListResponse> Search(
            [FromQuery] PostSearchRequest searchRequest,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            }

            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);

        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById([FromRoute] int id,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            }

            var post = _posts.GetById(id);
            return new PostResponse(post);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePostRequest newPost,
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

            var post = _posts.Create(newPost);

            var url = Url.Action("GetById", new { id = post.Id });
            var postResponse = new PostResponse(post);
            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update([FromRoute] int id, [FromBody] UpdatePostRequest update,
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

            var post = _posts.Update(id, update);
            return new PostResponse(post);
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
            
            _posts.Delete(id);
            return Ok();
        }
    }
}