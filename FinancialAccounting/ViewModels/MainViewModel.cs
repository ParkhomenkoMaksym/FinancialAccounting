using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FinancialAccounting.Models;
using FinancialAccounting.Services;
using FinancialAccounting.Views;

namespace FinancialAccounting.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        //FinanceData data = new FinanceData();
        private static int savedIndex;
        private static int newSavedIndex;
        private static decimal periodNum = 0;
        private static bool positivePeriod = true;

        private readonly FileServices fileService;
        //private decimal total;

        public ObservableCollection<Finance> Expenses { get; set; } = new();
        public ObservableCollection<Finance> Incomes { get; set; } = new();
        public ObservableCollection<Finance> Debtors { get; set; } = new();

        //int hour = 1, day = 8, week = 5, month = 4, sixMonths = 6, year = 2, fourYears = 4;

        public Dictionary<string, int> Periods { get; } = new Dictionary<string, int>()
        {
            {"hour", 1},
            {"day", 8},
            {"week", 5},
            {"month", 4},
            {"sixMonths", 6},
            {"year", 2},
            {"fourYears", 4}
        };

        //public string[] Periods { get; } =
        //[
        //    "hour",     // 1,
        //    "day",      // 8,
        //    "week",     // 5,
        //    "month",    // 4,
        //    "sixMonths",// 6,
        //    "year",     // 2,
        //    "fourYears" // 4 



        //];

        private int selectedPeriodIndex;

        public int SelectedPeriodIndex
        {
            get => selectedPeriodIndex;
            set
            {
                selectedPeriodIndex = value;
                OnPropertyChanged();
                UpdateTotals();
            }
        }

        private string total;

        public string Total
        {
            get => total;
            set
            {
                total = value;
                OnPropertyChanged();
            }
        }

        private string debtorTotal;

        public string DebtorTotal
        {
            get => debtorTotal;
            set
            {
                debtorTotal = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddIncomeCommand { get; }

        public MainViewModel()
        {
            fileService = new FileServices();

            LoadCommand = new Command(async () => await LoadData());

            _ = LoadData();

            AddIncomeCommand = new Command(async () => await AddIncome());
        }

        private async Task AddIncome()
        {
            await Shell.Current.GoToAsync(nameof(AddPage));
        }

        public async Task LoadData()
        {
            var data = await fileService.LoadDataAsync();

            Expenses.Clear();
            Incomes.Clear();
            Debtors.Clear();

            foreach(var item in data.Expenses) 
                Expenses.Add(item);

            foreach (var item in data.Incomes)
                Incomes.Add(item);

            foreach (var item in data.Debtors)
                Debtors.Add(item);

            SelectedPeriodIndex = data.SavedIndex;

            UpdateTotals();
        }

        public async Task SaveData()
        {
            FinanceData data = new()
            {
                Expenses = Expenses.ToList(),
                Incomes = Incomes.ToList(),
                Debtors = Debtors.ToList(),
                SavedIndex = SelectedPeriodIndex
            };

            await fileService.SaveDataAsync(data);
        }

        private void UpdateTotals()
        {
            decimal expensesSum = Expenses.Sum(x => x.Amount);
            decimal incomeSum = Incomes.Sum(x => x.Amount);
            decimal debtorSum = Debtors.Sum(x => x.Amount);

            Total = $"Total: {(incomeSum - expensesSum):F2}";
            DebtorTotal = $"Sum: {debtorSum:F2}";

        }

        public decimal periodAmount(int period, int newSavedIndex)
        {
 //           int hour = 1, day = 8, week = 5, month = 4, sixMonths = 6, year = 2, fourYears = 4;

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

            if (period < newSavedIndex)
            {
                positivePeriod = true;
                return recursAmount(newSavedIndex, period + 1);
            }
            else if (period > newSavedIndex)
            {
                positivePeriod = false;
                return recursAmount(period, newSavedIndex + 1);
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
