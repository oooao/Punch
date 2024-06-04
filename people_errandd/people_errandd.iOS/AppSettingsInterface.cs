using people_errandd.iOS;
using Foundation;
using UIKit;
using UserNotifications;
using UserNotificationsUI;
using Xamarin.Forms;
[assembly: Dependency(typeof(AppSettingsInterface))]
namespace people_errandd.iOS
{
    public class AppSettingsInterface : IAppSettingsHelper
    {
        public void OpenAppSetting()
        {
            NSString SettingString = UIApplication.OpenSettingsUrlString;
            var url = new NSUrl(SettingString);
            UIApplication.SharedApplication.OpenUrl(url);
            //Application.Current.UserAppTheme = OSAppTheme.Light;

        }

    }
}