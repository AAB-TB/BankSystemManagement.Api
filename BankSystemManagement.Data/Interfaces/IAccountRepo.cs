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
        Task<decimal> GetBalanceAsync(int accountId);
        
    }
}
