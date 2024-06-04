using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using people_errandd.ViewModels;
using people_errandd.Models;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;

namespace people_errandd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private bool allowTap = true;
        private readonly Login Login = new Login();
        public static string CompanyId;
         InformationViewModel _InfVM = new InformationViewModel();
        public LoginPage()
        {
            InitializeComponent();
            //隱藏navigationpage導航欄。
            NavigationPage.SetHasNavigationBar(this, false);
            Application.Current.UserAppTheme = OSAppTheme.Light;
            Animation ani = new Animation();
            company.Text = Preferences.Get("CompanyId","");
            if (string.IsNullOrEmpty(Preferences.Get("uuid", string.Empty)))
            {
                Preferences.Set("uuid", Guid.NewGuid().ToString());
            }
            Console.WriteLine(Preferences.Get("uuid", ""));
           // TelephonyManager mTelephonyMgr = 

        }
        private async void LogInButton(object sender, EventArgs e)
        {
            button.BackgroundColor = Color.FromHex("#34549E");
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    loading.IsRunning = true;
                    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    {
                        await DisplayAlert("", "網路錯誤，請檢查網路", "確認");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(company.Text))
                    {
                        await DisplayAlert("", "請勿輸入空白", "確認");
                    }
                    else if (await Login.ConfirmCompanyHash(company.Text.Trim()))
                    {
                        if (await Login.ConfirmUUID(Preferences.Get("uuid", "")))
                        {
                            if (!await Login.Reviewed())
                            {                           
                                await DisplayAlert("審核中", "尚未審核完畢,請稍後再試", "確認");
                                return;
                            }
                            if (!await Login.AccountEnabled())
                            {
                                await DisplayAlert("", "帳號已停用", "確認");
                                return;
                            }
                            Preferences.Set("HashAccount", await Login.GetHashAccount(Preferences.Get("uuid", "")));
                            Preferences.Set("CompanyId", company.Text.Trim());
                            await _InfVM.GetUserName(Preferences.Get("HashAccount",""));
                            Navigation.InsertPageBefore(new MainPage(), this);                           
                            await Navigation.PopAsync();              
                        }
                        else
                        {
                            CompanyId = company.Text.Trim();
                            await PopupNavigation.Instance.PushAsync(new VerificationPage("首次登入"));
                        }
                    }
                    else
                    {
                        await DisplayAlert("錯誤", "輸入錯誤", "請重新輸入");
                    }
                }
            }
            finally
            {
                    
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    loading.IsRunning = false;
                    allowTap = true;
                    return false;
                });
                
            }
        }
        private async void QuestionButton(object sender, EventArgs e)
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    await DisplayAlert("", "有任何問題，請與我們聯繫", "確定");
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
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            transition.Opacity = 0;
            await transition.FadeTo(1, 2500);
            await image.ScaleTo(1.5, 1000, Easing.CubicIn);
            await image.ScaleTo(1, 1000, Easing.CubicOut);
        }

        private void CompanyIdChecked(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(company.Text))
            {
                Preferences.Set("CompanyID", "");
            }
        }
        void OnLogInButtonPressed(object sender, EventArgs args)
        {
            button.BackgroundColor = Color.FromHex("#50618C");

        }
        /*
private async void Test_Clicked(object sender, EventArgs e)
{
   await PopupNavigation.Instance.PushAsync(new VerificationPage("繼承資料"));
}
*/
    }
}