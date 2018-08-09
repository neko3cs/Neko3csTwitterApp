using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using Reactive.Bindings;
using Neko3csTwitterApp.Models;
using CoreTweet;
using System.Diagnostics;
using System.Text;

namespace Neko3csTwitterApp.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public ReactiveProperty<string> PinCode => PinCode;
        private ReactiveProperty<string> pinCode = new ReactiveProperty<string>();

        public ReactiveProperty<string> UserIcon => userIcon;
        private ReactiveProperty<string> userIcon = new ReactiveProperty<string>();

        public ReactiveProperty<bool> IsVisibleUserIcon => isVisibleUserIcon;
        private ReactiveProperty<bool> isVisibleUserIcon = new ReactiveProperty<bool>();

        public ReactiveProperty<string> UserInfo => userInfo;
        private ReactiveProperty<string> userInfo = new ReactiveProperty<string>();

        public ReactiveCommand StartingAuthCommand { get; private set; }

        public ReactiveCommand UserAuthCommand { get; private set; }


        public SettingsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Settings";


            StartingAuthCommand.Subscribe(async _ =>
            {
                UserAccount.Session = await OAuth.AuthorizeAsync(UserAccount.APIKey, UserAccount.APISecret);
                Process.Start(UserAccount.Session.AuthorizeUri.AbsoluteUri);
            });

            UserAuthCommand.Subscribe(async _ =>
            {
                try
                {
                    UserAccount.Tokens = await UserAccount.Session.GetTokensAsync(PinCode.Value);

                    if (UserAccount.Tokens.Account.VerifyCredentialsAsync() != null)
                    {
                        // TODO : Xamarinでの画像の扱い方
                        //userIcon = new BitmapImage(new Uri(UserAccount.Tokens.Account.VerifyCredentials().ProfileImageUrl));
                        userIcon.Value = "hoge";
                        isVisibleUserIcon.Value = true;

                        // TODO : API変わったせいかなぜか取得できない
                        //var name = await UserAccount.Tokens.Account.VerifyCredentialsAsync().Name;
                        //var screenName = await UserAccount.Tokens.Account.VerifyCredentialsAsync().ScreenName;
                        var name = "hogehoge";
                        var screenName = "fugafuga";


                        var builder = new StringBuilder();
                        builder.AppendLine($"UserName : { name }");
                        builder.Append($"UserID : @{ screenName }");
                        userInfo.Value = builder.ToString();
                    }
                    else
                    {
                        /* 認証失敗(なんか対処して) */
                    }
                }
                catch
                {
                    throw;
                }
            });
        }
    }
}
