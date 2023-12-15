using AutoMapper;
using BankSystemManagement.Data.DataModels;
using BankSystemManagement.Data.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Data.Repos
{
    public class LoanRepo:ILoanRepo
    {
        private readonly DapperContext _dapperContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LoanRepo> _logger;
        private readonly IConfiguration _configuration;

        public LoanRepo(DapperContext dapperContext, IMapper mapper, ILogger<LoanRepo> logger, IConfiguration configuration)
        {
            _dapperContext = dapperContext;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> CreateLoanAsync(int adminUserId, int customerUserId, decimal amount, int accountTypeId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("AdminUserId", adminUserId);
                    parameters.Add("CustomerUserId", customerUserId);
                    parameters.Add("Amount", amount);
                    parameters.Add("AccountTypesId", accountTypeId);  // New parameter for account type
                    parameters.Add("Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure to create a loan
                    await connection.ExecuteAsync("sp_CreateLoan", parameters, commandType: CommandType.StoredProcedure);

                    // Check the value of the output parameter
                    var success = parameters.Get<bool>("Success");

                    // Return true if the stored procedure indicates success
                    return success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating loan: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CheckAdminExistsAsync(int adminUserId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("AdminUserId", adminUserId);
                    parameters.Add("AdminExists", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure to check if the admin user exists
                    await connection.ExecuteAsync("sp_CheckAdminExists", parameters, commandType: CommandType.StoredProcedure);

                    // Return the value of the output parameter
                    return parameters.Get<bool>("AdminExists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking admin existence: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> CheckAccountTypeExistsAsync(int customerUserId, int accountTypesId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("CustomerUserId", customerUserId);
                    parameters.Add("AccountTypesId", accountTypesId);
                    parameters.Add("AccountTypeExists", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure to check if the customer has the specified account type
                    await connection.ExecuteAsync("sp_CheckAccountTypeExists", parameters, commandType: CommandType.StoredProcedure);

                    // Return the value of the output parameter
                    return parameters.Get<bool>("AccountTypeExists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking account type existence: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CheckCustomerExistsAsync(int customerId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("CustomerID", customerId);
                    parameters.Add("Exists", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure to check if the customer exists
                    await connection.ExecuteAsync("sp_CheckCustomerExists", parameters, commandType: CommandType.StoredProcedure);

                    // Check the value of the output parameter
                    var exists = parameters.Get<bool>("Exists");

                    // Return true if the customer exists
                    return exists;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking customer existence: {ex.Message}");
                throw;
            }
        }

    }
}
