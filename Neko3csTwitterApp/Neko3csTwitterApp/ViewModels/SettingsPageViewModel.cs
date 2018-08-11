using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using CoreTweet;
using Neko3csTwitterApp.Models;
using Prism.Navigation;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Neko3csTwitterApp.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        #region ReactiveProperty
        public ReactiveProperty<string> PinCode => PinCode;
        private ReactiveProperty<string> pinCode = new ReactiveProperty<string>();

        public ReactiveProperty<ImageSource> UserIcon => userIcon;
        private ReactiveProperty<ImageSource> userIcon = new ReactiveProperty<ImageSource>();

        public ReactiveProperty<bool> IsVisibleUserIcon { get; private set; }

        public ReactiveProperty<string> UserInfo => userInfo;
        private ReactiveProperty<string> userInfo = new ReactiveProperty<string>();
        #endregion

        #region ReactiveCommand
        public ReactiveCommand StartingAuthCommand { get; private set; }

        public ReactiveCommand UserAuthCommand { get; private set; }
        #endregion

        public SettingsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Settings";

            IsVisibleUserIcon = UserIcon.Select(x => x != null)
                                        .ToReactiveProperty();

            StartingAuthCommand.Subscribe(async _ =>
            {
                TwitterUser.Session = await OAuth.AuthorizeAsync(TwitterUser.APIKey, TwitterUser.APISecret);
                Process.Start(TwitterUser.Session.AuthorizeUri.AbsoluteUri);
            });

            UserAuthCommand.Subscribe(async _ =>
            {
                try
                {
                    TwitterUser.Tokens = await TwitterUser.Session.GetTokensAsync(PinCode.Value);

                    if (TwitterUser.Tokens.Account.VerifyCredentialsAsync() != null)
                    {
                        var response = await TwitterUser.Tokens.Account.VerifyCredentialsAsync();

                        userIcon.Value = ImageSource.FromUri(new Uri(response.ProfileImageUrl));
                        IsVisibleUserIcon.Value = true;

                        var builder = new StringBuilder();
                        builder.AppendLine($"UserName : { response.Name }");
                        builder.Append($"UserID : @{ response.ScreenName }");
                        userInfo.Value = builder.ToString();
                    }
                    else
                    {
                        App.Logger.Warn("ログイン認証失敗");
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            });
        }
    }
}
