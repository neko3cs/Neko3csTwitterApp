using System;
using System.Text;
using Neko3csTwitterApp.Models;
using Prism.Navigation;
using Reactive.Bindings;

namespace Neko3csTwitterApp.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        #region ReactiveProperty
        public ReactiveProperty<string> TimeLineString => timeLineString;
        private ReactiveProperty<string> timeLineString = new ReactiveProperty<string>("No Tweet Read.");
        #endregion

        #region ReactiveCommand
        public ReactiveCommand ReloadTimeLineCommand { get; private set; }
        #endregion

        public HomePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Home";

            ReloadTimeLineCommand.Subscribe(async _ =>
            {
                try
                {
                    if (TwitterUser.Tokens == null) { return; }

                    var homeTimeLine = await TwitterUser.Tokens.Statuses.HomeTimelineAsync();
                    var buff = new StringBuilder();
                    foreach (var status in homeTimeLine)
                    {
                        buff.AppendLine("------------------------------");
                        buff.AppendLine($"{status.User.Name}({status.User.ScreenName})");
                        buff.AppendLine(status.Text);
                    }

                    TimeLineString.Value = buff.ToString();
                }
                catch (Exception ex)
                {
                    App.Logger.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            });
        }
    }
}