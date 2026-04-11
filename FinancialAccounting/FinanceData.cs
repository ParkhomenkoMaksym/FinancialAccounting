using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAccounting
{
    public class FinanceData
    {
        public List<Finance> Expenses { get; set; } = new();
        public List<Finance> Income { get; set; } = new();
        public List<Finance> Debtors { get; set; } = new();
    }
}
