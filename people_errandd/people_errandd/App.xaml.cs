using System;
using Xamarin.Forms;
using people_errandd.Views;
using people_errandd.ViewModels;
using people_errandd.Models;
using System.Globalization;
using Xamarin.Essentials;
using Plugin.SharedTransitions;
using System.Threading.Tasks;


namespace people_errandd
{
    public interface IAppSettingsHelper
    {
        void OpenAppSetting();
    }
    public partial class App : Application
    {
        //static Database dataBase;
        Work work = new Work();
        InformationViewModel information = new InformationViewModel();
        geoLocation location = new geoLocation();
        public static double Latitude { get; set; }
        public static double Longitude { get; set; }

        //public static Database DataBase
        //{
        //    get
        //    {
        //        if (dataBase == null)
        //        {
        //            dataBase = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data.db3"));
        //        }
        //        return dataBase;
        //    }
        //}
        public App()
        {
            InitializeComponent();
            CultureInfo ChineseCulture = new CultureInfo("zh-TW");
            CultureInfo.DefaultThreadCurrentCulture = ChineseCulture;
            Device.SetFlags(new[] { "Expander_Experimental" });
            MainPage = new SharedTransitionNavigationPage(new LoginPage());
            
            //NavigationPage
        }

        protected async override void OnStart()
        {
            Console.WriteLine(Preferences.Get("companyHash", ""));
            bool hasKey = Preferences.ContainsKey("HashAccount");
            if (hasKey)
            {               
                    MainPage = new SharedTransitionNavigationPage(new MainPage());
                    await information.GetUserName(Preferences.Get("HashAccount", ""));
                    await GetLocation();
                    await GetConnectivity("start");
                    await location.GetLocationText(Latitude, Longitude);
                if (!await Login.AccountEnabled())
                {
                    MainPage = new SharedTransitionNavigationPage(new LoginPage());
                    await App.Current.MainPage.DisplayAlert("", "帳號已停用", "確認");
                    Preferences.Remove("HashAccount");
                }
            }
            var Seconds = TimeSpan.FromSeconds(8);
            Device.StartTimer(Seconds, () => {
                location.GetLocationText(Latitude,Longitude);
                GetLocation();
                return true;
            });            
        }
        protected override void OnSleep()
        {

        }
        protected override async void OnResume()
        {
            await GetConnectivity("resume");
            MessagingCenter.Send<App>(this, "Hi");
        }
        private async Task GetLocation()
        {
            try
            {
                (Latitude, Longitude) = await location.GetLocation("Back");
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }
        private async Task GetConnectivity(string status)
        {
            Page page = MainPage;            
            try
            {
                if (status == "start")
                {

                    switch (await work.GetWorkType())
                    {
                        case 0:
                        case 1:
                            await work.PostWork(1, Latitude, Longitude, false);
                            break;
                        case 2:
                            await work.PostWork(2, Latitude, Longitude, false);
                            break;
                        default:
                            break;
                    }
                    if (!await HttpResponse.Defence() && Preferences.ContainsKey("HashAccount"))
                    {
                        await page.DisplayAlert("","錯誤","確定");
                        MainPage = new SharedTransitionNavigationPage(new LoginPage());
                        Preferences.Clear();
                        Preferences.Set("uuid", Guid.NewGuid().ToString());
                        await App.Current.MainPage.DisplayAlert("", "偵測到違規行為，帳號已停用", "確認");                       
                    }
                }
                else
                {
                    await work.GetWorkType();
                }          
            }
            catch (Exception)
            {
                await page.DisplayAlert("", "請檢查網路狀態", "確定");
                return;
            }
        }
    }
}