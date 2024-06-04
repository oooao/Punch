using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using people_errandd.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace people_errandd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class AdvanceGoOut : Rg.Plugins.Popup.Pages.PopupPage
    {

        public AdvanceGoOut(string _Title)
        {

            InitializeComponent();
            PageTitle.Text = _Title;
            
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Arrival.IsVisible = await GoOutViewModel.ConfirmArrival();
        }
        private void StartTripTapped(object sender, EventArgs e)
        {

            Start.BackgroundColor = Color.FromHex("#FFCE78");
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Start.BackgroundColor = Color.FromHex("#FBDDAA");
                return true;
            });
        }

        private void ArrivalTapped(object sender, EventArgs e)
        {
            Arrival.BackgroundColor = Color.FromHex("#E89C9D");
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Arrival.BackgroundColor = Color.FromHex("#F5B3B3");
                return true;
            });
        }
        private void BackTapped(object sender, EventArgs e)
        {

            Back.BackgroundColor = Color.FromHex("#9CB4F1");
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Back.BackgroundColor = Color.FromHex("#C2D2F6");
                return true;
            });
        }

    }
}