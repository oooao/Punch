using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using people_errandd.ViewModels;
using Rg.Plugins.Popup.Extensions;
using people_errandd.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace people_errandd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificationPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private bool allowTap = true;
        private readonly Login Login = new Login();
        private readonly InformationViewModel information = new InformationViewModel();
        Regex regexemail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public VerificationPage(string _Title)
        {
            InitializeComponent();
            PageTitle.Text = _Title;
        }

        private async void LoginButton(object sender, EventArgs e)
        {
            try
            {
                Button.IsEnabled = false;
                if(allowTap)
                {
                    allowTap = false;
                    if (string.IsNullOrWhiteSpace(UserNameResult.Text))
                    {
                        await DisplayAlert("錯誤", "請輸入姓名", "確認");                        
                    }
                    else if(string.IsNullOrWhiteSpace(UserEmailResult.Text))
                    {
                        await DisplayAlert("錯誤", "請輸入信箱", "確認");
                    }
                    else
                    {
                        Match matchemail = regexemail.Match(UserEmailResult.Text);
                        if (matchemail.Success)
                        {
                            if (!await information.ConfirmEmail(UserEmailResult.Text))
                            {
                                if (await Login.SetUUID() && await Login.SetInformation(UserNameResult.Text, UserEmailResult.Text))
                                {
                                    await DisplayAlert("已送出成功", "待管理者審核成功後，將發送通知信至郵件即可進行登入", "確認");
                                    Preferences.Set("CompanyId", LoginPage.CompanyId);
                                    await Navigation.PopPopupAsync();
                                    Preferences.Set("審核中", "");
                                }
                                else
                                {
                                    await DisplayAlert("網路錯誤", "請檢查網路", "確認");
                                }
                            }
                            else
                            {
                                await DisplayAlert("錯誤", "信箱已存在", "確認");
                            }
                        }
                        else
                        {
                            await DisplayAlert("格式錯誤", "電子郵件格式錯誤", "確認");
                        }
                    }
                }
                
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(2.5), () =>
                {
                    allowTap = true;
                    Button.IsEnabled = true;
                    return false;
                });
            }
           
        }
    }
}