using Application.DTOs.Jwt;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using Application.Features.Authenticate.Commands.RegisterCommand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiSocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request) => Ok(await Mediator.Send(new AuthenticateCommand()
        {
            Email = request.Email,
            Password = request.Password,
            UserName = request.UserName,
            IpAddress = GenerateIpAddress()
        }));

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request) => Ok(await Mediator.Send(new RegisterCommand()
        {
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
            UserName = request.UserName,
            Origin = Request.Headers["Origin"]
        }));

        private string GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
