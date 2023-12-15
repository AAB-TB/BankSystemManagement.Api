using AutoMapper;
using BankSystemManagement.CommonInterface.DTOs;
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
    public class AccountRepo:IAccountRepo
    {
        private readonly DapperContext _dapperContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountRepo> _logger;
        private readonly IConfiguration _configuration;

        public AccountRepo(DapperContext dapperContext, IMapper mapper, ILogger<AccountRepo> logger, IConfiguration configuration)
        {
            _dapperContext = dapperContext;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> AccountTransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {

            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("FromAccountID", fromAccountId);
                    parameters.Add("ToAccountID", toAccountId);
                    parameters.Add("Amount", amount);

                    parameters.Add("Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure for intra-account transfer
                    await connection.ExecuteAsync("sp_AccountTransfer", parameters, commandType: CommandType.StoredProcedure);

                    // Check the value of the output parameter
                    var success = parameters.Get<bool>("Success");

                    // Return true if the stored procedure indicates success
                    return success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during intra-account transfer: {ex.Message}");
                throw;
            }
        }
        public async Task<decimal> GetBalanceAsync(int accountId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("AccountId", accountId);
                    parameters.Add("Balance", dbType: DbType.Decimal, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("sp_GetAccountBalance", parameters, commandType: CommandType.StoredProcedure);

                    return parameters.Get<decimal>("Balance");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving account balance: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> CreateBankAccountAsync(int customerId, int accountTypeId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("CustomerID", customerId);
                    parameters.Add("AccountTypesId", accountTypeId);
                    parameters.Add("Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure to create an account
                    await connection.ExecuteAsync("sp_CreateBankAccount", parameters, commandType: CommandType.StoredProcedure);

                    // Check the value of the output parameter
                    var success = parameters.Get<bool>("Success");

                    // Return true if the stored procedure indicates success
                    return success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating account: {ex.Message}");
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

        public async Task<bool> CheckAccountOwnershipAsync(int userId, int accountId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("UserID", userId);
                    parameters.Add("AccountID", accountId);
                    parameters.Add("IsOwner", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    // Execute the stored procedure to check if the user owns the account
                    await connection.ExecuteAsync("sp_CheckAccountOwnership", parameters, commandType: CommandType.StoredProcedure);

                    // Check the value of the output parameter
                    var isOwner = parameters.Get<bool>("IsOwner");

                    // Return true if the user owns the account
                    return isOwner;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking account ownership: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AccountTransactionDto>> GetAccountTransactionsAsync(int accountId)
        {
            try
            {
                using (IDbConnection dbConnection = _dapperContext.GetDbConnection())
                {
                    dbConnection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("AccountID", accountId);

                    return await dbConnection.QueryAsync<AccountTransactionDto>("sp_GetAccountTransactions", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving account transactions: {ex.Message}");
                throw; // Rethrow the exception after logging
            }
        }

        public async Task<IEnumerable<CustomerAccountOverviewDto>> GetCustomerAccountsOverviewAsync(int customerId)
        {
            try
            {
                using (IDbConnection dbConnection = _dapperContext.GetDbConnection())
                {
                    dbConnection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("CustomerID", customerId);

                    var accountData = await dbConnection.QueryAsync<CustomerAccountOverviewDto>("sp_GetCustomerAccountsOverview", parameters, commandType: CommandType.StoredProcedure);




                    return accountData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving customer accounts overview: {ex.Message}");
                throw; // Rethrow the exception after logging
            }
        }
    }
}
