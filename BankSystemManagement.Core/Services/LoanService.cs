using AutoMapper;
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
            throw new NotImplementedException();
        }
    }
}
