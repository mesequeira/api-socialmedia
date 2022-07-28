using Application.Features.Users.Commands.CreateUserCommand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiSocialMedia.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UsersController : BaseApiController
    {
        //POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(DeleteUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
