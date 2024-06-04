using Android.Content;
using Android.OS;
using people_errandd.Droid;
using System;

using Xamarin.Forms;
using static Android.Provider.Settings;
using Application = Android.App.Application;
[assembly: Dependency(typeof(AppSettingsInterface))]
namespace people_errandd.Droid
{
    public class AppSettingsInterface : MainActivity, IAppSettingsHelper
    {

        public void OpenAppSetting()
        {
            var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
            Application.Context.StartActivity(intent);

        }
        string id = string.Empty;


        public string Id
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(id))
                    return id;

                id = Build.Serial;
                if (string.IsNullOrWhiteSpace(id) || id == Build.Unknown || id == "0")
                {
                    try
                    {
                        var context = Android.App.Application.Context;
                        id = Secure.GetString(context.ContentResolver, Secure.AndroidId);
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Warn("DeviceInfo", "Unable to get id: " + ex.ToString());
                    }
                }

                return id;
            }
        }
    }

}



