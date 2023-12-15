using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Core.DTOs
{
    public class AdminRegistrationDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
