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
            decimal debtorColumnSum = RenderDebtors(mainDebtorGrid, data.Debtors);

            btnTotal.Text = "Total: " + (incomeSum - expensesSum).ToString("F2");
            btnTotalDebtor.Text = "Sum: " + debtorColumnSum.ToString("F2");
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

            var addLabel = new Label
            {
                Text = (mainContainer == btnIncomeCreator) ? "Add Income +" : "Add Expenses +",
                TextColor = Colors.Gray
            };

            var addTap = new TapGestureRecognizer();
            addTap.Tapped += (s, e) =>
            {
                OnItemClicked(mainContainer, list, SaveDataAsync);
            };

            addLabel.GestureRecognizers.Add(addTap);

            int addCol = list.Count % 2;

            Grid.SetRow(addLabel, 0);
            Grid.SetColumn(addLabel, addCol);

            mainContainer.Children.Add(addLabel);

            return amountSum;
        }

        private decimal RenderDebtors(Grid grid, List<Finance> list)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();

            decimal sum = 0;
            int row = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 0)
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }

                var item = list[i];
                sum += item.Amount;

                var itemGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto }
                    }
                };

                var lblName = new Label { Text = item.Name };
                var lblValue = new Label { Text = item.Amount.ToString("F2") };

                itemGrid.Add(lblName, 0, 0);
                itemGrid.Add(lblValue, 1, 0);

                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) =>
                {
                    OnItemClicked(list, item, SaveDataAsync);
                };

                itemGrid.GestureRecognizers.Add(tap);

                int col = i % 2;

                Grid.SetRow(itemGrid, row);
                Grid.SetColumn(itemGrid, col);

                grid.Children.Add(itemGrid);

                if (col == 1)
                    row++;
            }

            // 🔥 Add "Add Debtor +"
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var addLabel = new Label
            {
                Text = "Add Debtor +",
                TextColor = Colors.Gray
            };

            var addTap = new TapGestureRecognizer();
            addTap.Tapped += (s, e) =>
            {
                OnItemClicked(grid, list, SaveDataAsync);
            };

            addLabel.GestureRecognizers.Add(addTap);

            int addCol = list.Count % 2;

            Grid.SetRow(addLabel, row);
            Grid.SetColumn(addLabel, addCol);

            grid.Children.Add(addLabel);

            return sum;
        }

        private async void OnItemClicked(List<Finance> list, Finance data, Func<Task> saveDataAsync)
        {
            await Navigation.PushModalAsync(new EditDeletePage(list, data, saveDataAsync));
        }

        private async void OnItemClicked(Grid mainGrid, List<Finance> list, Func<Task> saveDataAsync)
        {
            await Navigation.PushModalAsync(new AddDataPage(null, list, SaveDataAsync));
        }

        private async void OnItemClicked(VerticalStackLayout mainContainer, List<Finance> list, Func<Task> saveDataAsync)
        {
            await Navigation.PushModalAsync(new AddDataPage(mainContainer, list, SaveDataAsync));
        }

        //private async void btnExpenses_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushModalAsync(new AddDataPage(btnExpensesCreator, data.Expenses, SaveDataAsync));
        //}

        //private async void btnIncome_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushModalAsync(new AddDataPage(btnIncomeCreator, data.Income, SaveDataAsync));
        //}

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
