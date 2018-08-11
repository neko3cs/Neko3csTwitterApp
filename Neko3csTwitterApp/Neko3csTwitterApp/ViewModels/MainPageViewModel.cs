using Prism.Navigation;

namespace Neko3csTwitterApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Home";
        }
    }
}