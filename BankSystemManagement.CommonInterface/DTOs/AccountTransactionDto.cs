using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.CommonInterface.DTOs
{
    public class AccountTransactionDto
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }

        public string TransactionTypeName { get; set; }

        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
