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
    public class AddViewModel : BaseViewModel
    {
        private static int savedIndex;
        private static int newSavedIndex;
        private static decimal periodNum = 0;
        private static bool positivePeriod = true;
        private Func<Task> saveAction;

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

        public ObservableCollection<Finance> List { get; set; } = new();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        //public event PropertyChangedEventHandler? PropertyChanged;

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

        // ObservableCollection<Finance> list, Func<Task> saveAction
        public AddViewModel()
        {
            //this.List = list;
            //this.saveAction = saveAction;

            SaveCommand = new Command(async () => await Save());
            CancelCommand = new Command(async () => await Cancel());
        }

        private async Task Save()
        {
            periodNum = periodAmount(savedIndex, newSavedIndex);

            try
            {
                decimal parseAmount = decimal.Parse(Amount, NumberStyles.Any, CultureInfo.CurrentCulture);

                parseAmount = (positivePeriod) ? parseAmount * periodNum : parseAmount / periodNum;
                parseAmount = Math.Round(parseAmount, 2, MidpointRounding.AwayFromZero);

                Finance finance = new Finance(Name + ": ", parseAmount);
                List.Add(finance);

                await saveAction();

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Invalid number", "OK");
            }

            //await Navigation.PopModalAsync();
        }

        private async Task Cancel()
        {
            //await Navigation.PopModalAsync();
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
