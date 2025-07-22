using LendingService.Application.Dtos;
using LendingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LendingService.Controllers
{
    [Route("lendings")]
    [ApiController]
    public class LendingController: ControllerBase
    {
        private readonly ILendingService _lendingService;

        public LendingController(ILendingService lendingService)
        {
            _lendingService = lendingService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LendBook([FromBody] LendRequest lendRequest, CancellationToken cancellationToken)
        {
            if (!int.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var userEmail = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            if (userEmail is null)
            {
                return BadRequest("Invalid user email.");
            }

            var response = await _lendingService.LendAsync(lendRequest, userId, userEmail, cancellationToken);

            if (response.isFailed)
            {
                return BadRequest(response.Error);
            }

            return StatusCode(201, response.Value);
        }
    }
}