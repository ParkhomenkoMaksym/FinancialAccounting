using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using Android.Content.Res;
using FinancialAccounting.Models;
using FinancialAccounting.Services;
using FinancialAccounting.Views;

namespace FinancialAccounting.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Last saved period index used for recalculating values.
        private static int savedIndex;
        private static decimal periodNum = 0;
        private static bool positivePeriod = true;

        private ObservableCollection<Finance> expenses  = new();
        public ObservableCollection<Finance> Expenses 
        { 
            get => expenses;
            set 
            {
                expenses = value;
                OnPropertyChanged();
            } 
        }

        private ObservableCollection<Finance> incomes = new();
        public ObservableCollection<Finance> Incomes
        {
            get => incomes;
            set
            {
                incomes = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Finance> debtors = new();
        public ObservableCollection<Finance> Debtors
        {
            get => debtors;
            set
            {
                debtors = value;
                OnPropertyChanged();
            }
        }

        // Picker items shown in the UI.
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

        // Multipliers used to convert values between periods.
        public int[] PeriodsNum { get; } = new int[]
        {
            1, 8, 5, 4, 6, 2, 4
        };

        // Prevents recalculation before the initial data load completes.
        private bool startUI = false;

        private int selectedPeriodIndex;

        public int SelectedPeriodIndex
        {
            get => selectedPeriodIndex;
            set
            {
                selectedPeriodIndex = value;
                OnPropertyChanged();

                // Recalculate totals after the user changes the period.
                if(startUI) UpdateTotals();
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

        private Finance selectedExpense;
        public Finance SelectedExpense
        {
            get => selectedExpense;
            set
            {
                selectedExpense = value;
                OnPropertyChanged();
                EditExpense(selectedExpense);
            }
        }

        private Finance selectedIncome;
        public Finance SelectedIncome
        {
            get => selectedIncome;
            set
            {
                selectedIncome = value;
                OnPropertyChanged();
                EditIncome(selectedIncome);
            }
        }

        private Finance selectedDebtor;
        public Finance SelectedDebtor
        {
            get => selectedDebtor;
            set
            {
                selectedDebtor = value;
                OnPropertyChanged();
                EditDebtor(selectedDebtor);
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddIncomeCommand { get; }
        public ICommand AddExpenseCommand { get; }
        public ICommand AddDebtorCommand { get; }
        public ICommand EditIncomeCommand { get; }
        public ICommand EditExpenseCommand { get; }
        public ICommand EditDebtorCommand { get; }

        public MainViewModel()
        {
            // Load saved data when the view model is created.
            LoadCommand = new Command(async () => await LoadData());
            _ = LoadData();

            AddIncomeCommand = new Command(async () => await AddIncome());
            AddExpenseCommand = new Command(async () => await AddExpense());
            AddDebtorCommand = new Command(async () => await AddDebtor());

            EditIncomeCommand = new Command(async () => await EditIncome(SelectedIncome));
            EditExpenseCommand = new Command(async () => await EditExpense(SelectedExpense));
            EditDebtorCommand = new Command(async () => await EditDebtor(SelectedDebtor));
        }

        private async Task AddExpense()
        {
            await Application.Current.MainPage.Navigation
                .PushModalAsync(new AddPage(SelectedPeriodIndex, Expenses, "", SaveData));
        }

        private async Task AddIncome()
        {
            await Application.Current.MainPage.Navigation
                .PushModalAsync(new AddPage(SelectedPeriodIndex, Incomes, "", SaveData));
        }

        private async Task AddDebtor()
        {
            await Application.Current.MainPage.Navigation
                .PushModalAsync(new AddPage(SelectedPeriodIndex, Debtors, "debtor", SaveData));
        }

        private async Task EditExpense(Finance finance)
        {
            await Application.Current.MainPage.Navigation
                .PushModalAsync(new EditPage(SelectedPeriodIndex, Expenses, SaveData, "", finance));
        }

        private async Task EditIncome(Finance finance)
        {
            await Application.Current.MainPage.Navigation
                .PushModalAsync(new EditPage(SelectedPeriodIndex, Incomes, SaveData, "", finance));
        }

        private async Task EditDebtor(Finance finance)
        {
            await Application.Current.MainPage.Navigation
               .PushModalAsync(new EditPage(SelectedPeriodIndex, Debtors, SaveData, "debtor", finance));
        }

        public async Task LoadData()
        {
            // Read persisted values and populate the collections.
            var data = await FileServices.LoadDataAsync();
            periodNum = periodAmount(savedIndex, SelectedPeriodIndex);

            Expenses.Clear();
            Incomes.Clear();
            Debtors.Clear();

            foreach (var item in data.Expenses)
                Expenses.Add(item);

            foreach (var item in data.Incomes)
                Incomes.Add(item);

            foreach (var item in data.Debtors)
                Debtors.Add(item);

            // Update the summary labels.
            decimal expensesSum = Expenses.Sum(x => x.Amount);
            decimal incomeSum = Incomes.Sum(x => x.Amount);
            decimal debtorSum = Debtors.Sum(x => x.Amount);

            Total = $"Total: {(incomeSum - expensesSum):F2}";
            DebtorTotal = $"Sum: {debtorSum:F2}";

            savedIndex = data.SavedIndex;
            SelectedPeriodIndex = savedIndex;

            startUI = true;
        }

        public async Task SaveData()
        {
            // Save the current collections and selected period.
            FinanceData data = new()
            {
                Expenses = Expenses.ToList(),
                Incomes = Incomes.ToList(),
                Debtors = Debtors.ToList(),
                SavedIndex = SelectedPeriodIndex
            };

            await FileServices.SaveDataAsync(data);
        }

        // Recalculate all amounts for the newly selected period.
        private async void UpdateTotals()
        {
            periodNum = periodAmount(savedIndex, SelectedPeriodIndex);

            Expenses = updateData(Expenses);

            Incomes = updateData(Incomes);

            decimal expensesSum = Expenses.Sum(x => x.Amount);
            decimal incomeSum = Incomes.Sum(x => x.Amount);
            decimal debtorSum = Debtors.Sum(x => x.Amount);

            Total = $"Total: {(incomeSum - expensesSum):F2}";
            DebtorTotal = $"Sum: {debtorSum:F2}";

            savedIndex = SelectedPeriodIndex;

            await SaveData();
        }

        // Returns the conversion factor between two period indexes.
        public decimal periodAmount(int period, int newSavedIndex)
        {

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

        // Multiplies the period values recursively.
        public decimal recursAmount(int period, int newPeriod)
        {

            if (period == newPeriod)
            {
                return PeriodsNum[period];
            }

            return PeriodsNum[period] * recursAmount(period - 1, newPeriod);
        }

        // Applies the calculated conversion factor to each item.
        public ObservableCollection<Finance> updateData(ObservableCollection<Finance> finances)
        {

            decimal sum = 0;

            foreach (var item in finances)
            {
                item.Amount = (positivePeriod) ? item.Amount * periodNum : item.Amount / periodNum;

            }

            return finances;
        }


    }
}
