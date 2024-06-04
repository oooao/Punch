using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using people_errandd.ViewModels;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;

namespace people_errandd.ViewModels
{
    class AdvanceGoOutViewModel : INotifyPropertyChanged
    {
        GoOutViewModel GoOut = new GoOutViewModel();
        private bool allowTap = true;
        public ICommand StartTripCommand { private set; get; }
        public ICommand ArrivalCommand { private set; get; }
        public ICommand BackCommand { private set; get; }

        bool canTrip;
        bool canBack;
        double tripOpacity;
        double backOpacity;
        public event PropertyChangedEventHandler PropertyChanged;
        public static double Opacity(string _Type)
        {
            if (_Type == "trip")
            {
                if (Preferences.ContainsKey("TripNow"))
                {
                    return 0.2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (Preferences.ContainsKey("TripNow"))
                {
                    return 1;
                }
                else
                {
                    return 0.2;
                }

            }
        }

        public AdvanceGoOutViewModel()
        {
            CanTrip = !Preferences.ContainsKey("TripNow");
            CanBack = Preferences.ContainsKey("TripNow");
            TripOpacity= Opacity("trip");
            backOpacity = Opacity("back");
            StartTripCommand = new Command(StartTrip);
            ArrivalCommand = new Command(Arrival);
            BackCommand = new Command(Back);                       
        }
        public double TripOpacity
        {
            set
            {
                if (tripOpacity != value)
                {
                    tripOpacity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return tripOpacity;
            }
        }
        public double BackOpacity
        {
            set
            {
                if (backOpacity != value)
                {
                    backOpacity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return backOpacity;
            }
        }
        public bool CanTrip
        {
            set
            {
                if (canTrip != value)
                {
                    canTrip = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return canTrip;
            }
        }
        public bool CanBack
        {
            set
            {
                if (canBack != value)
                {
                    canBack = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return canBack;
            }
        }
        async void StartTrip()
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    if (await GoOut.PostGoOut(1))
                    {
                        await App.Current.MainPage.DisplayAlert("", "公出成功", "確認");
                        Preferences.Set("TripNow", "1");
                        CanTrip = false;
                        CanBack = true;
                        TripOpacity = 0.2;
                        BackOpacity = 1;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("", "網路錯誤", "確認");
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

        async void Arrival()
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    if (await GoOut.PostGoOut(2))
                    {
                        await App.Current.MainPage.DisplayAlert("", "到站成功", "確認");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("", "網路錯誤", "確認");
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
        async void Back()
        {
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    if (await GoOut.PostGoOut(3))
                    {
                        await App.Current.MainPage.DisplayAlert("", "返回成功", "確認");
                        Preferences.Remove("TripNow");
                        CanTrip = true;
                        CanBack = false;
                        TripOpacity = 1;
                        BackOpacity = 0.2;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("", "網路錯誤", "確認");
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
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)  
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        // Opacity
    }


}