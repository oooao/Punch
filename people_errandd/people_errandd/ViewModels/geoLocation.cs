using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using people_errandd.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace people_errandd.ViewModels
{
    class geoLocation:HttpResponse
    {
        public CancellationTokenSource cts;
        Geocoder geoCoder = new Geocoder();
       // MainPageViewModel main = new MainPageViewModel();
        public static string LocationNowText { get; set; }       
        public async Task<(double, double)> GetLocation(string status)
        {
            try
            {
                if (status != "Back")
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    return (location.Latitude, location.Longitude);
                }
                else
                {
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest()
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                   // await GetLocationText(location.Latitude, location.Longitude);
                    Preferences.Set("GpsText", "定位已開啟");
                    Preferences.Set("GpsButtonColor", "#5C76B1");
                    
                    return (location.Latitude, location.Longitude);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Preferences.Set("GpsText", "定位未開啟");
                Preferences.Set("GpsButtonColor", "#CA4848");
                LocationNowText = "";
            }
            //try
            //{
            //    if (status == "Back")
            //    {
            //        var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
            
            //        var location = await Geolocation.GetLocationAsync(request, cts.Token);
            //        Console.WriteLine("getLocation");
            //        return (location.Latitude, location.Longitude);
            //    }
            //    else
            //    {
            //        var location = await Geolocation.GetLastKnownLocationAsync();
            //        return (location.Latitude, location.Longitude);
            //    }

            //}
            //catch (Exception)
            //{
            //    Preferences.Set("gpsText", "定位未開啟");
            //    Preferences.Set("GpsButtonColor", "#CA4848");
            //    Console.WriteLine("ERROR");
            //}
            return (0, 0);
        }        
        public async Task<bool> GetCurrentLocation(double X,double Y)
        {
            try
            {
                string url = basic_url + ControllerNameCompany + "GetCompanySettingWorkRecordEnabled/" + Preferences.Get("CompanyHash", "");
                response = await client.GetAsync(url);
                GetResponse = await response.Content.ReadAsStringAsync();//將JSON轉成string
                await Log(url, null, response.StatusCode.ToString(), GetResponse);
                if (Convert.ToBoolean(GetResponse))
                {
                    return true;
                }
                url = basic_url + ControllerNameCompany + "Get_CompanyAddress?company_hash=" + Preferences.Get("CompanyHash", "");
                response = await client.GetAsync(url);
                Console.WriteLine(response.StatusCode.ToString());
                if (response.StatusCode.ToString() == "OK")
                {
                    GetResponse = await response.Content.ReadAsStringAsync();//將JSON轉成string
                    await Log(url, null, response.StatusCode.ToString(), GetResponse);
                    List<Address> addresses = JsonConvert.DeserializeObject<List<Address>>(GetResponse);
                    Preferences.Set("CompanyAddress", addresses[0].address);
                    Location locationCompany = new Location(addresses[0].coordinateX, addresses[0].coordinateY);
                    Location locationNow = new Location(X,Y);
                    double distance = Location.CalculateDistance(locationNow, locationCompany, DistanceUnits.Kilometers);
                    url = basic_url+ControllerNameCompany+"GetCompanyPositionDifference/"+Preferences.Get("CompanyHash","");
                    response = await client.GetAsync(url);
                    GetResponse = await response.Content.ReadAsStringAsync();
                    double x = Convert.ToDouble(GetResponse);
                    await Log(url, null, response.StatusCode.ToString(), GetResponse);
                    Console.WriteLine(x / 1000);
                    if (distance < x/1000)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        public async  Task GetLocationText(double X , double Y)
        {
            try
            {
                Position position = new Position(X, Y);
                IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                //if(DeviceInfo.Platform == DevicePlatform.Android && await GetTraslateText(possibleAddresses.FirstOrDefault()))
                //{
                //    LocationNowText = possibleAddresses.FirstOrDefault().Substring(5);
                //}
                //else
                //{
                //    LocationNowText = possibleAddresses.FirstOrDefault();
                //}
                LocationNowText = possibleAddresses.FirstOrDefault();
                Console.WriteLine(LocationNowText);
            }
            catch (Exception)
            {
            }
           
        }
        public async  Task<bool> GetTraslateText(string _Text)
        {
            string _language;
            object[] body = new object[] { new { Text = _Text } };
            var requestBody = JsonConvert.SerializeObject(body);
            // client.DefaultRequestHeaders.Add();
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + LanguageRoute);//uuuuuurl
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");//格式
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);//add 金鑰
                request.Headers.Add("Ocp-Apim-Subscription-Region", "global");//add 區域
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);//api
                string result = await response.Content.ReadAsStringAsync();
               List<Language> language= JsonConvert.DeserializeObject<List<Language>>(result);
                _language = language[0].language;
            }
            return _language == "zh-Hant";
        }

    }

}
