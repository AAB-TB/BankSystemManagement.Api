using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.CommonInterface.DTOs
{
    public class BankAccountCreationDto
    {
        public int CustomerId { get; set; }
        public int AccountTypeId { get; set; }
    }
}
