using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Core.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace BudgetBuddyAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        public UserController(IUserService userService, ITransactionService transactionService)
        {
            _userService = userService;
            _transactionService = transactionService;
        }
        
        /// <summary>
        /// Add user's address
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns>returns a string if successful</returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("add_address")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAddress([FromBody]AddAddressRequestDTO requestDTO)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _userService.AddAddress(requestDTO, userId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("get-all_address")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAddress([FromQuery]int pageNumber, int pageSize)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _userService.GetAllAddressByAppUserId(userId, pageSize, pageNumber);

            return StatusCode(result.StatusCode, result);
        }
        [HttpGet]
        [Route("get-all-transaction")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTransaction([FromQuery]int pageNumber, int pageSize)
        {

            var userId = "98765432-10fe-dcba-9876-426614174000";
            var result = await _transactionService.GetAllTransactions(pageSize, pageNumber, userId);

            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        [Route("add-transaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTransaction([FromBody]TransactionRequestDTO request, [FromQuery] int pageNumber, int pageSize)
        {
            var userId = "98765432-10fe-dcba-9876-426614174000";
            var result = await _transactionService.AddTransaction(pageSize, pageNumber, userId, request);

            return StatusCode(result.StatusCode, result);
        }

    }
}
