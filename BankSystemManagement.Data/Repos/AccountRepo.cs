using AutoMapper;
using BankSystemManagement.CommonInterface.DTOs;
using BankSystemManagement.Data.DataModels;
using BankSystemManagement.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
