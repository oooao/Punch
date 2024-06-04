using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using people_errandd.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using people_errandd.Views;



namespace people_errandd.ViewModels
{
    class InformationViewModel : HttpResponse
    {
        public async Task<bool> UpdateInformationRecord(string _name, string _phone, string _email)
        {
            List<information> informations = new List<information>();
            information information = new information()
            {
                hashaccount = Preferences.Get("HashAccount", ""),
                name = _name,
                phone = _phone,
                email = _email,
                img = ""
            };
            informations.Add(information);
            try
            {
                var WorkRecord = JsonConvert.SerializeObject(informations);
                HttpContent content = new StringContent(WorkRecord);
                string url = basic_url + ControllerNameInformation + "update_information";
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                response = await client.PutAsync(url, content);
                var result = response.Content.ReadAsStringAsync();
                await Log(url, WorkRecord, response.StatusCode.ToString(), result.Result);
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
        public async Task<information> GetInformation(string _HashAccount)
        {
            try
            {
                string url = basic_url + ControllerNameInformation + _HashAccount;
                response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                List<information> informations = JsonConvert.DeserializeObject<List<information>>(result);
                await Log(url, null, response.StatusCode.ToString(), result);
                return informations[0];
            }
            catch (Exception)
            {
            }
            return null;
        }
        public async Task<string> GetUserName(string _HashAccount)
        {
            try
            {
                string url = basic_url + ControllerNameInformation + _HashAccount;
                response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), result);
                if (response.StatusCode.ToString() == "OK")
                {
                    List<information> information = JsonConvert.DeserializeObject<List<information>>(result);
                    Preferences.Set("UserName", information[0].name);
                    return information[0].name;
                }

            }
            catch (Exception)
            {
            }
            return null;
        }
        public async Task<bool> ConfirmEmail(string _Email)
        {
            try
            {
                string url = basic_url + ControllerNameInformation + "BoolEmployeeInformationEmail?hash_company=" + Preferences.Get("CompanyHash", "") + "&email=" + _Email;
                response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), result);
                return Convert.ToBoolean(result);
            }
            catch (Exception)
            {
            }
            return true;
        }
    }
}


