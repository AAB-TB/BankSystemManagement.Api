using BankSystemManagement.Core.DTOs;
using BankSystemManagement.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(
            IUserService userService,
            ILogger<UserController> logger
            )
        {
            _userService = userService;
            _logger = logger;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest("Invalid input. Please provide a username and password.");
            }

            var token = await _userService.LoginUserAsync(loginRequest.Username, loginRequest.Password);

            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { Token = token });
        }

        [HttpPost("NewCustomerRegister")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int?>> RegisterUser([FromBody] CustomerRegistrationDto registrationDto)
        {
            try
            {
                var customerID = await _userService.CustomerRegistrationAsync(registrationDto);

                if (customerID.HasValue)
                {
                    return Ok($"Customer with the id number {customerID} is successfully created!");
                }
                else
                {
                    // You can customize the response based on your requirements
                    return BadRequest("User registration failed. Check the provided data and try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during user registration: {ex.Message}");
                return StatusCode(500, "Unexpected error during user registration.");
            }
        }

        [HttpPost("NewAdminRegister")]
        public async Task<ActionResult<bool>> RegisterAdmin([FromBody] AdminRegistrationDto registrationDTO)
        {
            try
            {
                var isRegistered = await _userService.AdminRegistrationAsync(registrationDTO);

                if (isRegistered)
                {
                    return Ok(true);
                }
                else
                {
                    // You can customize the response based on your requirements
                    return BadRequest("Admin registration failed. Check the provided data and try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during admin registration: {ex.Message}");
                return StatusCode(500, "Unexpected error during admin registration.");
            }
        }


        [HttpGet("GetCustomerDetails")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<CustomerDetailsDto>> GetCustomerDetailsAsync(int customerId)
        {
            try
            {
                // Call the service or repository method to get customer details
                var customerDetails = await _userService.GetCustomerDetailsAsync(customerId);

                // Check if customer details were found
                if (customerDetails != null)
                {
                    // Return the customer details
                    return Ok(customerDetails);
                }
                else
                {
                    // If customer details were not found, return a 404 Not Found response
                    return NotFound($"Customer with ID {customerId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                _logger.LogError($"Error retrieving customer details: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "Unexpected error retrieving customer details.");
            }
        }


    }
}
