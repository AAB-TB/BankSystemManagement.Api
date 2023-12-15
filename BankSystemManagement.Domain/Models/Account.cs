using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Domain.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Frequency { get; set; }
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }
        public int? AccountTypesId { get; set; }
        public int? CustomerID { get; set; }

        // Navigation properties
        public virtual AccountType AccountTypes { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
