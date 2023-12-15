using BankSystemManagement.CommonInterface.DTOs;
using BankSystemManagement.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly ILogger<LoanController> _logger;

        public LoanController(ILoanService loanService, ILogger<LoanController> logger)
        {
            _loanService = loanService;
            _logger = logger;
        }

        [HttpPost("CreateLoan")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanDto createLoanDto)
        {
            try
            {
                var result = await _loanService.CreateLoanAsync(createLoanDto.AdminUserId, createLoanDto.CustomerUserId, createLoanDto.Amount, createLoanDto.AccountTypeId);
                return result ? Ok("Loan created successfully") : BadRequest("Failed to create loan");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating loan: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }

}
