namespace FinancialAccounting
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            ButtonAndLabelCreator(btnExpenssesCreator, "Some Person:", "40");
            ButtonAndLabelCreator(btnIncomeCreator, "Salary:", "40");
        }

        public static void ButtonAndLabelCreator(VerticalStackLayout mainContainer, String name, String value)
        {

            var mainGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition{ Width = GridLength.Star },
                    new ColumnDefinition{ Width = GridLength.Auto }
                }   
            };

            var btnName = new Button
            {
                BackgroundColor = Colors.White,
                TextColor = Colors.Black,
                Padding = 0,
                HorizontalOptions = LayoutOptions.Start,
                Text = name
            };

            var lblName = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                Text = value
            };

            mainGrid.SetColumn(btnName, 0);
            mainGrid.Add(btnName);

            mainGrid.SetColumn(lblName, 1);
            mainGrid.Add(lblName);

            mainContainer.Add(mainGrid);
        }

        private void BtnExpensses_Clicked(object sender, EventArgs e)
        {
            
        }

        private void BtnIncome_Clicked(object sender, EventArgs e)
        {

        }
    }
}
