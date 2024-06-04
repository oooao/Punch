using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using people_errandd.Models;
using Xamarin.Essentials;
using System.Linq;
using Xamarin.Forms.Maps;

namespace people_errandd.Models
{
    public class Records : HttpResponse
    {
        public static async Task<bool> ReviewLeaveRecord(int id, bool review)
        {
            List<Audit> reviewLeaveRecords = new List<Audit>();
            Audit reviewLeave = new Audit()
            {
                LeaveRecordsId = id,
                Review = review
            };
            reviewLeaveRecords.Add(reviewLeave);
            
            string jsonData = JsonConvert.SerializeObject(reviewLeaveRecords);
            try
            {
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                string url = basic_url + ControllerNameLeaveRecord + "review_leaveRecord";
                response = await client.PutAsync(url, content);
                var _result = response.Content.ReadAsStringAsync();
                await Log(url, jsonData, response.StatusCode.ToString(), _result.Result);
                if (response.StatusCode.ToString().Equals("OK"))
                {
                    url = basic_url + ControllerNameLeaveRecord + "LeaveRecordId_Get_HashAccount?leaveRecordId=" + id;
                    response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    await Log(url, null, response.StatusCode.ToString(), result);
                    url = basic_url + ControllerNameInformation + result;
                    response = await client.GetAsync(url);
                    result = await response.Content.ReadAsStringAsync();
                    await Log(url, null, response.StatusCode.ToString(), result);
                    List<information> informations = JsonConvert.DeserializeObject<List<information>>(result);
                    var to_email = new List<string>
                    {
                        informations[0].email
                    }; 
                    if (review)
                    {
                        sendEmail(to_email, "差勤打卡請假審核通知", "<h1>您的請假申請</h1>\n<h1>審核通過</h1>\n請至差勤打卡APP請假紀錄進行確認，如有問題請連繫後台");
                    }                        
                    else
                    {
                        sendEmail(to_email, "差勤打卡請假審核通知", "<h1>您的請假申請</h1>\n<h1><font color='red'>審核遭拒</font></h1>\n請至差勤打卡APP請假紀錄進行確認，如有問題請連繫後台");
                    }                            
                    return true;
                }
            }
            catch (Exception)
            {              
            }
            return false;
        }
        public async Task<List<work>> GetWorkRecord(string date )
        {
            try
            {
                string url = basic_url + ControllerNameWorkRecord + "GetEmployeeAllWorkRecords/" + Preferences.Get("HashAccount", "");
                response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), result);
                List<work> workRecords = JsonConvert.DeserializeObject<List<work>>(result);               
                int i = 0;
                foreach (var work in workRecords)
                {
                    switch (work.workTypeId)
                    {
                        case 1:
                            workRecords[i].status = "上班";
                            workRecords[i].statuscolor = "#5C76B1";
                            workRecords[i].image = "worker.png";
                            break;
                        case 2:
                            workRecords[i].status = "下班";
                            workRecords[i].statuscolor = "#CA4848";
                            workRecords[i].image = "workeroff.png";
                            break;
                        default:
                            break;
                    }
                    workRecords[i].time = workRecords[i].createdTime.ToString();
                    Console.WriteLine(workRecords[i].time);
                    i++;
                }
                workRecords = workRecords.OrderByDescending(work => work.time).ToList();
                workRecords = workRecords.Where(work => work.time.Contains(date)).ToList();
                //List<work> workRecord;

                return workRecords;
            }
            catch (Exception)
            {
                Console.WriteLine("fail");
                //throw;
            }
            return null;
        }
        public async Task<List<GoOut>> GetGoOutsRecord(string date )
        {
            try
            {
                string url = basic_url + ControllerNameTripRecord + Preferences.Get("HashAccount", "");
                response = await client.GetAsync(url);                   
                var result = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), result);
                List<GoOut> GoOutRecords = JsonConvert.DeserializeObject<List<GoOut>>(result);
                GoOutRecords = GoOutRecords.Where(GoOut => GoOut.createdTime.ToString().Contains(date)).ToList();
                return GoOutRecords;
            }
            catch (Exception)
            {
                Console.WriteLine("Get GoOut fail");
                //throw;
            }
            return null;
        }
        public async Task<List<DayOff>> GetLeaveRecord(DateTime date , string employee_HashAccount)
        {
            try
            {
                string url = basic_url + ControllerNameLeaveRecord + "GetEmployeeAllLeaveRecords/" + Preferences.Get("HashAccount", "");
                response =await client.GetAsync(url);                 
                var result = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), result);
                List<DayOff> DayOffRecords = JsonConvert.DeserializeObject<List<DayOff>>(result);
                int i = 0;
                foreach(var rs in DayOffRecords)
                {
                    switch (rs.Leavetypeid)
                    {
                        case 1:
                            DayOffRecords[i].LeaveType = "事假";
                            break;
                        case 2:
                            DayOffRecords[i].LeaveType = "病假";
                            break;
                        case 3:
                            DayOffRecords[i].LeaveType = "喪假";
                            break;
                        case 4:
                            DayOffRecords[i].LeaveType = "產假";
                            break;
                        case 5:
                            DayOffRecords[i].LeaveType = "生理假";
                            break;
                        case 6:
                            DayOffRecords[i].LeaveType = "流產假";
                            break;
                        case 7:
                            DayOffRecords[i].LeaveType = "產前假";
                            break;
                        case 8:
                            DayOffRecords[i].LeaveType = "陪產假";
                            break;
                        case 9:
                            DayOffRecords[i].LeaveType = "特休";
                            break;
                    }                    
                    rs.status = rs.Review!=null? (bool)rs.Review ? "已審核" : "已拒絕":"待審核";                    
                    i++;
                }
                //DayOffRecords = DayOffRecords.Where(DayOff => DayOff.StartDate.ToString().Contains(date)).ToList();
                DayOffRecords = DayOffRecords.OrderByDescending(DayOff => DayOff.StartDate).ToList();
                DayOffRecords = DayOffRecords.Where(DayOff => date.CompareTo(DayOff.StartDate)>=0 && date.CompareTo(DayOff.EndDate)<=0).ToList();
                return DayOffRecords;
            }
            catch (Exception)
            {
                Console.WriteLine("Get DayOff fail");
                //throw;
            }
            return null;
        }
        public async Task<List<GoOut>> GetAdvanceGoOutsRecord(string date)
        {
            Geocoder geoCoder = new Geocoder();
            try
            {
                string url = basic_url + ControllerNameTrip2Record + "GetEmployeeTrip2Records/" + Preferences.Get("HashAccount", "");
                response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                await Log(url, null, response.StatusCode.ToString(), result);
                List<GoOut> GoOutRecords = JsonConvert.DeserializeObject<List<GoOut>>(result);
                int i = 0;              
                foreach (var goOut in GoOutRecords)
                {          
                    switch (goOut.trip2TypeId)
                    {
                        case 1:
                            GoOutRecords[i].status = "公出開始";
                            GoOutRecords[i].statuscolor = "#FCCE82";
                            GoOutRecords[i].advanceimage = "startgoout.png";
                            break;
                        case 2:
                            GoOutRecords[i].status = "到站";
                            GoOutRecords[i].statuscolor = "#DE5D5D";
                            GoOutRecords[i].advanceimage = "stop.png";
                            break;
                        case 3:
                            GoOutRecords[i].status = "公出結束";
                            GoOutRecords[i].statuscolor = "#83A6F2";
                            GoOutRecords[i].advanceimage = "finishgoout.png";
                            break;
                        default:
                            break;
                    }
                    GoOutRecords[i].address = goOut.address.Substring(5);
                    i++;                   
                }
                GoOutRecords = GoOutRecords.OrderByDescending(GoOut => GoOut.createdTime).ToList();
                GoOutRecords = GoOutRecords.Where(GoOut => GoOut.createdTime.ToString().Contains(date)).ToList();
                return GoOutRecords;
            }
            catch (Exception)
            {
                Console.WriteLine("Get GoOut fail");
                //throw;
            }
            return null;
        }
    }
}




