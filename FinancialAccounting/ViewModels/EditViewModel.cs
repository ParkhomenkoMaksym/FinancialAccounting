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
        private Finance finance;
        //private Func<Task> saveAction;
        private int savedIndex;
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

        public ICommand PlusCommand { get; }
        public ICommand MinusCommand { get; }

        public ObservableCollection<Finance> List { get; set; } = new();

        public string[] Periods { get; } = new string[]
        {
            "hour",     // 1,
            "day",      // 8,
            "week",     // 5,
            "month",    // 4,
            "sixMonths",// 6,
            "year",     // 2,
            "fourYears" // 4 
        };

        public int[] PeriodsNum { get; } = new int[]
        {
            1, 8, 5, 4, 6, 2, 4
        };

        private int selectedPeriodIndex;

        public int SelectedPeriodIndex
        {
            get => selectedPeriodIndex;
            set
            {
                selectedPeriodIndex = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Finance> ListUI { get; set; }
        public FinanceData Data { get; set; } = new FinanceData();
        public readonly Func<Task> saveData;

        private bool isPlusVisible = true;
        public bool IsPlusVisible
        {
            get => isPlusVisible;
            set
            {
                isPlusVisible = value;
                OnPropertyChanged();
            }
        }

        private bool isMinusVisible = true;
        public bool IsMinusVisible
        {
            get => isMinusVisible;
            set
            {
                isMinusVisible = value;
                OnPropertyChanged();
            }
        }

        private bool isLabelVisible = false;
        public bool IsLabelVisible
        {
            get => isLabelVisible;
            set
            {
                isLabelVisible = value;
                OnPropertyChanged();
            }
        }

        private bool isPeriodVisible = false;
        public bool IsPeriodVisible
        {
            get => isPeriodVisible;
            set
            {
                isPeriodVisible = value;
                OnPropertyChanged();
            }
        }

        public EditViewModel(int savedIndex, ObservableCollection<Finance> listUI, Func<Task> saveData, string debtorStatus, Finance finance)
        {
            this.savedIndex = savedIndex;
            selectedPeriodIndex = savedIndex;
            ListUI = listUI;
            this.saveData = saveData;
            this.finance = finance;

            if (debtorStatus == "")
            {
                //isPlusVisible = false;
                //isMinusVisible = false;
                isPeriodVisible = true;
                isLabelVisible = true;
            }

            Name = listUI[0].Name;
            Amount = listUI[0].Amount.ToString(CultureInfo.CurrentCulture);

            EditCommand = new Command(async () => await Edit());
            DeleteCommand = new Command(async () => await Delete());

            PlusCommand = new Command(async () => await Plus());
            MinusCommand = new Command(async () => await Minus());

        }

        private async Task Minus()
        {
            Amount += " - ";
        }

        private async Task Plus()
        {
            Amount += " + ";
        }

        private async Task Edit()
        {
            periodNum = periodAmount(savedIndex, SelectedPeriodIndex);

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
                decimal.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal firstValue);
                decimal result = firstValue;

                for (int i = 1; i < parts.Length; i++)
                {
                    if (decimal.TryParse(parts[i].Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                    {
                        if (symbol == '+')
                        {
                            result += value;
                        }
                        else
                        {
                            result -= value;
                        }

                        finance.Name = Name;
                        finance.Amount = (positivePeriod) ? result * periodNum : result / periodNum;
                    }

                }
            }

            await saveData();

            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task Delete()
        {
            ListUI.Remove(finance);
            await saveData();
            await Shell.Current.Navigation.PopModalAsync();
        }

        public decimal periodAmount(int period, int newPeriod)
        {

            if (period > newPeriod)
            {
                positivePeriod = true;
                return recursAmount(period, newPeriod + 1);
            }
            else if (period < newPeriod)
            {
                positivePeriod = false;
                return recursAmount(newPeriod, period + 1);
            }

            positivePeriod = true;
            return 1m;
        }

        public decimal recursAmount(int period, int newPeriod)
        {

            if (period == newPeriod)
            {
                return PeriodsNum[period];
            }

            return PeriodsNum[period] * recursAmount(period - 1, newPeriod);
        }
    }
}
