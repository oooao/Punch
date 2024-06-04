using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using people_errandd.Models;
using System.Collections.ObjectModel;
using people_errandd.ViewModels;

namespace people_errandd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Record : ContentPage
    {
        private bool allowTap = true;
        public static int RecordTypeId;
        public static DateTime dt;
        readonly Records Records = new Records();
        public static double Dt;
        protected override async void OnAppearing()
        {
            base.OnAppearing();            
            RecordTypeId = 1;
            SetDateButton();
            DateButtonSwitch(); 
            RecordTitle.Text = "打卡紀錄";
            Worklist.ItemsSource = await Records.GetWorkRecord(dt.ToString("yyyy/M/d"));
        }
        public Record()
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#EDEEEF");
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.FromHex("#555555");
        }
        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    Navigation.PopAsync();
                    RecordTypeId = 0;
                }
            }
            finally
            {
                allowTap = true;
            }
        }
        async void RecordChooseButton(object sender, EventArgs e)
        {

            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    var ActionSheet = await DisplayActionSheet("請選擇:", "Cancel", null, "上下班", "請假", "公出");
                    switch (ActionSheet)
                    {
                        case "Cancel":
                            break;
                        case "上下班":
                            Worklist.ItemsSource = await Records.GetWorkRecord(dt.ToString("yyyy/M/d"));
                            RecordTypeId = 1;
                            RecordTitle.Text = "打卡紀錄";
                            break;
                        case "請假":
                            Worklist.ItemsSource = await Records.GetLeaveRecord(dt,"");
                            Console.WriteLine(dt.ToString("yyyy/M/d"));
                            RecordTypeId = 2;
                            RecordTitle.Text = "請假紀錄";
                            break;
                        case "公出":
                            Worklist.ItemsSource = await Records.GetAdvanceGoOutsRecord(dt.ToString("yyyy/M/d"));
                            RecordTitle.Text = "公出紀錄";
                            RecordTypeId = 3;
                            break;
                    }
                }
            }
            finally
            {
                allowTap = true;
            }
        }
        public async Task Switch()
        {
            switch (RecordTypeId)
            {
                case 0:
                case 1:
                    Worklist.ItemsSource = await Records.GetWorkRecord(dt.ToString("yyyy/M/d"));
                    break;
                case 2:
                    Worklist.ItemsSource = await Records.GetLeaveRecord(dt,"");
                    break;
                case 3:
                    Worklist.ItemsSource = await Records.GetAdvanceGoOutsRecord(dt.ToString("yyyy/M/d"));
                    break;
            }
        }
        private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            SetDateButton();            
            DateButtonSwitch();
            await Switch();
        }
        private async void SunButton(object sender, EventArgs e)
        {
            SetDateButton();
            setDate(0);
            sun.BackgroundColor = Color.FromHex("#5C76B1");
            sun.TextColor = Color.FromHex("#FFFFFF");
            await Switch();
        }
        private async void MonButton(object sender, EventArgs e)
        {
            SetDateButton();
            setDate(1);
            mon.BackgroundColor = Color.FromHex("#5C76B1");
            mon.TextColor = Color.FromHex("#FFFFFF");
            await Switch();
        }
        private async void TueButton(object sender, EventArgs e)
        {
            SetDateButton();
            setDate(2);
            tue.BackgroundColor = Color.FromHex("#5C76B1");
            tue.TextColor = Color.FromHex("#FFFFFF");

            await Switch();
        }
        private async void WedButton(object sender, EventArgs e)
        {
            SetDateButton();
            setDate(3);
            wed.BackgroundColor = Color.FromHex("#5C76B1");
            wed.TextColor = Color.FromHex("#FFFFFF");
            await Switch();
        }
        private async void ThuButton(object sender, EventArgs e)
        {
            SetDateButton();
            setDate(4);
            thu.BackgroundColor = Color.FromHex("#5C76B1");
            thu.TextColor = Color.FromHex("#FFFFFF");
            await Switch();
        }
        private async void FriButton(object sender, EventArgs e)
        {
            SetDateButton();
            setDate(5);
            fri.BackgroundColor = Color.FromHex("#5C76B1");
            fri.TextColor = Color.FromHex("#FFFFFF");
            await Switch();
        }
        private async void SatButton(object sender, EventArgs e)
        {
            SetDateButton();
            setDate(6);
            sat.BackgroundColor = Color.FromHex("#5C76B1");
            sat.TextColor = Color.FromHex("#FFFFFF");
            await Switch();
        }
        private void setDate(double Date)
        {
            dt = _DatePicker.Date.AddDays(-Dt);
            dt = dt.AddDays(Date);
            _DatePicker.Date = dt;
        }
        private void SetDateButton()
        {
            thu.BackgroundColor = Color.FromHex("#FFFFFF");
            thu.TextColor = Color.FromHex("#000000");
            sun.BackgroundColor = Color.FromHex("#FFFFFF");
            sun.TextColor = Color.FromHex("#000000");
            mon.BackgroundColor = Color.FromHex("#FFFFFF");
            mon.TextColor = Color.FromHex("#000000");
            tue.BackgroundColor = Color.FromHex("#FFFFFF");
            tue.TextColor = Color.FromHex("#000000");
            wed.BackgroundColor = Color.FromHex("#FFFFFF");
            wed.TextColor = Color.FromHex("#000000");
            fri.BackgroundColor = Color.FromHex("#FFFFFF");
            fri.TextColor = Color.FromHex("#000000");
            sat.BackgroundColor = Color.FromHex("#FFFFFF");
            sat.TextColor = Color.FromHex("#000000");
        }
        private void DateButtonText(double _dt)
        {          
            dt = dt.AddDays(-_dt);
            sun.Text = dt.ToString("dd");
            mon.Text = dt.AddDays(1).ToString("dd");
            tue.Text = dt.AddDays(2).ToString("dd");
            wed.Text = dt.AddDays(3).ToString("dd");
            thu.Text = dt.AddDays(4).ToString("dd");
            fri.Text = dt.AddDays(5).ToString("dd");
            sat.Text = dt.AddDays(6).ToString("dd");
            dt = _DatePicker.Date;
        }
        private void DateButtonSwitch()
        {
            dt = _DatePicker.Date;
            Console.WriteLine(dt.ToString());
            switch (dt.ToString("dddd"))
            {
                case "星期日":
                    DateButtonText(0);
                    sun.BackgroundColor = Color.FromHex("#5C76B1");
                    sun.TextColor = Color.FromHex("#FFFFFF");
                    Dt = 0;
                    Console.WriteLine(dt.ToString());
                    break;
                case "星期一":
                    DateButtonText(1);
                    mon.BackgroundColor = Color.FromHex("#5C76B1");
                    mon.TextColor = Color.FromHex("#FFFFFF");
                    Dt = 1;
                    break;
                case "星期二":
                    DateButtonText(2);
                    tue.BackgroundColor = Color.FromHex("#5C76B1");
                    tue.TextColor = Color.FromHex("#FFFFFF");
                    Dt = 2;
                    break;
                case "星期三":
                    DateButtonText(3);
                    wed.BackgroundColor = Color.FromHex("#5C76B1");
                    wed.TextColor = Color.FromHex("#FFFFFF");
                    Dt = 3;
                    break;
                case "星期四":
                    DateButtonText(4);
                    thu.BackgroundColor = Color.FromHex("#5C76B1");
                    thu.TextColor = Color.FromHex("#FFFFFF");
                    Dt = 4;
                    break;
                case "星期五":
                    DateButtonText(5);
                    fri.BackgroundColor = Color.FromHex("#5C76B1");
                    fri.TextColor = Color.FromHex("#FFFFFF");
                    Dt = 5;
                    break;
                case "星期六":
                    DateButtonText(6);
                    sat.BackgroundColor = Color.FromHex("#5C76B1");
                    sat.TextColor = Color.FromHex("#FFFFFF");
                    Dt = 6;
                    break;
            }
        }
    }    
    public class RecordsSelector : DataTemplateSelector
    {
        public DataTemplate WorkRecords { get; set; }
        public DataTemplate DayOffRecords { get; set; }
        public DataTemplate GoOutRecords { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (Record.RecordTypeId)
            {
                case 2:
                    return DayOffRecords;
                case 3:
                    return GoOutRecords;
                default:
                    return WorkRecords;
            }
        }
    }


}