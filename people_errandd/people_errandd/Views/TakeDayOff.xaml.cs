using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using people_errandd.ViewModels;
using people_errandd.Models;
using System.Collections.Generic;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Threading;

namespace people_errandd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakeDayOff : ContentPage
    {
        private bool allowTap = true;
        private static int LeaveTypeId;
        readonly TakeDayOffViewModel takeDayOff = new TakeDayOffViewModel();
        public TakeDayOff()
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#EDEEEF");
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.FromHex("#555555");
            LeaveTypeId = 0;
            //var dayoffList = new List<string>();
            //dayoffList.Add("事假");
            //dayoffList.Add("病假");
            //dayoffList.Add("喪假");
            //dayoffList.Add("產假");
            //dayoffList.Add("生理假");
            //dayoffList.Add("流產假");
            //dayoffList.Add("產前假");
            //dayoffList.Add("陪產假");

            //var picker = new Picker { Title = "請選擇:", TitleColor = Color.FromHex("#696969") };
            //leaveType.ItemsSource = dayoffList;
            startTimePicker.IsVisible = !AlldaySwitch.IsToggled;
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
        private void Personal(object sender, CheckedChangedEventArgs e)
        {

            leavetype.Text = "事假";
            LeaveTypeId = 1;
        }
        private void Sick(object sender, CheckedChangedEventArgs e)
        {
            leavetype.Text = "病假";
            LeaveTypeId = 2;
        }
        private void Bereavement(object sender, CheckedChangedEventArgs e)
        {
            leavetype.Text = "喪假";
            LeaveTypeId = 3;
        }
        private void Maternity(object sender, CheckedChangedEventArgs e)
        {
            leavetype.Text = "產假";
            LeaveTypeId = 4;
        }
        private void Physiological(object sender, CheckedChangedEventArgs e)
        {
            leavetype.Text = "生理假";
            LeaveTypeId = 5;
        }
        private void Abortion(object sender, CheckedChangedEventArgs e)
        {
            leavetype.Text = "流產假";
            LeaveTypeId = 6;
        }
        private void Prenatal(object sender, CheckedChangedEventArgs e)
        {
            leavetype.Text = "產前假";
            LeaveTypeId = 7;
        }
        private void Paternity(object sender, CheckedChangedEventArgs e)
        {
            leavetype.Text = "陪產假";
            LeaveTypeId = 8;
        }
        private void Annual(object sender, CheckedChangedEventArgs e)
        {
            leavetype.Text = "特休假";
            LeaveTypeId = 9;
        }
        private async void EnterButton(object sender, EventArgs e)
        {

            try
            {
                if (allowTap)
                {
                    allowTap = false;                  
                    DateTime StartDateTime, EndDateTime;

                    if (startTimePicker.IsVisible)
                    {
                        StartDateTime = startDatePicker.Date + startTimePicker.Time;
                        EndDateTime = endDatePicker.Date + endTimePicker.Time;
                    }
                    else
                    {
                        string TimeFormat = endDatePicker.Date.ToString("d") + " 23:59:59";
                        Console.WriteLine(TimeFormat);
                        StartDateTime = startDatePicker.Date;
                        EndDateTime = DateTime.ParseExact(TimeFormat, "yyyy/M/d HH:mm:ss", null);
                        Console.WriteLine(EndDateTime.ToString());
                    }
                    
                    if (LeaveTypeId == 0)
                    {
                        await DisplayAlert("", "請選擇假別", "確認");                                        
                    }
                    else if (string.IsNullOrWhiteSpace(reason.Text))
                    {
                        await DisplayAlert("", "請填寫原因", "確認");
                    }
                    else
                    {                        
                        if (await takeDayOff.PostDayOff(StartDateTime, EndDateTime, LeaveTypeId, reason.Text))
                        {
                            await DisplayAlert("", "申請成功", "確認");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("", "網路錯誤，請重新嘗試", "確認");
                        }
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
        public void minTime()
        {
            if (startDatePicker.Date == endDatePicker.Date && endTimePicker.Time < startTimePicker.Time)
            {
                endTimePicker.Time = startTimePicker.Time;
            }
        }
        private void startDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            minTime();
        }

        private void endDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            minTime();
        }

        private void endTimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            minTime();
        }

        private void startTimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            minTime();
        }

        
    }
}

