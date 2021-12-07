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
    public class MRMController : Controller
    {

        #region Softwareservice


        public ActionResult GetSoftwareViewDetails(string ID)
        {
            int Id = Convert.ToInt16(ID);
            SoftwareServices Model = new SoftwareServices();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Softwareservice.id,DATE_FORMAT(Softwareservice.Date, '%d/%m/%y') as DATE,Agent.Name as AGENTNAME, CALLFROM,CALLTO,TicketNumber,RecordingURL,CALLREVIEW,TICKETREVIEW,Greeting,REMARKS,Probing,REMARKS2,`Tagging`, "
                + " REMARKS3,Details,REMARKS4,Solution,REMARKS5,reminder,REMARKS6,Timeline,REMARKS8,listening,REMARKS9,Phone,REMARKS10,Grammar,REMARKS11,"
                + " Professionalism,REMARKS12,tools,rude,Tagging2,mistakes,total,actiontaken"
                + " from `Softwareservice` inner join `Agent` on Agent.Id = Softwareservice.AGENTNAME where Softwareservice.id=" + Id;
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
            return PartialView("/Views/MRM/_ViewSoftwareservice.cshtml", Model);
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
                + " from Softwareservice " +
                " inner join `Agent` on Agent.Id = Softwareservice.AGENTNAME where month(Date)=" + month + " and year(Date)="
                + year + " group by agentname;" +
                "select count(agentname) as CallsAudited, sum(Total) as TOTALSCORE,round((sum(total) / (count(agentname)" +
                " * 100) * 100), 2) as QualityScore from Softwareservice  where month(Date) = " + month +
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
                                dt.Rows.Add("Total",ds.Tables[1].Rows[0]["CallsAudited"].ToString(), ds.Tables[1].Rows[0]["TOTALSCORE"].ToString(), ds.Tables[1].Rows[0]["QualityScore"].ToString());
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
                                Response.AddHeader("content-disposition", "attachment;filename=DashboardExport-" + month + "/" + year + ".xlsx");
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
        public ActionResult GetSoftwareservice(string ID)
        {
            int Id = Convert.ToInt16(ID);
            SoftwareServices Model = new SoftwareServices();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Softwareservice.id,DATE_FORMAT(Softwareservice.Date, '%d/%m/%Y') as DATE,AGENTNAME, CALLFROM,CALLTO,TicketNumber,RecordingURL,CALLREVIEW,TICKETREVIEW,Greeting,REMARKS,Probing,REMARKS2,`Tagging`, "
                + " REMARKS3,Details,REMARKS4,Solution,REMARKS5,reminder,REMARKS6,Timeline,REMARKS8,listening,REMARKS9,Phone,REMARKS10,Grammar,REMARKS11,"
                + " Professionalism,REMARKS12,tools,rude,Tagging2,mistakes,TOTAL,ACTIONTAKEN,Closing,RemarksClosing from `Softwareservice`"
                + " inner join `Agent` on Agent.Id = Softwareservice.AGENTNAME where Softwareservice.id=" + Id;
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
            return PartialView("/Views/MRM/_AddSoftwareservice.cshtml", Model);
        }
        public ActionResult SoftwareServices(SoftwareServices modl)
        {
            SoftwareServices Model = new SoftwareServices();
            if (string.IsNullOrEmpty(modl.Year))
                modl.Year = DateTime.Today.Year.ToString();
            if (string.IsNullOrEmpty(modl.Month))
                modl.Month = DateTime.Today.Month.ToString();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Softwareservice.id, DATE_FORMAT(Softwareservice.Date, '%d/%m/%y') as DATE,Agent.Name as AGENTNAME,CALLFROM,CALLTO, TicketNumber" +
                ",SUBSTRING(recordingurl,1,30) as RecordingURL,SUBSTRING(CALLREVIEW,1,20)as CALLREVIEW" +
                ", TICKETREVIEW from `Softwareservice` inner join `Agent` on Agent.Id = Softwareservice.AGENTNAME " +
                " where month(Date)=" + modl.Month + " and year(Date)="
                + modl.Year + " order by Softwareservice.DATE";
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
                return View("SoftwareServices", Model);
            }
        }
        public ActionResult DownloadExcelReportQA(string month, string year)
        {
            string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string query = "SELECT Softwareservice.id,DATE_FORMAT(Softwareservice.Date, '%d/%m/%y') as DATE,Agent.Name as AGENTNAME, CALLFROM,CALLTO,TicketNumber,RecordingURL,CALLREVIEW,TICKETREVIEW,Greeting,REMARKS,Probing,REMARKS2,`Tagging`, "
                + " REMARKS3,Details,REMARKS4,Solution,REMARKS5,reminder,REMARKS6,Timeline,REMARKS8,listening,REMARKS9,Phone,REMARKS10,Grammar,REMARKS11,"
                + " Professionalism,REMARKS12,tools,rude,Tagging2,mistakes,total,actiontaken"
                + " from `Softwareservice` inner join `Agent` on Agent.Id = Softwareservice.AGENTNAME"
                + " where month(Date)=" + month + " and year(Date)="
                + year + " order by Softwareservice.DATE desc";
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
                                Response.AddHeader("content-disposition", "attachment;filename=QAExport-" + month + "/" + year + ".xlsx");
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
        public ActionResult AddSoftwareservice()
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
            return PartialView("_AddSoftwareservice", model);
        }

        public int softwareServiceExistence(SoftwareServices model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `Softwareservice`";
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

        public ActionResult Savesoftwareservice(SoftwareServices model)
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
                //}
                //else
                //{
                //    TempData["Msg"] = "Data  Exist!";
                //}
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
            return RedirectToAction("SoftwareServices");
        }


        public string ManageSoftwareService(SoftwareServices model)
        {
            string Result = string.Empty;
            Result = "NotOk";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

            string Command = string.Empty;
            if (model.Id == 0)
            {
                Command = "INSERT INTO `Softwareservice`(" +
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
                Command = "UPDATE Softwareservice set  `DATE`=STR_TO_DATE('" + model.DATE + "','%d/%m/%Y')"
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
                    + "' where Softwareservice.Id=" + model.Id;
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
                //return RedirectToAction("Monthlysoftwareservice");
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";

                return RedirectToAction("SoftwareServices");
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
                cmd.CommandText = "delete from `Softwareservice` where `Softwareservice`.id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }

        #endregion

        #region  Monthlysoftwareservice

        public ActionResult Monthlysoftwareservice(Monthlyswservice modl)
        {
            Monthlyswservice Model = new Monthlyswservice();
            if (string.IsNullOrEmpty(modl.Year))
                modl.Year = DateTime.Today.Year.ToString();
            if (string.IsNullOrEmpty(modl.Month))
                modl.Month = DateTime.Today.Month.ToString();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "select Agent.Name as agentname, count(agentname) as CallsAudited, sum(Total) as TOTALSCORE, "
                + "round((sum(total)/ (count(agentname)*100)*100),2) as QualityScore"
                + " from Softwareservice " +
                " inner join `Agent` on Agent.Id = Softwareservice.AGENTNAME where month(Date)=" + modl.Month + " and year(Date)="
                + modl.Year + " group by agentname;" +
                "select count(agentname) as CallsAudited, sum(Total) as TOTALSCORE,round((sum(total) / (count(agentname)" +
                " * 100) * 100), 2) as QualityScore from Softwareservice  where month(Date) = " + modl.Month +
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
                return View("Monthlysoftwareservice", Model);
            }
        }

        public ActionResult Addmrmswservice()
        {
            Monthlyswservice model = new Monthlyswservice();

            return PartialView("_AddMonthlysoftwareservice", model);

        }

        public ActionResult Savemrmswservice(Monthlyswservice model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    int insertresult = MonthlyMRMServiceExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageMRMSwService(model);
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
                    string Result = ManageMRMSwService(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }



            }
            return RedirectToAction("Monthlysoftwareservice");

        }


        public string ManageMRMSwService(Monthlyswservice model)
        {
            string Result = string.Empty;
            Result = "NotOk";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            int monthNumber = 0;
            string monthno = string.Empty;
            monthNumber = DateTime.ParseExact(model.Month, "MMMM", CultureInfo.CurrentCulture).Month;
            if (monthNumber < 10)
            {
                monthno = "0" + monthNumber;
            }
            else
            {
                monthno = monthNumber.ToString();
            }

            var Date1 = model.Year + "-" + monthno + "-01";
            //var Date2 = strArr[1].Trim().ToString() + "-03-31";

            string Command = string.Empty;
            if (model.Id == 0)
            {
                Command = "INSERT INTO monthlyswservice(`month`,`year`, `budgeINR`,`ActualINR`,date) VALUES ('" + model.Month + "','" + model.Year + "'," + model.budgeINR + "," + model.ActualINR + " ,'" + Date1 + "');";
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

                Command = "UPDATE monthlyswservice set `budgeINR`='" + model.budgeINR + "', `ActualINR`='" + model.ActualINR + "',month='" + model.Month + "',Year='" + model.Year + "',date='" + Date1 + "' where monthlyswservice.id=" + model.Id;
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








        public int MonthlyMRMServiceExistence(Monthlyswservice model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `monthlyswservice` where month='" + model.Month + "' and `year`='" + model.Year + "'";
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





        public ActionResult GetMRMService(string ID)
        {
            int Id = Convert.ToInt16(ID);
            Monthlyswservice Model = new Monthlyswservice();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT id,month,year,budgeINR,ActualINR FROM monthlyswservice where monthlyswservice.id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.Month = reader.GetString(1);
                    Model.Year = reader.GetString(2);
                    Model.budgeINR = Convert.ToDouble(reader.GetDouble(3));
                    Model.ActualINR = Convert.ToDouble(reader.GetDouble(4));

                }

            }

            return PartialView("/Views/MRM/_AddMonthlysoftwareservice.cshtml", Model);
        }

        public ActionResult DeleteMRMservice(string ID)
        {
            try
            {
                DeleteMRMserviceDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("Monthlysoftwareservice");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";

                return RedirectToAction("Monthlysoftwareservice");
            }
        }


        public string DeleteMRMserviceDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "delete from monthlyswservice where monthlyswservice.id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }


        public ActionResult BarChart(string month, string year)
        {



            var dataSet = new DataSet();
            var dataTable = new DataTable();
            string monthid = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;

            if (month == "January")
            {
                monthid = "01";
            }
            else if (month == "February")
            {
                monthid = "02";
            }
            else if (month == "March")
            {
                monthid = "03";
            }
            else if (month == "April")
            {
                monthid = "04";
            }
            else if (month == "May")
            {
                monthid = "05";
            }
            else if (month == "June")
            {
                monthid = "06";
            }
            else if (month == "July")
            {
                monthid = "07";
            }
            else if (month == "August")
            {
                monthid = "08";
            }
            else if (month == "September")
            {
                monthid = "09";
            }
            else if (month == "October")
            {
                monthid = "10";
            }
            else if (monthid == "November")
            {
                monthid = "11";
            }
            else if (monthid == "December")
            {
                monthid = "12";
            }





            string pdate = "01" + "/" + "04" + "/" + year;

            string enddate = "30" + "/" + monthid + "/" + year;

            DateTime dtdateFrom = new DateTime();
            dtdateFrom = DateTime.ParseExact(pdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dfromdate = dtdateFrom.ToString("yyyy-MM-dd");

            DateTime dtdateEnd = new DateTime();
            dtdateEnd = DateTime.ParseExact(enddate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var denddate = dtdateEnd.ToString("yyyy-MM-dd");

            decimal backlog = 0;

            string backlogcommand = "select  sum(ActualINR)-  sum(budgeINR) as backlog from monthlyswservice where date >='" + dfromdate + "' and date <='" + denddate + "'";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(backlogcommand))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // read
                    {
                        backlog = Convert.ToDecimal(reader["backlog"]);

                    }



                }
            }

            Command = "select concat ( month,year) as monthyear, budgeINR as BudgetINR,ActualINR,Round((ActualINR/budgeINR)*100) as Percent  from monthlyswservice where monthlyswservice.month='" + month + "' and monthlyswservice.year=" + year + "";
            //Command = "select concat ( month,year) as monthyear, budgeINR as budgetINR ,ActualINR  from monthlyswservice where monthlyswservice.month='" + month + "' ";

            Command = Command + "   order by year(date), month(date)";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dataTable);
            }
            System.Data.DataColumn backColumn = new System.Data.DataColumn("Backlog", typeof(System.Decimal));
            backColumn.DefaultValue = backlog;
            dataTable.Columns.Add(backColumn);
            //dataTable.Columns.Remove(dataTable.Columns[3]);
            return Json(dataTable.DataTableToList<chartdata>(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  teamwise



        public ActionResult TeamwiseChart(string team, string month, string year)
        {



            var dataSet = new DataSet();
            var dataTable = new DataTable();




            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;








            //Command = "select concat ( month,year) as monthyear, sum(empcount) as employeeCount,sum(Billablehrs) as Billablehrs,sum(externalhrs) as ExternalBilledHrs,sum(Appinternalhrs) as  InternalProjectHrs,sum(unbilledhrs) as UnbilledHrs from `Weekteamwisedetails`,`weekTeamwise` where  weekTeamwise.id=Weekteamwisedetails.`teamid` and weekTeamwise.month='" + month + "' and weekTeamwise.year='" + year + "' and weekTeamwise.empname='" + team + "' ";

            Command = "select month as name, empcount as 'Employeecount',sum(Billablehrs) as 'BillableHrs',sum(externalhrs) as 'ExternalBilledHrs',sum(Appinternalhrs) as  'InternalProjectHrs',sum(unbilledhrs) as 'UnbilledHrs' from `Weekteamwisedetails`,`weekTeamwise` where  weekTeamwise.id=Weekteamwisedetails.`teamid` and weekTeamwise.month='" + month + "' and weekTeamwise.year='" + year + "' and weekTeamwise.empname='" + team + "' ";





            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dataTable);

            }


            //dataTable.Columns.Remove(dataTable.Columns[3]);
            return Json(dataTable.DataTableToList<TeamwiseChart>(), JsonRequestBehavior.AllowGet);
        }




        public ActionResult TeamwiseMRM()
        {
            List<TeamwiseModel> model = new List<TeamwiseModel>();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  id as teamwiseid ,empname,month,year from `weekTeamwise`";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                model = dtt.DataTableToList<TeamwiseModel>();
                return View("TeamwiseMRM", model);
            }


        }



        public ActionResult Addteamwise()
        {


            TeamwiseMRMModel model = new TeamwiseMRMModel();
            model.TeamwiseModel = new TeamwiseModel();

            return PartialView("_AddTeamwiseMRM", model);
        }
        public bool checkteamwisemrmexistence(string month, string year, string empname)
        {
            bool Result;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string query = "select id  from `weekTeamwise` where month ='" + month + "'  and year='" + year + "' and empname='" + empname + "'";
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


        public ActionResult SaveTeamwiseMRM(TeamwiseMRMModel model)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            int teamid = 0;

            if (model.TeamwiseModel.empname == "1")
                model.TeamwiseModel.empname = "Salini";
            else if (model.TeamwiseModel.empname == "2")
                model.TeamwiseModel.empname = "Sabari";
            else if (model.TeamwiseModel.empname == "3")
                model.TeamwiseModel.empname = "Arun";
            else if (model.TeamwiseModel.empname == "4")
                model.TeamwiseModel.empname = "Sheik";

            if (model.TeamwiseModel.month == "1")
                model.TeamwiseModel.month = "January";
            else if (model.TeamwiseModel.month == "2")
                model.TeamwiseModel.month = "February";
            else if (model.TeamwiseModel.month == "3")
                model.TeamwiseModel.month = "March";
            else if (model.TeamwiseModel.month == "4")
                model.TeamwiseModel.month = "April";
            else if (model.TeamwiseModel.month == "5")
                model.TeamwiseModel.month = "May";
            else if (model.TeamwiseModel.month == "6")
                model.TeamwiseModel.month = "June";
            else if (model.TeamwiseModel.month == "7")
                model.TeamwiseModel.month = "July";
            else if (model.TeamwiseModel.month == "8")
                model.TeamwiseModel.month = "August";
            else if (model.TeamwiseModel.month == "9")
                model.TeamwiseModel.month = "September";
            else if (model.TeamwiseModel.month == "10")
                model.TeamwiseModel.month = "October";
            else if (model.TeamwiseModel.month == "11")
                model.TeamwiseModel.month = "November";
            else if (model.TeamwiseModel.month == "12")
                model.TeamwiseModel.month = "December";

            if (model.TeamwiseModel.year == "1")
                model.TeamwiseModel.year = "2021";
            else if (model.TeamwiseModel.year == "2")
                model.TeamwiseModel.year = "2020";
            else if (model.TeamwiseModel.year == "3")
                model.TeamwiseModel.year = "2019";



            if (model.TeamwiseModel.teamwiseid == 0)
            {
                if (!checkteamwisemrmexistence(model.TeamwiseModel.month, model.TeamwiseModel.year, model.TeamwiseModel.empname))
                {
                    using (MySqlConnection mConnection = new MySqlConnection(connString))
                    {
                        mConnection.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = mConnection;
                        cmd.CommandText = "INSERT INTO `weekTeamwise`(`empname`,`month`,`year` ) VALUES ('" + model.TeamwiseModel.empname + "','" + model.TeamwiseModel.month + "','" + model.TeamwiseModel.year + "');select last_insert_id();";
                        teamid = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();


                    }




                    foreach (var item in model.TeamwiseModel.LstItems)
                    {

                        using (MySqlConnection mConnection = new MySqlConnection(connString))
                        {
                            mConnection.Open();
                            MySqlCommand cmd = new MySqlCommand();
                            cmd.Connection = mConnection;
                            cmd.CommandText = "INSERT INTO `Weekteamwisedetails`(`teamid`,`weekinmonth`,`Empcount`,`Billablehrs`, `Externalhrs`,`Appinternalhrs`,`unbilledhrs`) VALUES (" + teamid + ",'" + item.weekinmonth + "'," + item.Empcount + "," + item.Billablehrs + "," + item.Externalhrs + "," + item.Appinternalhrs + "," + item.unbilledhrs + ");";
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();


                        }

                    }
                    TempData["Msg"] = "Successfully Saved!";
                }
                else
                {
                    TempData["Msg"] = "Already Exist!";
                }

            }

            else
            {

                string Command = "UPDATE `weekTeamwise` set `empname`='" + model.TeamwiseModel.empname + "', `month`='" + model.TeamwiseModel.month + "',`year`='" + model.TeamwiseModel.year + "' where `weekTeamwise`.id=" + model.TeamwiseModel.teamwiseid + ";Delete from `Weekteamwisedetails` where teamid=" + model.TeamwiseModel.teamwiseid + ";";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                    {
                        myCmd.ExecuteNonQuery();

                    }

                }

                foreach (var item in model.TeamwiseModel.LstItems)
                {

                    using (MySqlConnection mConnection = new MySqlConnection(connString))
                    {
                        mConnection.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = mConnection;
                        cmd.CommandText = "INSERT INTO `Weekteamwisedetails`(`teamid`,`weekinmonth`,`Empcount`,`Billablehrs`,`Externalhrs`,`Appinternalhrs`,`unbilledhrs` ) VALUES (" + model.TeamwiseModel.teamwiseid + ",'" + item.weekinmonth + "'," + item.Empcount + "," + item.Billablehrs + "," + item.Externalhrs + "," + item.Appinternalhrs + "," + item.unbilledhrs + ");";
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();


                    }



                }
                TempData["Msg"] = "Successfully Updated!";
            }




            return RedirectToAction("TeamwiseMRM");


        }


        public ActionResult GetViewDetails(string ID)
        {
            int Id = Convert.ToInt16(ID);
            TeamwiseMRMModel model = new TeamwiseMRMModel();
            model.TeamwiseModel = new TeamwiseModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT `id`,`empname`, `month`,`year` from `weekTeamwise` where `weekTeamwise`.`id`=" + Id + ";select weekinmonth,empcount,`Billablehrs`,Externalhrs,Appinternalhrs,unbilledhrs from `Weekteamwisedetails` where teamid=" + Id + " order by weekinmonth";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                //ds.Tables.Add(new DataTable());
                adapter.Fill(ds);
                DataTable dtt = ds.Tables[0];
                model.TeamwiseModel.empname = dtt.Rows[0]["empname"].ToString();
                model.TeamwiseModel.month = dtt.Rows[0]["month"].ToString();
                model.TeamwiseModel.year = dtt.Rows[0]["year"].ToString();
                model.TeamwiseModel.LstItems = ds.Tables[1].DataTableToList<teamwiseitem>();
            }
            return PartialView("/Views/MRM/_ViewTeamwiseMRM.cshtml", model);
        }

        public ActionResult GetTeamwiseMRM(string ID)
        {
            int Id = Convert.ToInt16(ID);



            TeamwiseMRMModel model = new TeamwiseMRMModel();
            model.TeamwiseModel = new TeamwiseModel();

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT `id`,`empname`, `month`,`year` from `weekTeamwise` where `weekTeamwise`.`id`=" + Id + ";select weekinmonth,empcount,`Billablehrs`,Externalhrs,Appinternalhrs,unbilledhrs from `Weekteamwisedetails` where teamid=" + Id + " order by weekinmonth";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                //ds.Tables.Add(new DataTable());
                adapter.Fill(ds);
                DataTable dtt = ds.Tables[0];

                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    if (dtt.Rows[i]["empname"].ToString() == "Salini")
                    {
                        model.TeamwiseModel.empname = "1";
                    }
                    else if (dtt.Rows[i]["empname"].ToString() == "Sabari")
                    {
                        model.TeamwiseModel.empname = "2";
                    }
                    else if (dtt.Rows[i]["empname"].ToString() == "Arun")
                    {
                        model.TeamwiseModel.empname = "3";
                    }
                    else if (dtt.Rows[i]["empname"].ToString() == "Sheik")
                    {
                        model.TeamwiseModel.empname = "4";
                    }

                    if (dtt.Rows[i]["month"].ToString() == "January")
                    {
                        model.TeamwiseModel.month = "1";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "February")
                    {
                        model.TeamwiseModel.month = "2";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "March")
                    {
                        model.TeamwiseModel.month = "3";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "April")
                    {
                        model.TeamwiseModel.month = "4";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "May")
                    {
                        model.TeamwiseModel.month = "5";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "June")
                    {
                        model.TeamwiseModel.month = "6";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "July")
                    {
                        model.TeamwiseModel.month = "7";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "August")
                    {
                        model.TeamwiseModel.month = "8";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "September")
                    {
                        model.TeamwiseModel.month = "9";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "October")
                    {
                        model.TeamwiseModel.month = "10";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "November")
                    {
                        model.TeamwiseModel.month = "11";
                    }
                    else if (dtt.Rows[i]["month"].ToString() == "December")
                    {
                        model.TeamwiseModel.month = "12";
                    }
                    model.TeamwiseModel.teamwiseid = int.Parse(dtt.Rows[i]["id"].ToString());

                    if (dtt.Rows[i]["year"].ToString() == "2021")
                    {
                        model.TeamwiseModel.year = "1";
                    }
                    else if (dtt.Rows[i]["year"].ToString() == "2020")
                    {
                        model.TeamwiseModel.year = "2";
                    }
                    else if (dtt.Rows[i]["year"].ToString() == "2019")
                    {
                        model.TeamwiseModel.year = "3";
                    }


                }



                model.TeamwiseModel.LstItems = ds.Tables[1].DataTableToList<teamwiseitem>();

            }






            return PartialView("/Views/MRM/_EditTeamwiseMRM.cshtml", model);
        }

        public ActionResult Deleteteamwise(string ID)
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
                    cmd.CommandText = "Delete from `weekTeamwise` where id=" + Id + ";Delete from `Weekteamwisedetails` where `teamid`=" + Id + ";";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();


                }



                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("TeamwiseMRM");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";
                return RedirectToAction("TeamwiseMRM");
            }
        }


        #endregion

        #region Revenueplan

        public ActionResult MonthlyRevenueplan()
        {
            MonthlySwTarget Model = new MonthlySwTarget();
            //string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //string Command = "SELECT  id as Id ,month,year,target from `monthlySwTarget`";
            //using (MySqlConnection mConnection = new MySqlConnection(connString))
            //{
            //mConnection.Open();
            //MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
            //DataSet ds = new DataSet();
            //ds.Tables.Add(new DataTable());
            //adapter.Fill(ds.Tables[0]);
            //DataTable dtt = ds.Tables[0];
            //Model.LstMonthlySwTarget = dtt.DataTableToList<MonthlySwTarget>();
            //return View("MonthlyRevenuePlan", Model);
            //}
            return View("MonthlyRevenuePlan", Model);
        }

        public ActionResult AddRevenueplan()
        {
            MonthlySwTarget model = new MonthlySwTarget();

            return PartialView("_AddRevenueplan", model);

        }

        public ActionResult SaveRevenueplan(MonthlySwTarget model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    int insertresult = RevenueplanExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageRevenueplan(model);
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
                    string Result = ManageRevenueplan(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }



            }
            return RedirectToAction("MonthlyRevenuePlan");

        }

        public string ManageRevenueplan(MonthlySwTarget model)
        {
            string Result = string.Empty;
            Result = "NotOk";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            int monthNumber = 0;
            string monthno = string.Empty;
            monthNumber = DateTime.ParseExact(model.Month, "MMMM", CultureInfo.CurrentCulture).Month;
            if (monthNumber < 10)
            {
                monthno = "0" + monthNumber;
            }
            else
            {
                monthno = monthNumber.ToString();
            }

            var Date1 = model.Year + "-" + monthno + "-01";
            //var Date2 = strArr[1].Trim().ToString() + "-03-31";

            string Command = string.Empty;
            if (model.Id == 0)
            {
                Command = "INSERT INTO `monthlySwTarget`(`month`,`year`, `target`) VALUES ('" + model.Month + "','" + model.Year + "'," + model.target + ");";
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

                Command = "UPDATE monthlySwTarget set `target`=" + model.target + ",month='" + model.Month + "',Year='" + model.Year + "' where `monthlySwTarget`.`id`=" + model.Id;
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


        public JsonResult FillTargetMonthly(string month, string year)
        {



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;



            double budgeINR = 0.0;


            string Command = "SELECT `target` from  `monthlySwTarget` where `month`='" + month + "' and year='" + year + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                mConnection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        budgeINR = reader.GetDouble("target");

                    }
                }
            }


            var result = new { budgeINR = budgeINR };
            return Json(result, JsonRequestBehavior.AllowGet);


        }




        public int RevenueplanExistence(MonthlySwTarget model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `monthlySwTarget` where month='" + model.Month + "' and `year`='" + model.Year + "'";
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


        public ActionResult GetRevenuePlan(string ID)
        {
            int Id = Convert.ToInt16(ID);
            MonthlySwTarget Model = new MonthlySwTarget();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT id,month,year,target FROM `monthlySwTarget` where `monthlySwTarget`.id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.Month = reader.GetString(1);
                    Model.Year = reader.GetString(2);
                    if (reader["target"] != DBNull.Value)
                        Model.target = Convert.ToDouble(reader.GetDouble(3));

                }

            }

            return PartialView("/Views/MRM/_AddRevenueplan.cshtml", Model);
        }

        public ActionResult DeleteRevenuePlan(string ID)
        {
            try
            {
                DeleteRevenuePlanDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("MonthlyRevenueplan");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";

                return RedirectToAction("MonthlyRevenueplan");
            }
        }


        public string DeleteRevenuePlanDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "delete from `monthlySwTarget` where `monthlySwTarget`.id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }





        #endregion



    }
}