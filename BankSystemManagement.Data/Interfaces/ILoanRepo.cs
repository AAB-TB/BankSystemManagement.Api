using BankSystemManagement.CommonInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Data.Interfaces
{
    public interface ILoanRepo:ICommonLoanService,ICommonValidationService
    {
        Task<bool> CheckAdminExistsAsync(int adminUserId);
        Task<bool> CheckAccountTypeExistsAsync(int customerUserId, int accountTypesId);
    }
}
