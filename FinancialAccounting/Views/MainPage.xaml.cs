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

        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel();
        }

    }
}
