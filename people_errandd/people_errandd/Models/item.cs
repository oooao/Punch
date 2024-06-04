using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace people_errandd.Models
{
    public class work
    {
        //  public int ID { get; set; }

        public string hashAccount { get; set; }
        public int workTypeId { get; set; }
        public double coordinateX { get; set; }
        public double coordinateY { get; set; }

        public bool enabled { get; set; }
        public DateTime createdTime { get; set; }
        public string status { get; set; }
        public string statuscolor { get; set; }
        public string time { get; set; }
        public string image { get; set; }

    }
    public class Address
    {
        public string address { get; set; }
        public string CompanyHash { get; set; }
        public double coordinateX { get; set; }
        public double coordinateY { get; set; }

    }
    public class employee
    {
        public string employeeId { get; set; }
        public string hashaccount { get; set; }
        public string name { get; set; }
        public string phonecode { get; set; }
        public DateTime createdTime { get; set; }
        public string companyhash { get; set; }

        public bool enabled { get; set; }
    }
    public class DayOff
    {
        public string hashAccount { get; set; }
        public int Leavetypeid { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool? Review { get; set; }
        public string status { get; set; }
        public string image { get { return "nerd.png"; } }
        public DateTime createdTime { get; set; }
    }
    public class GoOut
    {
        public string hashAccount { get; set; }
        public string Location { get; set; }
        public string address { get; set; }
        public string Reason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string image { get { return "goout2.png"; } }
        public DateTime createdTime { get; set; }
        public double coordinateX { get; set; }
        public double coordinateY { get; set; }
        public int trip2TypeId { get; set; }
        public string status { get; set; }
        public string statuscolor { get; set; }
        public string advanceimage { get; set; }
    }
    public class information
    {
        public string name { get; set; }
        public string hashaccount { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string img { get; set; }

        public string department { get; set; }

        public string jobtitle { get; set; }

    }
    public class Language
    {
        public string language { get; set; }
    }
    public class Audit
    {
        public int LeaveRecordId { get; set; }
        public int LeaveRecordsId { get; set; }
        public string Name { get; set; }
        public string LeaveType { get; set; }
        public string Time { get; set; }
        public string Reason { get; set; }
        public bool? Review { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class Log
    {
        public string url { get; set; }
        public string input { get; set; }
        public string response { get; set; }
        public string output { get; set; }
    }
    public class defence
    {
        public string hashaccount { get; set; }
        public int LoginNumber{get;set;} 
    }

}