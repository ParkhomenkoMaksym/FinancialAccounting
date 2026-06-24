using FinancialAccounting.Models;
using FinancialAccounting.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FinancialAccounting.Views;

public partial class AddPage : ContentPage
{

    public AddPage(int savedIndex, ObservableCollection<Finance> listUI, string debtorStatus, Func<Task> saveData)
	{
		InitializeComponent();

        BindingContext = new AddViewModel(savedIndex, listUI, debtorStatus, saveData);
    }

}