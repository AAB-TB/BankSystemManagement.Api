using BankSystemManagement.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.CommonInterface.Interfaces
{
    public interface ICommonUserService
    {
        Task<int?> CustomerRegistrationAsync(CustomerRegistrationDto registrationDto);
        Task<bool> AdminRegistrationAsync(AdminRegistrationDto registrationDTO);
        Task<CustomerDetailsDto> GetCustomerDetailsAsync(int customerId);
        Task<string> LoginUserAsync(string username, string password);
    }

}
