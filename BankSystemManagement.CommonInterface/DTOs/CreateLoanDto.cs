using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.CommonInterface.DTOs
{
    public class CreateLoanDto
    {
        public int AdminUserId { get; set; }
        public int CustomerUserId { get; set; }
        public decimal Amount { get; set; }
        public int AccountTypeId { get; set; }
    }
}
