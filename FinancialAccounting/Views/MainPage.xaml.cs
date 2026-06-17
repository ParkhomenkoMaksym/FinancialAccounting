using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FinancialAccounting.ViewModels;

namespace FinancialAccounting.Views
{
    public partial class MainPage : ContentPage
    {
        //FinanceData data = new FinanceData();
        //private static int savedIndex;
        //private static int newSavedIndex;
        //private static decimal periodNum = 0;
        //private static bool positivePeriod = true;


        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel();
        }

    //    protected override async void OnAppearing()
    //    {
    //        base.OnAppearing();

    //        //btnExpensesCreator.Clear();
    //        //btnIncomeCreator.Clear();
            
    //        data = await LoadDataAsync();

    //        if(periodPicker.Items.Count == 0)
    //        {
    //            periodPicker.Items.Add("hour");
    //            periodPicker.Items.Add("day");
    //            periodPicker.Items.Add("week");
    //            periodPicker.Items.Add("month");
    //            periodPicker.Items.Add("six months");
    //            periodPicker.Items.Add("year");
    //            periodPicker.Items.Add("4 years");
    //        }

    //        savedIndex = data.SavedIndex;
    //        periodPicker.SelectedIndex = savedIndex;
    //        updatePeriod();
    //        //periodPicker.SelectedIndexChanged += periodPicker_SelectedIndexChanged;
    //        //updatePeriod();
    //    }

    //    public decimal AddNameAndValue(VerticalStackLayout mainContainer, List<Finance> list)
    //    {
    //        mainContainer.Children.Clear();
            
    //        decimal amountSum = 0;

    //        foreach (var item in list)
    //        {
    //            //amountSum += item.Amount;

    //            var mainGrid = new Grid
    //            {
    //                ColumnDefinitions =
    //                {
    //                    new ColumnDefinition{ Width = GridLength.Star },
    //                    new ColumnDefinition{ Width = GridLength.Auto }

    //                }
    //            };

    //            var lblName = new Label
    //            {
    //                //VerticalOptions = LayoutOptions.Center,
    //                Text = item.Name
    //            };

    //            //item.Amount = periodAmount(savedIndex);
    //            item.Amount = (positivePeriod)? item.Amount * periodNum : item.Amount / periodNum;
    //            amountSum += item.Amount;

    //            var lblValue = new Label
    //            {
    //                //VerticalOptions = LayoutOptions.Center,
    //                Text = item.Amount.ToString("F2")
    //            };

    //            mainGrid.Add(lblName);
    //            Grid.SetColumn(lblName, 0);

    //            mainGrid.Add(lblValue);
    //            Grid.SetColumn(lblValue, 1);

    //            var tapGesture = new TapGestureRecognizer();
    //            tapGesture.Tapped += (s, e) =>
    //            {
    //                OnItemClicked(list, item, "", SaveDataAsync);
    //            };

    //            mainGrid.GestureRecognizers.Add(tapGesture);

    //            // Add each grid separately
    //            mainContainer.Add(mainGrid);
    //        }

    //        var addLabel = new Label
    //        {
    //            Text = (mainContainer == btnIncomeCreator) ? "Add Income +" : "Add Expenses +",
    //            TextColor = Colors.Gray
    //        };

    //        var addTap = new TapGestureRecognizer();
    //        addTap.Tapped += (s, e) =>
    //        {
    //            OnItemClicked(mainContainer, list, SaveDataAsync);
    //        };

    //        addLabel.GestureRecognizers.Add(addTap);

    //        int addCol = list.Count % 2;

    //        Grid.SetRow(addLabel, 0);
    //        Grid.SetColumn(addLabel, addCol);

    //        mainContainer.Children.Add(addLabel);

    //        return amountSum;
    //    }

    //    private decimal RenderDebtors(Grid grid, List<Finance> list)
    //    {
    //        grid.Children.Clear();
    //        grid.RowDefinitions.Clear();

    //        decimal sum = 0;
    //        int row = 0;

    //        for (int i = 0; i < list.Count; i++)
    //        {
    //            if (i % 2 == 0)
    //            {
    //                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
    //            }

    //            var item = list[i];
    //            sum += item.Amount;

    //            var itemGrid = new Grid
    //            {
    //                ColumnDefinitions =
    //                {
    //                    new ColumnDefinition { Width = GridLength.Star },
    //                    new ColumnDefinition { Width = GridLength.Auto }
    //                }
    //            };

    //            var lblName = new Label { Text = item.Name };
    //            var lblValue = new Label { Text = item.Amount.ToString("F2") };

    //            itemGrid.Add(lblName, 0, 0);
    //            itemGrid.Add(lblValue, 1, 0);

    //            var tap = new TapGestureRecognizer();
    //            tap.Tapped += (s, e) =>
    //            {
    //                OnItemClicked(list, item, "debtor", SaveDataAsync);
    //            };

    //            itemGrid.GestureRecognizers.Add(tap);

    //            int col = i % 2;

    //            Grid.SetRow(itemGrid, row);
    //            Grid.SetColumn(itemGrid, col);

    //            grid.Children.Add(itemGrid);

    //            if (col == 1)
    //                row++;
    //        }

    //        // 🔥 Add "Add Debtor +"
    //        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

    //        var addLabel = new Label
    //        {
    //            Text = "Add Debtor +",
    //            TextColor = Colors.Gray
    //        };

    //        var addTap = new TapGestureRecognizer();
    //        addTap.Tapped += (s, e) =>
    //        {
    //            OnItemClicked(grid, list, SaveDataAsync);
    //        };

    //        addLabel.GestureRecognizers.Add(addTap);

    //        int addCol = list.Count % 2;

    //        Grid.SetRow(addLabel, row);
    //        Grid.SetColumn(addLabel, addCol);

    //        grid.Children.Add(addLabel);

    //        return sum;
    //    }

    //    private async void OnItemClicked(List<Finance> list, Finance data, string debtorStatus, Func<Task> saveDataAsync)
    //    {
    //        await Navigation.PushModalAsync(new EditDeletePage(savedIndex, list, data, debtorStatus, saveDataAsync));
    //        //periodPicker.Items.Clear();
    //    }

    //    private async void OnItemClicked(Grid mainGrid, List<Finance> list, Func<Task> saveDataAsync)
    //    {
    //        await Navigation.PushModalAsync(new AddDataPage(savedIndex, null, list, SaveDataAsync));
    //        //periodPicker.Items.Clear();
    //    }

    //    private async void OnItemClicked(VerticalStackLayout mainContainer, List<Finance> list, Func<Task> saveDataAsync)
    //    {
    //        await Navigation.PushModalAsync(new AddDataPage(savedIndex, mainContainer, list, SaveDataAsync));
    //        //periodPicker.Items.Clear();
    //    }

    //    private void periodPicker_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        newSavedIndex = periodPicker.SelectedIndex; 
    //        periodNum = periodAmount(savedIndex, newSavedIndex);
    //        updatePeriod();
    //    }

    //    public void updatePeriod()
    //    {
    //        decimal expensesSum = AddNameAndValue(btnExpensesCreator, data.Expenses);
    //        decimal incomeSum = AddNameAndValue(btnIncomeCreator, data.Incomes);
    //        decimal debtorColumnSum = RenderDebtors(mainDebtorGrid, data.Debtors);

    //        btnTotal.Text = "Total: " + (incomeSum - expensesSum).ToString("F2");
    //        btnTotalDebtor.Text = "Sum: " + debtorColumnSum.ToString("F2");

    //        savedIndex = newSavedIndex;
    //        periodNum = 1;
    //    }

    //    public decimal periodAmount(int period, int newSavedIndex)
    //    {
    //        int hour = 1, day = 8, week = 5, month = 4, sixMonths = 6, year = 2, fourYears = 4;

    //        Dictionary<int, int> formulas = new Dictionary<int, int>()
    //        {
    //            {0, hour},
    //            {1, day},
    //            {2, week},
    //            {3, month},
    //            {4, sixMonths},
    //            {5, year},
    //            {6, fourYears},
    //        };

    //        if (period < newSavedIndex)
    //        {
    //            positivePeriod = true;
    //            return recursAmount(newSavedIndex, period + 1, formulas);
    //        }
    //        else if (period > newSavedIndex)
    //        {
    //            positivePeriod = false;
    //            return recursAmount(period, newSavedIndex + 1, formulas);
    //        }

    //        positivePeriod = true;
    //        return 1m;
    //    }

    //    public decimal recursAmount(int period, int newPeriod, Dictionary<int, int> formulas)
    //    {

    //        if (period == newPeriod)
    //        {
    //            return formulas[period];
    //        }

    //        return formulas[period] * recursAmount(period - 1, newPeriod, formulas);
    //    }

    //    //public async Task SaveDataAsync()
    //    //{
    //    //    data.SavedIndex = savedIndex;

    //    //    string filePath = Path.Combine(FileSystem.AppDataDirectory, "finance.json");

    //    //    var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
    //    //    {
    //    //        WriteIndented = true,
    //    //    });

    //    //    await File.WriteAllTextAsync(filePath, json);
    //    //}

    //    //public async Task<FinanceData> LoadDataAsync()
    //    //{
    //    //    string filePath = Path.Combine(FileSystem.AppDataDirectory, "finance.json");

    //    //    if(!File.Exists(filePath)) 
    //    //        return new FinanceData();

    //    //    var json = await File.ReadAllTextAsync(filePath);

    //    //    return JsonSerializer.Deserialize<FinanceData>(json) ?? new FinanceData();
    //    //}

    }
}
