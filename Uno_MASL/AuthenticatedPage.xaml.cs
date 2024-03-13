using Microsoft.Identity.Client;

namespace Uno_MASL
{
    public sealed partial class AuthenticatedPage : Page
    {
        public AuthenticatedPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = App.Instance.ClientApplication;
            IEnumerable<IAccount> accounts = await client.GetAccountsAsync();
            while (accounts.Any())
            {
                //Remove account from local cache
                await client.RemoveAsync(accounts.FirstOrDefault());
                accounts = await client.GetAccountsAsync();
            }

            App.Instance.MainFrame.GoBack();
        }
    }
}
