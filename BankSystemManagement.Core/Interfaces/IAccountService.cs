using BankSystemManagement.CommonInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Core.Interfaces
{
    public interface IAccountService:ICommonAccountService
    {
        Task<bool> AccountTransferAsync(int userId, int fromAccountId, int toAccountId, decimal amount);
        Task<bool> CreateBankAccountAsync(int userId, int customerId, int accountTypeId);
    }
}
