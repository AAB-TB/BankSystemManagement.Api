using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.CommonInterface.DTOs
{
    public class CustomerAccountOverviewDto
    {
        public int AccountId { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
    }
}
