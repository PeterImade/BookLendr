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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetLendings(CancellationToken cancellationToken)
        {
            var response = await _lendingService.GetLendingsAsync(cancellationToken);
            return Ok(response.Value);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLending([FromRoute] int id, CancellationToken cancellationToken)
        {
            var response = await _lendingService.GetLendingAsync(id, cancellationToken);

            if (response.isFailed)
                return NotFound(response.Error);

            return Ok(response.Value);
        }
        
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetLendingsByUserId([FromQuery] int userId, CancellationToken cancellationToken)
        {
            var response = await _lendingService.GetLendingAsync(userId, cancellationToken);

            if (response.isFailed)
                return NotFound(response.Error);

            return Ok(response.Value);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnRequest returnRequest, CancellationToken cancellationToken)
        {
            var response = await _lendingService.ReturnBookAsync(returnRequest.LendingId, cancellationToken);

            if (response.isFailed)
                return NotFound(response.Error);

            return Ok(response.Value);
        }
    }
}