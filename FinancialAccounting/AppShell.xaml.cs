using FinancialAccounting.Views;

namespace FinancialAccounting
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddPage), typeof(AddPage));
        }
    }
}
