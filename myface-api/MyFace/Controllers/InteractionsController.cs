using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Services;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/interactions")]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionsRepo _interactions;
        private readonly IAuthService _authService;

        public InteractionsController(IInteractionsRepo interactions, IAuthService authService)
        {
            _interactions = interactions;
            _authService = authService;
        }

        [HttpGet("")]
        public ActionResult<ListResponse<InteractionResponse>> Search([FromQuery] SearchRequest search,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var auth = _authService.Authenticate(authorizationHeader);
            if (!(auth is null))
            {
                return auth;
            }

            var interactions = _interactions.Search(search);
            var interactionCount = _interactions.Count(search);
            return InteractionListResponse.Create(search, interactions, interactionCount);
        }

        [HttpGet("{id}")]
        public ActionResult<InteractionResponse> GetById([FromRoute] int id,
            [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            var interaction = _interactions.GetById(id);
            var likes = _interactions.CountLikes(id);
            var dislikes = _interactions.CountDislikes(id);
            return new InteractionResponse(interaction,likes, dislikes);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateInteractionRequest newUser,
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
            var userCredentials = _authService.userCredentials(authorizationHeader);
            var username = userCredentials[0];
            var interaction = _interactions.Create(newUser, username);

            var likes = _interactions.CountLikes(interaction.PostId);
            var dislikes = _interactions.CountDislikes(interaction.PostId);
            var url = Url.Action("GetById", new { id = interaction.Id });
            var responseViewModel = new InteractionResponse(interaction, likes, dislikes);
            return Created(url, responseViewModel);
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

            _interactions.Delete(id);
            return Ok();
        }
    }
}
