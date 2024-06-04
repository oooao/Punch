using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using people_errandd.Models;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;

namespace people_errandd.Models
{
    class Login : HttpResponse, INotifyPropertyChanged//繼承HttpResponse,實作INPC
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //private static string GetResponse;
        public async Task<bool> ConfirmCompanyHash(string code)
        {          
            try
            {
                string url = basic_url + ControllerNameCompany + code;//HTTP URL
                response = await client.GetAsync(url);//HTTP GET
                if (response.StatusCode.ToString() == "OK")
                {
                    GetResponse = await response.Content.ReadAsStringAsync();//將JSON轉成string
                    string[] _CompanyInformation = GetResponse.Split('\n');//分割字串
                    Preferences.Set("CompanyHash", _CompanyInformation[0]);
                    await Log(url, null, response.StatusCode.ToString(), GetResponse);
                    return true;
                }
            }
            catch (Exception)
            {
            }        
            return false;
        }
        public async Task<bool> ConfirmUUID(string UUID)//判斷裝置UUID是否存在資料庫
        {
            string url = basic_url + "Employees?phone_code=" + UUID + "&company_hash=" + Preferences.Get("CompanyHash", "");
            response = await client.GetAsync(url);
            if (response.StatusCode.ToString() == "OK")
            {
                GetResponse = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), GetResponse);
                return true;
            }
            return false;
        }
        public async Task<bool> SetUUID()
        {
            var _UUID = Preferences.Get("uuid", "");//存取裝置UUID  
            List<employee> employees = new List<employee>();
            employee employee = new employee()
            {
                companyhash =Preferences.Get("CompanyHash",""),
                phonecode = _UUID
            };
            employees.Add(employee);
            try
            {
                string url = basic_url + ControllerNameEmployee + "regist_employee";
                var str = JsonConvert.SerializeObject(employees);//序列化成json
                HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");//set content-Type header
                response = await client.PostAsync(url, content);//HTTP POST
                var result = response.Content.ReadAsStringAsync();
                await Log(url, str, response.StatusCode.ToString(),result.Result);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("SetUUIDError");
                return false;
            }
        }
        public async Task<string> GetHashAccount(string uuid)
        {
            string url = basic_url + "Employees?phone_code=" + uuid + "&company_hash=" + Preferences.Get("CompanyHash", "");
            response = await client.GetAsync(url);
            GetResponse = await response.Content.ReadAsStringAsync();//將JSON轉成string
            await Log(url, null, response.StatusCode.ToString(), GetResponse);
            return GetResponse;
        }
        public async Task<bool> SetInformation(string _Name, string _Email)
        {
            try
            {
                List<information> informations = new List<information>();
                information information = new information()
                {
                    hashaccount = await GetHashAccount(Preferences.Get("uuid", "")),
                    name = _Name,
                    email = _Email
                };
                Console.WriteLine(information.hashaccount);
                informations.Add(information);
                var str = JsonConvert.SerializeObject(informations);
                HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
                string url = basic_url + ControllerNameInformation + "add_information";
                response = await client.PostAsync(url, content);
                Preferences.Set("UserName", _Name);
                Preferences.Set("email", _Email);
                var result = response.Content.ReadAsStringAsync();
                if (response.StatusCode.ToString() == "OK")
                {
                    await Log(url, str, response.StatusCode.ToString(), result.Result);
                    return true;
                }
                else
                {
                    Console.WriteLine("SetInformationError");
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("error");
                return false;
            }
        }
        public async Task<bool> Reviewed()
        {
            try
            {
                string _Ha = await GetHashAccount(Preferences.Get("uuid", ""));
                string url = basic_url + ControllerNameInformation + _Ha;
                response = await client.GetAsync(url);
                GetResponse = await response.Content.ReadAsStringAsync();
                //information UserInf = JsonConvert.DeserializeObject<information>(GetResponse);
                string[] _UserInf = GetResponse.Split(',');
                await Log(url, null, response.StatusCode.ToString(), GetResponse);
                return _UserInf[1] != null;                
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static async Task<bool> AccountEnabled()
        {
            try
            {
                string url = basic_url + ControllerNameEmployee + "get_employee_enabled/" + Preferences.Get("HashAccount", "");
                response = await client.GetAsync(url);
                if (response.StatusCode.ToString() == "NoContent")
                {
                    return false;
                }
                var result = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), result);
                if (response.StatusCode.ToString() == "OK")
                {
                    return Convert.ToBoolean(result);
                }
                return true;                
            }
            catch(Exception)
            {
                return true;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                // 若 PropertyChanged 有被綁定，則將會執行這個事件，
                // 以進行頁面控制項的內容更新
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}