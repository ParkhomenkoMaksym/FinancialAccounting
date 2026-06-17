using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAccounting.Models
{
    public class Finance
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }


        public Finance(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
