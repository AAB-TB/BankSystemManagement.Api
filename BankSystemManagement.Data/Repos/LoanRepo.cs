using AutoMapper;
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
            throw new NotImplementedException();
        }
    }
}
