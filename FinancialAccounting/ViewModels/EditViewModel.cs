using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using FinancialAccounting.Models;

namespace FinancialAccounting.ViewModels
{
    public class EditViewModel : BaseViewModel
    {
        //private static int savedIndex;
        //private static int newSavedIndex;
        //private static decimal periodNum = 0;
        //private static bool positivePeriod = true;

        //private List<Finance> list;
        private Finance finance;
        private Func<Task> saveAction;
        private int savedIndex;
        private static int newSavedIndex = 2;
        private static decimal periodNum = 0;
        private static bool positivePeriod = true;

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

        private string amount;

        public string Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ObservableCollection<Finance> List { get; set; } = new();

        Dictionary<string, int> Periods = new Dictionary<string, int>()
        {
            {"hour", 1},
            {"day", 8},
            {"week", 5},
            {"month", 4},
            {"sixMonths", 6},
            {"year", 2},
            {"fourYears", 4}
        };

        //public event PropertyChangedEventHandler? PropertyChanged;

        //int period, ObservableCollection<Finance> list, Finance finance, string debtorStatus, Func<Task> saveAction
        public EditViewModel()
        {
            //savedIndex = period;
            //List = list;
            //this.finance = finance;
            EditCommand = new Command(async () => await Edit());
            DeleteCommand = new Command(async () => await Delete());
        }

        private async Task Edit()
        {
            periodNum = periodAmount(savedIndex, newSavedIndex);
            //lblAmount.Text += " " + symbol + " ";
            char symbol = Amount.Contains('-') ? '-' : '+';

            var parts = Amount.Split(symbol);

            if (parts.Length == 1)
            {
                if (decimal.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                {
                    finance.Name = Name;
                    finance.Amount = (positivePeriod) ? value * periodNum : value / periodNum;
                }
            }
            else
            {
                for (int i = 1; i < parts.Length; i++)
                {
                    if (decimal.TryParse(parts[i].Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                    {
                        if (symbol == '+')
                        {
                            finance.Name = Name;
                            finance.Amount += (positivePeriod) ? value * periodNum : value / periodNum;
                        }
                        else
                        {
                            finance.Name = Name;
                            finance.Amount -= (positivePeriod) ? value * periodNum : value / periodNum;
                        }
                    }

                }
            }

            await saveAction();

            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task Delete()
        {
            List.Remove(finance);
            await saveAction();
            await Shell.Current.Navigation.PopModalAsync();
        }

        public decimal periodAmount(int period, int newPeriod)
        {

            //Dictionary<int, int> formulas = new Dictionary<int, int>()
            //    {
            //        {0, hour},
            //        {1, day},
            //        {2, week},
            //        {3, month},
            //        {4, sixMonths},
            //        {5, year},
            //        {6, fourYears},
            //    };

            if (period > newSavedIndex)
            {
                positivePeriod = true;
                return recursAmount(period, newSavedIndex + 1);
            }
            else if (period < newSavedIndex)
            {
                positivePeriod = false;
                return recursAmount(newSavedIndex, period + 1);
            }

            positivePeriod = true;
            return 1m;
        }

        public decimal recursAmount(int period, int newPeriod)
        {

            if (period == newPeriod)
            {
                return Periods.ElementAt(period).Value;
            }

            return Periods.ElementAt(period).Value * recursAmount(period - 1, newPeriod);
        }
    }
}
