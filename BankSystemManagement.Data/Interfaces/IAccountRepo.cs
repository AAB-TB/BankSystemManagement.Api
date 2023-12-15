using BankSystemManagement.CommonInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Data.Interfaces
{
    public interface IAccountRepo:ICommonAccountService,ICommonValidationService
    {
        Task<bool> AccountTransferAsync(int fromAccountId, int toAccountId, decimal amount);
        Task<decimal> GetBalanceAsync(int accountId);
        Task<bool> CheckAccountOwnershipAsync(int userId, int accountId);
        Task<bool> CreateBankAccountAsync(int customerId, int accountTypeId);

    }
}
