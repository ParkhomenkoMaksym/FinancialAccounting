using FinancialAccounting.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace FinancialAccounting.Views;

public partial class EditPage : ContentPage
{
	//private List<Finance> list;
 //   private Finance item;
 //   private Func<Task> saveAction;
 //   private int savedIndex;
 //   private static int newSavedIndex = 2;
 //   private static decimal periodNum = 0;
 //   private static bool positivePeriod = true;

    public EditPage()
	{
		InitializeComponent();

        //int period, ObservableCollection< Finance > list, Finance finance, string debtorStatus, Func< Task > saveAction
        BindingContext = new EditViewModel();

  //      this.savedIndex = period;
		//this.list = list;
		//this.item = item;
		//this.saveAction = saveAction;

  //      periodPicker.Items.Add("hour");
  //      periodPicker.Items.Add("day");
  //      periodPicker.Items.Add("week");
  //      periodPicker.Items.Add("month");
  //      periodPicker.Items.Add("six months");
  //      periodPicker.Items.Add("year");
  //      periodPicker.Items.Add("4 years");

  //      periodPicker.SelectedIndex = savedIndex;

  //      if (debtorStatus == "")
  //      {
  //          btnPlus.IsVisible = false;
  //          btnMinus.IsVisible = false;
  //      }

  //      DisplayInfoList();
    }

	//public void DisplayInfoList()
	//{
	//	lblName.Text = item.Name;
 //       lblAmount.Text = item.Amount.ToString("F2");

 //   }

 //   private async void btnEdit_Clicked(object sender, EventArgs e)
 //   {
 //       periodNum = periodAmount(savedIndex, newSavedIndex);
 //       //lblAmount.Text += " " + symbol + " ";
 //       char symbol = lblAmount.Text.Contains('-') ? '-' : '+';

 //       var parts = lblAmount.Text.Split(symbol);

 //       if(parts.Length == 1)
 //       {
 //           if (decimal.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
 //           {
 //               item.Name = lblName.Text;
 //               item.Amount = (positivePeriod) ? value * periodNum : value / periodNum;
 //           }
 //       }
 //       else
 //       {
 //           for (int i = 1; i < parts.Length; i++)
 //           {
 //               if (decimal.TryParse(parts[i].Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
 //               {
 //                   if (symbol == '+')
 //                   {
 //                       item.Name = lblName.Text;
 //                       item.Amount += (positivePeriod) ? value * periodNum : value / periodNum;
 //                   }
 //                   else
 //                   {
 //                       item.Name = lblName.Text;
 //                       item.Amount -= (positivePeriod) ? value * periodNum : value / periodNum;
 //                   }
 //               }

 //           }
 //       }
       
 //       await saveAction();

 //       await Navigation.PopModalAsync();

 //   }

 //   private async void btnDelete_Clicked(object sender, EventArgs e)
 //   {
 //       list.Remove(item);
 //       await saveAction();
 //       await Navigation.PopModalAsync();
 //   }

 //   private void btnPlus_Clicked(object sender, EventArgs e)
 //   {
 //       lblAmount.Text += " + ";
 //   }

 //   private void btnMinus_Clicked(object sender, EventArgs e)
 //   {
 //       lblAmount.Text += " - ";
 //   }

 //   public decimal periodAmount(int period, int newPeriod)
 //   {
 //       int hour = 1, day = 8, week = 5, month = 4, sixMonths = 6, year = 2, fourYears = 4;

 //       Dictionary<int, int> formulas = new Dictionary<int, int>()
 //           {
 //               {0, hour},
 //               {1, day},
 //               {2, week},
 //               {3, month},
 //               {4, sixMonths},
 //               {5, year},
 //               {6, fourYears},
 //           };

 //       if (period > newSavedIndex)
 //       {
 //           positivePeriod = true;
 //           return recursAmount(period, newSavedIndex + 1, formulas);
 //       }
 //       else if (period < newSavedIndex)
 //       {
 //           positivePeriod = false;
 //           return recursAmount(newSavedIndex, period + 1, formulas);
 //       }

 //       positivePeriod = true;
 //       return 1m;

 //   }

 //   public decimal recursAmount(int period, int newPeriod, Dictionary<int, int> formulas)
 //   {

 //       if (period == newPeriod)
 //       {
 //           return formulas[period];
 //       }

 //       return formulas[period] * recursAmount(period - 1, newPeriod, formulas);
 //   }

 //   private void periodPicker_SelectedIndexChanged(object sender, EventArgs e)
 //   {
 //       newSavedIndex = periodPicker.SelectedIndex;
 //       //periodNum = periodAmount(savedIndex, newSavedIndex);
 //   }
}