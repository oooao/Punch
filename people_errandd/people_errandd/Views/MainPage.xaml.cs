using System;
using Xamarin.Forms;
using people_errandd.Models;
using people_errandd.ViewModels;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Text;
using Xamarin.CommunityToolkit;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Maps;

namespace people_errandd.Views
{
    public partial class MainPage : TabbedPage
    {

        private readonly Work Work = new Work();
        bool allowTap = true;
        private readonly geoLocation geoLocation = new geoLocation();

        public MainPage()
        {
            Application.Current.UserAppTheme = OSAppTheme.Light;
            InitializeComponent();
            var clr = Color.FromHex("#FFFFFF"); 
            this.BarBackgroundColor = clr;
            NavigationPage.SetHasNavigationBar(this, false);
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Audits.ItemsSource = await MainPageViewModel.GetAudit();
            Audits.IsVisible = Audits.ItemsSource != null;
            AuditText.IsVisible = Audits.IsVisible;
            workOn.IsEnabled = Preferences.Get(Preferences.Get("HashAccount", "") + "WorkOnButtonStauts", workOn.IsEnabled = true);
            workOff.IsEnabled = Preferences.Get(Preferences.Get("HashAccount", "") + "WorkOffButtonStauts", workOff.IsEnabled = false);
            workOn.Opacity = Preferences.Get(Preferences.Get("HashAccount", "") + "WorkOnButtonView", workOn.Opacity = 1);
            workOff.Opacity = Preferences.Get(Preferences.Get("HashAccount", "") + "WorkOffButtonView", workOff.Opacity = 0.2);
            workOnText.Opacity = Preferences.Get(Preferences.Get("HashAccount", "") + "WorkOnText", workOnText.Opacity = 1);
            workOffText.Opacity = Preferences.Get(Preferences.Get("HashAccount", "") + "WorkOffText", workOffText.Opacity = 0.2);
            Console.WriteLine(Preferences.Get("HashAccount", ""));
            username.Text = Preferences.Get("UserName", "");
            worktimetitle.Text = Preferences.Get(Preferences.Get("HashAccount", "") + "WorkTimeTitle", "");
            worktime.Text = Preferences.Get(Preferences.Get("HashAccount", "") + "WorkTime", "");
        }
        protected override void OnDisappearing()
        {
            if (geoLocation.cts != null && !geoLocation.cts.IsCancellationRequested)
                geoLocation.cts.Cancel();
            base.OnDisappearing();
        }

        private async void GoToWork(object sender, EventArgs e)
        {
            workOn.BackgroundColor = Color.White;
            try
            {
                if (allowTap)
                {
                    
                    allowTap = false;
                    if (await Work.GetWorkType() == 2 || await Work.GetWorkType() == 0)
                    {
                        (double x, double y) = await geoLocation.GetLocation("WorkOn");
                        if (await geoLocation.GetCurrentLocation(x, y) != true)
                        {
                            if (DeviceInfo.Platform == DevicePlatform.Android)
                            {
                                await DisplayAlert("位置錯誤", "請檢查定位是否開啟與所在位置\n(設定->安全性與定位->定位)", "確定");
                            }
                            else
                            {
                                await DisplayAlert("位置錯誤", "請檢查定位是否開啟與所在位置\n(設定->隱私->位置)", "確定");
                            }
                            //if (!await DisplayAlert("", "是否強制進行打卡?\n(公司位置:" + Preferences.Get("CompanyAddress", "") + ")", "確定", "取消"))
                            //    {
                            //        return;
                            //    }
                            return;
                        }
                        if (await Work.PostWork(1, x, y, true))
                        {
                            await DisplayAlert("", "上班打卡成功", "確定");
                            WorkOnSet();
                        }
                        else
                        {
                            await DisplayAlert("", "網路錯誤", "確定");
                        }
                    }
                    else if (await Work.GetWorkType() == 500)
                    {
                        await DisplayAlert("", "網路錯誤", "確定");
                    }
                    else
                    {
                        await DisplayAlert("", "已上班", "確定");
                    }
                }
            }
            catch (Exception)
            {
                
                await DisplayAlert("", "網路錯誤", "確定");
            }
            finally
            {
                
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    workOn.BackgroundColor = Color.White;
                    allowTap = true;
                    return false;
                });
            }
        }
        private async void OffWork(object sender, EventArgs e)
        {
            workOff.BackgroundColor = Color.White;
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    if (await Work.GetWorkType() == 1)
                    {
                        (double x, double y) = await geoLocation.GetLocation("WorkOff");
                        if (await geoLocation.GetCurrentLocation(x, y) != true)
                        {
                            if (DeviceInfo.Platform == DevicePlatform.Android)
                            {
                                await DisplayAlert("位置錯誤", "請檢查定位是否開啟與所在位置\n(設定->安全性與定位->定位)", "確定");
                            }
                            else
                            {
                                await DisplayAlert("位置錯誤", "請檢查定位是否開啟與所在位置\n(設定->隱私->位置)", "確定");
                            }
                            //if (!await DisplayAlert("", "是否強制進行打卡?\n(公司位置:" + Preferences.Get("CompanyAddress", "") + ")", "確定", "取消"))
                            //{
                            //    return;
                            //}
                            return;
                        }
                        if (await Work.PostWork(2, x, y, true))
                        {
                            await DisplayAlert("", "下班打卡成功", "確定");
                            WorkOffSet();
                        }
                        else
                        {
                            await DisplayAlert("", "網路錯誤", "確定");
                        }
                    }
                    else if (await Work.GetWorkType() == 500)
                    {
                        await DisplayAlert("", "網路錯誤", "確定");
                    }
                    else
                    {

                        await DisplayAlert("", "已下班", "確定");
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("", "網路錯誤", "確定");
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    workOff.BackgroundColor = Color.White;
                    allowTap = true;
                    return false;
                });
            }
        }

        public void WorkOffSet()
        {
            //Preferences.Set("statusNow", status.Text = "已下班");
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOffButtonStauts", workOff.IsEnabled = false);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOnButtonStauts", workOn.IsEnabled = true);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOnButtonView", workOn.Opacity = 1);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOffButtonView", workOff.Opacity = 0.2);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOnText", workOnText.Opacity = 1);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOffText", workOffText.Opacity = 0.2);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkTimeTitle", worktimetitle.Text = "下班打卡 at ");
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkTime", worktime.Text = DateTime.Now.ToString("t"));
            //Preferences.Set("statusBack", "#CB2E2E");
            //statusBack.BackgroundColor = Color.FromHex(Preferences.Get("statusBack", ""));
        }
        public void WorkOnSet()
        {
            //Preferences.Set("statusNow", status.Text = "上班中");
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOnButtonStauts", workOn.IsEnabled = false);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOffButtonStauts", workOff.IsEnabled = true);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOnButtonView", workOn.Opacity = 0.2);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOffButtonView", workOff.Opacity = 0.8);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOnText", workOnText.Opacity = 0.2);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkOffText", workOffText.Opacity = 0.8);
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkTimeTitle", worktimetitle.Text = "上班打卡 at ");
            Preferences.Set(Preferences.Get("HashAccount", "") + "WorkTime", worktime.Text = DateTime.Now.ToString("t"));
            //Preferences.Set("statusBack", "#4AD395");
            //statusBack.BackgroundColor = Color.FromHex(Preferences.Get("statusBack", ""));
        }
        private async void OnTapped(object sender, EventArgs e)
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    await Navigation.PushAsync(new ImportantAudit());
                }
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    allowTap = true;
                    return false;
                });
            }
        }
        private async void GoOutButton(object sender, EventArgs e)
        {
            GoOut.BackgroundColor = Color.White;
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    await PopupNavigation.Instance.PushAsync(new AdvanceGoOut("請選擇"));
                    // await Navigation.PushAsync(new GoOut());
                }
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    workOff.BackgroundColor = Color.White;
                    allowTap = true;
                    return false;
                });
            }
        }
        private async void AboutPageButton(object sender, EventArgs e)
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    await Navigation.PushAsync(new AboutPage());
                }
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    allowTap = true;
                    return false;
                });
            }
        }
        private async void DayOffButton(object sender, EventArgs e)
        {
            DayOff.BackgroundColor = Color.White;
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    await Navigation.PushAsync(new TakeDayOff());
                }
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    
                    allowTap = true;
                    return false;
                });
            }
        }

        private async void GpsButton(object sender, EventArgs e)
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    if (GPSText.Text == "定位未開啟")
                        DependencyService.Get<IAppSettingsHelper>().OpenAppSetting();
                    else
                    {
                        //var supportsUri = await Launcher.CanOpenAsync("");
                        //if (supportsUri)
                        //{
                        //    Console.WriteLine("true");
                        //    await Launcher.OpenAsync("");
                        //}                                
                        await PopupNavigation.Instance.PushAsync(new MapPage());
                    }
                }
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    allowTap = true;
                    return false;
                });
            }
        }
        void OnGoToWorkButtonPressed(object sender, EventArgs args)
        {
            workOn.BackgroundColor = Color.FromHex("#E0E0E0");
            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                workOn.BackgroundColor = Color.White;
                return true;
            });

        }
        void OnOffWorkButtonPressed(object sender, EventArgs args)
        {
            workOff.BackgroundColor = Color.FromHex("#E0E0E0");
            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                workOff.BackgroundColor = Color.White;
                return true;
            });

        }
        void OnGoOutButtonPressed(object sender, EventArgs args)
        {
            GoOut.BackgroundColor = Color.FromHex("#E0E0E0");
            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                GoOut.BackgroundColor = Color.White;
                return true;
            });

        }
        void OnDayOffButtonPressed(object sender, EventArgs args)
        {
            DayOff.BackgroundColor = Color.FromHex("#E0E0E0");
            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                DayOff.BackgroundColor = Color.White;
                return true;
            });

        }
        
    }
}

