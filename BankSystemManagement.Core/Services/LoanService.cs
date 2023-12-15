using AutoMapper;
using BankSystemManagement.Core.Interfaces;
using BankSystemManagement.Data.DataModels;
using BankSystemManagement.Data.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Core.Services
{
    public class LoanService:ILoanService
    {
        private readonly ILoanRepo _loanRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<LoanService> _logger;

        public LoanService(ILoanRepo loanRepo, IMapper mapper, ILogger<LoanService> logger)
        {
            _loanRepo = loanRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> CreateLoanAsync(int adminUserId, int customerUserId, decimal amount, int accountTypeId)
        {
            try
            {
                // Check if the admin user exists and is an admin
                var adminExists = await _loanRepo.CheckAdminExistsAsync(adminUserId);
                if (!adminExists)
                {
                    _logger.LogWarning("Admin user does not exist or is not an admin.");
                    return false;
                }

                // Check if the customer user exists
                var customerExists = await _loanRepo.CheckCustomerExistsAsync(customerUserId);
                if (!customerExists)
                {
                    _logger.LogWarning("Customer user does not exist.");
                    return false;
                }

                // Check if the customer has the specified account type
                var accountTypeExists = await _loanRepo.CheckAccountTypeExistsAsync(customerUserId, accountTypeId);
                if (!accountTypeExists)
                {
                    _logger.LogWarning("Customer does not have the specified account type.");
                    return false;
                }

               await _loanRepo.CreateLoanAsync(adminUserId, customerUserId, amount, accountTypeId);

                return true;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating loan: {ex.Message}");
                throw;
            }
        }
    }
}
