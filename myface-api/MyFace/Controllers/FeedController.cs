using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Services;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("feed")]
    public class FeedController : ControllerBase
    {
        private readonly IPostsRepo _posts;
        private readonly IAuthService _authService;

        public FeedController(IPostsRepo posts, IAuthService authService)
        {
            _posts = posts;
            _authService = authService;
        }

        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed([FromQuery] FeedSearchRequest searchRequest,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
            
        {
             var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            }

            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
    }
}
