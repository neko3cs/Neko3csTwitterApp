using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using System.Text;
using Reactive.Bindings;
using Neko3csTwitterApp.Models;

namespace Neko3csTwitterApp.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public ReactiveProperty<string> TimeLineString => timeLineString;
        private ReactiveProperty<string> timeLineString = new ReactiveProperty<string>("No Tweet Read.");

        public ReactiveCommand ReloadTimeLineCommand { get; private set; }

        public HomePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Home";

            ReloadTimeLineCommand.Subscribe(_ =>
            {
                try
                {
                    if (UserAccount.Tokens == null) { return; }

                    var timeLine = new StringBuilder();
                    foreach (var status in UserAccount.Tokens.Statuses.HomeTimeline())
                    {
                        timeLine.AppendLine("------------------------------");
                        timeLine.AppendLine($"{status.User.Name}({status.User.ScreenName})");
                        timeLine.AppendLine(status.Text);
                    }

                    TimeLineString.Value = timeLine.ToString();
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteError("例外エラーが発生しました。", ex);
                }
            });
        }
    }
}