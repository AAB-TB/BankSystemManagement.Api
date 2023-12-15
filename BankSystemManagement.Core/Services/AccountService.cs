using AutoMapper;
using BankSystemManagement.CommonInterface.DTOs;
using BankSystemManagement.Core.Interfaces;
using BankSystemManagement.Data.Interfaces;
using BankSystemManagement.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Core.Services
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepo _accountRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepo accountRepo, IMapper mapper, ILogger<AccountService> logger)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AccountTransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            try
            {
                // Check if the source account has sufficient balance
                decimal fromAccountBalance = await _accountRepo.GetBalanceAsync(fromAccountId);

                if (fromAccountBalance < amount)
                {
                    // Insufficient balance
                    return false;
                }

                // Initiate the account transfer
                await _accountRepo.AccountTransferAsync(fromAccountId, toAccountId, amount);

                // The transfer was successful
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during account transfer: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CreateBankAccountAsync(int customerId, int accountTypeId)
        {

            try
            {
                // Check if the customer exists before creating an account
                bool customerExists = await _accountRepo.CheckCustomerExistsAsync(customerId);

                if (!customerExists)
                {
                    _logger.LogWarning("Customer does not exist.");
                    return false;
                }
                // Initiate the Bank Account Creation
                await _accountRepo.CreateBankAccountAsync(customerId, accountTypeId);

                // Bank Creation is Successfull
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during Bank Creation: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AccountTransactionDto>> GetAccountTransactionsAsync(int accountId)
        {
            return await _accountRepo.GetAccountTransactionsAsync(accountId);
        }

        public async Task<IEnumerable<CustomerAccountOverviewDto>> GetCustomerAccountsOverviewAsync(int customerId)
        {
            return await _accountRepo.GetCustomerAccountsOverviewAsync(customerId);
        }
    }
}
