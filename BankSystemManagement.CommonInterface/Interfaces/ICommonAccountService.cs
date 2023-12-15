using BankSystemManagement.CommonInterface.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.CommonInterface.Interfaces
{
    public interface ICommonAccountService
    {
        Task<IEnumerable<CustomerAccountOverviewDto>> GetCustomerAccountsOverviewAsync(int customerId);
        Task<IEnumerable<AccountTransactionDto>> GetAccountTransactionsAsync(int accountId);
        Task<bool> CreateBankAccountAsync(int customerId, int accountTypeId);
        Task<bool> AccountTransferAsync(int fromAccountId, int toAccountId, decimal amount);
    }
}
