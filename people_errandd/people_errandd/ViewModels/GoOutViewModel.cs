using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using people_errandd.Models;
using Xamarin.Essentials;

namespace people_errandd.ViewModels
{
    class GoOutViewModel : HttpResponse
    {
        public async Task<bool> PostGoOut(DateTime _StartTime, DateTime _EndTime, string _location, string _reason)
        {
            List<GoOut> goOuts = new List<GoOut>();
            GoOut goOut = new GoOut()
            {
                hashAccount = Preferences.Get("HashAccount", ""),
                Location = _location,
                Reason = _reason,
                StartDate = _StartTime,
                EndDate = _EndTime,
            };
            goOuts.Add(goOut);
            try
            {
                var GoOutRecord = JsonConvert.SerializeObject(goOuts);
                HttpContent content = new StringContent(GoOutRecord);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                string url = basic_url + ControllerNameTripRecord + "add_TripRecord";
                response = await client.PostAsync(url, content);
                var result = response.Content.ReadAsStringAsync();
                await Log(url, GoOutRecord, response.StatusCode.ToString(), result.Result);
                if (response.StatusCode.ToString() == "OK")
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> PostGoOut(int TripType)
        {
            List<GoOut> goOuts = new List<GoOut>();
            GoOut goOut = new GoOut()
            {
                trip2TypeId = TripType,
                hashAccount = Preferences.Get("HashAccount", ""),
                coordinateX = App.Latitude,
                coordinateY = App.Longitude,
            };
            goOuts.Add(goOut);
            try
            {
                var GoOutRecord = JsonConvert.SerializeObject(goOuts);
                HttpContent content = new StringContent(GoOutRecord);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                string url = basic_url + ControllerNameTrip2Record + "add_trip2Record";
                response = await client.PostAsync(url, content);
                var result = response.Content.ReadAsStringAsync();
                await Log(url, GoOutRecord, response.StatusCode.ToString(), result.Result);
                if (response.StatusCode.ToString() == "OK")
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        public static async Task<bool> ConfirmArrival()
        {
            try
            {
                string url = basic_url + ControllerNameCompany +"GetCompanySettingTrip2Enabled/"+Preferences.Get("CompanyHash","");
                response = await client.GetAsync(url);
                GetResponse = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), GetResponse);
                return Convert.ToBoolean(GetResponse);
            }
            catch (Exception)
            {
                return false;
            }         
        }
    }

}
