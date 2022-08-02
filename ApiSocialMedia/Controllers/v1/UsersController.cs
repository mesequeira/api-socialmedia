using Application.Features.Users.Commands.CreateUserCommand;
using Application.Features.Users.Commands.DeleteUserCommand;
using Application.Features.Users.Commands.UpdateUserCommand;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiSocialMedia.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UsersController : BaseApiController
    {

        //GET: api/<controller>/<id:int>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await Mediator.Send(new GetUsuarioByIdQuery() { Id = id }));

        //GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery filter) => Ok(await Mediator.Send(new GetAllUsersQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber, UserName = filter.UserName }));

        //POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateUserCommand command) => Ok(await Mediator.Send(command));

        //DELETE: api/<controller>/<id:int>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await Mediator.Send(new DeleteUserCommand() { Id = id }));

        //PUT: api/<controller>/<id:int>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateUserCommand command) => Ok(await Mediator.Send(command));
    }
}
