using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.CommonInterface.Interfaces
{
    public interface ICommonLoanService
    {
        Task<bool> CreateLoanAsync(int adminUserId, int customerUserId, decimal amount, int accountTypeId);
    }
}
