using BPOAttendanceProject.Filters;
using BPOAttendanceProject.Models;
using ClosedXML.Excel;
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
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq.Expressions;
using System.Web.Hosting;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing.Imaging;

namespace BPOAttendanceProject.Controllers
{
    [UserFilter]
    public class QMSController : Controller
    {
        #region SHELL Outbound
        public ActionResult OutDashboard(Monthlyswservice modl)
        {
            Monthlyswservice Model = new Monthlyswservice();
            if (string.IsNullOrEmpty(modl.Year))
                modl.Year = DateTime.Today.Year.ToString();
            if (string.IsNullOrEmpty(modl.Month))
                modl.Month = DateTime.Today.Month.ToString();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "select Agent.Name as agentname, count(agentname) as CallsAudited, sum(Total) as TOTALSCORE, "
                + "round((sum(total)/ (count(agentname)*100)*100),2) as QualityScore"
                + " from ShellQA " +
                " inner join `Agent` on Agent.Id = ShellQA.AGENTNAME where month(Date)=" + modl.Month + " and year(Date)="
                + modl.Year + " group by agentname;" +
                "select count(agentname) as CallsAudited, sum(Total) as TOTALSCORE,round((sum(total) / (count(agentname)" +
                " * 100) * 100), 2) as QualityScore from ShellQA  where month(Date) = " + modl.Month +
                " and year(Date)= " + modl.Year + "; ";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                //ds.Tables.Add(new DataTable());
                //adapter.Fill(ds.Tables[0]);
                adapter.Fill(ds);
                DataTable dtt = ds.Tables[0];
                Model.LstMonthlyswservice = dtt.DataTableToList<Monthlyswservice>();
                Model.Year = modl.Year;
                Model.Month = modl.Month;
                Model.CallsAudited = ds.Tables[1].Rows[0]["CallsAudited"].ToString();
                Model.TotalScore = ds.Tables[1].Rows[0]["TOTALSCORE"].ToString();
                Model.QualityScore = ds.Tables[1].Rows[0]["QualityScore"].ToString();
                Session["MonthYear"] = modl.Month + "," + modl.Year;
                return View("OutDashboard", Model);
            }
        }
        public ActionResult OutQAServices(SoftwareServices modl)
        {
            SoftwareServices Model = new SoftwareServices();
            if (string.IsNullOrEmpty(modl.Year))
                modl.Year = DateTime.Today.Year.ToString();
            if (string.IsNullOrEmpty(modl.Month))
                modl.Month = DateTime.Today.Month.ToString();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT ShellQA.id, DATE_FORMAT(ShellQA.Date, '%d/%m/%y') as DATE,Agent.Name as AGENTNAME,CALLFROM,CALLTO, TicketNumber" +
                ",SUBSTRING(recordingurl,1,30) as RecordingURL,SUBSTRING(CALLREVIEW,1,20)as CALLREVIEW" +
                ", TICKETREVIEW from `ShellQA` inner join `Agent` on Agent.Id = ShellQA.AGENTNAME" +
                " where month(Date)=" + modl.Month + " and year(Date)="
                + modl.Year + " order by ShellQA.DATE";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.LstSoftwareServices = dtt.DataTableToList<SoftwareServices>();
                Model.Month = modl.Month;
                Model.Year = modl.Year;
                return View("OutQAServices", Model);
            }
        }
        public ActionResult ViewOutQAService(string ID)
        {
            int Id = Convert.ToInt16(ID);
            SoftwareServices Model = new SoftwareServices();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT ShellQA.id,DATE_FORMAT(ShellQA.Date, '%d/%m/%y') as DATE,Agent.Name as AGENTNAME, CALLFROM,CALLTO,TicketNumber,RecordingURL,CALLREVIEW,TICKETREVIEW,Greeting,REMARKS,Probing,REMARKS2,`Tagging`, "
                + " REMARKS3,Details,REMARKS4,Solution,REMARKS5,reminder,REMARKS6,Timeline,REMARKS8,listening,REMARKS9,Phone,REMARKS10,Grammar,REMARKS11,"
                + " Professionalism,REMARKS12,tools,rude,Tagging2,mistakes,total,actiontaken"
                + " from `ShellQA` inner join `Agent` on Agent.Id = ShellQA.AGENTNAME where ShellQA.id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        Model.DATE = reader.GetString(1);
                    Model.AGENTNAME = (reader.GetString(2));
                    if (!reader.IsDBNull(3))
                        Model.CALLFROM = (reader.GetString(3));
                    if (!reader.IsDBNull(4))
                        Model.CALLTO = (reader.GetString(4));
                    if (!reader.IsDBNull(5))
                        Model.TicketNumber = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        Model.RecordingURL = (reader.GetString(6));
                    if (!reader.IsDBNull(7))
                        Model.CALLREVIEW = (reader.GetString(7));
                    if (!reader.IsDBNull(8))
                        Model.TICKETREVIEW = (reader.GetString(8));
                    if (!reader.IsDBNull(9))
                        Model.Greeting = (reader.GetString(9));
                    if (!reader.IsDBNull(10))
                        Model.REMARKS = (reader.GetString(10));
                    if (!reader.IsDBNull(11))
                        Model.Probing = reader.GetString(11);
                    if (!reader.IsDBNull(12))
                        Model.REMARKS2 = reader.GetString(12);
                    if (!reader.IsDBNull(13))
                        Model.Tagging = reader.GetString(13);
                    if (!reader.IsDBNull(14))
                        Model.REMARKS3 = reader.GetString(14);
                    if (!reader.IsDBNull(15))
                        Model.Details = reader.GetString(15);
                    if (!reader.IsDBNull(16))
                        Model.REMARKS4 = reader.GetString(16);
                    if (!reader.IsDBNull(17))
                        Model.Solution = reader.GetString(17);
                    if (!reader.IsDBNull(18))
                        Model.REMARKS5 = reader.GetString(18);
                    if (!reader.IsDBNull(19))
                        Model.reminder = reader.GetString(19);
                    if (!reader.IsDBNull(20))
                        Model.REMARKS6 = reader.GetString(20);
                    if (!reader.IsDBNull(21))
                        Model.Timeline = reader.GetString(21);
                    if (!reader.IsDBNull(22))
                        Model.REMARKS8 = reader.GetString(22);
                    if (!reader.IsDBNull(23))
                        Model.listening = reader.GetString(23);
                    if (!reader.IsDBNull(24))
                        Model.REMARKS9 = reader.GetString(24);
                    if (!reader.IsDBNull(25))
                        Model.Phone = reader.GetString(25);
                    if (!reader.IsDBNull(26))
                        Model.REMARKS10 = reader.GetString(26);
                    if (!reader.IsDBNull(27))
                        Model.Grammar = reader.GetString(27);
                    if (!reader.IsDBNull(28))
                        Model.REMARKS11 = reader.GetString(28);
                    if (!reader.IsDBNull(29))
                        Model.Professionalism = reader.GetString(29);
                    if (!reader.IsDBNull(30))
                        Model.REMARKS12 = reader.GetString(30);
                    if (!reader.IsDBNull(31))
                        Model.tools = reader.GetString(31);
                    if (!reader.IsDBNull(32))
                        Model.rude = reader.GetString(32);
                    if (!reader.IsDBNull(33))
                        Model.Tagging2 = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                        Model.mistakes = reader.GetString(34);
                    if (!reader.IsDBNull(35))
                        Model.Total = reader.GetString(35);
                    if (!reader.IsDBNull(36))
                        Model.ActionTaken = reader.GetString(36);
                }
            }
            return PartialView("/Views/QMS/_ViewOutQAService.cshtml", Model);
        }
        public ActionResult AddOutQAService()
        {
            SoftwareServices model = new SoftwareServices();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT id, Name from `Agent`";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                model.LstAgent = dtt.DataTableToList<NewClientMgmt>();
            }
            model.CALLFROM = String.Empty;
            model.CALLTO = String.Empty;
            model.TicketNumber = String.Empty;
            model.RecordingURL = String.Empty;
            model.CALLREVIEW = String.Empty;
            model.TICKETREVIEW = String.Empty;
            model.Greeting = "0";
            model.REMARKS = String.Empty;
            model.Probing = "0";
            model.REMARKS2 = String.Empty;
            model.Tagging = "0";
            model.REMARKS3 = String.Empty;
            model.Details = "0";
            model.REMARKS4 = String.Empty;
            model.Solution = "0";
            model.REMARKS5 = String.Empty;
            model.reminder = "0";
            model.REMARKS6 = String.Empty;
            model.Timeline = "0";
            model.REMARKS8 = String.Empty;
            model.listening = "0";
            model.REMARKS9 = String.Empty;
            model.Phone = "0";
            model.REMARKS10 = String.Empty;
            model.Closing = "0";
            model.Grammar = "0";
            model.REMARKS11 = String.Empty;
            model.Professionalism = "0";
            model.REMARKS12 = String.Empty;
            model.tools = "0";
            model.rude = "0";
            model.Tagging2 = "0";
            model.mistakes = "0";
            return PartialView("_AddOutQAService", model);
        }
        public ActionResult SaveOutQAService(SoftwareServices model)
        {
            //if (ModelState.IsValid)
            //{
            if (model.Id == 0)
            {
                int insertresult = softwareServiceExistence(model);
                //if (insertresult == 0)
                //{
                string Result = ManageSoftwareService(model);
                if (Result.Trim('"') == "Ok")
                    TempData["Msg"] = "Successfully Saved!";
                else
                    TempData["Msg"] = "Unsuccessfull Operation!";
            }
            else
            {
                string Result = ManageSoftwareService(model);
                if (Result.Trim('"') == "Ok")
                    TempData["Msg"] = "Successfully Saved!";
                else
                    TempData["Msg"] = "Unsuccessfull Operation!";
            }
            //}
            return RedirectToAction("OutQAServices");
        }
        public string ManageSoftwareService(SoftwareServices model)
        {
            string Result = string.Empty;
            Result = "NotOk";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            string Command = string.Empty;
            if (model.Id == 0)
            {
                Command = "INSERT INTO `ShellQA`(" +
                    "`DATE`,`AGENTNAME`, `CALLFROM`,`CALLTO`,`TicketNumber`,`RecordingURL`,`CALLREVIEW`,`TICKETREVIEW`," +
                    "`Greeting`,`REMARKS`,`Probing`,`REMARKS2`,`Tagging` " +
                    ", `REMARKS3`,`Details`,`REMARKS4`,`Solution`,`REMARKS5`,`reminder`,`REMARKS6`,"
                    + "`Timeline`,`REMARKS8`,`listening` " +
                    " , `REMARKS9`,`Phone`,`REMARKS10`,`Grammar`,`REMARKS11`,`Professionalism`,`REMARKS12`,`tools`,`rude`," +
                    "`Tagging2`, `mistakes`,`TOTAL`, `ACTIONTAKEN`, Closing)" +
                    " VALUES (STR_TO_DATE('" + model.DATE + "','%d/%m/%Y'),'" + model.AGENTNAME + "','" + model.CALLFROM + "' ,'" + model.CALLTO +
                        "','" + model.TicketNumber + "','" + model.RecordingURL + "','" + model.CALLREVIEW +
                        "','" + model.TICKETREVIEW + "','" + model.Greeting + "','" + model.REMARKS + "','" + model.Probing + "','" + model.REMARKS2 +
                        "','" + model.Tagging + "','" + model.REMARKS3 + "'," +
                    "'" + model.Details + "','" + model.REMARKS4 + "','" + model.Solution + "' ,'" + model.REMARKS5 + "','" +
                    model.reminder + "','" + model.REMARKS6 + "','" + model.Timeline
                    + "','" + model.REMARKS8 + "','" + model.listening + "','" + model.REMARKS9 + "','" + model.Phone
                    + "','" + model.REMARKS10 + "','" + model.Grammar
                    + "','" + model.REMARKS11 + "','" + model.Professionalism + "','" + model.REMARKS12 + "','" + model.tools
                    + "','" + model.rude + "','" + model.Tagging2
                    + "','" + model.mistakes + "','" + model.Total + "','" + model.ActionTaken + "','" + model.Closing +
                "');";
                //Command = "INSERT INTO `Softwareservice`(`DATE`,`AGENTNAME`) VALUES ('" + (model.DATE.ToString()) + "','" + model.AGENTNAME + "');";
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
                Command = "UPDATE ShellQA set  `DATE`=STR_TO_DATE('" + model.DATE + "','%d/%m/%Y')"
                    + ",AGENTNAME='" + model.AGENTNAME + "',CALLFROM='" + model.CALLFROM
                    + "', CALLTO='" + model.CALLTO + "',TicketNumber='" + model.TicketNumber
                    + "', RecordingURL ='" + model.RecordingURL + "',CALLREVIEW='" + model.CALLREVIEW
                    + "',TICKETREVIEW='" + model.TICKETREVIEW + "',Greeting='" + model.Greeting
                    + "', REMARKS='" + (model.REMARKS == null ? "" : model.REMARKS).ToString().Replace("'", "") + "', Probing='" + model.Probing
                    + "',REMARKS2='" + (model.REMARKS2 == null ? "" : model.REMARKS2).ToString().Replace("'", "") + "',`Tagging`='" + model.Tagging
                    + "',REMARKS3='" + (model.REMARKS3 == null ? "" : model.REMARKS3).ToString().Replace("'", "") + "',`Details`='" + model.Details
                    + "',REMARKS4='" + (model.REMARKS4 == null ? "" : model.REMARKS4).ToString().Replace("'", "") + "',`Solution`='" + model.Solution
                    + "',REMARKS5='" + (model.REMARKS5 == null ? "" : model.REMARKS5).ToString().Replace("'", "") + "',`reminder`='" + model.reminder
                    + "',REMARKS6='" + (model.REMARKS6 == null ? "" : model.REMARKS6).ToString().Replace("'", "") + "',`Timeline`='" + model.Timeline
                    + "',REMARKS8='" + (model.REMARKS8 == null ? "" : model.REMARKS8).ToString().Replace("'", "") + "',`listening`='" + model.listening
                    + "',REMARKS9='" + (model.REMARKS9 == null ? "" : model.REMARKS9).ToString().Replace("'", "") + "',`Phone`='" + model.Phone
                    + "',REMARKS10='" + (model.REMARKS10 == null ? "" : model.REMARKS10).ToString().Replace("'", "") + "',`Grammar`='" + model.Grammar
                    + "',REMARKS11='" + (model.REMARKS11 == null ? "" : model.REMARKS11).ToString().Replace("'", "") + "',`Professionalism`='" + model.Professionalism
                    + "',REMARKS12='" + (model.REMARKS12 == null ? "" : model.REMARKS12).ToString().Replace("'", "") + "',`tools`='" + model.tools
                    + "',rude='" + model.rude + "',`Tagging2`='" + model.Tagging2
                    + "',mistakes='" + model.mistakes + "',Closing = '" + model.Closing
                    + "',total='" + model.Total + "',`ActionTaken`='" + model.ActionTaken
                    + "' where ShellQA.Id=" + model.Id;
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
        public ActionResult DeleteService(string ID)
        {
            try
            {
                DeleteServiceDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("OutQAServices");
            }
        }
        public string DeleteServiceDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "delete from `ShellQA` where `ShellQA`.id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }
        public int softwareServiceExistence(SoftwareServices model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `ShellQA`";
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
        public ActionResult GetOutQAService(string ID)
        {
            int Id = Convert.ToInt16(ID);
            SoftwareServices Model = new SoftwareServices();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT ShellQA.id,DATE_FORMAT(ShellQA.Date, '%d/%m/%Y') as DATE,AGENTNAME, CALLFROM,CALLTO,TicketNumber,RecordingURL,CALLREVIEW,TICKETREVIEW,Greeting,REMARKS,Probing,REMARKS2,`Tagging`, "
                + " REMARKS3,Details,REMARKS4,Solution,REMARKS5,reminder,REMARKS6,Timeline,REMARKS8,listening,REMARKS9,Phone,REMARKS10,Grammar,REMARKS11,"
                + " Professionalism,REMARKS12,tools,rude,Tagging2,mistakes,TOTAL,ACTIONTAKEN,Closing,RemarksClosing from `ShellQA`"
                + " inner join `Agent` on Agent.Id = ShellQA.AGENTNAME where ShellQA.id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    //Model.softwareservices = reader.GetString(1);
                    if (!reader.IsDBNull(1))
                        Model.DATE = (reader.GetString(1));
                    Model.AGENTNAME = (reader.GetString(2));
                    if (!reader.IsDBNull(3))
                        Model.CALLFROM = (reader.GetString(3));
                    if (!reader.IsDBNull(4))
                        Model.CALLTO = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        Model.TicketNumber = (reader.GetString(5));
                    if (!reader.IsDBNull(6))
                        Model.RecordingURL = (reader.GetString(6));
                    if (!reader.IsDBNull(7))
                        Model.CALLREVIEW = (reader.GetString(7));
                    if (!reader.IsDBNull(8))
                        Model.TICKETREVIEW = (reader.GetString(8));
                    if (!reader.IsDBNull(9))
                        Model.Greeting = (reader.GetString(9));
                    if (!reader.IsDBNull(10))
                        Model.REMARKS = reader.GetString(10);
                    if (!reader.IsDBNull(11))
                        Model.Probing = (reader.GetString(11));
                    if (!reader.IsDBNull(12))
                        Model.REMARKS2 = reader.GetString(12);
                    if (!reader.IsDBNull(13))
                        Model.Tagging = reader.GetString(13);
                    if (!reader.IsDBNull(14))
                        Model.REMARKS3 = reader.GetString(14);
                    if (!reader.IsDBNull(15))
                        Model.Details = reader.GetString(15);
                    if (!reader.IsDBNull(16))
                        Model.REMARKS4 = reader.GetString(16);
                    if (!reader.IsDBNull(17))
                        Model.Solution = reader.GetString(17);
                    if (!reader.IsDBNull(18))
                        Model.REMARKS5 = reader.GetString(18);
                    if (!reader.IsDBNull(19))
                        Model.reminder = reader.GetString(19);
                    if (!reader.IsDBNull(20))
                        Model.REMARKS6 = reader.GetString(20);
                    if (!reader.IsDBNull(21))
                        Model.Timeline = reader.GetString(21);
                    if (!reader.IsDBNull(22))
                        Model.REMARKS8 = reader.GetString(22);
                    if (!reader.IsDBNull(23))
                        Model.listening = reader.GetString(23);
                    if (!reader.IsDBNull(24))
                        Model.REMARKS9 = reader.GetString(24);
                    if (!reader.IsDBNull(25))
                        Model.Phone = reader.GetString(25);
                    if (!reader.IsDBNull(26))
                        Model.REMARKS10 = reader.GetString(26);
                    if (!reader.IsDBNull(27))
                        Model.Grammar = reader.GetString(27);
                    if (!reader.IsDBNull(28))
                        Model.REMARKS11 = reader.GetString(28);
                    if (!reader.IsDBNull(29))
                        Model.Professionalism = reader.GetString(29);
                    if (!reader.IsDBNull(30))
                        Model.REMARKS12 = reader.GetString(30);
                    if (!reader.IsDBNull(31))
                        Model.tools = reader.GetString(31);
                    if (!reader.IsDBNull(32))
                        Model.rude = reader.GetString(32);
                    if (!reader.IsDBNull(33))
                        Model.Tagging2 = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                        Model.mistakes = reader.GetString(34);
                    if (!reader.IsDBNull(35))
                        Model.Total = reader.GetString(35);
                    if (!reader.IsDBNull(36))
                        Model.ActionTaken = reader.GetString(36);
                    if (!reader.IsDBNull(37))
                        Model.Closing = reader.GetString(37);
                    if (!reader.IsDBNull(38))
                        Model.RemarksClosing = reader.GetString(38);
                }
                reader.Dispose();
                Command = "SELECT id, Name from `Agent`";
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.LstAgent = dtt.DataTableToList<NewClientMgmt>();
            }
            return PartialView("/Views/QMS/_AddOutQAService.cshtml", Model);
        }
        public JsonResult CalculateETo(string month, string year)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            double rate = 0.0;

            string Command = "SELECT `rate` from  `servicedollar` where `month`=" + month + " and year=" + year + "";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                mConnection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rate = reader.GetDouble("rate");

                    }
                }
            }

            var result = new { rate = rate };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadExcelReport(string month, string year)
        {
            if (Session["MonthYear"] != null)
            {
                month = Session["MonthYear"].ToString().Split(',')[0];
                year = Session["MonthYear"].ToString().Split(',')[1];
            }
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string query = "select Agent.Name as agentname, count(agentname) as CallsAudited, sum(Total) as TOTALSCORE, "
                + "round((sum(total)/ (count(agentname)*100)*100),2) as QualityScore"
                + " from ShellQA " +
                " inner join `Agent` on Agent.Id = ShellQA.AGENTNAME where month(Date)=" + month + " and year(Date)="
                + year + " group by agentname;" +
                "select count(agentname) as CallsAudited, sum(Total) as TOTALSCORE,round((sum(total) / (count(agentname)" +
                " * 100) * 100), 2) as QualityScore from ShellQA  where month(Date) = " + month +
                " and year(Date)= " + year + "; ";
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

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                DataTable dt = new DataTable();
                                dt = ds.Tables[0];
                                dt.Rows.Add("Total", ds.Tables[1].Rows[0]["CallsAudited"].ToString(), ds.Tables[1].Rows[0]["TOTALSCORE"].ToString(), ds.Tables[1].Rows[0]["QualityScore"].ToString());
                                wb.Worksheets.Add(dt);
                                //foreach (DataTable dt in ds.Tables)
                                //{
                                //    wb.Worksheets.Add(dt);
                                //}
                                string[] strArr = null;
                                char[] splitchar = { '/' };
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=OutDashboardExport-" + month + "/" + year + ".xlsx");
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
        public ActionResult DownloadExcelReportQA(string month, string year)
        {
            //if (Session["MonthYear"] != null)
            //{
            //    month = Session["MonthYear"].ToString().Split(',')[0];
            //    year = Session["MonthYear"].ToString().Split(',')[1];
            //}
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //string query = "select Agent.Name as agentname, agentname as CallsAudited, Total as TOTALSCORE, "
            //    + "round((sum(total)/ (count(agentname)*100)*100),2) as QualityScore"
            //    + " from ShellQA " +
            //    " inner join `Agent` on Agent.Id = ShellQA.AGENTNAME where month(Date)=" + month + " and year(Date)="
            //    + year + " group by agentname;" +
            //    "select count(agentname) as CallsAudited, sum(Total) as TOTALSCORE,round((sum(total) / (count(agentname)" +
            //    " * 100) * 100), 2) as QualityScore from ShellQA  where month(Date) = " + month +
            //    " and year(Date)= " + year + "; ";
            string query = "SELECT ShellQA.id,DATE_FORMAT(ShellQA.Date, '%d/%m/%y') as DATE,Agent.Name as AGENTNAME, CALLFROM,CALLTO,TicketNumber,RecordingURL,CALLREVIEW,TICKETREVIEW,Greeting,REMARKS,Probing,REMARKS2,`Tagging`, "
                + " REMARKS3,Details,REMARKS4,Solution,REMARKS5,reminder,REMARKS6,Timeline,REMARKS8,listening,REMARKS9,Phone,REMARKS10,Grammar,REMARKS11,"
                + " Professionalism,REMARKS12,tools,rude,Tagging2,mistakes,total,actiontaken"
                + " from `ShellQA` inner join `Agent` on Agent.Id = ShellQA.AGENTNAME"
                + " where month(Date)=" + month + " and year(Date)="
                + year + " order by ShellQA.DATE desc";
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

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                DataTable dt = new DataTable();
                                dt = ds.Tables[0];
                                //dt.Rows.Add("Total", ds.Tables[1].Rows[0]["CallsAudited"].ToString(), ds.Tables[1].Rows[0]["TOTALSCORE"].ToString(), ds.Tables[1].Rows[0]["QualityScore"].ToString());
                                wb.Worksheets.Add(dt);
                                //foreach (DataTable dt in ds.Tables)
                                //{
                                //    wb.Worksheets.Add(dt);
                                //}
                                string[] strArr = null;
                                char[] splitchar = { '/' };
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=OutQAExport-" + month + "/" + year + ".xlsx");
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
        public ActionResult OutParameters()
        {
            return View();
        }
        #endregion
        
    }
}