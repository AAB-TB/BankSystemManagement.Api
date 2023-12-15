using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemManagement.Domain.Models
{
    public class CustomerCredential
    {
        public int CredentialID { get; set; }
        public int CustomerID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
    }
}
