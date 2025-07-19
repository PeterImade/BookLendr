using LendingService.Application.Dtos;
using LendingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LendingService.Controllers
{
    [Route("api/lendings")]
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
            if (!int.TryParse(HttpContext.User?.FindFirst("Id")?.Value, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var response = await _lendingService.LendAsync(lendRequest, userId, cancellationToken);

            if (response.isFailed)
            {
                return BadRequest(response.Error);
            }

            return StatusCode(201, response.Value);
        }
    }
}