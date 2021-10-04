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
using System.Web;
using System.Web.Mvc;

namespace BPOAttendanceProject.Controllers
{
    public class InvoiceController : Controller
    {
        //
        // GET: /Error/

        public ActionResult TargetvsRevenue()
        {
            InvoiceModel Model = new InvoiceModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  id as Id ,`MonthName`,`YearName`,Target,Actual,Achievement from `MonthlyTargetActual`";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                adapter.Fill(ds.Tables[0]);
                DataTable dtt = ds.Tables[0];
                Model.InvoiceModelList = dtt.DataTableToList<InvoiceModel>();
                return View("MonthlyTargetAchieverevenue", Model);
            }


        }

        public ActionResult AddTarget()
        {
            InvoiceModel model = new InvoiceModel();
            return PartialView("_Addtargetrevenue", model);
        }



        public ActionResult SaveTargetrevenue(InvoiceModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    int insertresult = MonthlyTargetAchieverevenueExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageTargetRevenue(model);
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
                    string Result = ManageTargetRevenue(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }



            }
            return RedirectToAction("TargetvsRevenue");

        }


        public string ManageTargetRevenue(InvoiceModel model)
        {
            string Result = string.Empty;
            Result = "NotOk";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            int monthNumber = 0;
            string monthno=string.Empty;
            monthNumber = DateTime.ParseExact(model.MonthName, "MMMM", CultureInfo.CurrentCulture).Month;
            if (monthNumber < 10)
            {
                monthno = "0" + monthNumber;
            }
            else
            {
                monthno = monthNumber.ToString();
            }
            //string[] strArr = null;
            //char[] splitchar = { '-' };
            //strArr = Year.Split(splitchar);
            var Date1 = model.YearName + "-" + monthno + "-01";
            //var Date2 = strArr[1].Trim().ToString() + "-03-31";


            if (model.Id == 0)
            {
                string Command = "INSERT INTO MonthlyTargetActual(`MonthName`,`YearName`, `Target`,`Actual`,`Achievement`,date) VALUES ('" + model.MonthName + "','" + model.YearName + "'," + model.Target + "," + model.Actual + " ," + model.Achievement + ",'" + Date1  + "');";
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

                string Command = "UPDATE MonthlyTargetActual set `Target`='" + model.Target + "', `Actual`='" + model.Actual + "',`Achievement`=" + model.Achievement + ",MonthName='" + model.MonthName + "',YearName='" + model.YearName + "',date='" + Date1 + "' where MonthlyTargetActual.Id=" + model.Id;
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








        public int MonthlyTargetAchieverevenueExistence(InvoiceModel model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `MonthlyTargetActual` where `MonthName`='" + model.MonthName + "' and `YearName`='" + model.YearName + "'";
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



        public ActionResult GetTargetPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            InvoiceModel Model = new InvoiceModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Id,MonthName,YearName,Target,Actual,Achievement FROM MonthlyTargetActual where MonthlyTargetActual.Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.MonthName = reader.GetString(1);
                    Model.YearName = reader.GetString(2);
                    Model.Target = Convert.ToDouble(reader.GetDouble(3));
                    Model.Actual = Convert.ToDouble(reader.GetDouble(4));
                    Model.Achievement = Convert.ToDouble(reader.GetDouble(5));

                }

            }

            return PartialView("/Views/Invoice/_Addtargetrevenue.cshtml", Model);
        }


        public ActionResult Dashboardchart()
        {
            return View("Dashboardchart");
        }

        public ActionResult YearlyTarRevenueReport(string Year)
        {

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            string[] strArr = null;
            char[] splitchar = { '-' };
            strArr = Year.Split(splitchar);
            var Date1 = strArr[0].Trim().ToString() + "-04-01";
            var Date2 = strArr[1].Trim().ToString() + "-03-31";
            InvoiceModel Model = new InvoiceModel();



            Command = "select concat ( MonthName,YearName) as MonthName, Target,Actual,Achievement  from MonthlyTargetActual where date >='" + Date1 + "' and  date <='" + Date2 + "' ";

            Command = Command + "   order by year(date), month(date)";


            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }


            Model.InvoiceModelList = dt.DataTableToList<InvoiceModel>();

            ViewBag.Daylist = "Summary of Target Vs Achievement -ITeS  " + Year;
            return PartialView("_targetvsrevenuelist", Model);



        }

        public ActionResult DisplayPOCdata(string month, string year)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;

            PocWiseModel Model = new PocWiseModel();



            Command = "select  MonthName,YearName,`PocName`, Target,Actual,`Achieved`  from `MonthlyPOCDetails` where YearName ='" + year + "' and  MonthName ='" + month + "' ";

           


            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }
            Model.PocWiseModelList = dt.DataTableToList<PocWiseModel>();

            ViewBag.Monthlist = "POCwise Target Vs Achievement of   " +  month + "-" + year;
            return PartialView("_poctargetrevenue", Model);

        }



       

         public ActionResult BarChart(string Year)
       {
         
           string constr = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;

           var dataSet = new DataSet();
           var dataTable = new DataTable();

          


           string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
           string Command = string.Empty;
           string[] strArr = null;
           char[] splitchar = { '-' };
           strArr = Year.Split(splitchar);
          
           var Date1 = strArr[0].Trim().ToString() + "-04-01";
           var Date2 = strArr[1].Trim().ToString() + "-03-31";




           //Command = "select  concat (monthname(date),year(date)) as monthyear,Round(sum(targetrevenue),0) as Target,Round(sum(actualrevenue),0) as Actual,Round((sum(actualrevenue)/sum(targetrevenue)*100),0) as RevAchievement,monthname(date) as month from productionreport2020 where date >='" + Date1 + "' and  date <='" + Date2 + "' ";

           //Command = Command + "  group by monthname(date) order by year(date), month(date)";


           Command = "select concat ( MonthName,YearName) as monthyear, Target,Actual,Achievement  from MonthlyTargetActual where date >='" + Date1 + "' and  date <='" + Date2 + "' ";

           Command = Command + "   order by year(date), month(date)";


           using (MySqlConnection mConnection = new MySqlConnection(connString))
           {
               mConnection.Open();
               MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
               adapter.Fill(dataTable);

           }



          

           dataTable.Columns.Remove(dataTable.Columns[3]);
           //dataTable.Columns.Remove(dataTable.Columns[3]);

           //chartdata Model = new chartdata();

           //List<chartdata> chartDetails = new List<chartdata>();

           //for (int i = 0; i < dataTable.Rows.Count; i++)
           //{
           //    chartdata chartd = new chartdata();
             
           //    chartd.monthyear = dataTable.Rows[i]["StudentName"].ToString();
           //    chartd.Target = Convert.ToDouble(dataTable.Rows[i]["Target"].ToString());
           //    chartd.Actual = Convert.ToDouble(dataTable.Rows[i]["Actual"].ToString());
           //    chartDetails.Add(chartd);
           //}



           return Json(dataTable.DataTableToList<chartdata>(), JsonRequestBehavior.AllowGet);
       }



         public ActionResult DeleteTarget(string ID)
         {
             try
             {
                 DeleteTargetDetails(ID);
                 TempData["Msg"] = "Successfully Deleted";
                 return RedirectToAction("TargetvsRevenue");
             }
             catch (Exception)
             {

                 TempData["Msg"] = "Unsuccessfull Operation!";
                 return RedirectToAction("TargetvsRevenue");
             }
         }


         public string DeleteTargetDetails(string ID)
         {
             int Id = int.Parse(ID);
             string Result = "0";
             string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             using (MySqlConnection mConnection = new MySqlConnection(connString))
             {
                 mConnection.Open();
                 MySqlCommand cmd = new MySqlCommand();
                 cmd.Connection = mConnection;
                 cmd.CommandText = "delete from MonthlyTargetActual where MonthlyTargetActual.Id=" + Id;
                 cmd.ExecuteNonQuery();
                 cmd.Dispose();
                 Result = "1";
             }
             return Result;
         }

         public ActionResult UploadPOCWise()
         {
             return View("UploadPOCWise");
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Pocwisedata(HttpPostedFileBase upload,InvoiceModel model)
         {

             try
             {
                 string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                      
               if (ModelState.IsValid)
                {
                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        bool inserted;
                      if (upload != null && upload.ContentLength > 0)
                       {
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
                            return View("UploadPOCWise");
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
                                inserted = InsertPOCTablebyAdmin(dataTable, model.MonthName, model.YearName);
                                if (inserted)
                                {
                                   
                                        ModelState.AddModelError("File", "File Uploaded Successfully");
                                        return View("UploadPOCWise");
                                   


                                }

                                else
                                {
                                    ModelState.AddModelError("File", "Error in uploading file");
                                    return View("UploadPOCWise");
                                }
                            }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }

               return View("UploadPOCWise");
            }
            catch (Exception ex)
            {

                //string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                //message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                //message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                //ModelState.AddModelError(string.Empty, message);
                
                
                ModelState.AddModelError("File", "Error in uploading file");
                return View("UploadPOCWise");
            }

        }


         public bool InsertPOCTablebyAdmin(DataTable dtcurrenttable, string drpMonth, string drpYear)
         {

             try
             {
                 

                
                 string result2 = string.Empty;
                 string day = string.Empty;
                 string month = string.Empty;
                 string year = string.Empty;
                 string mysqlConnString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                 for (int i = 0; i < dtcurrenttable.Rows.Count; i++)
                 {

                    
                     double whome = 0;
                     int monthNumber = 0;
                     string monthno = string.Empty;
                     monthNumber = DateTime.ParseExact(drpMonth, "MMMM", CultureInfo.CurrentCulture).Month;
                     if (monthNumber < 10)
                     {
                         monthno = "0" + monthNumber;
                     }
                     else
                     {
                         monthno = monthNumber.ToString();
                     }
                     //string[] strArr = null;
                     //char[] splitchar = { '-' };
                     //strArr = Year.Split(splitchar);
                     var Date1 = drpYear + "-" + monthno + "-01";



                     string Command = string.Empty;



                     Command = "INSERT INTO `MonthlyPOCDetails`(MonthName,YearName,PocName,Target,Actual,Achieved,`date` ) VALUES ('" + drpMonth + "','" + drpYear + "','" + dtcurrenttable.Rows[i]["Name"].ToString() + "'," + dtcurrenttable.Rows[i]["Target"].ToString() + "," + dtcurrenttable.Rows[i]["Achieved"].ToString() + "," + dtcurrenttable.Rows[i]["Percentage"].ToString() + ",'" + Date1 + "');";

                    

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

         public ActionResult POCWiseReport()
         {
             return View("POCWiseReport");
         }

         public ActionResult POCReport(string Year)
         {

             TargetrevenueActualModel Model = new TargetrevenueActualModel();
             string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
             string Command = string.Empty;



             string[] strArr = null;
             char[] splitchar = { '-' };
             strArr = Year.Split(splitchar);
             var Date1 = strArr[0].Trim().ToString() + "-04-01";
             var Date2 = strArr[1].Trim().ToString() + "-03-31";


             //CONCAT(x.FNameTxt,'  ',x.LNameTxt)

             Command = "select monthname(date) as month, concat (LEFT(monthname(date),3),'-',year(date)) as date,PocName,Round(sum(Actual),0) as Actual from MonthlyPOCDetails where      date >='" + Date1 + "' and  date <='" + Date2 + "' ";

             //Command = "select monthname(date) as month, concat (LEFT(monthname(date),3),year(date)) as date,PocName,Round(sum(Actual),0) as Actual from MonthlyPOCDetails where      date >='" + Date1 + "' and  date <='" + Date2 + "' ";
             Command = Command + "  group by monthname(date),PocName order by year(date), month(date)";




             DataTable dt = new DataTable();
             using (MySqlConnection mConnection = new MySqlConnection(connString))
             {
                 mConnection.Open();
                 MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                 adapter.Fill(dt);

             }

             var data2 = dt.AsEnumerable().Select(x => new
             {
                 PocName = x.Field<String>("PocName"),
                 date = x.Field<String>("date"),
                 Actual = x.Field<Double>("Actual")
             });

             DataTable pivotDataTable = data2.ToPivotTable(
                 item => item.date,
                item => item.PocName,
                items => items.Any() ? items.Sum(x => x.Actual) : 0);



             //if (pivotDataTable.Rows.Count > 0)
             //{
             //    DataColumn dcolColumn = new DataColumn("Total");
             //    pivotDataTable.Columns.Add(dcolColumn);
             //    foreach (DataRow row in pivotDataTable.Rows)
             //    {
             //        double rowTotal = 0;
             //        foreach (DataColumn col in row.Table.Columns)
             //        {

             //            if (col.ColumnName != "PocName")
             //            {
             //                if (row[col].ToString() != "")
             //                    rowTotal += double.Parse(row[col].ToString());
             //            }
             //        }
             //        row["Total"] = rowTotal.ToString("#,##0");
             //    }


             //    DataRow totalsRow = pivotDataTable.NewRow();
             //    totalsRow[0] = "Total";
             //    foreach (DataColumn col in pivotDataTable.Columns)
             //    {
             //        double colTotal = 0;
             //        foreach (DataRow row in col.Table.Rows)
             //        {
             //            if (col.ColumnName != "PocName")
             //            {
             //                if (row[col].ToString() != "")
             //                    colTotal += double.Parse(row[col].ToString());
             //            }
             //        }
             //        if (col.ColumnName == "PocName")
             //        {
             //            totalsRow[col.ColumnName] = "Shan";
             //        }
             //        else
             //        {
             //            totalsRow[col.ColumnName] = colTotal.ToString("#,##0");
             //        }
             //    }

             //    pivotDataTable.Rows.Add(totalsRow);
             //}


             return PartialView("_Pocwisereport", pivotDataTable);


         }







    }
}