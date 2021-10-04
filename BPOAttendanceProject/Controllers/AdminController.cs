using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BPOAttendanceProject.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using BPOAttendanceProject.Filters;
using ExcelDataReader;
using System.IO;
using System.Web.Helpers;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Globalization;
using SpreadsheetLight;
using System.Web.UI.WebControls;
using System.Web.Hosting;
using ClosedXML.Excel;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Collections;
using System.Web.Security;
using System.Security.Principal;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;





namespace BPOAttendanceProject.Controllers
{
    [UserFilter]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/


        public ActionResult SaveEmployee(string firstname)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var root = "~/Content/Images";
                    bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                    if (!folderpath)
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
                    }
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), fileName);
                        files.SaveAs(path);
                        return Json(new { success = true, message = "File uploaded successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false, message = "Please select a file !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }




        public ActionResult GetConfigurationPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            ProjectConfiguration Model = new ProjectConfiguration();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT * FROM projectconfiguration where Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.Projectcode = reader.GetString(1);
                    Model.Eventcode = reader.GetString(2);
                    Model.Process = reader.GetString(3);
                    Model.ProductionPlannedHr = reader.GetDouble(4);
                    Model.location = reader.GetString(5);
                    Model.monthid = reader.GetInt32(6);
                    Model.monthname = reader.GetString(7);
                    Model.locationId = reader.GetInt32(8);
                    Model.year = reader.GetString(9);
                }

            }

            return PartialView("/Views/Admin/_ProjectConfiguration.cshtml", Model);
        }


        public ActionResult NotificationList()
        {

            return View();
        }

        public string CopyConfig(OpenCpyconfigModel model)
        {
            string Result = string.Empty;
            int CpylocationId;
            double CpyProductionPlannedHr;
            string CpyProjectcode, CpyEventcode, CpyProcess, Cpylocation, CpyToyear, monthid;
            Result = "NotOk";
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            string FYear = string.Empty;
            if (model.FromYear == "1")
            {
                FYear = "2019";
            }
            else if (model.FromYear == "2")
            {
                FYear = "2020";
            }

            //string SelectCommand = "SELECT EXISTS(SELECT * FROM projectconfiguration WHERE monthid='" + model.FromMonthId + "' and year='" + model.FromYear + "' and Process= '" + process + "' and location= '" + location + "' and monthname='" + monthname + "' and year=" + year + ") as exist";
            string SelectCommand = "SELECT * FROM projectconfiguration where month='" + model.FromMonthId + "'and year='" + FYear + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                using (MySqlDataAdapter adpfill = new MySqlDataAdapter(SelectCommand, mConnection))
                {
                    adpfill.Fill(dt);

                }
                if (dt.Rows.Count > 0)
                {
                    monthid = model.ToMonthId.ToString();
                    string TYear = string.Empty;
                    if (model.ToYear == "1")
                    {
                        TYear = "2019";
                    }
                    else if (model.ToYear == "2")
                    {
                        TYear = "2020";
                    }
                    CpyToyear = TYear;


                    string CpyTomonthname = string.Empty;
                    if (monthid == "1")
                    {
                        CpyTomonthname = "January";
                    }
                    else if (monthid == "2")
                    {
                        CpyTomonthname = "February";
                    }
                    else if (monthid == "3")
                    {
                        CpyTomonthname = "March";
                    }
                    else if (monthid == "4")
                    {
                        CpyTomonthname = "April";
                    }
                    else if (monthid == "5")
                    {
                        CpyTomonthname = "May";
                    }
                    else if (monthid == "6")
                    {
                        CpyTomonthname = "June";
                    }
                    else if (monthid == "7")
                    {
                        CpyTomonthname = "July";
                    }
                    else if (monthid == "8")
                    {
                        CpyTomonthname = "August";
                    }
                    else if (monthid == "9")
                    {
                        CpyTomonthname = "September";
                    }
                    else if (monthid == "10")
                    {
                        CpyTomonthname = "October";
                    }
                    else if (monthid == "11")
                    {
                        CpyTomonthname = "November";
                    }
                    else if (monthid == "12")
                    {
                        CpyTomonthname = "December";
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CpyProjectcode = dt.Rows[i]["Projectcode"].ToString().Replace("'", "''");
                        CpyEventcode = dt.Rows[i]["Eventcode"].ToString().Replace("'", "''");
                        CpyProcess = dt.Rows[i]["Process"].ToString().Replace("'", "''");
                        CpyProductionPlannedHr = Convert.ToDouble(dt.Rows[i]["ProductionPlannedHr"]);
                        Cpylocation = dt.Rows[i]["location"].ToString().Replace("'", "''");
                        CpylocationId = Convert.ToInt32(dt.Rows[i]["locationId"]);

                        string Command = "INSERT INTO projectconfiguration(`Projectcode`,`Eventcode`, `Process`,`ProductionPlannedHr`,`location`,`month`, `monthname`,`locationId`,year) VALUES ('" + CpyProjectcode + "','" + CpyEventcode + "','" + CpyProcess + "'," + CpyProductionPlannedHr + " ,'" + Cpylocation + "'," + model.ToMonthId + ",'" + CpyTomonthname + "', " + CpylocationId + ",'" + CpyToyear + "' );";
                        // using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                        using (MySqlConnection mmConnection = new MySqlConnection(connString))
                        {
                            mmConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mmConnection))
                            {
                                myCmd.ExecuteNonQuery();


                            }
                        }

                    }

                }


                else
                {
                    Result = "NotOk";
                }

                return Result;

            }



        }


        public ActionResult projectlist()
        {
            return View();
        }

        public static string GetProjectDate()
        {
            //you can get the data from the database
            DataTable dt = new DataTable();
            DataColumn date = new DataColumn("Date", typeof(string));
            DataColumn color = new DataColumn("Color", typeof(string));
            DataColumn tooltip = new DataColumn("Tooltip", typeof(string));

            dt.Columns.Add(date);
            dt.Columns.Add(color);
            dt.Columns.Add(tooltip);

            DataRow dr1 = dt.NewRow();

            dr1["Date"] = "2014-09-1";
            dr1["Color"] = "Red";
            dr1["Tooltip"] = "this is date";


            DataRow dr2 = dt.NewRow();

            dr2["Date"] = "2014-09-12";
            //the color name must be the same as the Css style Name
            dr2["Color"] = "Green";
            dr2["Tooltip"] = "this is date2";

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);

            string result = JsonConvert.SerializeObject(dt, new DataTableConverter());

            return result;


        }







        public ActionResult DollarSettings()
        {

            DollarModel Model = new DollarModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //string Command = "SELECT id,rate as dollarrate,PoundRate, DATE_FORMAT(`dollardate`, '%d/%m/%Y') as dollardate from `dollarsettings` order by Year(dollardate) Desc ,month(dollardate) Desc";

            string Command = "SELECT id,rate as dollarrate,PoundRate, DATE_FORMAT(`dollardate`, '%Y/%m/%d') as dollardate from `dollarsettings` order by Year(dollardate) Desc ,month(dollardate) Desc";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.DollarList = dtt.DataTableToList<DollarModel>();
                return View("DollarSettings", Model);
            }
        }

        public ActionResult GetDollar(string ID)
        {
            int Id = Convert.ToInt16(ID);
            DollarModel Model = new DollarModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT * FROM  `dollarsettings` where Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.id = Id;
                    Model.dollarrate =Convert.ToDouble( reader["rate"]);
                    Model.poundrate = Convert.ToDouble(reader["PoundRate"]);
                   // Model.dollardate = reader["dollardate"].ToString();
                    DateTime sdate = (DateTime)reader["dollardate"];

                    // Then format it as desired. Example:
                    Model.dollardate = sdate.ToString("dd/MM/yyyy");

                }

            }

            return PartialView("/Views/Admin/_EditDollar.cshtml", Model);
        }


        public ActionResult ChangePassword()
        {
            PasswordModel Model = new PasswordModel();
            return View("PasswordList", Model);
        }




        public ActionResult GetResourcePopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            ResourcePlan Model = new ResourcePlan();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT `Projectcode`,`eventcode`,`Startdate`,`Completiondate`,`TotaltargetPercent`,`CompletiontargetRecord`,Totalcharactersavailable,immediatecompletiontarget FROM `ResourcePlan` where Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = Id;
                    Model.Projectcode = reader.GetString(0);
                    Model.eventcode = reader.GetString(1);
                    Model.Startdate = reader.GetString(2);
                    Model.Completiondate = reader.GetString(3);
                    Model.TotaltargetP = reader.GetDouble(4);
                    Model.Completiontarget = reader.GetDouble(5);
                    Model.Totalcharactersavailable = reader.GetDouble(6);
                    Model.Immediatetarget = reader.GetDouble(7);

                }

            }

            return PartialView("/Views/Admin/_ResourcePlan.cshtml", Model);
        }




        public ActionResult GetMonthConfigurationPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            MonthlyConfiguration Model = new MonthlyConfiguration();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT id,month,Configuration,monthname,location,locationId,year,`Revenueconfiguration`,`workingdays` FROM monthlyconfiguration where Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.monthid = reader.GetInt32(1);
                    Model.Configuration = reader.GetInt32(2);
                    Model.monthname = reader.GetString(3);
                    Model.location = reader.GetString(4);
                    Model.locationId = reader.GetInt32(5);
                    Model.year = reader.GetString(6);
                    Model.Revenueconfiguration = reader.GetDouble(7);
                    Model.workingdays = reader.GetInt32(8);



                }

            }

            return PartialView("/Views/Admin/_MonthlyConfiguration.cshtml", Model);
        }

        public ActionResult ProjectResourcePlanList()
        {

            ResourcePlan Model = new ResourcePlan();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  id as Id ,`Projectcode`,`eventcode`,DATE_FORMAT(startdate, '%d/%m/%Y') as startdate,DATE_FORMAT(Completiondate,'%d/%m/%Y') as Completiondate,`TotaltargetPercent`,`CompletiontargetRecord` from `ResourcePlan`";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.ResourcePlanList = dtt.DataTableToList<ResourcePlan>();
                return View("ResourcePlanList", Model);
            }
        }


        public ActionResult HolidayList()
        {

            HolidayModel Model = new HolidayModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT id,`holidayname`,`location`, DATE_FORMAT(`holidaydate`, '%d/%m/%Y') as holidaydate from `Holiday`";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.HolidayList = dtt.DataTableToList<HolidayModel>();
                return View("HolidayList", Model);
            }
        }







        public ActionResult OpenMonthly(string Date)
        {
            //ViewPlanModel viewPlan = new ViewPlanModel();


          

                string projectcode = string.Empty;
                string eventcode = string.Empty;
                Double productionplanned = 0.0;
                Double actualproduction = 0.0;
                int holiday = 0;
                var formattedDate = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                var completiondate = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                int totaltarget = 0;
                string completion = string.Empty;
                Double completiontarget;
                Double immediatecompletiontarget = 0;
                int Achievement = 0;
                int employeecount = 0;
                CultureInfo ci = Thread.CurrentThread.CurrentCulture;

                int year = int.Parse(DateTime.Now.Year.ToString());
                ViewResourcePlan resourcemodel = new ViewResourcePlan();
                string Refdate = string.Empty;

                DateTime dt = DateTime.Now;
                Double actnoofcharacters = 0;

                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                string Command = "SELECT Projectcode,eventcode,DATE_FORMAT(`Startdate`, '%d/%m/%Y') as Startdate,DATE_FORMAT(Completiondate,'%d/%m/%Y') as Completiondate ,TotaltargetPercent,CompletiontargetRecord,Totalcharactersavailable,immediatecompletiontarget from `ResourcePlan`";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {


                    mConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            string DateString = reader.GetString("Completiondate");
                            var split = DateString.Split('/');
                            DateString = split[1] + "/" + split[0] + "/" + split[2];

                            //string iDate = "03/31/2020";
                            //DateTime oDate = Convert.ToDateTime(DateString);
                            DateTime dtnow = DateTime.Now;

                            //var date = dtnow.Date;
                            var currsplit = Date.ToString().Split('/');



                            projectcode = reader.GetString("Projectcode");
                            eventcode = reader.GetString("eventcode");
                            totaltarget = reader.GetInt32("TotaltargetPercent");
                            completion = reader.GetString("Completiondate");
                            completiontarget = reader.GetDouble("CompletiontargetRecord");
                            immediatecompletiontarget = reader.GetDouble("immediatecompletiontarget");
                            actnoofcharacters = reader.GetDouble("Totalcharactersavailable");



                            DateTime end = new DateTime(int.Parse(split[2].ToString()), int.Parse(split[1].ToString()), int.Parse(split[0].ToString()));
                            DateTime start = new DateTime(int.Parse(currsplit[2].ToString()), int.Parse(currsplit[1].ToString()), int.Parse(currsplit[0].ToString()));

                            Refdate = currsplit[2].ToString() + "-" + currsplit[1].ToString() + "-" + currsplit[0].ToString();
                            //DateTime end = new DateTime(int.Parse(currsplit[2].ToString()), int.Parse(currsplit[0].ToString()), int.Parse(currsplit[1].ToString()));


                            string monthName = new DateTime(int.Parse(currsplit[2].ToString()), int.Parse(currsplit[1].ToString()), int.Parse(currsplit[0].ToString())).ToString("MMMM", CultureInfo.InvariantCulture);
                            TimeSpan difference = end - start; //create TimeSpan object




                            int remday = difference.Days;
                            DataTable dtTable = new DataTable("Test");
                            using (MySqlConnection mholiConnection = new MySqlConnection(connString))
                            {
                                mholiConnection.Open();
                                string holicommand = "select count(*) as cnt from `Holiday` where holidaydate >= '" + formattedDate + "' and holidaydate <='" + DateString + "'";
                                MySqlCommand cmdholi = new MySqlCommand(holicommand, mholiConnection);
                                MySqlDataReader cmdreader = cmdholi.ExecuteReader();
                                while (cmdreader.Read())
                                {
                                    holiday = cmdreader.GetInt32("cnt");
                                }
                            }

                            using (MySqlConnection achieveConnection = new MySqlConnection(connString))
                            {
                                achieveConnection.Open();
                                string actualcommand = "select  COALESCE(sum(`Actualproduction`), 0) as Actualproduction  from `production` where `Projectcode` ='" + projectcode + "' and    process='Indexing' and Eventcode='" + eventcode + "' and date <='" + Refdate + "'";
                                MySqlCommand cmdactual = new MySqlCommand(actualcommand, achieveConnection);
                                MySqlDataReader cmdactualreader = cmdactual.ExecuteReader();
                                while (cmdactualreader.Read())
                                {
                                    actualproduction = Convert.ToDouble(cmdactualreader.GetString("Actualproduction"));
                                }

                            }


                            using (MySqlConnection todayachieveConnection = new MySqlConnection(connString))
                            {
                                todayachieveConnection.Open();
                                string todayactualcommand = "select  COALESCE(sum(`actualprodrecord`)/sum(`plannedprodrecord`)*100,0) as plannedproduction ,   count(*) as employeecount  from `productionreport2020` where `Projectcode` ='" + projectcode + "'   and process='Indexing' and Eventcode='" + eventcode + "' and date='" + Refdate + "'";
                                MySqlCommand cmdactualtoday = new MySqlCommand(todayactualcommand, todayachieveConnection);
                                MySqlDataReader cmdactualtreader = cmdactualtoday.ExecuteReader();
                                while (cmdactualtreader.Read())
                                {
                                    Achievement = cmdactualtreader.GetInt32("plannedproduction");
                                    employeecount = cmdactualtreader.GetInt32("employeecount");
                                }

                            }





                            using (MySqlConnection planConnection = new MySqlConnection(connString))
                            {
                                planConnection.Open();
                                string projectcommand = "select `ProductionPlannedHr` from `projectconfiguration` where `Projectcode` ='" + projectcode + "' and location='MDS' and monthname='" + monthName + "' and year=" + year + " and process='Indexing' and Eventcode='" + eventcode + "'";
                                MySqlCommand cmdproject = new MySqlCommand(projectcommand, planConnection);
                                MySqlDataReader cmdprojectreader = cmdproject.ExecuteReader();
                                while (cmdprojectreader.Read())
                                {
                                    productionplanned = Convert.ToDouble(cmdprojectreader.GetString("ProductionPlannedHr"));
                                }

                            }
                            Double balanceAchieve = (completiontarget - actualproduction) / remday;
                            resourcemodel.Projectcode = projectcode + "." + eventcode;
                            resourcemodel.Totaltarget = completiontarget;
                            resourcemodel.TotaltargetP = totaltarget;
                            resourcemodel.Completiondate = completion;
                            resourcemodel.Completiontarget = completiontarget;
                            resourcemodel.Referencedate = Date;
                            resourcemodel.AchievetillRefdate = actualproduction;
                            resourcemodel.Holiday = holiday;
                            resourcemodel.remainday = remday;
                            resourcemodel.balanceAchieve = completiontarget - actualproduction;
                            resourcemodel.balanceAchieveday = Math.Round(balanceAchieve);
                            resourcemodel.Indexingtarget = Math.Round(productionplanned);
                            resourcemodel.Noofhrsreqdday = 0;
                            resourcemodel.Noofassociatereqday = 0;
                            if (productionplanned!=0)
                            {
                            resourcemodel.Noofhrsreqdday = Convert.ToInt32(balanceAchieve / productionplanned);
                            resourcemodel.Noofassociatereqday = Convert.ToInt32(balanceAchieve / productionplanned) / 8;
                            if (Achievement == 0)
                                resourcemodel.todayachievepercent = 0;
                            else
                                resourcemodel.todayachievepercent = (Convert.ToInt32(balanceAchieve / productionplanned) / 8) / int.Parse(Achievement.ToString()) * 100;
                            }
                            resourcemodel.Projectdate = Date;
                            resourcemodel.todayachieve = int.Parse(Achievement.ToString());
                            resourcemodel.associatedeployed = employeecount;
                           
                            resourcemodel.ActualCharacters = actnoofcharacters;

                        }
                    }


                    return PartialView("/Views/Admin/_ViewResourceplan.cshtml", resourcemodel);
                }
            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}

        }

        public ActionResult SaveResourcePlanView(ResourcePlan model)
        {


            int insertresult = 0;
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                   
                        string Result = ManageResource(model);
                        if (Result.Trim('"') == "Ok")
                            TempData["Msg"] = "Successfully Saved!";
                        else
                            TempData["Msg"] = "Unsuccessfull Operation!";
                    
                }
                else
                {
                    string Result = ManageResource(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }


            }
            



            return RedirectToAction("ProjectResourcePlanList");
        }

        public string ManageResource(ResourcePlan model)
        {
            string Result = string.Empty;
            Result = "NotOk";
            DateTime dtstartdate = new DateTime();
            DateTime dtenddate = new DateTime();
            string startdate = model.Startdate;
            string enddate = model.Completiondate;
            string month = string.Empty;
            string day = string.Empty;
            int index1 = startdate.IndexOf(" 12:00:00 AM");
            int indexend=enddate.IndexOf(" 12:00:00 AM");

            if (index1 != -1)
            {
              startdate= startdate.Remove(index1);
              var split = startdate.Split('/');

              if (split[0].Length == 1)
                  month = "0" + split[0].ToString();
              else
                  month = split[0].ToString();

              if (split[1].Length == 1)
                  day = "0" + split[1].ToString();
              else
                  day = split[1].ToString();

              startdate = month + '/' + day + '/' + split[2].ToString();

             dtstartdate = DateTime.ParseExact(startdate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
             
            }

            else
            {
                 dtstartdate = DateTime.ParseExact(startdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            if (indexend != -1)
            {
              enddate= enddate.Remove(indexend);
              var split = enddate.Split('/');

              if (split[0].Length == 1)
                  month = "0" + split[0].ToString();
              else
                  month = split[0].ToString();

              if (split[1].Length == 1)
                  day = "0" + split[1].ToString();
              else
                  day = split[1].ToString();

              startdate = month + '/' + day + '/' + split[2].ToString();


              dtenddate = DateTime.ParseExact(enddate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
             
            }
            else
            {
                  dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
           
               
            var sdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = dtenddate.ToString("yyyy-MM-dd");

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            if (model.Id == 0)
            {

                string Command = "INSERT INTO `ResourcePlan`(`Projectcode`,`eventcode`,`Startdate`,`Completiondate`,`TotaltargetPercent`,`CompletiontargetRecord`,Totalcharactersavailable,immediatecompletiontarget ) VALUES ('" + model.Projectcode + "','" + model.eventcode + "','" + sdate + "','" + cdate + "' ," + model.TotaltargetP + ", " + model.Completiontarget + "," + model.Totalcharactersavailable + "," + model.Immediatetarget + " );";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }
            }
            else
            {

                string Command = "UPDATE ResourcePlan set `Projectcode`='" + model.Projectcode + "', `eventcode`='" + model.eventcode + "',`Startdate`= '" + sdate + "' ,`Completiondate`='" + cdate + "',`TotaltargetPercent`=" + model.TotaltargetP + ",`CompletiontargetRecord`=" + model.Completiontarget + ",Totalcharactersavailable=" + model.Totalcharactersavailable + ",immediatecompletiontarget=" + model.Immediatetarget + "  where ResourcePlan.Id=" + model.Id;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }

            }

            return Result;
        }



        public ActionResult DownloadExcelSheettt(int id)
        {
            NetworkCredential _smtpCredentials;
            string DirectoryPath = HostingEnvironment.MapPath("~/Documents/");
            DataTable dtTable = new DataTable("Test");
            dtTable.Columns.Add("ReportDate ", typeof(string));
            dtTable.Columns.Add("22/07/2019", typeof(string));
            dtTable.Rows.Add("Project Resource Plan", "");
            dtTable.Rows.Add("Project", "P2_61779.IDX.001");
            dtTable.Rows.Add("Total Target", "282193769");
            dtTable.Rows.Add("Immediate Target %", "100");
            dtTable.Rows.Add("Immediate Completion date", "29/07/2019");
            dtTable.Rows.Add("Immediate Completion target", "282193769");
            dtTable.Rows.Add("Reference date", "22/07/2019");
            dtTable.Rows.Add("Achievement till ref. date", 263214205.774);
            dtTable.Rows.Add("Intervening holiday", "1");
            dtTable.Rows.Add("Days remaining", 6);
            dtTable.Rows.Add("Balance to be achieved", "18979563.2254");
            dtTable.Rows.Add("Balanced to achieved/day", 3163260.537);
            dtTable.Rows.Add("Per Hour Indexing target", 6170);
            dtTable.Rows.Add("No of hrs reqd/day", "513");
            dtTable.Rows.Add("No of associates reqd/day", "64");
            dtTable.Rows.Add("Today/s Achievement%", "43");
            dtTable.Rows.Add("No of associates required /day @ today's % achievement", "149");
            dtTable.Rows.Add("Actual no. of Associates deployed", "152");
            dtTable.Rows.Add("Reason For Deviation", "Deadline Extended. Planning to complete project wellahead.");
            dtTable.Rows.Add("Actual characters available", "282193769");

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtTable);


                wb.SaveAs(DirectoryPath + "Report.xlsx");
            }
           
            //var EmailId = ConfigurationManager.AppSettings["EmailId"];
            var EmailId = "nisha.v@sblcorp.com";
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new MailAddress("mistool@sblinfo.org");
            mailMessage.Subject = " Test Report";
            mailMessage.Body = "Please find the attached  Report";
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(EmailId));
            MailAddress copy = new MailAddress("sooraj.tk@sblcorp.com");
            MailAddress copy1 = new MailAddress("keerthibabu@saibposervices.info");
            MailAddress copy2 = new MailAddress("sujit.menon@sblcorp.com");
            mailMessage.CC.Add(copy);
            mailMessage.CC.Add(copy1);
            mailMessage.CC.Add(copy2);
            string directoryName = Path.GetDirectoryName(DirectoryPath + "Report.xlsx");
            foreach (String filename in Directory.GetFiles(directoryName, "*.xlsx"))
            {
                mailMessage.Attachments.Add(new Attachment(filename));

            }

            SmtpClient smtp = new SmtpClient();

            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.EnableSsl = true;
            smtp.Host = "relay-hosting.secureserver.net";
            smtp.Port = 25;
            smtp.EnableSsl = false;

            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = mailMessage.From.Address;
            NetworkCred.Password = "x@VDl12639d6";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Send(mailMessage);
            TempData["Msg"] = "Successfully Send Mail!";
            return View();

        }



        public ActionResult DownloadExcelSheet(int id)
        {
            //ViewPlanModel viewPlan = new ViewPlanModel();

            //string projectcode=string.Empty;
            //string eventcode=string.Empty;
            //Double productionplanned = 0.0;
            //int holiday=0;
            //var formattedDate = DateTimeOffset.Now.ToString("yyyy-MM-dd");
            //var completiondate = DateTimeOffset.Now.ToString("yyyy-MM-dd");
            //int totaltarget=0;
            //string completion=string.Empty;
            //Double completiontarget;
            //Double Achievement;
            //CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            //string monthName = ci.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            //int year = int.Parse(DateTime.Now.Year.ToString());
            //ViewResourcePlan resourcemodel = new ViewResourcePlan();






            //string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //string Command = "SELECT Projectcode,eventcode,DATE_FORMAT(`Startdate`, '%d/%m/%Y') as Startdate,DATE_FORMAT(Completiondate,'%d/%m/%Y') as Completiondate ,TotaltargetPercent,CompletiontargetRecord,AchievementtillRef  from `ResourcePlan`";
            //using (MySqlConnection mConnection = new MySqlConnection(connString))
            //{


            //    mConnection.Open();
            //    MySqlCommand cmd = new MySqlCommand(Command, mConnection);
            //    MySqlDataReader reader = cmd.ExecuteReader();

            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {

            //            string DateString = reader.GetString("Completiondate");
            //            var split = DateString.Split('/');
            //            DateString = split[2] + "-" + split[1] + "-" + split[0];
            //            DateTime dtnow=DateTime.Now;
            //            DateTime dt =Convert.ToDateTime(reader.GetString("Completiondate"));
            //            projectcode=reader.GetString("Projectcode");
            //            eventcode = reader.GetString("eventcode");
            //            totaltarget=reader.GetInt32("TotaltargetPercent");
            //            completion=reader.GetString("Completiondate");
            //            completiontarget=reader.GetDouble("CompletiontargetRecord");
            //            Achievement=reader.GetDouble("AchievementtillRef");
            //            var datetime =dt.ToString("yyyy-MM-dd");
            //            //completiondate = reader.GetString("Completiondate");

            //            int remday =   (dt-dtnow).Days - 1;
            //            DataTable dtTable = new DataTable("Test");
            //            using (MySqlConnection mholiConnection = new MySqlConnection(connString))
            //            {
            //                mholiConnection.Open();
            //                string holicommand = "select count(*) as cnt from `Holiday` where holidaydate >= '" + formattedDate + "' and holidaydate <='" + DateString + "'";
            //                MySqlCommand cmdholi = new MySqlCommand(holicommand, mholiConnection);
            //                MySqlDataReader cmdreader = cmdholi.ExecuteReader();
            //                while (cmdreader.Read())
            //                {
            //                    holiday = cmdreader.GetInt32("cnt");
            //                }
            //            }

            //            using (MySqlConnection planConnection = new MySqlConnection(connString))
            //            {
            //                planConnection.Open();
            //                string projectcommand = "select `ProductionPlannedHr` from `projectconfiguration` where `Projectcode` ='" + projectcode + "' and location='MDS' and monthname='" + monthName + "' and year=" + year + " and process='Indexing' and Eventcode='" + eventcode + "'";
            //                MySqlCommand cmdproject = new MySqlCommand(projectcommand, planConnection);
            //                MySqlDataReader cmdprojectreader = cmdproject.ExecuteReader();
            //                while (cmdprojectreader.Read())
            //                {
            //                    productionplanned = Convert.ToDouble(cmdprojectreader.GetString("ProductionPlannedHr"));
            //                }

            //            }
            //            Double balanceAchieve=(completiontarget-Achievement)/remday;


            //            resourcemodel.Projectcode = projectcode+"."+eventcode;
            //            resourcemodel.TotaltargetP = totaltarget;
            //            resourcemodel.Completiondate = completion;
            //            resourcemodel.Completiontarget =completiontarget;
            //            resourcemodel.Referencedate=DateTime.Now.ToString("dd/MM/yyyy");
            //            resourcemodel.AchievetillRefdate=Achievement;
            //            resourcemodel.Holiday=holiday;
            //            resourcemodel.remainday=remday;
            //            resourcemodel.balanceAchieve=completiontarget-Achievement;
            //            resourcemodel.balanceAchieveday=balanceAchieve;
            //            resourcemodel.Indexingtarget =productionplanned;
            //            resourcemodel.Noofhrsreqdday =Convert.ToInt32(balanceAchieve/productionplanned);
            //            resourcemodel.Noofassociatereqday =Convert.ToInt32(balanceAchieve/productionplanned)/8;

            //resourcemodel.todayachievepercent=
            //resourcemodel.associatedeployed=
            //resourcemodel.ActualCharacters=
            string DirectoryPath = HostingEnvironment.MapPath("~/Documents/");
            DataTable dtTable = new DataTable("Test");


            dtTable.Columns.Add("ReportDate ", typeof(string));
            dtTable.Columns.Add("22/07/2019", typeof(string));
            dtTable.Rows.Add("Project Resource Plan", "");
            dtTable.Rows.Add("Project", "P2_61779.IDX.001");
            dtTable.Rows.Add("Total Target", "282193769");
            dtTable.Rows.Add("Immediate Target %", "100");
            dtTable.Rows.Add("Immediate Completion date", "29/07/2019");
            dtTable.Rows.Add("Reference date", "22/07/2019");
            dtTable.Rows.Add("Achievement till ref. date", 263214205.774);
            dtTable.Rows.Add("Intervening holiday", "1");
            dtTable.Rows.Add("Days remaining", 6);
            dtTable.Rows.Add("Balance to be achieved", "18979563.2254");
            dtTable.Rows.Add("Balanced to achieved/day", 3163260.537);
            dtTable.Rows.Add("Per Hour Indexing target", 6170);
            dtTable.Rows.Add("No of hrs reqd/day", "513");
            dtTable.Rows.Add("No of associates reqd/day", "64");
            dtTable.Rows.Add("Today/s Achievement%", "43");
            dtTable.Rows.Add("Todays Achievement%", "149");
            dtTable.Rows.Add("No of associates required/day @ today's % achievement", "152");
            dtTable.Rows.Add("Reason For Deviation", "Deadline Extended. Planning to complete project wellahead.");
            dtTable.Rows.Add("Actual characters available", "282193769");

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtTable);


                wb.SaveAs(DirectoryPath + "Report.xlsx");
            }

            var EmailId = ConfigurationManager.AppSettings["EmailId"];
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new MailAddress("mistool@sblinfo.org ");
            mailMessage.Subject = " Test Report";
            mailMessage.Body = "Please find the attached  Report";
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress("nisha.v@sblcorp.com"));
            //MailAddress copy = new MailAddress(ConfigurationManager.AppSettings["C1EmailId"]);
            //MailAddress copy1 = new MailAddress(ConfigurationManager.AppSettings["C2EmailId"]);
            //MailAddress copy2 = new MailAddress(ConfigurationManager.AppSettings["C3EmailId"]);
            //MailAddress copy3 = new MailAddress(ConfigurationManager.AppSettings["C4EmailId"]);
            //mailMessage.CC.Add(copy);
            //mailMessage.CC.Add(copy1);
            //mailMessage.CC.Add(copy2);
            //mailMessage.CC.Add(copy3);
            //string DirectoryPath1 = HostingEnvironment.MapPath(DirectoryPath + "MasterReport.xlsx");
            string directoryName = Path.GetDirectoryName(DirectoryPath + "Report.xlsx");
            // mailMessage.Attachments.Add(new Attachment(DirectoryPath1));

            foreach (String filename in Directory.GetFiles(directoryName, "*.xlsx"))
            {
                mailMessage.Attachments.Add(new Attachment(filename));

            }

            SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.EnableSsl = true;

            smtp.Host = "relay-hosting.secureserver.net";
            smtp.Port = 25;
            smtp.EnableSsl = false;

            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = mailMessage.From.Address;
            NetworkCred.Password = "x@VDl12639d6";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;

            smtp.Send(mailMessage);



            TempData["Msg"] = "Successfully Send Mail!";

            return View();
        }







        //string[] strArr = null;
        //char[] splitchar = { '/' };
        //strArr = date.Split(splitchar);
        //if (strArr.Length > 0)
        //    date = strArr[1] + "." + strArr[0] + "." + strArr[2];


        //Response.Clear();
        //Response.Buffer = true;
        //Response.Charset = "";
        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        ////Response.AddHeader("content-disposition", "attachment;filename=Master Report-" + date + ".xlsx");
        //Response.AddHeader("content-disposition", "attachment;filename=Report.xlsx");
        //wb.SaveAs(DirectoryPath + "MasterReport.xlsx");
        //using (MemoryStream MyMemoryStream = new MemoryStream())
        //{
        //    wb.SaveAs(MyMemoryStream);
        //    MyMemoryStream.WriteTo(Response.OutputStream);
        //    Response.Flush();
        //    Response.End();
        //}







        // return PartialView("/Views/Admin/_ViewResourceplan.cshtml", resourcemodel);
        //}

        //  }


        public ActionResult AddResource()
        {

            ResourcePlan Model = new ResourcePlan();
            return PartialView("/Views/Admin/_ResourcePlan.cshtml", Model);
        }

        public ActionResult NewDollar()
        {

            DollarModel Model = new DollarModel();
            return PartialView("/Views/Admin/_NewDollar.cshtml", Model);
        }
        public ActionResult NewUpload()
        {

            
            return PartialView("/Views/Admin/_Employee.cshtml");
        }


        public ActionResult MonthlyConfigurationList()
        {

            MonthlyConfiguration Model = new MonthlyConfiguration();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT * from `monthlyconfiguration` order by id desc";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.MonthConfList = dtt.DataTableToList<MonthlyConfiguration>();
                return View("MonthlyConfigurationList", Model);
            }
        }

        public ActionResult MonthlyConfigurationForm()
        {

            MonthlyConfiguration Model = new MonthlyConfiguration();
            return PartialView("/Views/Admin/_MonthlyConfiguration.cshtml", Model);
        }

        public ActionResult SaveMonthlyConfiguration(MonthlyConfiguration model)
        {

          



            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    int insertmonthresult = MonthconfigurationExistence(model);
                   if (insertmonthresult == 0)
                        {
                        string Result = ManageMonthlyConfiguration(model);
                        if (Result.Trim('"') == "Ok")
                            TempData["Msg"] = "Successfully Saved!";
                        else
                            TempData["Msg"] = "Unsuccessfull Operation!";
                    }
                    else
                    {
                        TempData["Msg"] = "Data  Exist!";
                    }
                }
                else
                {
                    string Result = ManageMonthlyConfiguration(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }


            }
            return RedirectToAction("MonthlyConfigurationList");



        }



        public ActionResult UpdateDollar(DollarModel model)
        {

            //DateTime dtdate = new DateTime();
            //dtdate = DateTime.ParseExact(model.dollardate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //var date = model.dollardate.ToString("yyyy-MM-dd");

            try
            {
                //var date = string.Empty;
                //string[] strArr = null;
                //char[] splitchar = { '/' };
                //strArr = model.dollardate.Split(splitchar);
                //if (strArr.Length > 0)
                //    date = strArr[2] + "/" + strArr[1] + "/" + strArr[0];


                string date = string.Empty;
                string[] strArrDate = null;
                char[] splitcharDate = { '/' };
                strArrDate = model.dollardate.Split(splitcharDate);

                if (strArrDate[0].Length == 1 && strArrDate[1].Length == 1)
                    date = strArrDate[2] + "-" + "0" + strArrDate[1] + "-" + "0" + strArrDate[0];
                else if (strArrDate[0].Length == 1 && strArrDate[1].Length > 1)
                    date = strArrDate[2] + "-" + strArrDate[1] + "-" + "0" + strArrDate[0];
                else if (strArrDate[0].Length > 1 && strArrDate[1].Length == 1)
                    date = strArrDate[2] + "-" + "0" + strArrDate[1] + "-" + "0" + strArrDate[0];
                else if (strArrDate[0].Length > 1 && strArrDate[1].Length > 1)
                    date = strArrDate[2] + "-" + strArrDate[1] + "-" + strArrDate[0];






                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                string Command = "UPDATE `dollarsettings` set `rate`=" + model.dollarrate + ", PoundRate=" + model.poundrate + ",  `dollardate`='" + date + "'  where dollarsettings.Id=" + model.id;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();

                    }

                }

                TempData["Msg"] = "Successfully Updated!";
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unsuccessfull Operation!";
            }


            return RedirectToAction("DollarSettings");
        }




        public ActionResult SaveNewDollar(DollarModel model)
        {
          


                string Result = string.Empty;
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                //DateTime dtdate = new DateTime();
                //dtdate = DateTime.ParseExact(model.dollardate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //var date = dtdate.ToString("yyyy-MM-dd");
                string date = string.Empty;
                string[] strArrDate = null;
                char[] splitcharDate = { '/' };
                strArrDate = model.dollardate.Split(splitcharDate);

                if (strArrDate[0].Length == 1 && strArrDate[1].Length == 1)
                    date = strArrDate[2] + "-" + "0" + strArrDate[1] + "-" + "0" + strArrDate[0];
                else if (strArrDate[0].Length == 1 && strArrDate[1].Length > 1)
                    date = strArrDate[2] + "-" + strArrDate[1] + "-" + "0" + strArrDate[0];
                else if (strArrDate[0].Length > 1 && strArrDate[1].Length == 1)
                    date = strArrDate[2] + "-" + "0" + strArrDate[1] + "-" + "0" + strArrDate[0];
                else if (strArrDate[0].Length > 1 && strArrDate[1].Length > 1)
                    date = strArrDate[2] + "-" + strArrDate[1] + "-" + strArrDate[0];



               
                try
                {
                    string Command = "INSERT INTO `dollarsettings`(`rate`,`dollardate`,PoundRate) VALUES (" + model.dollarrate + ",'" + date + "'," + model.poundrate + ");";
                    using (MySqlConnection mConnection = new MySqlConnection(connString))
                    {
                        mConnection.Open();
                        using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                        {
                            myCmd.ExecuteNonQuery();
                            TempData["Msg"] = "Successfully Saved!";

                        }

                    }

                }

            catch (Exception ex) 
                {
                    TempData["Msg"] = "Unsuccessfull Operation!";
                }
            //string path = Server.MapPath("~/bin/ApplicationError.txt");
            //// This text is added only once to the file.
            //if (!System.IO.File.Exists(path))
            //{
            //    using (System.IO.StreamWriter sw = System.IO.File.AppendText(path))
            //    {
            //        sw.WriteLine(datepicker);
            //        sw.WriteLine(model.dollardate);
            //    }
            //}
            


                return RedirectToAction("DollarSettings");
           
        }

       


        public ActionResult SaveViewResourceplan(ViewResourcePlan model)
        {


            string DirectoryPath = HostingEnvironment.MapPath("~/Documents/");
            DataTable dtTable = new DataTable("Test");
            dtTable.Columns.Add("ReportDate ", typeof(string));
            dtTable.Columns.Add(model.Projectdate, typeof(string));
            dtTable.Rows.Add("Project Resource Plan", "");
            dtTable.Rows.Add("Project", model.Projectcode + "." + model.eventcode);
            dtTable.Rows.Add("Total Target", model.Totaltarget);
            dtTable.Rows.Add("Immediate Target %", model.TotaltargetP);
            dtTable.Rows.Add("Immediate Completion date", model.Completiondate);
            dtTable.Rows.Add("Immediate Completion target", model.Completiontarget);
            dtTable.Rows.Add("Reference date", model.Projectdate);
            dtTable.Rows.Add("Achievement till ref. date", model.AchievetillRefdate);
            dtTable.Rows.Add("Intervening holiday", model.Holiday);
            dtTable.Rows.Add("Days remaining", model.remainday);
            dtTable.Rows.Add("Balance to be achieved", model.balanceAchieve);
            dtTable.Rows.Add("Balanced to achieved/day", model.balanceAchieveday);
            dtTable.Rows.Add("Per Hour Indexing target", model.Indexingtarget);
            dtTable.Rows.Add("No of hrs reqd/day", model.Noofhrsreqdday);
            dtTable.Rows.Add("No of associates reqd/day", model.Noofassociatereqday);
            dtTable.Rows.Add("Today/s Achievement%", model.todayachieve);
            dtTable.Rows.Add("No of associates required /day @ today's % achievement", model.todayachievepercent);
            dtTable.Rows.Add("Actual no. of Associates deployed", model.associatedeployed);
            dtTable.Rows.Add("Reason For Deviation", model.DeviationReason);
            dtTable.Rows.Add("Actual characters available", model.ActualCharacters);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtTable);


                wb.SaveAs(DirectoryPath + "Report.xlsx");
            }

            //var EmailId = ConfigurationManager.AppSettings["EmailId"];
            var EmailId = "nisha.v@sblcorp.com";
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new MailAddress("mistool@sblinfo.org");
            mailMessage.Subject = " Test Report";
            mailMessage.Body = "Please find the attached  Report";
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(EmailId));
            MailAddress copy = new MailAddress("sooraj.tk@sblcorp.com");
            MailAddress copy1 = new MailAddress("keerthibabu@saibposervices.info");
            MailAddress copy2 = new MailAddress("sujit.menon@sblcorp.com");
            mailMessage.CC.Add(copy);
            mailMessage.CC.Add(copy1);
            mailMessage.CC.Add(copy2);

            string directoryName = Path.GetDirectoryName(DirectoryPath + "Report.xlsx");
            foreach (String filename in Directory.GetFiles(directoryName, "*.xlsx"))
            {
                mailMessage.Attachments.Add(new Attachment(filename));

            }

            SmtpClient smtp = new SmtpClient();

            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.EnableSsl = true;
            smtp.Host = "relay-hosting.secureserver.net";
            smtp.Port = 25;
            smtp.EnableSsl = false;

            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = mailMessage.From.Address;
            NetworkCred.Password = "x@VDl12639d6";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Send(mailMessage);
            TempData["Msg"] = "Successfully Send Mail!";
            return RedirectToAction("ProjectResourcePlanList");
        }




        public string ManageResourcePlan(ResourcePlan model)
        {
            string Result = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string strtDate = model.Startdate;
            string complDate = model.Completiondate;
            string Command = "INSERT INTO `ResourcePlan`(`Projectcode`,`eventcode`, `Startdate`,`Completiondate` ,`TotaltargetPercent`,`CompletiontargetRecord`) VALUES ('" + model.Projectcode + "','" + model.eventcode + "','" + model.Startdate + "','" + model.Completiondate + "'," + model.TotaltargetP + " ,'" + model.Completiontarget + "');";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                {
                    myCmd.ExecuteNonQuery();
                    Result = "Ok";
                }

            }
            return Result;




        }




        public string ManageMonthlyConfiguration(MonthlyConfiguration model)
        {
            string Result = string.Empty;
            Result = "NotOk";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            string locationName = string.Empty;
            if (model.locationId == 1)
            {
                locationName = "KNPY";
            }
            else if (model.locationId == 2)
            {
                locationName = "TVM";
            }
            else if (model.locationId == 3)
            {
                locationName = "MDS";
            }
            else if (model.locationId == 4)
            {
                locationName = "MQC";
            }
            else if (model.locationId == 5)
            {
                locationName = "MNS";
            }
            else if (model.locationId == 6)
            {
                locationName = "KAKKANAD";
            }



            string monthname = string.Empty;
            if (model.monthid == 1)
            {
                monthname = "January";
            }
            else if (model.monthid == 2)
            {
                monthname = "February";
            }
            else if (model.monthid == 3)
            {
                monthname = "March";
            }
            else if (model.monthid == 4)
            {
                monthname = "April";
            }
            else if (model.monthid == 5)
            {
                monthname = "May";
            }
            else if (model.monthid == 6)
            {
                monthname = "June";
            }
            else if (model.monthid == 7)
            {
                monthname = "July";
            }
            else if (model.monthid == 8)
            {
                monthname = "August";
            }
            else if (model.monthid == 9)
            {
                monthname = "September";
            }
            else if (model.monthid == 10)
            {
                monthname = "October";
            }
            else if (model.monthid == 11)
            {
                monthname = "November";
            }
            else if (model.monthid == 12)
            {
                monthname = "December";
            }



            if (model.Id == 0)
            {

                string Command = "INSERT INTO `monthlyconfiguration`(`month`,`Configuration`, `monthname`,`location` ,`locationId`,year,Revenueconfiguration,workingdays) VALUES ('" + model.monthid + "','" + model.Configuration + "','" + monthname + "','" + locationName + "'," + model.locationId + " ,'" + model.year + "'," + model.Revenueconfiguration + "," + model.workingdays + ");";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }
            }
            else
            {

                string Command = "UPDATE monthlyconfiguration set `month`='" + model.monthid + "', `Configuration`=" + model.Configuration + ",`monthname`='" + monthname + "',location='" + locationName + "',`locationId`=" + model.locationId + ",`year`='" + model.year + "',Revenueconfiguration=" + model.Revenueconfiguration + ",workingdays=" + model.workingdays + " where monthlyconfiguration.Id=" + model.Id;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }

            }

            return Result;
        }




        public ActionResult ProjectConfigurationList()
        {

            ProjectConfiguration Model = new ProjectConfiguration();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT * from projectconfiguration order by id desc";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.ProjectConfList = dtt.DataTableToList<ProjectConfiguration>();
                return View("ProjectConfigurationList", Model);
            }
        }
        public ActionResult ProjectConfigurationForm()
        {

            ProjectConfiguration Model = new ProjectConfiguration();
            return PartialView("/Views/Admin/_ProjectConfiguration.cshtml", Model);
        }


        public ActionResult DeleteConfiguration(string ID)
        {
            try
            {
                DeleteConfigurationDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("ProjectConfigurationList");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("ProjectConfigurationList");
            }
        }




        public ActionResult DeleteMonthConfiguration(string ID)
        {
            try
            {
                DeleteMonthConfigurationDet(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("MonthlyConfigurationList");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("MonthlyConfigurationList");
            }
        }






        public ActionResult SaveProjectConfiguration(ProjectConfiguration model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    int insertresult = ProjectconfigurationExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageConfiguration(model);
                        if (Result.Trim('"') == "Ok")
                            TempData["Msg"] = "Successfully Saved!";
                        else
                            TempData["Msg"] = "Unsuccessfull Operation!";
                    }
                    else
                    {
                        TempData["Msg"] = "Data  Exist!";
                    }
                }
                else
                {
                    string Result = ManageConfiguration(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }



            }
            return RedirectToAction("ProjectConfigurationList");

        }




        public int RevenueConfigurationExistence(RevenueConfiguration model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `revenueconfiguration` where `Projectcode`='" + model.Projectcode + "' and `Eventcode`='" + model.Eventcode + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Result = reader.GetInt32(0);
                    }

                }
            }
            return Result;
        }


        public int ProjectconfigurationExistence(ProjectConfiguration model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `projectconfiguration` where `Projectcode`='" + model.Projectcode + "' and `Eventcode`='" + model.Eventcode + "' and Process='" + model.Process + "' and locationId=" + model.locationId + " and month=" + model.monthid + "";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Result = reader.GetInt32(0);
                    }

                }
            }
            return Result;
        }


    public int CheckExistenceofHoliday(HolidayModel model)
    {
         int Result = 0;
        


         string date = string.Empty;
         string[] strArrDate = null;
         char[] splitcharDate = { '/' };
         strArrDate = model.holidaydate.Split(splitcharDate);

         if (strArrDate[0].Length == 1 && strArrDate[1].Length == 1)
             date = strArrDate[2] + "-" + "0" + strArrDate[1] + "-" + "0" + strArrDate[0];
         else if (strArrDate[0].Length == 1 && strArrDate[1].Length > 1)
             date = strArrDate[2] + "-" + "0" + strArrDate[1] + "-" + strArrDate[0];
         else if (strArrDate[0].Length > 1 && strArrDate[1].Length == 1)
             date = strArrDate[2] + "-" + strArrDate[1] + "-" + "0" + strArrDate[0];
         else if (strArrDate[0].Length > 1 && strArrDate[1].Length > 1)
             date = strArrDate[2] + "-" + strArrDate[1] + "-" + strArrDate[0];



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `Holiday` where `holidayname`='" + model.holidayname + "' and `holidaydate`='" + date + "' and location='" + model.location + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Result = reader.GetInt32(0);
                    }

                }
            }
            return Result;

    }


        public int UserExistence(User model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `muser` where `UserName`='" + model.UserName + "' and `EmailId`='" + model.EmailId + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Result = reader.GetInt32(0);
                    }

                }
            }
            return Result;
        }


        public int MonthconfigurationExistence(MonthlyConfiguration model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `monthlyconfiguration` where `month`=" + model.monthid + " and `locationId`=" + model.locationId + " and year=" + model.year + "";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Result = reader.GetInt32(0);
                    }

                }
            }
            return Result;
        }







        public string ManageConfiguration(ProjectConfiguration model)
        {
            string Result = string.Empty;
            Result = "NotOk";



            string locationName = string.Empty;
            if (model.locationId == 1)
            {
                locationName = "KNPY";
            }
            else if (model.locationId == 2)
            {
                locationName = "TVM";
            }
            else if (model.locationId == 3)
            {
                locationName = "MDS";
            }
            else if (model.locationId == 4)
            {
                locationName = "MQC";
            }
            else if (model.locationId == 5)
            {
                locationName = "MNS";
            }
            string monthname = string.Empty;
            if (model.monthid == 1)
            {
                monthname = "January";
            }
            else if (model.monthid == 2)
            {
                monthname = "February";
            }
            else if (model.monthid == 3)
            {
                monthname = "March";
            }
            else if (model.monthid == 4)
            {
                monthname = "April";
            }
            else if (model.monthid == 5)
            {
                monthname = "May";
            }
            else if (model.monthid == 6)
            {
                monthname = "June";
            }
            else if (model.monthid == 7)
            {
                monthname = "July";
            }
            else if (model.monthid == 8)
            {
                monthname = "August";
            }
            else if (model.monthid == 9)
            {
                monthname = "September";
            }
            else if (model.monthid == 10)
            {
                monthname = "October";
            }
            else if (model.monthid == 11)
            {
                monthname = "November";
            }
            else if (model.monthid == 12)
            {
                monthname = "December";
            }

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            if (model.Id == 0)
            {
                string Command = "INSERT INTO projectconfiguration(`Projectcode`,`Eventcode`, `Process`,`ProductionPlannedHr`,`location`,`month`, `monthname`,`locationId`,year) VALUES ('" + model.Projectcode + "','" + model.Eventcode + "','" + model.Process + "'," + model.ProductionPlannedHr + " ,'" + locationName + "'," + model.monthid + ",'" + monthname + "', " + model.locationId + ",'" + model.year + "' );";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }
            }
            else
            {

                string Command = "UPDATE projectconfiguration set `Projectcode`='" + model.Projectcode + "', `Process`='" + model.Process + "',`ProductionPlannedHr`=" + model.ProductionPlannedHr + ",`location`='" + locationName + "',`month`=" + model.monthid + ",`monthname`='" + monthname + "',`locationId`=" + model.locationId + ",`year`='" + model.year + "'  where projectconfiguration.Id=" + model.Id;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }

            }

            return Result;
        }

        public string DeleteConfigurationDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "Delete from projectconfiguration where projectconfiguration.Id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }




        public string DeleteMonthConfigurationDet(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "Delete from `monthlyconfiguration` where `monthlyconfiguration`.Id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }

        public ActionResult OpenRevenue()
        {

            RevenueConfiguration Model = new RevenueConfiguration();
            return PartialView("/Views/Admin/_RevenueConfigurationForm.cshtml", Model);
        }


        public ActionResult OpenHoliday()
        {

            HolidayModel Model = new HolidayModel();
            return PartialView("/Views/Admin/_Holiday.cshtml", Model);
        }




        public ActionResult GetRevenueConfigurationPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            RevenueConfiguration Model = new RevenueConfiguration();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Id,Projectcode,Eventcode,Indexing,QC2,QC3,Audit,UAT,Rework,Price FROM `revenueconfiguration` where Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.Projectcode = reader.GetString(1);
                    Model.Eventcode = reader.GetString(2);
                    Model.Indexing = reader.GetDouble(3);
                    Model.Qc2 = reader.GetDouble(4);
                    Model.Qc3 = reader.GetDouble(5);
                    Model.Audit = reader.GetDouble(6);
                    Model.UAT = reader.GetDouble(7);
                    Model.Rework = reader.GetDouble(8);
                    Model.Price = reader.GetDouble(9);
                }

            }

            return PartialView("/Views/Admin/_RevenueConfigurationForm.cshtml", Model);
        }


        public ActionResult RevenueConfigurationList()
        {

            RevenueConfiguration Model = new RevenueConfiguration();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT * from revenueconfiguration order by id desc";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.RevenueConfList = dtt.DataTableToList<RevenueConfiguration>();
                return View("RevenueConfigurationList", Model);
            }
        }
        public ActionResult RevenueConfigurationForm()
        {

            RevenueConfiguration Model = new RevenueConfiguration();
            return PartialView("/Views/Admin/_RevenueConfigurationForm.cshtml", Model);
        }


        public ActionResult DeleteRevenueConfiguration(string ID)
        {
            try
            {
                DeleteRevenueConfigurationDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("RevenueConfigurationList");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("RevenueConfigurationList");
            }
        }

        public ActionResult SaveRevenueConfiguration(RevenueConfiguration model)
        {

            if (ModelState.IsValid)
            {
                int insertresult = RevenueConfigurationExistence(model);
                if (insertresult == 0)
                {
                    string Result = ManageRevenueConfiguration(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }
                else
                {
                    TempData["Msg"] = "Data  Exist!";
                }

            }
            return RedirectToAction("RevenueConfigurationList");

        }

        public string ManageRevenueConfiguration(RevenueConfiguration model)
        {
            string Result = string.Empty;
            Result = "NotOk";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            if (model.Id == 0)
            {
                string Command = "INSERT INTO revenueconfiguration(`Projectcode`,Eventcode,Indexing,QC2,QC3,Audit,UAT,Rework,Price ) VALUES ('" + model.Projectcode + "','" + model.Eventcode + "'," + model.Indexing + " ," + model.Qc2 + "," + model.Qc3 + "," + model.Audit + "," + model.UAT + "," + model.Rework + "," + model.Price + ");";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }
            }
            else
            {
                int isActive;
                if (model.IsActive == true)
                {
                    isActive = 1;
                }
                else
                {
                    isActive = 0;
                }
                string Command = "UPDATE `revenueconfiguration` set `Projectcode`='" + model.Projectcode + "', `Eventcode`='" + model.Eventcode + "',`Indexing`=" + model.Indexing + ",QC2=" + model.Qc2 + ",QC3=" + model.Qc3 + ",Audit=" + model.Audit + ",UAT=" + model.UAT + ",Rework = " + model.Rework + ",  Price=" + model.Price + " where revenueconfiguration.Id=" + model.Id;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }

            }

            return Result;
        }

        public string DeleteRevenueConfigurationDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "Delete from `revenueconfiguration` where `revenueconfiguration`.Id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }

        public static void WriteAttachment(string FileName, string FileType, string content)
        {
            HttpResponse Response = System.Web.HttpContext.Current.Response;
            Response.ClearHeaders();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
            Response.ContentType = FileType;
            Response.Write(content);
            Response.End();
        }



        [HttpPost]
        public ActionResult ConsolidatedReport(DailymasterProductionReport daily)
        {

            string date = string.Empty;
            //date =daily.Date;
            //string[] strArr = null;
            //char[] splitchar = { '/' };
            //strArr = date.Split(splitchar);
            //if (strArr.Length > 0)
            //    date = strArr[1] + "/" + strArr[0] + "/" + strArr[2];




            string pdate = string.Empty;
            string[] strArr = null;
            char[] splitchar = { '/' };
            strArr = daily.Date.Split(splitchar);
            if (strArr.Length > 0)
                pdate = strArr[1] + "/" + strArr[0] + "/" + strArr[2];


            DateTime dtdateTo = new DateTime();
            dtdateTo = DateTime.ParseExact(daily.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dtodate = dtdateTo.ToString("yyyy-MM-dd");



            string firstdate = "02" + "/" + strArr[1] + "/" + strArr[2];

            DateTime dtdateFrom = new DateTime();
            dtdateFrom = DateTime.ParseExact(firstdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dfromdate = dtdateFrom.ToString("yyyy-MM-dd");

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;



            using (MySqlConnection con = new MySqlConnection(constr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetBetweenProduction", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@firstdate", dfromdate);
                    cmd.Parameters.AddWithValue("@curdate", dtodate);
                    cmd.CommandTimeout = 8200;
                    cmd.ExecuteNonQuery();

                }
            }

            //string query = "select date, productionreport.location, sum(plannedhrs) as hoursplanned ,ROUND(sum(plannedhrrecord),0) as prodplanhrRecord,ROUND(sum(plannedprodrecord),0) as prodplanRecord,ROUND(sum(workedhrs),0) as  RecordsHours,ROUND(sum(actualprodrecord),0) as ActualProdRecords, ROUND((sum(actualprodrecord)/sum(plannedprodrecord))*100,0) as Achievement,ROUND(sum(targetrevenue*plannedprodrecord),2) as TargetRevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)),2) as actualrevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0))/sum(plannedprodrecord*targetrevenue)*100,0) as RevenueAchievement   from `productionreport` where  productionreport.date='" + dtodate + "' group by location;";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement ,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='TVM' and  productionreport.date='" + dtodate + "';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KNPY' and  productionreport.date='" + dtodate + "';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MDS' and  productionreport.date='" + dtodate + "';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MQC' and  productionreport.date='" + dtodate + "';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MNS' and  productionreport.date='" + dtodate + "';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KAKKANAD' and  productionreport.date='" + dtodate + "';";
            //query += "select date,productionreport.location, sum(plannedhrs) as hoursplanned ,ROUND(sum(plannedhrrecord),0) as prodplanhrRecord,ROUND(sum(plannedprodrecord),0) as prodplanRecord,ROUND(sum(workedhrs),0) as  RecordsHours,ROUND(sum(actualprodrecord),0) as ActualProdRecords, ROUND((sum(actualprodrecord)/sum(plannedprodrecord))*100,0) as Achievement,ROUND(sum(targetrevenue*plannedprodrecord),2) as TargetRevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)),2) as actualrevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0))/sum(plannedprodrecord*targetrevenue)*100,0) as RevenueAchievement   from `productionreport` group by date,productionreport.location;";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement ,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='TVM' order by date;";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KNPY' order by date;";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MDS' order by date;";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MQC' order by date;";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MNS' order by date;";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KAKKANAD' order by date;";
            //;


            //using (MySqlConnection con = new MySqlConnection(constr))
            //{
            //    using (MySqlCommand cmd = new MySqlCommand(query))
            //    {
            //        using (MySqlDataAdapter sda = new MySqlDataAdapter())
            //        {
            //            cmd.Connection = con;
            //            sda.SelectCommand = cmd;
            //            using (DataSet ds = new DataSet())
            //            {
            //                sda.Fill(ds);

            //                //Set Name of DataTables.

            //                ds.Tables[0].TableName = "Summary";
            //                ds.Tables[1].TableName = "TVM";
            //                ds.Tables[2].TableName = "KNPY";
            //                ds.Tables[3].TableName = "MDS";
            //                ds.Tables[4].TableName = "MQC";
            //                ds.Tables[5].TableName = "MNS";
            //                ds.Tables[6].TableName = "KAKKANAD";



            //                ds.Tables[7].TableName = "Summary-C";
            //                ds.Tables[8].TableName = "TVM-C";
            //                ds.Tables[9].TableName = "KNPY-C";
            //                ds.Tables[10].TableName = "MDS-C";
            //                ds.Tables[11].TableName = "MQC-C";
            //                ds.Tables[12].TableName = "MNS-C";
            //                ds.Tables[13].TableName = "KAKKANAD-C";
            //                date = ds.Tables[7].Rows[1]["date"].ToString();

            //                ds.Tables[0].Columns["date"].ColumnName = "Date";
            //                ds.Tables[0].Columns["location"].ColumnName = "Location";
            //                ds.Tables[0].Columns["hoursplanned"].ColumnName = "Hours planned";
            //                ds.Tables[0].Columns["prodplanhrRecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[0].Columns["prodplanRecord"].ColumnName = "Production   planned   Records";
            //                ds.Tables[0].Columns["RecordsHours"].ColumnName = "Hours worked";
            //                ds.Tables[0].Columns["ActualProdRecords"].ColumnName = "Actual Production Records";
            //                ds.Tables[0].Columns["Achievement"].ColumnName = "% Achievement";
            //                ds.Tables[0].Columns["TargetRevenue"].ColumnName = "Target Revenue INR";
            //                ds.Tables[0].Columns["ActualRevenue"].ColumnName = "Actual Revenue INR";
            //                ds.Tables[0].Columns["RevenueAchievement"].ColumnName = "% Revenue Achievement";
            //                ds.Tables[0].AcceptChanges();





            //                ds.Tables[1].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[1].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[1].Columns["process"].ColumnName = "Process";
            //                ds.Tables[1].Columns["project"].ColumnName = "Project";
            //                ds.Tables[1].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[1].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[1].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[1].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[1].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[1].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[1].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[1].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[1].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[1].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[1].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[1].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[1].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[1].AcceptChanges();

            //                ds.Tables[2].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[2].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[2].Columns["process"].ColumnName = "Process";
            //                ds.Tables[2].Columns["project"].ColumnName = "Project";
            //                ds.Tables[2].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[2].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[2].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[2].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[2].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[2].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[2].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[2].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[2].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[2].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[2].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[2].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[2].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[2].AcceptChanges();

            //                ds.Tables[3].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[3].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[3].Columns["process"].ColumnName = "Process";
            //                ds.Tables[3].Columns["project"].ColumnName = "Project";
            //                ds.Tables[3].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[3].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[3].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[3].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[3].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[3].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[3].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[3].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[3].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[3].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[3].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[3].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[3].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[3].AcceptChanges();


            //                ds.Tables[4].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[4].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[4].Columns["process"].ColumnName = "Process";
            //                ds.Tables[4].Columns["project"].ColumnName = "Project";
            //                ds.Tables[4].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[4].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[4].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[4].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[4].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[4].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[4].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[4].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[4].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[4].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[4].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[4].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[4].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[4].AcceptChanges();


            //                ds.Tables[5].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[5].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[5].Columns["process"].ColumnName = "Process";
            //                ds.Tables[5].Columns["project"].ColumnName = "Project";
            //                ds.Tables[5].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[5].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[5].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[5].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[5].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[5].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[5].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[5].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[5].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[5].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[5].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[5].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[5].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[5].AcceptChanges();



            //                ds.Tables[6].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[6].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[6].Columns["process"].ColumnName = "Process";
            //                ds.Tables[6].Columns["project"].ColumnName = "Project";
            //                ds.Tables[6].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[6].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[6].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[6].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[6].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[6].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[6].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[6].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[6].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[6].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[6].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[6].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[6].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[6].AcceptChanges();


            //                ds.Tables[7].Columns["date"].ColumnName = "Date";
            //                ds.Tables[7].Columns["location"].ColumnName = "Location";
            //                ds.Tables[7].Columns["hoursplanned"].ColumnName = "Hours planned";
            //                ds.Tables[7].Columns["prodplanhrRecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[7].Columns["prodplanRecord"].ColumnName = "Production   planned   Records";
            //                ds.Tables[7].Columns["RecordsHours"].ColumnName = "Hours worked";
            //                ds.Tables[7].Columns["ActualProdRecords"].ColumnName = "Actual Production Records";
            //                ds.Tables[7].Columns["Achievement"].ColumnName = "% Achievement";
            //                ds.Tables[7].Columns["TargetRevenue"].ColumnName = "Target Revenue INR";
            //                ds.Tables[7].Columns["ActualRevenue"].ColumnName = "Actual Revenue INR";
            //                ds.Tables[7].Columns["RevenueAchievement"].ColumnName = "% Revenue Achievement";
            //                ds.Tables[7].AcceptChanges();


            //                ds.Tables[8].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[8].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[8].Columns["process"].ColumnName = "Process";
            //                ds.Tables[8].Columns["project"].ColumnName = "Project";
            //                ds.Tables[8].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[8].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[8].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[8].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[8].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[8].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[8].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[8].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[8].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[8].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[8].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[8].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[8].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[8].AcceptChanges();


            //                ds.Tables[9].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[9].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[9].Columns["process"].ColumnName = "Process";
            //                ds.Tables[9].Columns["project"].ColumnName = "Project";
            //                ds.Tables[9].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[9].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[9].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[9].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[9].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[9].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[9].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[9].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[9].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[9].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[9].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[9].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[9].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[9].AcceptChanges();


            //                ds.Tables[10].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[10].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[10].Columns["process"].ColumnName = "Process";
            //                ds.Tables[10].Columns["project"].ColumnName = "Project";
            //                ds.Tables[10].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[10].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[10].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[10].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[10].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[10].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[10].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[10].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[10].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[10].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[10].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[10].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[10].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[10].AcceptChanges();


            //                ds.Tables[11].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[11].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[11].Columns["process"].ColumnName = "Process";
            //                ds.Tables[11].Columns["project"].ColumnName = "Project";
            //                ds.Tables[11].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[11].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[11].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[11].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[11].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[11].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[11].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[11].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[11].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[11].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[11].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[11].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[11].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[11].AcceptChanges();



            //                ds.Tables[12].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[12].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[12].Columns["process"].ColumnName = "Process";
            //                ds.Tables[12].Columns["project"].ColumnName = "Project";
            //                ds.Tables[12].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[12].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[12].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[12].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[12].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[12].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[12].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[12].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[12].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[12].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[12].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[12].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[12].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[12].AcceptChanges();


            //                ds.Tables[13].Columns["psn"].ColumnName = "PSN";
            //                ds.Tables[13].Columns["associate"].ColumnName = "Associates Name";
            //                ds.Tables[13].Columns["process"].ColumnName = "Process";
            //                ds.Tables[13].Columns["project"].ColumnName = "Project";
            //                ds.Tables[13].Columns["projectcode"].ColumnName = "Project Code";
            //                ds.Tables[13].Columns["eventcode"].ColumnName = "Event code";
            //                ds.Tables[13].Columns["tlname"].ColumnName = "TL's Name";
            //                ds.Tables[13].Columns["plannedhrs"].ColumnName = "Hours planned";
            //                ds.Tables[13].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            //                ds.Tables[13].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            //                ds.Tables[13].Columns["workedhrs"].ColumnName = "Hours worked";
            //                ds.Tables[13].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            //                ds.Tables[13].Columns["achievement"].ColumnName = "% Achievement";
            //                ds.Tables[13].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            //                ds.Tables[13].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            //                ds.Tables[13].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            //                ds.Tables[13].Columns["productivity"].ColumnName = "Productivity(per hr)";
            //                ds.Tables[13].AcceptChanges();







            //                using (XLWorkbook wb = new XLWorkbook())
            //                {
            //                    foreach (DataTable dt in ds.Tables)
            //                    {
            //                        wb.Worksheets.Add(dt);
            //                    }


            //                    Response.Clear();
            //                    Response.Buffer = true;
            //                    Response.Charset = "";
            //                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //                    Response.AddHeader("content-disposition", "attachment;filename=Master Report_" + date + ".xlsx");
            //                    // Response.AddHeader("content-disposition", "attachment;filename=Master Report.xlsx");
            //                    using (MemoryStream MyMemoryStream = new MemoryStream())
            //                    {
            //                        wb.SaveAs(MyMemoryStream);
            //                        MyMemoryStream.WriteTo(Response.OutputStream);
            //                        Response.Flush();
            //                        Response.End();
            //                    }

            //                }




            //            }
            //        }
            //    }
            //}



            return View("ConsolidatedProductionReport");





        }


        public ActionResult ConsolidatedView()
        {
            return View("ConsolidatedView");
        }


        public ActionResult DailyETO(string sdate, string enddate, string LocationId, string TL, string Clientcode, string Project, string Event, string Process)

        {

            DailyETO modeleto = new DailyETO();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = string.Empty;
            double dollarrate = 0;
            if (enddate != "")
            {
                DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);


                cdate = dtenddate.ToString("yyyy-MM-dd");
            }

          
            string filteration = LocationId;
            filteration = filteration + "," + TL;
            filteration = filteration + "," + Clientcode;
            filteration = filteration + "," + Project;
            filteration = filteration + "," + Event;
            filteration = filteration + "," + Process;
            Session["etoreport"] = filteration;







            if (LocationId == "KAKKANAD")
            {
                LocationId = "KKND";
            }

            DataTable dt = new DataTable();


            string dollarCommand = "SELECT rate FROM dollarsettings WHERE dollardate <='" + cdate + "' ORDER BY dollardate desc LIMIT 1";
            using (MySqlConnection tarConnection = new MySqlConnection(connString))
            {
                tarConnection.Open();
                MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dollarrate = reader.GetInt32(0);
                }
                tarConnection.Close();
            }



            string query = "select DATE_FORMAT(Date,'%d/%m/%Y') as Date,sum(actualrevenue) as Actualrevenue,sum(workedhrs)/8  as employeeno,productionreport2020.location as Location   from productionreport2020 where";   
            if (sdate != null && enddate != "")
            {

                query = query + " date >='" + stdate + "' AND date <='" + cdate + "' ";
            }

            if (sdate != null && enddate == "")
            {

                query = query + "  date ='" + stdate + "' ";
            }


            if (Clientcode != "ALL")
            {
                query = query + " and `project`='" + Clientcode + "'";
            }
            if (Project != "ALL")
            {
                query = query + " and `projectcode`='" + Project  + "'";
            }

            if (LocationId != "ALL")
            {
                query = query + " and `location`='" + LocationId + "'";
            }

            if (TL != "ALL")
            {
                query = query + " and `tlname`='" + TL + "'";
            }


            if (Event != "ALL")
            {
                query = query + " and `eventcode`='" + Event + "'";
            }

            if (Process != "ALL")
            {
                query = query + " and `process`='" + Process + "'";
            }

            query = query + "and process <> 'Training'";
            query = query + " group by Date";


            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);

                    }
                }
            }
            System.Data.DataColumn rateColumn = new System.Data.DataColumn("dollarrate", typeof(System.Double));
            rateColumn.DefaultValue = dollarrate;
            dt.Columns.Add(rateColumn);

            modeleto.LstDailyETO = dt.DataTableToList<DailyETO>();
            return PartialView("/Views/Admin/_ETODatewise.cshtml", modeleto);

        }





        public ActionResult DailyETOLocationwise(string Project, string LocationId, string sdate, string enddate, string Type)
        {
            try
            {

                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

                if (LocationId == "KAKKANAD")
                    LocationId = "KKND";

                if (Type == "Tabular")
                {




                    DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var stdate = dtstartdate.ToString("yyyy-MM-dd");
                    var cdate = dtenddate.ToString("yyyy-MM-dd");

                    DataTable dt = new DataTable();
                    DailyETO modeleto = new DailyETO();


                    using (MySqlConnection con = new MySqlConnection(connString))
                    {
                        using (MySqlCommand cmd = new MySqlCommand("GettempLocationwiseETO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@llocation", LocationId);
                            cmd.Parameters.AddWithValue("@startdate", stdate);
                            cmd.Parameters.AddWithValue("@enddate", cdate);
                            cmd.Parameters.AddWithValue("@projectcodee", Project);
                            cmd.CommandTimeout = 1500;
                            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                            {
                                sda.Fill(dt);

                            }
                        }
                    }




                    //modeleto.LstDailyETO = dt.DataTableToList<DailyETO>();
                    //return PartialView("/Views/Admin/_ETOLocationwise.cshtml", modeleto);

                    dt.Columns.Add("ddate", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string cellValue = dt.Rows[i][0].ToString();

                        DateTime dateAndTime = DateTime.Parse(cellValue);
                        cellValue = dateAndTime.ToString("dd/MM/yyyy");
                        dt.Rows[i]["ddate"] = cellValue;
                    }
                    dt.Columns.Remove("date");


                    dt.Columns["ddate"].ColumnName = "Date";

                    dt.Columns["Date"].SetOrdinal(0);

                    return PartialView("_ETOLocationwise", dt);

                }

                else
                {

                    return PartialView("Chartview");
                }
            }
            catch (Exception Ex)
            {
                DataTable dt = new DataTable();
                return PartialView("_ETOLocationwise", dt);
            }
           
        }





        public ActionResult DailyETOprojectwise(string Project, string LocationId, string sdate, string enddate, string Type)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            double dollarrate = 0.0;
            if (LocationId == "KAKKANAD")
            {
                LocationId = "KKND";
            }
            
            if (Type == "Tabular")
            {

                DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var stdate = dtstartdate.ToString("yyyy-MM-dd");
                var cdate = dtenddate.ToString("yyyy-MM-dd");

                DataTable dt = new DataTable();
                DailyETO modeleto = new DailyETO();


                //using (MySqlConnection con = new MySqlConnection(connString))
                //{
                //    using (MySqlCommand cmd = new MySqlCommand("GetMasterETO", con))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@llocation", LocationId);
                //        cmd.Parameters.AddWithValue("@startdate", stdate);
                //        cmd.Parameters.AddWithValue("@enddate", cdate);
                //        cmd.Parameters.AddWithValue("@projectcodee", Project);
                //        cmd.CommandTimeout = 1500;
                //        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                //        {
                //            sda.Fill(dt);

                //        }
                //    }
               

                //}

               
               




              

             

                string dollarCommand = "SELECT rate FROM dollarsettings WHERE dollardate <='" + cdate + "' ORDER BY dollardate desc LIMIT 1";
                using (MySqlConnection tarConnection = new MySqlConnection(connString))
                {
                    tarConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        dollarrate = reader.GetInt32(0);
                    }
                    tarConnection.Close();
                }



                string query = "select  projectcode,DATE_FORMAT(Date,'%d/%m/%Y') as Date,sum(actualrevenue) as Actualrevenue,COUNT(CASE WHEN project is not null  THEN 1 END)  as employeeno,productionreport2020.location as Location   from productionreport2020 where projectcode<>'' and projectcode is not null  ";
                if (sdate != null && enddate != "")
                {

                    query = query + " and date >='" + stdate + "' AND date <='" + cdate + "' ";
                }

                if (sdate != null && enddate == "")
                {

                    query = query + "and  date ='" + stdate + "' ";
                }


              
                if (Project != "ALL")
                {
                    query = query + " and `projectcode`='" + Project + "'";
                }

                if (LocationId != "ALL")
                {
                    query = query + " and `location`='" + LocationId + "'";
                }



                query = query + "group by projectcode, Date,location";


                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = mConnection;
                        mConnection.Open();
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);

                        }
                    }
                }
                System.Data.DataColumn rateColumn = new System.Data.DataColumn("dollarrate", typeof(System.Double));
                rateColumn.DefaultValue = dollarrate;
                dt.Columns.Add(rateColumn);

                //System.Data.DataColumn ETOColumn = new System.Data.DataColumn("ETOActualrevenue", typeof(System.Double));
                //ETOColumn.DefaultValue = dollarrate;
                //dt.Columns.Add(ETOColumn);



                modeleto.LstDailyETO = dt.DataTableToList<DailyETO>();
                return PartialView("/Views/Admin/_ETOProjectwise.cshtml", modeleto);

                }
                else
                {

                    return PartialView("Chartview");

                }



            }
     




        public ActionResult EmployeeETOIndividual(string Employee, string LocationId, string sdate, string enddate, string Type)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string ooutput=string.Empty;
            if (LocationId == "KAKKANAD")
                LocationId = "KKND";
            if (Employee != "ALL")
                ooutput = Employee.Split('[', ']')[1];
            else
                ooutput = "ALL";

            double dollarrate = 0.0;



            if (Type == "Tabular")
            {


                DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var stdate = dtstartdate.ToString("yyyy-MM-dd");
                var cdate = dtenddate.ToString("yyyy-MM-dd");

                DataTable dt = new DataTable();
                EmployeeETO modeleto = new EmployeeETO();



                string dollarCommand = "SELECT rate FROM dollarsettings WHERE dollardate <='" + cdate + "' ORDER BY dollardate desc LIMIT 1";
                using (MySqlConnection tarConnection = new MySqlConnection(connString))
                {
                    tarConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        dollarrate = reader.GetInt32(0);
                    }
                    tarConnection.Close();
                }





                using (MySqlConnection con = new MySqlConnection(connString))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand("DELETE from employeeETO where id<>0"))
                    {
                        cmd.Connection = con;

                        cmd.ExecuteNonQuery();
                    }

                }


                using (MySqlConnection con = new MySqlConnection(connString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("EmployeeETOIndividual", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@location", LocationId);
                        cmd.Parameters.AddWithValue("@startdate", stdate);
                        cmd.Parameters.AddWithValue("@enddate", cdate);
                        cmd.Parameters.AddWithValue("@employee", ooutput);
                        cmd.CommandTimeout = 1500;
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);

                        }
                    }
                }

                System.Data.DataColumn rateColumn = new System.Data.DataColumn("dollarrate", typeof(System.Double));
                rateColumn.DefaultValue = dollarrate;
                dt.Columns.Add(rateColumn);



                modeleto.LstEmployeeETO = dt.DataTableToList<EmployeeETO>();
                return PartialView("/Views/Admin/_ETOEmployeewisee.cshtml", modeleto);
            }

            else
            {
                return PartialView("Chartview");

            }
        }







        


        public ActionResult EmployeeETOReport(string Date)
        {


            string[] strArr = null;
            strArr = Session["etoreport"].ToString().Split(',');
            string LocationId = strArr[0].ToString();
            string TL = strArr[1].ToString();
            string Clientcode = strArr[2].ToString();
            string ProjectId = strArr[3].ToString();
            string Eventcode = strArr[4].ToString();
            string Process = strArr[5].ToString();


            if (LocationId == "KAKKANAD")
                LocationId = "KKND";


            DateTime dtdateFrom = new DateTime();
            dtdateFrom = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dfromdate = dtdateFrom.ToString("yyyy-MM-dd");
            double dollarrate = 0.0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            DataTable dt = new DataTable();
            EmployeeETO modeleto = new EmployeeETO();


            string dollarCommand = "SELECT rate FROM dollarsettings WHERE dollardate <='" + dfromdate + "' ORDER BY dollardate desc LIMIT 1";
            using (MySqlConnection tarConnection = new MySqlConnection(connString))
            {
                tarConnection.Open();
                MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dollarrate = reader.GetInt32(0);
                }
                tarConnection.Close();
            }



            string query1 = "select associate,actualrevenue,DATE_FORMAT(Date,'%d/%m/%Y') as Date from productionreport2020 where   date='" + dfromdate + "' ";

            if (LocationId != "ALL") 
            {
                query1=query1 + " and location='" + LocationId + "'";
            }
            if (TL != "ALL") 
            {
                query1=query1 + " and `tlname`='" + TL + "'";
            }
             if (Clientcode != "ALL") 
            {
                query1 = query1 + " and `project`='" + Clientcode + "'";
            }

             if (ProjectId != "ALL")
             {
                 query1 = query1 + " and `projectcode`='" + ProjectId + "'";
             }
             if (Eventcode != "ALL")
             {
                 query1 = query1 + " and `eventcode`='" + Eventcode + "'";
             }
             if (Process != "ALL")
             {
                 query1 = query1 + " and `process`='" + Process + "'";
             }

           
          


            using (MySqlConnection con = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query1))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);

                    }
                }

            }
            System.Data.DataColumn rateColumn = new System.Data.DataColumn("dollarrate", typeof(System.Double));
            rateColumn.DefaultValue = dollarrate;
            dt.Columns.Add(rateColumn);

            modeleto.LstEmployeeETO = dt.DataTableToList<EmployeeETO>();
            return PartialView("/Views/Admin/_ETOEmployeewise.cshtml", modeleto);

        }





        

        public ActionResult DownloadETO(string DDate)
        {

            string[] strArr = null;
            strArr = Session["etoreport"].ToString().Split(',');
            string LocationId = strArr[0].ToString();
            string TL = strArr[1].ToString();
            string Clientcode = strArr[2].ToString();
            string ProjectId = strArr[3].ToString();
            string Eventcode = strArr[4].ToString();
            string Process = strArr[5].ToString();


            if (LocationId == "KAKKANAD")
                LocationId = "KKND";
            
            
            
            
            DateTime dtdateFrom = new DateTime();
            dtdateFrom = DateTime.ParseExact(DDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dfromdate = dtdateFrom.ToString("yyyy-MM-dd");
            double dollarrate = 0.0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


            string dollarCommand = "SELECT rate FROM dollarsettings WHERE dollardate <='" + dfromdate + "' ORDER BY dollardate desc LIMIT 1";
            using (MySqlConnection tarConnection = new MySqlConnection(connString))
            {
                tarConnection.Open();
                MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dollarrate = reader.GetInt32(0);
                }
                tarConnection.Close();
            }

            

          
            DataTable dt = new DataTable("ETO");
            EmployeeETO modeleto = new EmployeeETO();
            string query1 = "select associate as Employee,Round(actualrevenue,2) as actualrevenue    from productionreport2020 where   date='" + dfromdate + "'";

            if (LocationId != "ALL")
            {
                query1 = query1 + " and location='" + LocationId + "'";
            }
            if (TL != "ALL")
            {
                query1 = query1 + " and `tlname`='" + TL + "'";
            }
            if (Clientcode != "ALL")
            {
                query1 = query1 + " and `project`='" + Clientcode + "'";
            }

            if (ProjectId != "ALL")
            {
                query1 = query1 + " and `projectcode`='" + ProjectId + "'";
            }
            if (Eventcode != "ALL")
            {
                query1 = query1 + " and `eventcode`='" + Eventcode + "'";
            }
            if (Process != "ALL")
            {
                query1 = query1 + " and `process`='" + Process + "'";
            }



            using (MySqlConnection con = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query1))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);

                    }
                }

            }
            System.Data.DataColumn rateColumn = new System.Data.DataColumn("dollarrate", typeof(System.Double));
            rateColumn.DefaultValue = dollarrate;
            dt.Columns.Add(rateColumn);
            dt.Columns.Add("EmployeeETO", typeof(decimal), "(actualrevenue/dollarrate)");
            System.Data.DataColumn dateColumn = new System.Data.DataColumn("Date", typeof(System.String));
            dateColumn.DefaultValue = DDate;
            dt.Columns.Add(dateColumn);
            System.Data.DataColumn ETOColumn = new System.Data.DataColumn("eto", typeof(System.Double));
            dt.Columns.Add(ETOColumn);
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                dt.Rows[i]["eto"] = Math.Round(Convert.ToDouble(dt.Rows[i]["EmployeeETO"]), 2);
            }
            dt.Columns["actualrevenue"].ColumnName = "Actual Revenue(INR)";
            dt.Columns["eto"].ColumnName = "Employee ETO(USD)";
            dt.Columns["Employee"].SetOrdinal(0);
            dt.Columns["Date"].SetOrdinal(1);
            dt.Columns["Actual Revenue(INR)"].SetOrdinal(2);
            dt.Columns["Employee ETO(USD)"].SetOrdinal(3);

            dt.ParentRelations.Clear();
            dt.ChildRelations.Clear();
            dt.Constraints.Clear();
            dt.Columns.Remove("EmployeeETO");
            dt.Columns.Remove("dollarrate");


           
            dt.AcceptChanges();
            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dt);
            //    Response.ClearHeaders();
            //    Response.Clear();
            //    Response.ClearContent();
            //    Response.ClearHeaders();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AddHeader("content-disposition", "attachment;filename=ETOReport" + DDate + ".xlsx");
            //    // Response.AddHeader("content-disposition", "attachment;filename=Master Report.xlsx");
            //    using (MemoryStream MyMemoryStream = new MemoryStream())
            //    {
            //        wb.SaveAs(MyMemoryStream);
            //        MyMemoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        Response.End();
            //    }
            //}
            string fileName = "ETOReport" + DDate + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dt, "Details");

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        private static DataTable AddAutoIncrementColumn()
        {

            DataColumn myDataColumn = new DataColumn();
            myDataColumn.AllowDBNull = false;
            myDataColumn.AutoIncrement = true;
            myDataColumn.AutoIncrementSeed = 1;
            myDataColumn.AutoIncrementStep = 1;
            myDataColumn.ColumnName = "autoID";
            myDataColumn.DataType = System.Type.GetType("System.Int32");
            myDataColumn.Unique = true;

            //Create a new datatable
            DataTable mydt = new DataTable();

            //Add this AutoIncrement Column to a new datatable
            mydt.Columns.Add(myDataColumn);

            return mydt;

        }

        public ActionResult DownloadYearlyReport(int PartId)
        {

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //var dt = new DataTable("Test");
        
            string[] strArr = null;
            strArr = Session["yearlyreport"].ToString().Split(',');
            string Command = string.Empty;
            string Year = strArr[0].ToString();
            string Clientcode = strArr[1].ToString();
            string ProjectId = strArr[2].ToString();
            string Eventcode = strArr[3].ToString();
            string Process = strArr[4].ToString();
            string Location = strArr[5].ToString();
            string TL = strArr[6].ToString();
            string Resource = strArr[7].ToString();
            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }
            string[] stryearArr = null;
            char[] splitchar = { '-' };
            stryearArr = Year.Split(splitchar);
            var Date1 = stryearArr[0].Trim().ToString() + "-04-01";
            var Date2 = stryearArr[1].Trim().ToString() + "-03-31";
            DataTable dtrate = new DataTable();
            string dollarCommand = "SELECT monthname(dollardate) as date, rate FROM dollarsettings where dollardate >='" + Date1 + "' and  dollardate <='" + Date2 + "' group by monthname(dollardate) order by year(dollardate), month(dollardate) ";
            using (MySqlConnection tarConnection = new MySqlConnection(connString))
            {
                tarConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(dollarCommand, tarConnection);
                adapter.Fill(dtrate);
                tarConnection.Close();
            }

            DataTable dt = new DataTable();

            Command = "select monthname(date) as month, concat (monthname(date),year(date)) as date,  Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),0) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),0) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where date >='" + Date1 + "' and  date <='" + Date2 + "' ";


            if (Clientcode != "ALL")
            {
                Command = Command + " and `project`='" + Clientcode + "'";
            }

            if (ProjectId != "ALL")
            {
                Command = Command + " and `projectcode`='" + ProjectId + "'";
            }
            if (Eventcode != "ALL")
            {
                Command = Command + " and `eventcode`='" + Eventcode + "'";
            }

            if (Process != "ALL")
            {
                Command = Command + " and `process`='" + Process + "'";
            }

            if (Location != "ALL")
            {
                Command = Command + " and  `location`='" + Location + "'";
            }

            if (TL != "ALL")
            {
                Command = Command + " and  `tlname`='" + TL + "'";
            }
            if (Resource != "ALL")
            {
                Command = Command + " and  `associate`='" + Resource + "'";
            }



            Command = Command + "  group by monthname(date) order by year(date), month(date)";


           
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

                System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));

                dt.Columns.Add(rateColumn);

            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    if (Equals(dt.Rows[i]["month"], dtrate.Rows[i]["date"]))
                        dt.Rows[i]["rate"] = dtrate.Rows[i]["rate"];
                }
            }

                DataColumn column = new DataColumn();
                column.DataType = typeof(double);

                column.Expression = "ActrevenueINR/rate/cnt";
                column.ColumnName = "eto";
                dt.Columns.Add(column);

                var sumhrs = dt.Compute("Sum(hoursplanned)", string.Empty);
                var sumplanhrrecord = dt.Compute("Sum(prodplanhrrecord)", string.Empty);
                var sumprodplanrecords = dt.Compute("Sum(prodplanrecords)", string.Empty);
                var sumhrsworked = dt.Compute("Sum(hoursworked)", string.Empty);
                var sumactprod = dt.Compute("Sum(Actualprodrecord)", string.Empty);
                var tarrev = dt.Compute("Sum(TarrevenueINR)", string.Empty);
                var actrev = dt.Compute("Sum(ActrevenueINR)", string.Empty);
                var ccnt = dt.Compute("Sum(cnt)", string.Empty);
                double etosum = Convert.ToDouble(actrev) / Convert.ToDouble(dt.Rows[0]["rate"]);
                var etosumm = Convert.ToDouble(etosum) / Convert.ToInt32(ccnt);


                dt.Columns["date"].ColumnName = "Month";
                dt.Columns["hoursplanned"].ColumnName = "Hours planned";
                dt.Columns["prodplanhrrecord"].ColumnName = "Production planned/Hr Records";
                dt.Columns["prodplanrecords"].ColumnName = "Production planned Records";
                dt.Columns["hoursworked"].ColumnName = "Hours Worked";
                dt.Columns["Actualprodrecord"].ColumnName = "Actual Production Records";
                dt.Columns["Achievement"].ColumnName = "% Achievement";
                dt.Columns["TarrevenueINR"].ColumnName = "Target Revenue INR";
                dt.Columns["ActrevenueINR"].ColumnName = "Actual Revenue INR";
                dt.Columns["RevAchievement"].ColumnName = "% Revenue Achievement";

                if (!dt.Columns.Contains("ETO(USD)"))
                {

                    System.Data.DataColumn ETOColumn = new System.Data.DataColumn("ETO(USD)", typeof(System.Double));
                    dt.Columns.Add(ETOColumn);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["ETO(USD)"] = Math.Round(Convert.ToDouble(dt.Rows[i]["eto"]), 2);
                }
                var eto = dt.Compute("Sum([ETO(USD)])", string.Empty);

                DataRow dr = dt.NewRow();
                dr[1] = "Total";
                dr[2] = sumhrs;
                dr[3] = sumplanhrrecord;
                dr[4] = sumprodplanrecords;
                dr[5] = sumhrsworked;
                dr[6] = sumactprod;
                dr[7] = Math.Round((Convert.ToDouble(sumactprod) / Convert.ToDouble(sumprodplanrecords)) * 100);
                dr[8] = tarrev;
                dr[9] = actrev;
                dr[10] = Math.Round((Convert.ToDouble(actrev) / Convert.ToDouble(tarrev)) * 100);
                dr[14] = Math.Round((Convert.ToDouble(etosumm)), 2);
                dt.Rows.Add(dr);

               
                //dt.AcceptChanges();

                DataView view = new DataView(dt);
                DataTable table2;
                if (PartId == 10)
                {
                    table2 = view.ToTable(false, "Month", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR", "% Revenue Achievement", "ETO(USD)");
                }
                else
                {
                    table2 = view.ToTable(false, "Month", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR", "% Revenue Achievement");

                }



                string fileName = "Yearly Production Details of "  + Year + ".xlsx";


               
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //Add DataTable in worksheet  
                    wb.Worksheets.Add(table2,"yearly");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        //Return xlsx Excel File  
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            
        }




        public ActionResult DownloadMonthlySummaryReport(int PartId)
        {

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //var dt = new DataTable("Test");
            double dollarrate = 0.0;

            DataTable dt = new DataTable();
            string Command = string.Empty;
            string[] strArr = null;
            strArr = Session["monthlysummaryreport"].ToString().Split(',');
            string Month = strArr[0].ToString();
            string Year = strArr[1].ToString();
            string Clientcode = strArr[2].ToString();
            string ProjectId = strArr[3].ToString();
            string Eventcode = strArr[4].ToString();
            string Process = strArr[5].ToString();
            string Location = strArr[6].ToString();
            string TL = strArr[7].ToString();
            string Resource = strArr[8].ToString();

            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }

            string dollarCommand = "SELECT rate FROM dollarsettings WHERE monthname(dollardate)='" + Month + "' and year(dollardate)=" + Year + "  ORDER BY dollardate desc LIMIT 1";
            using (MySqlConnection tarConnection = new MySqlConnection(constr))
            {
                tarConnection.Open();
                MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dollarrate = reader.GetInt32(0);
                }
                tarConnection.Close();
            }




            Command = "select   location,Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),0) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),0) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where monthname(date)='" + Month + "' and year(date)=" + Year + "";


            if (Clientcode != "ALL")
            {
                Command = Command + " and `project`='" + Clientcode + "'";
            }


            if (ProjectId != "ALL")
            {
                Command = Command + " and `projectcode`='" + ProjectId + "'";
            }
            if (Eventcode != "ALL")
            {
                Command = Command + " and `eventcode`='" + Eventcode + "'";
            }

            if (Process != "ALL")
            {
                Command = Command + " and `process`='" + Process + "'";
            }

            if (Location != "ALL")
            {
                Command = Command + " and  `location`='" + Location + "'";
            }

            if (TL != "ALL")
            {
                Command = Command + " and  `tlname`='" + TL + "'";
            }

            if (Resource != "ALL")
            {
                Command = Command + " and  `Associate`='" + Resource + "'";
            }

            Command = Command + "  group by location";



            using (MySqlConnection mConnection = new MySqlConnection(constr))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);


                System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                rateColumn.DefaultValue = dollarrate;
                dt.Columns.Add(rateColumn);


            }


         

                DataColumn column = new DataColumn();
                column.DataType = typeof(double);

                column.Expression = "ActrevenueINR/rate/cnt";
                column.ColumnName = "eto";
                dt.Columns.Add(column);

                var sumhrs = dt.Compute("Sum(hoursplanned)", string.Empty);
                var sumplanhrrecord = dt.Compute("Sum(prodplanhrrecord)", string.Empty);
                var sumprodplanrecords = dt.Compute("Sum(prodplanrecords)", string.Empty);
                var sumhrsworked = dt.Compute("Sum(hoursworked)", string.Empty);
                var sumactprod = dt.Compute("Sum(Actualprodrecord)", string.Empty);
                var tarrev = dt.Compute("Sum(TarrevenueINR)", string.Empty);
                var actrev = dt.Compute("Sum(ActrevenueINR)", string.Empty);
                var ccnt = dt.Compute("Sum(cnt)", string.Empty);
                double etosum = Convert.ToDouble(actrev) / Convert.ToDouble(dollarrate);
                var etosumm = Convert.ToDouble(etosum) / Convert.ToInt32(ccnt);

                //dt.Columns["date"].ColumnName = "Date";
                dt.Columns["location"].ColumnName = "Location";
                dt.Columns["hoursplanned"].ColumnName = "Hours planned";
                dt.Columns["prodplanhrrecord"].ColumnName = "Production planned/Hr Records";
                dt.Columns["prodplanrecords"].ColumnName = "Production planned Records";
                dt.Columns["hoursworked"].ColumnName = "Hours Worked";
                dt.Columns["Actualprodrecord"].ColumnName = "Actual Production Records";
                dt.Columns["Achievement"].ColumnName = "% Achievement";
                dt.Columns["TarrevenueINR"].ColumnName = "Target Revenue INR";
                dt.Columns["ActrevenueINR"].ColumnName = "Actual Revenue INR";
                dt.Columns["RevAchievement"].ColumnName = "% Revenue Achievement";

                if (!dt.Columns.Contains("ETO(USD)"))
                {

                    System.Data.DataColumn ETOColumn = new System.Data.DataColumn("ETO(USD)", typeof(System.Double));
                    dt.Columns.Add(ETOColumn);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["ETO(USD)"] = Math.Round(Convert.ToDouble(dt.Rows[i]["eto"]), 2);
                }
                var eto = dt.Compute("Sum([ETO(USD)])", string.Empty);

                DataRow dr = dt.NewRow();
                dr[0] = "Total";
                dr[1] = sumhrs;
                dr[2] = sumplanhrrecord;
                dr[3] = sumprodplanrecords;
                dr[4] = sumhrsworked;
                dr[5] = sumactprod;
                dr[6] = Math.Round((Convert.ToDouble(sumactprod) / Convert.ToDouble(sumprodplanrecords)) * 100);
                dr[7] = tarrev;
                dr[8] = actrev;
                dr[9] = Math.Round((Convert.ToDouble(actrev) / Convert.ToDouble(tarrev)) * 100);
                dr[11] = Math.Round((Convert.ToDouble(etosumm)));
                dr[13] = Math.Round((Convert.ToDouble(etosumm)), 2);
              
                dt.Rows.Add(dr);
                //dt.AcceptChanges();

                DataView view = new DataView(dt);
                DataTable table2;
                if (PartId == 10)
                {
                    table2 = view.ToTable(false, "Location", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR", "% Revenue Achievement", "ETO(USD)");
                }
                else
                {
                    table2 = view.ToTable(false, "Location", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR", "% Revenue Achievement");

                }



                string fileName = "Monthly Production Details of " + Month + Year + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //Add DataTable in worksheet  
                    wb.Worksheets.Add(table2, "Details");

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        //Return xlsx Excel File  
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }


                //return View("ConsolidatedReport");
            }


       




        


        // public ActionResult DownloadMonthlyReport()
        //{

        //    string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
        //    //var dt = new DataTable("Test");
        //    var dt = TempData["dtmonthly"] as DataTable;
        //     dt.TableName = "Monthly";
        //    if (dt != null)
        //    {

        //        var sumhrs = dt.Compute("Sum(hoursplanned)", string.Empty);
        //        var sumplanhrrecord = dt.Compute("Sum(prodplanhrrecord)", string.Empty);
        //        var sumprodplanrecords = dt.Compute("Sum(prodplanrecords)", string.Empty);
        //        var sumhrsworked = dt.Compute("Sum(hoursworked)", string.Empty);
        //        var sumactprod = dt.Compute("Sum(Actualprodrecord)", string.Empty);
        //        var tarrev = dt.Compute("Sum(TarrevenueINR)", string.Empty);
        //        var actrev = dt.Compute("Sum(ActrevenueINR)", string.Empty);

        //        dt.Columns["date"].ColumnName = "Date";
        //        dt.Columns["location"].ColumnName = "Location";
        //        dt.Columns["hoursplanned"].ColumnName = "Hours planned";
        //        dt.Columns["prodplanhrrecord"].ColumnName = "Production planned/Hr Records";
        //        dt.Columns["prodplanrecords"].ColumnName = "Production planned Records";
        //        dt.Columns["hoursworked"].ColumnName = "Hours Worked";
        //        dt.Columns["Actualprodrecord"].ColumnName = "Actual Production Records";
        //        dt.Columns["Achievement"].ColumnName = "% Achievement";
        //        dt.Columns["TarrevenueINR"].ColumnName = "Target Revenue INR";
        //        dt.Columns["ActrevenueINR"].ColumnName = "Actual Revenue INR";
        //        dt.Columns["RevAchievement"].ColumnName = "% Revenue Achievement";
        //        dt.Columns.Remove("cnt");
        //        dt.Columns.Remove("rate");



        //        DataRow dr = dt.NewRow();
        //        dr[1] = "Total";
        //        dr[2] = sumhrs;
        //        dr[3] = sumplanhrrecord;
        //        dr[4] = sumprodplanrecords;
        //        dr[5] = sumhrsworked;
        //        dr[6] = sumactprod;
        //        dr[7] = Math.Round((Convert.ToDouble(sumactprod) / Convert.ToDouble(sumprodplanrecords)) * 100);
        //        dr[8] = tarrev;
        //        dr[9] = actrev;
        //        dr[10] = Math.Round((Convert.ToDouble(actrev) / Convert.ToDouble(tarrev)) * 100);

        //        dt.Rows.Add(dr);

                

        //        string fileName = "DailyReport.xlsx";
        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            //Add DataTable in worksheet  
        //            wb.Worksheets.Add(dt);
        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                wb.SaveAs(stream);
        //                //Return xlsx Excel File  
        //                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //            }
        //        }  





        //    }

        //    return View("ConsolidatedReport");
        //}

        public ActionResult DownloadMonthlyReport(int PartId)
        {

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //var dt = new DataTable("Test");
            double dollarrate = 0.0;

            DataTable dt = new DataTable("Test");
            dt.Clear();
            string[] strArr = null;
            strArr = Session["monthlyreport"].ToString().Split(',');
            string Month = strArr[0].ToString();
            string Year = strArr[1].ToString();
            string Clientcode = strArr[2].ToString();
            string ProjectId = strArr[3].ToString();
            string Eventcode = strArr[4].ToString();
            string Process = strArr[5].ToString();
            string Location = strArr[6].ToString();
            string TL = strArr[7].ToString();
            string Resource = strArr[8].ToString();
            string Command = string.Empty;


            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }

            string dollarCommand = "SELECT rate FROM dollarsettings WHERE monthname(dollardate)='" + Month + "' and year(dollardate)=" + Year + "  ORDER BY dollardate desc LIMIT 1";
            using (MySqlConnection tarConnection = new MySqlConnection(constr))
            {
                tarConnection.Open();
                MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dollarrate = reader.GetInt32(0);
                }
                tarConnection.Close();
            }


            Command = "select   date_format(date, '%d/%m/%Y') as date,location,Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),0) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),0) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where monthname(date)='" + Month + "' and year(date)=" + Year + "";


            if (Clientcode != "ALL")
            {
                Command = Command + " and `project`='" + Clientcode + "'";
            }


            if (ProjectId != "ALL")
            {
                Command = Command + " and `projectcode`='" + ProjectId + "'";
            }
            if (Eventcode != "ALL")
            {
                Command = Command + " and `eventcode`='" + Eventcode + "'";
            }

            if (Process != "ALL")
            {
                Command = Command + " and `process`='" + Process + "'";
            }

            if (Location != "ALL")
            {
                Command = Command + " and  `location`='" + Location + "'";
            }

            if (TL != "ALL")
            {
                Command = Command + " and  `tlname`='" + TL + "'";
            }

            if (Resource != "ALL")
            {
                Command = Command + " and  `Associate`='" + Resource + "'";
            }




            Command = Command + "  group by date";









           
            using (MySqlConnection mConnection = new MySqlConnection(constr))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);


                System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                rateColumn.DefaultValue = dollarrate;
                dt.Columns.Add(rateColumn);


            }
           




            DataColumn column = new DataColumn();
            column.DataType = typeof(double);

            column.Expression = "ActrevenueINR/rate/cnt";
            column.ColumnName = "eto";
            dt.Columns.Add(column);

            var sumhrs = dt.Compute("Sum(hoursplanned)", string.Empty);
            var sumplanhrrecord = dt.Compute("Sum(prodplanhrrecord)", string.Empty);
            var sumprodplanrecords = dt.Compute("Sum(prodplanrecords)", string.Empty);
            var sumhrsworked = dt.Compute("Sum(hoursworked)", string.Empty);
            var sumactprod = dt.Compute("Sum(Actualprodrecord)", string.Empty);
            var tarrev = dt.Compute("Sum(TarrevenueINR)", string.Empty);
            var actrev = dt.Compute("Sum(ActrevenueINR)", string.Empty);
            var ccnt = dt.Compute("Sum(cnt)", string.Empty);
            double etosum = Convert.ToDouble(actrev) / Convert.ToDouble(dollarrate);
            var etosumm = Convert.ToDouble(etosum) / Convert.ToInt32(ccnt);

            dt.Columns["date"].ColumnName = "Date";
            dt.Columns["location"].ColumnName = "Location";
            dt.Columns["hoursplanned"].ColumnName = "Hours planned";
            dt.Columns["prodplanhrrecord"].ColumnName = "Production planned/Hr Records";
            dt.Columns["prodplanrecords"].ColumnName = "Production planned Records";
            dt.Columns["hoursworked"].ColumnName = "Hours Worked";
            dt.Columns["Actualprodrecord"].ColumnName = "Actual Production Records";
            dt.Columns["Achievement"].ColumnName = "% Achievement";
            dt.Columns["TarrevenueINR"].ColumnName = "Target Revenue INR";
            dt.Columns["ActrevenueINR"].ColumnName = "Actual Revenue INR";
            dt.Columns["RevAchievement"].ColumnName = "% Revenue Achievement";
            if (!dt.Columns.Contains("ETO(USD)"))
            {

                System.Data.DataColumn ETOColumn = new System.Data.DataColumn("ETO(USD)", typeof(System.Double));
                dt.Columns.Add(ETOColumn);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                dt.Rows[i]["ETO(USD)"] = Math.Round(Convert.ToDouble(dt.Rows[i]["eto"]), 2);
            }
            var eto = dt.Compute("Sum([ETO(USD)])", string.Empty);
            DataRow dr = dt.NewRow();
            dr[1] = "Total";
            dr[2] = sumhrs;
            dr[3] = sumplanhrrecord;
            dr[4] = sumprodplanrecords;
            dr[5] = sumhrsworked;
            dr[6] = sumactprod;
            dr[7] = Math.Round((Convert.ToDouble(sumactprod) / Convert.ToDouble(sumprodplanrecords)) * 100);
            dr[8] = tarrev;
            dr[9] = actrev;
            dr[10] = Math.Round((Convert.ToDouble(actrev) / Convert.ToDouble(tarrev)) * 100);
            dr[11] = Math.Round((Convert.ToDouble(etosumm)));
            dr[14] = Math.Round((Convert.ToDouble(etosumm)), 2);

            dt.Rows.Add(dr);
            //dt.AcceptChanges();

            DataView view = new DataView(dt);
            DataTable table2;
            if (PartId == 10)
            {
                table2 = view.ToTable(false, "Date", "Location", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR", "% Revenue Achievement", "ETO(USD)");
            }
            else
            {
                table2 = view.ToTable(false, "Date", "Location", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR", "% Revenue Achievement");

            }

            string fileName = "Monthly Production Details of " + Month + Year + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(table2, "Details");

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }


            //return View("ConsolidatedReport");
        }








         public ActionResult DownloadDailyReport(int PartId)
         {

             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             //var dt = new DataTable("Test");
             double dollarrate = 0.0;

             DataTable dt = new DataTable("Test");
             dt.Clear();
             string[] strArr = null;
             strArr = Session["dailyreport"].ToString().Split(',');
             string date = strArr[0].ToString();
             string LocationId = strArr[1].ToString();
             string Clientcode = strArr[2].ToString();
             string ProjectId = strArr[3].ToString();
             string Eventcode = strArr[4].ToString();
             string Process = strArr[5].ToString();
             string TL = strArr[6].ToString();
             String Resource = strArr[7].ToString();

             DateTime dtdate = new DateTime();
             dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
             var ddate = dtdate.ToString("yyyy-MM-dd");
           

             if (LocationId == "KAKKANAD")
             {
                 LocationId = "KKND";
             }

             string dollarCommand = "SELECT rate FROM dollarsettings WHERE dollardate <='" + ddate + "' ORDER BY dollardate desc LIMIT 1";
             using (MySqlConnection tarConnection = new MySqlConnection(constr))
             {
                 tarConnection.Open();
                 MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                 MySqlDataReader reader = cmd.ExecuteReader();
                 if (reader.Read())
                 {
                     dollarrate = reader.GetInt32(0);
                 }
                 tarConnection.Close();
              }
                string Command = string.Empty;
                Command = "select   date_format(date, '%d/%m/%Y') as date,location,Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),0) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),0) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where date='" + ddate + "'";

                if (Clientcode != "ALL")
                {
                    Command = Command + " and `project`='" + Clientcode + "'";
                }

                if (ProjectId != "ALL")
                {
                    Command = Command + " and `projectcode`='" + ProjectId + "'";
                }
                if (Eventcode != "ALL")
                {
                    Command = Command + " and `eventcode`='" + Eventcode + "'";
                }

                if (Process != "ALL")
                {
                    Command = Command + " and `process`='" + Process + "'";
                }

                if (LocationId != "ALL")
                {
                    Command = Command + " and  `location`='" + LocationId + "'";
                }

                if (TL != "ALL")
                {
                    Command = Command + " and  `tlname`='" + TL + "'";
                }

                if (Resource != "ALL")
                {
                    Command = Command + " and  `associate`='" + Resource + "'";
                }



                Command = Command + "  group by location";



                using (MySqlConnection mConnection = new MySqlConnection(constr))
                {
                    mConnection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                    adapter.Fill(dt);


                    System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                    rateColumn.DefaultValue = dollarrate;
                    dt.Columns.Add(rateColumn);
                }

                DataColumn column = new DataColumn();
                column.DataType = typeof(double);

                column.Expression = "ActrevenueINR/rate/cnt";
                column.ColumnName = "eto";
                dt.Columns.Add(column);

                var sumhrs = dt.Compute("Sum(hoursplanned)", string.Empty);
                var sumplanhrrecord = dt.Compute("Sum(prodplanhrrecord)", string.Empty);
                var sumprodplanrecords = dt.Compute("Sum(prodplanrecords)", string.Empty);
                var sumhrsworked = dt.Compute("Sum(hoursworked)", string.Empty);
                var sumactprod = dt.Compute("Sum(Actualprodrecord)", string.Empty);
                var tarrev = dt.Compute("Sum(TarrevenueINR)", string.Empty);
                var actrev = dt.Compute("Sum(ActrevenueINR)", string.Empty);
                var ccnt = dt.Compute("Sum(cnt)", string.Empty);
                double etosum = Convert.ToDouble(actrev) / Convert.ToDouble(dollarrate);
                var etosumm = Convert.ToDouble(etosum) /Convert.ToInt32(ccnt);

                dt.Columns["date"].ColumnName = "Date";
                dt.Columns["location"].ColumnName = "Location";
                dt.Columns["hoursplanned"].ColumnName = "Hours planned";
                dt.Columns["prodplanhrrecord"].ColumnName = "Production planned/Hr Records";
                dt.Columns["prodplanrecords"].ColumnName = "Production planned Records";
                dt.Columns["hoursworked"].ColumnName = "Hours Worked";
                dt.Columns["Actualprodrecord"].ColumnName = "Actual Production Records";
                dt.Columns["Achievement"].ColumnName = "% Achievement";
                dt.Columns["TarrevenueINR"].ColumnName = "Target Revenue INR";
                dt.Columns["ActrevenueINR"].ColumnName = "Actual Revenue INR";
                dt.Columns["RevAchievement"].ColumnName = "% Revenue Achievement";
                if (!dt.Columns.Contains("ETO(USD)"))
                {

                    System.Data.DataColumn ETOColumn = new System.Data.DataColumn("ETO(USD)", typeof(System.Double));
                    dt.Columns.Add(ETOColumn);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["ETO(USD)"] = Math.Round(Convert.ToDouble(dt.Rows[i]["eto"]), 2);
                }
                var eto = dt.Compute("Sum([ETO(USD)])", string.Empty);
                DataRow dr = dt.NewRow();
                dr[1] = "Total";
                dr[2] = sumhrs;
                dr[3] = sumplanhrrecord;
                dr[4] = sumprodplanrecords;
                dr[5] = sumhrsworked;
                dr[6] = sumactprod;
                if (Convert.ToDouble(sumprodplanrecords)==0)
                {
                    dr[7] = 0;
                }
                else
                {
                    dr[7] = Math.Round((Convert.ToDouble(sumactprod) / Convert.ToDouble(sumprodplanrecords)) * 100);
                }
               
                dr[8] = tarrev;
                dr[9] = actrev;
                if (Convert.ToDouble(tarrev) == 0)
                {
                    dr[10] = 0;
                }
                else
                {

                    dr[10] = Math.Round((Convert.ToDouble(actrev) / Convert.ToDouble(tarrev)) * 100);
                }
                dr[11] = Math.Round((Convert.ToDouble(etosumm)));
                dr[14] = Math.Round((Convert.ToDouble(etosumm)),2);
               
                dt.Rows.Add(dr);
                //dt.AcceptChanges();

                DataView view = new DataView(dt);
                DataTable table2;
                if (PartId == 10)
                {
                    table2 = view.ToTable(false, "Date", "Location", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR","% Revenue Achievement","ETO(USD)");
                }
                else
                {
                    table2 = view.ToTable(false, "Date", "Location", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR","% Revenue Achievement");

                }

                string fileName = "Daily Production Details-" + date + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //Add DataTable in worksheet  
                    wb.Worksheets.Add(table2, "Details");

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        //Return xlsx Excel File  
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }  


             //return View("ConsolidatedReport");
         }



         public ActionResult DDownloadDailyReport(int PartId)
        {

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //var dt = new DataTable("Test");

           
            DataTable dt = new DataTable("Test");
            dt.Clear();
            string[] strArr = null;
            strArr = Session["dailyreport"].ToString().Split(',') ;
            string Date = strArr[0].ToString();
            string LocationId = strArr[1].ToString();
            string Clientcode=strArr[2].ToString();
            string ProjectId=strArr[3].ToString();
            string Eventcode=strArr[4].ToString();
            string Process=strArr[5].ToString();
            string TL = strArr[6].ToString();
            String Resource = strArr[7].ToString();
            dt= Session["dtdaily"] as DataTable;
          
            if (dt != null)
            {
                
                    if (!dt.Columns.Contains("eto"))
                    {


                        DataColumn column = new DataColumn();
                        column.DataType = typeof(double);

                        column.Expression = "ActrevenueINR/rate/cnt";
                        column.ColumnName = "eto";
                        dt.Columns.Add(column);
                    }


                    var sumhrs = dt.Compute("Sum(hoursplanned)", string.Empty);
                    var sumplanhrrecord = dt.Compute("Sum(prodplanhrrecord)", string.Empty);
                    var sumprodplanrecords = dt.Compute("Sum(prodplanrecords)", string.Empty);
                    var sumhrsworked = dt.Compute("Sum(hoursworked)", string.Empty);
                    var sumactprod = dt.Compute("Sum(Actualprodrecord)", string.Empty);
                    var tarrev = dt.Compute("Sum(TarrevenueINR)", string.Empty);
                    var actrev = dt.Compute("Sum(ActrevenueINR)", string.Empty);
                    var etosum = dt.Compute("Sum(eto)", string.Empty);


                    dt.Columns["date"].ColumnName = "Date";
                    dt.Columns["location"].ColumnName = "Location";
                    dt.Columns["hoursplanned"].ColumnName = "Hours planned";
                    dt.Columns["prodplanhrrecord"].ColumnName = "Production planned/Hr Records";
                    dt.Columns["prodplanrecords"].ColumnName = "Production planned Records";
                    dt.Columns["hoursworked"].ColumnName = "Hours Worked";
                    dt.Columns["Actualprodrecord"].ColumnName = "Actual Production Records";
                    dt.Columns["Achievement"].ColumnName = "% Achievement";
                    dt.Columns["TarrevenueINR"].ColumnName = "Target Revenue INR";
                    dt.Columns["ActrevenueINR"].ColumnName = "Actual Revenue INR";
                    dt.Columns["RevAchievement"].ColumnName = "% Revenue Achievement";
                    if (!dt.Columns.Contains("ETO(USD)"))
                    {

                        System.Data.DataColumn ETOColumn = new System.Data.DataColumn("ETO(USD)", typeof(System.Double));
                        dt.Columns.Add(ETOColumn);
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        dt.Rows[i]["ETO(USD)"] = Math.Round(Convert.ToDouble(dt.Rows[i]["eto"]), 2);
                    }
                    var eto = dt.Compute("Sum([ETO(USD)])", string.Empty);
                    DataRow dr = dt.NewRow();
                    dr[1] = "Total";
                    dr[2] = sumhrs;
                    dr[3] = sumplanhrrecord;
                    dr[4] = sumprodplanrecords;
                    dr[5] = sumhrsworked;
                    dr[6] = sumactprod;
                    dr[7] = Math.Round((Convert.ToDouble(sumactprod) / Convert.ToDouble(sumprodplanrecords)) * 100);
                    dr[8] = tarrev;
                    dr[9] = actrev;
                    dr[10] = Math.Round((Convert.ToDouble(actrev) / Convert.ToDouble(tarrev)) * 100);
                    dr[11] = Math.Round((Convert.ToDouble(etosum)));

                    dr[14] = Math.Round((Convert.ToDouble(eto)));
                    dt.Rows.Add(dr);
                    //dt.AcceptChanges();
           
                DataView view = new DataView(dt);
                DataTable table2;
                if (PartId == 10)
                {
                     table2 = view.ToTable(false, "Date", "Location", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR", "ETO(USD)");
                }
                else
                {
                    table2 = view.ToTable(false, "Date", "Location", "Hours planned", "Production planned/Hr Records", "Hours Worked", "Actual Production Records", "% Achievement", "Target Revenue INR", "Actual Revenue INR");

                }
              
                string fileName = "Daily Production Details-" + dt.Rows[0]["Date"] + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //Add DataTable in worksheet  
                    wb.Worksheets.Add(table2,"Details");
                   
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        //Return xlsx Excel File  
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }  





            }

            return View("ConsolidatedReport");
        }
           
        
        public ActionResult DownloadConsolidatedReport(string date)
        {

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;



            //string query = "select date,productionreport.location, sum(plannedhrs) as hoursplanned ,ROUND(sum(plannedhrrecord),0) as prodplanhrRecord,ROUND(sum(plannedprodrecord),0) as prodplanRecord,ROUND(sum(workedhrs),0) as  RecordsHours,ROUND(sum(actualprodrecord),0) as ActualProdRecords, ROUND((sum(actualprodrecord)/sum(plannedprodrecord))*100,0) as Achievement,ROUND(sum(targetrevenue*plannedprodrecord),2) as TargetRevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)),2) as actualrevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0))/sum(plannedprodrecord*targetrevenue)*100,0) as RevenueAchievement   from `productionreport` group by productionreport.location,date;";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement ,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='TVM';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KNPY';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MDS';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MQC';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MNS';";
            //query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KAKKANAD';";
            //;

            string query = "select DATE_FORMAT(date, '%d/%m/%y') as date,productionreport.location, sum(plannedhrs) as hoursplanned ,ROUND(sum(plannedhrrecord),0) as prodplanhrRecord,ROUND(sum(plannedprodrecord),0) as prodplanRecord,ROUND(sum(workedhrs),0) as  RecordsHours,ROUND(sum(actualprodrecord),0) as ActualProdRecords, ROUND((sum(actualprodrecord)/sum(plannedprodrecord))*100,0) as Achievement,ROUND(sum(targetrevenue*plannedprodrecord),2) as TargetRevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)),2) as actualrevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0))/sum(plannedprodrecord*targetrevenue)*100,0) as RevenueAchievement   from `productionreport` group by productionreport.location,date;";
            query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement ,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='TVM';";
            query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KNPY';";
            query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MDS';";
            query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MQC';";
            query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MNS';";
            query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KAKKANAD';";
            ;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);

                            //Set Name of DataTables.
                            ds.Tables[0].TableName = "Summary";
                            ds.Tables[1].TableName = "TVM";
                            ds.Tables[2].TableName = "KNPY";
                            ds.Tables[3].TableName = "MDS";
                            ds.Tables[4].TableName = "MQC";
                            ds.Tables[5].TableName = "MNS";
                            ds.Tables[6].TableName = "KAKKANAD";
                            date = ds.Tables[0].Rows[0]["date"].ToString();
                            ds.Tables[0].Columns["date"].ColumnName = "Date";
                            ds.Tables[0].Columns["location"].ColumnName = "Location";
                            ds.Tables[0].Columns["hoursplanned"].ColumnName = "Hours planned";
                            ds.Tables[0].Columns["prodplanhrRecord"].ColumnName = "Production planned/Hr Records";
                            ds.Tables[0].Columns["prodplanRecord"].ColumnName = "Production   planned   Records";
                            ds.Tables[0].Columns["RecordsHours"].ColumnName = "Hours worked";
                            ds.Tables[0].Columns["ActualProdRecords"].ColumnName = "Actual Production Records";
                            ds.Tables[0].Columns["Achievement"].ColumnName = "% Achievement";
                            ds.Tables[0].Columns["TargetRevenue"].ColumnName = "Target Revenue INR";
                            ds.Tables[0].Columns["ActualRevenue"].ColumnName = "Actual Revenue INR";
                            ds.Tables[0].Columns["RevenueAchievement"].ColumnName = "% Revenue Achievement";
                            ds.Tables[0].AcceptChanges();





                            ds.Tables[1].Columns["psn"].ColumnName = "PSN";
                            ds.Tables[1].Columns["associate"].ColumnName = "Associates Name";
                            ds.Tables[1].Columns["process"].ColumnName = "Process";
                            ds.Tables[1].Columns["project"].ColumnName = "Project";
                            ds.Tables[1].Columns["projectcode"].ColumnName = "Project Code";
                            ds.Tables[1].Columns["eventcode"].ColumnName = "Event code";
                            ds.Tables[1].Columns["tlname"].ColumnName = "TL's Name";
                            ds.Tables[1].Columns["plannedhrs"].ColumnName = "Hours planned";
                            ds.Tables[1].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                            ds.Tables[1].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                            ds.Tables[1].Columns["workedhrs"].ColumnName = "Hours worked";
                            ds.Tables[1].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                            ds.Tables[1].Columns["achievement"].ColumnName = "% Achievement";
                            ds.Tables[1].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                            ds.Tables[1].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                            ds.Tables[1].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                            ds.Tables[1].Columns["productivity"].ColumnName = "Productivity(per hr)";
                            ds.Tables[1].AcceptChanges();

                            ds.Tables[2].Columns["psn"].ColumnName = "PSN";
                            ds.Tables[2].Columns["associate"].ColumnName = "Associates Name";
                            ds.Tables[2].Columns["process"].ColumnName = "Process";
                            ds.Tables[2].Columns["project"].ColumnName = "Project";
                            ds.Tables[2].Columns["projectcode"].ColumnName = "Project Code";
                            ds.Tables[2].Columns["eventcode"].ColumnName = "Event code";
                            ds.Tables[2].Columns["tlname"].ColumnName = "TL's Name";
                            ds.Tables[2].Columns["plannedhrs"].ColumnName = "Hours planned";
                            ds.Tables[2].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                            ds.Tables[2].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                            ds.Tables[2].Columns["workedhrs"].ColumnName = "Hours worked";
                            ds.Tables[2].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                            ds.Tables[2].Columns["achievement"].ColumnName = "% Achievement";
                            ds.Tables[2].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                            ds.Tables[2].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                            ds.Tables[2].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                            ds.Tables[2].Columns["productivity"].ColumnName = "Productivity(per hr)";
                            ds.Tables[2].AcceptChanges();

                            ds.Tables[3].Columns["psn"].ColumnName = "PSN";
                            ds.Tables[3].Columns["associate"].ColumnName = "Associates Name";
                            ds.Tables[3].Columns["process"].ColumnName = "Process";
                            ds.Tables[3].Columns["project"].ColumnName = "Project";
                            ds.Tables[3].Columns["projectcode"].ColumnName = "Project Code";
                            ds.Tables[3].Columns["eventcode"].ColumnName = "Event code";
                            ds.Tables[3].Columns["tlname"].ColumnName = "TL's Name";
                            ds.Tables[3].Columns["plannedhrs"].ColumnName = "Hours planned";
                            ds.Tables[3].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                            ds.Tables[3].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                            ds.Tables[3].Columns["workedhrs"].ColumnName = "Hours worked";
                            ds.Tables[3].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                            ds.Tables[3].Columns["achievement"].ColumnName = "% Achievement";
                            ds.Tables[3].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                            ds.Tables[3].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                            ds.Tables[3].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                            ds.Tables[3].Columns["productivity"].ColumnName = "Productivity(per hr)";
                            ds.Tables[3].AcceptChanges();


                            ds.Tables[4].Columns["psn"].ColumnName = "PSN";
                            ds.Tables[4].Columns["associate"].ColumnName = "Associates Name";
                            ds.Tables[4].Columns["process"].ColumnName = "Process";
                            ds.Tables[4].Columns["project"].ColumnName = "Project";
                            ds.Tables[4].Columns["projectcode"].ColumnName = "Project Code";
                            ds.Tables[4].Columns["eventcode"].ColumnName = "Event code";
                            ds.Tables[4].Columns["tlname"].ColumnName = "TL's Name";
                            ds.Tables[4].Columns["plannedhrs"].ColumnName = "Hours planned";
                            ds.Tables[4].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                            ds.Tables[4].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                            ds.Tables[4].Columns["workedhrs"].ColumnName = "Hours worked";
                            ds.Tables[4].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                            ds.Tables[4].Columns["achievement"].ColumnName = "% Achievement";
                            ds.Tables[4].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                            ds.Tables[4].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                            ds.Tables[4].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                            ds.Tables[4].Columns["productivity"].ColumnName = "Productivity(per hr)";
                            ds.Tables[4].AcceptChanges();


                            ds.Tables[5].Columns["psn"].ColumnName = "PSN";
                            ds.Tables[5].Columns["associate"].ColumnName = "Associates Name";
                            ds.Tables[5].Columns["process"].ColumnName = "Process";
                            ds.Tables[5].Columns["project"].ColumnName = "Project";
                            ds.Tables[5].Columns["projectcode"].ColumnName = "Project Code";
                            ds.Tables[5].Columns["eventcode"].ColumnName = "Event code";
                            ds.Tables[5].Columns["tlname"].ColumnName = "TL's Name";
                            ds.Tables[5].Columns["plannedhrs"].ColumnName = "Hours planned";
                            ds.Tables[5].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                            ds.Tables[5].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                            ds.Tables[5].Columns["workedhrs"].ColumnName = "Hours worked";
                            ds.Tables[5].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                            ds.Tables[5].Columns["achievement"].ColumnName = "% Achievement";
                            ds.Tables[5].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                            ds.Tables[5].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                            ds.Tables[5].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                            ds.Tables[5].Columns["productivity"].ColumnName = "Productivity(per hr)";
                            ds.Tables[5].AcceptChanges();



                            ds.Tables[6].Columns["psn"].ColumnName = "PSN";
                            ds.Tables[6].Columns["associate"].ColumnName = "Associates Name";
                            ds.Tables[6].Columns["process"].ColumnName = "Process";
                            ds.Tables[6].Columns["project"].ColumnName = "Project";
                            ds.Tables[6].Columns["projectcode"].ColumnName = "Project Code";
                            ds.Tables[6].Columns["eventcode"].ColumnName = "Event code";
                            ds.Tables[6].Columns["tlname"].ColumnName = "TL's Name";
                            ds.Tables[6].Columns["plannedhrs"].ColumnName = "Hours planned";
                            ds.Tables[6].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                            ds.Tables[6].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                            ds.Tables[6].Columns["workedhrs"].ColumnName = "Hours worked";
                            ds.Tables[6].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                            ds.Tables[6].Columns["achievement"].ColumnName = "% Achievement";
                            ds.Tables[6].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                            ds.Tables[6].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                            ds.Tables[6].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                            ds.Tables[6].Columns["productivity"].ColumnName = "Productivity(per hr)";
                            ds.Tables[6].AcceptChanges();




                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable dt in ds.Tables)
                                {

                                    if (dt == ds.Tables[0])
                                    {

                                        decimal hoursplanned = 0;
                                        decimal prodplanhrRecord = 0;
                                        decimal prodplanRecord = 0;
                                        decimal RecordsHours = 0;
                                        decimal ActualProdRecords = 0;
                                        decimal TargetRevenue = 0;
                                        decimal ActualRevenue = 0;
                                        foreach (DataRow row in dt.Rows)
                                        {



                                            if (row[2].ToString() != "")
                                            {
                                                hoursplanned += decimal.Parse(row[2].ToString());
                                            }
                                            if (row[3].ToString() != "")
                                            {
                                                prodplanhrRecord += decimal.Parse(row[3].ToString());
                                            }

                                            if (row[4].ToString() != "")
                                            {
                                                prodplanRecord += decimal.Parse(row[4].ToString());
                                            }

                                            if (row[5].ToString() != "")
                                            {
                                                RecordsHours += decimal.Parse(row[5].ToString());
                                            }

                                            if (row[6].ToString() != "")
                                            {
                                                ActualProdRecords += decimal.Parse(row[6].ToString());
                                            }

                                            if (row[8].ToString() != "")
                                            {
                                                TargetRevenue += decimal.Parse(row[8].ToString());
                                            }
                                            if (row[9].ToString() != "")
                                            {
                                                ActualRevenue += decimal.Parse(row[9].ToString());
                                            }
                                        }
                                        dt.Rows.Add("", "All Location", hoursplanned, prodplanhrRecord, prodplanRecord, RecordsHours, ActualProdRecords, Math.Round((ActualProdRecords / prodplanRecord) * 100), TargetRevenue, ActualRevenue, Math.Round((ActualRevenue / TargetRevenue) * 100));

                                    }










                                    if (dt != ds.Tables[0])
                                    {
                                        decimal plannedhrs = 0;
                                        decimal plannedhrrecord = 0;
                                        decimal plannedprodrecord = 0;
                                        decimal workedhrs = 0;
                                        decimal actualprodrecord = 0;
                                        decimal targetrevenue = 0;
                                        decimal actualrevenue = 0;
                                        foreach (DataRow row in dt.Rows)
                                        {
                                            if (row[7].ToString() != "")
                                            {
                                                plannedhrs += decimal.Parse(row[7].ToString());
                                            }
                                            if (row[8].ToString() != "")
                                            {
                                                plannedhrrecord += decimal.Parse(row[8].ToString());
                                            }
                                            if (row[9].ToString() != "")
                                            {
                                                plannedprodrecord += decimal.Parse(row[9].ToString());
                                            }
                                            if (row[10].ToString() != "")
                                            {
                                                workedhrs += decimal.Parse(row[10].ToString());
                                            }
                                            if (row[11].ToString() != "")
                                            {
                                                actualprodrecord += decimal.Parse(row[11].ToString());
                                            }
                                            if (row[16].ToString() != "")
                                            {
                                                targetrevenue += decimal.Parse(row[16].ToString());
                                            }
                                            if (row[17].ToString() != "")
                                            {
                                                actualrevenue += decimal.Parse(row[17].ToString());
                                            }
                                        }

                                        if (plannedhrs != 0 && plannedhrrecord != 0)
                                        {

                                            dt.Rows.Add("", "", "", "", "", "", "TOTALS", plannedhrs, plannedhrrecord, plannedprodrecord, workedhrs, actualprodrecord, (actualprodrecord / plannedprodrecord) * 100, "", "", "", targetrevenue, actualrevenue, (actualrevenue / targetrevenue) * 100);
                                        }
                                    }



                                    wb.Worksheets.Add(dt);
                                }

                                string[] strArr = null;
                                char[] splitchar = { '/' };
                                strArr = date.Split(splitchar);
                                if (strArr.Length > 0)
                                    date = strArr[1] + "." + strArr[0] + "." + strArr[2];


                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=Master Report-" + date + ".xlsx");
                                // Response.AddHeader("content-disposition", "attachment;filename=Master Report.xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }

                            }




                        }
                    }
                }
            }



            return View("ConsolidatedProductionReport");

        }

        public ActionResult EmployeeList()
        {
            NewEmployee Model = new NewEmployee();
            string user = System.Web.HttpContext.Current.User.Identity.Name;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT memployee.Id ,memployee.PSN,memployee.`AssociateName`,memployee.`Location`,memployee.DOJ from memployee where TLid=" + int.Parse(user) + "";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.EmployeeList = dtt.DataTableToList<NewEmployee>();

            }
            return View("EmployeeList", Model);
        }



        public ActionResult Userdetailsupload()
        {
            return View("Uploaduserdetails");
        }

        public ActionResult Uploadprojectconfiguration()
        {
            return View("UploadProjectConfiguration");
        }

        public ActionResult UploadRevenueconfiguration()
        {
            return View("UploadRevenueconfiguration");
        }

        public ActionResult DailyConsolidatedReportExcel(string date, string location)
        {

            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "select date,productionreport.location, sum(plannedhrs) as hoursplanned ,ROUND(sum(plannedhrrecord),0) as prodplanhrRecord,ROUND(sum(plannedprodrecord),0) as prodplanRecord,ROUND(sum(workedhrs),0) as  RecordsHours,ROUND(sum(actualprodrecord),0) as ActualProdRecords, ROUND((sum(actualprodrecord)/sum(plannedprodrecord))*100,0) as Achievement,ROUND(sum(targetrevenue*plannedprodrecord),0) as TargetRevenue,ROUND(sum(actualprodrecord*targetrevenue),2) as ActualRevenue,ROUND(sum(targetrevenue*actualprodrecord)/sum(plannedprodrecord*targetrevenue)*100,0) as RevenueAchievement   from bpoattendance.`productionreport` group by productionreport.location,date";


            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);
            }

            DataGrid DataGrd = new System.Web.UI.WebControls.DataGrid();

            dt.Columns["hoursplanned"].ColumnName = "Hours planned";
            dt.Columns["prodplanhrRecord"].ColumnName = "Production planned/Hr Records";
            dt.Columns["prodplanRecord"].ColumnName = "Production  planned  Records";
            dt.Columns["RecordsHours"].ColumnName = "Hours worked";
            dt.Columns["actualprodrecords"].ColumnName = "Actual Production Records";
            dt.Columns["achievement"].ColumnName = "% Achievement";
            dt.Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            dt.Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            dt.Columns["RevenueAchievement"].ColumnName = "% REVENUE ACHIEVEMENT";

            dt.AcceptChanges();

            DataGrd.DataSource = dt;
            DataGrd.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ConsolidatedReport.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            DataGrd.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            return View("ProductionuploadIndex");

        }


        public ActionResult CustomerRelease()
        {
            DataTable dt=new DataTable();
            FinalQcModel model = new FinalQcModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "select id,project,location,TL,date_format(`proddate`, '%d/%m/%Y') as proddate,`Eventcode`,`noofcharacters` from productiontocustomer";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }
            model.QcModelList = dt.DataTableToList<FinalQcModel>();
            return View("FinalQctoCusList", model);
        }

        public ActionResult GenerateRevenue()
        {
            DataTable dt = new DataTable();
            RevenuelistModel model = new RevenuelistModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "select id,projectcode,eventcode,`noofbatches`,invoicedcharacter,pendingcharacter,pendingp2,totalcharacter,date_format(`upldate`, '%d/%m/%Y') as upldate,location from revenuereport order by id desc";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }
            model.RevenueModelList = dt.DataTableToList<RevenuelistModel>();
            return View("RevenueDetailsList", model);
        }

        public ActionResult GetRevRepPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            RevenuelistModel Model = new RevenuelistModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT * FROM `revenuereport` where revenuereport.Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.projectcode = reader.GetString("projectcode");
                    Model.eventcode = reader.GetString("eventcode");

                    if (reader.IsDBNull(reader.GetOrdinal("pendingcharacter")))
                    {
                        Model.pendingcharacter = 0;
                    }
                    else
                    {
                        Model.pendingcharacter = reader.GetDouble("pendingcharacter");
                    }
                    if (reader.IsDBNull(reader.GetOrdinal("pendingp2")))
                    {
                        Model.pendingp2 = 0;
                    }
                    else
                    {
                        Model.pendingp2 = reader.GetDouble("pendingp2");
                    }
                    if (reader.IsDBNull(reader.GetOrdinal("totalcharacter")))
                    {
                        Model.Total = 0;
                    }
                    else
                    {
                        Model.Total = reader.GetDouble("totalcharacter");
                    }


                    //Model.pendingcharacter =reader.IsDBNull("pendingcharacter") ? (double?)null : reader.GetInt32("pendingcharacter");
                    Model.noofbatches = (reader["noofbatches"].GetType() != typeof(DBNull)) ? (Double)reader["noofbatches"] : 0;
                    Model.invoicedcharacter = (reader["invoicedcharacter"].GetType() != typeof(DBNull)) ? (Double)reader["invoicedcharacter"] : 0; reader.GetDouble("invoicedcharacter");
                    //Model.pendingcharacter = (reader["pendingcharacter"].GetType() != typeof(DBNull)) ? (Double)reader["pendingcharacter"] : 0; reader.GetDouble("pendingcharacter");
                    //Model.pendingp2 = (reader["pendingp2"].GetType() != typeof(DBNull)) ? (Double)reader["pendingp2"] : 0;
                    //Model.Total = (reader["totalcharacter"].GetType() != typeof(DBNull)) ? (Double)reader["totalcharacter"] : 0; reader.GetDouble("totalcharacter");
                    //Model.invoicedcharacter = reader.GetDouble("ratecharacter");
                    Model.Location = reader.GetString("location");
                    Model.upldate = reader.GetString("upldate");
                    Model.clientcode = reader.GetString("Clientcode");
                    Model.batchname = reader.GetString("batchname");
                    Model.RO = reader.GetString("RO");

                }

            }

            return PartialView("/Views/Admin/_GetRevRepPopup.cshtml", Model);
        }

        public ActionResult SaveRevenueDetails(RevenuelistModel model)
        {

            if (ModelState.IsValid)
            {

                string Result = ManageRevenueDetails(model);
                if (Result.Trim('"') == "Ok")
                    TempData["Msg"] = "Successfully Saved!";
                else
                    TempData["Msg"] = "Unsuccessfull Operation!";
            }


            return RedirectToAction("GenerateRevenue");
        }

        public string ManageRevenueDetails(RevenuelistModel model)
        {
            string Result = string.Empty;
            string location = string.Empty;
            Result = "NotOk";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "UPDATE `revenuereport` set `projectcode`='" + model.projectcode + "', `location`='" + model.Location + "',`noofbatches`=" + model.noofbatches + ",`invoicedcharacter`=" + model.invoicedcharacter + ",`pendingcharacter`=" + model.pendingcharacter + ",`pendingp2`=" + model.pendingp2 + ",`totalcharacter`=" + model.Total + ",`Clientcode`='" + model.clientcode + "',location='" + model.Location + "'    where `revenuereport`.Id=" + model.Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                {
                    myCmd.ExecuteNonQuery();
                    Result = "Ok";
                }

            }

            return Result;
        }




        public ActionResult PromotionRelease()
        {
            DataTable dt = new DataTable();
            PromotionModel model = new PromotionModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "select id,concat(project,'.',eventcode) as project,`noofbatches`,`Totalpromotion`,date_format(`proddate`, '%d/%m/%Y') as proddate,location from promotiontocustomer order by id desc";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }
            model.PromotionModelList = dt.DataTableToList<PromotionModel>();
            return View("PromotionList", model);
        }


       

        
         public ActionResult GetQCListPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            FinalQcModel Model = new FinalQcModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT * FROM productiontocustomer where productiontocustomer.Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.project = reader.GetString("project");
                    Model.Location = reader.GetString("location");
                    Model.noofbatches = reader.GetDouble("noofbatches");
                    Model.TL = reader.GetString("TL");
                    Model.proddate = reader.GetString("proddate");
                    Model.Eventcode = reader["Eventcode"].ToString();
                }

            }
           
            return PartialView("/Views/Admin/_GetQcListPopup.cshtml", Model);
        }


         public string DeleteCustomerRelease(string ID)
         {
             int Id = int.Parse(ID);
             string Result = "0";
             string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection mConnection = new MySqlConnection(connString))
             {
                 mConnection.Open();
                 MySqlCommand cmd = new MySqlCommand();
                 cmd.Connection = mConnection;
                 cmd.CommandText = "delete from  `productiontocustomer`  where productiontocustomer.Id=" + Id;
                 cmd.ExecuteNonQuery();
                 cmd.Dispose();
                 Result = "1";
             }
             return Result;
         }

         public string DeletePromotionDetails(string ID)
         {
             int Id = int.Parse(ID);
             string Result = "0";
             string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection mConnection = new MySqlConnection(connString))
             {
                 mConnection.Open();
                 MySqlCommand cmd = new MySqlCommand();
                 cmd.Connection = mConnection;
                 cmd.CommandText = "delete from promotiontocustomer where promotiontocustomer.Id=" + Id;
                 cmd.ExecuteNonQuery();
                 cmd.Dispose();
                 Result = "1";
             }
             return Result;
         }





        public ActionResult DeleteQCCusList(string ID)
        {
            try
            {
                DeleteCustomerRelease(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("CustomerRelease");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("CustomerRelease");
            }
        }

        public ActionResult DeletePromotionList(string ID)
        {
            try
            {
                DeletePromotionDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("PromotionRelease");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("PromotionRelease");
            }
        }







         public ActionResult GetPromotionListPopup(string ID)
         {
             int Id = Convert.ToInt16(ID);
             FinalQcModel Model = new FinalQcModel();
             string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             string Command = "SELECT * FROM `promotiontocustomer` where promotiontocustomer.Id=" + Id;
             using (MySqlConnection mConnection = new MySqlConnection(connString))
             {
                 mConnection.Open();
                 MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                 MySqlDataReader reader = cmd.ExecuteReader();
                 if (reader.Read())
                 {
                     Model.Id = reader.GetInt32(0);
                     Model.project = reader.GetString("project");
                     Model.Eventcode = reader.GetString("eventcode");
                     Model.noofbatches = reader.GetDouble("noofbatches");
                     Model.totalpromotion = reader.GetDouble("Totalpromotion");
                     Model.Location = reader.GetString("location");
                     Model.proddate = reader.GetString("proddate");
                     Model.characterrate = reader.GetDouble("characterrate");
                     Model.Clientcode = reader.GetString("Clientcode");
                     Model.proddate = reader.GetString("proddate");
                 }

             }

             return PartialView("/Views/Admin/_GetPromotionListPopup.cshtml", Model);
         }

         public string ManagePromoDetails(PromotionModel model)
         {
             string Result = string.Empty;
             string location = string.Empty;
             Result = "NotOk";
              string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
              string Command = "UPDATE `promotiontocustomer` set `project`='" + model.project + "', `location`='" + model.Location + "',`noofbatches`=" + model.noofbatches + ",`Totalpromotion`=" + model.totalpromotion + ",`characterrate`=" + model.ratecharacter + " where promotiontocustomer.Id=" + model.Id;
                 using (MySqlConnection mConnection = new MySqlConnection(connString))
                 {
                     mConnection.Open();
                     using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                     {
                         myCmd.ExecuteNonQuery();
                         Result = "Ok";
                     }

                 }

             return Result;
         }





         public string ManageQcDetails(FinalQcModel model)
         {
             string Result = string.Empty;
             string location = string.Empty;
             Result = "NotOk";
              string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
              string Command = "UPDATE `productiontocustomer` set `project`='" + model.project + "', `location`='" + model.Location + "',`noofbatches`=" + model.noofbatches + ",`TL`='" + model.TL + "',Eventcode='" + model.Eventcode + "' where `productiontocustomer`.Id=" + model.Id;
                 using (MySqlConnection mConnection = new MySqlConnection(connString))
                 {
                     mConnection.Open();
                     using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                     {
                         myCmd.ExecuteNonQuery();
                         Result = "Ok";
                     }

                 }

             return Result;
         }






         public ActionResult SavePromoDetails(PromotionModel model)
        {
          
            if (ModelState.IsValid)
            {
              
                    string Result = ManagePromoDetails(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }

          
            return RedirectToAction("PromotionRelease");
        }

         public ActionResult SaveQC(FinalQcModel model)
         {
            
             if (ModelState.IsValid)
             {
                 
                     string Result = ManageQcDetails(model);
                     if (Result.Trim('"') == "Ok")
                         TempData["Msg"] = "Successfully Saved!";
                     else
                         TempData["Msg"] = "Unsuccessfull Operation!";
                 


             }
             return RedirectToAction("CustomerRelease");

         }




        








        public ActionResult Rep_Valid(string date)
        {
      
            SummarySheetModel Model = new SummarySheetModel();
            if (date != "")
            {
                ViewBag.Date = date;
                string[] strArr = null;
                char[] splitchar = { '/' };
                strArr = date.Split(splitchar);
                if (strArr.Length > 0)
                    date = strArr[1] + "/" + strArr[0] + "/" + strArr[2];

               // int days = DateTime.DaysInMonth(int.Parse(strArr[2]), int.Parse(strArr[0]));
                string ddate = strArr[2] + "-" + strArr[1] + "-" + strArr[0];
                string Command = string.Empty;
                DataTable dt = new DataTable();
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                Double dollarrate=0.0;

                //string tarCommand = "SELECT COALESCE(SUM(Revenueconfiguration),0) AS Revenueconfiguration FROM monthlyconfiguration where MONTH =" + strArr[0] + " and year=" + strArr[2] + "";
                //using (MySqlConnection tarConnection = new MySqlConnection(connString))
                //{
                //    tarConnection.Open();
                //    MySqlCommand cmd = new MySqlCommand(tarCommand, tarConnection);
                //    MySqlDataReader reader = cmd.ExecuteReader();
                //    if (reader.Read())
                //    {
                //        tarrevenue = reader.GetInt32(0);
                //    }

                //}

                string dollarCommand = "SELECT rate FROM dollarsettings WHERE dollardate <='" + ddate + "' ORDER BY dollardate desc LIMIT 1";
                using (MySqlConnection tarConnection = new MySqlConnection(connString))
                {
                    tarConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        dollarrate = reader.GetInt32(0);
                    }
                    tarConnection.Close();
                }


                string llocation = string.Empty;
                string locCommand = "SELECT location FROM Holiday where `holidaydate`='" + ddate + "'";
                using (MySqlConnection tarConnection = new MySqlConnection(connString))
                {
                    tarConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(locCommand, tarConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        llocation = llocation + "," + (reader.GetString(0));
                        
                    }
                    if (llocation.Length > 0)
                    {
                        llocation = llocation.Remove(0, 1);
                        ViewBag.llocation = llocation;
                    }
                }
                


                //Command = "SELECT date,ROUND(sum(actualrevenue), 0)  as actualrevenue  ,ROUND(sum(targetrevenue), 0) as targetrevenue,ROUND((sum(actualrevenue)/sum(targetrevenue)*100),0) as achievement from `productionreport2020` where    MONTH(date) =" + strArr[0] + " and year(date)=" + strArr[2] + " group by  date";

                //Command = "SELECT date, sum(actualrevenue) as actualrevenue  ,sum(targetrevenue) as targetrevenue,(sum(actualrevenue)/sum(targetrevenue)*100) as achievement from `productionreport2020` where `location`='" + LocationId + "' and   MONTH(date) =" + strArr[0] + " and year(date)=" + strArr[2] + " group by  date";

                Command = "select   date_format(date, '%d/%m/%Y') as date,location,Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),0) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),2) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where date='" + ddate + "' group by location";

                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                    adapter.Fill(dt);
                   

                    System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                    rateColumn.DefaultValue = dollarrate;
                    dt.Columns.Add(rateColumn);


                }
                Model.lstSummarySheetmodel = dt.DataTableToList<SummarySheetModel>();
            }


                return PartialView("/Views/Admin/_UploadRevenue.cshtml", Model);
           

        }



        public ActionResult Deletemisentry(string ID)
        {

           string month=string.Empty;
           string day=string.Empty;
           string ddate=string.Empty;
           string passdate = string.Empty;

           
              var split = ID.Split('/');

              if (split[0].Length == 1)
                  day = "0" + split[0].ToString();
              else
                  day = split[0].ToString();

              if (split[1].Length == 1)
                  month = "0" + split[1].ToString();
              else
                  month = split[1].ToString();

              ddate =split[2].ToString() + '-' +  month + '-' + day ;
             passdate= month + '/' + day  + '/' + split[2].ToString();

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "Delete from `productionreport2020` where `productionreport2020`.`date`='" + ddate + "' and id<>0";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
               
            }

            return RedirectToAction("ProductionuploadIndexByAdmin");

            //return RedirectToAction("Rep_Valid", "Admin", new { date = passdate });
                
         

        }



        public ActionResult LocationwiseFileupload(string LocationId, string date)
        {


            ReasonModel Model = new ReasonModel();
            string[] strArr = null;
            char[] splitchar = { '/' };
            strArr = date.Split(splitchar);
            if (strArr.Length > 0)
                date = strArr[1] + "/" + strArr[0] + "/" + strArr[2];

            int days = DateTime.DaysInMonth(int.Parse(strArr[2]), int.Parse(strArr[0]));


            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //string Command = "SELECT  Distinct `date`,`location`from `production` where `location`='" + LocationId + "' and   MONTH(STR_TO_DATE(`date`, '%m/%d/%Y')) = " + strArr[0] + "";
            string Command = "SELECT  Distinct `date`,`location`from `production` where `location`='" + LocationId + "' and   MONTH(date) =" + strArr[0] + " and year(date)=" + strArr[2] + "";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);
                Model.ReasonModelList = dt.DataTableToList<ReasonModel>();

            }




            return PartialView("/Views/Admin/_UploadReason.cshtml", Model);

        }

        public ActionResult NewEmployee()
        {
            NewEmployee Model = new NewEmployee();
            return PartialView("/Views/Admin/_NewEmployee.cshtml", Model);
        }




        public ActionResult DailyPrintReport(string date, string location)
        {

            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,`achievement`,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode FROM `productionreport`";


            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);
            }

            DataGrid DataGrd = new System.Web.UI.WebControls.DataGrid();


            dt.Columns.Add("productivity", typeof(Double));

            foreach (DataRow dr in dt.Rows)
            {
                //need to set value to MyRow column 

                dr["actualrevenue"] = Math.Round(Convert.ToDouble(dr["actualrevenue"]), 2);
                if (dr["plannedhrrecord"].ToString() != "")
                {

                    dr["plannedhrrecord"] = Math.Round(Convert.ToDouble(dr["plannedhrrecord"]), 0);
                }
                if (dr["plannedprodrecord"].ToString() != "")
                {
                    dr["plannedprodrecord"] = Math.Round(Convert.ToDouble(dr["plannedprodrecord"]));
                }
                if (dr["targetrevenue"].ToString() != "")
                {
                    dr["targetrevenue"] = Math.Round(Convert.ToDecimal(dr["targetrevenue"]), 2);
                }
                if (dr["actualrevenue"].ToString() != "")
                {
                    dr["actualrevenue"] = Math.Round(Convert.ToDecimal(dr["actualrevenue"]), 2);
                }
                if (dr["revenueachievement"].ToString() != "")
                    dr["revenueachievement"] = Math.Round(Convert.ToDecimal(dr["revenueachievement"]));

                if (dr["actualprodrecord"].ToString() == "0")
                {
                    dr["productivity"] = 0;
                }
                else
                {

                    dr["productivity"] = Double.Parse(dr["actualprodrecord"].ToString()) / Double.Parse(dr["workedhrs"].ToString());
                    dr["productivity"] = Math.Round(Convert.ToDouble(dr["productivity"]), 0);
                }
            }

            decimal plannedhrs = 0;
            decimal plannedhrrecord = 0;
            decimal plannedprodrecord = 0;
            decimal workedhrs = 0;
            decimal actualprodrecord = 0;
            decimal targetrevenue = 0;
            decimal actualrevenue = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row[7].ToString() != "")
                {
                    plannedhrs += decimal.Parse(row[7].ToString());
                }
                plannedhrrecord += decimal.Parse(row[8].ToString());
                plannedprodrecord += decimal.Parse(row[9].ToString());
                if (row[10].ToString() != "")
                {
                    workedhrs += decimal.Parse(row[10].ToString());
                }

                actualprodrecord += decimal.Parse(row[11].ToString());
                targetrevenue += decimal.Parse(row[16].ToString());
                actualrevenue += decimal.Parse(row[17].ToString());
            }



            dt.Rows.Add("", "", "", "", "", "", "TOTALS", plannedhrs, plannedhrrecord, plannedprodrecord, workedhrs, actualprodrecord, (actualprodrecord / plannedprodrecord) * 100, "", "", "", targetrevenue, actualrevenue, (actualrevenue / targetrevenue) * 100);







            dt.Columns["psn"].ColumnName = "PSN";
            dt.Columns["associate"].ColumnName = "Associates Name";
            dt.Columns["process"].ColumnName = "Process";
            dt.Columns["project"].ColumnName = "Project";
            dt.Columns["projectcode"].ColumnName = "Project Code";
            dt.Columns["eventcode"].ColumnName = "Event code";
            dt.Columns["tlname"].ColumnName = "TL's Name";
            dt.Columns["plannedhrs"].ColumnName = "Hours planned";
            dt.Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
            dt.Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
            dt.Columns["workedhrs"].ColumnName = "Hours worked";
            dt.Columns["actualprodrecord"].ColumnName = "Actual Production Records";
            dt.Columns["achievement"].ColumnName = "% Achievement";
            dt.Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
            dt.Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
            dt.Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
            dt.Columns["productivity"].ColumnName = "Productivity(per hr)";

            dt.AcceptChanges();


            DataGrd.DataSource = dt;
            DataGrd.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + dt.Rows[0]["location"].ToString() + ".xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            DataGrd.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            return View("ProductionuploadIndex");



        }



        public ActionResult UserList()
        {

            User Model = new User();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT muser.Id ,muser.Username,muser.password,muser.Firstname,muser.LastName, muser.EmailId,muser.Mobile,muser.IsActive,muser.status,muser.RoleId,mrole.role FROM muser,mrole where muser.Roleid=mrole.Id  and status=0";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.UserList = dtt.DataTableToList<User>();
                return View("UserList", Model);
            }
        }
        public ActionResult UserForm()
        {

            User Model = new User();
            return PartialView("/Views/Admin/_UserForm.cshtml", Model);
        }

        public ActionResult GetUserPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            User Model = new User();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT * FROM muser where muser.Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.UserName = reader.GetString(1);
                    Model.FirstName = reader.GetString(3);
                    Model.LastName = reader.GetString(4);
                    Model.EmailId = reader.GetString(5);
                    Model.IsActive = Convert.ToBoolean(reader.GetInt32(10));
                    Model.PM = reader["PM"].ToString();
                    Model.RoleId =int.Parse(reader["RoleId"].ToString());


            if (reader["location"].ToString()== "TVM")
            {
                 Model.locationId = 1;
            }
            else if (reader["location"].ToString() == "KNPY")
            {
                Model.locationId = 2;
            }
            else if (reader["location"].ToString() == "MDS")
            {
               Model.locationId = 3;
            }
            else if (reader["location"].ToString() == "MQC")
            {
                Model.locationId = 4;
            }
            else if (reader["location"].ToString() == "MNS")
            {
                Model.locationId = 5;
            }

            else if (reader["location"].ToString() == "KAKKANAD")
            {
                Model.locationId = 6;
            }


                }

            }

            return PartialView("/Views/Admin/_UserForm.cshtml", Model);
        }
        public ActionResult DeleteUser(string ID)
        {
            try
            {
                DeleteUserDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("UserList");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("UserList");
            }
        }


        public ActionResult GetHolidayPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            HolidayModel Model = new HolidayModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT id,holidayname,holidaydate,location FROM `Holiday` where `Holiday`.Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.id = reader.GetInt32(0);
                    if (reader["holidayname"] != DBNull.Value)
                    {
                        Model.holidayname = reader.GetString(1);
                    }
                  
                    Model.holidaydate = reader.GetString(2);
                    Model.location = reader.GetString(3);

                }

            }

            return PartialView("/Views/Admin/_Holiday.cshtml", Model);
        }
        public ActionResult DeleteHoliday(string ID)
        {
            try
            {
                int Id = int.Parse(ID);
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = mConnection;
                    cmd.CommandText = "Delete from Holiday where id=" + Id;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("HolidayList");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("HolidayList");
            }
        }



        public ActionResult DeleteResourcePopup(string ID)
        {
            try
            {
                DeleteResourceDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("ProjectResourcePlanList");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("ProjectResourcePlanList");
            }
        }



        public void DeleteResourceDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = " delete from ResourcePlan where  ResourcePlan.Id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            //return Result;
        }











        public ActionResult DeleteRevPopup(string ID)
        {
            try
            {
                DeleteRevDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("GenerateRevenue");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("GenerateRevenue");
            }
        }


        public ActionResult DeleteDollar(string ID)
        {
            try
            {
                int Id = int.Parse(ID);
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = mConnection;
                    cmd.CommandText = "Delete from dollarsettings where id=" + Id;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }
                TempData["Msg"] = "Successfully Deleted!";
               
            }
            catch (Exception Ex)
            {
                TempData["Msg"] = "Unsuccessfull Operation!";
               
            }
            return RedirectToAction("DollarSettings");
        }


         

        public string DeleteRevDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = " delete from revenuereport where  revenuereport.Id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }


        public string DeleteUserDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "update muser set status=1 where muser.Id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }

        public ActionResult SaveHoliday(HolidayModel model)
        {
            try
            {
                int insertresult = 0;
                if (ModelState.IsValid)
                {
                    if (model.id == 0)
                    {
                        insertresult = CheckExistenceofHoliday(model);
                        if (insertresult == 0)
                        {
                            string Result = SaveHHoliday(model);
                            if (Result.Trim('"') == "Ok")
                                TempData["Msg"] = "Successfully Saved!";
                            else
                                TempData["Msg"] = "Unsuccessfull Operation!";
                        }
                        else
                        {
                            TempData["Msg"] = "Data  Exist!";
                        }
                    }
                    else
                    {
                        string Result = SaveHHoliday(model);
                        if (Result.Trim('"') == "Ok")
                            TempData["Msg"] = "Successfully Saved!";
                        else
                            TempData["Msg"] = "Unsuccessfull Operation!";
                    }



                }

                return RedirectToAction("HolidayList");
            }
            catch (Exception ex)
            {
                 return RedirectToAction("HolidayList");
            }
           

        }

      

        public string SaveHHoliday(HolidayModel model)
        {

           
            string Result = string.Empty;
            Result = "NotOk";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //DateTime dtdate = new DateTime();
            //dtdate = DateTime.ParseExact(model.holidaydate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            ////dtdate = DateTime.ParseExact(model.holidaydate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //var date = dtdate.ToString("yyyy-MM-dd");
            string date=string.Empty;
            string[] strArrDate = null;
            char[] splitcharDate = { '/' };
            strArrDate = model.holidaydate.Split(splitcharDate);

            if (strArrDate[0].Length == 1 && strArrDate[1].Length == 1)
                date = strArrDate[2] + "-" + "0" + strArrDate[1] + "-" + "0" + strArrDate[0];
            else if (strArrDate[0].Length == 1 && strArrDate[1].Length > 1)
                date = strArrDate[2] + "-"  + strArrDate[1] +  "-" + "0" + strArrDate[0];
            else if (strArrDate[0].Length > 1 && strArrDate[1].Length == 1)
                date = strArrDate[2] + "-" + "0" + strArrDate[1] + "-" + "0" + strArrDate[0];
            else if (strArrDate[0].Length > 1 && strArrDate[1].Length > 1)
                date = strArrDate[2] + "-" + strArrDate[1] + "-" + strArrDate[0];



            if (model.id == 0)
            {

               

                string Command = "INSERT INTO `Holiday`(`holidaydate`,`holidayname`,`location` ) VALUES ('" + date + "','" + model.holidayname + "','" + model.location + "');";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }
               
                       
                           

            }
            else
            {

                string Command = "UPDATE Holiday set `holidaydate`='" + date + "', `holidayname`='" + model.holidayname + "',`location`='" + model.location + "' where Holiday.id=" + model.id;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }

            }

            return Result;
        }





        public ActionResult SaveUser(User model)
        {
            int insertresult = 0;
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    insertresult = UserExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageUser(model);
                        if (Result.Trim('"') == "Ok")
                            TempData["Msg"] = "Successfully Saved!";
                        else
                            TempData["Msg"] = "Unsuccessfull Operation!";
                    }
                    else
                    {
                        TempData["Msg"] = "Data  Exist!";
                    }
                }
                else
                {
                    string Result = ManageUser(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }


            }
            return RedirectToAction("UserList");
        }

        public string        ManageUser(User model)
        {
            string Result = string.Empty;
            string location = string.Empty;
            Result = "NotOk";

            if (model.locationId == 1)
            {
                location = "TVM";
            }
            else if (model.locationId == 2)
            {
                location = "KNPY";
            }
            else if (model.locationId == 3)
            {
                location = "MDS";
            }
            else if (model.locationId == 4)
            {
                location = "MQC";
            }
            else if (model.locationId == 5)
            {
                location = "MNS";
            }

            else if (model.locationId == 6)
            {
                location = "KAKKANAD";
            }



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            if (model.Id == 0)
            {
                string Command = "INSERT INTO muser(`Id`,`UserName`, `Password`,`FirstName`,`LastName`,`EmailId`,IsActive,Status,RoleId,Location,PM ) VALUES (" + model.UserName + ",'" + model.UserName + "','" + model.Password + "','" + model.FirstName + "' ,'" + model.LastName + "', '" + model.EmailId + "'," + model.IsActive + ",0," + model.RoleId + ",'" + location + "','" + model.PM + "');";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }
            }
            else
            {
                int isActive;
                if (model.IsActive == true)
                {
                    isActive = 1;
                }
                else
                {
                    isActive = 0;
                }
                string Command = "UPDATE muser set `UserName`='" + model.UserName + "', `FirstName`='" + model.FirstName + "',`LastName`='" + model.LastName + "',`EmailId`='" + model.EmailId + "',location='" + location + "',IsActive=" + isActive + ",pm='" + model.PM + "',roleid=" + model.RoleId + " where muser.Id=" + model.Id;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                        Result = "Ok";
                    }

                }

            }

            return Result;
        }

        public ActionResult SaveReasonEntry(ReasonModel model)
        {
            string location = string.Empty;
            if (model.id == 0)
            {
                location = "KNPY";
            }
            else if (model.id == 1)
            {
                location = "TVM";
            }
            else if (model.id == 2)
            {
                location = "MDS";
            }
            else if (model.id == 3)
            {
                location = "MQC";
            }
            else if (model.id == 4)
            {
                location = "MNS";
            }

            else if (model.id == 5)
            {
                location = "KAKKANAD";
            }
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            string Command = "INSERT INTO productionupload(`date`, `Remarks`,`Location` ) VALUES ('" + model.date + "','" + model.Remarks + "', '" + location + "');";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                {
                    myCmd.ExecuteNonQuery();


                }
            }
            return View("ProductionuploadIndex");
        }

        public ActionResult ETOProjectwise()
        {
            Projectmodel model = new Projectmodel();

            return View("ETOCalculationProjectwise", model);
        }

        public ActionResult ETOLocationwise()
        {
            Projectmodel model = new Projectmodel();
            return View("ETOCalculationLocationwise", model);
        }

        public ActionResult ETOEmployee()
        {
            Projectmodel model = new Projectmodel();
            return View("ETOEmployeeDatewise", model);
        }



        





        public ActionResult ETOCalculation()
        {

            List<SelectListItem> ClientCodes = new List<SelectListItem>();
            List<SelectListItem> ProjectCodes = new List<SelectListItem>();
            List<SelectListItem> EventCodes = new List<SelectListItem>();
            List<SelectListItem> TLs = new List<SelectListItem>();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();
            Projectmodel model = new Projectmodel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            
            //using (MySqlConnection mConnection = new MySqlConnection(connString))
            //{
            //    string query = " SELECT distinct `project` from `productionreport2020` where year(date)=" + int.Parse(year) + " and month(date)=" + int.Parse(month) + "   and project is not null order by `project`";
            //    using (MySqlCommand cmd = new MySqlCommand(query))
            //    {
            //        cmd.Connection = mConnection;
            //        mConnection.Open();
            //        using (MySqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            while (sdr.Read())
            //            {
            //                ClientCodes.Add(new SelectListItem
            //                {
            //                    Text = sdr["project"].ToString(),
            //                    Value = sdr["project"].ToString()
            //                });
            //            }
            //        }
            //        // PeCodes.Add(new SelectListItem() { Value = "-1", Text = "ALL" });
            //        mConnection.Close();
            //    }
            //}





            //using (MySqlConnection mConnection = new MySqlConnection(connString))
            //{
            //    string query = " SELECT distinct `projectcode` from `productionreport2020` where year(date)=" + int.Parse(year) + " and month(date)=" + int.Parse(month) + " and projectcode is not null   order by `projectcode`";
            //    using (MySqlCommand cmd = new MySqlCommand(query))
            //    {
            //        cmd.Connection = mConnection;
            //        mConnection.Open();
            //        using (MySqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            while (sdr.Read())
            //            {
            //                ProjectCodes.Add(new SelectListItem
            //                {
            //                    Text = sdr["projectcode"].ToString(),
            //                    Value = sdr["projectcode"].ToString()
            //                });
            //            }
            //        }

            //        mConnection.Close();
            //    }
            //}

            //using (MySqlConnection mConnection = new MySqlConnection(connString))
            //{
            //    string query = " SELECT distinct `eventcode` from `productionreport2020` where year(date)=" + int.Parse(year) + " and month(date)=" + int.Parse(month) + " and eventcode is not null  order by `eventcode`";
            //    using (MySqlCommand cmd = new MySqlCommand(query))
            //    {
            //        cmd.Connection = mConnection;
            //        mConnection.Open();
            //        using (MySqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            while (sdr.Read())
            //            {
            //                EventCodes.Add(new SelectListItem
            //                {
            //                    Text = sdr["eventcode"].ToString(),
            //                    Value = sdr["eventcode"].ToString()
            //                });
            //            }
            //        }

            //        mConnection.Close();
            //    }
            //}

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `tlname` from `productionreport2020` where year(date)=" + int.Parse(year) + " and month(date)=" + int.Parse(month) + " and tlname is not null  order by tlname";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            TLs.Add(new SelectListItem
                            {
                                Text = sdr["tlname"].ToString(),
                                Value = sdr["tlname"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }

            //ViewBag.ProjectCodes = ProjectCodes;

            //ViewBag.EventCodes = EventCodes;
            //ViewBag.ClientCodes = ClientCodes;
            ViewBag.TLs = TLs;
            return View("ETOCalculation", model);
           
        }


        public ActionResult CheckStringWithOneColumn(string ActualValue)
        {
            string Result = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT muser.Username FROM muser where muser.Username='" + ActualValue + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    Result = "true";
                }
                else
                {
                    Result = "false";
                }
            }
            return new JsonResult { Data = Boolean.Parse(Result), ContentType = "Json", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevenueConfUpload(HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("UploadRevenueconfiguration");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        string TL = string.Empty;




                        if (Session["RoleId"].ToString() != "1")
                        {
                            if (TL != Session["UserName"].ToString())
                            {
                                ModelState.AddModelError("File", "Team Lead is not valid");
                                return View("UploadRevenueconfiguration");
                            }
                        }

                        //if (LocationDataInsert())
                        //{
                        //    ModelState.AddModelError("File", "Already uploaded the file");
                        //    return View("FileUploadIndex");
                        //}
                        //else
                        //{

                        InsertRevenueConfiguration(dataTable);
                        ModelState.AddModelError("File", "File Uploaded Successfully");
                        return View("UploadRevenueconfiguration");
                        // }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }
                return View("UploadRevenueconfiguration");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("File", ex.Message + ex.StackTrace + ex.Source);
                return View("UploadRevenueconfiguration");
            }
        }


        public void InsertRevenueConfiguration(DataTable dt)
        {
            string prj = string.Empty;
            string eventcod = string.Empty;
            try
            {



                double Index = 0;
                double QC2 = 0;
                double QC3 = 0;
                double Audit = 0;
                double UAT = 0;
                double Rework = 0;
                double Training = 0;
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                int year = int.Parse(ConfigurationManager.AppSettings["Year"]);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    Index = 0;
                    QC2 = 0;
                    QC3 = 0;
                    Audit = 0;
                    UAT = 0;
                    Rework = 0;
                    Training = 0;
                    string Projectcode = dt.Rows[i]["Projectcode"].ToString().Replace("'", "''");
                    string eventcode = dt.Rows[i]["eventcode"].ToString().Replace("'", "''");
                    prj = Projectcode;
                    eventcod = eventcode;
                    if (dt.Rows[i]["Indexing"].ToString() != "")
                    {
                        if (dt.Rows[i]["Indexing"].ToString() != " ")
                            Index = Convert.ToDouble(dt.Rows[i]["Indexing"]);
                    }

                    if (dt.Rows[i]["QC2"].ToString() != "")
                        QC2 = Convert.ToDouble(dt.Rows[i]["QC2"]);

                    if (dt.Rows[i]["QC3"].ToString() != "")
                        QC3 = Convert.ToDouble(dt.Rows[i]["QC3"]);

                    if (dt.Rows[i]["Audit"].ToString() != "")
                        Audit = Convert.ToDouble(dt.Rows[i]["Audit"]);

                    if (dt.Rows[i]["UAT"].ToString() != "")
                        UAT = Convert.ToDouble(dt.Rows[i]["UAT"]);

                    if (dt.Rows[i]["Training"].ToString() != "")
                        Training = Convert.ToDouble(dt.Rows[i]["Training"]);

                    if (dt.Rows[i]["Rework"].ToString() != "")
                        Rework = Convert.ToDouble(dt.Rows[i]["Rework"]);


                    string existcommand = "SELECT EXISTS(SELECT * FROM `revenueconfiguration` WHERE Projectcode='" + Projectcode + "' and Eventcode='" + eventcode + "') as exist";

                    int col1Value = 0;
                    using (MySqlConnection mConnection = new MySqlConnection(connString))
                    {
                        MySqlCommand cmd = new MySqlCommand(existcommand, mConnection);
                        mConnection.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                col1Value = int.Parse(reader[0].ToString());


                            }
                        }
                    }
                    if (col1Value == 0)
                    {
                        string Command = "INSERT INTO `revenueconfiguration`(`Projectcode`, `Eventcode`,Indexing,QC2,QC3,UAT,Rework,Audit,Training ) VALUES ('" + Projectcode + "','" + eventcode + "', " + Index + ", " + QC2 + "," + QC3 + " ," + UAT + "," + Rework + "," + Audit + "," + Training + ");";
                        using (MySqlConnection mConnection = new MySqlConnection(connString))
                        {
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                            {
                                myCmd.ExecuteNonQuery();


                            }
                        }

                    }

                    else
                    {

                        string Command = "Update `revenueconfiguration` set Indexing= " + Index + ",QC2=" + QC2 + ",QC3=" + QC3 + ",UAT=" + UAT + ",Rework=" + Rework + ",Audit=" + Audit + ",Training=" + Training + "  Where  `Projectcode`='" + Projectcode + "' and `Eventcode`= '" + eventcode + "';";
                        using (MySqlConnection mConnection = new MySqlConnection(connString))
                        {
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                            {
                                myCmd.ExecuteNonQuery();


                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", prj + eventcod);
            }

        }

        public ActionResult OpenCpyConfig()
        {

            OpenCpyconfigModel Model = new OpenCpyconfigModel();
            return PartialView("/Views/Admin/_OpenCpyConfig.cshtml");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectConfUpload(HttpPostedFileBase upload, string month, int location)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("UploadProjectConfiguration");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        string TL = string.Empty;




                        if (Session["RoleId"].ToString() != "1")
                        {
                            if (TL != Session["UserName"].ToString())
                            {
                                ModelState.AddModelError("File", "Team Lead is not valid");
                                return View("UploadProjectConfiguration");
                            }
                        }

                        //if (LocationDataInsert())
                        //{
                        //    ModelState.AddModelError("File", "Already uploaded the file");
                        //    return View("FileUploadIndex");
                        //}
                        //else
                        //{

                        InsertProjectConfiguration(dataTable, month, location);
                        ModelState.AddModelError("File", "File Uploaded Successfully");
                        return View("UploadProjectConfiguration");
                        // }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }
                return View("UploadProjectConfiguration");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("UploadProjectConfiguration");
            }
        }


        

        [HttpPost]
        public ActionResult GetClientcode(string startdate, string enddate)
        {
            string[] startArr = null;
            string[] endArr = null;
            char[] splitchar = { '/' };
            if (startdate != "")
            {
                startArr = startdate.Split(splitchar);
                if (startArr.Length > 0)
                    startdate = startArr[2] + "-" + startArr[1] + "-" + startArr[0];
            }

            if (enddate != "")
            {
                endArr = enddate.Split(splitchar);
                if (endArr.Length > 0)
                    enddate = endArr[2] + "-" + endArr[1] + "-" + endArr[0];
            }
            string query = "select distinct `Project`  from productionreport2020 where   project is not null and date>='" + startdate + "'";

            if (enddate != "")
            {
                query = query + " and date <='" + enddate + "'";
            }
            List<Client> objclient = new List<Client>();
           DataTable dt=new DataTable();
           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
           using (MySqlConnection con = new MySqlConnection(constr))
           {
               using (MySqlCommand cmd = new MySqlCommand(query, con))
               {

                   using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                   {

                       sda.Fill(dt);
                     

                   }
               }
           }


          

           objclient = dt.DataTableToList<Client>();
           SelectList obgclient = new SelectList(objclient, "project", "project", 0);
           return Json(obgclient);



        }


        [HttpPost]
        public ActionResult FillprojectbyClient(string Clientcode,string startdate, string enddate)
        {

            string[] startArr = null;
            string[] endArr = null;
            char[] splitchar = { '/' };
            startArr = startdate.Split(splitchar);
            if (startArr.Length > 0)
                startdate = startArr[2] + "-" + startArr[1] + "-" + startArr[0];


            if (enddate != "")
            {
                endArr = enddate.Split(splitchar);
                if (endArr.Length > 0)
                    enddate = endArr[2] + "-" + endArr[1] + "-" + endArr[0];
            }
            string query = string.Empty;
             query = "select distinct `Projectcode`  from productionreport2020  where   projectcode is not null";
            if(Clientcode!="ALL")

                query = query +  " and `Project`  ='" + Clientcode + "'";

            query = query + " and date>='" + startdate + "'";

            if (enddate != "")
            {
                query = query + " and date <='" + enddate + "'";
            }
            List<Project> objclient = new List<Project>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }
          
            objclient = dt.DataTableToList<Project>();
            SelectList obgclient = new SelectList(objclient, "projectcode", "projectcode", 0);
            return Json(obgclient);



        }



        [HttpPost]
        public ActionResult BindTL(string LocationId)
        {
            string query = string.Empty;
           

            if (LocationId == "KAKKANAD")
                LocationId = "KKND";
            if (LocationId == "ALL")
            {

                query = "select distinct `tlname`  from productionreport2020 where tlname is not null and tlname<>' '  order by `tlname`";
            }
            else
            {
                query = "select distinct `tlname`  from productionreport2020 where tlname is not null and tlname<>' '  and location='" + LocationId + "' order by `tlname`";
            }

            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);
        }



        [HttpPost]
        public ActionResult BindNamePSNResource(string TL)
        {
            string query = string.Empty;



            query = "select distinct `psn`,`associate`  from productionreport2020 where  associate is not null and associate<>' '";
           

            if (TL != "ALL")
            {
                query = query + " and tlname='" + TL + "'";
            }
            query = query + "  order by associate";
            List<Resource> objresource = new List<Resource>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objresource = dt.DataTableToList<Resource>();
            SelectList obgresource = new SelectList(objresource, "psn", "associate", 0);
            return Json(obgresource);
        }









        [HttpPost]
        public ActionResult BindResource(string Location,string TL)
        {
            string query = string.Empty;

            if (Location == null)
                Location = "ALL";
            if (TL == null)
                TL = "ALL";

            query = "select distinct `psn`  from productionreport2020 where projectcode is not null";
            if (Location != "ALL")
            {
                 query =query + " and location='" + Location + "'";
            }

             if (TL != "ALL")
            {
                query = query + " and TL='" + TL + "'";
            }

             List<Resource> objresource = new List<Resource>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objresource = dt.DataTableToList<Resource>();
            SelectList obgresource = new SelectList(objresource, "psn", "psn", 0);
            return Json(obgresource);
        }








         [HttpPost]
        public ActionResult Bindemployee(string Location, string fromdate, string enddate)
        {

            string query = string.Empty;
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endddate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtsdate = new DateTime();
            DateTime dtedate = new DateTime();
            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }
            if (fromdate != null && fromdate != "")
            {
                dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtsdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endddate = dtedate.ToString("yyyy-MM-dd");
            }


            query = "select distinct `psn`,`associate`  from productionreport2020 where  associate is not null and associate<>' '";


           
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }
            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }
            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endddate + "'";
            }

            query = query + "  order by associate";
            List<Resource> objresource = new List<Resource>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objresource = dt.DataTableToList<Resource>();
            SelectList obgresource = new SelectList(objresource, "psn", "associate", 0);
            return Json(obgresource);

            
        }



         [HttpPost]
         public ActionResult BindETOClient()
        {

           
            string currentMonth = DateTime.Now.Month.ToString();
            string currentYear = DateTime.Now.Year.ToString();
            string query = string.Empty;

         
            query = "select distinct project  from productionreport2020 where  month(date)=" + currentMonth + " and year(date)=" + currentYear + " and project is not null ;";

            List<Client> objclient = new List<Client>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objclient = dt.DataTableToList<Client>();
            SelectList obgclient = new SelectList(objclient, "project", "project", 0);
            return Json(obgclient);
        }


         [HttpPost]
         public ActionResult BindETOproject()
         {


             string currentMonth = DateTime.Now.Month.ToString();
             string currentYear = DateTime.Now.Year.ToString();
             string query = string.Empty;


             query = "select distinct projectcode  from productionreport2020 where  month(date)=" + currentMonth + " and year(date)=" + currentYear + " and project is not null ;";

             List<Project> objclient = new List<Project>();
             DataTable dt = new DataTable();
             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }


             objclient = dt.DataTableToList<Project>();
             SelectList obgclient = new SelectList(objclient, "Id", "projectcode", 0);
             return Json(obgclient);
         }
        








        [HttpPost]
        public ActionResult BindProductivityProject(string Clientcode)
        {
            List<Project> objproject = new List<Project>();
            DataTable dt = new DataTable();
            string query = string.Empty;
            if (Clientcode=="ALL")
                query = "select distinct projectcode   from productionreport2020 where projectcode is not null and projectcode<>' '";
            else
                query = "select distinct projectcode   from productionreport2020 where projectcode is not null and projectcode<>' ' and project='" + Clientcode + "'";
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }

            objproject = dt.DataTableToList<Project>();
            SelectList obgproject = new SelectList(objproject, "Id", "Projectcode", 0);
            return Json(obgproject);
        }


        







       [HttpPost]
        public ActionResult Bindproject(string date, string LocationId)
        {
            string[] strArr = null;
            if (LocationId == "KAKKANAD")
                   LocationId = "KKND";
            if (date != null)
            {
                
                char[] splitchar = { '/' };
                strArr = date.Split(splitchar);
                if (strArr.Length > 0)
                    date = strArr[1] + "." + strArr[0] + "." + strArr[2];
            }

            string monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(strArr[1]));
           string query=string.Empty;
           if (LocationId=="All")

            query = "select distinct CONCAT(projectcode) as projectcode  from productionreport2020 where project is not null and  MONTHNAME(date)='" + monthname + "' and year(date)=" + strArr[2] + ";";
           else
               query = "select distinct CONCAT(projectcode) as projectcode  from productionreport2020 where project is not null and location='" + LocationId + "' and MONTHNAME(date)='" + monthname + "' and year(date)=" + strArr[2] + ";";
           
           List<Project> objproject = new List<Project>();
           DataTable dt=new DataTable();
           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
           using (MySqlConnection con = new MySqlConnection(constr))
           {
               using (MySqlCommand cmd = new MySqlCommand(query, con))
               {

                   using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                   {

                       sda.Fill(dt);
                     

                   }
               }
           }


           objproject = dt.DataTableToList<Project>();
           SelectList obgproject = new SelectList(objproject, "Id", "Projectcode", 0);
           return Json(obgproject);
        }


       [HttpPost]
       public ActionResult BindEvent(string ProjectId, string fromdate, string Clientcode)
       {

           var date = DateTime.Now.ToString("yyyy-MM-dd"); 
           string query = string.Empty;
           DateTime dtdate = new DateTime();
          
         
           query = "select distinct eventcode from productionreport2020 where eventcode is not null and eventcode<>' ' ";
           if (ProjectId != "ALL")
           {
               query =query + " and projectcode='" + ProjectId + "'";
           }
           if (fromdate != null && fromdate != "")
           {
               dtdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               date = dtdate.ToString("yyyy-MM-dd");
               query = query + " and productionreport2020.date='" + date + "'";
           }

            if (Clientcode!="ALL")
           {
              query =query + " and project='" + Clientcode + "'" ;
           }

           
           
           
          

           List<Event> objevent = new List<Event>();
           DataTable dt = new DataTable();
           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
           using (MySqlConnection con = new MySqlConnection(constr))
           {
               using (MySqlCommand cmd = new MySqlCommand(query, con))
               {

                   using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                   {

                       sda.Fill(dt);


                   }
               }
           }


           objevent = dt.DataTableToList<Event>();
           SelectList obgevent = new SelectList(objevent, "eventcode", "eventcode", 0);
           return Json(obgevent);
       }


       [HttpPost]
       public ActionResult BindClientcode()
       {

         
           string query = string.Empty;
          


           query = "select distinct project from productionreport2020 where project is not null and project<>' ' ";







           List<Client> objevent = new List<Client>();
           DataTable dt = new DataTable();
           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
           using (MySqlConnection con = new MySqlConnection(constr))
           {
               using (MySqlCommand cmd = new MySqlCommand(query, con))
               {

                   using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                   {

                       sda.Fill(dt);


                   }
               }
           }


           objevent = dt.DataTableToList<Client>();
           SelectList obgevent = new SelectList(objevent, "project", "project", 0);
           return Json(obgevent);
       }



        
        
        
       [HttpPost]
       public ActionResult BindProjRevEvent(string ClientId,string ProjectId,string fromdate,string enddate,string Location )
       {

            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             string query = string.Empty;
             DateTime dtstartdate = new DateTime();


             var findate = DateTime.Now.ToString("yyyy-MM-dd");
            
             DateTime dtfindate = new DateTime();

             if(Location=="KAKKANAD")
                 Location="KKND";
             query = "select distinct eventcode from productionreport2020 where eventcode is not null and eventcode<>' ' ";

             if (ClientId != "ALL")
             {
                 query = query + " and project='" + ClientId + "'";
             }

             if (ProjectId != "ALL")
             {
                 query = query + " and projectcode='" + ProjectId + "'";
             }
             if (Location != "ALL")
             {
                 query = query + " and location='" + Location + "'";
             }

            
             if (fromdate != null && fromdate != "")
             {
                 dtstartdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtstartdate.ToString("yyyy-MM-dd");
                 query = query + " and productionreport2020.date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 dtfindate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 findate = dtfindate.ToString("yyyy-MM-dd");
                 query = query + " and productionreport2020.date<='" + findate + "'";
             }


           





             List<Event> objevent = new List<Event>();
             DataTable dt = new DataTable();
             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }


             objevent = dt.DataTableToList<Event>();
             SelectList obgevent = new SelectList(objevent, "eventcode", "eventcode", 0);
             return Json(obgevent);
         }
     
        
        
        
        [HttpPost]
       public ActionResult BindMonthEvent(string ProjectId, string Month, string Clientcode,string Year,string Location)
       {

           if (Location == "KAKKANAD")
               Location = "KKND";
            
            
            string  query = "select distinct eventcode from productionreport2020  where eventcode is not null and eventcode<>''";

           if (ProjectId != "ALL" )  
           {
               query=query +" and projectcode='" + ProjectId  + "'";
           }
           if (Clientcode != "ALL" && Clientcode != null )
           {
              query=  query +" and project='" + Clientcode  + "'";
           }
           if (Location != "ALL")
           {
               query = query + " and location='" + Location + "'";
           }

           if (Month!="Select")
           {
            query=  query +"  and    monthname(productionreport2020.`date`)='" + Month + "'";
           }
            
           if (Year !="Select")
           {
                query=  query +"  and    year(productionreport2020.`date`)=" + Year + "";
           }
                
        
           List<Event> objevent = new List<Event>();
           DataTable dt = new DataTable();
           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
           using (MySqlConnection con = new MySqlConnection(constr))
           {
               using (MySqlCommand cmd = new MySqlCommand(query, con))
               {

                   using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                   {

                       sda.Fill(dt);


                   }
               }
           }


           objevent = dt.DataTableToList<Event>();
           SelectList obgevent = new SelectList(objevent, "eventcode", "eventcode", 0);
           return Json(obgevent);
       }

        [HttpPost]
        public ActionResult BindYearEvent(string ProjectId, string Clientcode, string Year)
        {
            string mmonthname = string.Empty;
           
            string query = string.Empty;
            

            query = "select distinct eventcode from productionreport2020 where eventcode is not null and eventcode<>''";

            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'"; 
            }
            if (ProjectId != "ALL")
            {
                query = query + " and Projectcode='" + ProjectId + "'";
            }
            if (Year != "Select")
            {
                string[] strArr = null;
                char[] splitchar = { '-' };
                strArr = Year.Split(splitchar);
               query = query + "and  year(productionreport2020.`date`)=" + strArr[0] + "";
            }
            

            List<Event> objevent = new List<Event>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objevent = dt.DataTableToList<Event>();
            SelectList obgevent = new SelectList(objevent, "eventcode", "eventcode", 0);
            return Json(obgevent);
        }






      








       [HttpPost]
       public ActionResult BindMonthProjectcode(string Location,string Clientcode, string Month,string Year)
       {

           string mmonthname = string.Empty;
           string query = string.Empty;

           if (Location == "KAKKANAD")
               Location = "KKND";

           query = "select distinct projectcode  from productionreport2020  where projectcode is not null and projectcode<>' '";
          
           if (Clientcode != "ALL")
           {
               query = query + " and project='" + Clientcode + "'";
           }

           if (Location != "ALL")
           {
               query = query + " and location='" + Location + "'";
           }

          if (Month!="Select")
           {
               query=query + " and monthname(productionreport2020.`date`)='" + Month + "'" ;
           }

           if (Year!="Select")
           {

               query = query + " and year(productionreport2020.`date`)='" + Year + "'";
           }


           List<Project> objproject = new List<Project>();
           DataTable dt = new DataTable();
           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
           using (MySqlConnection con = new MySqlConnection(constr))
           {
               using (MySqlCommand cmd = new MySqlCommand(query, con))
               {

                   using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                   {

                       sda.Fill(dt);


                   }
               }
           }


           objproject = dt.DataTableToList<Project>();
           SelectList obgproject = new SelectList(objproject, "projectcode", "projectcode", 0);
           return Json(obgproject);
       }


       [HttpPost]
       public ActionResult BindYearProjectcode(string Clientcode, string Year)
       {

           string mmonthname = string.Empty;
           string query = string.Empty;
           string[] strArr = null;
           if (Year != "Select")
           {
               
               char[] splitchar = { '-' };
               strArr = Year.Split(splitchar);
           }

            query = "select distinct projectcode  from productionreport2020  where projectcode is not null and projectcode<>' '";

            if (Clientcode != "ALL")
            {
                query = query + "and project='" + Clientcode + "'";
            }
            if (Year != "Select")
            {
                query = query + "and year(date)='" + strArr[0] + "'";
            }

           List<Project> objproject = new List<Project>();
           DataTable dt = new DataTable();
           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
           using (MySqlConnection con = new MySqlConnection(constr))
           {
               using (MySqlCommand cmd = new MySqlCommand(query, con))
               {

                   using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                   {

                       sda.Fill(dt);


                   }
               }
           }


           objproject = dt.DataTableToList<Project>();
           SelectList obgproject = new SelectList(objproject, "projectcode", "projectcode", 0);
           return Json(obgproject);
       }





        public void InsertProjectConfiguration(DataTable dt, string monthid, int locationid)
        {


            string location = string.Empty;



            if (locationid == 1)
            {
                location = "TVM";
            }
            else if (locationid == 2)
            {
                location = "KNPY";
            }
            else if (locationid == 3)
            {
                location = "MDS";
            }
            else if (locationid == 4)
            {
                location = "MQC";
            }
            else if (locationid == 5)
            {
                location = "MNS";
            }
            else if (locationid == 6)
            {
                location = "KAKKANAD";
            }

            string monthname = string.Empty;
            if (monthid == "1")
            {
                monthname = "January";
            }
            else if (monthid == "2")
            {
                monthname = "February";
            }
            else if (monthid == "3")
            {
                monthname = "March";
            }
            else if (monthid == "4")
            {
                monthname = "April";
            }
            else if (monthid == "5")
            {
                monthname = "May";
            }
            else if (monthid == "6")
            {
                monthname = "June";
            }
            else if (monthid == "7")
            {
                monthname = "July";
            }
            else if (monthid == "8")
            {
                monthname = "August";
            }
            else if (monthid == "9")
            {
                monthname = "September";
            }
            else if (monthid == "10")
            {
                monthname = "October";
            }
            else if (monthid == "11")
            {
                monthname = "November";
            }
            else if (monthid == "12")
            {
                monthname = "December";
            }
            double processed = 0;
            List<string> Process = new List<string>();
            Process.Add("Indexing");
            Process.Add("QC2");
            Process.Add("QC3");
            Process.Add("Audit");
            Process.Add("UAT");
            Process.Add("Training");
            Process.Add("Rework");
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            int year = int.Parse(ConfigurationManager.AppSettings["Year"]);
            string esproject = string.Empty;
            string esevent = string.Empty;
            string Billingmode = string.Empty;
            double priceperunit = 0.0;
            double percent = 0.0;
            string currency = string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                esproject = dt.Rows[i]["Projectcode"].ToString().Replace("'", "''");
                esevent = dt.Rows[i]["Eventcode"].ToString().Replace("'", "''");
                Billingmode = dt.Rows[i]["Billingmode"].ToString().Replace("'", "''");
                priceperunit =double.Parse(dt.Rows[i]["priceperunit"].ToString());
                percent = double.Parse(dt.Rows[i]["percent"].ToString());
                currency = dt.Rows[i]["percent"].ToString();
                for (int k = 0; k < Process.Count; k++)
                {
                    processed = 0;
                    string Projectcode = dt.Rows[i]["Projectcode"].ToString();
                    string eventcode = dt.Rows[i]["eventcode"].ToString();

                    if (dt.Rows[i][Process[k]].ToString() != "")
                        processed = Convert.ToDouble(dt.Rows[i][Process[k].ToString()]);
                    string process = Process[k].ToString();
                    string existcommand = "SELECT EXISTS(SELECT * FROM projectconfiguration WHERE Projectcode='" + esproject + "' and Eventcode='" + esevent + "' and Process= '" + process + "' and location= '" + location + "' and monthname='" + monthname + "' and year=" + year + ") as exist";

                    int col1Value = 0;
                    using (MySqlConnection mConnection = new MySqlConnection(connString))
                    {
                        MySqlCommand cmd = new MySqlCommand(existcommand, mConnection);
                        mConnection.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                col1Value = int.Parse(reader[0].ToString());


                            }
                        }
                    }
                    if (col1Value == 0)
                    {
                        string Command = "INSERT INTO `projectconfiguration`(`Projectcode`, `Eventcode`,`Process`,`ProductionPlannedHr`,`location`,`month`,`monthname`,`locationId`,`year`,billingmode,priceperunit,percentage,currency ) VALUES ('" + esproject + "','" + esevent + "', '" + process + "', " + processed + ",'" + location + "' ," + monthid + ",'" + monthname + "'," + locationid + "," + year + ",'" + Billingmode + "'," + priceperunit + "," + percent + "," + currency + ");";
                        using (MySqlConnection mConnection = new MySqlConnection(connString))
                        {
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                            {
                                myCmd.ExecuteNonQuery();


                            }
                        }


                    }

                    else
                    {
                        string Command = "UPDATE `projectconfiguration` set ProductionPlannedHr=" + processed + "  where  Projectcode='" + esproject + "' and Eventcode='" + esevent + "' and Process= '" + process + "' and  location='" + location + "' and month=" + monthid + " and year=" + year + " and id<>0";
                        using (MySqlConnection mConnection = new MySqlConnection(connString))
                        {
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                            {
                                myCmd.ExecuteNonQuery();


                            }
                        }

                    }


                }
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserUpload(HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("Uploaduserdetails");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        string TL = string.Empty;




                        if (Session["RoleId"].ToString() != "1")
                        {
                            if (TL != Session["UserName"].ToString())
                            {
                                ModelState.AddModelError("File", "Team Lead is not valid");
                                return View("Uploaduserdetails");
                            }
                        }

                        //if (LocationDataInsert())
                        //{
                        //    ModelState.AddModelError("File", "Already uploaded the file");
                        //    return View("FileUploadIndex");
                        //}
                        //else
                        //{

                        InsertUserDataTable(dataTable);
                        ModelState.AddModelError("File", "File Uploaded Successfully");
                        return View("Uploaduserdetails");
                        // }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }
                return View("Uploaduserdetails");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("Uploaduserdetails");
            }
        }


        public bool ExistUser()
        {
            return true;
        }



        public void InsertUserDataTable(DataTable dt)
        {


            //if (!ExistUser()) ;
            //{
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            // string Command = "INSERT INTO `memployee` (`PSN`, `AssociateName`,`Location`,`TL`,`DOJ`,`Experience`) VALUES (@psn,@associatename,@location,@tl,@Doj,@Experience);";

            //string Command = "INSERT INTO `memployee` (`PSN`, `AssociateName`,`Location`,`TL`,`DOJ`,`Experience`) VALUES (@psn,@associatename,@location,@tl,@Doj,@Experience) WHERE NOT EXISTS ( SELECT * FROM memployee  WHERE `PSN` =@psn)";

            string Command = "INSERT INTO memployee (`PSN`, `AssociateName`,`Location`,`TL`,`DOJ`,`Experience`) SELECT * FROM (SELECT @psn,@associatename,@location,@tl,@Doj,@Experience) AS tmp WHERE NOT EXISTS (SELECT PSN FROM memployee WHERE PSN = @psn) LIMIT 1";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                using (MySqlTransaction trans = mConnection.BeginTransaction())
                {
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection, trans))
                    {
                        myCmd.CommandType = CommandType.Text;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            myCmd.Parameters.Clear();

                            if (dt.Columns.Contains("PSN"))
                            {
                                myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN"]);
                            }

                            if (dt.Columns.Contains("Assocaite Name"))
                            {
                                myCmd.Parameters.AddWithValue("@associatename", dt.Rows[i]["Assocaite Name"]);
                            }

                            if (dt.Columns.Contains("Location"))
                            {
                                myCmd.Parameters.AddWithValue("@location", dt.Rows[i]["Location"]);
                            }
                            if (dt.Columns.Contains("TL Name"))
                            {
                                myCmd.Parameters.AddWithValue("@tl", dt.Rows[i]["TL Name"]);
                            }

                            myCmd.Parameters.AddWithValue("@Doj", dt.Rows[i]["DOJ"]);
                            myCmd.Parameters.AddWithValue("@Experience", dt.Rows[i]["Experience"]);
                            myCmd.ExecuteNonQuery();

                        }
                        trans.Commit();
                    }
                }
            }
            //}
        }






        public ActionResult Fileupload()
        {
            return View("FileUpload");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload, string dateFrom)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("FileUploadIndex");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;
                        if (dataTable.Rows.Count > 0)
                        {
                            if (dataTable.Rows[0]["Date"].GetType() == typeof(DateTime))
                            {
                                sheetdate = Convert.ToDateTime(dataTable.Rows[0]["Date"]);
                            }
                            else
                            {
                                sheetdate = DateTime.FromOADate(Convert.ToDouble(dataTable.Rows[0]["Date"]));
                            }
                            if (dataTable.Columns.Contains("TL"))
                            {
                                TL = dataTable.Rows[0]["TL"].ToString();
                            }
                            else
                            {
                                TL = dataTable.Rows[0]["RO"].ToString();
                            }
                        }

                        string Exceldate = String.Format("{0:MM/dd/yyyy}", sheetdate).Replace("-", "/");

                        if (dateFrom != Exceldate)
                        {
                            ModelState.AddModelError("File", "Date is not valid");
                            return View("FileUploadIndex");
                        }

                        if (Session["RoleId"].ToString() != "1")
                        {
                            if (TL != Session["UserName"].ToString())
                            {
                                ModelState.AddModelError("File", "Team Lead is not valid");
                                return View("FileUploadIndex");
                            }
                        }

                        //if (LocationDataInsert())
                        //{
                        //    ModelState.AddModelError("File", "Already uploaded the file");
                        //    return View("FileUploadIndex");
                        //}
                        //else
                        //{

                        InsertDataTable(dataTable);
                        ModelState.AddModelError("File", "File Uploaded Successfully");
                        return View("FileUploadIndex");
                        // }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }
                return View("FileUploadIndex");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("FileUploadIndex");
            }
        }





        public void InsertDataTable(DataTable dt)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "INSERT INTO attendancereport (date, psn,name,tl,Attendance,Project,Totaltarget,Totalproduction,Totalhours,Charactercount ) VALUES (@date,@psn,@name,@TL,@Attendance,@Project,@Totaltarget,@Totalproduction,@Totalhours,@charactercount);";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                using (MySqlTransaction trans = mConnection.BeginTransaction())
                {
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection, trans))
                    {
                        myCmd.CommandType = CommandType.Text;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //if (i == 18)
                            //{
                            //    string tt = "11";
                            //}
                            if (dt.Rows[i]["Date"] != null)
                            {
                                myCmd.Parameters.Clear();
                                if (dt.Rows[i]["Date"].GetType() == typeof(DateTime))
                                {
                                    myCmd.Parameters.AddWithValue("@date", Convert.ToDateTime(dt.Rows[i]["Date"]));
                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@date", DateTime.FromOADate(Convert.ToDouble(dt.Rows[i]["Date"])));
                                }


                                if (dt.Columns.Contains("PSN NO"))
                                {
                                    myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN NO"]);
                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["psn"]);
                                }


                                myCmd.Parameters.AddWithValue("@name", dt.Rows[i]["Name"]);
                                if (dt.Columns.Contains("TL"))
                                {
                                    myCmd.Parameters.AddWithValue("@TL", dt.Rows[i]["TL"]);
                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@TL", dt.Rows[i]["RO"]);
                                }
                                myCmd.Parameters.AddWithValue("@Attendance", dt.Rows[i]["Attendance"]);
                                myCmd.Parameters.AddWithValue("@Project", dt.Rows[i]["Project"]);


                                if (dt.Rows[i]["Attendance"].ToString() == "R" || dt.Rows[i]["Attendance"].ToString() == "L")
                                {
                                    myCmd.Parameters.AddWithValue("@Totalproduction", 0);
                                    myCmd.Parameters.AddWithValue("@Totalhours", 0);
                                    myCmd.Parameters.AddWithValue("@charactercount", 0);
                                    myCmd.Parameters.AddWithValue("@Totaltarget", 0);

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@Totalproduction", dt.Rows[i]["Total Production"]);
                                    myCmd.Parameters.AddWithValue("@Totalhours", dt.Rows[i]["Total Hours"]);
                                    myCmd.Parameters.AddWithValue("@charactercount", dt.Rows[i]["Charater Count"]);
                                    myCmd.Parameters.AddWithValue("@Totaltarget", dt.Rows[i]["Total Target"]);

                                }

                                myCmd.ExecuteNonQuery();
                            }
                        }
                        trans.Commit();
                    }
                }
            }
        }

        public ActionResult DailyTeamleadview()
        {
            DailyTeamViewModel model = new DailyTeamViewModel();

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Id, CONCAT(muser.FirstName,' ',muser.LastName) as FirstName  FROM muser where roleid=2";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(new DataTable());
                adapter.Fill(dataSet.Tables[0]);
                DataTable dtt = dataSet.Tables[0];
                model.UserList = dtt.DataTableToList<User>();
                model.Id = int.Parse(Session["UserId"].ToString());
                ViewBag.hdnFlag = Session["DisplayName"];
            }

            return View("DailyTeamLeadViewIndex", model);
        }

        public ActionResult FileuploadIndex()
        {
            return View("FileUploadIndex");
        }


        public ActionResult DailyPProductionReport()
        {

            return View("DailymasterProductionReportIndex");
        }

        public ActionResult PeriodicProductionReportIndex()
        {
            return View("PeriodicProductionReportIndex");
        }

        public ActionResult ConsolidatedProductionReport()
        {

            return View("ConsolidatedProductionReport");
        }



        public ActionResult PeriodicProductionReportLocationwise(string fromdate, string todate, string LocationId)
        {
            DailymasterProductionViewModel model = new DailymasterProductionViewModel();
            string[] strArr = null;
            char[] splitchar = { '/' };
            strArr = fromdate.Split(splitchar);
            if (strArr.Length > 0)
                fromdate = strArr[2] + "-" + strArr[1] + "-" + strArr[0];

            string[] strArrTo = null;
            char[] splitcharTo = { '/' };
            strArrTo = todate.Split(splitcharTo);
            if (strArrTo.Length > 0)
                todate = strArrTo[2] + "-" + strArrTo[1] + "-" + strArrTo[0];


            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetPeriodicProduction", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@fromdate", fromdate);
                    cmd.Parameters.AddWithValue("@todate", todate);
                    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        DataTable dtt = ds.Tables[0];
                        model.LstDailymasterProductionReport = dtt.DataTableToList<DailymasterProductionViewModel>();



                    }
                }
            }
            return PartialView("PeriodicProductionReportList", model);




        }

        //public ActionResult PeriodicProductionReport(string fromdate, string todate, string UserId, string TypeId, string GraphType)
        //{
        //    PeriodicProductionViewModel model = new PeriodicProductionViewModel();


        //    string[] strArr = null;
        //    char[] splitchar = { '/' };
        //    strArr = fromdate.Split(splitchar);
        //    if (strArr.Length > 0)
        //        fromdate = strArr[2] + "-" + strArr[1] + "-" + strArr[0];

        //    string[] strArrTo = null;
        //    char[] splitcharTo = { '/' };
        //    strArrTo = todate.Split(splitcharTo);
        //    if (strArrTo.Length > 0)
        //        todate = strArrTo[2] + "-" + strArrTo[1] + "-" + strArrTo[0];
        //    TempData["fromdate"] = fromdate;
        //    TempData["todate"] = todate;
        //    TempData["userId"] = UserId;
        //    TempData["GraphType"] = GraphType;

        //    if (TypeId == "Tabular")
        //    {
        //        string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
        //        using (MySqlConnection con = new MySqlConnection(constr))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand("GetProductionPeriodical", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.AddWithValue("@fromdate", fromdate);
        //                cmd.Parameters.AddWithValue("@todate", todate);
        //                cmd.Parameters.AddWithValue("@userId", UserId);
        //                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
        //                {
        //                    DataSet ds = new DataSet();
        //                    sda.Fill(ds);
        //                    DataTable dtt = ds.Tables[0];
        //                    model.LstDailyProductionReport = dtt.DataTableToList<DailyTLProduction>();

        //                    DataTable dtsummary = ds.Tables[1];
        //                    model.LstDailyTLwiseProductionReport = dtsummary.DataTableToList<DailyTLwiseProduction>();

        //                }
        //            }
        //        }
        //        return PartialView("PeriodicProductionView", model);
        //    }
        //    else
        //    {


        //        return PartialView("PeriodicProductionChartView");
        //    }




        //}

















        public ActionResult Openreasonentry()
        {

            return PartialView("/Views/Admin/_ReasonEntry.cshtml");
        }

        public ActionResult Openconfiguration()
        {

            ProjectConfiguration Model = new ProjectConfiguration();
            return PartialView("/Views/Admin/_ProjectConfiguration.cshtml", Model);
        }

        public ActionResult Openbulkprojectconfiguration()
        {

            return PartialView("/Views/Admin/UploadProjectConfiguration.cshtml");

        }

        public ActionResult Openbulkrevenueconfiguration()
        {

            return PartialView("/Views/Admin/UploadRevenueconfiguration.cshtml");

        }

        public ActionResult OpenbulkEmployee()
        {

            return PartialView("/Views/Admin/UploadEmployee.cshtml");

        }






        public ActionResult OpenbulkUser()
        {

            return PartialView("/Views/Admin/Uploaduserdetails.cshtml");

        }

        public ActionResult OpenMonthconfiguration()
        {

            MonthlyConfiguration Model = new MonthlyConfiguration();
            return PartialView("/Views/Admin/_MonthlyConfiguration.cshtml", Model);
        }


        public ActionResult TeamleaduploadIndex()
        {

            return View("TeamLeadUploadViewIndex");


        }



        public ActionResult TeamleadPending()
        {

            return View("TeamleadPending");


        }



        public ActionResult UploadMember()
        {
            return View("EmployeeMapping");
        }

        public ActionResult Uploadbulkemployees()
        {
            return View("uploadbulkemployee");
        }




        public ActionResult ProductionuploadSummary()
        {
            ModelState.Clear();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select", Value = "0" },
                new SelectListItem { Text = "TVM", Value = "1" },
                new SelectListItem { Text = "KNPY", Value = "2" },
                new SelectListItem { Text = "MDS", Value = "3" },
                new SelectListItem { Text = "MQC", Value = "4" },
                new SelectListItem { Text = "MNS", Value = "5" },
                new SelectListItem { Text = "KAKKANAD", Value = "6" },
            };
            //Assigning generic list to ViewBag
            ViewBag.Locations = ObjList;

            return View("ProductionuploadSummary");
        }





        public ActionResult ProductionuploadIndexbyTL()
        {
            ModelState.Clear();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select", Value = "0" },
                new SelectListItem { Text = "TVM", Value = "1" },
                new SelectListItem { Text = "KNPY", Value = "2" },
                new SelectListItem { Text = "MDS", Value = "3" },
                new SelectListItem { Text = "MQC", Value = "4" },
                new SelectListItem { Text = "MNS", Value = "5" },
                new SelectListItem { Text = "KAKKANAD", Value = "6" },
            };
            //Assigning generic list to ViewBag
            ViewBag.Locations = ObjList;

            return View("ProductionUploadIndex");
        }




        public ActionResult UploadDailyCustomer()
        {
            ModelState.Clear();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select", Value = "0" },
                new SelectListItem { Text = "TVM", Value = "1" },
                new SelectListItem { Text = "KNPY", Value = "2" },
                new SelectListItem { Text = "MDS", Value = "3" },
                new SelectListItem { Text = "MQC", Value = "4" },
                new SelectListItem { Text = "MNS", Value = "5" },
                new SelectListItem { Text = "KAKKANAD", Value = "6" },
            };
            //Assigning generic list to ViewBag
            ViewBag.Locations = ObjList;

            return PartialView("UploadDailyCustomerDetails");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDailyCustomer(HttpPostedFileBase upload, string from, string FooBarDropDown)
        {

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

                if (ModelState.IsValid)
                {

                    string dtfrom = string.Empty;
                    DateTime dtdate = new DateTime();
                    dtdate = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var date = dtdate.ToString("yyyy-MM-dd");

                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                       


                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("UploadDailyCustomerDetails");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;

                        if (dataTable.Rows.Count > 0)
                        {
                            inserted = InsertCustomerProductionDetails(dataTable, from, FooBarDropDown);
                            if (inserted)
                            {
                                ModelState.AddModelError("File", "File Uploaded Successfully");
                                return View("FinalQctoCusList");

                            }
                            else
                            {
                                ModelState.AddModelError("File", "Error in uploading file");
                                return View("UploadDailyCustomerDetails");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                        return View("UploadDailyCustomerDetails");
                    }
                }

                return View("UploadDailyCustomerDetails");
            }
            catch (Exception ex)
            {

                //string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                //message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                //ModelState.AddModelError(string.Empty, message);


                ModelState.AddModelError("File", "Error in uploading file");
                return View("FinalQctoCusList");
            }

        }


        public bool InsertCustomerProductionDetails(DataTable dt, string datefrom, string Location)
        {
            string locationName = string.Empty;
            if (Location == "1")
            {
                locationName = "TVM";
            }
            else if (Location == "2")
            {
                locationName = "KNPY";
            }
            else if (Location == "3")
            {
                locationName = "MDS";
            }
            else if (Location == "4")
            {
                locationName = "MQC";
            }
            else if (Location == "5")
            {
                locationName = "MNS";
            }
            else if (Location == "6")
            {
                locationName = "KAKKANAD";
            }

            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(datefrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var date = dtdate.ToString("yyyy-MM-dd");

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO productiontocustomer (project,Eventcode,location,noofcharacters,TL,proddate ) VALUES (@project,@Eventcode,@location,@noofcharacters,@tl,@proddate)";
                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                     


                        using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                        {

                                myCmd.Parameters.AddWithValue("@project", dt.Rows[i]["ProjectCode"]);
                                myCmd.Parameters.AddWithValue("@Eventcode", dt.Rows[i]["Eventcode"]);
                                myCmd.Parameters.AddWithValue("@location", dt.Rows[i]["location"]);
                                myCmd.Parameters.AddWithValue("@noofcharacters", dt.Rows[i]["NoofCharacters"]);
                                myCmd.Parameters.AddWithValue("@TL", dt.Rows[i]["TL"]);
                                myCmd.Parameters.AddWithValue("@proddate", date);
                                int result = myCmd.ExecuteNonQuery();
                         }


                    }
                   
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }



            }
        }

        public bool ExistRecord(string date, string project, string location, int noofcharacter, string TL, string Eventcode)
        {
            bool exist=false;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
             
            string query = "SELECT count(*) from `productiontocustomer` where project ='" + project  + "' and location='" + location  + "' and `noofcharacters`=" + noofcharacter + " and TL='" + TL + "' and `Eventcode`='" + Eventcode  + "' and proddate='" + date + "'";
                        using (MySqlCommand cmd = new MySqlCommand(query))
                        {
                            cmd.Connection = mConnection;
                            mConnection.Open();
                            using (MySqlDataReader sdr = cmd.ExecuteReader())
                            {
                                if (sdr.Read())
                                {
                                exist= true;
                                }
                               
                            }

                            mConnection.Close();
                        }
              }
            return exist;

        }


        public ActionResult Associate()
        {

            List<SelectListItem> ProjectCodes = new List<SelectListItem>();
            

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `projectcode` from `productionreport2020` where year(date)=2020 and projectcode<>'' order by `pecode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ProjectCodes.Add(new SelectListItem
                            {
                                Text = sdr["projectcode"].ToString(),
                                Value = sdr["projectcode"].ToString()
                            });
                        }
                    }
                    
                    mConnection.Close();
                }
            }
            ViewBag.ProjectCodes = ProjectCodes;
            
            return View("AssociatewiseProductivity");
        }

        public ActionResult BindAssociateProject(string startdate, string enddate, string PSN)
        {
            string query = string.Empty;
            var sdate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtstartdate = new DateTime();
            if (startdate != null)
            {
                dtstartdate = DateTime.ParseExact(startdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                sdate = dtstartdate.ToString("yyyy-MM-dd");
            }
            var edate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtenddate = new DateTime();
            if (enddate != null)
            {
                dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                edate = dtenddate.ToString("yyyy-MM-dd");
            }

            query = "select distinct projectcode from productionreport2020 where projectcode is not null and projectcode<>' ' ";

            if (startdate != null && startdate != "")
            {

                query = query + " and productionreport2020.date>='" + sdate + "'";
            }

            if (enddate != null && enddate != "")
            {

                query = query + " and productionreport2020.date<='" + edate + "'";
            }

            if (PSN != null)
            {
                query = query + " and productionreport2020.psn='" + PSN + "'";
            }

            List<Project> objproject = new List<Project>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Project>();
            SelectList obgproject = new SelectList(objproject, "projectcode", "projectcode", 0);
            return Json(obgproject);

        }

        public ActionResult AssociateProductivity(string startdate, string enddate, string ProjectId, string PSN)
        {
            string query =string.Empty;
            AssociatewiseModel model = new AssociatewiseModel();

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


            DateTime dtstartdate = DateTime.ParseExact(startdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var stdate = dtstartdate.ToString("yyyy-MM-dd");


            DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var edate = dtenddate.ToString("yyyy-MM-dd");


            if (ProjectId == "ALL")

                query = "select associate,date_format(date, '%d/%m/%Y') as date, `projectcode`,`eventcode`,process,Round(`plannedprodrecord`) as plannedprodrecord ,`actualprodrecord`,`workedhrs`,Round(`actualprodrecord`/`workedhrs`) as Productivity from productionreport2020 where date >='" + stdate + "'  and date <= '" + edate + "' and PSN=" + int.Parse(PSN) + " group by date, projectcode order by date ";
            else
                query = "select associate,date_format(date, '%d/%m/%Y') as date, `projectcode`,`eventcode`,process,Round(`plannedprodrecord`) as plannedprodrecord,`actualprodrecord`,`workedhrs`,Round(`actualprodrecord`/`workedhrs`) as Productivity from productionreport2020 where date>='" + stdate + "' and date <= '" + edate + "' and PSN=" + int.Parse(PSN) + " and `projectcode`='" + ProjectId + "' group by date, projectcode order by date  ";


         
           DataTable dt=new DataTable();
           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
           using (MySqlConnection con = new MySqlConnection(constr))
           {
               using (MySqlCommand cmd = new MySqlCommand(query, con))
               {

                   using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                   {

                       sda.Fill(dt);
                     

                   }
               }
           }
           model.LstAssociatewiseModel = dt.DataTableToList<AssociatewiseModel>();
           return PartialView("_AssociatewiseProductivityList", model);


        }
#region Test

        public ActionResult Sample()
        {
            var model = new MyViewModel();
            return PartialView("_uploadsample",model);
        }
        [HttpPost]
        public ActionResult Sample(MyViewModel mRegister)
        {
            if (ModelState.IsValid)
            {
                //TO:DO
                var fileName = Path.GetFileName(mRegister.file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                mRegister.file.SaveAs(path);
                ViewBag.Message = "File has been uploaded successfully";
                ModelState.Clear();
            }
            return View();
        }

#endregion
        public ActionResult UploadRevenueReport()
        {
            ModelState.Clear();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select", Value = "0" },
                new SelectListItem { Text = "TVM", Value = "1" },
                new SelectListItem { Text = "KNPY", Value = "2" },
                new SelectListItem { Text = "MDS", Value = "3" },
                new SelectListItem { Text = "MQC", Value = "4" },
                new SelectListItem { Text = "MNS", Value = "5" },
                new SelectListItem { Text = "KAKKANAD", Value = "6" },
            };
            //Assigning generic list to ViewBag
            ViewBag.Locations = ObjList;

            return PartialView("_UploadRevenueReport");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadRevenueReport1(HttpPostedFileBase upload)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "Acceptable file extentions are.xls and .xlsx");
                            return View("uploaddailyrevenuedetails");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        if (dataTable.Rows.Count > 0)
                        {
                            string path = Server.MapPath("~/bin/ApplicationError.txt");
                            using (System.IO.StreamWriter sw = System.IO.File.AppendText(path))
                            {
                                sw.WriteLine(dataTable.Rows.Count);

                            }
                            inserted = InsertRevenueReportDetails(dataTable);
                            using (System.IO.StreamWriter sw = System.IO.File.AppendText(path))
                            {
                                sw.WriteLine(inserted);

                            }
                            
                            if (inserted)
                            {
                                ModelState.AddModelError("File", "File Uploaded Successfully");
                                return View("uploaddailyrevenuedetails");

                            }
                            else
                            {
                                ModelState.AddModelError("File", "Error in uploading file,Please check column format or File already uploaded");
                                return View("uploaddailyrevenuedetails");
                            }
                        }


                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }

                return View("uploaddailyrevenuedetails");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("uploaddailyrevenuedetails");
            }

        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadRevenueReport(HttpPostedFileBase upload)
        {

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

                if (ModelState.IsValid)
                {

                  

                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {




                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return PartialView("/Views/Admin/_UploadRevenueReport.cshtml");

                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;

                        if (dataTable.Rows.Count > 0)
                        {
                            inserted = InsertRevenueReportDetails(dataTable);
                            if (inserted)
                            {
                                ModelState.AddModelError("File", "File Uploaded Successfully");
                                return RedirectToAction("GenerateRevenue");

                            }
                            else
                            {
                                ModelState.AddModelError("File", "Error in uploading file");
                                return PartialView("/Views/Admin/_UploadRevenueReport.cshtml");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }

                return View("GenerateRevenue");
            }
            catch (Exception ex)
            {

                //string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                //message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                //ModelState.AddModelError(string.Empty, message);


                ModelState.AddModelError("File", "Error in uploading file");
                return PartialView("/Views/Admin/_UploadRevenueReport.cshtml");
            }

        }

        public bool checkrevenueentrydata(string Projectcode, string eventcode, string batchname, string RO,double noofbatches,double invoicedcharacter, double ratecharacter, string location, string upldate, string clientcode)
        {
            bool Result;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string query = "select Projectcode  from `revenuereport` where `projectcode` ='" + Projectcode + "' and `eventcode`='" + eventcode + "'  and  batchname='" + batchname + "' and RO='" + RO + "' and invoicedcharacter=" + invoicedcharacter + "  and ratecharacter =" + ratecharacter + " and location='" + location + "' and clientcode='" + clientcode + "' and upldate='" + upldate + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {

                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }


                }
            }

            return Result;

        }




        public bool InsertRevenueReportDetails(DataTable dt)
        {
            

           
            string clientcode = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO revenuereport (projectcode,eventcode,`noofbatches`,invoicedcharacter,ratecharacter,location,upldate,Clientcode,`batchname`,`RO`) VALUES (@project,@event,@noofbatches,@invcharacter,@ratecharacter,@location,@upldate,@clientcode,@batchname,@RO)";
                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string project = dt.Rows[i][2].ToString();

                        var s = dt.Rows[i][2].ToString();
                        var commands = s.Split(new[] { '.' }, 2);

                        var Projectcode = commands[0];  // !say
                        var eventcode = commands[1];

                        string[] strArr = null;
                        char[] splitchar = { '_' };
                        strArr = project.Split(splitchar);
                        if (strArr.Length > 0)
                            clientcode = strArr[0].ToString();

                        var ddate = dt.Rows[i][0].ToString();
                        string fromdate = string.Empty;
                        string[] strArrDate = null;
                        char[] splitcharDate = { '/' };
                        strArrDate = ddate.Split(splitcharDate);
                        if (strArrDate.Length > 0)
                        {
                            string value = string.Empty;
                            value = strArrDate[2].ToString();
                            int index = value.IndexOf(" 12:00:00 AM");
                            if (index != -1)
                            {
                                value = value.Remove(index);
                            }
                            if (strArrDate[0].Length == 1 && strArrDate[1].Length == 1)
                                fromdate = value + "-" + "0" + strArrDate[0] + "-" + "0" + strArrDate[1];
                            else if (strArrDate[0].Length == 1 && strArrDate[1].Length > 1)
                                fromdate = value + "-" + "0" + strArrDate[0] + "-" + strArrDate[1];
                            else if (strArrDate[0].Length > 1 && strArrDate[1].Length == 1)
                                fromdate = value + "-" + strArrDate[0] + "-" + "0" + strArrDate[1];
                            else if (strArrDate[0].Length > 1 && strArrDate[1].Length > 1)
                                fromdate = value + "-" + strArrDate[0] + "-" + strArrDate[1];
                        }
                      if (!checkrevenueentrydata(Projectcode, eventcode, dt.Rows[i][1].ToString(), dt.Rows[i][3].ToString(),double.Parse(dt.Rows[i][4].ToString()), double.Parse(dt.Rows[i][5].ToString()), double.Parse(dt.Rows[i][6].ToString()),dt.Rows[i][7].ToString(), fromdate, clientcode))
                        {
                        using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                        {
                            myCmd.Parameters.AddWithValue("@project", Projectcode);
                            myCmd.Parameters.AddWithValue("@event", eventcode);
                            myCmd.Parameters.AddWithValue("@batchname", dt.Rows[i][1]);
                            myCmd.Parameters.AddWithValue("@RO", dt.Rows[i][3]);
                            myCmd.Parameters.AddWithValue("@noofbatches", dt.Rows[i][4]);
                            myCmd.Parameters.AddWithValue("@invcharacter", dt.Rows[i][5]);
                            myCmd.Parameters.AddWithValue("@ratecharacter", dt.Rows[i][6]);
                            myCmd.Parameters.AddWithValue("@location", dt.Rows[i][7]);
                            myCmd.Parameters.AddWithValue("@upldate", fromdate);
                            myCmd.Parameters.AddWithValue("@clientcode", clientcode);
                          
                            int result = myCmd.ExecuteNonQuery();
                        }


                    }

                      else
                      {
                          return false;
                          break;
                      }


                    }

                    return true;
                }
                catch (Exception)
                {

                    return false;
                }



            }
        }






        public ActionResult UploadDailyPromotion()
        {
            ModelState.Clear();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select", Value = "0" },
                new SelectListItem { Text = "TVM", Value = "1" },
                new SelectListItem { Text = "KNPY", Value = "2" },
                new SelectListItem { Text = "MDS", Value = "3" },
                new SelectListItem { Text = "MQC", Value = "4" },
                new SelectListItem { Text = "MNS", Value = "5" },
                new SelectListItem { Text = "KAKKANAD", Value = "6" },
            };
            //Assigning generic list to ViewBag
            ViewBag.Locations = ObjList;

            return PartialView("_UploadDailyPromotionDetails");
        }


        public ActionResult UploadPromotion()
        {
            ModelState.Clear();
            return View("uploaddailypromotiondetails");
        }

        public ActionResult UploadRevenue()
        {
            ModelState.Clear();
            return View("uploaddailyrevenuedetails");
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDailyPromotion(HttpPostedFileBase upload)
        {

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

                if (ModelState.IsValid)
                {

                    //string dtfrom = string.Empty;
                    //DateTime dtdate = new DateTime();
                    //dtdate = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                  
                    
                    

                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {




                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return PartialView("/Views/Admin/_UploadDailyPromotionDetails.cshtml");
                            
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;

                        if (dataTable.Rows.Count > 0)
                        {
                            inserted = InsertPromotionDetails(dataTable);
                            if (inserted)
                            {
                                ModelState.AddModelError("File", "File Uploaded Successfully");
                                return RedirectToAction("PromotionRelease"); ;

                            }
                            else
                            {
                                ModelState.AddModelError("File", "Error in uploading file");
                                return PartialView("/Views/Admin/_UploadDailyPromotionDetails.cshtml");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }

                return View("PromotionList");
            }
            catch (Exception ex)
            {

                //string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                //message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                //ModelState.AddModelError(string.Empty, message);


                ModelState.AddModelError("File", "Error in uploading file");
                return PartialView("/Views/Admin/_UploadDailyPromotionDetails.cshtml");
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDailyPromotion1(HttpPostedFileBase upload)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "Acceptable file extentions are.xls and .xlsx");
                            return View("uploaddailypromotiondetails");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        if (dataTable.Rows.Count > 0)
                        {
                            inserted = InsertPromotionDetails(dataTable);
                            if (inserted)
                            {
                                ModelState.AddModelError("File", "File Uploaded Successfully");
                                return View("uploaddailypromotiondetails");

                            }
                            else
                            {
                                ModelState.AddModelError("File", "Error in uploading file or Already uploaded file,Please check column format");
                                return View("uploaddailypromotiondetails");
                            }
                        }


                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }

                return View("uploaddailypromotiondetails");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("uploaddailypromotiondetails");
            }

        }



       


        public bool InsertPromotionDetails(DataTable dt)
        {
            

          
            string clientcode = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO promotiontocustomer (project,eventcode,`noofbatches`,`Totalpromotion`,`characterrate`,`location`,proddate,Clientcode) VALUES (@project,@event,@noofbatches,@totalpromotion,@ratecharacter,@location,@proddate,@clientcode)";
                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string project = dt.Rows[i][1].ToString();

                        var s = dt.Rows[i][1].ToString();
                        var commands = s.Split(new[] { '.' }, 2);

                        var Projectcode = commands[0];  // !say
                        var eventcode = commands[1];   

                        string[] strArr = null;
                        char[] splitchar = { '_' };
                        strArr = project.Split(splitchar);
                        if (strArr.Length > 0)
                            clientcode = strArr[0].ToString();



                       var ddate = dt.Rows[i][0].ToString();
                       string fromdate = string.Empty;
                        string[] strArrDate = null;
                        char[] splitcharDate = { '/' };
                        strArrDate = ddate.Split(splitcharDate);
                        if (strArrDate.Length > 0)
                        {
                            string value = string.Empty;
                            value = strArrDate[2].ToString().Substring(0, 4);
                            //int index = value.IndexOf(" 12:00:00 AM");
                            //if (index != -1)
                            //{
                            //    value = value.Remove(index);
                            //}
                            if (strArrDate[0].Length == 1 && strArrDate[1].Length == 1)
                                fromdate = value + "-" + "0" + strArrDate[0] + "-" + "0" + strArrDate[1];
                            else if (strArrDate[0].Length == 1 && strArrDate[1].Length > 1)
                                fromdate = value + "-" + "0" + strArrDate[0] + "-" + strArrDate[1];
                            else if (strArrDate[0].Length > 1 && strArrDate[1].Length == 1)
                                fromdate = value + "-" + strArrDate[0] + "-" + "0" + strArrDate[1];
                            else if (strArrDate[0].Length > 1 && strArrDate[1].Length > 1)
                                fromdate = value + "-" +  strArrDate[0] + "-" + strArrDate[1];
                        }

                        if (!checkpromotionentrydata(Projectcode, eventcode, double.Parse(dt.Rows[i][2].ToString()), double.Parse(dt.Rows[i][3].ToString()), double.Parse(dt.Rows[i][4].ToString()), dt.Rows[i][5].ToString(), fromdate, clientcode))
                        {
                            using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                            {
                                myCmd.Parameters.AddWithValue("@project", Projectcode);
                                myCmd.Parameters.AddWithValue("@event", eventcode);
                                myCmd.Parameters.AddWithValue("@noofbatches", dt.Rows[i][2]);
                                myCmd.Parameters.AddWithValue("@totalpromotion", dt.Rows[i][3]);
                                myCmd.Parameters.AddWithValue("@ratecharacter", dt.Rows[i][4]);
                                myCmd.Parameters.AddWithValue("@location", dt.Rows[i][5]);
                                myCmd.Parameters.AddWithValue("@proddate", fromdate);
                                myCmd.Parameters.AddWithValue("@clientcode", clientcode);
                                int result = myCmd.ExecuteNonQuery();
                            }
                           
                        }
                        else
                        {
                            return false;
                            break;
                        }

                       
                    }

                    return true;
                }
                catch (Exception)
                {

                    return false;
                }



            }
        }


        public bool checkpromotionentrydata(string Projectcode, string eventcode, double noofbatches,double totalpromotion,double ratecharacter,string location,string proddate,string clientcode)
        {
            bool Result;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string query = "select Project  from `promotiontocustomer` where `project` ='" + Projectcode + "' and `eventcode`='" + eventcode + "'  and noofbatches=" + noofbatches + "  and Totalpromotion=" + totalpromotion + " and characterrate =" + ratecharacter + " and location='" + location + "' and clientcode='" + clientcode + "' and proddate='" + proddate + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {

                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }


                }
            }

            return Result;

        }






        public bool InsertUploadProductionTable(DataTable dt, string datefrom, string Location)
        {
            string locationName = string.Empty;
            if (Location == "1")
            {
                locationName = "TVM";
            }
            else if (Location == "2")
            {
                locationName = "KNPY";
            }
            else if (Location == "3")
            {
                locationName = "MDS";
            }
            else if (Location == "4")
            {
                locationName = "MQC";
            }
            else if (Location == "5")
            {
                locationName = "MNS";
            }
            else if (Location == "6")
            {
                locationName = "KAKKANAD";
            }

            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(datefrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var date = dtdate.ToString("yyyy-MM-dd");

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks,Date,Location,teamleadid,workathome ) VALUES (@psn, @process,@project, @projectcode,@eventcode,@hoursplanned,@hoursworked,@Actualproduction,@Remarks,@date,@location,@teamleadid,@workathome)";
                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                        {
                            if (dt.Rows[i]["PSN"].ToString() != "")
                            {
                                myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN"]);
                                myCmd.Parameters.AddWithValue("@process", dt.Rows[i]["Process"]);
                                myCmd.Parameters.AddWithValue("@project", dt.Rows[i]["Project"]);
                                myCmd.Parameters.AddWithValue("@projectcode", dt.Rows[i]["Project Code"]);
                                myCmd.Parameters.AddWithValue("@eventcode", dt.Rows[i]["Event code"]);
                                myCmd.Parameters.AddWithValue("@hoursplanned", dt.Rows[i]["Hours planned"]);
                                myCmd.Parameters.AddWithValue("@hoursworked", dt.Rows[i]["Hours worked"]);
                                if (dt.Rows[i]["Actual Production Records"].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", Convert.ToInt32((dt.Rows[i]["Actual Production Records"])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", 0);

                                }
                                myCmd.Parameters.AddWithValue("@Remarks", dt.Rows[i]["Remarks"]);
                                myCmd.Parameters.AddWithValue("@date", date);
                                myCmd.Parameters.AddWithValue("@location", locationName);
                                myCmd.Parameters.AddWithValue("@teamleadid", int.Parse(Session["UserId"].ToString()));
                                if (dt.Rows[i]["Work @ Home"].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", Convert.ToInt32((dt.Rows[i]["Work @ Home"])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", 0);

                                }


                                int result = myCmd.ExecuteNonQuery();
                            }


                        }
                    }
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }



            }
        }



        public ActionResult    ProductionuploadIndexByAdmin()
        {
            string strnot = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
          
            DateTime dt = DateTime.Now;

            for (int i = 0; i < 5; i++)
            {
                dt = dt.AddDays(-2);
                var ddate = dt.ToString("yyyy-MM-dd");
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT date from `productionreport2020` where date='" + ddate + "'";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.Read())
                        {


                        }
                        else
                        {
                            strnot=strnot + "," + ddate;
                        }
                    }

                    mConnection.Close();
                }
            }
            
            }




           

            ModelState.Clear();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select", Value = "0" },
                new SelectListItem { Text = "TVM", Value = "1" },
                new SelectListItem { Text = "KNPY", Value = "2" },
                new SelectListItem { Text = "MDS", Value = "3" },
                new SelectListItem { Text = "MQC", Value = "4" },
                new SelectListItem { Text = "MNS", Value = "5" },
                new SelectListItem { Text = "KAKKANAD", Value = "6" },
            };
            //Assigning generic list to ViewBag
            ViewBag.Locations = ObjList;
           
            if (strnot.Length > 0)
            {
                strnot = strnot.Remove(0, 1);
                
                ViewBag.Strnot = " Data Upload for  " + strnot + " is Pending";
            }
            return View("ProductionuploadIndexByAdmin");

            //RevenueModel Model = new RevenueModel();
            //string date=  DateTime.Today.ToString("dd-MM-yyyy");
            //string[] strArr = null;
            //char[] splitchar = { '-' };
            //strArr = date.Split(splitchar);
            //if (strArr.Length > 0)
            //    date = strArr[1] + "/" + strArr[0] + "/" + strArr[2];

            //int days = DateTime.DaysInMonth(int.Parse(strArr[2]), int.Parse(strArr[1]));

            //string Command = string.Empty;
            //DataTable dt = new DataTable();
            //string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            //Command = "SELECT date,ROUND(sum(actualrevenue), 0)  as actualrevenue  ,ROUND(sum(targetrevenue), 0) as targetrevenue,ROUND((sum(actualrevenue)/sum(targetrevenue)*100),0) as achievement from `productionreport2020` where    MONTH(date) =" + strArr[1] + " and year(date)=" + strArr[2] + " group by  date";

            //using (MySqlConnection mConnection = new MySqlConnection(connString))
            //{
            //    mConnection.Open();
            //    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
            //    adapter.Fill(dt);
            //    Model.RevenueModelList = dt.DataTableToList<RevenueModel>();

            //}

            //return View("ProductionuploadIndexByAdmin",Model);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadProductionSummary(HttpPostedFileBase upload, string dateFrom, string FooBarDropDown)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("ProductionUploadIndex");
                        }

                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                        //Return as DataSet and Set the First Row as Column Name
                        DataSet dataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });


                        DataTableCollection table = dataSet.Tables;

                        //Store it in DataTable
                        DataTable dataTable = table[1];

                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;

                        if (LocationDataInsert(dateFrom, FooBarDropDown))
                        {
                            ModelState.AddModelError("File", "Already uploaded the file " + dateFrom);
                            return View("ProductionUploadIndex");
                        }
                        else
                        {


                            if (dataTable.Rows.Count > 0)
                            {
                                inserted = InsertSummaryProductionTable(dataTable, dateFrom, FooBarDropDown);
                                if (inserted)
                                {
                                    InsertReason(dateFrom, FooBarDropDown);
                                    ModelState.AddModelError("File", "File Uploaded Successfully");
                                    return View("ProductionUploadIndex");
                                }
                                else
                                {
                                    ModelState.AddModelError("File", "Error in uploading file");
                                    return View("ProductionUploadIndex");
                                }
                            }
                        }





                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }

                return View("ProductionUploadIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndex");
            }

        }

        public bool InsertSummaryProductionTable(DataTable dt, string datefrom, string Location)
        {
            string locationName = string.Empty;
            if (Location == "1")
            {
                locationName = "TVM";
            }
            else if (Location == "2")
            {
                locationName = "KNPY";
            }
            else if (Location == "3")
            {
                locationName = "MDS";
            }
            else if (Location == "4")
            {
                locationName = "MQC";
            }
            else if (Location == "5")
            {
                locationName = "MNS";
            }
            else if (Location == "6")
            {
                locationName = "KAKKANAD";
            }

            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(datefrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var date = dtdate.ToString("yyyy-MM-dd");

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks,Date,Location,teamleadid,workathome ) VALUES (@psn, @process,@project, @projectcode,@eventcode,@hoursplanned,@hoursworked,@Actualproduction,@Remarks,@date,@location,@teamleadid,@workathome)";
                    connection.Open();
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                        {
                            if (dt.Rows[i][0].ToString() != "")
                            {
                                myCmd.Parameters.AddWithValue("@psn", dt.Rows[i][0]);
                                myCmd.Parameters.AddWithValue("@process", dt.Rows[i][3]);
                                myCmd.Parameters.AddWithValue("@project", dt.Rows[i][4]);
                                myCmd.Parameters.AddWithValue("@projectcode", dt.Rows[i][5]);
                                myCmd.Parameters.AddWithValue("@eventcode", dt.Rows[i][6]);
                                myCmd.Parameters.AddWithValue("@hoursplanned", dt.Rows[i][8]);
                                myCmd.Parameters.AddWithValue("@hoursworked", dt.Rows[i][11]);
                                if (dt.Rows[i][12].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", Convert.ToInt32((dt.Rows[i][12])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", 0);

                                }
                                myCmd.Parameters.AddWithValue("@Remarks", dt.Rows[i][15]);
                                myCmd.Parameters.AddWithValue("@date", date);
                                myCmd.Parameters.AddWithValue("@location", locationName);
                                myCmd.Parameters.AddWithValue("@teamleadid", int.Parse(Session["UserId"].ToString()));
                                if (dt.Rows[i][14].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", Convert.ToInt32((dt.Rows[i][14])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", 0);

                                }


                                int result = myCmd.ExecuteNonQuery();
                            }


                        }
                    }
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }



            }
        }





        public bool InsertUploadCusDetails(DataTable dt, string datefrom, string Location)
        {
            string locationName = string.Empty;
            if (Location == "1")
            {
                locationName = "TVM";
            }
            else if (Location == "2")
            {
                locationName = "KNPY";
            }
            else if (Location == "3")
            {
                locationName = "MDS";
            }
            else if (Location == "4")
            {
                locationName = "MQC";
            }
            else if (Location == "5")
            {
                locationName = "MNS";
            }
            else if (Location == "6")
            {
                locationName = "KAKKANAD";
            }

            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(datefrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var date = dtdate.ToString("yyyy-MM-dd");

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks,Date,Location,teamleadid,workathome ) VALUES (@psn, @process,@project, @projectcode,@eventcode,@hoursplanned,@hoursworked,@Actualproduction,@Remarks,@date,@location,@teamleadid,@workathome)";
                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                        {
                            if (dt.Rows[i]["PSN"].ToString() != "")
                            {
                                myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN"]);
                                myCmd.Parameters.AddWithValue("@process", dt.Rows[i]["Process"]);
                                myCmd.Parameters.AddWithValue("@project", dt.Rows[i]["Project"]);
                                myCmd.Parameters.AddWithValue("@projectcode", dt.Rows[i]["Project Code"]);
                                myCmd.Parameters.AddWithValue("@eventcode", dt.Rows[i]["Event code"]);
                                myCmd.Parameters.AddWithValue("@hoursplanned", dt.Rows[i]["Hours planned"]);
                                myCmd.Parameters.AddWithValue("@hoursworked", dt.Rows[i]["Hours worked"]);
                                if (dt.Rows[i]["Actual Production Records"].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", Convert.ToInt32((dt.Rows[i]["Actual Production Records"])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", 0);

                                }
                                myCmd.Parameters.AddWithValue("@Remarks", dt.Rows[i]["Remarks"]);
                                myCmd.Parameters.AddWithValue("@date", date);
                                myCmd.Parameters.AddWithValue("@location", locationName);
                                myCmd.Parameters.AddWithValue("@teamleadid", int.Parse(Session["UserId"].ToString()));
                                if (dt.Rows[i]["Work @ Home"].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", Convert.ToInt32((dt.Rows[i]["Work @ Home"])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", 0);

                                }


                                int result = myCmd.ExecuteNonQuery();
                            }


                        }
                    }
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }



            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductionUploadbyAdmin(HttpPostedFileBase upload, string from, string FooBarDropDown, bool Kakkanad, bool TVM, bool KNPY, bool MDS, bool MNS, bool MQC)
        { 

            try
            {
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

                if (ModelState.IsValid)
                {

                    string dtfrom = string.Empty;
                    DateTime dtdate = new DateTime();
                    dtdate = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var date = dtdate.ToString("yyyy-MM-dd");
                    
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {
                        
                        if (Kakkanad == true )
                        {

                            string Command = "INSERT INTO Holiday(holidaydate, location) VALUES ('" + date + "','KKND'  );";
                            using (MySqlConnection mConnection = new MySqlConnection(connString))
                            {
                                mConnection.Open();
                                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                                {
                                    myCmd.ExecuteNonQuery();


                                }
                            }

                        }
                         if (TVM == true)
                        {

                            string Command = "INSERT INTO Holiday(holidaydate, location) VALUES ('" + date + "','TVM'  );";
                            using (MySqlConnection mConnection = new MySqlConnection(connString))
                            {
                                mConnection.Open();
                                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                                {
                                    myCmd.ExecuteNonQuery();


                                }
                            }
                        }

                        if (KNPY == true)
                        {

                            string Command = "INSERT INTO Holiday(holidaydate, location) VALUES ('" + date + "','KNPY'  );";
                            using (MySqlConnection mConnection = new MySqlConnection(connString))
                            {
                                mConnection.Open();
                                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                                {
                                    myCmd.ExecuteNonQuery();


                                }
                            }
                        }

                           
                        if (MDS == true)
                             {

                                 string Command = "INSERT INTO Holiday(holidaydate, location) VALUES ('" + date + "','MDS'  );";
                            using (MySqlConnection mConnection = new MySqlConnection(connString))
                            {
                                mConnection.Open();
                                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                                {
                                    myCmd.ExecuteNonQuery();


                                }
                            }
                        }

                         if (MQC == true)

                             {

                                 string Command = "INSERT INTO Holiday(holidaydate, location) VALUES ('" + date + "','MQC'  );";
                            using (MySqlConnection mConnection = new MySqlConnection(connString))
                            {
                                mConnection.Open();
                                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                                {
                                    myCmd.ExecuteNonQuery();


                                }
                            }
                        }
                       if (MNS == true)
                        {

                            string Command = "INSERT INTO Holiday(holidaydate, location) VALUES ('" + date + "','MNS'  );";
                            using (MySqlConnection mConnection = new MySqlConnection(connString))
                            {
                                mConnection.Open();
                                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                                {
                                    myCmd.ExecuteNonQuery();


                                }
                            }
                        }


                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("ProductionUploadIndex");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;

                            if (dataTable.Rows.Count > 0)
                            {
                                inserted = InsertProductionTablebyAdmin(dataTable, from, FooBarDropDown);
                                if (inserted)
                                {

                                    string dtlocation = CheckTargetrevenue(from, FooBarDropDown);

                                    if (dtlocation == string.Empty)
                                    {
                                        // InsertReason(dateFrom, FooBarDropDown);
                                        ModelState.AddModelError("File", "File Uploaded Successfully" + " But target revenue is different in location " + dtlocation);
                                        return View("ProductionUploadIndexByAdmin");
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("File", "File Uploaded Successfully");
                                        return View("ProductionUploadIndexByAdmin");
                                    }


                                }

                                else
                                {
                                    ModelState.AddModelError("File", "Error in uploading file");
                                    return View("ProductionUploadIndexByAdmin");
                                }
                            }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }

                return View("ProductionUploadIndexByAdmin");
            }
            catch (Exception ex)
            {

                //string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                //message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                //ModelState.AddModelError(string.Empty, message);
                
                
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndexByAdmin");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductionUpload(HttpPostedFileBase upload, string dateFrom, string FooBarDropDown)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("ProductionUploadIndex");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;

                        if (LocationDataInsert(dateFrom, FooBarDropDown))
                        {
                            ModelState.AddModelError("File", "Already uploaded the file " + dateFrom);
                            return View("ProductionUploadIndex");
                        }
                        else
                        {


                            if (dataTable.Rows.Count > 0)
                            {
                                inserted = InsertProductionTable(dataTable, dateFrom, FooBarDropDown);
                                if (inserted)
                                {
                                    InsertReason(dateFrom, FooBarDropDown);
                                    ModelState.AddModelError("File", "File Uploaded Successfully");
                                    return View("ProductionUploadIndex");
                                }
                                else
                                {
                                    ModelState.AddModelError("File", "Error in uploading file");
                                    return View("ProductionUploadIndex");
                                }
                            }
                        }





                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }

                return View("ProductionUploadIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndex");
            }

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BulkEmployeeUpload(HttpPostedFileBase upload)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("ProductionUploadIndex");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;



                        if (dataTable.Rows.Count > 0)
                        {
                            UploadBulkemployeedetails(dataTable);

                        }
                    }





                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }


                return View("ProductionUploadIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndex");
            }

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadBulkemployee(HttpPostedFileBase upload)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("ProductionUploadIndex");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;



                        if (dataTable.Rows.Count > 0)
                        {
                            UploadBulkemployeedetails(dataTable);

                        }
                    }





                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }


                return View("ProductionUploadIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndex");
            }

        }

        public ActionResult UploadBulkemployeedetails(DataTable dt)
        {

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {

                    string cmdText = "INSERT IGNORE INTO `memployee` (`PSN`,`AssociateName`,`Location`,'TL',`DOJ`,`TLId` ) VALUES (@psn, @associatename,@location,@tlname, @doj,@tlid)";
                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        using (MySqlCommand myCmd = new MySqlCommand("INSERT IGNORE INTO `memployee` (`PSN`,`AssociateName`,`Location`,'TL',`DOJ`,`TLId` ) VALUES(" + dt.Rows[i]["Psn"] + ",'" + dt.Rows[i]["Assocaite Name"] + "', '" + dt.Rows[i]["Location"] + "','" + dt.Rows[i]["TL Name"] + "'"))
                        {


                            //myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["Psn"]);
                            //myCmd.Parameters.AddWithValue("@associatename", dt.Rows[i]["Assocaite Name"]);
                            //myCmd.Parameters.AddWithValue("@location", dt.Rows[i]["location"]);
                            //myCmd.Parameters.AddWithValue("@tlname", dt.Rows[i]["TL Name"]);

                            //// myCmd.Parameters.AddWithValue("@doj",dt.Rows[i]["doj"]);
                            //myCmd.Parameters.AddWithValue("@doj", "03/02/2020");
                            //myCmd.Parameters.AddWithValue("@tlid", 100);

                            int result = myCmd.ExecuteNonQuery();
                            myCmd.Dispose();
                        }
                    }



                }
                return View();

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndex");
            }


        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertBulkUpload(HttpPostedFileBase upload, string month, string location)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("ProductionUploadIndex");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;



                        if (dataTable.Rows.Count > 0)
                        {
                            UploadBulkprojectconfiguration(dataTable, month, location);

                        }
                    }





                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }


                return View("ProductionUploadIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndex");
            }

        }



        public bool UploadBulkprojectconfiguration(DataTable dt, string month, string location)
        {

            string monthname = string.Empty;
            string locationname = string.Empty;
            if (month == "1")
            {
                monthname = "January";
            }
            else if (month == "2")
            {
                monthname = "February";
            }
            else if (month == "3")
            {
                monthname = "March";
            }
            else if (month == "4")
            {
                monthname = "April";
            }
            else if (month == "5")
            {
                monthname = "May";
            }
            else if (month == "6")
            {
                monthname = "June";
            }
            else if (month == "7")
            {
                monthname = "July";
            }
            else if (month == "8")
            {
                monthname = "August";
            }
            else if (month == "9")
            {
                monthname = "September";
            }
            else if (month == "10")
            {
                monthname = "October";
            }
            else if (month == "11")
            {
                monthname = "November";
            }
            else if (month == "12")
            {
                monthname = "December";
            }


            if (location == "1")
            {
                locationname = "KNPY";
            }
            else if (location == "2")
            {

                locationname = "TVM";
            }
            else if (location == "3")
            {
                locationname = "MDS";
            }
            else if (location == "4")
            {
                locationname = "MQC";
            }
            else if (location == "5")
            {
                locationname = "MNS";

            }
            else if (location == "6")
            {
                locationname = "KAKKANAD";
            }


            List<string> dprocess = new List<string>();
            dprocess.Add("Indexing");
            dprocess.Add("QC2");
            dprocess.Add("QC3");
            dprocess.Add("UAT");
            dprocess.Add("Audit");
            dprocess.Add("Training");
            dprocess.Add("Rework");




            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO `projectconfiguration` (`Projectcode`,`Eventcode`,`Process`,`ProductionPlannedHr`,`location`,`month`,`monthname`,`locationId`,`year` ) VALUES (@Projectcode, @Eventcode,@Process, @ProductionPlannedHr,@location,@month,@monthname,@locationId,@year)";

                    //string cmdText = "INSERT  INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks,Date,Location,teamleadid,workathome ) VALUES (@psn, @process,@project, @projectcode,@eventcode,@hoursplanned,@hoursworked,@Actualproduction,@Remarks,@date,@location,@teamleadid,@workathome)";

                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {






                        foreach (var process in dprocess)
                        {


                            using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                            {


                                myCmd.Parameters.AddWithValue("@Projectcode", dt.Rows[i]["Projectcode"]);
                                myCmd.Parameters.AddWithValue("@Eventcode", dt.Rows[i]["Eventcode"]);
                                myCmd.Parameters.AddWithValue("@Process", process);


                                if (process == "Training" || process == "Rework")
                                {
                                    myCmd.Parameters.AddWithValue("@ProductionPlannedHr", 0);
                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@ProductionPlannedHr", dt.Rows[i][process]);
                                }

                                myCmd.Parameters.AddWithValue("@location", locationname);
                                myCmd.Parameters.AddWithValue("@month", int.Parse(month));
                                myCmd.Parameters.AddWithValue("@monthname", monthname);
                                myCmd.Parameters.AddWithValue("@locationId", int.Parse(location));
                                myCmd.Parameters.AddWithValue("@year", dt.Rows[i]["Year"]);







                                int result = myCmd.ExecuteNonQuery();
                                myCmd.Dispose();
                            }
                        }


                    }

                    return true;
                }
                catch (Exception)
                {

                    return false;
                }






                //string Command = "INSERT INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction ) VALUES ();";
                //using (MySqlConnection mConnection = new MySqlConnection(connString))
                //    {
                //        mConnection.Open();
                //        using (MySqlTransaction trans = mConnection.BeginTransaction())
                //        {
                //            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection, trans))
                //            {
                //                myCmd.CommandType = CommandType.Text;
                //                for (int i = 0; i < dt.Rows.Count; i++)
                //                {
                //                            myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN"]);
                //                            myCmd.Parameters.AddWithValue("@process", dt.Rows[i]["Process"]);
                //                            myCmd.Parameters.AddWithValue("@project", dt.Rows[i]["Project"]);
                //                            myCmd.Parameters.AddWithValue("@projectcode", dt.Rows[i]["Project Code"]);
                //                            myCmd.Parameters.AddWithValue("@eventcode", dt.Rows[i]["Event code"]);
                //                            myCmd.Parameters.AddWithValue("@hoursplanned", dt.Rows[i]["Hours planned"]);
                //                            myCmd.Parameters.AddWithValue("@hoursworked", dt.Rows[i]["Hours worked"]);
                //                            myCmd.Parameters.AddWithValue("@Actualproduction", Convert.ToInt32((dt.Rows[i]["Actual Production Records"])));

                //                            myCmd.ExecuteNonQuery();
                //                             trans.Commit();
                //                }

                //            }
                //        }
                //    }
            }
        }













        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertSampleprojfileUpload(HttpPostedFileBase upload)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("ProductionUploadIndex");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;



                        if (dataTable.Rows.Count > 0)
                        {
                            Insertsampleprojectconf(dataTable);

                        }
                    }





                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }


                return View("ProductionUploadIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndex");
            }

        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertConsolidationUpload(HttpPostedFileBase upload)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("SampleFileupload");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;



                        if (dataTable.Rows.Count > 0)
                        {
                            InsertConsolidationUpload(dataTable);

                        }
                    }





                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }


                return View("ProductionUploadIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndex");
            }

        }













        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertSamplefileUpload(HttpPostedFileBase upload)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    bool inserted;
                    if (upload != null && upload.ContentLength > 0)
                    {

                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View("ProductionUploadIndex");
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];



                        string[] columnNames = (from dc in dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                select dc.ColumnName).ToArray();

                        DateTime sheetdate = DateTime.Now;
                        string TL = string.Empty;



                        if (dataTable.Rows.Count > 0)
                        {
                            InsertsampleProductionTable(dataTable);

                        }
                    }





                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }


                return View("ProductionUploadIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "Error in uploading file");
                return View("ProductionUploadIndex");
            }

        }






        public bool Insertsampleprojectconf(DataTable dt)
        {


            int locationid = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO `projectconfiguration` (`Projectcode`,`Eventcode`,`Process`,`ProductionPlannedHr`,`location`,`month`,`monthname`,`locationId`,`year` ) VALUES (@Projectcode, @Eventcode,@Process, @ProductionPlannedHr,@location,@month,@monthname,@locationId,@year)";

                    //string cmdText = "INSERT  INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks,Date,Location,teamleadid,workathome ) VALUES (@psn, @process,@project, @projectcode,@eventcode,@hoursplanned,@hoursworked,@Actualproduction,@Remarks,@date,@location,@teamleadid,@workathome)";

                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {


                        if (dt.Rows[i]["location"].ToString() == "KNPY")
                        {
                            locationid = 1;
                        }
                        else if (dt.Rows[i]["location"].ToString() == "TVM")
                        {
                            locationid = 2;
                        }
                        else if (dt.Rows[i]["location"].ToString() == "MDS")
                        {
                            locationid = 3;
                        }
                        else if (dt.Rows[i]["location"].ToString() == "MQC")
                        {
                            locationid = 4;
                        }
                        else if (dt.Rows[i]["location"].ToString() == "MNS")
                        {
                            locationid = 5;

                        }
                        else if (dt.Rows[i]["location"].ToString() == "KAKKANAD")
                        {
                            locationid = 6;
                        }




                        using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                        {


                            myCmd.Parameters.AddWithValue("@Projectcode", dt.Rows[i]["Projectcode"]);
                            myCmd.Parameters.AddWithValue("@Eventcode", dt.Rows[i]["Eventcode"]);
                            myCmd.Parameters.AddWithValue("@Process", dt.Rows[i]["Process"]);
                            myCmd.Parameters.AddWithValue("@ProductionPlannedHr", dt.Rows[i]["ProductionPlannedHr"]);
                            myCmd.Parameters.AddWithValue("@location", dt.Rows[i]["location"]);
                            myCmd.Parameters.AddWithValue("@month", dt.Rows[i]["month"]);
                            myCmd.Parameters.AddWithValue("@monthname", dt.Rows[i]["monthname"]);
                            myCmd.Parameters.AddWithValue("@locationId", locationid);
                            myCmd.Parameters.AddWithValue("@year", dt.Rows[i]["year"]);




                            int result = myCmd.ExecuteNonQuery();
                            myCmd.Dispose();
                        }


                    }

                    return true;
                }
                catch (Exception)
                {

                    return false;
                }






                //string Command = "INSERT INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction ) VALUES ();";
                //using (MySqlConnection mConnection = new MySqlConnection(connString))
                //    {
                //        mConnection.Open();
                //        using (MySqlTransaction trans = mConnection.BeginTransaction())
                //        {
                //            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection, trans))
                //            {
                //                myCmd.CommandType = CommandType.Text;
                //                for (int i = 0; i < dt.Rows.Count; i++)
                //                {
                //                            myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN"]);
                //                            myCmd.Parameters.AddWithValue("@process", dt.Rows[i]["Process"]);
                //                            myCmd.Parameters.AddWithValue("@project", dt.Rows[i]["Project"]);
                //                            myCmd.Parameters.AddWithValue("@projectcode", dt.Rows[i]["Project Code"]);
                //                            myCmd.Parameters.AddWithValue("@eventcode", dt.Rows[i]["Event code"]);
                //                            myCmd.Parameters.AddWithValue("@hoursplanned", dt.Rows[i]["Hours planned"]);
                //                            myCmd.Parameters.AddWithValue("@hoursworked", dt.Rows[i]["Hours worked"]);
                //                            myCmd.Parameters.AddWithValue("@Actualproduction", Convert.ToInt32((dt.Rows[i]["Actual Production Records"])));

                //                            myCmd.ExecuteNonQuery();
                //                             trans.Commit();
                //                }

                //            }
                //        }
                //    }
            }
        }





        public bool InsertConsolidationUpload(DataTable dt)
        {


            string ddate = string.Empty;
            string fromdate = string.Empty;

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO Consolidatedreport (Date,Location,Plannedhrs,productionplanhrrecord,productionplanrecord,hrworked,ActProdRecord,Achievement,TargetRevenue,ActualRevenue,RevenueAchievement ) VALUES (@Date, @Location,@Plannedhrs,@productionplanhrrecord,@productionplanrecord,@hrworked,@ActProdRecord,@Achievement,@TargetRevenue,@ActualRevenue,@RevenueAchievement)";

                    //string cmdText = "INSERT  INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks,Date,Location,teamleadid,workathome ) VALUES (@psn, @process,@project, @projectcode,@eventcode,@hoursplanned,@hoursworked,@Actualproduction,@Remarks,@date,@location,@teamleadid,@workathome)";

                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                        {
                            if (dt.Rows[i]["DATE"].ToString() != "")
                            {

                                ddate = dt.Rows[i]["DATE"].ToString();

                                string[] strArr = null;
                                char[] splitchar = { '/' };
                                strArr = ddate.Split(splitchar);
                                if (strArr.Length > 0)
                                {
                                    string value = string.Empty;
                                    value = strArr[2].ToString();
                                    int index = value.IndexOf(" 12:00:00 AM");
                                    if (index != -1)
                                    {
                                        value = value.Remove(index);
                                    }
                                    if (strArr[0].Length == 1 && strArr[1].Length == 1)
                                        fromdate = "0" + strArr[0] + "/" + "0" + strArr[1] + "/" + value;
                                    else if (strArr[0].Length == 1 && strArr[1].Length > 1)
                                        fromdate = "0" + strArr[0] + "/" + strArr[1] + "/" + value;
                                    else if (strArr[0].Length > 1 && strArr[1].Length == 1)
                                        fromdate = strArr[0] + "/" + "0" + strArr[1] + "/" + value;
                                    else if (strArr[0].Length > 1 && strArr[1].Length > 1)
                                        fromdate = strArr[0] + "/" + strArr[1] + "/" + value;
                                }

                                myCmd.Parameters.AddWithValue("@Date", fromdate);
                                myCmd.Parameters.AddWithValue("@Location", dt.Rows[i]["LOCATION"]);
                                myCmd.Parameters.AddWithValue("@Plannedhrs", dt.Rows[i]["Hours planned"]);
                                myCmd.Parameters.AddWithValue("@productionplanhrrecord", dt.Rows[i]["Production planned/Hr Records"]);
                                myCmd.Parameters.AddWithValue("@productionplanrecord", dt.Rows[i]["Production    planned       Records"]);
                                myCmd.Parameters.AddWithValue("@hrworked", dt.Rows[i]["Hours worked"]);
                                myCmd.Parameters.AddWithValue("@ActProdRecord", dt.Rows[i]["Actual Production Records"]);

                                if (dt.Rows[i]["% Achievement"].ToString() == "")
                                {
                                    myCmd.Parameters.AddWithValue("@Achievement", 0);
                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@Achievement", double.Parse(dt.Rows[i]["% Achievement"].ToString()) * 100);
                                }

                                myCmd.Parameters.AddWithValue("@TargetRevenue", dt.Rows[i]["TARGET REVENUE INR"]);
                                myCmd.Parameters.AddWithValue("@ActualRevenue", dt.Rows[i]["ACTUAL REVENUE INR"]);
                                if (double.Parse(dt.Rows[i]["% REVENUE ACHIEVEMENT"].ToString()) == 0)
                                {
                                    myCmd.Parameters.AddWithValue("@RevenueAchievement", 0);
                                }
                                else
                                {

                                    myCmd.Parameters.AddWithValue("@RevenueAchievement", double.Parse(dt.Rows[i]["% REVENUE ACHIEVEMENT"].ToString()) * 100);
                                }



                                int result = myCmd.ExecuteNonQuery();
                                myCmd.Dispose();
                            }



                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }







            }
        }

















        public bool InsertsampleProductionTable(DataTable dt)
        {


            string ddate = string.Empty;
            string fromdate = string.Empty;

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks,Date,Location,teamleadid,workathome ) VALUES (@psn, @process,@project, @projectcode,@eventcode,@hoursplanned,@hoursworked,@Actualproduction,@Remarks,@date,@location,@teamleadid,@workathome)";

                    //string cmdText = "INSERT  INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks,Date,Location,teamleadid,workathome ) VALUES (@psn, @process,@project, @projectcode,@eventcode,@hoursplanned,@hoursworked,@Actualproduction,@Remarks,@date,@location,@teamleadid,@workathome)";

                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                        {
                            if (dt.Rows[i]["PSN"].ToString() != "")
                            {

                                ddate = dt.Rows[i]["DATE"].ToString();

                                string[] strArr = null;
                                char[] splitchar = { '/' };
                                strArr = ddate.Split(splitchar);
                                if (strArr.Length > 0)
                                {
                                    string value = string.Empty;
                                    value = strArr[2].ToString();
                                    int index = value.IndexOf(" 12:00:00 AM");
                                    if (index != -1)
                                    {
                                        value = value.Remove(index);
                                    }
                                    if (strArr[0].Length == 1 && strArr[1].Length == 1)
                                        fromdate = "0" + strArr[0] + "/" + "0" + strArr[1] + "/" + value;
                                    else if (strArr[0].Length == 1 && strArr[1].Length > 1)
                                        fromdate = "0" + strArr[0] + "/" + strArr[1] + "/" + value;
                                    else if (strArr[0].Length > 1 && strArr[1].Length == 1)
                                        fromdate = strArr[0] + "/" + "0" + strArr[1] + "/" + value;
                                    else if (strArr[0].Length > 1 && strArr[1].Length > 1)
                                        fromdate = strArr[0] + "/" + strArr[1] + "/" + value;
                                }

                                myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN"]);
                                myCmd.Parameters.AddWithValue("@process", dt.Rows[i]["Process"]);
                                myCmd.Parameters.AddWithValue("@project", dt.Rows[i]["Project"]);
                                myCmd.Parameters.AddWithValue("@projectcode", dt.Rows[i]["Project Code"]);
                                myCmd.Parameters.AddWithValue("@eventcode", dt.Rows[i]["Event code"]);
                                myCmd.Parameters.AddWithValue("@hoursplanned", dt.Rows[i]["Hours planned"]);
                                myCmd.Parameters.AddWithValue("@hoursworked", dt.Rows[i]["Hours worked"]);
                                if (dt.Rows[i]["Actual Production Records"].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", Convert.ToInt32((dt.Rows[i]["Actual Production Records"])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", 0);

                                }
                                myCmd.Parameters.AddWithValue("@Remarks", dt.Rows[i]["Remarks"]);
                                myCmd.Parameters.AddWithValue("@date", fromdate);
                                myCmd.Parameters.AddWithValue("@location", dt.Rows[i]["LOCATION"]);
                                myCmd.Parameters.AddWithValue("@teamleadid", int.Parse(Session["UserId"].ToString()));
                                if (dt.Rows[i]["Work @ Home"].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", Convert.ToInt32((dt.Rows[i]["Work @ Home"])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", 0);

                                }


                                int result = myCmd.ExecuteNonQuery();
                                myCmd.Dispose();
                            }



                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }






                //string Command = "INSERT INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction ) VALUES ();";
                //using (MySqlConnection mConnection = new MySqlConnection(connString))
                //    {
                //        mConnection.Open();
                //        using (MySqlTransaction trans = mConnection.BeginTransaction())
                //        {
                //            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection, trans))
                //            {
                //                myCmd.CommandType = CommandType.Text;
                //                for (int i = 0; i < dt.Rows.Count; i++)
                //                {
                //                            myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN"]);
                //                            myCmd.Parameters.AddWithValue("@process", dt.Rows[i]["Process"]);
                //                            myCmd.Parameters.AddWithValue("@project", dt.Rows[i]["Project"]);
                //                            myCmd.Parameters.AddWithValue("@projectcode", dt.Rows[i]["Project Code"]);
                //                            myCmd.Parameters.AddWithValue("@eventcode", dt.Rows[i]["Event code"]);
                //                            myCmd.Parameters.AddWithValue("@hoursplanned", dt.Rows[i]["Hours planned"]);
                //                            myCmd.Parameters.AddWithValue("@hoursworked", dt.Rows[i]["Hours worked"]);
                //                            myCmd.Parameters.AddWithValue("@Actualproduction", Convert.ToInt32((dt.Rows[i]["Actual Production Records"])));

                //                            myCmd.ExecuteNonQuery();
                //                             trans.Commit();
                //                }

                //            }
                //        }
                //    }
            }
        }




        public ActionResult RemoveEntry()
        {
            return View("RemoveEntry");
        }

        public ActionResult RemoveTLEntry()
        {
            return View("RemoveTeamleadEntry");
        }


        public ActionResult SampleFileupload()
        {
            return View();
        }

        public ActionResult SampleProjectFileupload()
        {
            return View();
        }


        public ActionResult Bulkupload()
        {
            return View();
        }

        public ActionResult SampleConsolidation()
        {
            return View();
        }

        public ActionResult PendingTL()
        {
            return View();
        }
        public ActionResult PendingTLAdmin()
        {
            return View();
        }



        public ActionResult ViewProduction()
        {
            return View();
        }

        public ActionResult ETOEmployeewise()
        {
            return View("ResourcewiseRevenue");
        }

        public ActionResult YearlyProduction()
        {
            return View("YearlyProductionDetails");
        }

        public ActionResult YearlyProductionReport(string Year, string Location, string Clientcode, string ProjectId, string Eventcode, string Process, string TL, string Associate)
        {

            double dollarrate = 0.0;
            SummarySheetModel Model = new SummarySheetModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }
            string[] strArr = null;
            char[] splitchar = { '-' };
            strArr = Year.Split(splitchar);
            var Date1 = strArr[0].Trim().ToString() + "-04-01";
            var Date2 = strArr[1].Trim().ToString() + "-03-31";
            DataTable dtrate = new DataTable();
            string dollarCommand = "SELECT monthname(dollardate) as date, rate FROM dollarsettings where dollardate >='" + Date1 + "' and  dollardate <='" + Date2 + "' group by monthname(dollardate) order by year(dollardate), month(dollardate) ";
            using (MySqlConnection tarConnection = new MySqlConnection(connString))
            {
                tarConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(dollarCommand, tarConnection);
                adapter.Fill(dtrate);
                tarConnection.Close();
            }

          
            
             Command = "select monthname(date) as month, concat (monthname(date),year(date)) as date,  Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),0) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),0) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where date >='" + Date1 + "' and  date <='" + Date2 + "' ";


             if (Clientcode != "ALL")
             {
                 Command = Command + " and `project`='" + Clientcode + "'";
             }

            if (ProjectId != "ALL")
            {
                Command = Command + " and `projectcode`='" + ProjectId + "'";
            }
            if (Eventcode != "ALL")
            {
                Command = Command + " and `eventcode`='" + Eventcode + "'";
            }

            if (Process != "ALL")
            {
                Command = Command + " and `process`='" + Process + "'";
            }

            if (Location != "ALL")
            {
                Command = Command + " and  `location`='" + Location + "'";
            }

            if (TL != "ALL")
            {
                Command = Command + " and  `tlname`='" + TL + "'";
            }
            if (Associate != "ALL")
            {
                Command = Command + " and  `associate`='" + Associate + "'";
            }



            Command = Command + "  group by monthname(date) order by year(date), month(date)";
            
            
            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

                System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                
                dt.Columns.Add(rateColumn);

            }

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    for (int c = 0; c < dt.Columns.Count; c++)
            //    {
            //        if (Equals(dt.Rows[i]["month"], dtrate.Rows[i]["date"]))
            //            dt.Rows[i]["rate"] = dtrate.Rows[i]["rate"];
            //    }
            //}


            var query = from r1 in dt.AsEnumerable()
                        join r2 in dtrate.AsEnumerable() on r1.Field<string>("month") equals r2.Field<string>("date") into r3
                        from r4 in r3.DefaultIfEmpty()
                        select new
                        {
                            month = r1.Field<string>("month"),
                            date = r1.Field<string>("date"),
                            rate = r4 == null ? 0.00 : r4.Field<double>("rate"),
                            hoursplanned = r1.Field<double>("hoursplanned"),
                            prodplanhrrecord = r1.Field<double>("prodplanhrrecord"),
                            prodplanrecords = r1.Field<double>("prodplanrecords"),
                            hoursworked = r1.Field<double>("hoursworked"),
                            Actualprodrecord = r1.Field<decimal>("Actualprodrecord"),
                            Achievement = r1.Field<double>("Achievement"),
                            TarrevenueINR = r1.Field<double>("TarrevenueINR"),
                            ActrevenueINR = r1.Field<double>("ActrevenueINR"),
                            RevAchievement = r1.Field<double>("RevAchievement"),
                            cnt = r1.Field<double>("cnt"),
                           
                           
                          
                        };

            var result = query.ToDataTable();






            string filteration = Year;
            filteration = filteration + "," + Clientcode;
            filteration = filteration + "," + ProjectId;
            filteration = filteration + "," + Eventcode;
            filteration = filteration + "," + Process;
            filteration = filteration + "," + Location;
            filteration = filteration + "," + TL;
            filteration = filteration + "," + Associate;






            Session["yearlyreport"] = filteration;
            TempData["dtyearly"] = dt;
            Model.lstSummarySheetmodel = result.DataTableToList<SummarySheetModel>();
            ViewBag.Yearlist = "Yearly Production Details - " + Year;
            return PartialView("_YearlyPoductionDetailsList", Model);
        }

        public ActionResult MonthlyProduction()
        {
            return View("MonthlyProductionDetails");
        }

        public ActionResult MonthlyProductionReport(string Month, int Year, string Location,string Clientcode,string ProjectId, string Eventcode, string Process,string TL, string Associate)
        {

            double dollarrate = 0.0;
            SummarySheetModel Model = new SummarySheetModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }

            string dollarCommand = "SELECT rate FROM dollarsettings WHERE monthname(dollardate)='" + Month + "' and year(dollardate)=" + Year + "  ORDER BY dollardate desc LIMIT 1";
            using (MySqlConnection tarConnection = new MySqlConnection(connString))
            {
                tarConnection.Open();
                MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dollarrate = reader.GetInt32(0);
                }
                tarConnection.Close();
            }

            string filteration = Month;
            filteration = filteration + "," + Year;
            filteration = filteration + "," + Clientcode;
            filteration = filteration + "," + ProjectId;
            filteration = filteration + "," + Eventcode;
            filteration = filteration + "," + Process;
            filteration = filteration + "," + Location;
            filteration = filteration + "," + TL;
            filteration = filteration + "," + Associate;



            Command = "select   date_format(date, '%d/%m/%Y') as date,location,sum(plannedhrs) as hoursplanned,sum(plannedhrrecord) as prodplanhrrecord,sum(plannedprodrecord) as prodplanrecords,sum(workedhrs) as hoursworked,sum(actualprodrecord) as Actualprodrecord,sum(actualprodrecord)/sum(plannedprodrecord)*100 as Achievement,sum(targetrevenue) as TarrevenueINR,sum(actualrevenue) as ActrevenueINR,(sum(actualrevenue)/sum(targetrevenue)*100) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where monthname(date)='" + Month + "' and year(date)=" + Year + "";


            if (Clientcode != "ALL")
            {
                Command = Command + " and `project`='" + Clientcode + "'";
            }


            if (ProjectId != "ALL")
            {
                Command = Command + " and `projectcode`='" + ProjectId + "'";
            }
            if (Eventcode != "ALL")
            {
                Command = Command + " and `eventcode`='" + Eventcode + "'";
            }

            if (Process != "ALL")
            {
                Command = Command + " and `process`='" + Process + "'";
            }

            if (Location != "ALL")
            {
                Command = Command + " and  `location`='" + Location + "'";
            }

            if (TL != "ALL")
            {
                Command = Command + " and  `tlname`='" + TL + "'";
            }

            if (Associate != "ALL")
            {
                Command = Command + " and  `Associate`='" + Associate + "'";
            }




            Command = Command + "  group by date";
            
            
            
            
            
            
            
            
            
            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);


                System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                rateColumn.DefaultValue = dollarrate;
                dt.Columns.Add(rateColumn);


            }

            TempData["dtmonthly"] = dt;
            Model.lstSummarySheetmodel = dt.DataTableToList<SummarySheetModel>();
            string str = "Monthly Production Details - " + Month + "," + Year;
            if (Location != "ALL")
            {
                str = str + ", " + Location;
            }
            if (Clientcode != "ALL")
            {
                str = str + ", " + Clientcode;
            }
            if (ProjectId != "ALL")
            {
                str = str + ", " + ProjectId;
            }

            if (Eventcode != "ALL")
            {
                str = str + ", " + Eventcode;
            }

            if (Process != "ALL")
            {
                str = str + ", " + Process;
            }
            if (TL != "ALL")
            {
                str = str + ", " + TL;
            }
            if (Associate != "ALL")
            {
                str = str + ", " + Associate;
            }



            Session["monthlyreport"] = filteration;


            ViewBag.Monthlist=str;
            return PartialView("_MonthlyProductionDetailsList", Model);
        }

        public ActionResult MonthlySummaryReport(string Month, int Year, string Location, string Clientcode, string ProjectId, string Eventcode, string Process, string TL, string Associate)
        {

            double dollarrate = 0.0;
            SummarySheetModel Model = new SummarySheetModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }

            string dollarCommand = "SELECT rate FROM dollarsettings WHERE monthname(dollardate)='" + Month + "' and year(dollardate)=" + Year + "  ORDER BY dollardate desc LIMIT 1";
            using (MySqlConnection tarConnection = new MySqlConnection(connString))
            {
                tarConnection.Open();
                MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dollarrate = reader.GetInt32(0);
                }
                tarConnection.Close();
            }


            string filteration = Month;
            filteration = filteration + "," + Year;
            filteration = filteration + "," + Clientcode;
            filteration = filteration + "," + ProjectId;
            filteration = filteration + "," + Eventcode;
            filteration = filteration + "," + Process;
            filteration = filteration + "," + Location;
            filteration = filteration + "," + TL;
            filteration = filteration + "," + Associate;





             Command = "select   location,Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),0) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),0) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where monthname(date)='" + Month + "' and year(date)=" + Year + "";


             if (Clientcode != "ALL")
             {
                 Command = Command + " and `project`='" + Clientcode + "'";
             }


            if (ProjectId != "ALL")
            {
                Command = Command + " and `projectcode`='" + ProjectId + "'";
            }
            if (Eventcode != "ALL")
            {
                Command = Command + " and `eventcode`='" + Eventcode + "'";
            }

            if (Process != "ALL")
            {
                Command = Command + " and `process`='" + Process + "'";
            }

            if (Location != "ALL")
            {
                Command = Command + " and  `location`='" + Location + "'";
            }

            if (TL != "ALL")
            {
                Command = Command + " and  `tlname`='" + TL + "'";
            }

            if (Associate != "ALL")
            {
                Command = Command + " and  `Associate`='" + Associate + "'";
            }

            Command = Command + "  group by location";
            
            
            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);


                System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                rateColumn.DefaultValue = dollarrate;
                dt.Columns.Add(rateColumn);


            }
            TempData["dtsummarymonthly"] = dt;
            Model.lstSummarySheetmodel = dt.DataTableToList<SummarySheetModel>();

            string str = "Monthly Production Summary Details - " + Month + "," + Year;
            if (Location != "ALL")
            {
                str = str + ", " + Location;
            }
            if (Clientcode != "ALL")
            {
                str = str + ", " + Clientcode;
            }
            if (ProjectId != "ALL")
            {
                str = str + ", " + ProjectId;
            }

            if (Eventcode != "ALL")
            {
                str = str + ", " + Eventcode;
            }

            if (Process != "ALL")
            {
                str = str + ", " + Process;
            }
            if (TL != "ALL")
            {
                str = str + ", " + TL;
            }
            if (Associate != "ALL")
            {
                str = str + ", " + Associate;
            }

            ViewBag.Monthlist = str;
            TempData["monthlylist"] = str;
            Session["monthlysummaryreport"] = filteration;
            return PartialView("_MonthlySummaryDetailsList", Model);
        }

        public ActionResult MISRevenueReport()
        {
            Projectmodel Model = new Projectmodel();

            List<SelectListItem> Projectcodes = new List<SelectListItem>();
            List<SelectListItem> Eventcodes = new List<SelectListItem>();
            List<SelectListItem> Clientcodes = new List<SelectListItem>();

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select distinct eventcode from `revenuereport`", con))
                {

                    using (MySqlDataReader sda = cmd.ExecuteReader())
                    {

                        while (sda.Read())
                        {
                            Eventcodes.Add(new SelectListItem
                            {
                                Text = sda["eventcode"].ToString(),
                                Value = sda["eventcode"].ToString()
                            });
                        }


                    }
                }
                con.Close();

            }












            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct projectcode as project from `revenuereport` order by project";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Projectcodes.Add(new SelectListItem
                            {
                                Text = sdr["project"].ToString(),
                                Value = sdr["project"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `Clientcode` from `revenuereport`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Clientcodes.Add(new SelectListItem
                            {
                                Text = sdr["Clientcode"].ToString(),
                                Value = sdr["Clientcode"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }









            ViewBag.Projectcodes = Projectcodes;
            ViewBag.Eventcodes = Eventcodes;
            ViewBag.Clientcodes = Clientcodes;
          
            return View("RevenueDetailReport", Model);



        }


        public ActionResult RevenueDetailedReport(string Location, string projectcode, string sdate, string enddate, string clientcode, string eventcode)
        {



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = string.Empty;
            if (enddate != "")
            {
                DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);


                cdate = dtenddate.ToString("yyyy-MM-dd");
            }


            RevenuelistModel model = new RevenuelistModel();

            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }

            DataTable dt = new DataTable();

            string query = "SELECT   date_format(RR.upldate, '%d/%m/%Y') as upldate,RR.location, RR.projectcode,RR.eventcode,sum(distinct RR.noofbatches) as noofbatches ,COALESCE(sum(distinct RR.invoicedcharacter),0) as invoicedcharacter,COALESCE(sum(PR.actualprodrecord ),0) as actualprodrecord,COALESCE(sum(distinct PC.Totalpromotion),0) as promotionrecord FROM revenuereport RR LEFT JOIN productionreport2020 PR ON RR.projectcode = PR.projectcode AND RR.eventcode = PR.eventcode AND RR.location=PR.location LEFT JOIN promotiontocustomer PC ON PR.projectcode = PC.project AND PR.eventcode = PC.eventcode AND PR.location=PC.location where";

            if (sdate != null && enddate != "")
            {

                query = query + " RR.upldate >='" + stdate + "' AND RR.upldate <='" + cdate + "' ";
            }

            if (sdate != null && enddate == "")
            {

                query = query + "  RR.upldate ='" + stdate + "' ";
            }


            if (projectcode != "ALL")
            {
                query = query + " and RR.projectcode='" + projectcode + "'";
            }

            if (eventcode != "ALL")
            {
                query = query + " and RR.eventcode='" + eventcode + "'";
            }

            if (clientcode != "ALL")
            {
                query = query + " and RR.Clientcode='" + clientcode + "'";
            }

            if (Location != "All")
            {
                query = query + " and RR.location='" + Location + "'";
            }

            query = query + "group by RR.upldate, RR.projectcode,RR.eventcode,RR.location";


            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);

                    }
                }
            }

            model.RevenueModelList = dt.DataTableToList<RevenuelistModel>();
            return PartialView("_RevenueReportList", model);


        }
 







        public ActionResult PromotionReport()
        {
            Projectmodel Model = new Projectmodel();
          
            List<SelectListItem> Projectcodes = new List<SelectListItem>();
            List<SelectListItem> Eventcodes = new List<SelectListItem>();
            List<SelectListItem> Clientcodes = new List<SelectListItem>();

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select distinct eventcode from `promotiontocustomer`", con))
                {

                    using (MySqlDataReader sda = cmd.ExecuteReader())
                    {

                        while (sda.Read())
                        {
                            Eventcodes.Add(new SelectListItem
                            {
                                Text = sda["eventcode"].ToString(),
                                Value = sda["eventcode"].ToString()
                            });
                        }


                    }
                }
                con.Close();

            }



           








            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct project from `promotiontocustomer` order by project";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Projectcodes.Add(new SelectListItem
                            {
                                Text = sdr["project"].ToString(),
                                Value = sdr["project"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `Clientcode` from `promotiontocustomer`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Clientcodes.Add(new SelectListItem
                            {
                                Text = sdr["Clientcode"].ToString(),
                                Value = sdr["Clientcode"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }









            ViewBag.Projectcodes = Projectcodes;
            ViewBag.Eventcodes = Eventcodes;
            ViewBag.Clientcodes = Clientcodes;
            return View("PromotionDetailsReport", Model);



        }


        public ActionResult PromotionDetailsReport(string Location, string projectcode, string sdate, string enddate, string clientcode, string eventcode)
        {



             string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
              DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
              var stdate = dtstartdate.ToString("yyyy-MM-dd");
              var cdate=string.Empty;
              if (enddate != "")
              {
                  DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);


                   cdate = dtenddate.ToString("yyyy-MM-dd");
              }

          
            PromotionDetailModel model = new PromotionDetailModel();

            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }

           DataTable dt = new DataTable();

           string query = "select project,`eventcode`,sum(`noofbatches`) as batches,sum(`Totalpromotion`) as promotion,sum(`Totalpromotion`)*characterrate as revenue,location from `promotiontocustomer` where "; 

            if(sdate!=null && enddate!="") 
            {

                query = query + " proddate >='" + stdate + "' AND proddate <='" + cdate + "' ";
            }

            if (sdate != null && enddate == "")
            {

                query = query + "  proddate ='" + stdate + "' ";
            }

            
            if (projectcode != "ALL")
            {
                query = query + " and `project`='" + projectcode + "'";
            }
            if (clientcode != "ALL")
            {
                query = query + " and `Clientcode`='" + clientcode + "'";
            }

            if (Location != "All")
            {
                query = query + " and `location`='" + Location + "'";
            }

            if (eventcode != "ALL")
            {
                query = query + " and `eventcode`='" + eventcode + "'";
            }

           query=query + "group by project,eventcode,location order by proddate desc;";


            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);

                    }
                }
            }

            model.lstpromotionDetail = dt.DataTableToList<PromotionDetailModel>();
            return PartialView("_PromotionReportList", model);


        }
        #region ConsolidatedReport



        [HttpPost]
        public ActionResult GetEventsForMonth(int month, int year)
        {
            //Need to return string as key (representing the day of the month) as:
            // 1. Cannot serialize a dictionary with an int key, only object or string.
            // 2. Javascript (the intended recipient) uses strings for "associative" arrays.
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT DAY(`productionreport2020`.`date`) FROM `productionreport2020` WHERE MONTH(date)=" + month + " and Year(date)=" + year + "";
            // using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
            Dictionary<string, string> events = new Dictionary<string, string>();
            using (MySqlConnection mmConnection = new MySqlConnection(connString))
            {
                mmConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(Command, mmConnection))
                {
                    MySqlDataReader reader = myCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (!events.ContainsKey(reader.GetString(0)))
                        {
                            events.Add((reader.GetString(0)).ToString(), string.Format("event on day {0}/{1}/{2}", reader.GetString(0), month, year));

                        }
                    }


                }
            }
            Thread.Sleep(500); //Simulate a database delay.
            return Json(events);





        }


        public ActionResult BindLastproddate()
        {
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //DateTime d=new DateTime();
            string d = string.Empty;
            DateTime t = new DateTime();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = "SELECT MAX(date) as proddate FROM productionreport2020 ";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                       if (sdr.Read())
                        {
                            t = Convert.ToDateTime(sdr["proddate"]);
                         d =t.Year + ", " + t.Month + ", " + t.Day; 
                            //d = "2012, 3, 10";
                        }
                    }
                   
                    mConnection.Close();
                }
            }

            return Json(d);
        }


        public ActionResult ConsolidatedReport()
        {

            PecodeModel Model = new PecodeModel();
            List<SelectListItem> ClientCodes = new List<SelectListItem>();
            List<SelectListItem> ProjectCodes = new List<SelectListItem>();
            List<SelectListItem> EventCodes = new List<SelectListItem>();
            List<SelectListItem> MProjectCodes = new List<SelectListItem>();
            List<SelectListItem> MClientCodes = new List<SelectListItem>();
            List<SelectListItem> MEventCodes = new List<SelectListItem>();
            List<SelectListItem> YProjectCodes = new List<SelectListItem>();
            List<SelectListItem> YClientCodes = new List<SelectListItem>();
            List<SelectListItem> YEventCodes = new List<SelectListItem>();
            string month= DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `project` from `productionreport2020` where year(date)=" + int.Parse(year) + " and month(date)=" + int.Parse(month) + "   and project is not null and projectcode is not null order by `project`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ClientCodes.Add(new SelectListItem
                            {
                                Text = sdr["project"].ToString(),
                                Value = sdr["project"].ToString()
                            });
                        }
                    }
                    // PeCodes.Add(new SelectListItem() { Value = "-1", Text = "ALL" });
                    mConnection.Close();
                }
            }





            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `projectcode` from `productionreport2020` where year(date)=" + int.Parse(year) + " and month(date)=" + int.Parse(month) + " and projectcode is not null   order by `projectcode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ProjectCodes.Add(new SelectListItem
                            {
                                Text = sdr["projectcode"].ToString(),
                                Value = sdr["projectcode"].ToString()
                            });
                        }
                    }
                    
                    mConnection.Close();
                }
            }

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `eventcode` from `productionreport2020` where year(date)=" + int.Parse(year) + " and month(date)=" + int.Parse(month) + " and eventcode is not null  order by `eventcode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            EventCodes.Add(new SelectListItem
                            {
                                Text = sdr["eventcode"].ToString(),
                                Value = sdr["eventcode"].ToString()
                            });
                        }
                    }
                  
                    mConnection.Close();
                }
            }


            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `project` from `productionreport2020` where  project is not null  and project<>' ' order by `project`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            MClientCodes.Add(new SelectListItem
                            {
                                Text = sdr["project"].ToString(),
                                Value = sdr["project"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }



            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `projectcode` from `productionreport2020` where  projectcode  is not null    order by `projectcode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            MProjectCodes.Add(new SelectListItem
                            {
                                Text = sdr["projectcode"].ToString(),
                                Value = sdr["projectcode"].ToString()
                            });
                        }
                    }
                   
                    mConnection.Close();
                }
            }

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `eventcode` from `productionreport2020` where  eventcode is not null  order by `eventcode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            MEventCodes.Add(new SelectListItem
                            {
                                Text = sdr["eventcode"].ToString(),
                                Value = sdr["eventcode"].ToString()
                            });
                        }
                    }
                    
                    mConnection.Close();
                }
            }



            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `project` from `productionreport2020` where  projectcode is not null and project is not null  order by `projectcode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            YClientCodes.Add(new SelectListItem
                            {
                                Text = sdr["project"].ToString(),
                                Value = sdr["project"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }



            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `projectcode` from `productionreport2020` where  projectcode is not null order by `projectcode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            YProjectCodes.Add(new SelectListItem
                            {
                                Text = sdr["projectcode"].ToString(),
                                Value = sdr["projectcode"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `eventcode` from `productionreport2020` where  eventcode is not null order by `eventcode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            YEventCodes.Add(new SelectListItem
                            {
                                Text = sdr["eventcode"].ToString(),
                                Value = sdr["eventcode"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }









            ViewBag.ProjectCodes = ProjectCodes;

            ViewBag.EventCodes = EventCodes;
            ViewBag.ClientCodes = ClientCodes;

            ViewBag.MProjectCodes = MProjectCodes;

            ViewBag.MEventCodes = MEventCodes;

            ViewBag.MClientCodes = MClientCodes;

            ViewBag.YProjectCodes = YProjectCodes;

            ViewBag.YEventCodes = YEventCodes;

            ViewBag.YClientCodes = YClientCodes;



            return View("ConsolidatedReport", Model);
        }




        public ActionResult BindMonthlyAssociate(string Location, string Month, string Year)
        {

            string query = string.Empty;
          
           

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct associate  from productionreport2020 where  project is not null and project<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (Month != null )
            {
                query = query + " and monthname(date)='" + Month + "' ";
            }

            if (Year != "Select")
            {
                query = query + " and year(date)=" + int.Parse(Year) + "";
            }

            query = query + "  order by project";
            List<Resource> objproject = new List<Resource>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Resource>();
            SelectList obgproject = new SelectList(objproject, "associate", "associate", 0);
            return Json(obgproject);



        }

        public ActionResult BindMonthlyClientbyLocation(string Location, string Month,string Year)
        {

            string query = string.Empty;
          
           

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct project  from productionreport2020 where  project is not null and project<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (Month != null )
            {
                query = query + " and monthname(date)='" + Month + "' ";
            }

            if (Year != "Select")
            {
                query = query + " and year(date)=" + int.Parse(Year) + "";
            }

            query = query + "  order by project";
            List<Client> objproject = new List<Client>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Client>();
            SelectList obgproject = new SelectList(objproject, "project", "project", 0);
            return Json(obgproject);



        }

        public ActionResult BindyearClientbyLocation(string Location, string year)
        {

            string query = string.Empty;
           

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct project  from productionreport2020 where  project is not null and project<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (year != "Select")
            {
                if (year != null && year != "")
                {
                    string[] strArr = null;
                    char[] splitchar = { '-' };
                    strArr = year.Split(splitchar);
                    query = query + " and year(date)=" + strArr[0] + "";

                }
            }

            query = query + "  order by project";
            List<Client> objproject = new List<Client>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Client>();
            SelectList obgproject = new SelectList(objproject, "project", "project", 0);
            return Json(obgproject);



        }

        public ActionResult BindClientbyLocationfromtoDate(string fromdate,string enddate,string Location)
        {

            string query = string.Empty;
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endsdate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtstartdate = new DateTime();
            DateTime dtenddate = new DateTime();
            if (fromdate != null && fromdate != "")
            {
                dtstartdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtstartdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endsdate = dtenddate.ToString("yyyy-MM-dd");
            }



            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct project  from productionreport2020 where  project is not null and project<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }

            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endsdate + "'";
            }

            query = query + "  order by project";
            List<Client> objproject = new List<Client>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Client>();
            SelectList obgproject = new SelectList(objproject, "project", "project", 0);
            return Json(obgproject);



        }






        public ActionResult BindClientbyLocation(string Location, string fromdate)
        {

            string query = string.Empty;
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtdate = new DateTime();
            if (fromdate != null && fromdate != "")
            {
                dtdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date = dtdate.ToString("yyyy-MM-dd");
            }

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct project  from productionreport2020 where  project is not null and project<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date='" + date + "'";
            }

            query = query + "  order by project";
            List<Client> objproject = new List<Client>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Client>();
            SelectList obgproject = new SelectList(objproject, "project", "project", 0);
            return Json(obgproject);



        }

        public ActionResult BindTLbyProject(string Location, string Clientcode, string Projectcode)
        {

            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (Projectcode != "ALL")
            {
                query = query + " and projectcode='" + Projectcode + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }
           


            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }

        public ActionResult BindyearTLbyProject(string Location, string Clientcode, string Projectcode, string Year)
        {

            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (Projectcode != "ALL")
            {
                query = query + " and projectcode='" + Projectcode + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

           

            if (Year != "Select")
            {
               
                string[] strArr = null;
                char[] splitchar = { '-' };
                strArr = Year.Split(splitchar);
                query = query + " and year(date)=" + strArr[0] + "";
            }

            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }






        public ActionResult BindMonthTLbyProject(string Location, string Clientcode, string Projectcode, string Month, string Year)
        {

            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (Projectcode != "ALL")
            {
                query = query + " and projectcode='" + Projectcode + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (Month != "Select")
            {
                query = query + " and monthname(date)='" + Month + "'";
            }

            if (Year != "Select")
            {
                query = query + " and year(date)='" + Year + "'";
            }

            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }

        public ActionResult BindTLbyEvent(string Location, string Clientcode, string Projectcode, string Eventcode, string fromdate)
        {

            string query = string.Empty;
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtdate = new DateTime();
            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (Projectcode != "ALL")
            {
                query = query + " and projectcode='" + Projectcode + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }
            if (Eventcode != "ALL")
            {
                query = query + " and eventcode='" + Eventcode + "'";
            }
            if (fromdate != null && fromdate != "")
            {
                dtdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date = dtdate.ToString("yyyy-MM-dd");
                query = query + " and productionreport2020.date='" + date + "'";
            }
          
            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }

        public ActionResult BindMonthTLbyEvent(string Location, string Clientcode, string Projectcode, string Eventcode,string Month,string Year)
        {

            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (Projectcode != "ALL")
            {
                query = query + " and projectcode='" + Projectcode + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }
            if (Eventcode != "ALL")
            {
                query = query + " and eventcode='" + Eventcode + "'";
            }

            if (Month != "Select")
            {
                query = query + " and monthname(date)='" + Month + "'";
            }

            if (Year != "Select")
            {
                query = query + " and year(date)='" + Year + "'";
            }

            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }




        public ActionResult BindTLbyProcess(string Location, string Clientcode, string Projectcode, string Eventcode,string Process,string fromdate)
        {

            string query = string.Empty;
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtdate = new DateTime();
            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (Projectcode != "ALL")
            {
                query = query + " and projectcode='" + Projectcode + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }
            if (Eventcode != "ALL")
            {
                query = query + " and eventcode='" + Eventcode + "'";
            }

            if (Process != "ALL")
            {
                query = query + " and process='" + Process + "'";
            }
             if (fromdate != null && fromdate != "")
            {
                dtdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date = dtdate.ToString("yyyy-MM-dd");
                query = query + " and productionreport2020.date='" + date + "'";
            }
            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }


        public ActionResult BindMonthTLbyProcess(string Location, string Clientcode, string Projectcode, string Eventcode,string Process,string Month,string Year)
        {

            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (Projectcode != "ALL")
            {
                query = query + " and projectcode='" + Projectcode + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }
            if (Eventcode != "ALL")
            {
                query = query + " and eventcode='" + Eventcode + "'";
            }

            if (Process != "ALL")
            {
                query = query + " and process='" + Process + "'";
            }

            if (Month != "Select")
            {
                query = query + " and monthname(date)='" + Month + "'";
            }

            if (Year != "Select")
            {
                query = query + " and year(date)='" + Year + "'";
            }


            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }

       





        public ActionResult BindMonthlyTLbyLocation(string Location,string Month, string Year)
        {

            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (Month != "Select")
            {
                query = query + " and monthname(date)='" + Month + "'";
            }

            if (Year != "Select")
            {
                query = query + " and year(date)='" + Year + "'";
            }

            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }



        public ActionResult BindDailyProjectbyLocation(string Location, string fromdate)
        {

            string query = string.Empty;
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtdate = new DateTime();
            



            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct projectcode  from productionreport2020 where  projectcode is not null and projectcode<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (fromdate != null && fromdate != "")
            {
                dtdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date = dtdate.ToString("yyyy-MM-dd");
                query = query + " and productionreport2020.date='" + date + "'";
            }




            query = query + "  order by projectcode";
            List<Project> objtl = new List<Project>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<Project>();
            SelectList obgtl = new SelectList(objtl, "projectcode", "projectcode", 0);
            return Json(obgtl);



        }





        public ActionResult BindTLbyLocationDate(string startDate,string Location)
        {

            string query = string.Empty;
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtdate = new DateTime();
            



            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (startDate != null && startDate != "")
            {
                dtdate = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date = dtdate.ToString("yyyy-MM-dd");
                query = query + " and productionreport2020.date='" + date + "'";
            }




            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }


        public ActionResult BindTLbyLocation(string Location)
        {

            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct tlname  from productionreport2020 where  tlname is not null and tlname<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            



            query = query + "  order by tlname";
            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);



        }



        public ActionResult BindYearlyTLbyResource(string TL, string year, string Location)
        {

            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct associate  from productionreport2020 where  associate is not null and associate<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

          

            if (year != "Select")
            {
                string[] strArr = null;
                if (year != "Select")
                {

                    char[] splitchar = { '-' };
                    strArr = year.Split(splitchar);
                }
                query = query + " and year(date)='" + strArr[0] + "'";
            }


            if (TL != "ALL")
            {
                query = query + " and tlname='" + TL + "'";
            }

            query = query + "  order by associate";
            List<Resource> objresource = new List<Resource>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objresource = dt.DataTableToList<Resource>();
            SelectList obgtl = new SelectList(objresource, "associate", "associate", 0);
            return Json(obgtl);



        }




        public ActionResult BindMonthlyTLbyResource(string TL,string Location,string Clientcode,string Projectcode, string Eventcode,string Process, string Month, string Year)
        {

            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct associate  from productionreport2020 where  associate is not null and associate<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (Month != "Select")
            {
                query = query + " and monthname(date)='" + Month + "'";
            }

            if (Year != "Select")
            {
                query = query + " and year(date)='" + Year + "'";
            }


            if (TL != "ALL")
            {
                query = query + " and tlname='" + TL + "'";
            }

            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (Projectcode != "ALL")
            {
                query = query + " and projectcode='" + Projectcode + "'";
            }

            if (Eventcode != "ALL")
            {
                query = query + " and eventcode='" + Eventcode + "'";
            }

            if (Process != "ALL")
            {
                query = query + " and process='" + Process + "'";
            }


            query = query + "  order by associate";
            List<Resource> objresource = new List<Resource>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objresource = dt.DataTableToList<Resource>();
            SelectList obgtl = new SelectList(objresource, "associate", "associate", 0);
            return Json(obgtl);



        }









        public ActionResult BindTLbyResource(string tl, string fromdate, string Location, string ProjectId, string Clientcode, string Eventcode, string Process)
        {




            var date = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtdate = new DateTime();
            string query = string.Empty;

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct associate  from productionreport2020 where  associate is not null and associate<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (fromdate != null && fromdate != "")
            {
                dtdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date = dtdate.ToString("yyyy-MM-dd");
                query = query + " and productionreport2020.date='" + date + "'";
            }


            if (tl != "ALL")
            {
                query = query + " and tlname='" + tl + "'";
            }

            if (ProjectId != "ALL")
            {
                query = query + " and `projectcode`='" + ProjectId + "'";
            }

            if (Clientcode != "ALL")
            {
                query = query + " and `project`='" + Clientcode + "'";
            }
            if (Eventcode != "ALL")
            {
                query = query + " and `eventcode`='" + Eventcode + "'";
            }
            if (Process != "ALL")
            {
                query = query + " and `process`='" + Process + "'";
            }

            query = query + "  order by associate";
            List<Resource> objresource = new List<Resource>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objresource = dt.DataTableToList<Resource>();
            SelectList obgtl = new SelectList(objresource, "associate", "associate", 0);
            return Json(obgtl);



        }


        public ActionResult BindProjectdaily(string Clientcode, string fromdate, string Location)
        {

            string query = string.Empty;
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtdate = new DateTime();
            if (fromdate != null && fromdate!="")
            {
                dtdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date = dtdate.ToString("yyyy-MM-dd");
            }


            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct projectcode  from productionreport2020 where  projectcode is not null and projectcode<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (fromdate != null && fromdate!="")
            {
                query = query + " and date='" + date + "'";
            }

            
            query = query + "  order by project";
            List<Project> objproject = new List<Project>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Project>();
            SelectList obgproject = new SelectList(objproject, "projectcode", "projectcode", 0);
            return Json(obgproject);



        }



       






        public ActionResult DailyReport(string date, string LocationId, string Clientcode, string ProjectId, string Eventcode, string Process, string TL, String Resource)
        {

            SummarySheetModel Model = new SummarySheetModel();
            DataTable dt = new DataTable();
            string pdate = string.Empty;
            double dollarrate = 0.0;
            try
            {

                DateTime dtdate = new DateTime();
                dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var ddate = dtdate.ToString("yyyy-MM-dd");
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

                if (LocationId == "KAKKANAD")
                {
                    LocationId = "KKND";
                }

                string dollarCommand = "SELECT rate FROM dollarsettings WHERE dollardate <='" + ddate + "' ORDER BY dollardate desc LIMIT 1";
                using (MySqlConnection tarConnection = new MySqlConnection(connString))
                {
                    tarConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        dollarrate = reader.GetInt32(0);
                    }
                    tarConnection.Close();
                }

                string llocation = string.Empty;
                string locCommand = "SELECT location FROM Holiday where `holidaydate`='" + ddate + "'";
                using (MySqlConnection locConnection = new MySqlConnection(connString))
                {
                    locConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(locCommand, locConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        llocation = llocation + "," + (reader.GetString(0));

                    }
                    if (llocation.Length > 0)
                    {
                        llocation = llocation.Remove(0, 1);
                        ViewBag.llocation = llocation;
                    }
                }




                string Command = string.Empty;
                Command = "select   date_format(date, '%d/%m/%Y') as date,location,Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),0) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),0) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where date='" + ddate + "'";

                if (Clientcode != "ALL")
                {
                    Command = Command + " and `project`='" + Clientcode + "'";
                }

                if (ProjectId != "ALL")
                {
                    Command = Command + " and `projectcode`='" + ProjectId + "'";
                }
                if (Eventcode != "ALL")
                {
                    Command = Command + " and `eventcode`='" + Eventcode + "'";
                }

                if (Process != "ALL")
                {
                    Command = Command + " and `process`='" + Process + "'";
                }

                if (LocationId != "ALL")
                {
                    Command = Command + " and  `location`='" + LocationId + "'";
                }

                if (TL != "ALL")
                {
                    Command = Command + " and  `tlname`='" + TL + "'";
                }

                if (Resource != "ALL")
                {
                    Command = Command + " and  `associate`='" + Resource + "'";
                }



                Command = Command + "  group by location";



                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                    adapter.Fill(dt);


                    System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                    rateColumn.DefaultValue = dollarrate;
                    dt.Columns.Add(rateColumn);


                }
                Session["dtdaily"] = dt;
                Model.lstSummarySheetmodel = dt.DataTableToList<SummarySheetModel>();
                string filteration = date;
                filteration = filteration+","+LocationId;
                filteration = filteration+","+Clientcode;
                filteration = filteration+","+ProjectId;
                filteration = filteration+","+Eventcode;
                filteration = filteration+","+Process;
                filteration = filteration+","+TL;
                filteration = filteration+","+Resource;
                 string str = "Daily Production  Details - " + date;
                 if (LocationId != "ALL")
                 {
                     str = str + ", " + LocationId;
                    
                 }
                 if (Clientcode != "ALL")
                 {
                     str = str + ", " + Clientcode;
                    
                 }
                 if (ProjectId != "ALL")
                 {
                     str = str + ", " + ProjectId;
                    
                 }
                
                 if (Eventcode != "ALL")
                 {
                     str = str + ", " + Eventcode;
                     
                 }

                 if (Process != "ALL")
                 {
                     str = str + ", " + Process;
                    
                 }
                 if (TL != "ALL")
                 {
                     str = str + ", " + TL;
                    
                 }
                 if (Resource != "ALL")
                 {
                     str = str + ", " + Resource;
                  
                 }
                 Session["dailyreport"] = filteration;
                 ViewBag.Daylist = str;
                 TempData["dailylist"] = str;
                return PartialView("/Views/Admin/_Dailyreport.cshtml", Model);

            }

            catch (Exception ex)
            {

                Model.lstSummarySheetmodel = null;
                return PartialView("/Views/Admin/_Dailyreport.cshtml", Model);

            }
        }


        [HttpPost]
        public ActionResult BindProjectcode(string Clientcode, string fromdate)
        {


            string query = string.Empty;
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtdate = new DateTime();
            if (fromdate != null)
            {
                dtdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date = dtdate.ToString("yyyy-MM-dd");
            }
            if (Clientcode == "ALL")
            {
                query = "select distinct projectcode from productionreport2020 where projectcode is not null and  productionreport2020.`date`='" + date + "'";
            }
            else if (Clientcode == "")
            {
                query = "select distinct projectcode from productionreport2020  where projectcode is not null and productionreport2020.`date`='" + date + "'";
            }

            else
            {
                query = "select distinct projectcode from productionreport2020  where projectcode is not null and project='" + Clientcode + "' and productionreport2020.`date`='" + date + "';";
            }

            List<Project> objproject = new List<Project>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Project>();
            SelectList obgproject = new SelectList(objproject, "projectcode", "projectcode", 0);
            return Json(obgproject);
        }



        public ActionResult ProjectwiseDailyReport(string date, string LocationId, string Clientcode, string ProjectId, string Eventcode, string Process, string TL, string Resource)
        {

            SummarySheetModel Model = new SummarySheetModel();
            DataTable dt = new DataTable();
            
            double dollarrate = 0.0;
            try
            {

                DateTime dtdate = new DateTime();
                dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var ddate = dtdate.ToString("yyyy-MM-dd");
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


                string dollarCommand = "SELECT rate FROM dollarsettings WHERE dollardate <='" + ddate + "' ORDER BY dollardate desc LIMIT 1";
                using (MySqlConnection tarConnection = new MySqlConnection(connString))
                {
                    tarConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        dollarrate = reader.GetInt32(0);
                    }
                    tarConnection.Close();
                }




                //Model.ETO = ETO;
                string Command = string.Empty;
                Command = "select  `projectcode`, Eventcode,process,Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),2) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),0) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where `projectcode` is not null and `projectcode`<>' ' and date='" + ddate + "' and location='" + LocationId + "'";

                if (Clientcode != "ALL")
                {
                    Command = Command + " and `project`='" + Clientcode + "'";
                }

                if (ProjectId != "ALL")
                {
                    Command = Command + " and `projectcode`='" + ProjectId + "'";
                }
                if (Eventcode != "ALL")
                {
                    Command = Command + " and `eventcode`='" + Eventcode + "'";
                }

                if (Process != "ALL")
                {
                    Command = Command + " and `process`='" + Process + "'";
                }

                if (LocationId != "ALL")
                {
                    Command = Command + " and  `location`='" + LocationId + "'";
                }

                if (TL != "ALL")
                {
                    Command = Command + " and  `tlname`='" + TL + "'";
                }

                if (Resource != "ALL")
                {
                    Command = Command + " and  `associate`='" + Resource + "'";
                }



                Command = Command + "  group by projectcode,eventcode,process";



                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                    adapter.Fill(dt);


                    System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                    rateColumn.DefaultValue = dollarrate;
                    dt.Columns.Add(rateColumn);


                }
               
                Model.lstSummarySheetmodel = dt.DataTableToList<SummarySheetModel>();
                return PartialView("/Views/Admin/_projectlocationreport.cshtml", Model);

            }

            catch (Exception ex)
            {

                Model.lstSummarySheetmodel = null;
                return PartialView("/Views/Admin/_projectlocationreport.cshtml", Model);

            }
        }


        public ActionResult ProjectwiseMonthlyReport(string Month, string Year, string Location, string Clientcode, string ProjectId, string Eventcode, string Process, string TL, string Associate)
        {

            SummarySheetModel Model = new SummarySheetModel();
            DataTable dt = new DataTable();

            double dollarrate = 0.0;
            try
            {

              
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                string dollarCommand = "SELECT rate FROM dollarsettings WHERE monthname(dollardate)='" + Month + "' and year(dollardate)=" + Year + "  ORDER BY dollardate desc LIMIT 1";
                using (MySqlConnection tarConnection = new MySqlConnection(connString))
                {
                    tarConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(dollarCommand, tarConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        dollarrate = reader.GetInt32(0);
                    }
                    tarConnection.Close();
                }





                string Command = string.Empty;
                Command = "select  `projectcode`, Eventcode,process,Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),2) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),0) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,Round(sum(workedhrs)/8) as cnt from productionreport2020 where `projectcode` is not null and `projectcode`<>' ' and monthname(date)='" + Month + "' and year(date)=" + Year + "";

                if (Clientcode != "ALL")
                {
                    Command = Command + " and `project`='" + Clientcode + "'";
                }

                if (ProjectId != "ALL")
                {
                    Command = Command + " and `projectcode`='" + ProjectId + "'";
                }
                if (Eventcode != "ALL")
                {
                    Command = Command + " and `eventcode`='" + Eventcode + "'";
                }

                if (Process != "ALL")
                {
                    Command = Command + " and `process`='" + Process + "'";
                }

                if (Location != "ALL")
                {
                    Command = Command + " and  `location`='" + Location + "'";
                }

                if (TL != "ALL")
                {
                    Command = Command + " and  `tlname`='" + TL + "'";
                }

                if (Associate != "ALL")
                {
                    Command = Command + " and  `associate`='" + Associate + "'";
                }



                Command = Command + "  group by  projectcode,eventcode,process";



                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                    adapter.Fill(dt);


                    System.Data.DataColumn rateColumn = new System.Data.DataColumn("rate", typeof(System.Double));
                    rateColumn.DefaultValue = dollarrate;
                    dt.Columns.Add(rateColumn);


                }

                Model.lstSummarySheetmodel = dt.DataTableToList<SummarySheetModel>();
                return PartialView("/Views/Admin/_projectmonthlylocationreport.cshtml", Model);

            }

            catch (Exception ex)
            {

                Model.lstSummarySheetmodel = null;
                return PartialView("/Views/Admin/_projectmonthlylocationreport.cshtml", Model);

            }
        }










        public string GetArrayStream()
        {
            
            StringBuilder str = new StringBuilder();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string arrayCommand = "SELECT date FROM productionreport2020 WHERE year(date)=2020 and month(date)>5 ORDER BY date";
            using (MySqlConnection arrayConnection = new MySqlConnection(connString))
            {
                arrayConnection.Open();
                MySqlCommand cmd = new MySqlCommand(arrayCommand, arrayConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while ((reader.Read()))
                {
                     if (reader.HasRows)
                    {
                       
                        str.Append(Convert.ToDateTime(reader["date"]).ToString("d-M-yyyy"));
                    }
                     str.Append("|");
                }
                 str.Remove(str.Length - 1, 1);
                 string ss = str.ToString();
                 string[] myArray = str.ToString().Split(',');
                 return str.ToString();
            }
        }
        
        
        #endregion


        #region projectwiseProductivity

        public ActionResult BindClientbyLocationinProductivity(string Location, string fromdate,string enddate)
        {

            string query = string.Empty;
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endddate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtsdate = new DateTime();
            DateTime dtedate = new DateTime();
            if (fromdate != null && fromdate != "")
            {
                dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtsdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endddate = dtedate.ToString("yyyy-MM-dd");
            }
            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct project  from productionreport2020 where  project is not null and project<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }
            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endddate + "'";
            }
            query = query + "  order by project";
            List<Client> objproject = new List<Client>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Client>();
            SelectList obgproject = new SelectList(objproject, "project", "project", 0);
            return Json(obgproject);



        }


        [HttpPost]
        public ActionResult BindEventHighlow( string fromdate, string enddate, string Clientcode,string ProjectId,string Location)
        {
            List<Event> objevent = new List<Event>();
            DataTable dt = new DataTable();
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endddate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtsdate = new DateTime();
            DateTime dtedate = new DateTime();
            if (Location == "KAKKANAD")
                Location = "KKND";

            if (fromdate != null && fromdate != "")
            {
                dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtsdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endddate = dtedate.ToString("yyyy-MM-dd");
            }
            string query = string.Empty;

            query = "select distinct eventcode  from productionreport2020 where  project is not null and project<>' '";
            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }
            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endddate + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (ProjectId != "ALL")
            {
                query = query + " and projectcode='" + ProjectId + "'";
            }
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }

            objevent = dt.DataTableToList<Event>();
            SelectList obgevent = new SelectList(objevent, "eventcode", "eventcode", 0);
            return Json(obgevent);
        }
        




        

         [HttpPost]
        public ActionResult BindprojectproductivityEvent(string ProjectId, string fromdate, string enddate, string Clientcode)
        {
            List<Event> objevent = new List<Event>();
            DataTable dt = new DataTable();
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endddate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtsdate = new DateTime();
            DateTime dtedate = new DateTime();
            if (fromdate != null && fromdate != "")
            {
                dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtsdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endddate = dtedate.ToString("yyyy-MM-dd");
            }
            string query = string.Empty;
           
            query = "select distinct eventcode  from productionreport2020 where  project is not null and project<>' '";
            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }
            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endddate + "'";
            }
            if (ProjectId != "ALL")
            {
                query = query + " and projectcode='" + ProjectId + "'";
            }
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }

            objevent = dt.DataTableToList<Event>();
            SelectList obgevent = new SelectList(objevent, "eventcode", "eventcode", 0);
            return Json(obgevent);
        }
        




        [HttpPost]
         public ActionResult BindProjectProductivity(string Clientcode, string fromdate, string enddate, string Location)
        {
            List<Project> objproject = new List<Project>();
            DataTable dt = new DataTable();
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endddate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtsdate = new DateTime();
            DateTime dtedate = new DateTime();
            if (Location == "KAKKANAD")
                Location = "KKND";
            if (fromdate != null && fromdate != "")
            {
                dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtsdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endddate = dtedate.ToString("yyyy-MM-dd");
            }
            string query = string.Empty;

            query = "select distinct projectcode  from productionreport2020 where  project is not null and project<>' '";
            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }
            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endddate + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }

            objproject = dt.DataTableToList<Project>();
            SelectList obgproject = new SelectList(objproject, "Id", "Projectcode", 0);
            return Json(obgproject);
        }
        



        #endregion



        #region resourcewiseProductivity

        [HttpPost]
        public ActionResult BindTLResourcePage(string LocationId, string fromdate, string enddate)
        {
            string query = string.Empty;
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endddate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtsdate = new DateTime();
            DateTime dtedate = new DateTime();
            if (fromdate != null && fromdate != "")
            {
                dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtsdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endddate = dtedate.ToString("yyyy-MM-dd");
            }

            if (LocationId == "KAKKANAD")
                LocationId = "KKND";


            query = "select distinct `tlname`  from productionreport2020 where tlname is not null and tlname<>' '  ";
            if (LocationId != "ALL")
            {
                query = query + "  and location='" + LocationId + "'" ;
            }
            
          


            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }
            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endddate + "'";
            }

            query = query + " order by `tlname`";

            List<TL> objtl = new List<TL>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objtl = dt.DataTableToList<TL>();
            SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
            return Json(obgtl);
        }

        [HttpPost]
        public ActionResult BindNamePSNResourcePage(string TL,  string fromdate, string enddate,string Location)
        {
            string query = string.Empty;
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endddate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtsdate = new DateTime();
            DateTime dtedate = new DateTime();
            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }
            if (fromdate != null && fromdate != "")
            {
                dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtsdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endddate = dtedate.ToString("yyyy-MM-dd");
            }


            query = "select distinct `psn`,`associate`  from productionreport2020 where  associate is not null and associate<>' '";


            if (TL != "ALL")
            {
                query = query + " and tlname='" + TL + "'";
            }
            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }
            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }
            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endddate + "'";
            }

            query = query + "  order by associate";
            List<Resource> objresource = new List<Resource>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objresource = dt.DataTableToList<Resource>();
            SelectList obgresource = new SelectList(objresource, "psn", "associate", 0);
            return Json(obgresource);
        }

        

         [HttpPost]
        public ActionResult BindResourcePageClientcode(string LocationId, string fromdate, string enddate,string TL)
        {
            string query = string.Empty;
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endddate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtsdate = new DateTime();
            DateTime dtedate = new DateTime();
            if (LocationId == "KAKKANAD")
            {
                LocationId = "KKND";
            }

            if (fromdate != null && fromdate != "")
            {
                dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtsdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endddate = dtedate.ToString("yyyy-MM-dd");
            }


            query = "select distinct project  from productionreport2020 where  project is not null and project<>' '";


          
            if (LocationId != "ALL")
            {
                query = query + " and location='" + LocationId + "'";
            }
            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }
            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endddate + "'";
            }
            if (TL != "ALL")
            {
                query = query + " and tlname='" + TL + "'";
            }


            query = query + "  order by project";
            List<Client> objresource = new List<Client>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objresource = dt.DataTableToList<Client>();
            SelectList obgresource = new SelectList(objresource, "project", "project", 0);
            return Json(obgresource);
        }

        







         [HttpPost]
         public ActionResult BindResourcewiseProjectcode(string Clientcode, string fromdate, string enddate,string Location,string TL)
        {
            List<Project> objproject = new List<Project>();
            DataTable dt = new DataTable();
            var startdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endddate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtsdate = new DateTime();
            DateTime dtedate = new DateTime();
            if (fromdate != null && fromdate != "")
            {
                dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                startdate = dtsdate.ToString("yyyy-MM-dd");
            }
            if (enddate != null && enddate != "")
            {
                dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endddate = dtedate.ToString("yyyy-MM-dd");
            }
            string query = string.Empty;

            query = "select distinct projectcode  from productionreport2020 where  project is not null and project<>' '";
            if (Clientcode != "ALL")
            {
                query = query + " and project='" + Clientcode + "'";
            }
            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + startdate + "'";
            }
            if (enddate != null && enddate != "")
            {
                query = query + "  and date<='" + endddate + "'";
            }

            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }
            if (TL != "ALL")
            {
                query = query + " and tlname='" + TL + "'";
            }
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }

            objproject = dt.DataTableToList<Project>();
            SelectList obgproject = new SelectList(objproject, "Id", "Projectcode", 0);
            return Json(obgproject);
        }



         [HttpPost]
         public ActionResult GetClientcodebyResource(string Location, string fromdate, string enddate, string TL,string Resource )
         {
             string ooutput=string.Empty;

             if (Location == "KAKKANAD")
                 Location = "KKND";
             
             if (Resource!="ALL")
             ooutput = Resource.Split('[', ']')[1];
            
             
             List<Client> objproject = new List<Client>();
             DataTable dt = new DataTable();
             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             var endddate = DateTime.Now.ToString("yyyy-MM-dd");
             DateTime dtsdate = new DateTime();
             DateTime dtedate = new DateTime();
             if (fromdate != null && fromdate != "")
             {
                 dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtsdate.ToString("yyyy-MM-dd");
             }
             if (enddate != null && enddate != "")
             {
                 dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 endddate = dtedate.ToString("yyyy-MM-dd");
             }
             string query = string.Empty;

             query = "select distinct project  from productionreport2020 where  project is not null and project<>' '";
            
             if (fromdate != null && fromdate != "")
             {
                 query = query + "  and date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 query = query + "  and date<='" + endddate + "'";
             }

             if (Location != "ALL")
             {
                 query = query + " and location='" + Location + "'";
             }
             if (TL != "ALL")
             {
                 query = query + " and tlname='" + TL + "'";
             }
             if (Resource != "ALL")
             {
                 query = query + " and psn='" + ooutput + "'";
             }
             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }

             objproject = dt.DataTableToList<Client>();
             SelectList obgproject = new SelectList(objproject, "project", "project", 0);
             return Json(obgproject);
         }



         [HttpPost]
         public ActionResult GetClientprojectcodebyResource(string fromdate, string enddate, string Location, string TL, string Resource,string Clientcode)
         {
             List<Project> objproject = new List<Project>();
             DataTable dt = new DataTable();
             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             var endddate = DateTime.Now.ToString("yyyy-MM-dd");
             DateTime dtsdate = new DateTime();
             DateTime dtedate = new DateTime();

             string ooutput = string.Empty;

             if (Location == "KAKKANAD")
                 Location = "KKND";


             if (Resource != "ALL")
                 ooutput = Resource.Split('[', ']')[1];
            

             if (fromdate != null && fromdate != "")
             {
                 dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtsdate.ToString("yyyy-MM-dd");
             }
             if (enddate != null && enddate != "")
             {
                 dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 endddate = dtedate.ToString("yyyy-MM-dd");
             }
             string query = string.Empty;

             query = "select distinct projectcode  from productionreport2020 where  project is not null and project<>' '";

             if (fromdate != null && fromdate != "")
             {
                 query = query + "  and date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 query = query + "  and date<='" + endddate + "'";
             }

             if (Location != "ALL")
             {
                 query = query + " and location='" + Location + "'";
             }
             if (TL != "ALL")
             {
                 query = query + " and tlname='" + TL + "'";
             }
             if (Resource != "ALL")
             {
                 query = query + " and psn='" + ooutput + "'";
             }
             if (Clientcode != "ALL")
             {
                 query = query + " and project='" + Clientcode + "'";
             }


             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }

             objproject = dt.DataTableToList<Project>();
             SelectList obgproject = new SelectList(objproject, "Id", "Projectcode", 0);
             return Json(obgproject);
         }




        

         [HttpPost]
         public ActionResult BindResourcebyEvent(string Location,string fromdate, string enddate,  string TL, string Resource, string Clientcode,string Project)
         {
             string ooutput = string.Empty;

             if (Location=="KAKKANAD")
                 Location="KKND";


             if (Resource != "ALL")
                 ooutput = Resource.Split('[', ']')[1];
            
           
             List<Event> objproject = new List<Event>();
             DataTable dt = new DataTable();
             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             var endddate = DateTime.Now.ToString("yyyy-MM-dd");
             DateTime dtsdate = new DateTime();
             DateTime dtedate = new DateTime();
             if (fromdate != null && fromdate != "")
             {
                 dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtsdate.ToString("yyyy-MM-dd");
             }
             if (enddate != null && enddate != "")
             {
                 dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 endddate = dtedate.ToString("yyyy-MM-dd");
             }
             string query = string.Empty;

             query = "select distinct eventcode  from productionreport2020 where  eventcode is not null and eventcode<>' '";

             if (fromdate != null && fromdate != "")
             {
                 query = query + "  and date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 query = query + "  and date<='" + endddate + "'";
             }

             if (Location != "ALL")
             {
                 query = query + " and location='" + Location + "'";
             }
             if (TL != "ALL")
             {
                 query = query + " and tlname='" + TL + "'";
             }
             if (Resource != "ALL")
             {
                 query = query + " and psn='" + ooutput + "'";
             }
             if (Clientcode != "ALL")
             {
                 query = query + " and project='" + Clientcode + "'";
             }

             if (Project != "ALL")
             {
                 query = query + " and projectcode='" + Project + "'";
             }
             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }

             objproject = dt.DataTableToList<Event>();
             SelectList obgproject = new SelectList(objproject, "eventcode", "eventcode", 0);
             return Json(obgproject);
         }





        #endregion

         #region tlwiseproductivity
         [HttpPost]
         public ActionResult BindTLwise(string LocationId, string fromdate, string enddate)
         {

            
             DataTable dt = new DataTable();
             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             var endddate = DateTime.Now.ToString("yyyy-MM-dd");
             DateTime dtsdate = new DateTime();
             DateTime dtedate = new DateTime();
             if (LocationId == "KAKKANAD")
             {
                 LocationId = "KKND";
             }

             if (fromdate != null && fromdate != "")
             {
                 dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtsdate.ToString("yyyy-MM-dd");
             }
             if (enddate != null && enddate != "")
             {
                 dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 endddate = dtedate.ToString("yyyy-MM-dd");
             }
             string query = string.Empty;

             query = "select distinct tlname  from productionreport2020 where  tlname  is not null and tlname<>' '";
             
             if (fromdate != null && fromdate != "")
             {
                 query = query + "  and date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 query = query + "  and date<='" + endddate + "'";
             }

             if (LocationId != "ALL")
             {
                 query = query + " and location='" + LocationId + "'";
             }
            


             List<TL> objtl = new List<TL>();
           
             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }


             objtl = dt.DataTableToList<TL>();
             SelectList obgtl = new SelectList(objtl, "tlname", "tlname", 0);
             return Json(obgtl);
         }

         public ActionResult BindEventTLwise(string Clientcode, string LocationId, string fromdate, string enddate, string TL, string ProjectId)
         {


             DataTable dt = new DataTable();
             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             var endddate = DateTime.Now.ToString("yyyy-MM-dd");
             DateTime dtsdate = new DateTime();
             DateTime dtedate = new DateTime();
             if (LocationId == "KAKKANAD")
             {
                 LocationId = "KKND";
             }

             if (fromdate != null && fromdate != "")
             {
                 dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtsdate.ToString("yyyy-MM-dd");
             }
             if (enddate != null && enddate != "")
             {
                 dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 endddate = dtedate.ToString("yyyy-MM-dd");
             }
             string query = string.Empty;

             query = "select distinct eventcode  from productionreport2020 where  project  is not null and project<>' '";

             if (fromdate != null && fromdate != "")
             {
                 query = query + "  and date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 query = query + "  and date<='" + endddate + "'";
             }

             if (LocationId != "ALL")
             {
                 query = query + " and location='" + LocationId + "'";
             }

             if (TL != "ALL")
             {
                 query = query + " and tlname='" + TL + "'";
             }

             if (Clientcode != "ALL")
             {
                 query = query + " and project='" + Clientcode + "'";
             }

             if (ProjectId != "ALL")
             {
                 query = query + " and projectcode='" + ProjectId + "'";
             }

             List<Event> objtl = new List<Event>();

             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }


             objtl = dt.DataTableToList<Event>();
             SelectList obgtl = new SelectList(objtl, "eventcode", "eventcode", 0);
             return Json(obgtl);
         }





         public ActionResult BindTLwiseClientcode(string LocationId, string fromdate, string enddate,string TL)
         {


             DataTable dt = new DataTable();
             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             var endddate = DateTime.Now.ToString("yyyy-MM-dd");
             DateTime dtsdate = new DateTime();
             DateTime dtedate = new DateTime();
             if (LocationId == "KAKKANAD")
             {
                 LocationId = "KKND";
             }

             if (fromdate != null && fromdate != "")
             {
                 dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtsdate.ToString("yyyy-MM-dd");
             }
             if (enddate != null && enddate != "")
             {
                 dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 endddate = dtedate.ToString("yyyy-MM-dd");
             }
             string query = string.Empty;

             query = "select distinct project  from productionreport2020 where  project  is not null and project<>' '";

             if (fromdate != null && fromdate != "")
             {
                 query = query + "  and date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 query = query + "  and date<='" + endddate + "'";
             }

             if (LocationId != "ALL")
             {
                 query = query + " and location='" + LocationId + "'";
             }

             if (TL != "ALL")
             {
                 query = query + " and tlname='" + TL + "'";
             }

             List<Client> objtl = new List<Client>();

             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }


             objtl = dt.DataTableToList<Client>();
             SelectList obgtl = new SelectList(objtl, "project", "project", 0);
             return Json(obgtl);
         }


         [HttpPost]
         public ActionResult BindProductivityyProject(string Clientcode, string LocationId, string fromdate, string enddate, string TL)
         {
             DataTable dt = new DataTable();
             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             var endddate = DateTime.Now.ToString("yyyy-MM-dd");
             DateTime dtsdate = new DateTime();
             DateTime dtedate = new DateTime();
             if (LocationId == "KAKKANAD")
             {
                 LocationId = "KKND";
             }

             if (fromdate != null && fromdate != "")
             {
                 dtsdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtsdate.ToString("yyyy-MM-dd");
             }
             if (enddate != null && enddate != "")
             {
                 dtedate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 endddate = dtedate.ToString("yyyy-MM-dd");
             }
             string query = string.Empty;

             query = "select distinct projectcode  from productionreport2020 where  projectcode  is not null and projectcode<>' '";

             if (fromdate != null && fromdate != "")
             {
                 query = query + "  and date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 query = query + "  and date<='" + endddate + "'";
             }

             if (LocationId != "ALL")
             {
                 query = query + " and location='" + LocationId + "'";
             }

             if (TL != "ALL")
             {
                 query = query + " and tlname='" + TL + "'";
             }
             if (Clientcode != "ALL")
             {
                 query = query + " and project='" + Clientcode + "'";
             }

             List<Project> objproject = new List<Project>();

             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }


             objproject = dt.DataTableToList<Project>();
             SelectList obgproject = new SelectList(objproject, "Id", "Projectcode", 0);
             return Json(obgproject);
         }
        





         #endregion
         #region ETO



         public ActionResult BindClientbyLocationETO(string Location, string fromdate, string enddate)
        {

            string query = string.Empty;
            var stdate = DateTime.Now.ToString("yyyy-MM-dd");
            var endate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dtdate = new DateTime();
            DateTime dtenddate = new DateTime();

            if (fromdate != null && fromdate != "")
            {
                dtdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                stdate = dtdate.ToString("yyyy-MM-dd");
            }

            if (enddate != null && enddate != "")
            {
                dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endate = dtenddate.ToString("yyyy-MM-dd");
            }

            if (Location == "KAKKANAD")
                Location = "KKND";

            query = "select distinct project  from productionreport2020 where  project is not null and project<>' '";


            if (Location != "ALL")
            {
                query = query + " and location='" + Location + "'";
            }

            if (fromdate != null && fromdate != "")
            {
                query = query + "  and date>='" + stdate + "'";
            }

            if (endate != null && endate != "")
            {
                query = query + "  and date<='" + endate + "'";
            }


            query = query + "  order by project";
            List<Client> objproject = new List<Client>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                    }
                }
            }


            objproject = dt.DataTableToList<Client>();
            SelectList obgproject = new SelectList(objproject, "project", "project", 0);
            return Json(obgproject);



        }


         [HttpPost]
         public ActionResult FillprojectbyClientETO(string Clientcode, string startdate, string enddate, string Location)
         {

             string[] startArr = null;
             string[] endArr = null;
             char[] splitchar = { '/' };
             startArr = startdate.Split(splitchar);
             if (startArr.Length > 0)
                 startdate = startArr[2] + "-" + startArr[1] + "-" + startArr[0];
             if (Location == "KAKKANAD")
                 Location = "KKND";
             if (enddate != "")
             {
                 endArr = enddate.Split(splitchar);
                 if (endArr.Length > 0)
                     enddate = endArr[2] + "-" + endArr[1] + "-" + endArr[0];
             }

             

             string query = string.Empty;
             query = "select distinct `Projectcode`  from productionreport2020  where   projectcode is not null";
             if (Clientcode != "ALL")

                 query = query + " and `Project`  ='" + Clientcode + "'";

             query = query + " and date>='" + startdate + "'";

             if (enddate != "")
             {
                 query = query + " and date <='" + enddate + "'";
             }
             if (Location != "ALL")
             {
                 query = query + " and `location`  ='" + Location + "'";
             }


             List<Project> objclient = new List<Project>();
             DataTable dt = new DataTable();
             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }

             objclient = dt.DataTableToList<Project>();
             SelectList obgclient = new SelectList(objclient, "projectcode", "projectcode", 0);
             return Json(obgclient);



         }

         [HttpPost]
         public ActionResult BindEventETO(string ProjectId, string fromdate, string enddate, string Clientcode, string Location, string TL)
         {

             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             string query = string.Empty;
             DateTime dtstartdate = new DateTime();


             var findate = DateTime.Now.ToString("yyyy-MM-dd");
            
             DateTime dtfindate = new DateTime();

             if(Location=="KAKKANAD")
                 Location="KKND";
             query = "select distinct eventcode from productionreport2020 where eventcode is not null and eventcode<>' ' ";
             if (ProjectId != "ALL")
             {
                 query = query + " and projectcode='" + ProjectId + "'";
             }
             if (Location != "ALL")
             {
                 query = query + " and location='" + Location + "'";
             }

             if (TL != "ALL")
             {
                 query = query + " and tlname='" + TL + "'";
             }
             if (fromdate != null && fromdate != "")
             {
                 dtstartdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtstartdate.ToString("yyyy-MM-dd");
                 query = query + " and productionreport2020.date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 dtfindate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 findate = dtfindate.ToString("yyyy-MM-dd");
                 query = query + " and productionreport2020.date<='" + findate + "'";
             }


             if (Clientcode != "ALL")
             {
                 query = query + " and project='" + Clientcode + "'";
             }






             List<Event> objevent = new List<Event>();
             DataTable dt = new DataTable();
             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }


             objevent = dt.DataTableToList<Event>();
             SelectList obgevent = new SelectList(objevent, "eventcode", "eventcode", 0);
             return Json(obgevent);
         }

        

         [HttpPost]
         public ActionResult BindprojectMonthETO(string fromdate, string enddate, string LocationId)
         {

             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             string query = string.Empty;
             DateTime dtstartdate = new DateTime();


             var findate = DateTime.Now.ToString("yyyy-MM-dd");

             DateTime dtfindate = new DateTime();

             
             if (LocationId == "KAKKANAD")
                 LocationId = "KKND";

             

             query = "select distinct  projectcode  from productionreport2020 where project is not null";

             if (LocationId != "ALL")
                 query = query + "  and location='" + LocationId + "'";

             if (fromdate != null && fromdate != "")
             {
                 dtstartdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtstartdate.ToString("yyyy-MM-dd");
                 query = query + " and productionreport2020.date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 dtfindate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 findate = dtfindate.ToString("yyyy-MM-dd");
                 query = query + " and productionreport2020.date<='" + findate + "'";
             }

             List<Project> objproject = new List<Project>();
             DataTable dt = new DataTable();
             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }


             objproject = dt.DataTableToList<Project>();
             SelectList obgproject = new SelectList(objproject, "Id", "Projectcode", 0);
             return Json(obgproject);
         }





         [HttpPost]
         public ActionResult BindprojectETO(string fromdate, string enddate, string LocationId)
         {

             var startdate = DateTime.Now.ToString("yyyy-MM-dd");
             string query = string.Empty;
             DateTime dtstartdate = new DateTime();


             var findate = DateTime.Now.ToString("yyyy-MM-dd");

             DateTime dtfindate = new DateTime();

             
             if (LocationId == "KAKKANAD")
                 LocationId = "KKND";

             

             query = "select distinct  projectcode  from productionreport2020 where project is not null";

             if (LocationId != "ALL")
                 query = query + "  and location='" + LocationId + "'";

             if (fromdate != null && fromdate != "")
             {
                 dtstartdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 startdate = dtstartdate.ToString("yyyy-MM-dd");
                 query = query + " and productionreport2020.date>='" + startdate + "'";
             }
             if (enddate != null && enddate != "")
             {
                 dtfindate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 findate = dtfindate.ToString("yyyy-MM-dd");
                 query = query + " and productionreport2020.date<='" + findate + "'";
             }

             List<Project> objproject = new List<Project>();
             DataTable dt = new DataTable();
             string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                 using (MySqlCommand cmd = new MySqlCommand(query, con))
                 {

                     using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                     {

                         sda.Fill(dt);


                     }
                 }
             }


             objproject = dt.DataTableToList<Project>();
             SelectList obgproject = new SelectList(objproject, "Id", "Projectcode", 0);
             return Json(obgproject);
         }





         #endregion

         public ActionResult ProductionReport()
        {
            Projectmodel Model = new Projectmodel();
          
            List<SelectListItem> Projectcodes = new List<SelectListItem>();
            List<SelectListItem> Tls = new List<SelectListItem>();
            List<SelectListItem> Associates = new List<SelectListItem>();


            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select Id,CONCAT(Firstname,' ',LastName) as FirstName from muser where   Roleid=2 and  isactive=true  order by location", con))
                {

                    using (MySqlDataReader sda = cmd.ExecuteReader())
                    {

                        while (sda.Read())
                        {
                            Tls.Add(new SelectListItem
                            {
                                Text = sda["FirstName"].ToString(),
                                Value = sda["Id"].ToString()
                            });
                        }


                    }
                }
                con.Close();

            }



            using (MySqlConnection con = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("Select PSN,`AssociateName` from memployee where isActive=true", con))
                {
                    con.Open();
                    using (MySqlDataReader pda = cmd.ExecuteReader())
                    {

                        while (pda.Read())
                        {
                            Associates.Add(new SelectListItem
                            {
                                Text = pda["AssociateName"].ToString(),
                                Value = pda["PSN"].ToString()
                            });
                        }


                    }
                }
                con.Close();
            }








            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `Projectcode` from `productionreport2020` where year(date)=2020 and projectcode<>'' order by `pecode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Projectcodes.Add(new SelectListItem
                            {
                                Text = sdr["Projectcode"].ToString(),
                                Value = sdr["Projectcode"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }

            ViewBag.Projectcodes = Projectcodes;
            ViewBag.Tls = Tls;
            ViewBag.Associates = Associates;
            return View("ProductionDetailsReport", Model);



        }


        public ActionResult ProductionDetailReport(string Location, string Process, string projectcode, string sdate, string enddate, string Tl, string Associate)
        {



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = dtenddate.ToString("yyyy-MM-dd");

            DataTable dataTable = new DataTable();
            ProductionDetailModel model = new ProductionDetailModel();

            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }


            using (MySqlConnection con = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("ProductionDetailReport", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@llocation", Location);
                    cmd.Parameters.AddWithValue("@startdate", stdate);
                    cmd.Parameters.AddWithValue("@enddate", cdate);
                    cmd.Parameters.AddWithValue("@pprocess", Process);
                    cmd.Parameters.AddWithValue("@pproject", projectcode);
                    cmd.Parameters.AddWithValue("@tl", Tl);
                    cmd.Parameters.AddWithValue("@Associate", Associate);
                    cmd.CommandTimeout = 1500;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dataTable);

                    }
                }
            }

            model.lstproductionDetail = dataTable.DataTableToList<ProductionDetailModel>();
            return PartialView("_ProductionReportList", model);


        }






        public ActionResult Tlwiseproductivity()
        {

            Projectmodel Model = new Projectmodel();
            //string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //string Command = "SELECT distinct `projectcode` from `productionreport2020` where year(date)=2020 and projectcode<>'' order by `projectcode`";
            //using (MySqlConnection mConnection = new MySqlConnection(connString))
            //{
            //    mConnection.Open();
            //    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
            //    DataSet ds = new DataSet();
            //    ds.Tables.Add(new DataTable());
            //    adapter.Fill(ds.Tables[0]);
            //    DataTable dtt = ds.Tables[0];
            //    Model.ProjectModelList = dtt.DataTableToList<Projectmodel>();
            //    return View("Tlwiseproductivity", Model);

            //}

            List<SelectListItem> Projectcodes = new List<SelectListItem>();

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `Projectcode` from `productionreport2020` where year(date)=2020 and projectcode<>'' order by `pecode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Projectcodes.Add(new SelectListItem
                            {
                                Text = sdr["Projectcode"].ToString(),
                                Value = sdr["Projectcode"].ToString()
                            });
                        }
                    }
                   
                    mConnection.Close();
                }
            }

            ViewBag.Projectcodes = Projectcodes;




           return View("Tlwiseproductivity", Model);

        }


        public ActionResult TlproductivityReport(string Process, string Location, string projectcode, string TL, string clientcode, string sdate, string enddate, string eventcode)
        {



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = dtenddate.ToString("yyyy-MM-dd");

            DataTable dataTable = new DataTable();
            TLproductivityModel model = new TLproductivityModel();

            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }


            string Command = string.Empty;

            Command = "select tlname,sum(plannedprodrecord) as Plannedproduction,sum(actualprodrecord) as Actualproduction,(sum(actualprodrecord)/sum(plannedprodrecord))*100 as Achievement, sum(actualprodrecord)/sum(workedhrs) as Productivity from productionreport2020 where   productionreport2020.`date`  >='" + stdate + "' and productionreport2020.`date`  <='" + cdate + "' and tlname<>''";


            if (clientcode != "ALL")
            {
                Command = Command + " and `project`='" + clientcode + "'";
            }


            if (projectcode != "ALL")
            {
                Command = Command + " and `projectcode`='" + projectcode + "'";
            }


            if (eventcode != "ALL")
            {
                Command = Command + " and `eventcode`='" + eventcode + "'";
            }



            if (Process != "ALL")
            {
                Command = Command + " and `process`='" + Process + "'";
            }

            if (Location != "ALL")
            {
                Command = Command + " and  `location`='" + Location + "'";
            }

            if (TL != "ALL")
            {
                Command = Command + " and  `tlname`='" + TL + "'";
            }

            Command = Command + " group by tlname order by tlname;";



            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dataTable);
            }







            //using (MySqlConnection con = new MySqlConnection(connString))
            //{
            //    using (MySqlCommand cmd = new MySqlCommand("productivitytl", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@llocation", Location);
            //        cmd.Parameters.AddWithValue("@startdate", stdate);
            //        cmd.Parameters.AddWithValue("@enddate", cdate);
            //        cmd.Parameters.AddWithValue("@pprocess", Process);
            //        cmd.Parameters.AddWithValue("@pproject", projectcode);
            //        cmd.CommandTimeout = 1500;
            //        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            //        {
            //            sda.Fill(dataTable);

            //        }
            //    }
            //}





            model.LstTLproductivityModel = dataTable.DataTableToList<TLproductivityModel>();
            return PartialView("_TlwwiseproductivityList", model);


        }

        public ActionResult ProjectwiseRevenue()
        {

                PecodeModel Model = new PecodeModel();
                //List<SelectListItem> ProjectCodes = new List<SelectListItem>();
                //List<SelectListItem> EventCodes = new List<SelectListItem>();

                //string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                //using (MySqlConnection mConnection = new MySqlConnection(connString))
                //{
                //    string query = " SELECT distinct `projectcode` from `productionreport2020` where year(date)=2020 and projectcode is not null and projectcode!=' '  order by `pecode`";
                //    using (MySqlCommand cmd = new MySqlCommand(query))
                //    {
                //        cmd.Connection = mConnection;
                //        mConnection.Open();
                //        using (MySqlDataReader sdr = cmd.ExecuteReader())
                //        {
                //            while (sdr.Read())
                //            {
                //                ProjectCodes.Add(new SelectListItem
                //                {
                //                    Text = sdr["projectcode"].ToString(),
                //                    Value = sdr["projectcode"].ToString()
                //                });
                //            }
                //        }
                //       // PeCodes.Add(new SelectListItem() { Value = "-1", Text = "ALL" });
                //        mConnection.Close();
                //    }
                //}

                //using (MySqlConnection mConnection = new MySqlConnection(connString))
                //{
                //    string query = " SELECT distinct `eventcode` from `productionreport2020` where year(date)=2020 and eventcode!=' ' and eventcode is not null order by `eventcode`";
                //    using (MySqlCommand cmd = new MySqlCommand(query))
                //    {
                //        cmd.Connection = mConnection;
                //        mConnection.Open();
                //        using (MySqlDataReader sdr = cmd.ExecuteReader())
                //        {
                //            while (sdr.Read())
                //            {
                //                EventCodes.Add(new SelectListItem
                //                {
                //                    Text = sdr["eventcode"].ToString(),
                //                    Value = sdr["eventcode"].ToString()
                //                });
                //            }
                //        }
                //        // PeCodes.Add(new SelectListItem() { Value = "-1", Text = "ALL" });
                //        mConnection.Close();
                //    }
                //}



                //ViewBag.ProjectCodes = ProjectCodes;

                //ViewBag.EventCodes = EventCodes;




            return View("ProjectwiserevenueAchievement",Model);
        }

        public ActionResult ProjectwiseRevenueAchievement(string Process, string sdate, string enddate, string Projectcode, string Eventcode, string Location, string Clientcode)
        {



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = dtenddate.ToString("yyyy-MM-dd");

            DataTable dt = new DataTable();
            ProjectproductivityModel model = new ProjectproductivityModel();

            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }



            













            using (MySqlConnection con = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("projectwiseRevAchievement", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@startdate", stdate);
                    cmd.Parameters.AddWithValue("@enddate", cdate);
                    cmd.Parameters.AddWithValue("@pprocess", Process);
                    cmd.Parameters.AddWithValue("@pprojectcode", Projectcode);
                    cmd.Parameters.AddWithValue("@eeventcode", Eventcode);
                    cmd.Parameters.AddWithValue("@llocation", Location);
                    cmd.Parameters.AddWithValue("@cclientcode", Clientcode);
                    cmd.CommandTimeout = 1500;

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);

                    }
                }
            }

            if (dt.Rows.Count > 0)
            {
                DataColumn dcolColumn = new DataColumn("Total");
                dt.Columns.Add(dcolColumn);
                foreach (DataRow row in dt.Rows)
                {
                    double rowTotal = 0;
                    foreach (DataColumn col in row.Table.Columns)
                    {

                        if (col.ColumnName != "PECODE")
                        {
                            if (row[col].ToString() != "")
                                rowTotal += double.Parse(row[col].ToString());
                        }
                    }
                    row["Total"] = rowTotal.ToString("#,##0");
                }


                DataRow totalsRow = dt.NewRow();
                totalsRow[0] = "Total";
                foreach (DataColumn col in dt.Columns)
                {
                    double colTotal = 0;
                    foreach (DataRow row in col.Table.Rows)
                    {
                        if (col.ColumnName != "PECODE")
                        {
                            if (row[col].ToString() != "")
                                colTotal += double.Parse(row[col].ToString());
                        }
                    }
                    if (col.ColumnName == "PECODE")
                    {
                        totalsRow[col.ColumnName] = "Total";
                    }
                    else
                    {
                        totalsRow[col.ColumnName] = colTotal.ToString("#,##0");
                    }
                }

                dt.Rows.Add(totalsRow);
            }


            return PartialView("_ProjectRevAchievement", dt);
        }



        public ActionResult ProjectwiseRevenueAchievementETO(string Process, string sdate, string enddate, string Projectcode, string Eventcode, string Location, string Clientcode)
        {



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = dtenddate.ToString("yyyy-MM-dd");

            DataTable dt = new DataTable();
            ProjectproductivityModel model = new ProjectproductivityModel();

            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }

            using (MySqlConnection con = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("ProjectwiseRevenueAchievementETO", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@startdate", stdate);
                    cmd.Parameters.AddWithValue("@enddate", cdate);
                    cmd.Parameters.AddWithValue("@pprocess", Process);
                    cmd.Parameters.AddWithValue("@pprojectcode", Projectcode);
                    cmd.Parameters.AddWithValue("@eeventcode", Eventcode);
                    cmd.Parameters.AddWithValue("@llocation", Location);
                    cmd.Parameters.AddWithValue("@cclientcode", Clientcode);
                    cmd.CommandTimeout = 1500;

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);

                    }
                }
            }



            DataColumn dcolColumn = new DataColumn("Total");
            dt.Columns.Add(dcolColumn);
            foreach (DataRow row in dt.Rows)
            {
                double rowTotal = 0;
                foreach (DataColumn col in row.Table.Columns)
                {

                    if (col.ColumnName != "PECODE")
                    {
                        if (row[col].ToString() != "")
                            rowTotal += double.Parse(row[col].ToString());
                    }
                }
                row["Total"] = rowTotal;
            }


            DataRow totalsRow = dt.NewRow();
            totalsRow[0] = "Total";
            foreach (DataColumn col in dt.Columns)
            {
                double colTotal = 0;
                foreach (DataRow row in col.Table.Rows)
                {
                    if (col.ColumnName != "PECODE")
                    {
                        if (row[col].ToString() != "")
                            colTotal += double.Parse(row[col].ToString());
                    }
                }
                if (col.ColumnName == "PECODE")
                {
                    totalsRow[col.ColumnName] = "Total";
                }
                else
                {
                    totalsRow[col.ColumnName] = colTotal;
                }
            }

            dt.Rows.Add(totalsRow);


            return PartialView("_ProjectRevAchievement", dt);
        }






        //public ActionResult TlproductivityReport(string Location, string Process, string projectcode, string sdate, string enddate)
        //{



        //    string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


        //    DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //    DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //    var stdate = dtstartdate.ToString("yyyy-MM-dd");
        //    var cdate = dtenddate.ToString("yyyy-MM-dd");

        //    DataTable dataTable = new DataTable();
        //    TLproductivityModel model = new TLproductivityModel();



        //    using (MySqlConnection con = new MySqlConnection(connString))
        //    {
        //        using (MySqlCommand cmd = new MySqlCommand("productivitytl", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@llocation", Location);
        //            cmd.Parameters.AddWithValue("@startdate", stdate);
        //            cmd.Parameters.AddWithValue("@enddate", cdate);
        //            cmd.Parameters.AddWithValue("@pprocess", Process);
        //            cmd.Parameters.AddWithValue("@pproject", projectcode);
        //            cmd.CommandTimeout = 1500;
        //            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
        //            {
        //                sda.Fill(dataTable);

        //            }
        //        }
        //    }

        //    model.LstTLproductivityModel = dataTable.DataTableToList<TLproductivityModel>();
        //    return PartialView("_TLwiseProductivityList.cshtml", model);


        //}







        public ActionResult Resourcewiseproductivity()
        {
           
            Projectmodel Model = new Projectmodel();
            //string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //string Command = "SELECT distinct `projectcode` from `productionreport2020` where year(date)=2020 and projectcode<>'' order by `projectcode`";
            //using (MySqlConnection mConnection = new MySqlConnection(connString))
            //{
            //    mConnection.Open();
            //    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
            //    DataSet ds = new DataSet();
            //    ds.Tables.Add(new DataTable());
            //    adapter.Fill(ds.Tables[0]);
            //    DataTable dtt = ds.Tables[0];
            //    Model.ProjectModelList = dtt.DataTableToList<Projectmodel>();



                List<SelectListItem> Projectcodes = new List<SelectListItem>();

                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    string query = " SELECT distinct `Projectcode` from `productionreport2020` where year(date)=2020 and projectcode<>'' order by `pecode`";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = mConnection;
                        mConnection.Open();
                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                Projectcodes.Add(new SelectListItem
                                {
                                    Text = sdr["Projectcode"].ToString(),
                                    Value = sdr["Projectcode"].ToString()
                                });
                            }
                        }

                        mConnection.Close();
                    }
                }

                ViewBag.Projectcodes = Projectcodes;




                return View("Resourcewiseproductivity", Model);

            
        }


        public ActionResult ResourcewiseproductivityReport(string Process, string Location, string projectcode, string TL, string Resource, string clientcode, string sdate, string enddate, string eventcode)
        {
            string ooutput=string.Empty;
            if (Location == "KAKKANAD")
            {
                Location = "KKND";

            }
            if (Resource!="ALL")
             ooutput = Resource.Split('[', ']')[1];

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = dtenddate.ToString("yyyy-MM-dd");

            DataTable dataTable = new DataTable();
            ResourceproductivityModel model = new ResourceproductivityModel();



            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }


            string Command = string.Empty;



            Command = "select associate,sum(targetrevenue) as Targetrevenue,sum(actualrevenue) as Actualrevenue,sum(actualprodrecord) as `actualprodrecord`,sum(plannedprodrecord)    as `plannedprodrecord` ,     sum(actualprodrecord)/sum(workedhrs) as Productivity from productionreport2020 where   associate<>''  and productionreport2020.`date`  >= '" + stdate + "' and productionreport2020.`date`  <='" + cdate + "'";


            if (clientcode != "ALL")
            {
                Command = Command + " and `project`='" + clientcode + "'";
            }


            if (projectcode != "ALL")
            {
                Command = Command + " and `projectcode`='" + projectcode + "'";
            }


            if (eventcode != "ALL")
            {
                Command = Command + " and `eventcode`='" + eventcode + "'";
            }



            if (Process != "ALL")
            {
                Command = Command + " and `process`='" + Process + "'";
            }

            if (Location != "ALL")
            {
                Command = Command + " and  `location`='" + Location + "'";
            }

            if (TL != "ALL")
            {
                Command = Command + " and  `tlname`='" + TL + "'";
            }

            if (Resource != "ALL")
            {
                Command = Command + " and  `psn`='" + ooutput + "'";
            }

            Command = Command + "  group by associate order by associate;";



            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dataTable);
            }







            //using (MySqlConnection con = new MySqlConnection(connString))
            //{
            //    using (MySqlCommand cmd = new MySqlCommand("productivityresource", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@llocation", Location);
            //        cmd.Parameters.AddWithValue("@startdate", stdate);
            //        cmd.Parameters.AddWithValue("@enddate", cdate);
            //        cmd.Parameters.AddWithValue("@pprocess", Process);
            //        cmd.Parameters.AddWithValue("@pproject", projectcode);
            //        cmd.CommandTimeout = 1500;
            //        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            //        {
            //            sda.Fill(dataTable);

            //        }
            //    }
            //}

            model.LstResourceproductivityModel = dataTable.DataTableToList<ResourceproductivityModel>();
            return PartialView("_ResourcewiseproductivityList", model);


        }

        public ActionResult Productivity()
        {
            Projectmodel Model = new Projectmodel();
            List<SelectListItem> ClientCodes = new List<SelectListItem>();
            List<SelectListItem> Projectcodes = new List<SelectListItem>();
            List<SelectListItem> TLs = new List<SelectListItem>();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {

                //string query = " SELECT distinct `project` from `productionreport2020` where year(date)=" + int.Parse(year) + " and month(date)=" + int.Parse(month) + "   and project is not null order by `project`";

                string query = " SELECT distinct `project` from `productionreport2020` where     project is not null and project <>' ' order by `project`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ClientCodes.Add(new SelectListItem
                            {
                                Text = sdr["project"].ToString(),
                                Value = sdr["project"].ToString()
                            });
                        }
                    }
                    // PeCodes.Add(new SelectListItem() { Value = "-1", Text = "ALL" });
                    mConnection.Close();
                }
            }





            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `Projectcode` from `productionreport2020` where year(date)=2020 and projectcode<>'' order by `pecode`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Projectcodes.Add(new SelectListItem
                            {
                                Text = sdr["Projectcode"].ToString(),
                                Value = sdr["Projectcode"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                string query = " SELECT distinct `tlname` from `productionreport2020` where year(date)=" + int.Parse(year) + " and month(date)=" + int.Parse(month) + " and tlname is not null and tlname<>' '  order by tlname";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            TLs.Add(new SelectListItem
                            {
                                Text = sdr["tlname"].ToString(),
                                Value = sdr["tlname"].ToString()
                            });
                        }
                    }

                    mConnection.Close();
                }
            }

            ViewBag.Projectcodes = Projectcodes;
            ViewBag.TLs = TLs;
            ViewBag.ClientCodes = ClientCodes;
          


            return View("Productivity");
        }


        public ActionResult Projectwiseproductivity()
        {

            Projectmodel model = new Projectmodel();
            
            
            return View("Projectwiseproductivity");
        }

        public ActionResult ProjectwiseproductivityReport(string Process, string Location, string Project, string clientcode, string eventcode, string sdate, string enddate)
        {

           

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = dtenddate.ToString("yyyy-MM-dd");

            DataTable dataTable = new DataTable();
            ProjectproductivityModel model = new ProjectproductivityModel();

            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }

            //using (MySqlConnection con = new MySqlConnection(connString))
            //{
            //    using (MySqlCommand cmd = new MySqlCommand("productivityproject", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@llocation", Location);
            //        cmd.Parameters.AddWithValue("@startdate", stdate);
            //        cmd.Parameters.AddWithValue("@enddate", cdate);
            //        cmd.Parameters.AddWithValue("@project", Project);
            //        cmd.Parameters.AddWithValue("@pprocess", Process);
            //        cmd.CommandTimeout = 1500;
            //        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            //        {
            //            sda.Fill(dataTable);

            //        }
            //    }
            //}

            string Command = string.Empty;

            Command = "select CONCAT(projectcode, ' ', eventcode) AS pecode ,sum(plannedprodrecord) as  plannedprodrecord,sum(actualprodrecord) as  actualprodrecord,   sum(targetrevenue) as Targetrevenue,   sum(actualrevenue) as Actualrevenue,sum(actualrevenue)/sum(targetrevenue) as revenueachievement , sum(actualprodrecord)/sum(workedhrs) as Productivity from productionreport2020 where  actualprodrecord<>0 and  productionreport2020.`date`  >='" + stdate + "' and productionreport2020.`date`  <='" + cdate + "' and pecode<>''";

            if (clientcode != "ALL")
            {
                Command = Command + " and `project`='" + clientcode + "'";
            }


            if (Project != "ALL")
            {
                Command = Command + " and `projectcode`='" + Project + "'";
            }

            if (eventcode != "ALL")
            {
                Command = Command + " and `eventcode`='" + eventcode + "'";
            }




            if (Process != "ALL")
            {
                Command = Command + " and `process`='" + Process + "'";
            }

            if (Location != "ALL")
            {
                Command = Command + " and  `location`='" + Location + "'";
            }


            Command = Command + "  group by projectcode,eventcode;";



            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dataTable);
            }



            model.LstProjectproductivityModel = dataTable.DataTableToList<ProjectproductivityModel>();
            return PartialView("_ProjectwiseproductivityList", model);

           
        }


        public string SaveMonthCopy(OpenCpyconfigModel model)
        {
            string Result = string.Empty;
            int CpylocationId;
            double CpyProductionPlannedHr;
            string CpyProjectcode, CpyEventcode, CpyProcess, Cpylocation, CpyToyear, monthid;
            Result = "NotOk";
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            string FYear = string.Empty;
            if (model.FromYear == "1")
            {
                FYear = "2019";
            }
            else if (model.FromYear == "2")
            {
                FYear = "2020";
            }

            //string SelectCommand = "SELECT EXISTS(SELECT * FROM projectconfiguration WHERE monthid='" + model.FromMonthId + "' and year='" + model.FromYear + "' and Process= '" + process + "' and location= '" + location + "' and monthname='" + monthname + "' and year=" + year + ") as exist";
            string SelectCommand = "SELECT * FROM projectconfiguration where month='" + model.FromMonthId + "'and year='" + FYear + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                using (MySqlDataAdapter adpfill = new MySqlDataAdapter(SelectCommand, mConnection))
                {
                    adpfill.Fill(dt);

                }
                if (dt.Rows.Count > 0)
                {
                    monthid = model.ToMonthId.ToString();
                    string TYear = string.Empty;
                    if (model.ToYear == "1")
                    {
                        TYear = "2019";
                    }
                    else if (model.ToYear == "2")
                    {
                        TYear = "2020";
                    }
                    CpyToyear = TYear;


                    string CpyTomonthname = string.Empty;
                    if (monthid == "1")
                    {
                        CpyTomonthname = "January";
                    }
                    else if (monthid == "2")
                    {
                        CpyTomonthname = "February";
                    }
                    else if (monthid == "3")
                    {
                        CpyTomonthname = "March";
                    }
                    else if (monthid == "4")
                    {
                        CpyTomonthname = "April";
                    }
                    else if (monthid == "5")
                    {
                        CpyTomonthname = "May";
                    }
                    else if (monthid == "6")
                    {
                        CpyTomonthname = "June";
                    }
                    else if (monthid == "7")
                    {
                        CpyTomonthname = "July";
                    }
                    else if (monthid == "8")
                    {
                        CpyTomonthname = "August";
                    }
                    else if (monthid == "9")
                    {
                        CpyTomonthname = "September";
                    }
                    else if (monthid == "10")
                    {
                        CpyTomonthname = "October";
                    }
                    else if (monthid == "11")
                    {
                        CpyTomonthname = "November";
                    }
                    else if (monthid == "12")
                    {
                        CpyTomonthname = "December";
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CpyProjectcode = dt.Rows[i]["Projectcode"].ToString().Replace("'", "''");
                        CpyEventcode = dt.Rows[i]["Eventcode"].ToString().Replace("'", "''");
                        CpyProcess = dt.Rows[i]["Process"].ToString().Replace("'", "''");
                        CpyProductionPlannedHr = Convert.ToDouble(dt.Rows[i]["ProductionPlannedHr"]);
                        Cpylocation = dt.Rows[i]["location"].ToString().Replace("'", "''");
                        CpylocationId = Convert.ToInt32(dt.Rows[i]["locationId"]);

                        string Command = "INSERT INTO projectconfiguration(`Projectcode`,`Eventcode`, `Process`,`ProductionPlannedHr`,`location`,`month`, `monthname`,`locationId`,year) VALUES ('" + CpyProjectcode + "','" + CpyEventcode + "','" + CpyProcess + "'," + CpyProductionPlannedHr + " ,'" + Cpylocation + "'," + model.ToMonthId + ",'" + CpyTomonthname + "', " + CpylocationId + ",'" + CpyToyear + "' );";
                        // using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                        using (MySqlConnection mmConnection = new MySqlConnection(connString))
                        {
                            mmConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mmConnection))
                            {
                                myCmd.ExecuteNonQuery();


                            }
                        }

                    }

                }


                else
                {
                    Result = "NotOk";
                }

                return Result;

            }



        }


     


        public static List<DateTime> GetDates(int year, int month)
        {
            var dates = new List<DateTime>();

            // Loop from the first day of the month until we hit the next month, moving forward a day at a time
            for (var date = new DateTime(year, month, 25); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates;
        }





        public ActionResult locationwisetarget(string month,string year, string Location)
        {
            int yearr = int.Parse(year.ToString());
            int monthid=0;
            string mmonthid = string.Empty;
            if (month == "January")
            {
                monthid = 1;
            }
            else if (month == "February")
            {
                monthid = 2;
            }
            else if (month == "March")
            {
                monthid = 3;
            }
            else if (month == "April")
            {
                monthid = 4;
            }
            else if (month == "May")
            {
                monthid = 5;
            }
            else if (month == "June")
            {
                monthid = 6;
            }

            else if (month == "July")
            {
                monthid = 7;
            }
            else if (month == "August")
            {
                monthid = 8;
            }
            else if (month == "September")
            {
                monthid = 9;
            }
            else if (month == "October")
            {
                monthid = 10;
            }
            else if (month == "November")
            {
                monthid = 11;
            }

            else if (month == "December")
            {
                monthid = 12;
            }

            if (monthid.ToString().Length < 2)
                mmonthid = "0" + monthid;
            else
                mmonthid = monthid.ToString();

            DataTable dt = new DataTable();
            double kknd = 0.0;
            double knpy = 0.0;
            double mds = 0.0;
            double mns=0.0;
            double mqc=0.0;
            double tvm = 0.0;
            double total = 0.0;
            double sinloc = 0.0;
            string firstdate = "01" + "/" + mmonthid + "/" + yearr;
            DateTime dtdateFrom = new DateTime();
            dtdateFrom = DateTime.ParseExact(firstdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dfromdate = dtdateFrom.ToString("yyyy-MM-dd");
            int days = DateTime.DaysInMonth(yearr, monthid);
            string enddate = days + "/" + mmonthid + "/" + yearr;
            DateTime dtdateTo = new DateTime();
            dtdateTo = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dTodate = dtdateTo.ToString("yyyy-MM-dd");
            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("locationtargetrevenue", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@llocation", Location);
                    cmd.Parameters.AddWithValue("@ddatefrom", dfromdate);
                    cmd.Parameters.AddWithValue("@ddateto", dTodate);
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);
                      


                    }
                }
            }

            int totalColumns = dt.Columns.Count;
            dt.Columns.Add("ddate", typeof(string));
            dt.Columns.Add("Total", typeof(double));

            if (Location == "All")
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string cellValue = dt.Rows[i][0].ToString();
                    double locationtotal = 0;
                    DateTime dateAndTime = DateTime.Parse(cellValue);
                    cellValue = dateAndTime.ToString("dd/MM/yyyy");
                    dt.Rows[i]["ddate"] = cellValue;
                    if (totalColumns > 3)
                    {
                        if (yearr == 2019)
                        {
                            locationtotal = double.Parse(dt.Rows[i][1].ToString()) + double.Parse(dt.Rows[i][2].ToString()) + double.Parse(dt.Rows[i][3].ToString()) + double.Parse(dt.Rows[i][4].ToString()) + double.Parse(dt.Rows[i][5].ToString());
                            knpy = knpy + double.Parse(dt.Rows[i][1].ToString());
                            mds = mds + double.Parse(dt.Rows[i][2].ToString());
                            mns = mns + double.Parse(dt.Rows[i][3].ToString());
                            mqc = mqc + double.Parse(dt.Rows[i][4].ToString());
                            tvm = tvm + double.Parse(dt.Rows[i][5].ToString());
                        }
                        else
                        {

                            locationtotal = double.Parse(dt.Rows[i][1].ToString()) + double.Parse(dt.Rows[i][2].ToString()) + double.Parse(dt.Rows[i][3].ToString()) + double.Parse(dt.Rows[i][4].ToString()) + double.Parse(dt.Rows[i][5].ToString()) + double.Parse(dt.Rows[i][6].ToString());
                            kknd = kknd + double.Parse(dt.Rows[i][1].ToString());
                            knpy = knpy + double.Parse(dt.Rows[i][2].ToString());
                            mds = mds + double.Parse(dt.Rows[i][3].ToString());
                            mns = mns + double.Parse(dt.Rows[i][4].ToString());
                            mqc = mqc + double.Parse(dt.Rows[i][5].ToString());
                            tvm = tvm + double.Parse(dt.Rows[i][6].ToString());
                        }
                    }
                    else
                        kknd = kknd + double.Parse(dt.Rows[i][1].ToString());
                    dt.Rows[i]["Total"] = locationtotal;
                }
                total = kknd + knpy + mds + mns + mqc + tvm;
                dt.Columns.Remove("date");

                dt.Columns["ddate"].ColumnName = "Date";

                dt.Columns["Date"].SetOrdinal(0);

                ViewBag.KAKKANAD = kknd;
                ViewBag.KNPY = knpy;
                ViewBag.MDS = mds;
                ViewBag.MNS = mns;
                ViewBag.MQC = mqc;
                ViewBag.TVM = tvm;
                ViewBag.TOTAL = total;
                return PartialView("locationtargetrevenue", dt);

            }

            else
            {
                double loctotal = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string cellValue = dt.Rows[i][0].ToString();
                    
                    DateTime dateAndTime = DateTime.Parse(cellValue);
                    cellValue = dateAndTime.ToString("dd/MM/yyyy");
                    dt.Rows[i]["ddate"] = cellValue;
                    //if (totalColumns > 3)
                    //{
                    //    if (yearr == 2019)
                    //    {
                    //        locationtotal = double.Parse(dt.Rows[i][1].ToString()) + double.Parse(dt.Rows[i][2].ToString()) + double.Parse(dt.Rows[i][3].ToString()) + double.Parse(dt.Rows[i][4].ToString()) + double.Parse(dt.Rows[i][5].ToString());
                    //        knpy = knpy + double.Parse(dt.Rows[i][1].ToString());
                    //        mds = mds + double.Parse(dt.Rows[i][2].ToString());
                    //        mns = mns + double.Parse(dt.Rows[i][3].ToString());
                    //        mqc = mqc + double.Parse(dt.Rows[i][4].ToString());
                    //        tvm = tvm + double.Parse(dt.Rows[i][5].ToString());
                    //    }
                    //    else
                    //    {

                    //        locationtotal = double.Parse(dt.Rows[i][1].ToString()) + double.Parse(dt.Rows[i][2].ToString()) + double.Parse(dt.Rows[i][3].ToString()) + double.Parse(dt.Rows[i][4].ToString()) + double.Parse(dt.Rows[i][5].ToString()) + double.Parse(dt.Rows[i][6].ToString());
                    //        kknd = kknd + double.Parse(dt.Rows[i][1].ToString());
                    //        knpy = knpy + double.Parse(dt.Rows[i][2].ToString());
                    //        mds = mds + double.Parse(dt.Rows[i][3].ToString());
                    //        mns = mns + double.Parse(dt.Rows[i][4].ToString());
                    //        mqc = mqc + double.Parse(dt.Rows[i][5].ToString());
                    //        tvm = tvm + double.Parse(dt.Rows[i][6].ToString());
                    //    }
                    //}
                    //else
                    //    kknd = kknd + double.Parse(dt.Rows[i][1].ToString());
                    loctotal =loctotal + double.Parse(dt.Rows[i][1].ToString());
                    dt.Rows[i]["Total"] = loctotal;
                }
                total = loctotal;
                dt.Columns.Remove("date");

                dt.Columns["ddate"].ColumnName = "Date";

                dt.Columns["Date"].SetOrdinal(0);

                ViewBag.Location = total;
                //ViewBag.KNPY = knpy;
                //ViewBag.MDS = mds;
                //ViewBag.MNS = mns;
                //ViewBag.MQC = mqc;
                //ViewBag.TVM = tvm;
                ViewBag.TOTAL = total;
                return PartialView("singlelocationtargetrevenue", dt);

            }



            
        }



        public ActionResult resourcewisetarget(string month,string year, string Location)
        {
            int yearr = int.Parse(year.ToString());
            int monthid=0;
            string mmonthid = string.Empty;
            if (month == "January")
            {
                monthid = 1;
            }
            else if (month == "February")
            {
                monthid = 2;
            }
            else if (month == "March")
            {
                monthid = 3;
            }
            else if (month == "April")
            {
                monthid = 4;
            }
            else if (month == "May")
            {
                monthid = 5;
            }
            else if (month == "June")
            {
                monthid = 6;
            }

            else if (month == "July")
            {
                monthid = 7;
            }
            else if (month == "August")
            {
                monthid = 8;
            }
            else if (month == "September")
            {
                monthid = 9;
            }
            else if (month == "October")
            {
                monthid = 10;
            }
            else if (month == "November")
            {
                monthid = 11;
            }

            else if (month == "December")
            {
                monthid = 12;
            }

            if (monthid.ToString().Length < 2)
                mmonthid = "0" + monthid;
            else
                mmonthid = monthid.ToString();

            DataTable dt = new DataTable();
            string firstdate = "01" + "/" + mmonthid + "/" + yearr;
            DateTime dtdateFrom = new DateTime();
            dtdateFrom = DateTime.ParseExact(firstdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dfromdate = dtdateFrom.ToString("yyyy-MM-dd");
            int days = DateTime.DaysInMonth(yearr, monthid);
            string enddate = days + "/" + mmonthid + "/" + yearr;
            DateTime dtdateTo = new DateTime();
            dtdateTo = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dTodate = dtdateTo.ToString("yyyy-MM-dd");

            if (Location == "KAKKANAD")
            {
                Location = "KKND";
            }
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("resourcetargetrevenue", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@llocation", Location);
                    cmd.Parameters.AddWithValue("@ddatefrom", dfromdate);
                    cmd.Parameters.AddWithValue("@ddateto", dTodate);
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);
                      


                    }
                }
            }

            return PartialView("resourcetargetrevenue", dt);





            
        }

        public ActionResult ViewProductionReport(string date)
        {

            List<string> Teamlead = new List<string>();
            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var Ddate = dtdate.ToString("yyyy-MM-dd");
            DailymasterProductionViewModel model = new DailymasterProductionViewModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            string user = System.Web.HttpContext.Current.User.Identity.Name;
            string cmdquery=string.Empty;
            string Command =string.Empty;
            if (user == "1250")

                Command = "Select psn,process,`project`,`projectcode`,`eventcode`,`hoursplanned`,`hoursworked`,`Actualproduction` from production where   date='" + Ddate + "' and location in('MDS','MNS','MQC')";
            else
                Command = "Select psn,process,`project`,`projectcode`,`eventcode`,`hoursplanned`,`hoursworked`,`Actualproduction` from production where   date='" + Ddate + "' and location ='" + Session["location"].ToString() + "'";
            
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(new DataTable());
                    adapter.Fill(dataSet.Tables[0]);
                    DataTable dtt = dataSet.Tables[0];
                    model.LstDailymasterProductionReport = dtt.DataTableToList<DailymasterProductionViewModel>();

                }

            return PartialView("_viewproductionList", model);
        }






        public ActionResult ReportsNotUpdatedByTLAdmin(string date)
        {
            List<string> Teamlead = new List<string>();
            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var Ddate = dtdate.ToString("yyyy-MM-dd");
            User model = new User();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("Select Id,CONCAT(Firstname,' ',LastName) as FirstName,location from muser where   Roleid=2 and  isactive=true and  Id Not in( select distinct teamleadid from production where date='" + Ddate + "' ) order by location", con))
                {



                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);
                        model.UserList = dt.DataTableToList<User>();


                    }
                }
            }

            return PartialView("_NotUpdated", model);




        }





        public ActionResult ReportsNotUpdatedByTL(string date)
        {
            List<string> Teamlead = new List<string>();
            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var Ddate = dtdate.ToString("yyyy-MM-dd");
            User model = new User();
            // string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            // DataTable dt = new DataTable();
            // using (MySqlConnection con = new MySqlConnection(connString))
            // {
            //     using (MySqlCommand cmd = new MySqlCommand("Select Id,CONCAT(Firstname,' ',LastName) as FirstName from muser where location='" + Session["location"].ToString() + "' and Id Not in( select distinct teamleadid from production where date='" + Ddate + "' and location ='" + Session["location"].ToString() + "')", con))
            //     {
            //         cmd.CommandType = CommandType.Text;

            //         using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            //         {

            //             sda.Fill(dt);
            //             model.UserList = dt.DataTableToList<User>();


            //         }
            //     }
            // }

            //return PartialView("_NotUpdated", model);
            string user = System.Web.HttpContext.Current.User.Identity.Name;
            string cmdquery=string.Empty;

            if (user == "1250")
                cmdquery = "Select Id,CONCAT(Firstname,' ',LastName) as FirstName,location from muser where location in('MDS','MNS','MQC') and  Roleid=2 and  isactive=true and  Id Not in( select distinct teamleadid from production where date='" + Ddate + "' and location in ('MNS','MDS','MQC'))";
            else
                cmdquery = "Select Id,CONCAT(Firstname,' ',LastName) as FirstName,location from muser where location='" + Session["location"].ToString() + "' and  Roleid=2 and  isactive=true and  Id Not in( select distinct teamleadid from production where date='" + Ddate + "' and location ='" + Session["location"].ToString() + "')";
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(cmdquery, con))
                {



                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);
                        model.UserList = dt.DataTableToList<User>();
                        //model.cnt = int.Parse(ds.Tables[1].Rows[0][0].ToString());

                    }
                }
            }

            return PartialView("_NotUpdated", model);
        }

        public ActionResult RemoveTLmistakeEntry(FormCollection collection)
        {

            string ddate = collection["dateFrom"];

            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(ddate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var date = dtdate.ToString("yyyy-MM-dd");



            //string location = collection["Location"];
            //string locationname = string.Empty;
            //if (int.Parse(location) == 6)
            //    locationname = "KAKKANAD";
            //else if (int.Parse(location) == 5)
            //    locationname = "MNS";
            //else if (int.Parse(location) == 4)
            //    locationname = "MQC";
            //else if (int.Parse(location) == 3)
            //    locationname = "MDS";
            //else if (int.Parse(location) == 2)
            //    locationname = "KNPY";
            //else if (int.Parse(location) == 1)
            //    locationname = "TVM";


            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "Delete from production where date='" + date + "' and  location='" + Session["location"].ToString() + "' and  id <>0";

                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }

            ModelState.AddModelError("File", "Deleted Successfully");
            return View("RemoveTeamleadEntry");
        }







        public ActionResult RemovemistakeEntry(FormCollection collection)
        {

            string ddate = collection["dateFrom"];

            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(ddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var date = dtdate.ToString("yyyy-MM-dd");



            string location = collection["Location"];
            string locationname = string.Empty;
            if (int.Parse(location) == 6)
                locationname = "KAKKANAD";
            else if (int.Parse(location) == 5)
                locationname = "MNS";
            else if (int.Parse(location) == 4)
                locationname = "MQC";
            else if (int.Parse(location) == 3)
                locationname = "MDS";
            else if (int.Parse(location) == 2)
                locationname = "KNPY";
            else if (int.Parse(location) == 1)
                locationname = "TVM";


            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                if (int.Parse(location) == 7)
                {
                    cmd.CommandText = "Delete from productionreport2020 where date='" + date + "' and id <>0";
                }
                else
                {
                    cmd.CommandText = "Delete from productionreport2020 where date='" + date + "' and  location='" + locationname + "' and  id <>0";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }

            ModelState.AddModelError("File", "Deleted Successfully");
            return View("RemoveEntry");
        }

        
 public string CheckTargetrevenue(string datefrom, string location)
{
    try
    {
        string locationName = string.Empty;
        if (location == "1")
        {
            locationName = "TVM";
        }
        else if (location == "2")
        {
            locationName = "KNPY";
        }
        else if (location == "3")
        {
            locationName = "MDS";
        }
        else if (location == "4")
        {
            locationName = "MQC";
        }
        else if (location == "5")
        {
            locationName = "MNS";
        }
        else if (location == "6")
        {
            locationName = "KAKKANAD";
        }
        string col1Value = string.Empty;
        DateTime dtdate = new DateTime();
        dtdate = DateTime.ParseExact(datefrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        var date = dtdate.ToString("yyyy-MM-dd");

        string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
        string Command = string.Empty;
        if (location=="0")
       Command =  "SELECT date,ROUND(sum(targetrevenue),0) as targetrevenue , revenueconfiguration, p1.location FROM monthlyconfiguration p1, productionreport2020 p2 WHERE p1.location=p2.location AND p1.revenueconfiguration <> p2.targetrevenue AND monthname=monthname('" + date + "') and year=year('" + date + "') and date='" + date + "'" ;
        else
       Command = "SELECT date,ROUND(sum(targetrevenue),0) as targetrevenue , revenueconfiguration, p1.location FROM monthlyconfiguration p1, productionreport2020 p2 WHERE p1.location=p2.location AND p1.revenueconfiguration <> p2.targetrevenue AND monthname=monthname('" + date + "') and year=year('" + date + "') and location='" + locationName + "' and date='" + date + "'";

        

        
        
        using (MySqlConnection mConnection = new MySqlConnection(connString))
        {
            MySqlCommand cmd = new MySqlCommand(Command, mConnection);
            mConnection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    col1Value =col1Value + " " + reader["location"].ToString();

                } 
            }

        }

        return col1Value;

    }

    catch (Exception)
    {
        return "Error";
        //lblError.Text = ex.Message;
    }



        }
        public bool LocationDataInsert(string datefrom, string location)
        {
            try
            {
                string locationName = string.Empty;
                if (location == "1")
                {
                    locationName = "TVM";
                }
                else if (location == "2")
                {
                    locationName = "KNPY";
                }
                else if (location == "3")
                {
                    locationName = "MDS";
                }
                else if (location == "4")
                {
                    locationName = "MQC";
                }
                else if (location == "5")
                {
                    locationName = "MNS";
                }
                else if (location == "6")
                {
                    locationName = "KAKKANAD";
                }

                DateTime dtdate = new DateTime();
                dtdate = DateTime.ParseExact(datefrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                var date = dtdate.ToString("yyyy-MM-dd");


                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
               // string Command = "SELECT count(*) FROM production WHERE  location='" + locationName + "' AND date='" + date + "'";
                string Command = "SELECT count(*) FROM production WHERE  location='" + locationName + "' AND date='" + datefrom + "' and `teamleadid`=" + int.Parse(Session["UserId"].ToString()) + "";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    int col1Value = 0;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            col1Value = int.Parse(reader[0].ToString());


                        } if (col1Value > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }

            }


            catch (Exception)
            {
                return false;
                //lblError.Text = ex.Message;
            }



        }


        public void InsertReason(string datefrom, string location)
        {
            try
            {
                string locationName = string.Empty;
                if (location == "1")
                {
                    locationName = "TVM";
                }
                else if (location == "2")
                {
                    locationName = "KNPY";
                }
                else if (location == "3")
                {
                    locationName = "MDS";
                }
                else if (location == "4")
                {
                    locationName = "MQC";
                }
                else if (location == "5")
                {
                    locationName = "MNS";
                }
                else if (location == "6")
                {
                    locationName = "KAKKANAD";
                }


                string date = string.Empty;
                string[] strArr = null;
                char[] splitchar = { '/' };
                strArr = datefrom.Split(splitchar);
                if (strArr.Length > 0)
                    date = strArr[2] + "/" + strArr[0] + "/" + strArr[1];


                string status = "Uploaded";
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

                string Command = "INSERT INTO productionupload(`date`, `Remarks`,`Location` ) VALUES ('" + date + "','" + status + "', '" + locationName + "');";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();


                    }
                }


            }


            catch (Exception)
            {

                //lblError.Text = ex.Message;
            }
        }



        public bool     InsertProductionTablebyAdmin(DataTable dtcurrenttable, string datefrom, string Location)
        {
           
            try
            {
                string locationName = string.Empty;
                double plannedhrs = 0;
                double plannedprodhrrecord = 0;
                double prodplantarrecord = 0;
                double workedhrs = 0;
                double Achievement = 0;
                double targetrevenue = 0;
                double actualrevenue = 0;
                double revenueachieve = 0;
                double actualrecord = 0;
                double productivity = 0;
               
                if (Location == "1")
                {
                    locationName = "TVM";
                }
                else if (Location == "2")
                {
                    locationName = "KNPY";
                }
                else if (Location == "3")
                {
                    locationName = "MDS";
                }
                else if (Location == "4")
                {
                    locationName = "MQC";
                }
                else if (Location == "5")
                {
                    locationName = "MNS";
                }
                else if (Location == "6")
                {
                    locationName = "KAKKANAD";
                }
                string result2 = string.Empty;
                string day = string.Empty;
                string month = string.Empty;
                string year = string.Empty;
                string mysqlConnString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                for (int i = 0; i < dtcurrenttable.Rows.Count; i++)
                {
                   
                    /////////////////////////////////////


                    //string k = dtcurrenttable.Rows[i]["DATE"].ToString();
                    //int index1 = dtcurrenttable.Rows[i]["DATE"].ToString().IndexOf(" 12:00:00 am");
                    //string j = index1.ToString();

                    //if (index1 != -1)
                    //{
                    //    result2 = dtcurrenttable.Rows[i]["DATE"].ToString().Remove(index1);
                    //}
                    ////string datedd = dtcurrenttable.Rows[i]["DATE"].ToString();
                    //var split = result2.Split('/');
                    //if (split.Count() > 0)
                    //{
                    //    year = split[2].ToString();
                    //    if (split[1].Length == 1)
                    //        month = "0" + split[1].ToString();
                    //    else
                    //        month = split[1].ToString();

                    //    if (split[0].Length == 1)
                    //        day = "0" + split[0].ToString();
                    //    else
                    //        day = split[0].ToString();

                    //}

                    //result2 = day + '/' + month + '/' + year;

                    //DateTime dtdate = new DateTime();
                    //dtdate = DateTime.ParseExact(result2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //var date = dtdate.ToString("yyyy-MM-dd");



                    /////////////////////////////////////

                    double whome = 0;
                    if (dtcurrenttable.Rows[i]["Work @ Home"].ToString() != "")
                    {
                        whome = Double.Parse(dtcurrenttable.Rows[i]["Work @ Home"].ToString());
                    }
                    DateTime dtdate = new DateTime();
                    dtdate = DateTime.ParseExact(datefrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var date = dtdate.ToString("yyyy-MM-dd");
                    string Command = string.Empty;
                    string Remarks = dtcurrenttable.Rows[i]["Remarks"].ToString().Replace("'", "\\'");

                    if (dtcurrenttable.Rows[i]["Hours planned"].ToString() == "")
                    {
                        plannedhrs = 0;
                    }
                    else
                    {
                        plannedhrs = Convert.ToDouble(dtcurrenttable.Rows[i]["Hours planned"]);
                    }

                    if (dtcurrenttable.Rows[i]["ProductionRecords"].ToString() == "")
                    {
                        plannedprodhrrecord = 0;
                    }
                    else
                    {
                        plannedprodhrrecord = Convert.ToDouble(dtcurrenttable.Rows[i]["ProductionRecords"]);
                    }

                    if (dtcurrenttable.Rows[i]["ProductionTargetRecords"].ToString() == "")
                    {
                        prodplantarrecord = 0;
                    }
                    else
                    {
                        prodplantarrecord = Convert.ToDouble(dtcurrenttable.Rows[i]["ProductionTargetRecords"]);
                    }

                    if (dtcurrenttable.Rows[i]["ProductionTargetRecords"].ToString() == "" || dtcurrenttable.Rows[i]["ProductionTargetRecords"].ToString() == "0" || dtcurrenttable.Rows[i]["Actual Production Records"].ToString() == "")
                    {

                        Achievement = 0;

                    }
                    else
                    {
                        Achievement = Math.Round((Convert.ToDouble(dtcurrenttable.Rows[i]["Actual Production Records"]) / Convert.ToDouble(dtcurrenttable.Rows[i]["ProductionTargetRecords"]) * 100));

                    }

                    if (dtcurrenttable.Rows[i]["TARGET REVENUE INR"].ToString() == "")
                    {
                        targetrevenue = 0;
                    }
                    else
                    {
                        targetrevenue = Convert.ToDouble(dtcurrenttable.Rows[i]["TARGET REVENUE INR"]);
                    }

                    if (dtcurrenttable.Rows[i]["ACTUAL REVENUE INR"].ToString() == "")
                    {
                        actualrevenue = 0;
                    }
                    else
                    {
                        actualrevenue = Convert.ToDouble(dtcurrenttable.Rows[i]["ACTUAL REVENUE INR"]);
                    }

                    if (dtcurrenttable.Rows[i]["TARGET REVENUE INR"].ToString() == "" || dtcurrenttable.Rows[i]["TARGET REVENUE INR"].ToString() == "0" || dtcurrenttable.Rows[i]["ACTUAL REVENUE INR"].ToString()=="0")
                    {
                        revenueachieve = 0;
                    }
                    else
                    {
                        revenueachieve = Math.Round((Convert.ToDouble(dtcurrenttable.Rows[i]["ACTUAL REVENUE INR"]) / Convert.ToDouble(dtcurrenttable.Rows[i]["TARGET REVENUE INR"]) * 100)); ;
                    }

                    if (dtcurrenttable.Rows[i]["Actual Production Records"].ToString().Trim() == "")
                    {
                        actualrecord = 0;
                    }
                    else
                    {
                        actualrecord =int.Parse(dtcurrenttable.Rows[i]["Actual Production Records"].ToString());
                    }

                    if (dtcurrenttable.Rows[i]["Hours worked"].ToString().Trim() == "" || dtcurrenttable.Rows[i]["Hours worked"].ToString() == "0" || dtcurrenttable.Rows[i]["Hours worked"].ToString().Trim() == " ")
                    {
                        workedhrs = 0;
                    }
                    else
                    {
                        workedhrs = double.Parse(dtcurrenttable.Rows[i]["Hours worked"].ToString());
                    }


                    if (dtcurrenttable.Rows[i]["Productivity"].ToString() == "")
                    {
                        productivity = 0;
                    }
                    else
                    {
                        productivity = double.Parse(dtcurrenttable.Rows[i]["Productivity"].ToString());
                    }




                    if (dtcurrenttable.Rows[i]["Actual Production Records"].ToString() == "" && whome == 0 && workedhrs==0)
                    {
                        Command = "INSERT INTO Bpst.`productionreport2020`( psn, `associate`,tlname,`Remarks`,location,`date` ) VALUES (" + dtcurrenttable.Rows[i]["PSN"].ToString() + ",'" + dtcurrenttable.Rows[i]["Associates Name"].ToString() + "','" + dtcurrenttable.Rows[i]["TLs Name"].ToString() + "' ,'" + Remarks + "','" + dtcurrenttable.Rows[i]["LOCATION"] + "','" + date + "');";

                    }
                    else
                    {
                        Command = "INSERT INTO Bpst.`productionreport2020`( psn,`associate`, `Process`,project,`Projectcode`, `Eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,`achievement`,`remarks`,location,`targetrevenue`,`actualrevenue`,`revenueachievement`,`pecode`,`workathome`,`TotHr`,`date` ) VALUES (" + dtcurrenttable.Rows[i]["PSN"].ToString() + ",'" + dtcurrenttable.Rows[i]["Associates Name"].ToString() + "','" + dtcurrenttable.Rows[i]["Process"].ToString() + "','" + dtcurrenttable.Rows[i]["Project"].ToString() + "','" + dtcurrenttable.Rows[i]["Project Code"].ToString() + "','" + dtcurrenttable.Rows[i]["Event code"].ToString() + "','" + dtcurrenttable.Rows[i]["TLs Name"].ToString() + "' ,  " + plannedhrs + " ," + plannedprodhrrecord + "," + prodplantarrecord + ", " + workedhrs + "," + actualrecord + "," + Achievement + ",'" + Remarks + "','" + dtcurrenttable.Rows[i]["LOCATION"] + "'," + targetrevenue + "," + actualrevenue + "," + revenueachieve + ",'" + dtcurrenttable.Rows[i]["PECODE"] + "'," + whome + "," + productivity + ",'" + date + "');";

                    }

                    using (MySqlConnection mConnection = new MySqlConnection(mysqlConnString))
                    {
                        mConnection.Open();
                        using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                        {
                            myCmd.ExecuteNonQuery();

                        }
                    }


                }
                return true;
            }
            


            catch (Exception ex)
            {
                //string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message + datefrom);
                //message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                //ModelState.AddModelError(string.Empty, message);
                
                return false;
            }
            }
        


        public bool InsertProductionTable(DataTable dt, string datefrom, string Location)
        {
            string locationName = string.Empty;
            if (Location == "1")
            {
                locationName = "TVM";
            }
            else if (Location == "2")
            {
                locationName = "KNPY";
            }
            else if (Location == "3")
            {
                locationName = "MDS";
            }
            else if (Location == "4")
            {
                locationName = "MQC";
            }
            else if (Location == "5")
            {
                locationName = "MNS";
            }
            else if (Location == "6")
            {
                locationName = "KAKKANAD";
            }

            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(datefrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var date = dtdate.ToString("yyyy-MM-dd");

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    string cmdText = "INSERT IGNORE INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks,Date,Location,teamleadid,workathome ) VALUES (@psn, @process,@project, @projectcode,@eventcode,@hoursplanned,@hoursworked,@Actualproduction,@Remarks,@date,@location,@teamleadid,@workathome)";
                    connection.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        using (MySqlCommand myCmd = new MySqlCommand(cmdText, connection))
                        {
                            if (dt.Rows[i]["PSN"].ToString() != "")
                            {
                                myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN"]);
                                myCmd.Parameters.AddWithValue("@process", dt.Rows[i]["Process"]);
                                myCmd.Parameters.AddWithValue("@project", dt.Rows[i]["Project"]);
                                myCmd.Parameters.AddWithValue("@projectcode", dt.Rows[i]["Project Code"]);
                                myCmd.Parameters.AddWithValue("@eventcode", dt.Rows[i]["Event code"]);
                                myCmd.Parameters.AddWithValue("@hoursplanned", dt.Rows[i]["Hours planned"]);
                                myCmd.Parameters.AddWithValue("@hoursworked", dt.Rows[i]["Hours worked"]);
                                if (dt.Rows[i]["Actual Production Records"].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", Convert.ToInt32((dt.Rows[i]["Actual Production Records"])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@Actualproduction", 0);

                                }
                                myCmd.Parameters.AddWithValue("@Remarks", dt.Rows[i]["Remarks"]);
                                myCmd.Parameters.AddWithValue("@date", date);
                                myCmd.Parameters.AddWithValue("@location", locationName);
                                myCmd.Parameters.AddWithValue("@teamleadid", int.Parse(Session["UserId"].ToString()));
                                if (dt.Rows[i]["Work @ Home"].ToString() != "")
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", Convert.ToInt32((dt.Rows[i]["Work @ Home"])));

                                }
                                else
                                {
                                    myCmd.Parameters.AddWithValue("@workathome", 0);

                                }


                                int result = myCmd.ExecuteNonQuery();
                            }


                        }
                    }
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }






                //string Command = "INSERT INTO production (psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction ) VALUES ();";
                //using (MySqlConnection mConnection = new MySqlConnection(connString))
                //    {
                //        mConnection.Open();
                //        using (MySqlTransaction trans = mConnection.BeginTransaction())
                //        {
                //            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection, trans))
                //            {
                //                myCmd.CommandType = CommandType.Text;
                //                for (int i = 0; i < dt.Rows.Count; i++)
                //                {
                //                            myCmd.Parameters.AddWithValue("@psn", dt.Rows[i]["PSN"]);
                //                            myCmd.Parameters.AddWithValue("@process", dt.Rows[i]["Process"]);
                //                            myCmd.Parameters.AddWithValue("@project", dt.Rows[i]["Project"]);
                //                            myCmd.Parameters.AddWithValue("@projectcode", dt.Rows[i]["Project Code"]);
                //                            myCmd.Parameters.AddWithValue("@eventcode", dt.Rows[i]["Event code"]);
                //                            myCmd.Parameters.AddWithValue("@hoursplanned", dt.Rows[i]["Hours planned"]);
                //                            myCmd.Parameters.AddWithValue("@hoursworked", dt.Rows[i]["Hours worked"]);
                //                            myCmd.Parameters.AddWithValue("@Actualproduction", Convert.ToInt32((dt.Rows[i]["Actual Production Records"])));

                //                            myCmd.ExecuteNonQuery();
                //                             trans.Commit();
                //                }

                //            }
                //        }
                //    }
            }
        }




        public ActionResult DailyValidationinProject(string date, string LocationId)
        {
            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var ddate = dtdate.ToString("yyyy-MM-dd");

            NotexistModel model = new NotexistModel();
            DataTable dt = new DataTable();

            //string[] strArr = null;
            //char[] splitchar = { '/' };
            //strArr = date.Split(splitchar);
            //if (strArr.Length > 0)
            //    date = strArr[1] + "/" + strArr[0] + "/" + strArr[2];



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "Delete from noentryconfiguration";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }


            DataSet ds = new DataSet();
            //dataSet.Tables.Add(new DataTable());
            //adapter.Fill(dataSet.Tables[0]);
            //DataTable dtt = dataSet.Tables[0];

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("ProjectExist", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ddate", ddate);


                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        sda.Fill(ds);
                        model.NotexistList = ds.Tables[0].DataTableToList<NotexistModel>();
                        model.cnt = int.Parse(ds.Tables[1].Rows[0][0].ToString());

                    }
                }
            }

            return PartialView("_NotExistForm", model);
        }


        //public ActionResult Uploadprojectconfiguration()
        //{
        //    return View();
        //}



        public ActionResult DailyTeamview()
        {
            DailyTeamViewModel model = new DailyTeamViewModel();

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Id, CONCAT(muser.FirstName,' ',muser.LastName) as FirstName  FROM muser where roleid=2";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(new DataTable());
                adapter.Fill(dataSet.Tables[0]);
                DataTable dtt = dataSet.Tables[0];
                model.UserList = dtt.DataTableToList<User>();

            }

            return View("DailyTeamViewIndex", model);
        }

        public ActionResult DailyProductionview()
        {
            return View("DailyProductionViewIndex");
        }

        public ActionResult PeriodicProductionview()
        {
            PeriodicProductionViewModel model = new PeriodicProductionViewModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Id, CONCAT(muser.FirstName,' ',muser.LastName) as FirstName  FROM muser where roleid=2";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(new DataTable());
                adapter.Fill(dataSet.Tables[0]);
                DataTable dtt = dataSet.Tables[0];
                model.UserList = dtt.DataTableToList<User>();

            }

            return View("PeriodicProductionViewIndex", model);
        }



        public ActionResult PeriodicRevenueview()
        {

            return View("RevenueChartIndex");
        }

        public ActionResult TeamleadProductionReport(string date)
        {
            TeamleadProductionViewModel model = new TeamleadProductionViewModel();
            DataTable dt = new DataTable();
            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var ddate = dtdate.ToString("yyyy-MM-dd");
            string user = System.Web.HttpContext.Current.User.Identity.Name;
            string Command = string.Empty;
            //string[] strArr = null;
            //   char[] splitchar = { '/' };
            //   strArr = date.Split(splitchar);
            //   if (strArr.Length > 0)
            //       date = strArr[1] + "/" + strArr[0] + "/" + strArr[2];

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            if (user != null)
            {

                if (user == "1202" || user == "1250")
                    Command = "select psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks from production where location='" + Session["location"].ToString() + "' and production.`date`='" + ddate + "' ";

                else
                    Command = "select psn,process,project,projectcode,eventcode,hoursplanned,hoursworked,Actualproduction,Remarks from production where location='" + Session["location"].ToString() + "' and production.`date`='" + ddate + "' and teamleadid=" + user + " ";
            }
            using (MySqlConnection mConnection = new MySqlConnection(constr))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(new DataTable());
                adapter.Fill(dataSet.Tables[0]);
                DataTable dtt = dataSet.Tables[0];
                model.LstTeamleadProductionReport = dtt.DataTableToList<TeamleadProductionViewModel>();
            }
            //using (MySqlConnection con = new MySqlConnection(constr))
            //{
            //    using (MySqlCommand cmd = new MySqlCommand("GetteamleadwiseProduction", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.AddWithValue("@ddate", ddate);
            //        cmd.Parameters.AddWithValue("@location", Session["location"].ToString());

            //        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            //        {

            //            sda.Fill(dt);
            //            model.LstTeamleadProductionReport = dt.DataTableToList<TeamleadProductionViewModel>();


            //        }
            //    }
            //}

            return PartialView("_DailyTeamleadproductionview", model);


        }



        //public ActionResult Sample()
        //{
        //    DailymasterProductionViewModel model = new DailymasterProductionViewModel();
        //    string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
        //    string Command = "SELECT *  FROM production where date='04/20/2019'";
        //    using (MySqlConnection mConnection = new MySqlConnection(connString))
        //    {
        //        mConnection.Open();
        //        MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
        //        DataSet dataSet = new DataSet();
        //        dataSet.Tables.Add(new DataTable());
        //        adapter.Fill(dataSet.Tables[0]);
        //        DataTable dtt = dataSet.Tables[0];
        //        model.LstDailymasterProductionReport = dtt.DataTableToList<DailymasterProductionViewModel>();

        //    }

        //    return View("Samplepage", model);


        //}








        public ActionResult DailylocationwiseProductionReport(string date, string LocationId)
        {
            DailymasterProductionViewModel model = new DailymasterProductionViewModel();
            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var ddate = dtdate.ToString("yyyy-MM-dd");

            DataTable dt = new DataTable();
            try
            {
                //string[] strArr = null;
                //char[] splitchar = { '/' };
                //strArr = date.Split(splitchar);
                //if (strArr.Length > 0)
                //    date = strArr[1] + "/" + strArr[0] + "/" + strArr[2];

                string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("GetMasterProduction", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@location", LocationId);
                        cmd.Parameters.AddWithValue("@ddate", ddate);
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {

                            sda.Fill(dt);
                            model.LstDailymasterProductionReport = dt.DataTableToList<DailymasterProductionViewModel>();


                        }
                    }
                }

                return PartialView("DailyMasterProductionView", model);


            }

            catch (Exception ex)
            {
                model.LstDailymasterProductionReport = dt.DataTableToList<DailymasterProductionViewModel>();
                return PartialView("DailyMasterProductionView", model);

            }
        }


        public ActionResult UploadReport(string date, string LocationId)
        {
            DataTable dt = new DataTable();
            DateTime dtdate = new DateTime();
            dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var ddate = dtdate.ToString("yyyy-MM-dd");
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "select id,project,location,date_format(proddate, '%d/%m/%Y') as proddate,noofcharacters,TL,Eventcode from productiontocustomer where proddate='" + ddate + "' order by location";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }
            return View();
        }




        public ActionResult DailyconsolidatedProductionReport(string date, string LocationId)
        {
            //DailyconsolidatedViewModel model = new DailyconsolidatedViewModel();
            SummarySheetModel Model = new SummarySheetModel();
            DataTable dt = new DataTable();
            string pdate = string.Empty;
            try
            {
                //string[] strArr = null;
                //char[] splitchar = { '/' };
                //strArr = date.Split(splitchar);
                //if (strArr.Length > 0)
                //    pdate = strArr[1] + "/" + strArr[0] + "/" + strArr[2];


                DateTime dtdate = new DateTime();
                dtdate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var ddate = dtdate.ToString("yyyy-MM-dd");



                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                //using (MySqlConnection con = new MySqlConnection(constr))
                //{
                //    using (MySqlCommand cmd = new MySqlCommand("GetConsolidatedProduction", con))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@location", LocationId);
                //        cmd.Parameters.AddWithValue("@ddate", ddate);
                //        cmd.CommandTimeout = 1500;
                //        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                //        {

                //            sda.Fill(dt);
                //            model.LstDailyconsolidated = dt.DataTableToList<DailyconsolidatedViewModel>();


                //        }
                //        model.LstDailyconsolidated.ToList().ForEach(s => s.Date = pdate);
                //    }
                //}

               string Command = "select   date_format(date, '%d/%m/%Y') as date,location,Round(sum(plannedhrs),0) as hoursplanned,Round(sum(plannedhrrecord),0) as prodplanhrrecord,Round(sum(plannedprodrecord),0) as prodplanrecords,Round(sum(workedhrs),0) as hoursworked,sum(actualprodrecord) as Actualprodrecord,Round((sum(actualprodrecord)/sum(plannedprodrecord)*100),2) as Achievement,Round(sum(targetrevenue),0) as TarrevenueINR,Round(sum(actualrevenue),2) as ActrevenueINR,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement from productionreport2020 where date='" + ddate + "' group by location";

                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                    adapter.Fill(dt);

                }
                Model.lstSummarySheetmodel = dt.DataTableToList<SummarySheetModel>();

                //return PartialView("/Views/Admin/_DailyConsolidatedProductionReport", Model);
                return PartialView("/Views/Admin/_DailySampleview.cshtml",Model);

            }

            catch (Exception ex)
            {
                // model.LstDailymasterProductionReport = dt.DataTableToList<DailymasterProductionViewModel>();
                Model.lstSummarySheetmodel = null;
                return PartialView("_DailyConsolidatedProductionReport", Model);

            }
        }

      





        public ActionResult DailyTeamViewReport(string date, string UserId)
        {

            string[] strArr = null;
            char[] splitchar = { '/' };
            strArr = date.Split(splitchar);
            if (strArr.Length > 0)
                date = strArr[2] + "-" + strArr[1] + "-" + strArr[0];
            DailyTeamView model = new DailyTeamView();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetDailyTeamView", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tl", UserId);
                    cmd.Parameters.AddWithValue("@ddate", date);
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        DataTable dtt = ds.Tables[0];
                        model.LstDailyTeamReport = dtt.DataTableToList<DailyProduction>();
                        model.Summaryinfo = new SummaryReport();
                        DataTable dtsummary = ds.Tables[1];
                        model.Summaryinfo = dtsummary.DataTableToList<SummaryReport>().First();

                    }
                }
            }

            return PartialView("DailyTeamView", model);
        }
        public ActionResult DailyProductionReport(string date)
        {

            string[] strArr = null;
            char[] splitchar = { '/' };
            strArr = date.Split(splitchar);
            if (strArr.Length > 0)
                date = strArr[2] + "-" + strArr[1] + "-" + strArr[0];


            DailyProductionViewModel model = new DailyProductionViewModel();
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetProductionbytl", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ddate", date);
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        DataTable dtt = ds.Tables[0];
                        model.LstDailyProductionReport = dtt.DataTableToList<DailyTLProduction>();

                        DataTable dtsummary = ds.Tables[1];
                        model.LstDailyTLwiseProductionReport = dtsummary.DataTableToList<DailyTLwiseProduction>();

                    }
                }
            }


            return PartialView("DailyProductionView", model);
        }

        public ActionResult PeriodicProductionReport(string fromdate, string todate, string UserId, string TypeId, string GraphType)
        {
            PeriodicProductionViewModel model = new PeriodicProductionViewModel();


            string[] strArr = null;
            char[] splitchar = { '/' };
            strArr = fromdate.Split(splitchar);
            if (strArr.Length > 0)
                fromdate = strArr[2] + "-" + strArr[1] + "-" + strArr[0];

            string[] strArrTo = null;
            char[] splitcharTo = { '/' };
            strArrTo = todate.Split(splitcharTo);
            if (strArrTo.Length > 0)
                todate = strArrTo[2] + "-" + strArrTo[1] + "-" + strArrTo[0];
            TempData["fromdate"] = fromdate;
            TempData["todate"] = todate;
            TempData["userId"] = UserId;
            TempData["GraphType"] = GraphType;

            if (TypeId == "Tabular")
            {
                string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("GetProductionPeriodical", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@fromdate", fromdate);
                        cmd.Parameters.AddWithValue("@todate", todate);
                        cmd.Parameters.AddWithValue("@userId", UserId);
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            DataTable dtt = ds.Tables[0];
                            model.LstDailyProductionReport = dtt.DataTableToList<DailyTLProduction>();

                            DataTable dtsummary = ds.Tables[1];
                            model.LstDailyTLwiseProductionReport = dtsummary.DataTableToList<DailyTLwiseProduction>();

                        }
                    }
                }
                return PartialView("PeriodicProductionView", model);
            }
            else
            {


                return PartialView("PeriodicProductionChartView");
            }






        }

        public ActionResult RevenueReport(string fromdate, string todate)
        {
            try
            {


                PeriodicProductionViewModel model = new PeriodicProductionViewModel();
                string[] strArr = null;
                char[] splitchar = { '/' };
                strArr = fromdate.Split(splitchar);
                if (strArr.Length > 0)
                    fromdate = strArr[1] + "/" + strArr[0] + "/" + strArr[2];

                string[] strArrTo = null;
                char[] splitcharTo = { '/' };
                strArrTo = todate.Split(splitcharTo);
                if (strArrTo.Length > 0)
                    todate = strArrTo[1] + "/" + strArrTo[0] + "/" + strArrTo[2];

                TempData["fromdate"] = fromdate;
                TempData["todate"] = todate;

                string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("GetChartProduction", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fromdate", fromdate);
                        cmd.Parameters.AddWithValue("@todate", todate);
                        cmd.CommandTimeout = 90;
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);


                        }
                    }
                }

                return PartialView("Revenuechartview");

            }

            catch (Exception ex)
            {


                Logger(ex.Message + ex.Source);

                return View("Revenuechartview");
            }
        }

        public ActionResult projectwiseReport()
        {
            try
            {

                return PartialView("Chartview");

            }

            catch (Exception ex)
            {


                Logger(ex.Message + ex.Source);

                return View("Revenuechartview");
            }
        }

        public ActionResult locationwiseReport()
        {
            try
            {

                return PartialView("Chartview");

            }

            catch (Exception ex)
            {


                Logger(ex.Message + ex.Source);

                return View("Revenuechartview");
            }
        }



        




        #region Chart Component


        public string employeeetoChart(string Employee, string LocationId, string sdate, string enddate, string Type)
        {

            if (LocationId == "KAKKANAD")
                LocationId = "KKND";

            string DirectoryPath1 = HostingEnvironment.MapPath("~/Documents/output.png");
            string DirectoryPath = HostingEnvironment.MapPath("~/Documents/");
            string ooutput=string.Empty;

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            if (Employee != "ALL")
            {
                ooutput = Employee.Split('[', ']')[1];
            }
            else
            {
                ooutput = "ALL";
            }

            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = dtenddate.ToString("yyyy-MM-dd");

            DataTable dataTable = new DataTable();
            EmployeeETO modeleto = new EmployeeETO();




            using (MySqlConnection con = new MySqlConnection(connString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("DELETE from employeeETO where id<>0"))
                {
                    cmd.Connection = con;

                    cmd.ExecuteNonQuery();
                }

            }


            using (MySqlConnection con = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("EmployeeETOIndividual", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@location", LocationId);
                    cmd.Parameters.AddWithValue("@startdate", stdate);
                    cmd.Parameters.AddWithValue("@enddate", cdate);
                    cmd.Parameters.AddWithValue("@employee", ooutput);
                    cmd.CommandTimeout = 1500;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dataTable);

                    }
                }
            }



            dataTable.Columns.Remove("id");
            dataTable.Columns.Remove("psn");
            dataTable.Columns.Remove("associate");
            dataTable.Columns.Remove("projectcode");
            dataTable.Columns.Remove("actualrevenue");





            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 1300;
            chart.Height = 550;
            chart.BackColor = System.Drawing.Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = System.Drawing.Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = System.Drawing.Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateRevenueTitle());
            chart.Legends.Add(CreateLegend());

            for (int i = 1; i < dataTable.Columns.Count; i++)
            {

                chart.Series.Add(CreatePeriodicSeries(dataTable, SeriesChartType.Column, i));

            }



            chart.ChartAreas.Add(CreateChartArea());


            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(ms))
            {
                image.Save(DirectoryPath + "output.png", ImageFormat.Png);

                // image.Save("D://output.png", ImageFormat.Png);

                using (System.Drawing.Image image1 = System.Drawing.Image.FromFile(DirectoryPath1))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image1.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }









        }










        public string locationetoChart(string Project, string LocationId, string sdate, string enddate, string Type)
        {

            string DirectoryPath1 = HostingEnvironment.MapPath("~/Documents/output.png");
            string DirectoryPath = HostingEnvironment.MapPath("~/Documents/");

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            var dataSet = new DataSet();
            var dataTable = new DataTable();


            DateTime dtstartdate = DateTime.ParseExact(sdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtenddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var stdate = dtstartdate.ToString("yyyy-MM-dd");
            var cdate = dtenddate.ToString("yyyy-MM-dd");


            if (LocationId == "KAKKANAD")
            {
                LocationId = "KKND";
            }

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetLocationwiseETO", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@llocation", LocationId);
                    cmd.Parameters.AddWithValue("@startdate", stdate);
                    cmd.Parameters.AddWithValue("@enddate", cdate);
                    cmd.Parameters.AddWithValue("@projectcodee", Project);
                    cmd.CommandTimeout = 1500;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dataTable);

                    }
                }
            }





            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 1300;
            chart.Height = 550;
            chart.BackColor = System.Drawing.Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = System.Drawing.Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = System.Drawing.Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateRevenueTitle());
            chart.Legends.Add(CreateLegend());

            for (int i = 1; i < dataTable.Columns.Count; i++)
            {

                chart.Series.Add(CreatePeriodicSeries(dataTable, SeriesChartType.Column, i));

            }



            chart.ChartAreas.Add(CreateChartArea());


            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(ms))
            {
                image.Save(DirectoryPath + "output.png", ImageFormat.Png);

                // image.Save("D://output.png", ImageFormat.Png);

                using (System.Drawing.Image image1 = System.Drawing.Image.FromFile(DirectoryPath1))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image1.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }









        }








        public string CreateperiodicChart()
        {


            string DirectoryPath1 = HostingEnvironment.MapPath("~/Documents/output.png");
            string DirectoryPath = HostingEnvironment.MapPath("~/Documents/");
            
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            var dataSet = new DataSet();
            var dataTable = new DataTable();

            string Command = "SELECT date,ETOActualrevenue/employeeno as AverageETO  FROM tempeto";
            using (MySqlConnection mConnection = new MySqlConnection(constr))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
              
                dataSet.Tables.Add(new DataTable());
                adapter.Fill(dataSet.Tables[0]);
                dataTable = dataSet.Tables[0];
               

            }




            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 1300;
            chart.Height = 550;
            chart.BackColor = System.Drawing.Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = System.Drawing.Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = System.Drawing.Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateRevenueTitle());
            chart.Legends.Add(CreateLegend());

            for (int i = 1; i < dataTable.Columns.Count; i++)
            {

                chart.Series.Add(CreatePeriodicSeries(dataTable, SeriesChartType.Column, i));

            }



            chart.ChartAreas.Add(CreateChartArea());


            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(ms))
            {
                image.Save(DirectoryPath + "output.png", ImageFormat.Png); 
            
           // image.Save("D://output.png", ImageFormat.Png);
          
            using (System.Drawing.Image image1 = System.Drawing.Image.FromFile(DirectoryPath1))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image1.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }



            //return File(ms.GetBuffer(), @"image/png");
        }



        [NonAction]
        public Series CreatePeriodicSeries(DataTable dt, SeriesChartType chartType, int i)
        {

            Series seriesDetail = new Series();

            seriesDetail.IsValueShownAsLabel = true;
            //seriesDetail.Color = Color.FromArgb(198, 99, 99);
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            seriesDetail["DrawingStyle"] = "Cylinder";
            seriesDetail["PieDrawingStyle"] = "SoftEdge";
            DataPoint point;
            point = new DataPoint();

            foreach (DataRow dr in dt.Rows)
            {

                if (dr[i].ToString() != "")
                {
                    seriesDetail.Name = dr.Table.Columns[i].ColumnName;
                    int y = Convert.ToInt32((dr[i]));
                    string x = dr["date"].ToString();
                    string removeString = " 12:00:00 AM";
                    int index = x.IndexOf(removeString);
                    string cleanPath = (index < 0)
                        ? x
                        : x.Remove(index, removeString.Length);
                    seriesDetail.Points.AddXY(cleanPath, y);

                }

            }
            return seriesDetail;


        }






        //[NonAction]
        //public Series CreateRevenueSeries(DataTable dt, SeriesChartType chartType, int i)
        //{

        //    Series seriesDetail = new Series();

        //    seriesDetail.IsValueShownAsLabel = true;
        //    //seriesDetail.Color = Color.FromArgb(198, 99, 99);
        //    seriesDetail.ChartType = chartType;
        //    seriesDetail.BorderWidth = 2;
        //    seriesDetail["DrawingStyle"] = "Cylinder";
        //    seriesDetail["PieDrawingStyle"] = "SoftEdge";
        //    DataPoint point;
        //    point = new DataPoint();

        //    foreach (DataRow dr in dt.Rows)
        //    {

        //        if (dr[i].ToString() != "")
        //        {
        //            seriesDetail.Name = dr.Table.Columns[i].ColumnName;
        //            int y = Convert.ToInt32((dr[i]));
        //            string x = dr["date"].ToString();
        //            string removeString = " 12:00:00 AM";
        //            int index = x.IndexOf(removeString);
        //            string cleanPath = (index < 0)
        //                ? x
        //                : x.Remove(index, removeString.Length);
        //            seriesDetail.Points.AddXY(cleanPath, y);

        //        }

        //    }
        //    return seriesDetail;


        //}







        public string CreateRevenueChart()
        {

            string fromdate = Convert.ToString(TempData["fromdate"]);
            string todate = Convert.ToString(TempData["todate"]);
            //string UserId = Convert.ToString(TempData["userId"]);
            //string GraphType = Convert.ToString(TempData["GraphType"]);
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            var dataSet = new DataSet();
            var dataTable = new DataTable();



            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetRevenuePeriodic", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromdate", fromdate);
                    cmd.Parameters.AddWithValue("@todate", todate);

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        dataTable = ds.Tables[0];

                    }
                }
            }




            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 1300;
            chart.Height = 550;
            chart.BackColor = System.Drawing.Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = System.Drawing.Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = System.Drawing.Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateRevenueTitle());
            chart.Legends.Add(CreateLegend());

            for (int i = 1; i < dataTable.Columns.Count; i++)
            {

                chart.Series.Add(CreateRevenueSeries(dataTable, SeriesChartType.Bar, i));

            }



            chart.ChartAreas.Add(CreateChartArea());


            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            image.Save("D://output.png", ImageFormat.Png);

            using (System.Drawing.Image image1 = System.Drawing.Image.FromFile("D://output.png"))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image1.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }



            //return File(ms.GetBuffer(), @"image/png");
        }



        [NonAction]
        public Series CreateRevenueSeries(DataTable dt, SeriesChartType chartType, int i)
        {

            Series seriesDetail = new Series();

            seriesDetail.IsValueShownAsLabel = true;
            //seriesDetail.Color = Color.FromArgb(198, 99, 99);
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            seriesDetail["DrawingStyle"] = "Cylinder";
            seriesDetail["PieDrawingStyle"] = "SoftEdge";
            DataPoint point;
            point = new DataPoint();

            foreach (DataRow dr in dt.Rows)
            {

                if (dr[i].ToString() != "")
                {
                    seriesDetail.Name = dr.Table.Columns[i].ColumnName;
                    int y = Convert.ToInt32((dr[i]));
                    string x = dr["date"].ToString();
                    string removeString = " 12:00:00 AM";
                    int index = x.IndexOf(removeString);
                    string cleanPath = (index < 0)
                        ? x
                        : x.Remove(index, removeString.Length);
                    seriesDetail.Points.AddXY(cleanPath, y);

                }

            }
            return seriesDetail;


        }










        public string CreateChart()
        {

            string fromdate = Convert.ToString(TempData["fromdate"]);
            string todate = Convert.ToString(TempData["todate"]);
            string UserId = Convert.ToString(TempData["userId"]);
            string GraphType = Convert.ToString(TempData["GraphType"]);
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            var dataSet = new DataSet();
            var dataTable = new DataTable();
            if (UserId == "ALL" && GraphType == "Character")
            {



                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("GetProductionPeriodicRecordAll", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fromdate", fromdate);
                        cmd.Parameters.AddWithValue("@todate", todate);

                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            dataTable = ds.Tables[0];

                        }
                    }
                }

            }
            else if (UserId != "ALL" && GraphType == "Character")
            {

                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("GetProductionPeriodicRecordIndividual", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fromdate", fromdate);
                        cmd.Parameters.AddWithValue("@todate", todate);
                        cmd.Parameters.AddWithValue("@tl", UserId);
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            dataTable = ds.Tables[0];

                        }
                    }
                }







            }








            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 1300;
            chart.Height = 550;
            chart.BackColor = System.Drawing.Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = System.Drawing.Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = System.Drawing.Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle());
            chart.Legends.Add(CreateLegend());

            for (int i = 1; i < dataTable.Columns.Count; i++)
            {

                chart.Series.Add(CreateSeries(dataTable, SeriesChartType.Bar, i));

            }



            chart.ChartAreas.Add(CreateChartArea());


            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            image.Save("D://output.png", ImageFormat.Png);

            using (System.Drawing.Image image1 = System.Drawing.Image.FromFile("D://output.png"))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image1.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }



            //return File(ms.GetBuffer(), @"image/png");
        }


        [NonAction]
        public Title CreateRevenueTitle()
        {
            Title title = new Title();


            title.Text = "ETO Report";


            title.ShadowColor = System.Drawing.Color.FromArgb(32, 0, 0, 0);
            title.Font = new System.Drawing.Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = System.Drawing.Color.FromArgb(26, 59, 105);

            return title;
        }




        [NonAction]
        public Title CreateTitle()
        {
            Title title = new Title();

            if (Convert.ToString(TempData["userId"]) == "ALL")
            {
                title.Text = "Periodic production Report of all Team Leads";
            }
            else
            {
                title.Text = "Periodic production Report of " + Convert.ToString(TempData["userId"]);
            }

            title.ShadowColor = System.Drawing.Color.FromArgb(32, 0, 0, 0);
            title.Font = new System.Drawing.Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = System.Drawing.Color.FromArgb(26, 59, 105);

            return title;
        }

        [NonAction]
        public Legend CreateLegend()
        {
            Legend legend = new Legend();
            legend.Name = "Periodic production Report";
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.BackColor = System.Drawing.Color.Transparent;
            legend.Font = new System.Drawing.Font(new System.Drawing.FontFamily("Trebuchet MS"), 9);
            legend.LegendStyle = LegendStyle.Row;

            return legend;
        }

        [NonAction]
        public Series CreateSeries(DataTable dt, SeriesChartType chartType, int i)
        {

            Series seriesDetail = new Series();

            seriesDetail.IsValueShownAsLabel = true;
            //seriesDetail.Color = Color.FromArgb(198, 99, 99);
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            seriesDetail["DrawingStyle"] = "Cylinder";
            seriesDetail["PieDrawingStyle"] = "SoftEdge";
            DataPoint point;
            point = new DataPoint();

            foreach (DataRow dr in dt.Rows)
            {

                if (dr[i].ToString() != "")
                {
                    seriesDetail.Name = dr.Table.Columns[i].ColumnName;
                    int y = Convert.ToInt32((dr[i]));
                    string x = dr["date"].ToString();
                    string removeString = " 12:00:00 AM";
                    int index = x.IndexOf(removeString);
                    string cleanPath = (index < 0)
                        ? x
                        : x.Remove(index, removeString.Length);
                    seriesDetail.Points.AddXY(cleanPath, y);

                }

            }
            return seriesDetail;


        }

        [NonAction]
        public ChartArea CreateChartArea()
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Result Chart";
            chartArea.BackColor = System.Drawing.Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font = new System.Drawing.Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;

            return chartArea;
        }

        #endregion

        public ActionResult ExportAttendance(string Date)
        {
            try
            {
                DateTime dtdate = new DateTime();
                dtdate = DateTime.ParseExact(Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                var date = dtdate.ToString("yyyy-MM-dd");


                CultureInfo ci = Thread.CurrentThread.CurrentCulture;
                string monthName = ci.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                int year = int.Parse(DateTime.Now.Year.ToString());


                string user = System.Web.HttpContext.Current.User.Identity.Name;
                DateTime startDate;
                DateTime endDate = DateTime.Now;
                if (Date != null)
                {


                    string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                    string query = "Delete from productioncopy where id<>0;";
                    query += " insert into productioncopy(psn,AssociateName,Doj,project,projectcode,eventcode,process,`prodplanhrrecord`,hoursplanned,hoursworked,Actualproduction,Remarks,location,date,teamleadid,workathome) select  production.psn,(select AssociateName from memployee where memployee.psn= production.psn) as AssociateName,(select DOJ from memployee where memployee.psn= production.psn) as DOJ,production.project,production.projectcode,production.eventcode,production.process,(select ProductionPlannedHr  from  projectconfiguration where  projectconfiguration.Projectcode=production.projectcode and projectconfiguration.Eventcode =production.eventcode  and  projectconfiguration.Process =production.process and projectconfiguration.location=production.location and projectconfiguration.monthname='" + monthName + "' and projectconfiguration.year=" + year + ") as productionplannedhr,hoursplanned,hoursworked,Actualproduction,Remarks,production.location,production.date,(select CONCAT(FirstName,' ',LastName) from muser where muser.id= production.teamleadid) as teamleadid,`workathome`   from production where  teamleadid=" + int.Parse(user.ToString()) + "  and    date='" + date + "' and production.location='" + Session["location"].ToString() + "';";
                    query += "select psn,AssociateName,Doj,process,project,projectcode,eventcode,teamleadid,hoursplanned,prodplanhrrecord,hoursworked,Actualproduction,workathome,Remarks,location,date,CONCAT(projectcode,eventcode) as pecode from productioncopy where date= '" + date + "';";
                    query += "select Configuration From monthlyconfiguration where monthname='" + monthName + "'  and year=" + year + "     and monthlyconfiguration.location='" + Session["location"].ToString() + "';";
                    query += "SELECT SUM(hoursplanned) as amount FROM production where date='" + date + "' and teamleadid=" + int.Parse(user.ToString()) + " and location='" + Session["location"].ToString() + "';";

                    int monthsApart = 0;

                    using (MySqlConnection con = new MySqlConnection(constr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query))
                        {
                            using (MySqlDataAdapter sda = new MySqlDataAdapter())
                            {
                                cmd.Connection = con;
                                sda.SelectCommand = cmd;
                                using (DataSet ds = new DataSet())
                                {
                                    sda.Fill(ds);

                                    //Set Name of DataTables.
                                    ds.Tables[0].TableName = "production";
                                    ds.Tables[1].TableName = "configuration";
                                    ds.Tables[2].TableName = "hoursplanned";


                                    DataTable dtconfig = ds.Tables[1];
                                    DataTable dthour = ds.Tables[2];
                                    DataTable dt = ds.Tables[0];




                                    ds.Tables[0].TableName = "production";
                                    ds.Tables[1].TableName = "configuration";
                                    ds.Tables[2].TableName = "hoursplanned";
                                    // ds.Tables[2].Columns.Add("Production planned/Hr Records", typeof(object));
                                    dt.Columns.Add("ProductionplannedRecords", typeof(object));
                                    dt.Columns.Add("Achievement", typeof(object));
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        Logger(dt.Rows[i]["DOJ"].ToString());
                                        if (dt.Rows[i]["Prodplanhrrecord"].ToString() != "" && dt.Rows[i]["hoursplanned"].ToString() != "")
                                        {
                                            dt.Rows[i]["Prodplanhrrecord"] = (double.Parse(dtconfig.Rows[0][0].ToString()) / double.Parse(dthour.Rows[0][0].ToString())) * double.Parse(dt.Rows[i]["Prodplanhrrecord"].ToString());
                                            dt.Rows[i]["ProductionplannedRecords"] = double.Parse(dt.Rows[i]["Prodplanhrrecord"].ToString()) * double.Parse(dt.Rows[i]["hoursplanned"].ToString());
                                            dt.Rows[i]["Achievement"] = double.Parse(dt.Rows[i]["Actualproduction"].ToString()) / double.Parse(dt.Rows[i]["ProductionplannedRecords"].ToString()) * 100;
                                        }
                                        //startDate = Convert.ToDateTime(dt.Rows[i]["DOJ"].ToString());

                                        // startDate = Convert.ToDateTime(DateTime.ParseExact(dt.Rows[i]["DOJ"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture));

                                        startDate = DateTime.ParseExact(dt.Rows[i]["DOJ"].ToString(), "dd/MM/yyyy", null);

                                        monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
                                        dt.Rows[i]["DOJ"] = Math.Abs(monthsApart);
                                    }



                                    dt.AcceptChanges();


                                    ds.Tables[0].Columns["psn"].SetOrdinal(0);
                                    ds.Tables[0].Columns["AssociateName"].SetOrdinal(1);
                                    ds.Tables[0].Columns["DOJ"].SetOrdinal(2);
                                    ds.Tables[0].Columns["process"].SetOrdinal(3);
                                    ds.Tables[0].Columns["project"].SetOrdinal(4);
                                    ds.Tables[0].Columns["projectcode"].SetOrdinal(5);
                                    ds.Tables[0].Columns["eventcode"].SetOrdinal(6);
                                    ds.Tables[0].Columns["teamleadid"].SetOrdinal(7);
                                    ds.Tables[0].Columns["hoursplanned"].SetOrdinal(8);
                                    ds.Tables[0].Columns["prodplanhrrecord"].SetOrdinal(9);
                                    ds.Tables[0].Columns["ProductionplannedRecords"].SetOrdinal(10);
                                    ds.Tables[0].Columns["hoursworked"].SetOrdinal(11);
                                    ds.Tables[0].Columns["Actualproduction"].SetOrdinal(12);
                                    ds.Tables[0].Columns["Achievement"].SetOrdinal(13);
                                    ds.Tables[0].Columns["Remarks"].SetOrdinal(14);
                                    ds.Tables[0].Columns["workathome"].SetOrdinal(15);
                                    ds.Tables[0].Columns["location"].SetOrdinal(16);
                                    ds.Tables[0].Columns["Date"].SetOrdinal(17);


                                    ds.Tables[0].Columns["process"].ColumnName = "Process";
                                    ds.Tables[0].Columns["project"].ColumnName = "Project";
                                    ds.Tables[0].Columns["projectcode"].ColumnName = "Project Code";
                                    ds.Tables[0].Columns["eventcode"].ColumnName = "Event code";
                                    ds.Tables[0].Columns["teamleadid"].ColumnName = "TL's Name";
                                    ds.Tables[0].Columns["hoursplanned"].ColumnName = "Hours planned";
                                    ds.Tables[0].Columns["prodplanhrrecord"].ColumnName = "Production planned/Hr Records";
                                    ds.Tables[0].Columns["ProductionplannedRecords"].ColumnName = "Production planned  Records";
                                    ds.Tables[0].Columns["hoursworked"].ColumnName = "Hours worked";
                                    ds.Tables[0].Columns["Actualproduction"].ColumnName = "Actual Production Records";
                                    ds.Tables[0].Columns["workathome"].ColumnName = "Work @ home";
                                    ds.Tables[0].Columns["DOJ"].ColumnName = "Experience";

                                    // ds.Tables[0].Columns["achievement"].ColumnName = "% Achievement";
                                    // ds.Tables[0].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                                    // ds.Tables[1].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                                    // ds.Tables[1].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                                    //ds.Tables[1].Columns["productivity"].ColumnName = "Productivity(per hr)";
                                    ds.Tables[1].AcceptChanges();






                                    using (XLWorkbook wb = new XLWorkbook())
                                    {


                                        wb.Worksheets.Add(dt);


                                        //string[] strArr = null;
                                        //char[] splitchar = { '/' };
                                        //strArr = date.Split(splitchar);
                                        //if (strArr.Length > 0)
                                        //    date = strArr[1] + "." + strArr[0] + "." + strArr[2];


                                        Response.Clear();
                                        Response.Buffer = true;
                                        Response.Charset = "";
                                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                        Response.AddHeader("content-disposition", "attachment;filename=productionReport_" + Date + ".xlsx");
                                        // Response.AddHeader("content-disposition", "attachment;filename=Master Report.xlsx");
                                        using (MemoryStream MyMemoryStream = new MemoryStream())
                                        {
                                            wb.SaveAs(MyMemoryStream);
                                            MyMemoryStream.WriteTo(Response.OutputStream);
                                            Response.Flush();
                                            Response.End();
                                        }

                                    }




                                }
                            }
                        }
                    }





                }

                return RedirectToAction("DisplayDetails");
            }
            catch (Exception ex)
            {
                Logger(ex.Message + ex.Source);
                return RedirectToAction("DisplayDetails");
            }
        }





        public ActionResult SendEmail()
        {
            try
            {

                string DirectoryPath = HostingEnvironment.MapPath("~/Documents/");

                string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                string date = string.Empty;
                // DataTable dtCloned=new DataTable();

                string query = "select DATE_FORMAT(date, '%d/%m/%y') as date,productionreport.location, sum(plannedhrs) as hoursplanned ,ROUND(sum(plannedhrrecord),0) as prodplanhrRecord,ROUND(sum(plannedprodrecord),0) as prodplanRecord,ROUND(sum(workedhrs),0) as  RecordsHours,ROUND(sum(actualprodrecord),0) as ActualProdRecords, ROUND((sum(actualprodrecord)/sum(plannedprodrecord))*100,0) as Achievement,ROUND(sum(targetrevenue*plannedprodrecord),2) as TargetRevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)),2) as actualrevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0))/sum(plannedprodrecord*targetrevenue)*100,0) as RevenueAchievement   from `productionreport` group by productionreport.location,date;";
                query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement ,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='TVM';";
                query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KNPY';";
                query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MDS';";
                query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MQC';";
                query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MNS';";
                query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,DATE_FORMAT(date, '%d/%m/%y') as date,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KAKKANAD';";
                ;


                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                //Set Name of DataTables.
                                ds.Tables[0].TableName = "Summary";
                                ds.Tables[1].TableName = "TVM";
                                ds.Tables[2].TableName = "KNPY";
                                ds.Tables[3].TableName = "MDS";
                                ds.Tables[4].TableName = "MQC";
                                ds.Tables[5].TableName = "MNS";
                                ds.Tables[6].TableName = "KAKKANAD";
                                date = Convert.ToDateTime(ds.Tables[0].Rows[0]["date"]).ToString("dd/MM/yyyy");
                                ds.Tables[0].Columns["date"].ColumnName = "Date";
                                ds.Tables[0].Columns["location"].ColumnName = "Location";
                                ds.Tables[0].Columns["hoursplanned"].ColumnName = "Hours planned";
                                ds.Tables[0].Columns["prodplanhrRecord"].ColumnName = "Production planned/Hr Records";
                                ds.Tables[0].Columns["prodplanRecord"].ColumnName = "Production   planned   Records";
                                ds.Tables[0].Columns["RecordsHours"].ColumnName = "Hours worked";
                                ds.Tables[0].Columns["ActualProdRecords"].ColumnName = "Actual Production Records";
                                ds.Tables[0].Columns["Achievement"].ColumnName = "% Achievement";
                                ds.Tables[0].Columns["TargetRevenue"].ColumnName = "Target Revenue INR";
                                ds.Tables[0].Columns["ActualRevenue"].ColumnName = "Actual Revenue INR";
                                ds.Tables[0].Columns["RevenueAchievement"].ColumnName = "% Revenue Achievement";
                                ds.Tables[0].AcceptChanges();





                                ds.Tables[1].Columns["psn"].ColumnName = "PSN";
                                ds.Tables[1].Columns["associate"].ColumnName = "Associates Name";
                                ds.Tables[1].Columns["process"].ColumnName = "Process";
                                ds.Tables[1].Columns["project"].ColumnName = "Project";
                                ds.Tables[1].Columns["projectcode"].ColumnName = "Project Code";
                                ds.Tables[1].Columns["eventcode"].ColumnName = "Event code";
                                ds.Tables[1].Columns["tlname"].ColumnName = "TL's Name";
                                ds.Tables[1].Columns["plannedhrs"].ColumnName = "Hours planned";
                                ds.Tables[1].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                                ds.Tables[1].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                                ds.Tables[1].Columns["workedhrs"].ColumnName = "Hours worked";
                                ds.Tables[1].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                                ds.Tables[1].Columns["achievement"].ColumnName = "% Achievement";
                                ds.Tables[1].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                                ds.Tables[1].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                                ds.Tables[1].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                                ds.Tables[1].Columns["productivity"].ColumnName = "Productivity(per hr)";
                                ds.Tables[1].AcceptChanges();

                                ds.Tables[2].Columns["psn"].ColumnName = "PSN";
                                ds.Tables[2].Columns["associate"].ColumnName = "Associates Name";
                                ds.Tables[2].Columns["process"].ColumnName = "Process";
                                ds.Tables[2].Columns["project"].ColumnName = "Project";
                                ds.Tables[2].Columns["projectcode"].ColumnName = "Project Code";
                                ds.Tables[2].Columns["eventcode"].ColumnName = "Event code";
                                ds.Tables[2].Columns["tlname"].ColumnName = "TL's Name";
                                ds.Tables[2].Columns["plannedhrs"].ColumnName = "Hours planned";
                                ds.Tables[2].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                                ds.Tables[2].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                                ds.Tables[2].Columns["workedhrs"].ColumnName = "Hours worked";
                                ds.Tables[2].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                                ds.Tables[2].Columns["achievement"].ColumnName = "% Achievement";
                                ds.Tables[2].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                                ds.Tables[2].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                                ds.Tables[2].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                                ds.Tables[2].Columns["productivity"].ColumnName = "Productivity(per hr)";
                                ds.Tables[2].AcceptChanges();

                                ds.Tables[3].Columns["psn"].ColumnName = "PSN";
                                ds.Tables[3].Columns["associate"].ColumnName = "Associates Name";
                                ds.Tables[3].Columns["process"].ColumnName = "Process";
                                ds.Tables[3].Columns["project"].ColumnName = "Project";
                                ds.Tables[3].Columns["projectcode"].ColumnName = "Project Code";
                                ds.Tables[3].Columns["eventcode"].ColumnName = "Event code";
                                ds.Tables[3].Columns["tlname"].ColumnName = "TL's Name";
                                ds.Tables[3].Columns["plannedhrs"].ColumnName = "Hours planned";
                                ds.Tables[3].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                                ds.Tables[3].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                                ds.Tables[3].Columns["workedhrs"].ColumnName = "Hours worked";
                                ds.Tables[3].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                                ds.Tables[3].Columns["achievement"].ColumnName = "% Achievement";
                                ds.Tables[3].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                                ds.Tables[3].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                                ds.Tables[3].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                                ds.Tables[3].Columns["productivity"].ColumnName = "Productivity(per hr)";
                                ds.Tables[3].AcceptChanges();


                                ds.Tables[4].Columns["psn"].ColumnName = "PSN";
                                ds.Tables[4].Columns["associate"].ColumnName = "Associates Name";
                                ds.Tables[4].Columns["process"].ColumnName = "Process";
                                ds.Tables[4].Columns["project"].ColumnName = "Project";
                                ds.Tables[4].Columns["projectcode"].ColumnName = "Project Code";
                                ds.Tables[4].Columns["eventcode"].ColumnName = "Event code";
                                ds.Tables[4].Columns["tlname"].ColumnName = "TL's Name";
                                ds.Tables[4].Columns["plannedhrs"].ColumnName = "Hours planned";
                                ds.Tables[4].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                                ds.Tables[4].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                                ds.Tables[4].Columns["workedhrs"].ColumnName = "Hours worked";
                                ds.Tables[4].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                                ds.Tables[4].Columns["achievement"].ColumnName = "% Achievement";
                                ds.Tables[4].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                                ds.Tables[4].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                                ds.Tables[4].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                                ds.Tables[4].Columns["productivity"].ColumnName = "Productivity(per hr)";
                                ds.Tables[4].AcceptChanges();


                                ds.Tables[5].Columns["psn"].ColumnName = "PSN";
                                ds.Tables[5].Columns["associate"].ColumnName = "Associates Name";
                                ds.Tables[5].Columns["process"].ColumnName = "Process";
                                ds.Tables[5].Columns["project"].ColumnName = "Project";
                                ds.Tables[5].Columns["projectcode"].ColumnName = "Project Code";
                                ds.Tables[5].Columns["eventcode"].ColumnName = "Event code";
                                ds.Tables[5].Columns["tlname"].ColumnName = "TL's Name";
                                ds.Tables[5].Columns["plannedhrs"].ColumnName = "Hours planned";
                                ds.Tables[5].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                                ds.Tables[5].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                                ds.Tables[5].Columns["workedhrs"].ColumnName = "Hours worked";
                                ds.Tables[5].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                                ds.Tables[5].Columns["achievement"].ColumnName = "% Achievement";
                                ds.Tables[5].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                                ds.Tables[5].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                                ds.Tables[5].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                                ds.Tables[5].Columns["productivity"].ColumnName = "Productivity(per hr)";
                                ds.Tables[5].AcceptChanges();





                                ds.Tables[6].Columns["associate"].ColumnName = "Associates Name";
                                ds.Tables[6].Columns["process"].ColumnName = "Process";
                                ds.Tables[6].Columns["project"].ColumnName = "Project";
                                ds.Tables[6].Columns["projectcode"].ColumnName = "Project Code";
                                ds.Tables[6].Columns["eventcode"].ColumnName = "Event code";
                                ds.Tables[6].Columns["tlname"].ColumnName = "TL's Name";
                                ds.Tables[6].Columns["plannedhrs"].ColumnName = "Hours planned";
                                ds.Tables[6].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
                                ds.Tables[6].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
                                ds.Tables[6].Columns["workedhrs"].ColumnName = "Hours worked";
                                ds.Tables[6].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
                                ds.Tables[6].Columns["achievement"].ColumnName = "% Achievement";
                                ds.Tables[6].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
                                ds.Tables[6].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
                                ds.Tables[6].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
                                ds.Tables[6].Columns["productivity"].ColumnName = "Productivity(per hr)";
                                ds.Tables[6].AcceptChanges();




                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {


                                        if (dt == ds.Tables[0])
                                        {

                                            decimal hoursplanned = 0;
                                            decimal prodplanhrRecord = 0;
                                            decimal prodplanRecord = 0;
                                            decimal RecordsHours = 0;
                                            decimal ActualProdRecords = 0;
                                            decimal TargetRevenue = 0;
                                            decimal ActualRevenue = 0;



                                            foreach (DataRow row in dt.Rows)
                                            {

                                                string Command = "insert into Consolidatedreport(Date,Location,Plannedhrs,productionplanhrrecord,productionplanrecord,hrworked,ActProdRecord,Achievement,TargetRevenue,ActualRevenue,RevenueAchievement) values('" + row[0].ToString() + "','" + row[1].ToString() + "'," + row[2] + "," + row[3] + "," + row[4] + " , " + row[5] + "," + row[6] + "," + row[7] + "," + row[8] + "," + row[9] + "," + row[10] + ")";

                                                using (MySqlConnection mConnection = new MySqlConnection(constr))
                                                {
                                                    mConnection.Open();
                                                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                                                    {
                                                        myCmd.ExecuteNonQuery();
                                                    }
                                                }








                                                row["date"] = date;


                                                if (row[2].ToString() != "")
                                                {
                                                    hoursplanned += decimal.Parse(row[2].ToString());
                                                }
                                                if (row[3].ToString() != "")
                                                {
                                                    prodplanhrRecord += decimal.Parse(row[3].ToString());
                                                }

                                                if (row[4].ToString() != "")
                                                {
                                                    prodplanRecord += decimal.Parse(row[4].ToString());
                                                }

                                                if (row[5].ToString() != "")
                                                {
                                                    RecordsHours += decimal.Parse(row[5].ToString());
                                                }

                                                if (row[6].ToString() != "")
                                                {
                                                    ActualProdRecords += decimal.Parse(row[6].ToString());
                                                }

                                                if (row[8].ToString() != "")
                                                {
                                                    TargetRevenue += decimal.Parse(row[8].ToString());
                                                }
                                                if (row[9].ToString() != "")
                                                {
                                                    ActualRevenue += decimal.Parse(row[9].ToString());
                                                }
                                            }

                                            dt.Rows.Add("", "All Location", hoursplanned, prodplanhrRecord, prodplanRecord, RecordsHours, ActualProdRecords, Math.Round((ActualProdRecords / prodplanRecord) * 100), TargetRevenue, ActualRevenue, Math.Round((ActualRevenue / TargetRevenue) * 100));

                                        }





                                        if (dt != ds.Tables[0])
                                        {
                                            decimal plannedhrs = 0;
                                            decimal plannedhrrecord = 0;
                                            decimal plannedprodrecord = 0;
                                            decimal workedhrs = 0;
                                            decimal actualprodrecord = 0;
                                            decimal targetrevenue = 0;
                                            decimal actualrevenue = 0;
                                            foreach (DataRow row in dt.Rows)
                                            {
                                                row["date"] = date;

                                                if (row[7].ToString() != "")
                                                {
                                                    plannedhrs += decimal.Parse(row[7].ToString());
                                                }
                                                if (row[8].ToString() != "")
                                                {
                                                    plannedhrrecord += decimal.Parse(row[8].ToString());
                                                }
                                                if (row[9].ToString() != "")
                                                {
                                                    plannedprodrecord += decimal.Parse(row[9].ToString());
                                                }
                                                if (row[10].ToString() != "")
                                                {
                                                    workedhrs += decimal.Parse(row[10].ToString());
                                                }
                                                if (row[11].ToString() != "")
                                                {
                                                    actualprodrecord += decimal.Parse(row[11].ToString());
                                                }
                                                if (row[16].ToString() != "")
                                                {
                                                    targetrevenue += decimal.Parse(row[16].ToString());
                                                }
                                                if (row[17].ToString() != "")
                                                {
                                                    actualrevenue += decimal.Parse(row[17].ToString());
                                                }
                                            }
                                            dt.AcceptChanges();
                                            if (plannedhrs != 0 && plannedhrrecord != 0)
                                            {

                                                dt.Rows.Add("", "", "", "", "", "", "TOTALS", plannedhrs, plannedhrrecord, plannedprodrecord, workedhrs, actualprodrecord, (actualprodrecord / plannedprodrecord) * 100, "", "", "", targetrevenue, actualrevenue, (actualrevenue / targetrevenue) * 100);
                                            }
                                        }



                                        wb.Worksheets.Add(dt);
                                    }
                                    wb.SaveAs(DirectoryPath + "MasterReport.xlsx");
                                    string[] strArr = null;
                                    char[] splitchar = { '/' };
                                    strArr = date.Split(splitchar);
                                    if (strArr.Length > 0)
                                        date = strArr[0] + "." + strArr[1] + "." + strArr[2];



                                }



                            }
                        }
                    }

                    var EmailId = ConfigurationManager.AppSettings["EmailId"];
                    ////creating the object of mailmessage
                    //System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    //mailMessage.From = new MailAddress("mistool@sblinfo.org ");
                    //mailMessage.Subject = "Enclose ALL Location Master Report " + date;
                    //mailMessage.Body = "Please find the attached daily production Report on" + date;
                    //mailMessage.IsBodyHtml = true;
                    //mailMessage.To.Add(new MailAddress(EmailId));


                    ////string DirectoryPath1 = HostingEnvironment.MapPath(DirectoryPath + "MasterReport.xlsx");
                    //string directoryName = Path.GetDirectoryName(DirectoryPath + "MasterReport.xlsx");
                    //// mailMessage.Attachments.Add(new Attachment(DirectoryPath1));

                    //foreach (String filename in Directory.GetFiles(directoryName, "*.xlsx"))
                    //{
                    //    mailMessage.Attachments.Add(new Attachment(filename));

                    //}

                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new MailAddress("mistool@sblinfo.org ");
                    mailMessage.Subject = "Enclose ALL Location Master Report " + date;
                    mailMessage.Body = "Please find the attached daily production Report on" + date;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(new MailAddress(EmailId));
                    MailAddress copy = new MailAddress(ConfigurationManager.AppSettings["C1EmailId"]);
                    MailAddress copy1 = new MailAddress(ConfigurationManager.AppSettings["C2EmailId"]);
                    MailAddress copy2 = new MailAddress(ConfigurationManager.AppSettings["C3EmailId"]);
                    MailAddress copy3 = new MailAddress(ConfigurationManager.AppSettings["C4EmailId"]);
                    mailMessage.CC.Add(copy);
                    mailMessage.CC.Add(copy1);
                    mailMessage.CC.Add(copy2);
                    mailMessage.CC.Add(copy3);
                    //string DirectoryPath1 = HostingEnvironment.MapPath(DirectoryPath + "MasterReport.xlsx");
                    string directoryName = Path.GetDirectoryName(DirectoryPath + "MasterReport.xlsx");
                    // mailMessage.Attachments.Add(new Attachment(DirectoryPath1));

                    foreach (String filename in Directory.GetFiles(directoryName, "*.xlsx"))
                    {
                        mailMessage.Attachments.Add(new Attachment(filename));

                    }

                    SmtpClient smtp = new SmtpClient();
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 587;
                    //smtp.EnableSsl = true;

                    smtp.Host = "relay-hosting.secureserver.net";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;

                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = mailMessage.From.Address;
                    NetworkCred.Password = "x@VDl12639d6";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;

                    smtp.Send(mailMessage);

                    Logger("Send Mail");

                    TempData["Msg"] = "Successfully Send Mail!";


                }


                return View("DailymasterProductionReportIndex");

            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Mail not send!";

                Logger(ex.Message + ex.Source);

                return View("DailymasterProductionReportIndex");
            }

        }








        public ActionResult SendTLEmail()
        {
            try
            {





                List<string> Teamlead = new List<string>();
                string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                string date = string.Empty;
                DataSet ds = new DataSet();
                string[] strArr = null;
                char[] splitchar = { '/' };
                string Command = "SELECT distinct date from productionreport; ";
                using (MySqlConnection mConnection = new MySqlConnection(constr))
                {
                    MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            date = reader.GetString("date");
                        }
                    }
                }



                string query = "SELECT  count(*) as usercount FROM muser  where `Roleid`=2 and muser.isactive=true;";
                query += "select  count(distinct `teamleadid`) as ccntteam from production where date='" + date + "';";
                query += "select  Id,username from muser  where `Roleid`=2 and muser.isactive=true;";
                query += "select  distinct `teamleadid`  from production where date='" + date + "';";







                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);

                            if (ds.Tables[0].Rows[0]["usercount"].ToString() != ds.Tables[1].Rows[0]["ccntteam"].ToString())
                            {


                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[2].Rows)
                                    {
                                        Teamlead.Add(dr["Id"].ToString());


                                    }

                                }




                                if (ds.Tables[3].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[3].Rows)
                                    {

                                        bool exists = Teamlead.Exists(element => element == dr["teamleadid"].ToString());
                                        Teamlead.Remove(dr["teamleadid"].ToString());

                                    }
                                }


                            }




                        }
                    }
                }
                string EmailId = string.Empty;
                for (int i = 0; i < Teamlead.Count; i++)
                {



                    string Ccommand = "SElECT `Emailid` from  `muser` where `Id`=" + Teamlead[i] + "; ";
                    using (MySqlConnection mConnection = new MySqlConnection(constr))
                    {
                        MySqlCommand cmd = new MySqlCommand(Ccommand, mConnection);
                        mConnection.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EmailId = reader.GetString("Emailid");
                            }
                        }
                    }







                    //creating the object of mailmessage
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new MailAddress("mistool@sblinfo.org ");
                    mailMessage.Subject = "Upload your Daily Production Report " + date;
                    mailMessage.Body = "Please upload your daily production Report on" + date;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(new MailAddress(EmailId));



                    SmtpClient smtp = new SmtpClient();
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 587;
                    //smtp.EnableSsl = true;

                    smtp.Host = "relay-hosting.secureserver.net";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;

                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = mailMessage.From.Address;
                    NetworkCred.Password = "x@VDl12639d6";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;

                    smtp.Send(mailMessage);


                }

                TempData["Msg"] = "Successfully Send Mail!";





                return View("DailymasterProductionReportIndex");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = "Mail not send!";

                Logger(ex.Message + ex.Source);

                return View("DailymasterProductionReportIndex");
            }

        }

        //   public ActionResult SendEmail()
        //   {
        //       try
        //       {

        //           string DirectoryPath = HostingEnvironment.MapPath("~/Documents/");

        //           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
        //           string date = string.Empty;


        //           string query = "select date,productionreport.location, sum(plannedhrs) as hoursplanned ,ROUND(sum(plannedhrrecord),0) as prodplanhrRecord,ROUND(sum(plannedprodrecord),0) as prodplanRecord,ROUND(sum(workedhrs),0) as  RecordsHours,ROUND(sum(actualprodrecord),0) as ActualProdRecords, ROUND((sum(actualprodrecord)/sum(plannedprodrecord))*100,0) as Achievement,ROUND(sum(targetrevenue*plannedprodrecord),2) as TargetRevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)),2) as actualrevenue,ROUND(SUM(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0))/sum(plannedprodrecord*targetrevenue)*100,2) as RevenueAchievement   from `productionreport` group by productionreport.location,date;";
        //           query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement ,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='TVM';";
        //           query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KNPY';";
        //           query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MDS';";
        //           query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MQC';";
        //           query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='MNS';";
        //           query += "SELECT  `psn`,`associate`,`process`,`project`,`projectcode`,`eventcode`,`tlname`,`plannedhrs`,`plannedhrrecord`,`plannedprodrecord`,`workedhrs`,`actualprodrecord`,ROUND((`actualprodrecord`/`plannedprodrecord`)*100,0) as achievement,`remarks`,`location`,`date`,targetrevenue*plannedprodrecord as targetrevenue ,IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0) as actualrevenue,(IFNULL(targetrevenue*actualprodrecord,0) + IFNULL(targetrevenue*workathome,0)/(targetrevenue*plannedprodrecord)*100) as revenueachievement,CONCAT(projectcode,eventcode) as pecode,ROUND((`actualprodrecord`/`workedhrs`),0) as productivity FROM `productionreport` where location='KAKKANAD';";
        //           ;


        //           using (MySqlConnection con = new MySqlConnection(constr))
        //           {
        //               using (MySqlCommand cmd = new MySqlCommand(query))
        //               {
        //                   using (MySqlDataAdapter sda = new MySqlDataAdapter())
        //                   {
        //                       cmd.Connection = con;
        //                       sda.SelectCommand = cmd;
        //                       using (DataSet ds = new DataSet())
        //                       {
        //                           sda.Fill(ds);

        //                           //Set Name of DataTables.
        //                           ds.Tables[0].TableName = "Summary";
        //                           ds.Tables[1].TableName = "TVM";
        //                           ds.Tables[2].TableName = "KNPY";
        //                           ds.Tables[3].TableName = "MDS";
        //                           ds.Tables[4].TableName = "MQC";
        //                           ds.Tables[5].TableName = "MNS";
        //                           ds.Tables[6].TableName = "KAKKANAD";
        //                           date = ds.Tables[0].Rows[0]["date"].ToString();
        //                           ds.Tables[0].Columns["date"].ColumnName = "Date";
        //                           ds.Tables[0].Columns["location"].ColumnName = "Location";
        //                           ds.Tables[0].Columns["hoursplanned"].ColumnName = "Hours planned";
        //                           ds.Tables[0].Columns["prodplanhrRecord"].ColumnName = "Production planned/Hr Records";
        //                           ds.Tables[0].Columns["prodplanRecord"].ColumnName = "Production   planned   Records";
        //                           ds.Tables[0].Columns["RecordsHours"].ColumnName = "Hours worked";
        //                           ds.Tables[0].Columns["ActualProdRecords"].ColumnName = "Actual Production Records";
        //                           ds.Tables[0].Columns["Achievement"].ColumnName = "% Achievement";
        //                           ds.Tables[0].Columns["TargetRevenue"].ColumnName = "Target Revenue INR";
        //                           ds.Tables[0].Columns["ActualRevenue"].ColumnName = "Actual Revenue INR";
        //                           ds.Tables[0].Columns["RevenueAchievement"].ColumnName = "% Revenue Achievement";
        //                           ds.Tables[0].AcceptChanges();





        //                           ds.Tables[1].Columns["psn"].ColumnName = "PSN";
        //                           ds.Tables[1].Columns["associate"].ColumnName = "Associates Name";
        //                           ds.Tables[1].Columns["process"].ColumnName = "Process";
        //                           ds.Tables[1].Columns["project"].ColumnName = "Project";
        //                           ds.Tables[1].Columns["projectcode"].ColumnName = "Project Code";
        //                           ds.Tables[1].Columns["eventcode"].ColumnName = "Event code";
        //                           ds.Tables[1].Columns["tlname"].ColumnName = "TL's Name";
        //                           ds.Tables[1].Columns["plannedhrs"].ColumnName = "Hours planned";
        //                           ds.Tables[1].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
        //                           ds.Tables[1].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
        //                           ds.Tables[1].Columns["workedhrs"].ColumnName = "Hours worked";
        //                           ds.Tables[1].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
        //                           ds.Tables[1].Columns["achievement"].ColumnName = "% Achievement";
        //                           ds.Tables[1].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
        //                           ds.Tables[1].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
        //                           ds.Tables[1].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
        //                           ds.Tables[1].Columns["productivity"].ColumnName = "Productivity(per hr)";
        //                           ds.Tables[1].AcceptChanges();

        //                           ds.Tables[2].Columns["psn"].ColumnName = "PSN";
        //                           ds.Tables[2].Columns["associate"].ColumnName = "Associates Name";
        //                           ds.Tables[2].Columns["process"].ColumnName = "Process";
        //                           ds.Tables[2].Columns["project"].ColumnName = "Project";
        //                           ds.Tables[2].Columns["projectcode"].ColumnName = "Project Code";
        //                           ds.Tables[2].Columns["eventcode"].ColumnName = "Event code";
        //                           ds.Tables[2].Columns["tlname"].ColumnName = "TL's Name";
        //                           ds.Tables[2].Columns["plannedhrs"].ColumnName = "Hours planned";
        //                           ds.Tables[2].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
        //                           ds.Tables[2].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
        //                           ds.Tables[2].Columns["workedhrs"].ColumnName = "Hours worked";
        //                           ds.Tables[2].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
        //                           ds.Tables[2].Columns["achievement"].ColumnName = "% Achievement";
        //                           ds.Tables[2].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
        //                           ds.Tables[2].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
        //                           ds.Tables[2].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
        //                           ds.Tables[2].Columns["productivity"].ColumnName = "Productivity(per hr)";
        //                           ds.Tables[2].AcceptChanges();

        //                           ds.Tables[3].Columns["psn"].ColumnName = "PSN";
        //                           ds.Tables[3].Columns["associate"].ColumnName = "Associates Name";
        //                           ds.Tables[3].Columns["process"].ColumnName = "Process";
        //                           ds.Tables[3].Columns["project"].ColumnName = "Project";
        //                           ds.Tables[3].Columns["projectcode"].ColumnName = "Project Code";
        //                           ds.Tables[3].Columns["eventcode"].ColumnName = "Event code";
        //                           ds.Tables[3].Columns["tlname"].ColumnName = "TL's Name";
        //                           ds.Tables[3].Columns["plannedhrs"].ColumnName = "Hours planned";
        //                           ds.Tables[3].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
        //                           ds.Tables[3].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
        //                           ds.Tables[3].Columns["workedhrs"].ColumnName = "Hours worked";
        //                           ds.Tables[3].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
        //                           ds.Tables[3].Columns["achievement"].ColumnName = "% Achievement";
        //                           ds.Tables[3].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
        //                           ds.Tables[3].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
        //                           ds.Tables[3].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
        //                           ds.Tables[3].Columns["productivity"].ColumnName = "Productivity(per hr)";
        //                           ds.Tables[3].AcceptChanges();


        //                           ds.Tables[4].Columns["psn"].ColumnName = "PSN";
        //                           ds.Tables[4].Columns["associate"].ColumnName = "Associates Name";
        //                           ds.Tables[4].Columns["process"].ColumnName = "Process";
        //                           ds.Tables[4].Columns["project"].ColumnName = "Project";
        //                           ds.Tables[4].Columns["projectcode"].ColumnName = "Project Code";
        //                           ds.Tables[4].Columns["eventcode"].ColumnName = "Event code";
        //                           ds.Tables[4].Columns["tlname"].ColumnName = "TL's Name";
        //                           ds.Tables[4].Columns["plannedhrs"].ColumnName = "Hours planned";
        //                           ds.Tables[4].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
        //                           ds.Tables[4].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
        //                           ds.Tables[4].Columns["workedhrs"].ColumnName = "Hours worked";
        //                           ds.Tables[4].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
        //                           ds.Tables[4].Columns["achievement"].ColumnName = "% Achievement";
        //                           ds.Tables[4].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
        //                           ds.Tables[4].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
        //                           ds.Tables[4].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
        //                           ds.Tables[4].Columns["productivity"].ColumnName = "Productivity(per hr)";
        //                           ds.Tables[4].AcceptChanges();


        //                           ds.Tables[5].Columns["psn"].ColumnName = "PSN";
        //                           ds.Tables[5].Columns["associate"].ColumnName = "Associates Name";
        //                           ds.Tables[5].Columns["process"].ColumnName = "Process";
        //                           ds.Tables[5].Columns["project"].ColumnName = "Project";
        //                           ds.Tables[5].Columns["projectcode"].ColumnName = "Project Code";
        //                           ds.Tables[5].Columns["eventcode"].ColumnName = "Event code";
        //                           ds.Tables[5].Columns["tlname"].ColumnName = "TL's Name";
        //                           ds.Tables[5].Columns["plannedhrs"].ColumnName = "Hours planned";
        //                           ds.Tables[5].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
        //                           ds.Tables[5].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
        //                           ds.Tables[5].Columns["workedhrs"].ColumnName = "Hours worked";
        //                           ds.Tables[5].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
        //                           ds.Tables[5].Columns["achievement"].ColumnName = "% Achievement";
        //                           ds.Tables[5].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
        //                           ds.Tables[5].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
        //                           ds.Tables[5].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
        //                           ds.Tables[5].Columns["productivity"].ColumnName = "Productivity(per hr)";
        //                           ds.Tables[5].AcceptChanges();





        //                           ds.Tables[6].Columns["associate"].ColumnName = "Associates Name";
        //                           ds.Tables[6].Columns["process"].ColumnName = "Process";
        //                           ds.Tables[6].Columns["project"].ColumnName = "Project";
        //                           ds.Tables[6].Columns["projectcode"].ColumnName = "Project Code";
        //                           ds.Tables[6].Columns["eventcode"].ColumnName = "Event code";
        //                           ds.Tables[6].Columns["tlname"].ColumnName = "TL's Name";
        //                           ds.Tables[6].Columns["plannedhrs"].ColumnName = "Hours planned";
        //                           ds.Tables[6].Columns["plannedhrrecord"].ColumnName = "Production planned/Hr Records";
        //                           ds.Tables[6].Columns["plannedprodrecord"].ColumnName = "Production planned  Records";
        //                           ds.Tables[6].Columns["workedhrs"].ColumnName = "Hours worked";
        //                           ds.Tables[6].Columns["actualprodrecord"].ColumnName = "Actual Production Records";
        //                           ds.Tables[6].Columns["achievement"].ColumnName = "% Achievement";
        //                           ds.Tables[6].Columns["revenueachievement"].ColumnName = " % Revenue Achievement";
        //                           ds.Tables[6].Columns["targetrevenue"].ColumnName = "TARGET REVENUE INR";
        //                           ds.Tables[6].Columns["actualrevenue"].ColumnName = "ACTUAL REVENUE INR";
        //                           ds.Tables[6].Columns["productivity"].ColumnName = "Productivity(per hr)";
        //                           ds.Tables[6].AcceptChanges();




        //                           using (XLWorkbook wb = new XLWorkbook())
        //                           {
        //                               foreach (DataTable dt in ds.Tables)
        //                               {

        //                                   if (dt == ds.Tables[0])
        //                                   {


        //                                       string Checkcommand = "SElECT * from  `Consolidatedreport` where `Date`='" + dt.Rows[0]["date"] + "'; ";
        //                                       using (MySqlConnection mConnection = new MySqlConnection(constr))
        //                                       {
        //                                           MySqlCommand cmdquery = new MySqlCommand(Checkcommand, mConnection);
        //                                           mConnection.Open();
        //                                           MySqlDataReader cmdreader = cmdquery.ExecuteReader();

        //                                           if (cmdreader.HasRows)
        //                                           {
        //                                               MySqlCommand cmddelquery = new MySqlCommand("Delete from  `Consolidatedreport` where `Date`='" + dt.Rows[0]["date"] + "'; ", mConnection);
        //                                               cmddelquery.ExecuteNonQuery();
        //                                           }

        //                                           else
        //                                           {

        //                                               decimal hoursplanned = 0;
        //                                               decimal prodplanhrRecord = 0;
        //                                               decimal prodplanRecord = 0;
        //                                               decimal RecordsHours = 0;
        //                                               decimal ActualProdRecords = 0;
        //                                               decimal TargetRevenue = 0;
        //                                               decimal ActualRevenue = 0;
        //                                               foreach (DataRow row in dt.Rows)
        //                                               {



        //                                                   string cmdinsertquery = @"insert into Consolidatedreport (Date ,location, `Plannedhrs`, `productionplanhrrecord`, `productionplanrecord`,`hrworked`,`ActProdRecord`,`Achievement`,`TargetRevenue`,`ActualRevenue`,`RevenueAchievement`,`Disporder`)
        //		                                    values ('" + row[0].ToString() + "' ,'" + row[1].ToString() + "', " + row[2] + "," + row[3] + "," +  row[4] + " ," + row[5] + "," + row[6] + "," + row[7] + "," + row[8] + "," + row[9] + "," + row[10] + ",10 ) ";
        //                                                   MySqlCommand sCommand = new MySqlCommand(cmdinsertquery, con);
        //                                                   sCommand.ExecuteNonQuery();





        //                                                   if (row[2].ToString() != "")
        //                                                   {
        //                                                       hoursplanned += decimal.Parse(row[2].ToString());
        //                                                   }
        //                                                   if (row[3].ToString() != "")
        //                                                   {
        //                                                       prodplanhrRecord += decimal.Parse(row[3].ToString());
        //                                                   }

        //                                                   if (row[4].ToString() != "")
        //                                                   {
        //                                                       prodplanRecord += decimal.Parse(row[4].ToString());
        //                                                   }

        //                                                   if (row[5].ToString() != "")
        //                                                   {
        //                                                       RecordsHours += decimal.Parse(row[5].ToString());
        //                                                   }

        //                                                   if (row[6].ToString() != "")
        //                                                   {
        //                                                       ActualProdRecords += decimal.Parse(row[6].ToString());
        //                                                   }

        //                                                   if (row[8].ToString() != "")
        //                                                   {
        //                                                       TargetRevenue += decimal.Parse(row[8].ToString());
        //                                                   }
        //                                                   if (row[9].ToString() != "")
        //                                                   {
        //                                                       ActualRevenue += decimal.Parse(row[9].ToString());
        //                                                   }
        //                                                   dt.Rows.Add("", "All Location", hoursplanned, prodplanhrRecord, prodplanRecord, RecordsHours, ActualProdRecords, Math.Round((ActualProdRecords / prodplanRecord) * 100), TargetRevenue, ActualRevenue, Math.Round((ActualRevenue / TargetRevenue) * 100));

        //                                               }
        //                                           }

        // }
        //                                   }





        //                                   if (dt != ds.Tables[0])
        //                                   {
        //                                       decimal plannedhrs = 0;
        //                                       decimal plannedhrrecord = 0;
        //                                       decimal plannedprodrecord = 0;
        //                                       decimal workedhrs = 0;
        //                                       decimal actualprodrecord = 0;
        //                                       decimal targetrevenue = 0;
        //                                       decimal actualrevenue = 0;
        //                                       foreach (DataRow row in dt.Rows)
        //                                       {
        //                                           if (row[7].ToString() != "")
        //                                           {
        //                                               plannedhrs += decimal.Parse(row[7].ToString());
        //                                           }
        //                                           if (row[8].ToString() != "")
        //                                           {
        //                                               plannedhrrecord += decimal.Parse(row[8].ToString());
        //                                           }
        //                                           if (row[9].ToString() != "")
        //                                           {
        //                                               plannedprodrecord += decimal.Parse(row[9].ToString());
        //                                           }
        //                                           if (row[10].ToString() != "")
        //                                           {
        //                                               workedhrs += decimal.Parse(row[10].ToString());
        //                                           }
        //                                           if (row[11].ToString() != "")
        //                                           {
        //                                               actualprodrecord += decimal.Parse(row[11].ToString());
        //                                           }
        //                                           if (row[16].ToString() != "")
        //                                           {
        //                                               targetrevenue += decimal.Parse(row[16].ToString());
        //                                           }
        //                                           if (row[17].ToString() != "")
        //                                           {
        //                                               actualrevenue += decimal.Parse(row[17].ToString());
        //                                           }
        //                                       }

        //                                       if (plannedhrs != 0 && plannedhrrecord != 0)
        //                                       {

        //                                           dt.Rows.Add("", "", "", "", "", "", "TOTALS", plannedhrs, plannedhrrecord, plannedprodrecord, workedhrs, actualprodrecord, Math.Round((actualprodrecord / plannedprodrecord) * 100,2), "", "", "", Math.Round(targetrevenue,2),Math.Round(actualrevenue,2), Math.Round((actualrevenue / targetrevenue) * 100),2);
        //                                       }
        //                                   }



        //                                   wb.Worksheets.Add(dt);
        //                               }
        //                               wb.SaveAs(DirectoryPath + "MasterReport.xlsx");
        //                               string[] strArr = null;
        //                               char[] splitchar = { '/' };
        //                               strArr = date.Split(splitchar);
        //                               if (strArr.Length > 0)
        //                                   date = strArr[1] + "." + strArr[0] + "." + strArr[2];



        //                           }



        //                       }
        //                   }
        //               }

        //             var EmailId = ConfigurationManager.AppSettings["EmailId"];

        //             //   var EmailId = "nisha.v@sblcorp.com"; ;
        //               //creating the object of mailmessage
        //               System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
        //               mailMessage.From = new MailAddress("mistool@sblinfo.org ");
        //               mailMessage.Subject = "Enclose ALL Location Master Report " + date;
        //               mailMessage.Body = "Please find the attached daily production Report on" + date;
        //               mailMessage.IsBodyHtml = true;
        //               mailMessage.To.Add(new MailAddress(EmailId));
        //               MailAddress copy = new MailAddress(ConfigurationManager.AppSettings["C1EmailId"]);
        //               MailAddress copy1 = new MailAddress(ConfigurationManager.AppSettings["C2EmailId"]);
        //               MailAddress copy2 = new MailAddress(ConfigurationManager.AppSettings["C3EmailId"]);
        //               MailAddress copy3 = new MailAddress(ConfigurationManager.AppSettings["C4EmailId"]);
        //               mailMessage.CC.Add(copy);
        //               mailMessage.CC.Add(copy1);
        //               mailMessage.CC.Add(copy2);
        //               mailMessage.CC.Add(copy3);
        //               //string DirectoryPath1 = HostingEnvironment.MapPath(DirectoryPath + "MasterReport.xlsx");
        //               string directoryName = Path.GetDirectoryName(DirectoryPath + "MasterReport.xlsx");
        //               // mailMessage.Attachments.Add(new Attachment(DirectoryPath1));

        //               foreach (String filename in Directory.GetFiles(directoryName, "*.xlsx"))
        //               {
        //                   mailMessage.Attachments.Add(new Attachment(filename));

        //               }
        //               SmtpClient smtp = new SmtpClient();
        //               smtp.Host = "smtp.gmail.com";
        //               smtp.Port = 587;
        //               smtp.EnableSsl = true;

        //               //smtp.Host = "relay-hosting.secureserver.net";
        //               //smtp.Port = 25;
        //               //smtp.EnableSsl = false;

        //               NetworkCredential NetworkCred = new NetworkCredential();
        //               NetworkCred.UserName = mailMessage.From.Address;
        //               NetworkCred.Password = "x@VDl12639d6";
        //               smtp.UseDefaultCredentials = true;
        //               smtp.Credentials = NetworkCred;

        //               smtp.Send(mailMessage);

        //               Logger("Send Mail");

        //               TempData["Msg"] = "Successfully Send Mail!";


        //           }


        //           return View("DailymasterProductionReportIndex");

        //       }
        //       catch (Exception ex)
        //       {
        //           TempData["Msg"] = "Mail not send!";

        //           Logger(ex.Message + ex.Source);

        //           return View("DailymasterProductionReportIndex");
        //       }

        //   }


        //               System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
        //               mailMessage.From = new MailAddress("mistool@sblinfo.org ");
        //               mailMessage.Subject = "Enclose ALL Location Master Report " + date;
        //               mailMessage.Body = "Please find the attached daily production Report on" + date;
        //               mailMessage.IsBodyHtml = true;
        //               mailMessage.To.Add(new MailAddress(EmailId));
        //               MailAddress copy = new MailAddress(ConfigurationManager.AppSettings["C1EmailId"]);
        //               MailAddress copy1 = new MailAddress(ConfigurationManager.AppSettings["C2EmailId"]);
        //               MailAddress copy2 = new MailAddress(ConfigurationManager.AppSettings["C3EmailId"]);
        //               MailAddress copy3 = new MailAddress(ConfigurationManager.AppSettings["C4EmailId"]);
        //               mailMessage.CC.Add(copy);
        //               mailMessage.CC.Add(copy1);
        //               mailMessage.CC.Add(copy2);
        //               mailMessage.CC.Add(copy3);
        //               //string DirectoryPath1 = HostingEnvironment.MapPath(DirectoryPath + "MasterReport.xlsx");
        //               string directoryName = Path.GetDirectoryName(DirectoryPath + "MasterReport.xlsx");
        //               // mailMessage.Attachments.Add(new Attachment(DirectoryPath1));

        //               foreach (String filename in Directory.GetFiles(directoryName, "*.xlsx"))
        //               {
        //                   mailMessage.Attachments.Add(new Attachment(filename));

        //               }
        //               SmtpClient smtp = new SmtpClient();
        //               smtp.Host = "smtp.gmail.com";
        //               smtp.Port = 587;
        //               smtp.EnableSsl = true;

        //               //smtp.Host = "relay-hosting.secureserver.net";
        //               //smtp.Port = 25;
        //               //smtp.EnableSsl = false;

        //               NetworkCredential NetworkCred = new NetworkCredential();
        //               NetworkCred.UserName = mailMessage.From.Address;
        //               NetworkCred.Password = "x@VDl12639d6";
        //               smtp.UseDefaultCredentials = true;
        //               smtp.Credentials = NetworkCred;

        //               smtp.Send(mailMessage);





        public ActionResult MonthlyTarget()
        {
            //return View("MonthlyTargetedrecords");

            Targetrevenue Model = new Targetrevenue();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //string Command = "SELECT     MONTHNAME(STR_TO_DATE(date, '%m/%d/%Y')) as month,location, sum(targetrevenue) as targetrevenue,sum(actualrevenue) as actualrevenue  from MonthlylocTarAct group by MONTHNAME(STR_TO_DATE(date, '%m/%d/%Y')),location;";
            string Command = " SELECT  disporder,  MONTHNAME(STR_TO_DATE(date, '%m/%d/%Y')) as month,location, sum(targetrevenue) as targetrevenue,sum(actualrevenue) as actualrevenue  from `Consolidatedreport` group by disporder, MONTHNAME(STR_TO_DATE(date, '%m/%d/%Y')),location;";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.TargetrevenueList = dtt.DataTableToList<Targetrevenue>();
                return View("MonthlyTargetedrecords", Model);
            }




        }


        public ActionResult MonthlyReport(string fromdate, string todate)
        {
            MonthlyRecord Model = new MonthlyRecord();

            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetTargetVsActual", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromdate", fromdate);
                    cmd.Parameters.AddWithValue("@todate", todate);
                    cmd.CommandTimeout = 9600; ;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        DataTable dtt = ds.Tables[0];
                        Model.LstMonthrecord = dtt.DataTableToList<MonthlyRecord>();
                    }
                }
            }


            return PartialView("_MonthlyRecord", Model);
        }


        public static void VerifyDir(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch { }
        }

        public static void Logger(string lines)
        {


            string path = HostingEnvironment.MapPath("~/Documents/");
            VerifyDir(path);
            string fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "_Logs.txt";
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(path + fileName, true);
                file.WriteLine(DateTime.Now.ToString() + ": " + lines);
                file.Close();
            }
            catch (Exception) { }
        }

        public JsonResult CalculateProductionRecords(string Project, string Event, string Process, string Hrcount)
        {

            string monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            var Location = new object(); ;

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            string Commandloc = "SELECT location from memployee where TL='" + Session["DisplayName"].ToString() + "'";
            using (MySqlConnection mlocConnection = new MySqlConnection(connString))
            {
                MySqlCommand cmdloc = new MySqlCommand(Commandloc, mlocConnection);
                mlocConnection.Open();

                Location = cmdloc.ExecuteScalar();
                mlocConnection.Close();
                //return (getValue == null) ? string.Empty : getValue.ToString();


            }

            double productionplanhr = 0.0;
            string Command = "SELECT `ProductionPlannedHr` from  `projectconfiguration` where `Projectcode`='" + Project + "' and `Eventcode`='" + Event + "' and Process='" + Process + "'  and monthname='" + monthname + "' and location='" + Location + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                mConnection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        productionplanhr = reader.GetDouble("ProductionPlannedHr");
                    }
                }
            }




            return Json(Math.Round(productionplanhr, 2), JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public ActionResult EditAttendance(SummaryModel model, string dateFrom)
        {

            try
            {

                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                string psn = string.Empty;
                string Leave = string.Empty;
                string projectcode = string.Empty;
                string eventcode = string.Empty;
                string process = string.Empty;
                string prodplannedhrs = string.Empty;
                string Command = string.Empty;
                string Actualhrs = string.Empty;
                string Associate = string.Empty;
                string project = string.Empty;
                string[] words = new String[2];
                int ActualProduction = 0;
                DateTime dtdate = new DateTime();
                dtdate = DateTime.ParseExact(dateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                var date = dtdate.ToString("yyyy-MM-dd");
                string user = System.Web.HttpContext.Current.User.Identity.Name;

                string CCommand = "Delete from `production` where date ='" + date + "'  and  teamleadid =" + int.Parse(user.ToString()) + " and id<>0;";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(CCommand, mConnection))
                    {
                        myCmd.ExecuteNonQuery();
                    }
                    mConnection.Close();
                }

                for (int i = 0; i < model.EmployeeList.Count; i++)
                {

                    string strName = string.Empty;
                    //if (model.EmployeeList[i].Leave.ToString() == "P" || model.EmployeeList[i].Leave.ToString() == "PL" || model.EmployeeList[i].Leave.ToString() == "UPL" || model.EmployeeList[i].Leave.ToString() == "HUPL") ;
                    //{
                    strName = model.EmployeeList[i].Leave == null ? null : model.EmployeeList[i].Leave;

                    if (strName != null)
                    {
                        if (model.EmployeeList[i].project != null)
                        {
                            project = model.EmployeeList[i].project.ToString();
                        }
                        else
                        {
                            project = "";

                        }

                        if (model.EmployeeList[i].projectcode != null)
                        {
                            words = model.EmployeeList[i].projectcode.Split('-');
                        }
                        else
                        {
                            words[0] = null;
                            words[1] = null;
                        }

                        if (model.EmployeeList[i].projectcode != null)
                        {
                            words = model.EmployeeList[i].projectcode.Split('-');
                        }
                        else
                        {
                            words[0] = null;
                            words[1] = null;
                        }


                        psn = model.EmployeeList[i].PSN.ToString();
                        process = model.EmployeeList[i].process.ToString();

                        projectcode = words[0];
                        eventcode = words[1];
                        ActualProduction = int.Parse(model.EmployeeList[i].ActualProduction.ToString());

                        if (model.EmployeeList[i].Leave.ToString() == "P" || model.EmployeeList[i].Leave.ToString() == "HPL" || model.EmployeeList[i].Leave.ToString() == "HUPL")
                            // Command = "INSERT INTO `production`(`psn`,`process`,project,  ,`hoursplanned`,hoursworked,Actualproduction,Remarks,location,date,teamleadid,workathome) VALUES ('" + model.EmployeeList[i].PSN.ToString() + "','" + model.EmployeeList[i].process.ToString() + "','" + model.EmployeeList[i].project.ToString() + "','" + words[0].ToString() + "','" + words[1].ToString() + "','" + model.EmployeeList[i].hoursplanned.ToString() + "','" + model.EmployeeList[i].hoursworked.ToString() + "','" + model.EmployeeList[i].ActualProduction.ToString() + "','" + model.EmployeeList[i].Leave.ToString() + "','KAKKANAD','" + dateFrom + "',1600 ," + model.EmployeeList[i].workathome + ");";
                            Command = "INSERT INTO `production`(`psn`,`process`,project,`projectcode`,`eventcode`,`hoursplanned`,hoursworked,Actualproduction,Remarks,location,date,teamleadid,workathome) VALUES ('" + psn + "','" + process + "','" + project + "','" + projectcode + "','" + eventcode + "','" + model.EmployeeList[i].hoursplanned.ToString() + "','" + model.EmployeeList[i].hoursworked.ToString() + "'," + ActualProduction + ",'" + model.EmployeeList[i].Leave.ToString() + "','" + Session["location"].ToString() + "','" + date + "'," + int.Parse(user.ToString()) + "," + model.EmployeeList[i].workathome + ");";


                        else
                            Command = "INSERT INTO `production`(`psn`,Remarks,location,date,teamleadid,workathome) VALUES ('" + model.EmployeeList[i].PSN.ToString() + "','" + model.EmployeeList[i].Leave.ToString() + "','" + Session["location"].ToString() + "','" + date + "'," + int.Parse(Session["UserId"].ToString()) + " ,0);";

                        using (MySqlConnection mConnection = new MySqlConnection(connString))
                        {
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                            {
                                myCmd.ExecuteNonQuery();
                            }
                        }
                    }

                }






                TempData["Msg"] = "Successfully Updated!";
                return RedirectToAction("DisplayDetails");

            }

            catch (Exception ex)
            {
                TempData["Msg"] = "Data not Saved!";
                return RedirectToAction("DisplayDetails");
            }
        }









        [HttpPost]
        public ActionResult SaveAttendance(SummaryModel model, string dateFrom)
        {

            try
            {

                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                string psn = string.Empty;
                string Leave = string.Empty;
                string projectcode = string.Empty;
                string eventcode = string.Empty;
                string process = string.Empty;
                string prodplannedhrs = string.Empty;
                string Command = string.Empty;
                string Actualhrs = string.Empty;
                string Associate = string.Empty;
                string project = string.Empty;
                string[] words = new String[2];
                int ActualProduction = 0;
                DateTime dtdate = new DateTime();
                dtdate = DateTime.ParseExact(dateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                var date = dtdate.ToString("yyyy-MM-dd");


                for (int i = 0; i < model.EmployeeList.Count; i++)
                {

                    string strName = string.Empty;
                    //if (model.EmployeeList[i].Leave.ToString() == "P" || model.EmployeeList[i].Leave.ToString() == "PL" || model.EmployeeList[i].Leave.ToString() == "UPL" || model.EmployeeList[i].Leave.ToString() == "HUPL") ;
                    //{
                    strName = model.EmployeeList[i].Leave == null ? null : model.EmployeeList[i].Leave;

                    if (strName != null)
                    {
                        if (model.EmployeeList[i].project != null)
                        {
                            project = model.EmployeeList[i].project.ToString();
                        }
                        else
                        {
                            project = "";

                        }

                        if (model.EmployeeList[i].projectcode != null)
                        {
                            words = model.EmployeeList[i].projectcode.Split('-');
                        }
                        else
                        {
                            words[0] = null;
                            words[1] = null;
                        }

                        if (model.EmployeeList[i].projectcode != null)
                        {
                            words = model.EmployeeList[i].projectcode.Split('-');
                        }
                        else
                        {
                            words[0] = null;
                            words[1] = null;
                        }


                        psn = model.EmployeeList[i].PSN.ToString();
                        process = model.EmployeeList[i].process.ToString();

                        projectcode = words[0];
                        eventcode = words[1];
                        ActualProduction = int.Parse(model.EmployeeList[i].ActualProduction.ToString());
                        string user = System.Web.HttpContext.Current.User.Identity.Name;
                        if (model.EmployeeList[i].Leave.ToString() == "P" || model.EmployeeList[i].Leave.ToString() == "HPL" || model.EmployeeList[i].Leave.ToString() == "HUPL")
                            // Command = "INSERT INTO `production`(`psn`,`process`,project,  ,`hoursplanned`,hoursworked,Actualproduction,Remarks,location,date,teamleadid,workathome) VALUES ('" + model.EmployeeList[i].PSN.ToString() + "','" + model.EmployeeList[i].process.ToString() + "','" + model.EmployeeList[i].project.ToString() + "','" + words[0].ToString() + "','" + words[1].ToString() + "','" + model.EmployeeList[i].hoursplanned.ToString() + "','" + model.EmployeeList[i].hoursworked.ToString() + "','" + model.EmployeeList[i].ActualProduction.ToString() + "','" + model.EmployeeList[i].Leave.ToString() + "','KAKKANAD','" + dateFrom + "',1600 ," + model.EmployeeList[i].workathome + ");";
                            Command = "INSERT INTO `production`(`psn`,`process`,project,`projectcode`,`eventcode`,`hoursplanned`,hoursworked,Actualproduction,Remarks,location,date,teamleadid,workathome) VALUES ('" + psn + "','" + process + "','" + project + "','" + projectcode + "','" + eventcode + "','" + model.EmployeeList[i].hoursplanned.ToString() + "','" + model.EmployeeList[i].hoursworked.ToString() + "'," + ActualProduction + ",'" + model.EmployeeList[i].Leave.ToString() + "','" + Session["location"].ToString() + "','" + date + "'," + int.Parse(user.ToString()) + "," + model.EmployeeList[i].workathome + ");";


                        else
                            Command = "INSERT INTO `production`(`psn`,Remarks,location,date,teamleadid,workathome) VALUES ('" + model.EmployeeList[i].PSN.ToString() + "','" + model.EmployeeList[i].Leave.ToString() + "','" + Session["location"].ToString() + "','" + date + "'," + int.Parse(Session["UserId"].ToString()) + " ,0);";

                        using (MySqlConnection mConnection = new MySqlConnection(connString))
                        {
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                            {
                                myCmd.ExecuteNonQuery();
                            }
                        }
                    }

                }






                TempData["Msg"] = "Successfully Saved!";
                return RedirectToAction("DisplayDetails");

            }

            catch (Exception ex)
            {
                TempData["Msg"] = "Data not Saved!";
                return RedirectToAction("DisplayDetails");
            }
        }


        //public ActionResult DisplayDetails()
        //{
        //    SlowModel model = new SlowModel();
        //    DataSet ds = new DataSet();
        //    string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
        //    using (MySqlConnection con = new MySqlConnection(constr))
        //    {
        //        using (MySqlCommand cmd = new MySqlCommand("GetEmployees", con))
        //        {

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@tl", int.Parse(Session["UserId"].ToString()));

        //            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
        //            {

        //                sda.Fill(ds);

        //            }
        //        }
        //    }
        //    DataTable dtt = ds.Tables[0];
        //    model.LstEmployee = dtt.DataTableToList<TestEmployee>();


        //    return View("ProductionEntryDetails",model);
        //}


        public ActionResult DisplayDetails()
        {
            string userid = string.Empty;
            string loc = string.Empty;
            SummaryModel model = new SummaryModel();
            DataSet ds = new DataSet();

            string user = System.Web.HttpContext.Current.User.Identity.Name;

            //foreach (string key in System.Web.HttpContext.Current.Request.Form.AllKeys)
            //{
            //    string value = System.Web.HttpContext.Current.Request.Form[key];
            //}

            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            string monthName1 = ci.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            int year = int.Parse(DateTime.Now.Year.ToString());


            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string query = "Delete from productiontemp where id<>0;";
            query += "insert into productiontemp(psn,process,projectcode,eventcode,location,date,teamleadid,project,hoursplanned,hoursworked)select psn,process,projectcode,eventcode,location,date,teamleadid,project,hoursplanned,hoursworked from production where teamleadid=" + user + " and location='" + Session["location"].ToString() + "' and date=(select max(date) from production where teamleadid=" + user + " and location='" + Session["location"].ToString() + "');";
            query += "select memployee.PSN,AssociateName,productiontemp.project,CONCAT(productiontemp.projectcode,'-', productiontemp.eventcode) as projectcode,    productiontemp.process,productiontemp.hoursplanned,productiontemp.hoursworked   from memployee left outer join productiontemp on memployee.psn=productiontemp.psn where memployee.tlid=" + user + " and isactive=true   order by psn;";
            query += "select distinct CONCAT(projectcode,'-', eventcode) as projectcode  from projectconfiguration where location='" + Session["location"].ToString() + "' and monthname='" + monthName1 + "' and year=" + year + ";";
            query += "SELECT distinct project  FROM production where project !='';";




            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;

                        sda.Fill(ds);

                    }
                }



                //using (MySqlConnection con = new MySqlConnection(constr))
                //{
                //    using (MySqlCommand cmd = new MySqlCommand("GetEmployeeList", con))
                //    {

                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@tl", int.Parse(userid.ToString()));
                //       // cmd.Parameters.AddWithValue("@tl", 1600);
                //       // cmd.CommandTimeout = 1400;
                //        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                //        {

                //            sda.Fill(ds);

                //        }
                //    }
                //}




                // model.EmployeeList = ds.Tables[0].DataTableToList<Employee>();


                List<Employee> EmployeeList = new List<Employee>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Employee employee = new Employee();
                    employee.PSN = ds.Tables[0].Rows[i]["PSN"].ToString();
                    employee.Associatename = ds.Tables[0].Rows[i]["Associatename"].ToString();
                    employee.project = ds.Tables[0].Rows[i]["project"].ToString();
                    employee.projectcode = ds.Tables[0].Rows[i]["projectcode"].ToString();
                    //employee.eventcode = ds.Tables[0].Rows[i]["eventcode"].ToString();
                    employee.process = ds.Tables[0].Rows[i]["process"].ToString();
                    employee.hoursplanned = ds.Tables[0].Rows[i]["hoursplanned"].ToString();
                    employee.hoursworked = ds.Tables[0].Rows[i]["hoursworked"].ToString();
                    //employee.ActualProduction = double.Parse(ds.Tables[0].Rows[i]["ActualProduction"].ToString());
                    //employee.workathome = double.Parse(ds.Tables[0].Rows[i]["workathome"].ToString());
                    //employee.Leave = ds.Tables[0].Rows[i]["Leave"].ToString();
                    EmployeeList.Add(employee);
                }

                model.EmployeeList = EmployeeList;


                List<string> project = new List<string>();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    project.Add(dr["projectcode"].ToString());
                }
                var projects = project;
                model.ProjconfList = GetSelectListItems(projects);


                //List<string> Event = new List<string>();
                //foreach (DataRow dr in ds.Tables[2].Rows)
                //{
                //    Event.Add(dr["eventcode"].ToString());
                //}
                //var Events = Event;
                //model.EventList = GetSelectListItems(Events);

                List<string> projectcode = new List<string>();
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    projectcode.Add(dr["project"].ToString());
                }
                var projectscode = projectcode;
                model.ProjectList = GetSelectListItems(projectscode);


                return View("EntryEmployeeDetails", model);

            }
        }






        public ActionResult EntryDetails()
        {
            string userid = string.Empty;
            string loc = string.Empty;
            SummaryModel model = new SummaryModel();
            DataSet ds = new DataSet();

            string user = System.Web.HttpContext.Current.User.Identity.Name;



            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            string monthName1 = ci.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            int year = int.Parse(DateTime.Now.Year.ToString());


            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string query = "Delete from Editproductiontemp where id<>0;";
            query += "insert into Editproductiontemp(psn,process,projectcode,eventcode,location,date,teamleadid,project,hoursplanned,hoursworked,`Actualproduction`,`Leave`)select psn,process,projectcode,eventcode,location,date,teamleadid,project,hoursplanned,hoursworked,Actualproduction,Remarks from production where teamleadid=" + user + " and location='" + Session["location"].ToString() + "' and date='2020-02-17';";
            query += "select memployee.PSN,AssociateName,Editproductiontemp.project,CONCAT(Editproductiontemp.projectcode,'-', Editproductiontemp.eventcode) as projectcode,    Editproductiontemp.process,Editproductiontemp.hoursplanned,Editproductiontemp.hoursworked,Editproductiontemp.Actualproduction,Editproductiontemp.`Leave`   from memployee left outer join Editproductiontemp on memployee.psn=Editproductiontemp.psn where memployee.tlid=" + user + " and isactive=true   order by psn;";
            query += "select distinct CONCAT(projectcode,'-', eventcode) as projectcode  from projectconfiguration where location='" + Session["location"].ToString() + "' and monthname='" + monthName1 + "' and year=" + year + ";";
            query += "SELECT distinct project  FROM production where project !='';";




            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;

                        sda.Fill(ds);

                    }
                }






                List<Employee> EmployeeList = new List<Employee>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Employee employee = new Employee();
                    employee.PSN = ds.Tables[0].Rows[i]["PSN"].ToString();
                    employee.Associatename = ds.Tables[0].Rows[i]["Associatename"].ToString();
                    employee.project = ds.Tables[0].Rows[i]["project"].ToString();
                    employee.projectcode = ds.Tables[0].Rows[i]["projectcode"].ToString();
                    //employee.eventcode = ds.Tables[0].Rows[i]["eventcode"].ToString();
                    employee.process = ds.Tables[0].Rows[i]["process"].ToString();
                    employee.hoursplanned = ds.Tables[0].Rows[i]["hoursplanned"].ToString();
                    employee.hoursworked = ds.Tables[0].Rows[i]["hoursworked"].ToString();
                    if (ds.Tables[0].Rows[i]["ActualProduction"].ToString() != "")
                        employee.ActualProduction = Convert.ToDouble(ds.Tables[0].Rows[i]["ActualProduction"]);
                    else
                        employee.ActualProduction = 0;
                    // employee.workathome = double.Parse(ds.Tables[0].Rows[i]["workathome"].ToString());

                    if (ds.Tables[0].Rows[i]["Leave"].ToString() != "")
                        employee.Leave = ds.Tables[0].Rows[i]["Leave"].ToString();
                    EmployeeList.Add(employee);
                }

                model.EmployeeList = EmployeeList;


                List<string> project = new List<string>();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    project.Add(dr["projectcode"].ToString());
                }
                var projects = project;
                model.ProjconfList = GetSelectListItems(projects);


                //List<string> Event = new List<string>();
                //foreach (DataRow dr in ds.Tables[2].Rows)
                //{
                //    Event.Add(dr["eventcode"].ToString());
                //}
                //var Events = Event;
                //model.EventList = GetSelectListItems(Events);

                List<string> projectcode = new List<string>();
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    projectcode.Add(dr["project"].ToString());
                }
                var projectscode = projectcode;
                model.ProjectList = GetSelectListItems(projectscode);


                return View("EditEntryEmployeeDetails", model);

            }
        }





        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {

            var selectList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            string path = Server.MapPath("~/bin/ApplicationError.txt");
            // This text is added only once to the file.
            if (!System.IO.File.Exists(path))
            {
                // Create a file to write to.
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(filterContext.Exception);
                }
            }
            else
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(filterContext.Exception);
                }
            }
            Exception e = filterContext.Exception;
            //Log Exception e
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error"
            };
        }

    }

    public static class EnumerableToDataTableConverter
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var newRow = dataTable.NewRow();
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    newRow[Props[i].Name] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(newRow);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }



}
