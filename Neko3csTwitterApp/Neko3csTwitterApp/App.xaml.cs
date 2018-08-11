using Neko3csTwitterApp.Views;
using Prism;
using Prism.Ioc;
using Prism.Logging;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Neko3csTwitterApp
{
    public partial class App : PrismApplication
    {
        #region "Global Instance"
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Prismログをデバッグログへ表示
            containerRegistry.RegisterSingleton<ILoggerFacade, DebugLogger>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<HomePage>();
            containerRegistry.RegisterForNavigation<SettingsPage>();
        }
    }
}