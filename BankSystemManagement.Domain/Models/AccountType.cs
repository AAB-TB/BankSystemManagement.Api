using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Domain.Models
{
    public class AccountType
    {
        public int AccountTypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
    }
}
