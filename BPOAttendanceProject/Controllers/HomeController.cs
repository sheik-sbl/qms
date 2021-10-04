using BPOAttendanceProject.Filters;
using BPOAttendanceProject.Models;
using ExcelDataReader;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace BPOAttendanceProject.Controllers
{


    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {

            //GetNotifications();
           // SendEmail();
            //SendMilestoneNotification(); 
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View("LoginPage");
        }


        private void SendMilestoneNotification()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                // string date = DateTime.Today.ToString("dd/MM/yyyy");

                //dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var addedDate = DateTime.Now.Date.AddDays(2);
                var Ddate = addedDate.ToString("yyyy-MM-dd");
                DataTable dt = new DataTable();
                string query = "SELECT `projectcode`,`projectname`,MilestoneName, MilestoneDate,ProjectMilestoneItem.id FROM `ProjectInformation`,ProjectMilestoneItem WHERE  ProjectInformation`.id=ProjectMilestoneItem.projectinfoid  and ProjectMilestoneItem.mailsent=0 and  `MilestoneDate`='" + Ddate + "';";
                using (MySqlConnection mConnection = new MySqlConnection(constr))
                {
                    mConnection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, mConnection);
                    adapter.Fill(dt);
                }

                string sdata;
                string tabl = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    string ww = "<table><tr><td>Code </td><td>Project Name</td><td>Milestone Date</td><td>Milestone Name</td></tr>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tabl = tabl + "<tr><td>" + dt.Rows[i]["projectcode"] + "</td><td>" + dt.Rows[i]["projectname"] + "</td><td>" + dt.Rows[i]["MilestoneDate"] + "</td><td>" + dt.Rows[i]["MilestoneName"] + "</td></tr>";
                    }
                    sdata = ww + tabl + "</table>";
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new MailAddress("mistool@sblinfo.org ");
                    mailMessage.Subject = "Project Milestone Date is due";




                    mailMessage.Body = "The following project milestone date is coming.Please check it <br/>Details are <br/>" + sdata;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(new MailAddress(ConfigurationManager.AppSettings["MileEmailId"]));
                   
                    //mailMessage.To.Add(new MailAddress("keerthibabu@saibposervices.com"));

                    SmtpClient smtp = new SmtpClient();
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 587;
                    //smtp.EnableSsl = true;
                    //smtp.UseDefaultCredentials = false;
                    smtp.Host = "relay-hosting.secureserver.net";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;

                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = mailMessage.From.Address;
                    NetworkCred.Password = "x@VDl12639d6";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;

                    smtp.Send(mailMessage);


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        string Command = "UPDATE `ProjectMilestoneItem` set mailsent=1  where Id=" + dt.Rows[i]["id"];
                        using (MySqlConnection mConnection = new MySqlConnection(constr))
                        {
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                            {
                                myCmd.ExecuteNonQuery();

                            }

                        }
                    }


                }


                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress("mistool@sblinfo.org");
                //    mail.To.Add("nisha.v@sblcorp.com");
                //    mail.Subject = "Hello World";
                //    mail.Body = "<h1>Hello</h1>";
                //    mail.IsBodyHtml = true;


                //    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                //    {
                //        smtp.Credentials = new NetworkCredential("mistool@sblinfo.org", "x@VDl12639d6");
                //        smtp.EnableSsl = true;
                //        smtp.Send(mail);
                //    }
                //}


            }
            catch (Exception ex)
            {
            }

        }


        private void SendEmail()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                // string date = DateTime.Today.ToString("dd/MM/yyyy");

                //dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var addedDate = DateTime.Now.Date.AddDays(2);
                var Ddate = addedDate.ToString("yyyy-MM-dd");
                DataTable dt = new DataTable();
                string query = "SELECT `ProjectId`,`ProjectName`,`milestonedate`,MilestoneDetails.id FROM `mMilestone`,MilestoneDetails WHERE  mMilestone.`milestoneid`=`MilestoneDetails`.`mmileId`  and MilestoneDetails.mailsent=0 and  `milestonedate`='" + Ddate + "';";
                using (MySqlConnection mConnection = new MySqlConnection(constr))
                {
                    mConnection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, mConnection);
                    adapter.Fill(dt);
                }

                string sdata;
                string tabl = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    string ww = "<table><tr><td>Id</td><td>Name</td><td>MilestoneDate</td></tr>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tabl = tabl + "<tr><td>" + dt.Rows[i]["ProjectId"] + "</td><td>" + dt.Rows[i]["ProjectName"] + "</td><td>" + dt.Rows[i]["milestonedate"] + "</td></tr>";
                    }
                    sdata = ww + tabl + "</table>";
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new MailAddress("mistool@sblinfo.org ");
                    mailMessage.Subject = "Project Milestone Date is due";




                    mailMessage.Body = "The following project milestone date is coming.Please check it <br/>Details are <br/>" + sdata;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(new MailAddress(ConfigurationManager.AppSettings["MileEmailId"]));
                    mailMessage.To.Add(new MailAddress("keerthibabu@saibposervices.com"));

                    SmtpClient smtp = new SmtpClient();
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 587;
                    //smtp.EnableSsl = true;
                    //smtp.UseDefaultCredentials = false;
                    smtp.Host = "relay-hosting.secureserver.net";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;

                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = mailMessage.From.Address;
                    NetworkCred.Password = "x@VDl12639d6";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;

                    smtp.Send(mailMessage);


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        string Command = "UPDATE `MilestoneDetails` set mailsent=1  where Id=" + dt.Rows[i]["id"];
                        using (MySqlConnection mConnection = new MySqlConnection(constr))
                        {
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                            {
                                myCmd.ExecuteNonQuery();

                            }

                        }
                    }


                }


                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress("mistool@sblinfo.org");
                //    mail.To.Add("nisha.v@sblcorp.com");
                //    mail.Subject = "Hello World";
                //    mail.Body = "<h1>Hello</h1>";
                //    mail.IsBodyHtml = true;


                //    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                //    {
                //        smtp.Credentials = new NetworkCredential("mistool@sblinfo.org", "x@VDl12639d6");
                //        smtp.EnableSsl = true;
                //        smtp.Send(mail);
                //    }
                //}


            }
            catch (Exception ex)
            {
            }

        }




        private void GetNotifications()
        {

            List<string> location = new List<string>();
            List<string> Teamlead = new List<string>();
            


            // adding elements in firstlist 
            location.Add("TVM");
            location.Add("KNPY");
            location.Add("MDS");
            location.Add("MQC");
            location.Add("MNS");


           
           
            
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string date = DateTime.Today.ToString("dd/MM/yyyy");
            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var Ddate = dtdate.ToString("yyyy-MM-dd");
            DataSet ds=new DataSet();
            string[] strArr = null;
            char[] splitchar = { '/' };
            strArr = date.Split(splitchar);
            if (strArr.Length > 0)
                date = strArr[1] + "/" + strArr[0] + "/" + strArr[2];

            string query = "SELECT count(*) as cnt FROM monthlyconfiguration WHERE    `month`=MONTH(STR_TO_DATE('" + date + "', '%m/%d/%Y'));";
            query += "SELECT  location FROM monthlyconfiguration  where `month`=MONTH(STR_TO_DATE('" + date + "', '%m/%d/%Y'));";
            query += "SELECT  count(*) FROM monthlyconfiguration  where `month`=MONTH(STR_TO_DATE('" + date + "', '%m/%d/%Y'));";
            query += "SELECT  count(*) as usercount FROM muser  where `Roleid`=2 and muser.isactive=true;";
            query += "select  count(distinct `teamleadid`) as ccntteam from production where date='" + Ddate + "';";
            query += "select  Id,username from muser  where `Roleid`=2 and muser.isactive=true;";
            query += "select  distinct `teamleadid`  from production where date='" + Ddate + "';";
            

            

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        
                            sda.Fill(ds);
                            if (int.Parse(ds.Tables[0].Rows[0]["cnt"].ToString()) < 6)
                            BPOAttendanceProject.Utility.CurrentSession.MonthconfigurationCount = ds.Tables[0].Rows[0]["cnt"].ToString();
                            if (ds.Tables[1].Rows.Count >= 0)
                            {
                                foreach (DataRow dr in ds.Tables[1].Rows)
                                {
                                    bool exists = location.Exists(element => element == dr["location"].ToString());
                                    location.Remove(dr["location"].ToString());

                                }
                                BPOAttendanceProject.Utility.CurrentSession.location = location.ToList();
                            }
                            else
                            {
                                BPOAttendanceProject.Utility.CurrentSession.location = null;
                            }

                            if (ds.Tables[5].Rows.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables[5].Rows)
                                {
                                    Teamlead.Add(dr["Id"].ToString());


                                }
                                BPOAttendanceProject.Utility.CurrentSession.Teamlead = Teamlead.ToList();
                            }
                            else
                            {
                                BPOAttendanceProject.Utility.CurrentSession.Teamlead = null;
                            }


                            if (ds.Tables[3].Rows[0]["usercount"].ToString() != ds.Tables[4].Rows[0]["ccntteam"].ToString())
                            {
                                BPOAttendanceProject.Utility.CurrentSession.notifycount = "0";
                                BPOAttendanceProject.Utility.CurrentSession.teamleadCount = ds.Tables[4].Rows[0]["ccntteam"].ToString();

                                if (ds.Tables[6].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[6].Rows)
                                    {

                                        bool exists = Teamlead.Exists(element => element == dr["teamleadid"].ToString());
                                        Teamlead.Remove(dr["teamleadid"].ToString());

                                    }
                                }


                            }

                    }

                }
            }
        }
    }
}