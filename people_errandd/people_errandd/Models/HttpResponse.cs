using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using people_errandd.Models;
using System.Text;
using System.Net.Mail;

namespace people_errandd.Models
{

    public class HttpResponse
    {
        //static company c = new company();
        public static HttpClient client = new HttpClient();

        public static HttpResponseMessage response = new HttpResponseMessage();
        public static readonly string basic_url = "http://60.249.202.153:7000/api/";//主機ㄉURL  
        public static readonly string ControllerNameCompany = "Companies/";//Company api
        public static readonly string ControllerNameEmployee = "employees/";//Employee api
        public static readonly string ControllerNameWorkRecord = "EmployeeWorkRecords/";//workRecord api
        public static readonly string ControllerNameLeaveRecord = "EmployeeLeaveRecords/";//LeaveRecord api
        public static readonly string ControllerNameTripRecord = "EmployeeTripRecords/";//TripRecord api
        public static readonly string ControllerNameInformation = "EmployeeInformations/";//EmployeeInformations api
        public static readonly string ControllerNameTrip2Record = "EmployeeTrip2Record/";//TripRecord api
        public static readonly string subscriptionKey = "2f553f597a914324882f8f0f3db42a41";//azure translate api key
        public static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";//azure translate url
        public static readonly string LanguageRoute = "/detect?api-version=3.0";//language route             
        public static string GetResponse { get; set; }

        public static void sendEmail(List<string> to_email, string email_subject, string email_body)//寄EMAIL
        {
            try
            {
                MailMessage mail = new MailMessage();
                //前面是發信email後面是顯示的名稱
                mail.From = new MailAddress("C108118242@nkust.edu.tw", "差勤打卡");

                //收信者email
                if (to_email != null && to_email.Count > 0)//防呆
                {
                    foreach (string mailAddress in to_email)
                    {
                        mail.To.Add(new MailAddress(mailAddress));
                    }
                }

                //設定優先權
                mail.Priority = MailPriority.Normal;

                //標題
                mail.Subject = email_subject;

                //內容
                mail.Body = email_body;

                //內容使用html
                mail.IsBodyHtml = true;

                //設定gmail的smtp (這是google的)
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

                //您在gmail的帳號密碼
                MySmtp.Credentials = new System.Net.NetworkCredential("like3yy@gmail.com", "nkust.edu.tw");

                //開啟ssl
                MySmtp.EnableSsl = true;

                //發送郵件
                MySmtp.SendMailAsync(mail);

                //放掉宣告出來的MySmtp
                MySmtp = null;

                //放掉宣告出來的mail
                // mail.Dispose();
                Console.WriteLine("成功發送EMAIL通知!");
            }
            catch (Exception)
            {
                Console.WriteLine("發送EMAIL通知失敗!");
            }
        }
        public static async Task Log(string _url,string  _input,string _response,string _output)
        {
            List<Log> logs = new List<Log>();
            Log log = new Log()
            {
                url = _url,
                input = _input,
                response = _response,
                output = _output
            };
            logs.Add(log);
            try
            {
                var Record = JsonConvert.SerializeObject(logs);
                HttpContent content = new StringContent(Record);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(basic_url + ControllerNameCompany + "AddLog", content);
            }
            catch (Exception)
            {
            }
        }
        public static async Task<bool> Defence()
        {
            try
            {
                Console.WriteLine(Preferences.Get(Preferences.Get("HashAccount", "") + "NOS", ""));
            int number;
            if (!Preferences.ContainsKey(Preferences.Get("HashAccount", "")+"NOS"))
            {
                number = 1;
                Console.WriteLine("1");
            }
            else
            {
                Console.WriteLine("2");
                number =  Convert.ToInt32(Preferences.Get(Preferences.Get("HashAccount", "")+"NOS",""));
                number++;
            }
            List<defence> defences = new List<defence>();
            var d = new defence()
            {
                hashaccount = Preferences.Get("HashAccount", ""),
                LoginNumber = number
            };
            defences.Add(d);
                var str = JsonConvert.SerializeObject(defences);
                HttpContent content = new StringContent(str);
                string url = basic_url + ControllerNameEmployee + "update_employee_login_number";
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                response = await client.PutAsync(url, content);
                var result = response.Content.ReadAsStringAsync();
                await Log(url, str, response.StatusCode.ToString(), result.Result);               
                if (response.StatusCode.ToString() == "OK")
                {
                    if (Convert.ToBoolean(result.Result))
                    {
                        Preferences.Set(Preferences.Get("HashAccount", "")+"NOS",Convert.ToString(number));
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
            }
            return true;
        }
    }
}