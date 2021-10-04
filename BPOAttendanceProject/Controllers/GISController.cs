using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using BPOAttendanceProject.Models;
using System.Globalization;
using System.Collections;
using BPOAttendanceProject.Filters;

namespace BPOAttendanceProject.Controllers
{
     [UserFilter]
    public class GISController : Controller
    {

             #region gisservice


             public ActionResult GetGISViewDetails(string ID)
             {
                 int Id = Convert.ToInt16(ID);
                 GisServices Model = new GisServices();
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 string Command = "SELECT id,internalbilling,externalbilling, Resources,Attrition,BillsInternal,notbilled,Total,etoininr,etoinusd,month,year,bestperformer,`Idlehrs` from `GisService` where GisService.id=" + Id;
                 using (MySqlConnection mConnection = new MySqlConnection(connString))
                 {
                     mConnection.Open();
                     MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                     MySqlDataReader reader = cmd.ExecuteReader();
                     if (reader.Read())
                     {
                         Model.Id = reader.GetInt32(0);
                         //Model.softwareservices = reader.GetString(1);
                         Model.internalbilling = Convert.ToDouble(reader.GetDouble(1));
                         Model.externalbilling = Convert.ToDouble(reader.GetDouble(2));
                         Model.Resources = Convert.ToDouble(reader.GetDouble(3));
                         Model.Attrition = reader.GetString(4);
                         Model.billedincluded = Convert.ToDouble(reader.GetDouble(5));
                         Model.notbilled = Convert.ToDouble(reader.GetDouble(6));
                         Model.Total = Convert.ToDouble(reader.GetDouble(7));
                         Model.etoinINR = Convert.ToDouble(reader.GetDouble(8));
                         Model.etoinUSD = Convert.ToDouble(reader.GetDouble(9));
                         Model.bestperformer = reader.GetString(12);
                         Model.Month = reader.GetString(10);
                         Model.Year = reader.GetString(11);
                         if (reader["Idlehrs"] != DBNull.Value)
                         Model.idlehrs = Convert.ToDouble(reader.GetDouble(13));
                     }

                 }

                 return PartialView("/Views/GIS/_ViewGisservice.cshtml", Model);
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





             public ActionResult GetGisservices(string ID)
             {
                 int Id = Convert.ToInt16(ID);
                 GisServices Model = new GisServices();
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 string Command = "SELECT id,internalbilling,externalbilling, Resources,Attrition,BillsInternal,notbilled,Total,etoininr,etoinusd,month,year,bestperformer,`Idlehrs` from `GisService` where GisService.id=" + Id;
                 using (MySqlConnection mConnection = new MySqlConnection(connString))
                 {
                     mConnection.Open();
                     MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                     MySqlDataReader reader = cmd.ExecuteReader();
                     if (reader.Read())
                     {
                         Model.Id = reader.GetInt32(0);
                         //Model.softwareservices = reader.GetString(1);
                         Model.internalbilling = Convert.ToDouble(reader.GetDouble(1));
                         Model.externalbilling = Convert.ToDouble(reader.GetDouble(2));
                         Model.Resources = Convert.ToDouble(reader.GetDouble(3));
                         Model.Attrition = reader.GetString(4);
                         Model.billedincluded = Convert.ToDouble(reader.GetDouble(5));
                         Model.notbilled = Convert.ToDouble(reader.GetDouble(6));
                         Model.Total = Convert.ToDouble(reader.GetDouble(7));
                         Model.etoinINR = Convert.ToDouble(reader.GetDouble(8));
                         Model.etoinUSD = Convert.ToDouble(reader.GetDouble(9));
                         Model.bestperformer = reader.GetString(12);
                         string month = reader.GetString(10);
                         string year = reader.GetString(11);
                         if (reader["Idlehrs"] != DBNull.Value)
                         Model.idlehrs = Convert.ToDouble(reader.GetDouble(13));
                         if (month == "January")
                         {
                             Model.Month = "1";
                         }
                         else if (month == "February")
                         {
                             Model.Month = "2";
                         }
                         else if (month == "March")
                         {
                             Model.Month = "3";
                         }
                         else if (month == "April")
                         {
                             Model.Month = "4";
                         }
                         else if (month == "May")
                         {
                             Model.Month = "5";
                         }
                         else if (month == "June")
                         {
                             Model.Month = "6";
                         }
                         else if (month == "July")
                         {
                             Model.Month = "7";
                         }
                         else if (month == "August")
                         {
                             Model.Month = "8";
                         }
                         else if (month == "September")
                         {
                             Model.Month = "9";
                         }
                         else if (month == "October")
                         {
                             Model.Month = "10";
                         }
                         else if (month == "November")
                         {
                             Model.Month = "11";
                         }
                         else if (month == "December")
                         {
                             Model.Month = "12";
                         }


                         if (year == "2021")
                         {
                             Model.Year = "1";
                         }
                         else if (year == "2020")
                         {
                             Model.Year = "2";
                         }
                         else if (year == "2019")
                         {
                             Model.Year = "3";
                         }







                     }

                 }

                 return PartialView("/Views/GIS/_AddGISservice.cshtml", Model);
             }

             public ActionResult GisServices()
             {
                 GisServices Model = new GisServices();
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 string Command = "SELECT id, month,year,internalbilling,externalbilling, Resources,`etoininr`,etoinusd from `GisService`";
                 using (MySqlConnection mConnection = new MySqlConnection(connString))
                 {
                     mConnection.Open();
                     MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                     DataSet ds = new DataSet();
                     ds.Tables.Add(new DataTable());
                     adapter.Fill(ds.Tables[0]);
                     DataTable dtt = ds.Tables[0];
                     Model.LstGisServices = dtt.DataTableToList<GisServices>();
                     return View("GisServices", Model);
                 }


             }

             public ActionResult AddGisservice()
             {
                 GisServices model = new GisServices();

                 return PartialView("_AddGISservice", model);

             }

             public int GisServiceExistence(GisServices model)
             {
                 int Result = 0;
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 string Command = "SELECT count(*) as cnt FROM `GisService` where month='" + model.Month + "' and `year`='" + model.Year + "'";
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




             public ActionResult SaveGisservices(GisServices model)
             {
                 if (ModelState.IsValid)
                 {
                     if (model.Id == 0)
                     {
                         int insertresult = GisServiceExistence(model);
                         if (insertresult == 0)
                         {
                             string Result = ManageGisService(model);
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
                         string Result = ManageGisService(model);
                         if (Result.Trim('"') == "Ok")
                             TempData["Msg"] = "Successfully Saved!";
                         else
                             TempData["Msg"] = "Unsuccessfull Operation!";
                     }



                 }
                 return RedirectToAction("GisServices");

             }


             public string ManageGisService(GisServices model)
             {
                 string Result = string.Empty;
                 Result = "NotOk";
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

                 if (model.Month == "1")
                     model.Month = "January";
                 else if (model.Month == "2")
                     model.Month = "February";
                 else if (model.Month == "3")
                     model.Month = "March";
                 else if (model.Month == "4")
                     model.Month = "April";
                 else if (model.Month == "5")
                     model.Month = "May";
                 else if (model.Month == "6")
                     model.Month = "June";
                 else if (model.Month == "7")
                     model.Month = "July";
                 else if (model.Month == "8")
                     model.Month = "August";
                 else if (model.Month == "9")
                     model.Month = "September";
                 else if (model.Month == "10")
                     model.Month = "October";
                 else if (model.Month == "11")
                     model.Month = "November";
                 else if (model.Month == "12")
                     model.Month = "December";

                 if (model.Year == "1")
                     model.Year = "2021";
                 else if (model.Year == "2")
                     model.Year = "2020";
                 else if (model.Year == "3")
                     model.Year = "2019";

                 //string monthno = string.Empty;
                 //monthNumber = DateTime.ParseExact(model.Month, "MMMM", CultureInfo.CurrentCulture).Month;
                 //if (monthNumber < 10)
                 //{
                 //    monthno = "0" + monthNumber;
                 //}
                 //else
                 //{
                 //    monthno = monthNumber.ToString();
                 //}

                 //var Date1 = model.Year + "-" + monthno + "-01";
                 //var Date2 = strArr[1].Trim().ToString() + "-03-31";

                 string Command = string.Empty;
                 if (model.Id == 0)
                 {
                     Command = "INSERT INTO `GisService`(`month`,`year`, `internalbilling`,`externalbilling`,`Total`,etoininr,etoinusd,`Resources`,`Attrition`,`BillsInternal`,`notbilled`,bestperformer,`Idlehrs`) VALUES ('" + model.Month + "','" + model.Year + "'," + model.internalbilling + " ," + model.externalbilling + "," + model.Total + "," + model.etoinINR + "," + model.etoinUSD + "," + model.Resources + ",'" + model.Attrition + "'," + model.billedincluded + "," + model.notbilled + ",'" + model.bestperformer + "'," + model.idlehrs + ");";
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

                     Command = "UPDATE GisService set  `internalbilling`=" + model.internalbilling + ",externalbilling=" + model.externalbilling + ",Total=" + model.Total + ", etoininr=" + model.etoinINR + ",etoinusd=" + model.etoinUSD + ", Resources =" + model.Resources + ",Attrition='" + model.Attrition + "',BillsInternal=" + model.billedincluded + ",notbilled=" + model.notbilled + ", month='" + model.Month + "', year='" + model.Year + "',bestperformer='" + model.bestperformer + "',Idlehrs=" + model.idlehrs + " where GisService.Id=" + model.Id;
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
                     return RedirectToAction("GisServices");
                 }
                 catch (Exception)
                 {

                     TempData["Msg"] = "Unsuccessfull Operation!";

                     return RedirectToAction("GisServices");
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
                     cmd.CommandText = "delete from `GisService` where `GisService`.id=" + Id;
                     cmd.ExecuteNonQuery();
                     cmd.Dispose();
                     Result = "1";
                 }
                 return Result;
             }




             #endregion



             #region  MonthlyGisService

             public ActionResult MonthlyGisService()
             {
                 MonthlyGisservice Model = new MonthlyGisservice();
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 //string Command = "SELECT  id as Id ,month,year,budgeINR,ActualINR from `monthlyGisservice`";
                 string Command = "SELECT id as Id, month,year,budgeINR,ActualINR,yyyy, mm, cbacklog, (cbacklog + ( SELECT SUM(ActualINR-budgeINR) FROM `monthlyGisservice`  WHERE date >= STR_TO_DATE(CONCAT_WS('-', yyyy, mm, 1),'%Y-%c-%e') - INTERVAL 1 MONTH AND   date <  STR_TO_DATE(CONCAT_WS('-', yyyy, mm, 1),'%Y-%c-%e')))  AS cumbacklog FROM (SELECT id as Id, month,year,budgeINR,ActualINR, EXTRACT(YEAR FROM date) AS yyyy, EXTRACT(MONTH FROM date) AS mm, SUM(ActualINR-budgeINR) AS cbacklog FROM `monthlyGisservice` GROUP BY EXTRACT(YEAR FROM date), EXTRACT(MONTH FROM date)) AS x ORDER BY yyyy, mm";



                 using (MySqlConnection mConnection = new MySqlConnection(connString))
                 {
                     mConnection.Open();
                     MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                     DataSet ds = new DataSet();
                     ds.Tables.Add(new DataTable());
                     adapter.Fill(ds.Tables[0]);
                     DataTable dtt = ds.Tables[0];
                     Model.LstMonthlyGisservice = dtt.DataTableToList<MonthlyGisservice>();
                     return View("MonthlyGisService", Model);
                 }


             }

             public ActionResult AddMonthlyGisservice()
             {
                 MonthlyGisservice model = new MonthlyGisservice();

                 return PartialView("_AddMonthlyGisservice", model);

             }



             public ActionResult SaveGisservice(MonthlyGisservice model)
             {
                 if (ModelState.IsValid)
                 {
                     if (model.Id == 0)
                     {
                         int insertresult = MonthlyGISServiceExistence(model);
                         if (insertresult == 0)
                         {
                             string Result = ManageGisService(model);
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
                         string Result = ManageGisService(model);
                         if (Result.Trim('"') == "Ok")
                             TempData["Msg"] = "Successfully Saved!";
                         else
                             TempData["Msg"] = "Unsuccessfull Operation!";
                     }



                 }
                 return RedirectToAction("MonthlyGisService");

             }


             public string ManageGisService(MonthlyGisservice model)
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
                     Command = "INSERT INTO monthlyGisservice(`month`,`year`, `budgeINR`,`ActualINR`,date) VALUES ('" + model.Month + "','" + model.Year + "'," + model.budgeINR + "," + model.ActualINR + " ,'" + Date1 + "');";
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

                     Command = "UPDATE monthlyGisservice set `budgeINR`='" + model.budgeINR + "', `ActualINR`='" + model.ActualINR + "',month='" + model.Month + "',Year='" + model.Year + "',date='" + Date1 + "' where monthlyGisservice.id=" + model.Id;
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








             public int MonthlyGISServiceExistence(MonthlyGisservice model)
             {
                 int Result = 0;
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 string Command = "SELECT count(*) as cnt FROM `monthlyGisservice` where month='" + model.Month + "' and `year`='" + model.Year + "'";
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





             public ActionResult GetGisService(string ID)
             {
                 int Id = Convert.ToInt16(ID);
                 MonthlyGisservice Model = new MonthlyGisservice();
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 string Command = "SELECT id,month,year,budgeINR,ActualINR FROM monthlyGisservice where monthlyGisservice.id=" + Id;
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

                 return PartialView("/Views/GIS/_AddMonthlyGisservice.cshtml", Model);
             }

             public ActionResult DeleteGisservice(string ID)
             {
                 try
                 {
                     DeletGisserviceDetails(ID);
                     TempData["Msg"] = "Successfully Deleted";
                     return RedirectToAction("MonthlyGisService");
                 }
                 catch (Exception)
                 {

                     TempData["Msg"] = "Unsuccessfull Operation!";

                     return RedirectToAction("MonthlyGisService");
                 }
             }


             public string DeletGisserviceDetails(string ID)
             {
                 int Id = int.Parse(ID);
                 string Result = "0";
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 using (MySqlConnection mConnection = new MySqlConnection(connString))
                 {
                     mConnection.Open();
                     MySqlCommand cmd = new MySqlCommand();
                     cmd.Connection = mConnection;
                     cmd.CommandText = "delete from monthlyGisservice where monthlyGisservice.id=" + Id;
                     cmd.ExecuteNonQuery();
                     cmd.Dispose();
                     Result = "1";
                 }
                 return Result;
             }


             public ActionResult BarChart(string month,string year)
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

                 string backlogcommand = "select  sum(ActualINR)-  sum(budgeINR) as backlog from monthlyGisservice where date >='" + dfromdate + "' and date <='" + denddate + "'";

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







                 Command = "select concat ( month,year) as monthyear, budgeINR as BudgetINR,ActualINR,CEIL((ActualINR/budgeINR)*100) as Percent  from monthlyGisservice where monthlyGisservice.month='" + month + "'  and  monthlyGisservice.year=" + year + "";

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
                 MonthlyGISTarget Model = new MonthlyGISTarget();
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 string Command = "SELECT  id as Id ,month,year,target from `monthlyGISTarget`";
                 using (MySqlConnection mConnection = new MySqlConnection(connString))
                 {
                     mConnection.Open();
                     MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                     DataSet ds = new DataSet();
                     ds.Tables.Add(new DataTable());
                     adapter.Fill(ds.Tables[0]);
                     DataTable dtt = ds.Tables[0];
                     Model.LstMonthlyGISTarget = dtt.DataTableToList<MonthlyGISTarget>();
                     return View("MonthlyRevenuePlan", Model);
                 }


             }

             public ActionResult AddRevenueplan()
             {
                 MonthlyGISTarget model = new MonthlyGISTarget();

                 return PartialView("_AddRevenueplan", model);

             }

             public ActionResult SaveRevenueplan(MonthlyGISTarget model)
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

             public string ManageRevenueplan(MonthlyGISTarget model)
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
                     Command = "INSERT INTO `monthlyGISTarget`(`month`,`year`, `target`) VALUES ('" + model.Month + "','" + model.Year + "'," + model.target + ");";
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

                     Command = "UPDATE monthlyGISTarget set `target`=" + model.target + ",month='" + model.Month + "',Year='" + model.Year + "' where `monthlyGISTarget`.`id`=" + model.Id;
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


                 string Command = "SELECT `target` from  `monthlyGISTarget` where `month`='" + month + "' and year='" + year + "'";
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




             public int RevenueplanExistence(MonthlyGISTarget model)
             {
                 int Result = 0;
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 string Command = "SELECT count(*) as cnt FROM `monthlyGISTarget` where month='" + model.Month + "' and `year`='" + model.Year + "'";
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
                 MonthlyGISTarget Model = new MonthlyGISTarget();
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 string Command = "SELECT id,month,year,target FROM `monthlyGISTarget` where `monthlyGISTarget`.id=" + Id;
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

                 return PartialView("/Views/GIS/_AddRevenueplan.cshtml", Model);
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
                     cmd.CommandText = "delete from `monthlyGISTarget` where `monthlyGISTarget`.id=" + Id;
                     cmd.ExecuteNonQuery();
                     cmd.Dispose();
                     Result = "1";
                 }
                 return Result;
             }





             #endregion
       
    }
}
