using AutoMapper;
using BankSystemManagement.CommonInterface.DTOs;
using BankSystemManagement.Core.Interfaces;
using BankSystemManagement.Data.Interfaces;
using Microsoft.Extensions.Logging;
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
            throw new NotImplementedException();
        }

        public async Task<bool> CreateBankAccountAsync(int customerId, int accountTypeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AccountTransactionDto>> GetAccountTransactionsAsync(int accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CustomerAccountOverviewDto>> GetCustomerAccountsOverviewAsync(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
