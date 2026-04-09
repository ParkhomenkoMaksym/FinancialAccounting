
using System.Globalization;

namespace FinancialAccounting;

public partial class AddDataPage : ContentPage
{
    private readonly VerticalStackLayout mainContainer;
    private List<Finance> list;
    private readonly Func<Task> saveAction;

	public AddDataPage(VerticalStackLayout mainContainer, List<Finance> list, Func<Task> saveAction)
	{
		InitializeComponent();

        this.mainContainer = mainContainer;
        this.list = list;
        this.saveAction = saveAction;
	}

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        try
        {
            decimal amount = Decimal.Parse(value.Text, NumberStyles.Any, CultureInfo.CurrentCulture);
            amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
            Finance item = new Finance(name.Text + ": ", amount);
            list.Add(item);
            await saveAction();
        }
        catch
        {
            await DisplayAlert("Error", "Invalid number", "OK");
        }
        
        
        

        await Navigation.PopModalAsync();
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}