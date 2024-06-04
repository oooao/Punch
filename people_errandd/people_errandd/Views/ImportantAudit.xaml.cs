using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using people_errandd.ViewModels;
using people_errandd.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace people_errandd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportantAudit : ContentPage
    {
        public bool allowTap = true;
        public ImportantAudit()
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#EDEEEF");
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.FromHex("#555555");


        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Audits.ItemsSource = await MainPageViewModel.GetAudit();
        }
        private async void Review(object sender, EventArgs e)
        {
            try
            {
                if (allowTap)
                {
                    Console.WriteLine(((Button)sender).CommandParameter);
                    int id = Convert.ToInt32(((Button)sender).CommandParameter);
                    if (await Records.ReviewLeaveRecord(id, true))
                    {
                        await DisplayAlert("", "核准審核成功", "確認");
                        Audits.ItemsSource = await MainPageViewModel.GetAudit();
                    }
                    else
                    {
                        await Error();
                    }

                }
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(2.5), () =>
                {
                    allowTap = true;
                    return false;
                });
            }

        }
        private async void Reject(object sender, EventArgs e)
        {
            try
            {
                if (allowTap)
                {
                    Console.WriteLine(((Button)sender).CommandParameter);
                    int id = Convert.ToInt32(((Button)sender).CommandParameter);
                    if (await Records.ReviewLeaveRecord(id, false))
                    {
                        await DisplayAlert("", "拒絕審核成功", "確認");
                        Audits.ItemsSource = await MainPageViewModel.GetAudit();
                    }
                    else
                    {
                        await Error();
                    }
                }
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(2.5), () =>
                {
                    allowTap = true;
                    return false;
                });
            }
        }
        async Task Error()
        {
            await DisplayAlert("", "錯誤，請重新瀏覽此頁面", "確認");
            Audits.ItemsSource = await MainPageViewModel.GetAudit();
        }
    }
}