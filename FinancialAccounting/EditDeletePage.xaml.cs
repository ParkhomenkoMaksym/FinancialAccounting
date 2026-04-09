using System.Globalization;

namespace FinancialAccounting;

public partial class EditDeletePage : ContentPage
{
	private List<Finance> list;
    private Finance item;
    private Func<Task> saveAction;

    public EditDeletePage(List<Finance> list, Finance item, Func<Task> saveAction)
	{
		InitializeComponent();

		this.list = list;
		this.item = item;
		this.saveAction = saveAction;

		DisplayInfoList();
    }

	public void DisplayInfoList()
	{
		lblName.Text = item.Name;
        lblAmount.Text = item.Amount.ToString("F2");

    }

    private async void btnEdit_Clicked(object sender, EventArgs e)
    {
        //lblAmount.Text += " " + symbol + " ";
        char symbol = lblAmount.Text.Contains('-') ? '-' : '+';

        var parts = lblAmount.Text.Split(symbol);

        for (int i = 1; i < parts.Length; i++)
        {
            if (decimal.TryParse(parts[i].Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
            {
                if (symbol == '+')
                {
                    item.Amount += value;
                }
                else
                {
                    item.Amount -= value;
                }


            }

        }
            //decimal total = 0;

            
            //foreach (var part in parts)
            //{
            //    if (decimal.TryParse(part.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
            //    {
            //        if (symbol == '+')
            //        {
            //            total += value;
            //        }
            //        else
            //        {
            //            total -= value;
            //        }
            //    }
            //}

            //total = Math.Round(total, 2, MidpointRounding.AwayFromZero);
            //item.Amount = value;

            await saveAction();

        await Navigation.PopModalAsync();

    }

    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        list.Remove(item);
        await saveAction();
        await Navigation.PopModalAsync();
    }

    private void btnPlus_Clicked(object sender, EventArgs e)
    {
        lblAmount.Text += " + ";
    }

    private void btnMinus_Clicked(object sender, EventArgs e)
    {
        lblAmount.Text += " - ";
    }

    //public async void AddNumbers(char symbol, decimal amount)
    //{
    //    try
    //    {
    //        //item.Name = lblName.Text + ": ";

    //        lblAmount.Text += " " + symbol + " ";

    //        var parts = lblAmount.Text.Split(symbol);

    //        //decimal total = 0;

    //        foreach (var part in parts)
    //        {
    //            if (decimal.TryParse(part.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
    //            {
    //                if (symbol == '+')
    //                {
    //                    amount += value;
    //                }
    //                else
    //                {
    //                    amount -= value;
    //                }
    //            }   
    //        }

    //        total = Math.Round(total, 2, MidpointRounding.AwayFromZero);
    //        item.Amount = total;
    //        await saveAction();
    //    }
    //    catch
    //    {
    //        await DisplayAlert("Error", "Invalid number", "OK");
    //    }
    //}
}