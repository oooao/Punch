using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using people_errandd.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace people_errandd.ViewModels
{
    class MainPageViewModel : HttpResponse, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        DateTime dateTime;
        string LocationNowText, _GpsText, _GpsTextColor;
        public MainPageViewModel()
        {
            this.DateTime = DateTime.Now;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {            
                //LocationText = geoLocation.LocationNowText;
                DateTime = DateTime.Now;
                GpsText = Preferences.Get("GpsText", "");
                GpsTextColor = Preferences.Get("GpsButtonColor", "");
                return true;
            });
        }
        public DateTime DateTime
        {
            get
            {
                return dateTime;
            }
            set
            {
                if (dateTime != value)
                {
                    dateTime = value;
                    OnPropertyChanged();
                }
            }
        }
        public string GpsText
        {
            get
            {
                return _GpsText;
            }
            set
            {
                if (_GpsText != value)
                {
                    _GpsText = value;
                    OnPropertyChanged();
                }
            }
        }
        public string GpsTextColor
        {
            get
            {
                return _GpsTextColor;
            }
            set
            {
                if (_GpsTextColor != value)
                {
                    _GpsTextColor = value;
                    OnPropertyChanged();
                }
            }
        }

        //public string LocationText
        //{
        //    private set
        //    {
        //        if (LocationNowText != value)
        //        {
        //            LocationNowText = value;
        //            OnPropertyChanged();
        //        }

        //    }
        //    get
        //    {
        //        return LocationNowText;
        //    }
        //}      
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public static async Task<List<Audit>> GetAudit()
        {
            try
            {
                string url = basic_url + ControllerNameLeaveRecord + "Review_LeaveRecord?hash_company=" + Preferences.Get("CompanyHash", "") + "&hash_account=" + Preferences.Get("HashAccount", "");
                response = await client.GetAsync(url);
                Console.WriteLine(url);
                var result = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), result);
                List<Audit> Audits = JsonConvert.DeserializeObject<List<Audit>>(result);
                int i = 0;
                foreach (var a in Audits)
                {
                    Audits[i].Time = a.StartDate.ToString("yyyy/MM/dd HH:mm") + " - " + a.EndDate.ToString("yyyy/MM/dd HH:mm");
                    Console.WriteLine(a.LeaveRecordId);
                    i++;
                }           
                return Audits.Count!=0? Audits:null;
            }
            catch (Exception)
            {
                return null;              
            }
        }
    }
}
