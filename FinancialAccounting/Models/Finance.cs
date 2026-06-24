using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinancialAccounting.ViewModels;

namespace FinancialAccounting.Models
{
    public class Finance : BaseViewModel
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private decimal amount;
        public decimal Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }


        public Finance(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
