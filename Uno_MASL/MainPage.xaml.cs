using Microsoft.Identity.Client;
using Uno.UI.MSAL;
using Windows.UI.Popups;

namespace Uno_MASL
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] scopes = { "user.read" };

            try
            {
                AuthenticationResult result = await App
                   .Instance
                   .ClientApplication
                   .AcquireTokenInteractive(scopes)
                   .WithUnoHelpers()
                   .ExecuteAsync();

                if (!string.IsNullOrEmpty(result.AccessToken))
                {
                    App.Instance.MainFrame!.Navigate(typeof(AuthenticatedPage));
                }
            }
            catch (Exception ex)
            {
                DispatcherQueue.TryEnqueue(async () =>
                {
                    var dialog = new MessageDialog(content: string.Empty);
                    dialog.Title = "Error while login";
                    dialog.Content = ex.Message;

#if WINDOWS
                    WinRT.Interop.InitializeWithWindow.Initialize(dialog, WinRT.Interop.WindowNative.GetWindowHandle(App.Instance.MainWindow));
#endif

                    await dialog.ShowAsync().AsTask();
                });
            }
        }
    }
}
