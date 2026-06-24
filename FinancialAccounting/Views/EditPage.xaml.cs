using FinancialAccounting.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using FinancialAccounting.Models;

namespace FinancialAccounting.Views;

public partial class EditPage : ContentPage
{

    public EditPage(int savedIndex, ObservableCollection<Finance> listUI, Func<Task> saveData, string debtorStatus, Finance finance)
    {
        InitializeComponent();

        BindingContext = new EditViewModel(savedIndex, listUI, saveData, debtorStatus, finance);
    }
}