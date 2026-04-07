
namespace FinancialAccounting;

public partial class PromptWindow : ContentPage
{
    private readonly VerticalStackLayout mainContainer;
    private Dictionary<string, string> list;

	public PromptWindow(VerticalStackLayout mainContainer, Dictionary<string, string> list)
	{
		InitializeComponent();

        this.mainContainer = mainContainer;
        this.list = list;
	}

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        list.Add(name.Text, value.Text);
        MainPage mainPage = new MainPage();
        mainPage.UpdateGrid(mainContainer,list);

        await Navigation.PopModalAsync();
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}