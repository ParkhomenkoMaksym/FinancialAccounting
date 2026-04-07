using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialAccounting
{
    public partial class MainPage : ContentPage
    {
        private Dictionary<string, string> _expenssesList = new Dictionary<string, string>();
        private Dictionary<string, string> _incomeList = new Dictionary<string, string>();

        public MainPage()
        {
            InitializeComponent();

            //NameAndValueCreator(btnExpenssesCreator, "Some Person:", "40");
            //NameAndValueCreator(btnIncomeCreator, "Salary:", "40");
        }

        public void UpdateGrid(VerticalStackLayout mainContainer, Dictionary<string, string> list)
        {
            var mainGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition{ Width = GridLength.Star },
                    new ColumnDefinition{ Width = GridLength.Auto }
                }   
            };

            foreach (var item in list)
            {
                var btnName = new Button
                {
                    BackgroundColor = Colors.White,
                    TextColor = Colors.Black,
                    Padding = 0,
                    HorizontalOptions = LayoutOptions.Start,
                    Text = item.Key
                };

                var lblValue = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    Text = item.Value
                };

                mainGrid.SetColumn(btnName, 0);
                mainGrid.Add(btnName);

                mainGrid.SetColumn(lblValue, 1);
                mainGrid.Add(lblValue);
            }

            mainContainer.Add(mainGrid);
        }

        //public void AddNameAndValue(VerticalStackLayout mainContainer, Dictionary<string, string> list)
        //{
        //    var list = 
        //    if (mainContainer.Equals(btnExpenssesCreator))
        //    {
        //        _expenssesList.Add(name, value);
        //        UpdateGrid(mainContainer, _expenssesList);
        //    } else
        //    {
        //        _incomeList.Add(name, value);
        //        UpdateGrid(mainContainer, _expenssesList);
        //    }

        //    UpdateGrid(mainContainer, _expenssesList);

        //}

        public void DeleteNameAndValue()
        {

        }

        private async void btnExpensses_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PromptWindow(btnExpenssesCreator, _expenssesList));
        }

        private async void btnIncome_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PromptWindow(btnIncomeCreator, _incomeList));
        }
    }
}
