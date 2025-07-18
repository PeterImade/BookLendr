using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands;
using UserService.Application.DTOs;
using UserService.Application.Queries;

namespace UserService.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var command = new CreateUserCommand { UserRegisterDTO = userRegisterDTO };
            await _mediator.Send(command);
            return StatusCode(201, command);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO loginDTO)
        {
            var command = new LoginUserCommand { UserLoginDTO  = loginDTO };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet] 
        public async Task<ActionResult<UserResponseDTO>> GetUserByEmail([FromQuery] GetUserByEmailQuery query)
        { 
            var user = await _mediator.Send(query); 
            return Ok(user);
        }
    }
}
