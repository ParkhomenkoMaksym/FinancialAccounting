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
using FinancialAccounting.Services;

namespace FinancialAccounting.ViewModels
{
    public class AddViewModel : BaseViewModel
    {
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

        public ObservableCollection<Finance> ListUI { get; set; }

        public FinanceData Data { get; set; } = new FinanceData();
        public readonly Func<Task> saveData;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

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

        private string title;

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

        public AddViewModel(int savedIndex, ObservableCollection<Finance> listUI, string debtorStatus, Func<Task> saveData)
        {
            this.savedIndex = savedIndex;
            selectedPeriodIndex = savedIndex;
            ListUI = listUI;
            this.saveData = saveData;

            if (debtorStatus == "")
            {
                //isPlusVisible = false;
                //isMinusVisible = false;
                isPeriodVisible = true;
                isLabelVisible = true;
            }

            SaveCommand = new Command(async () => await Save());
            CancelCommand = new Command(async () => await Cancel());
        }

        private async Task Save()
        {
            periodNum = periodAmount(savedIndex, SelectedPeriodIndex);

            try
            {
                decimal parseAmount = decimal.Parse(Amount, NumberStyles.Any, CultureInfo.CurrentCulture);

                parseAmount = (positivePeriod) ? parseAmount * periodNum : parseAmount / periodNum;
                parseAmount = Math.Round(parseAmount, 2, MidpointRounding.AwayFromZero);

                Finance finance = new Finance(Name + ": ", parseAmount);

                ListUI.Add(finance);

                await saveData();

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Invalid number", "OK");
            }

        }

        private async Task Cancel()
        {
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
