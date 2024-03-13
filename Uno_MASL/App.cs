using Microsoft.Identity.Client;
using Uno.UI.MSAL;

namespace Uno_MASL
{
    public class App : Application
    {
        private const string _clientId = "APPLICATION_CLIENT_ID";
        private const string _tenantId = "DIRECTORY_TENANT_ID";
        private const string _redirectUrl = "mytestapp://auth";

        public IPublicClientApplication ClientApplication { get; private set; }

        public Window? MainWindow { get; private set; }

        public Frame MainFrame { get; private set; }

        public static App Instance { get; private set; }

        public App()
        {
            Instance = this;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
            MainWindow = new Window();
#else
            MainWindow = Microsoft.UI.Xaml.Window.Current;
#endif

#if DEBUG
            MainWindow.EnableHotReload();
#endif

            ClientApplication = PublicClientApplicationBuilder
                .Create(_clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
                .WithRedirectUri(_redirectUrl)
                .WithUnoHelpers()
#if __IOS__
                .WithIosKeychainSecurityGroup("com.test.maslmobile")
#endif
                .Build();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (MainWindow.Content is not Frame rootFrame)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                MainFrame = new Frame();

                // Place the frame in the current Window
                MainWindow.Content = MainFrame;

                MainFrame.NavigationFailed += OnNavigationFailed;
            }

            if (MainFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                MainFrame.Navigate(typeof(MainPage), args.Arguments);
            }

            // Ensure the current window is active
            MainWindow.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new InvalidOperationException($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
        }
    }
}
