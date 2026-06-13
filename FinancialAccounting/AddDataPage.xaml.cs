
using System.Globalization;

namespace FinancialAccounting;

public partial class AddDataPage : ContentPage
{
    private int mainPeriod;
    private readonly VerticalStackLayout mainContainer;
    private List<Finance> list;
    private readonly Func<Task> saveAction;
    private static int savedIndex = 2;
    private static int newSavedIndex = 2;
    private static decimal periodNum = 0;
    private static bool positivePeriod = true;

    public AddDataPage(int period, VerticalStackLayout mainContainer, List<Finance> list, Func<Task> saveAction)
	{
		InitializeComponent();

        this.mainPeriod = period;
        this.mainContainer = mainContainer;
        this.list = list;
        this.saveAction = saveAction;

        periodPicker.Items.Add("hour");
        periodPicker.Items.Add("day");
        periodPicker.Items.Add("week");
        periodPicker.Items.Add("month");
        periodPicker.Items.Add("six months");
        periodPicker.Items.Add("year");
        periodPicker.Items.Add("4 years");

        periodPicker.SelectedIndex = savedIndex;

    }

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        periodNum = periodAmount(savedIndex, newSavedIndex);

        try
        {
            decimal amount = Decimal.Parse(value.Text, NumberStyles.Any, CultureInfo.CurrentCulture);
            
            amount = (positivePeriod) ? amount * periodNum : amount / periodNum;
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

    private void periodPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        newSavedIndex = periodPicker.SelectedIndex;
        //periodNum = periodAmount(savedIndex, newSavedIndex);
    }

    public decimal periodAmount(int period, int newPeriod)
    {
        int hour = 1, day = 8, week = 5, month = 4, sixMonths = 6, year = 2, fourYears = 4;

        Dictionary<int, int> formulas = new Dictionary<int, int>()
            {
                {0, hour},
                {1, day},
                {2, week},
                {3, month},
                {4, sixMonths},
                {5, year},
                {6, fourYears},
            };

        if (period > newSavedIndex)
        {
            positivePeriod = true;
            return recursAmount(period, newSavedIndex + 1, formulas);
        }
        else if (period < newSavedIndex)
        {
            positivePeriod = false;
            return recursAmount(newSavedIndex, period + 1, formulas);
        }

        positivePeriod = true;
        return 1m;
    }

    public decimal recursAmount(int period, int newPeriod, Dictionary<int, int> formulas)
    {

        if (period == newPeriod)
        {
            return formulas[period];
        }

        return formulas[period] * recursAmount(period - 1, newPeriod, formulas);
    }

}