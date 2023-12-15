using AutoMapper;
using BankSystemManagement.CommonInterface.Interfaces;
using BankSystemManagement.Core.DTOs;
using BankSystemManagement.Data.DataModels;
using BankSystemManagement.Data.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Data.Repos
{
    public class UserRepo:IUserRepo
    {
        private readonly DapperContext _dapperContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRepo> _logger;
        private readonly IConfiguration _configuration;

        public UserRepo(DapperContext dapperContext, IMapper mapper, ILogger<UserRepo> logger, IConfiguration configuration)
        {
            _dapperContext = dapperContext;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> AdminRegistrationAsync(AdminRegistrationDto registrationDTO)
        {

            try
            {
                using (IDbConnection dbConnection = _dapperContext.GetDbConnection())
                {
                    dbConnection.Open();

                    // Hash the password before storing it
                    string hashedPassword = HashPassword(registrationDTO.PasswordHash);

                    // Create DynamicParameters
                    var parameters = new DynamicParameters();
                    parameters.Add("Username", registrationDTO.Username);
                    parameters.Add("PasswordHash", hashedPassword);

                    // Add output parameter for success status
                    parameters.Add("Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure
                    await dbConnection.ExecuteAsync("AdminRegistration", parameters, commandType: CommandType.StoredProcedure);

                    // Retrieve the success status from the output parameter
                    bool success = parameters.Get<bool>("Success");

                    if (success)
                    {
                        // Log successful registration
                        _logger.LogInformation($"Admin '{registrationDTO.Username}' registered successfully.");
                    }
                    else
                    {
                        // Log registration failure
                        _logger.LogWarning($"Admin registration failed for '{registrationDTO.Username}'. Username may already exist.");
                    }

                    return success;
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                _logger.LogError($"Error during admin registration: {ex.Message}");
                return false;
            }
        }

        public async Task<int?> CustomerRegistrationAsync(CustomerRegistrationDto registrationDto)
        {
            try
            {
                using (IDbConnection dbConnection = _dapperContext.GetDbConnection())
                {
                    dbConnection.Open();

                    // Hash the password before storing it
                    string hashedPassword = HashPassword(registrationDto.PasswordHash);

                    // Create DynamicParameters
                    var parameters = new DynamicParameters();
                    parameters.Add("Gender", registrationDto.Gender);
                    parameters.Add("Givenname", registrationDto.Givenname);
                    parameters.Add("Surname", registrationDto.Surname);
                    parameters.Add("Streetaddress", registrationDto.Streetaddress);
                    parameters.Add("City", registrationDto.City);
                    parameters.Add("Zipcode", registrationDto.Zipcode);
                    parameters.Add("Country", registrationDto.Country);
                    parameters.Add("CountryCode", registrationDto.CountryCode);
                    parameters.Add("Birthday", registrationDto.Birthday);
                    parameters.Add("Telephonecountrycode", registrationDto.Telephonecountrycode);
                    parameters.Add("Telephonenumber", registrationDto.Telephonenumber);
                    parameters.Add("Emailaddress", registrationDto.Emailaddress);
                    parameters.Add("AccountTypeId", registrationDto.AccountTypeId);
                    parameters.Add("Username", registrationDto.Username);
                    parameters.Add("PasswordHash", hashedPassword);
                    parameters.Add("CustomerID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure
                    await dbConnection.ExecuteAsync("CustomerRegistration", parameters, commandType: CommandType.StoredProcedure);

                    // Retrieve output parameter
                    int? customerID = parameters.Get<int?>("CustomerID");
                    bool registrationSuccess = parameters.Get<bool>("Success");

                    // Log successful or failed registration
                    if (registrationSuccess && customerID.HasValue)
                    {
                        _logger.LogInformation($"User '{registrationDto.Username}' with CustomerID {customerID} registered successfully.");
                    }
                    else
                    {
                        _logger.LogWarning($"User registration for '{registrationDto.Username}' failed.");
                    }

                    return customerID;
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                _logger.LogError($"Error during user registration: {ex.Message}");
                return null;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public async Task<CustomerDetailsDto> GetCustomerDetailsAsync(int customerId)
        {
            try
            {
                using (IDbConnection dbConnection = _dapperContext.GetDbConnection())
                {
                    dbConnection.Open();

                    // Create DynamicParameters
                    var parameters = new DynamicParameters();
                    parameters.Add("CustomerID", customerId);

                    // Execute the stored procedure to get customer details
                    var result = await dbConnection.QueryAsync<CustomerDetailsDto>("sp_GetCustomerDetails", parameters, commandType: CommandType.StoredProcedure);

                    // If the result is not null or empty, return the first item (assuming CustomerDetailsDto is a class representing customer details)
                    return result?.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                _logger.LogError($"Error retrieving customer details: {ex.Message}");
                return null;
            }
        }

        public async Task<string> LoginUserAsync(string username, string password)
        {
            try
            {
                // Hash the provided password
                var hashedPassword = HashPassword(password);

                // Use Dapper connection for SQL query
                using (IDbConnection dbConnection = _dapperContext.GetDbConnection())
                {
                    dbConnection.Open();

                    // Create DynamicParameters
                    var parameters = new DynamicParameters();
                    parameters.Add("Username", username);
                    parameters.Add("Password", hashedPassword);
                    parameters.Add("UserID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("RoleName", dbType: DbType.String, size: 20, direction: ParameterDirection.Output);
                    parameters.Add("Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure
                    await dbConnection.ExecuteAsync("CommonLogin", parameters, commandType: CommandType.StoredProcedure);

                    // Retrieve output parameters
                    int userId = parameters.Get<int>("UserID");
                    string roleName = parameters.Get<string>("RoleName");
                    bool success = parameters.Get<bool>("Success");

                    if (!success || userId == 0 || string.IsNullOrEmpty(roleName))
                    {
                        _logger.LogWarning("Invalid username or password.");
                        return null; // Invalid credentials
                    }

                    // User is valid, generate a token
                    var token = GenerateToken(userId, roleName);

                    return token;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error authenticating user: {ex.Message}");
                return "An error occurred while authenticating. Please try again later.";
            }
        }

        private string GenerateToken(int userId, string userRole)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Role, userRole),
        // Add any additional claims as needed
    };

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Set expiration to 60 minutes
            var expires = DateTime.UtcNow.AddMinutes(60);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:44362", // Set issuer
                audience: "https://localhost:44362", // Set audience
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            // Remove double quotes from the token string
            tokenString = tokenString.Replace("\"", string.Empty);

            return tokenString;
        }

    }
}
