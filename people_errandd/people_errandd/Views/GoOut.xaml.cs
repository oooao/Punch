using System;
using people_errandd.Models;
using people_errandd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace people_errandd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoOut : ContentPage
    {
        GoOutViewModel goOut = new GoOutViewModel();
        private bool allowTap = true;
        public GoOut()
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#EDEEEF");
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.FromHex("#555555");
        }
          void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    Navigation.PopModalAsync(true);
                }
            }
            finally
            {
                allowTap = true;
            }
        }

        private async void EnterButton(object sender, EventArgs e)
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    DateTime StartDateTime = startDatePicker.Date + startTimePicker.Time;
                    DateTime EndDateTime = endDatePicker.Date + endTimePicker.Time;
                    if (await goOut.PostGoOut(StartDateTime, EndDateTime, locationEntry.Text, reasonEntry.Text))
                    {
                        await DisplayAlert("", "申請成功", "OK");                       
                        locationEntry.Text = "";
                        reasonEntry.Text = "";
                    }
                    else
                    {
                        await DisplayAlert("Error", "錯誤" + StartDateTime, "OK");                        
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("", "格式錯誤,請重新輸入", "OK");
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
        private void AlldaySwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (AlldaySwitch.IsToggled == true)
            {
                startTimePicker.IsVisible = false;
                endTimePicker.IsVisible = false;
            }
            else
            {
                startTimePicker.IsVisible = true;
                endTimePicker.IsVisible = true;
            }
        }
        private void VistitClient(object sender, CheckedChangedEventArgs e)
        {
            goouttype.Text = "拜訪客戶";
           
        }
        private void VistitManuacturer(object sender, CheckedChangedEventArgs e)
        {
            goouttype.Text = "拜訪廠商";
          
        }
        private void Demo(object sender, CheckedChangedEventArgs e)
        {
            goouttype.Text = "Demo";
        
        }
        private void ParticipateActivity(object sender, CheckedChangedEventArgs e)
        {
            goouttype.Text = "參加活動";

        }
        private void Meeting(object sender, CheckedChangedEventArgs e)
        {
            goouttype.Text = "會議";
           
        }
        private void CustomerService(object sender, CheckedChangedEventArgs e)
        {
            goouttype.Text = "客戶服務";
         
        }
        private void Other(object sender, CheckedChangedEventArgs e)
        {
            goouttype.Text = "其他";
           
        }
    }
}