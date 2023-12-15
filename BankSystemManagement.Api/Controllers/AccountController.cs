using BankSystemManagement.CommonInterface.DTOs;
using BankSystemManagement.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(
            IAccountService accountService,
            ILogger<AccountController> logger
            )
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet("GetCustomerAccountOverview/{customerId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<CustomerAccountOverviewDto>> GetCustomerAccountsOverviewAsync(int customerId)
        {
            try
            {
                var accountOverview = await _accountService.GetCustomerAccountsOverviewAsync(customerId);

                if (accountOverview != null)
                {
                    return Ok(accountOverview);
                }
                else
                {
                    return NotFound($"No account overview found for customer with ID {customerId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving account overview: {ex.Message}");
                return StatusCode(500, "Unexpected error retrieving account overview.");
            }
        }

        [HttpGet("GetAccountTransactions/{accountId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<AccountTransactionDto>>> GetAccountTransactionsAsync(int accountId)
        {
            try
            {
                var accountTransactions = await _accountService.GetAccountTransactionsAsync(accountId);

                if (accountTransactions != null && accountTransactions.Any())
                {
                    return Ok(accountTransactions);
                }
                else
                {
                    return NotFound($"No transactions found for account with ID {accountId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving account transactions: {ex.Message}");
                return StatusCode(500, "Unexpected error retrieving account transactions.");
            }
        }
        [HttpPost("CreateBankAccount")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateBankAccountAsync([FromBody] BankAccountCreationDto bankAccountCreationDto)
        {
            try
            {
                var result = await _accountService.CreateBankAccountAsync(bankAccountCreationDto.CustomerId, bankAccountCreationDto.AccountTypeId);

                if (result)
                {
                    return Ok("Account created successfully.");
                }
                else
                {
                    return BadRequest("Failed to create account. Please check the provided data.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating account: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("AccountTransfer")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AccountTransferAsync([FromBody] AccountTransferDto transferDto)
        {
            try
            {
                var result = await _accountService.AccountTransferAsync(transferDto.FromAccountId, transferDto.ToAccountId, transferDto.Amount);

                if (result)
                {
                    return Ok("transaction successful.");
                }
                else
                {
                    return BadRequest("Failed to perform intra-account transfer. Please check the provided data.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during intra-account transfer: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }




    }
}
