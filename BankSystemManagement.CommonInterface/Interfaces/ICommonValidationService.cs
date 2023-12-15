using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.CommonInterface.Interfaces
{
    public interface ICommonValidationService
    {
        Task<bool> CheckCustomerExistsAsync(int customerId);
    }
}
