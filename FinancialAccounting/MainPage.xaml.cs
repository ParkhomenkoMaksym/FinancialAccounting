using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialAccounting
{
    public partial class MainPage : ContentPage
    {
        FinanceData data = new FinanceData();

        public MainPage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            btnExpensesCreator.Clear();
            btnIncomeCreator.Clear();
            
            data = await LoadDataAsync();

            decimal expensesSum = AddNameAndValue(btnExpensesCreator, data.Expenses);
            decimal incomeSum = AddNameAndValue(btnIncomeCreator, data.Income);

            btnTotal.Text = "Total: " + (incomeSum - expensesSum).ToString("F2");
        }

        public decimal AddNameAndValue(VerticalStackLayout mainContainer, List<Finance> list)
        {
            decimal amountSum = 0;

            foreach (var item in list)
            {
                amountSum += item.Amount;

                var mainGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition{ Width = GridLength.Star },
                        new ColumnDefinition{ Width = GridLength.Auto }

                    }
                };

                var lblName = new Label
                {
                    //VerticalOptions = LayoutOptions.Center,
                    Text = item.Name
                };

                var lblValue = new Label
                {
                    //VerticalOptions = LayoutOptions.Center,
                    Text = item.Amount.ToString("F2")
                };

                mainGrid.Add(lblName);
                Grid.SetColumn(lblName, 0);

                mainGrid.Add(lblValue);
                Grid.SetColumn(lblValue, 1);

                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) =>
                {
                    OnItemClicked(list, item, SaveDataAsync);
                };

                mainGrid.GestureRecognizers.Add(tapGesture);

                // Add each grid separately
                mainContainer.Add(mainGrid);
            }
            return amountSum;
        }

        private async void OnItemClicked(List<Finance> list, Finance data, Func<Task> saveDataAsync)
        {
            await Navigation.PushModalAsync(new EditDeletePage(list, data, saveDataAsync));
        }

        private async void btnExpenses_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddDataPage(btnExpensesCreator, data.Expenses, SaveDataAsync));
        }

        private async void btnIncome_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddDataPage(btnIncomeCreator, data.Income, SaveDataAsync));
        }

        public async Task SaveDataAsync()
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "finance.json");

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
            });

            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<FinanceData> LoadDataAsync()
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "finance.json");

            if(!File.Exists(filePath)) 
                return new FinanceData();

            var json = await File.ReadAllTextAsync(filePath);

            return JsonSerializer.Deserialize<FinanceData>(json) ?? new FinanceData();
        }

    }
}
