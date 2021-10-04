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
using Newtonsoft.Json;
using System.Text;

namespace BPOAttendanceProject.Controllers
{
    [UserFilter]
    public class MMSController : Controller
    {
        //
        // GET: /Chart/


        #region ProjectManagement

        public ActionResult ProjectMgmtList()
        {
            ProjectMgmt Model = new ProjectMgmt();
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  `ProjectMgmt`.Id,Month,Year,Client.ClientType as clienttype,Client.ClientName as clientname,Target,Gained,Round((Gained/Target)*100,0) as Achieved,Client.Id as clientId from `ProjectMgmt`,Client where `ProjectMgmt`.ClientId=Client.Id";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);
                Model.LstProjectMgmt = dt.DataTableToList<ProjectMgmt>();
                return View("ProjectList", Model);
            }


        }

        public ActionResult AddProject()
        {
            ProjectMgmt model = new ProjectMgmt();
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  Id,`ClientName` from `Client`";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);
            }
            model.LstClientMgmt = dt.DataTableToList<ClientMgmt>();
            return PartialView("_AddProject", model);

        }
        public string BindClient(string clienttype)
        {
            DataTable dt = new DataTable();
            List<ClientMgmt> objproject = new List<ClientMgmt>();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  Id,`ClientName` from `Client` where `ClientType`='" + clienttype + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }

            var JSONString = new StringBuilder();

            if (dt.Rows.Count > 0)
            {
                JSONString.Append("[");
                foreach (DataRow dr in dt.Rows)
                {

                    JSONString.Append("{\"Id\":\"" + dr["Id"] + "\",\"ClientName\":\""
                                       + dr["ClientName"] + "\"},");
                }
                JSONString.Remove(JSONString.Length - 1, 1).Append("]");
            }

            return JSONString.ToString();




        }





        private int GetclientId(string Clientname)
        {
            int clientid = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Id FROM `Client` where `ClientName`='" + Clientname + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clientid = reader.GetInt32(0);
                    }

                }
            }
            return clientid;
        }


        public ActionResult SaveProjectMgmt(ProjectMgmt model, string ddlclient)
        {
            if (ModelState.IsValid)
            {

                model.clientId = int.Parse(ddlclient.ToString());
                if (model.Id == 0)
                {
                    int insertresult = ProjectMgmtExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageProjectMgmt(model);
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
                    string Result = ManageProjectMgmt(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }



            }
            return RedirectToAction("ProjectMgmtList");

        }

        public string ManageProjectMgmt(ProjectMgmt model)
        {
            string Result = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            if (model.Id == 0)
            {
                Command = "INSERT INTO ProjectMgmt(Month,Year,Clienttype,clientId,Target,Gained) VALUES ('" + model.Month + "'," + model.Year + ",'" + model.clienttype + "'," + model.clientId + "," + model.Target + "," + model.Gained + ");";
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

                Command = "UPDATE ProjectMgmt set Month='" + model.Month + "',Year=" + model.Year + ", `ClientType`='" + model.clienttype + "', ClientId=" + model.clientId + ",Target=" + model.Target + ",Gained=" + model.Gained + " where `ProjectMgmt`.`Id`=" + model.Id;
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




        public int ProjectMgmtExistence(ProjectMgmt model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `ProjectMgmt` where Month='" + model.Month + "' and `Year`=" + model.Year + " and ClientId=" + model.clientId + " and Clienttype='" + model.clienttype + "'";
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


        public ActionResult GetProjectPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            ProjectMgmt Model = new ProjectMgmt();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT ProjectMgmt.Id,Month,Year,ProjectMgmt.ClientType,ClientId,Target,Gained,`ClientName` FROM `ProjectMgmt`,`Client` where `ProjectMgmt`.ClientId=`Client`.Id  and  `ProjectMgmt`.Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.Month = reader.GetString(1);
                    Model.Year = reader.GetInt32(2);
                    Model.clienttype = reader.GetString(3);
                    Model.clientId = reader.GetInt32(4);
                    Model.Target = reader.GetDouble(5);
                    Model.Gained = reader.GetDouble(6);
                    Model.clientname = reader.GetString(7);
                }



            }
            DataTable dt = new DataTable();
            string clientCommand = "SELECT  Id,`ClientName` from `Client`";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(clientCommand, mConnection);
                adapter.Fill(dt);
            }
            Model.LstClientMgmt = dt.DataTableToList<ClientMgmt>();
            return PartialView("/Views/MMS/_AddProject.cshtml", Model);
        }

        public ActionResult DeleteProject(string ID)
        {
            try
            {
                DeleteProjectDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("ProjectMgmtList");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";

                return RedirectToAction("ProjectMgmtList");
            }
        }


        public string DeleteProjectDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "delete from `ProjectMgmt` where `ProjectMgmt`.Id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }



        public ActionResult MMSViewDetails()
        {
            string Month = DateTime.Now.AddMonths(-1).ToString("MMMM");
            string Year = DateTime.Now.Year.ToString();
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  Client.ClientName as clientname,Target,Gained,Round((Gained/Target)*100,0) as Achieved from `ProjectMgmt`,Client where `ProjectMgmt`.ClientId=Client.Id and ProjectMgmt.`Clienttype`='MMS' and ProjectMgmt.Month='" + Month + "' and ProjectMgmt.Year= " + Year + "";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }

            DataTable transposedTable = GenerateTransposedTable(dt);
            return PartialView("/Views/MMS/_ViewClientMMSlist.cshtml", transposedTable);
        }

        public ActionResult OnlineViewDetails()
        {

            string Month = DateTime.Now.AddMonths(-1).ToString("MMMM");
            string Year = DateTime.Now.Year.ToString();
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  Client.ClientName as clientname,Target,Gained,Round((Gained/Target)*100,0) as Achieved from `ProjectMgmt`,Client where `ProjectMgmt`.ClientId=Client.Id and ProjectMgmt.`Clienttype`='BPO Online' and ProjectMgmt.Month='" + Month + "' and ProjectMgmt.Year= " + Year + "";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }
            DataTable transposedTable = GenerateTransposedTable(dt);
            return PartialView("/Views/MMS/_ViewClientBPOlist.cshtml", transposedTable);
        }


        public ActionResult CallViewDetails()
        {

            string Month = DateTime.Now.AddMonths(-1).ToString("MMMM");
            string Year = DateTime.Now.Year.ToString();
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  Client.ClientName as clientname,Target,Gained,Round((Gained/Target)*100,0) as Achieved from `ProjectMgmt`,Client where `ProjectMgmt`.ClientId=Client.Id and ProjectMgmt.`Clienttype`='Call Center' and ProjectMgmt.Month='" + Month + "' and ProjectMgmt.Year= " + Year + "";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }
            DataTable transposedTable = GenerateTransposedTable(dt);
            return PartialView("/Views/MMS/_ViewClientCalllist.cshtml", transposedTable);
        }



        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }






        #endregion


        #region Updates


        public ActionResult UpdatesList()
        {
            Updates Model = new Updates();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  Id,Month,Year,Comments from Updates order by Id desc ";
            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);

                adapter.Fill(dt);

                Model.LstUpdates = dt.DataTableToList<Updates>();
                return View("UpdatesList", Model);
            }


        }

        public ActionResult AddUpdates()
        {
            Updates model = new Updates();
            return PartialView("_AddUpdates", model);

        }




        public ActionResult SaveUpdates(Updates model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    int insertresult = UpdateExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageUpdates(model);
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
                    string Result = ManageUpdates(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }



            }
            return RedirectToAction("UpdatesList");

        }

        public string ManageUpdates(Updates model)
        {
            string Result = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            if (model.Id == 0)
            {
                Command = "INSERT INTO Updates(Month,Year,Comments) VALUES ('" + model.Month + "'," + model.Year + ",'" + model.Comments + "');";
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

                Command = "UPDATE Updates set `Month`='" + model.Month + "', `Year`=" + model.Year + ",comments='" + model.Comments + "' where `Updates`.`Id`=" + model.Id;
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




        public int UpdateExistence(Updates model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `Updates` where Month='" + model.Month + "' and `Year`='" + model.Year + "' and Comments='" + model.Comments + "'";
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


        public ActionResult GetUpdatesPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            Updates Model = new Updates();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Id,Month,Year,Comments FROM `Updates` where `Updates`.Id=" + Id;
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
                    Model.Comments = reader.GetString(3);
                }



            }

            return PartialView("/Views/MMS/_AddUpdates.cshtml", Model);
        }

        public ActionResult Deleteupdates(string ID)
        {
            try
            {
                Deleteupdatedetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("UpdateList");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";

                return RedirectToAction("UpdatesList");
            }
        }


        public string Deleteupdatedetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "delete from `Updates` where `Updates`.Id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }


        #endregion

        #region ClientManagement
        public ActionResult NewClientMgmtList()
        {
            NewClientMgmt Model = new NewClientMgmt();
            DataTable dt1 = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Id, Name FROM Agent";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt1);
            }
            Model.LstClientMgmt = dt1.DataTableToList<NewClientMgmt>();
            return View("NewClientList", Model);
        }

        public ActionResult GetClientViewDetails()
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command1 = "SELECT Id as BPOId,ClientName as BPOclient from Client  where clienttype='BPO Online'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command1, mConnection);
                adapter.Fill(dt2);

            }


            string Command = "SELECT Id as MMSId,ClientName as MMSclient from Client  where clienttype='MMS'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt1);

            }



            if (dt1.Rows.Count > dt2.Rows.Count)
            {

            }

            else
            {
            }



            //DataTable dtSourcePvt = new DataTable();
            //dtSourcePvt.Columns.Add("MMSId");
            //dtSourcePvt.Columns.Add("MMS");
            //dtSourcePvt.Columns.Add("BPOId");
            //dtSourcePvt.Columns.Add("BPO Online");
            //DataRow workRow;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{

            //    if (dt.Rows[i]["ClientType"].ToString() == "MMS")
            //    {

            //        if (dt.Rows[i]["ClientName"].ToString() != "")
            //        {
            //            workRow = dtSourcePvt.NewRow();
            //            workRow[0] = dt.Rows[i]["Id"].ToString();
            //            workRow[1] = dt.Rows[i]["ClientName"].ToString();
            //            dtSourcePvt.Rows.Add(workRow);
            //        }
            //    }

            //}
            //int k = 0;
            //for (int j = 0; j < dt.Rows.Count; j++)
            //{
            //    if (dt.Rows[j]["ClientType"].ToString() == "BPO Online")
            //    {

            //        if (dt.Rows[j]["ClientName"].ToString() != null)
            //        {
            //            dtSourcePvt.Rows[k]["BPOId"] = dt.Rows[j]["Id"].ToString();
            //            dtSourcePvt.Rows[k]["BPO Online"] = dt.Rows[j]["ClientName"].ToString();
            //            k = k + 1;
            //        }
            //    }

            //}

            return PartialView("/Views/MMS/_ViewClientlist.cshtml", dt3);

        }






        public ActionResult ClientMgmtList()
        {
            ClientMgmt Model = new ClientMgmt();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  Id,ClientType,ClientName from Client ";
            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);
                Model.LstClientMgmt = dt.DataTableToList<ClientMgmt>();
                return View("CientsList", Model);
            }


        }

        public ActionResult AddClient()
        {
            ClientMgmt model = new ClientMgmt();
            return PartialView("_AddClient", model);

        }




        public ActionResult SaveClientMgmt(ClientMgmt model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    int insertresult = 0;
                    //int insertresult = ClientMgmtExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageClientMgmt(model);
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
                    string Result = ManageClientMgmt(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }



            }
            return RedirectToAction("NewClientMgmtList");

        }

        public string ManageClientMgmt(ClientMgmt model)
        {
            string Result = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            if (model.Id == 0)
            {
                Command = "INSERT INTO Agent(Name) VALUES ('" + model.Name + "');";
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
                Command = "UPDATE Agent set `Name`='" + model.Name + "' where `Id`=" + model.Id;
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




        public int ClientMgmtExistence(ClientMgmt model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `Client` where ClientType='" + model.clienttype + "' and `ClientName`='" + model.clientname + "'";
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


        public ActionResult GetClientPopup(string ID)
        {
            int Id = Convert.ToInt16(ID);
            ClientMgmt Model = new ClientMgmt();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT Id, Name FROM `Agent` where Id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.Name = reader.GetString(1);
                }
            }
            return PartialView("/Views/MMS/_AddClient.cshtml", Model);
        }

        public ActionResult DeleteClient(string ID)
        {
            try
            {
                DeleteClientDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("NewClientMgmtList");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";

                return RedirectToAction("NewClientMgmtList");
            }
        }


        public string DeleteClientDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "delete from `Agent` where Id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }





        #endregion






        #region  MonthlyRevenue

        public ActionResult MonthlyMMSRevenue()
        {
            MonthlyMMS Model = new MonthlyMMS();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            //string Command = "SELECT  id as Id ,month,year,budgeINR,ActualINR from `monthlyMMS`";

            string Command = "SELECT id as Id, month,year,budgeINR,ActualINR,yyyy, mm, cbacklog, (cbacklog + ( SELECT SUM(ActualINR-budgeINR) FROM monthlyMMS  WHERE date >= STR_TO_DATE(CONCAT_WS('-', yyyy, mm, 1),'%Y-%c-%e') - INTERVAL 1 MONTH AND   date <  STR_TO_DATE(CONCAT_WS('-', yyyy, mm, 1),'%Y-%c-%e')))  AS cumbacklog FROM (SELECT id as Id, month,year,budgeINR,ActualINR, EXTRACT(YEAR FROM date) AS yyyy, EXTRACT(MONTH FROM date) AS mm, SUM(ActualINR-budgeINR) AS cbacklog FROM monthlyMMS GROUP BY EXTRACT(YEAR FROM date), EXTRACT(MONTH FROM date)) AS x ORDER BY yyyy, mm";



            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);
                Model.LstMonthlyMMS = dt.DataTableToList<MonthlyMMS>();
                return View("MonthlyMMS", Model);
            }


        }

        public ActionResult AddMMS()
        {
            MonthlyMMS model = new MonthlyMMS();

            return PartialView("_AddMonthlyMMS", model);

        }

        public ActionResult DetailsMMS(string month, string year)
        {
            DataTable dataTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            MonthlyMMS Model = new MonthlyMMS();
            decimal backlog = 0;
            decimal backlogMMS = 0;
            decimal backlogOnline = 0;
            decimal backlogCall = 0;
            int monthNumber = 0;
            string monthno = string.Empty;
            monthNumber = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month;
            if (monthNumber < 10)
            {
                monthno = "0" + monthNumber;
            }
            else
            {
                monthno = monthNumber.ToString();
            }

            var dfromdate = year + "-" + "04" + "-01";

            var denddate = year + "-" + monthno + "-30";

            string backlogcommand = "select  sum(ActualINR)-  sum(budgeINR) as backlog from `monthlyMMS` where date >='" + dfromdate + "' and date <='" + denddate + "'";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(backlogcommand))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // read
                    {

                        if (!reader.IsDBNull(0))
                        {


                            backlog = Convert.ToDecimal(reader["backlog"]);
                        }

                    }



                }

            }

            string backlogMMScommand = "select  sum(MMSActualINR)-  sum(MMSbudgeINR) as backlog from `monthlyMMS` where date >='" + dfromdate + "' and date <='" + denddate + "'";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(backlogMMScommand))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // read
                    {
                        if (!reader.IsDBNull(0))
                        {
                            backlogMMS = Convert.ToDecimal(reader["backlog"]);
                        }

                    }



                }

            }


            string backlogOncommand = "select  sum(ONbudgeINR)-  sum(ONActualINR) as backlog from `monthlyMMS` where date >='" + dfromdate + "' and date <='" + denddate + "'";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(backlogOncommand))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // read
                    {
                        if (!reader.IsDBNull(0))
                        {

                            backlogOnline = Convert.ToDecimal(reader["backlog"]);
                        }

                    }



                }

            }

            string backlogcallcommand = "select  sum(CallbudgeINR)-  sum(CallActualINR) as backlog from `monthlyMMS` where date >='" + dfromdate + "' and date <='" + denddate + "'";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(backlogcallcommand))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // read
                    {
                        if (!reader.IsDBNull(0))
                        {
                            backlogCall = Convert.ToDecimal(reader["backlog"]);
                        }

                    }



                }

            }





            Command = "SELECT  month ,year,budgeINR,ActualINR,`MMSbudgeINR`,`MMSActualINR`,`ONbudgeINR`,`ONActualINR`,Comments,CallbudgeINR,CallActualINR FROM `monthlyMMS` where `monthlyMMS`.month='" + month + "' and `monthlyMMS`.year=" + year + "";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    Model.Month = reader.GetString(0);
                    Model.Year = reader.GetString(1);
                    Model.budgeINR = Convert.ToDouble(reader.GetDouble(2));
                    Model.ActualINR = Convert.ToDouble(reader.GetDouble(3));
                    Model.MMSbudgeINR = Convert.ToDouble(reader.GetDouble(4));
                    Model.MMSActualINR = Convert.ToDouble(reader.GetDouble(5));
                    Model.ONbudgeINR = Convert.ToDouble(reader.GetDouble(6));
                    Model.ONActualINR = Convert.ToDouble(reader.GetDouble(7));
                    if (!reader.IsDBNull(8))
                    {
                        Model.Comments = reader.GetString(8);

                    }
                    if (!reader.IsDBNull(9))
                    {
                        Model.CallbudgeINR = Convert.ToDouble(reader.GetDouble(9));

                    }
                    if (!reader.IsDBNull(10))
                    {
                        Model.CallActualINR = Convert.ToDouble(reader.GetDouble(10));

                    }
                    Model.cbacklog = backlog;
                    Model.cumbacklog = backlogMMS;
                    Model.onbacklog = backlogOnline;
                    Model.callbacklog = backlogCall;
                }

            }




            return PartialView("_DetailMMS", Model);

        }


        public ActionResult SaveMMSservice(MonthlyMMS model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    int insertresult = MonthlyMMSServiceExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageMMSService(model);
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
                    string Result = ManageMMSService(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }



            }
            return RedirectToAction("MonthlyMMSRevenue");

        }

        public string ManageMMSService(MonthlyMMS model)
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
                Command = "INSERT INTO monthlyMMS(`month`,`year`, `budgeINR`,`ActualINR`,`MMSbudgeINR`,`MMSActualINR`,`ONbudgeINR`,`ONActualINR`,date,Comments,CallbudgeINR,CallActualINR) VALUES ('" + model.Month + "','" + model.Year + "'," + model.budgeINR + "," + model.ActualINR + " ," + model.MMSbudgeINR + "," + model.MMSActualINR + "," + model.ONbudgeINR + "," + model.ONActualINR + ", '" + Date1 + "','" + model.Comments + "'," + model.CallbudgeINR + "," + model.CallActualINR + ");";
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

                Command = "UPDATE monthlyMMS set `budgeINR`=" + model.budgeINR + ", `ActualINR`=" + model.ActualINR + ",`MMSbudgeINR`=" + model.MMSbudgeINR + ",`MMSActualINR`=" + model.MMSActualINR + ",ONbudgeINR=" + model.ONbudgeINR + ", ONActualINR=" + model.ONActualINR + ",CallbudgeINR=" + model.CallbudgeINR + ",CallActualINR=" + model.CallActualINR + ", month='" + model.Month + "',Year='" + model.Year + "',date='" + Date1 + "',Comments='" + model.Comments + "', CallbudgeINR=" + model.CallbudgeINR + ",CallActualINR=" + model.CallActualINR + "   where `monthlyMMS`.`id`=" + model.Id;
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




        public int MonthlyMMSServiceExistence(MonthlyMMS model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `monthlyMMS` where month='" + model.Month + "' and `year`='" + model.Year + "'";
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


        public ActionResult GetMMSService(string ID)
        {
            int Id = Convert.ToInt16(ID);
            MonthlyMMS Model = new MonthlyMMS();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT id,month,year,budgeINR,ActualINR,`MMSbudgeINR`,`MMSActualINR`,`ONbudgeINR`,`ONActualINR`,Comments,CallbudgeINR,CallActualINR FROM `monthlyMMS` where `monthlyMMS`.id=" + Id;
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
                    if (!reader.IsDBNull(4))
                    {

                        Model.ActualINR = Convert.ToDouble(reader.GetDouble(4));
                        Model.MMSbudgeINR = Convert.ToDouble(reader.GetDouble(5));
                        Model.MMSActualINR = Convert.ToDouble(reader.GetDouble(6));
                        Model.ONbudgeINR = Convert.ToDouble(reader.GetDouble(7));
                        Model.ONActualINR = Convert.ToDouble(reader.GetDouble(8));
                    }
                    if (!reader.IsDBNull(9))
                    {
                        Model.Comments = reader.GetString(9);
                    }
                    if (!reader.IsDBNull(10))
                    {
                        Model.CallbudgeINR = Convert.ToDouble(reader.GetDouble(10));
                        Model.CallActualINR = Convert.ToDouble(reader.GetDouble(11));
                    }

                }



            }

            return PartialView("/Views/MMS/_AddMonthlyMMS.cshtml", Model);
        }

        public ActionResult DeleteMMMservice(string ID)
        {
            try
            {
                DeleteMMSserviceDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("MonthlyMMSRevenue");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";

                return RedirectToAction("MonthlyMMSRevenue");
            }
        }


        public string DeleteMMSserviceDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "delete from `monthlyMMS` where `monthlyMMS`.id=" + Id;
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

            string backlogcommand = "select  sum(ActualINR)-  sum(budgeINR) as backlog from monthlyMMS where date >='" + dfromdate + "' and date <='" + denddate + "'";

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







            Command = "select concat ( month,year) as monthyear, budgeINR as BudgetINR,ActualINR,CEIL((ActualINR/budgeINR)*100) as Percent  from `monthlyMMS` where `monthlyMMS`.month='" + month + "' and `monthlyMMS`.year=" + year + "";

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
            return Json(dataTable.DataTableToList<chartMMS>(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult BarMMSChart(string month, string year)
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

            string backlogcommand = "select  sum(MMSActualINR)-  sum(MMSbudgeINR) as backlog from monthlyMMS where date >='" + dfromdate + "' and date <='" + denddate + "'";

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






            Command = "select concat ( month,year) as monthyear, `MMSbudgeINR` as BudgetINR,`MMSActualINR` as ActualINR,CEIL((MMSActualINR/MMSbudgeINR)*100) as Percent  from `monthlyMMS` where `monthlyMMS`.month='" + month + "' and  `monthlyMMS`.year=" + year + "";

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
            return Json(dataTable.DataTableToList<chartMMS>(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult BarBPOChart(string month, string year)
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

            string backlogcommand = "select  sum(ONActualINR)-  sum(ONbudgeINR) as backlog from monthlyMMS where date >='" + dfromdate + "' and date <='" + denddate + "'";

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






            Command = "select concat ( month,year) as monthyear, `ONbudgeINR` as BudgetINR,`ONActualINR` as  ActualINR,CEIL((ONActualINR/ONbudgeINR)*100) as Percent  from `monthlyMMS` where `monthlyMMS`.month='" + month + "' and  `monthlyMMS`.year=" + year + "";

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
            return Json(dataTable.DataTableToList<chartMMS>(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult BarCallChart(string month, string year)
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

            string backlogcommand = "select  sum(CallActualINR)-  sum(CallbudgeINR) as backlog from monthlyMMS where date >='" + dfromdate + "' and date <='" + denddate + "'";

            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(backlogcommand))
                {
                    cmd.Connection = mConnection;
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // read
                    {
                        if (!reader.IsDBNull(0))
                        {
                            backlog = Convert.ToDecimal(reader["backlog"]);
                        }

                    }



                }
            }






            Command = "select concat ( month,year) as monthyear, `CallbudgeINR` as BudgetINR,`CallActualINR` as  ActualINR,CEIL((CallActualINR/CallbudgeINR)*100) as Percent  from `monthlyMMS` where `monthlyMMS`.month='" + month + "' and  `monthlyMMS`.year=" + year + "";

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
            return Json(dataTable.DataTableToList<chartMMS>(), JsonRequestBehavior.AllowGet);
        }



        public ActionResult Dashboard()
        {
            return View("DashboardDetails");
        }

        public ActionResult YearlyRevenueReport(string Year)
        {

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            string[] strArr = null;
            char[] splitchar = { '-' };
            strArr = Year.Split(splitchar);
            var Date1 = strArr[0].Trim().ToString() + "-04-01";
            var Date2 = strArr[1].Trim().ToString() + "-03-31";
            MonthlyMMS Model = new MonthlyMMS();



            Command = "select  month as Month,year as Year, budgeINR,ActualINR ,(ActualINR/budgeINR)*100 as Achievement  from monthlyMMS where date >='" + Date1 + "' and  date <='" + Date2 + "' ";

            Command = Command + "   order by year(date), month(date)";


            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }


            Model.LstMonthlyMMS = dt.DataTableToList<MonthlyMMS>();

            //var myResult = new
            //{
            //    Name = dt.DataTableToList<MonthlyMMS>(),
            //    Information = dt.DataTableToList<MonthlyMMS>()
            //};
            //return Json(new
            //{
            //    myResult,
            //    JsonRequestBehavior.AllowGet
            //});  



            //ViewBag.Daylist = "Summary of Target Vs Achievement -BPO Online and MMS  " + Year + " in INR";
            return PartialView("_DashboardList", Model);



        }

        public ActionResult YearlyETORevenueReport(string Year)
        {

            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = string.Empty;
            string[] strArr = null;
            char[] splitchar = { '-' };
            strArr = Year.Split(splitchar);
            var Date1 = strArr[0].Trim().ToString() + "-04-01";
            var Date2 = strArr[1].Trim().ToString() + "-03-31";
            MonthlyMMS Model = new MonthlyMMS();



            Command = "select concat ( month,year) as Month, budgeINR,ActualINR ,(ActualINR/budgeINR)*100 as Achievement  from monthlyMMS where date >='" + Date1 + "' and  date <='" + Date2 + "' ";

            Command = Command + "   order by year(date), month(date)";


            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);

            }


            Model.LstMonthlyMMS = dt.DataTableToList<MonthlyMMS>();

            //ViewBag.Daylist = "Summary of Target Vs Achievement -BPO Online and MMS  " + Year + " in USD"; ;
            return PartialView("_DashboardUSDList", Model);



        }


        public ActionResult YearlyChart(string Year)
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






            Command = "select concat ( month,year) as monthyear, `budgeINR` as Target, `ActualINR` as Achieved,ROUND((ActualINR/budgeINR)*100,0) as  Percentage  from `monthlyMMS` where date >='" + Date1 + "' and  date <='" + Date2 + "' ";

            Command = Command + "   order by year(date), month(date)";


            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dataTable);

            }


            //dataTable.Columns.Remove(dataTable.Columns[3]);
            return Json(dataTable.DataTableToList<chartYearly>(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult FillTargetMonthly(string month, string year)
        {



            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;



            double budgeINR = 0.0;
            double mmstarget = 0.0;
            double bpotarget = 0.0;
            double calltarget = 0.0;

            string Command = "SELECT `target`,`mmstarget`,`bpotarget`,`calltarget` from  `monthlyMMSTarget` where `month`='" + month + "' and year='" + year + "'";
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
                        if (!reader.IsDBNull(1))
                        {
                            mmstarget = reader.GetDouble("mmstarget");
                        }
                        if (!reader.IsDBNull(2))
                        {
                            bpotarget = reader.GetDouble("bpotarget");
                        }
                        if (!reader.IsDBNull(3))
                        {
                            calltarget = reader.GetDouble("calltarget");
                        }
                    }
                }
            }


            var result = new { budgeINR = budgeINR, mmstarget = mmstarget, bpotarget = bpotarget, calltarget = calltarget };
            return Json(result, JsonRequestBehavior.AllowGet);


        }


        #endregion

        #region MMSAdditionalInfo

        public ActionResult AdditionalServices()
        {
            MMSAdditional Model = new MMSAdditional();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT AddInfo_id as Id,  `MMS_AddInfo`.Month, `MMS_AddInfo`.Year,Team_Size as TeamSize, Target,(Resigned/`Team_Size`)*100 as Actual from `MMS_AddInfo`";
            DataTable dt = new DataTable();
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);
                Model.LstMMSAdditional = dt.DataTableToList<MMSAdditional>();
                return View("AdditionalServices", Model);
            }
        }

        public ActionResult GetAdditionalservice(string ID)
        {
            int Id = Convert.ToInt16(ID);
            MMSAdditional Model = new MMSAdditional();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT AddInfo_id,Attrition,Month,Year, Team_Size,Resigned,Size,Billable,NonBillable,MMSsize,MMSBillable,MMSNonBillable,Target,MMSComments,BPOComments,`CallComments`,`CallSize`,`CallBillable`,`CallNonBillable`,`CallETOINR`,`CallETOUSD` from `MMS_AddInfo` where MMS_AddInfo.AddInfo_id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.Attrition = reader.GetString(1);
                    string month = reader.GetString(2);
                    string year = reader.GetString(3);
                    Model.TeamSize = reader.GetInt32(4);
                    Model.Resigned = reader.GetInt32(5);
                    Model.Size = reader.GetInt32(6);
                    Model.Billable = Convert.ToDouble(reader.GetDouble(7));
                    Model.NonBillable = Convert.ToDouble(reader.GetDouble(8));
                    Model.MMSSize = reader.GetInt32(9);
                    Model.MMSBillable = Convert.ToDouble(reader.GetDouble(10));
                    Model.MMSNonBillable = Convert.ToDouble(reader.GetDouble(11));
                    Model.Target = Convert.ToDouble(reader.GetDouble(12));

                    if (!reader.IsDBNull(13))
                    {
                        Model.MMSComments = reader.GetString(13);
                    }
                    if (!reader.IsDBNull(14))
                    {
                        Model.BPOComments = reader.GetString(14);
                    }
                    if (!reader.IsDBNull(15))
                    {
                        Model.CallComments = reader.GetString(15);
                    }
                    if (!reader.IsDBNull(16))
                    {
                        Model.CallSize = reader.GetInt32(16);
                    }
                    if (!reader.IsDBNull(17))
                    {
                        Model.CallBillable = Convert.ToDouble(reader.GetDouble(17));
                    }
                    if (!reader.IsDBNull(18))
                    {
                        Model.CallNonBillable = Convert.ToDouble(reader.GetDouble(18));
                    }
                    if (!reader.IsDBNull(19))
                    {
                        Model.CallETOINR = Convert.ToDouble(reader.GetDouble(19));
                    }
                    if (!reader.IsDBNull(20))
                    {
                        Model.CallETOUSD = Convert.ToDouble(reader.GetDouble(20));
                    }



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

            return PartialView("/Views/MMS/_AddAdditionalServices.cshtml", Model);
        }
        public ActionResult GetAdditionalViewDetails(string ID)
        {
            int Id = Convert.ToInt16(ID);
            MMSAdditional Model = new MMSAdditional();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT AddInfo_id,Attrition,`MMS_AddInfo`.Month,`MMS_AddInfo`.Year, Team_Size, Resigned,Size,Billable,NonBillable,MMSsize,MMSBillable,MMSNonBillable,ROUND(`monthlyMMS`.`MMSActualINR`/`MMSsize`,0) as MMSETOINR, ROUND(`monthlyMMS`.`MMSActualINR`/MMSsize/72,0) as  MMSETOUSD,ROUND(`monthlyMMS`.`ONActualINR`/`Size`,0) as BPOETOINR, ROUND(`monthlyMMS`.`ONActualINR`/Size/72,0) as  BPOETOUSD,  Target,ROUND((Resigned/`Team_Size`)*100,0) as Actual,MMSComments,BPOComments,ROUND((monthlyMMS.`ActualINR`/Team_Size),0) as ETO ,CallComments,CallSize,CallBillable,CallNonBillable,ROUND(`monthlyMMS`.`CallActualINR`/`Size`,0) as callETOINR, ROUND(`monthlyMMS`.`CallActualINR`/Size/72,0) as  CallETOUSD from `MMS_AddInfo` LEFT JOIN `monthlyMMS` ON `MMS_AddInfo`.Month=`monthlyMMS`.month and  `MMS_AddInfo`.Year=`monthlyMMS`.year  where     MMS_AddInfo.AddInfo_id=" + Id;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Model.Id = reader.GetInt32(0);
                    Model.Attrition = reader.GetString(1);
                    Model.Month = reader.GetString(2);
                    Model.Year = reader.GetString(3);
                    Model.TeamSize = reader.GetInt32(4);
                    Model.Resigned = reader.GetInt32(5);
                    Model.Size = reader.GetInt32(6);
                    Model.Billable = Convert.ToDouble(reader.GetDouble(7));
                    Model.NonBillable = Convert.ToDouble(reader.GetDouble(8));
                    Model.MMSSize = reader.GetInt32(9);
                    Model.MMSBillable = Convert.ToDouble(reader.GetDouble(10));
                    Model.MMSNonBillable = Convert.ToDouble(reader.GetDouble(11));

                    if (!reader.IsDBNull(12))
                    {
                        Model.MMSETOINR = Convert.ToDouble(reader.GetDouble(12));
                        Model.MMSETOUSD = Convert.ToDouble(reader.GetDouble(13));
                        Model.BPOETOINR = Convert.ToDouble(reader.GetDouble(14));
                        Model.BPOETOUSD = Convert.ToDouble(reader.GetDouble(15));

                    }

                    if (!reader.IsDBNull(16))
                    {
                        Model.Target = Convert.ToDouble(reader.GetDouble(16));
                        Model.Actual = Convert.ToDouble(reader.GetDouble(17));
                    }

                    if (!reader.IsDBNull(18))
                    {
                        Model.MMSComments = reader.GetString(18);
                    }
                    if (!reader.IsDBNull(19))
                    {
                        Model.BPOComments = reader.GetString(19);
                    }
                    if (!reader.IsDBNull(20))
                    {
                        Model.ETO = Convert.ToDouble(reader.GetDouble(20));
                    }
                    if (!reader.IsDBNull(21))
                    {
                        Model.CallComments = reader.GetString(21);
                    }
                    if (!reader.IsDBNull(22))
                    {
                        Model.CallSize = reader.GetInt32(22);
                    }

                    if (!reader.IsDBNull(23))
                    {
                        Model.CallBillable = reader.GetInt32(23);
                    }
                    if (!reader.IsDBNull(24))
                    {
                        Model.CallNonBillable = reader.GetInt32(24);
                    }
                    if (!reader.IsDBNull(25))
                    {
                        Model.CallETOINR = Convert.ToDouble(reader.GetDouble(25));
                    }

                    if (!reader.IsDBNull(26))
                    {
                        Model.CallETOUSD = Convert.ToDouble(reader.GetDouble(26));
                    }

                }

            }

            return PartialView("/Views/MMS/_ViewAdditionalservice.cshtml", Model);
        }
        public ActionResult AddAdditionalServices()
        {
            MMSAdditional model = new MMSAdditional();

            return PartialView("_AddAdditionalServices", model);

        }
        public string ManageAdditionalServices(MMSAdditional model)
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



            string Command = string.Empty;
            if (model.Id == 0)
            {
                Command = "INSERT INTO `MMS_AddInfo`(`Attrition`,`Month`,`Year`, `Team_Size`,`Resigned`,`Size`,`Billable`,`NonBillable`,`MMSsize`,`MMSBillable`,`MMSNonBillable`,Target,MMSComments,BPOComments,CallSize,CallBillable,CallNonBillable,CallComments) VALUES ('" + model.Attrition + "','" + model.Month + "','" + model.Year + "'," + model.TeamSize + " ," + model.Resigned + "," + model.Size + "," + model.Billable + "," + model.NonBillable + "," + model.MMSSize + "," + model.MMSBillable + "," + model.MMSNonBillable + "," + model.Target + ",'" + model.MMSComments + "','" + model.BPOComments + "'," + model.CallSize + "," + model.CallBillable + "," + model.CallNonBillable + ",'" + model.CallComments + "')";
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

                Command = "UPDATE MMS_AddInfo set `Attrition`='" + model.Attrition + "', `Month`='" + model.Month + "',Year='" + model.Year + "',Team_Size=" + model.TeamSize + ", Resigned=" + model.Resigned + ",Size=" + model.Size + ", Billable =" + model.Billable + ",NonBillable='" + model.NonBillable + "',MMSsize=" + model.MMSSize + ",MMSBillable=" + model.MMSBillable + ", MMSNonBillable='" + model.MMSNonBillable + "',Target=" + model.Target + ",MMSComments='" + model.MMSComments + "',BPOComments='" + model.BPOComments + "',CallComments='" + model.CallComments + "',CallSize=" + model.CallSize + ",CallBillable=" + model.CallBillable + ",CallNonBillable=" + model.CallNonBillable + "  where MMS_AddInfo.AddInfo_id=" + model.Id;
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
        public ActionResult SaveAdditionalService(MMSAdditional model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    int insertresult = AdditionalServiceExistence(model);
                    if (insertresult == 0)
                    {
                        string Result = ManageAdditionalServices(model);
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
                    string Result = ManageAdditionalServices(model);
                    if (Result.Trim('"') == "Ok")
                        TempData["Msg"] = "Successfully Saved!";
                    else
                        TempData["Msg"] = "Unsuccessfull Operation!";
                }



            }
            return RedirectToAction("AdditionalServices");

        }
        public int AdditionalServiceExistence(MMSAdditional model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `MMS_AddInfo` where Month='" + model.Month + "' and `Year`='" + model.Year + "'";
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


        public string DeleteAdditionalServiceDetails(string ID)
        {
            int Id = int.Parse(ID);
            string Result = "0";
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = mConnection;
                cmd.CommandText = "delete from `MMS_AddInfo` where `MMS_AddInfo`.AddInfo_id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }
        public ActionResult DeleteAdditionalService(string ID)
        {
            try
            {
                DeleteAdditionalServiceDetails(ID);
                TempData["Msg"] = "Successfully Deleted";
                return RedirectToAction("AdditionalServices");
            }
            catch (Exception)
            {

                TempData["Msg"] = "Unsuccessfull Operation!";

                return RedirectToAction("AdditionalServices");
            }
        }

        #endregion



        #region Revenueplan


        public ActionResult MonthlyRevenueplan()
        {
            MonthlyMMSTarget Model = new MonthlyMMSTarget();
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT  id as Id ,month,year,target,mmstarget,bpotarget,calltarget from `monthlyMMSTarget`";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                adapter.Fill(dt);
                Model.LstMonthlyMMSTarget = dt.DataTableToList<MonthlyMMSTarget>();
                return View("MonthlyRevenuePlan", Model);
            }


        }

        public ActionResult AddRevenueplan()
        {
            MonthlyMMSTarget model = new MonthlyMMSTarget();

            return PartialView("_AddRevenueplan", model);

        }

        public ActionResult SaveRevenueplan(MonthlyMMSTarget model)
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

        public string ManageRevenueplan(MonthlyMMSTarget model)
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
                Command = "INSERT INTO `monthlyMMSTarget`(`month`,`year`, `target`,`mmstarget`,`bpotarget`,calltarget) VALUES ('" + model.Month + "','" + model.Year + "'," + model.target + "," + model.mmstarget + " ," + model.bpotarget + "," + model.calltarget + ");";
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

                Command = "UPDATE monthlyMMSTarget set `target`=" + model.target + ", `mmstarget`=" + model.mmstarget + ",`Bpotarget`=" + model.bpotarget + " ,calltarget=" + model.calltarget + ",  month='" + model.Month + "',Year='" + model.Year + "' where `monthlyMMSTarget`.`id`=" + model.Id;
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




        public int RevenueplanExistence(MonthlyMMSTarget model)
        {
            int Result = 0;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT count(*) as cnt FROM `monthlyMMS` where month='" + model.Month + "' and `year`='" + model.Year + "'";
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
            MonthlyMMSTarget Model = new MonthlyMMSTarget();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT id,month,year,target,mmstarget,BPOtarget,calltarget FROM `monthlyMMSTarget` where `monthlyMMSTarget`.id=" + Id;
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
                    if (!reader.IsDBNull(4))
                    {

                        Model.mmstarget = Convert.ToDouble(reader.GetDouble(4));
                        Model.bpotarget = Convert.ToDouble(reader.GetDouble(5));

                    }
                    if (!reader.IsDBNull(6))
                    {
                        Model.calltarget = Convert.ToDouble(reader.GetDouble(6));
                    }
                }

            }

            return PartialView("/Views/MMS/_AddRevenueplan.cshtml", Model);
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
                cmd.CommandText = "delete from `monthlyMMSTarget` where `monthlyMMSTarget`.id=" + Id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Result = "1";
            }
            return Result;
        }





        #endregion


    }
}
