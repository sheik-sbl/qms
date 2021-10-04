using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using BPOAttendanceProject.Models;
using Newtonsoft.Json.Linq;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Transactions;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace BPOAttendanceProject.Controllers
{
    //[Authorize]
    //[InitializeSimpleMembership]
    //[UserFilter]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {

            return View("LoginPage");
        }

        //
        // POST: /Account/Login    
        //
        // POST: /Account/LogOff

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LogOff()
        //{
        //    WebSecurity.Logout();

        //    return RedirectToAction("Index", "Home");
        //}

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AllowAnonymous]
        public ActionResult LogOff()
        {

            Session.Abandon();
            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();
            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Login", "Home");


            //HttpCookie cookie = new HttpCookie("AuthToken");
            //cookie.Expires = DateTime.Now.AddDays(-1d);
            //Response.Cookies.Add(cookie);
            //Session.Abandon();
            ////WebSecurity.Logout();


        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Disassociate(string provider, string providerUserId)
        //{
        //    string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
        //    ManageMessageId? message = null;

        //    // Only disassociate the account if the currently logged in user is the owner
        //    if (ownerAccount == User.Identity.Name)
        //    {
        //        // Use a transaction to prevent the user from deleting their last login credential
        //        using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
        //        {
        //            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //            if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
        //            {
        //                OAuthWebSecurity.DeleteAccount(provider, providerUserId);
        //                scope.Complete();
        //                message = ManageMessageId.RemoveLoginSuccess;
        //            }
        //        }
        //    }

        //    return RedirectToAction("Manage", new { Message = message });
        //}

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {

            return View("ChangePassword");


        }

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult UserLogin(LoginModel model, string returnUrl)
        //{
        //    try
        //    {



        //        bool gt = ReadData(model.username, model.password);

        //        if (gt == true)
        //        {

        //            return RedirectToAction("Index", "Home");

        //        }

        //        else
        //        {

        //            // ViewBag.LoginStatus = gt;
        //            ViewBag.LoginStatus = "Sorry! Login credential is not valid.";
        //            return View("LoginPage");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.LoginStatus = ex;
        //        return View("LoginPage");
        //    }                
        //    }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UserLogin(LoginModel model, string returnUrl)
        {
            try
            {
                string username = model.username;
                string password = model.password;
                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;


                //string Command = "SELECT muser.Id,muser.UserName,muser.Password,muser.Roleid, CONCAT(muser.FirstName,' ',muser.LastName) as DisplayName FROM muser WHERE  muser.Status=0 and muser.isactive=true and  muser.UserName='" + username + "' AND muser.Password='" + password + "'";
                string Command = string.Empty;
                if (username == "admin")
                    Command = "SELECT muser.Id,muser.UserName,muser.Password,muser.Roleid, CONCAT(muser.FirstName,' ',muser.LastName) as DisplayName,'admin' as location FROM muser WHERE  muser.Status=0 and muser.isactive=true and  muser.UserName='" + username + "' AND muser.Password='" + password + "'";
                else if (username == "1202" || username == "1250")
                    Command = "SELECT muser.Id,muser.UserName,muser.Password,muser.Roleid, CONCAT(muser.FirstName,' ',muser.LastName) as DisplayName, location FROM muser WHERE  muser.Status=0 and muser.isactive=true and  muser.UserName='" + username + "' AND muser.Password='" + password + "'";
                else
                    Command = "SELECT muser.Id,muser.UserName,muser.Password,muser.Roleid, CONCAT(muser.FirstName,' ',COALESCE(muser.LastName,'')) as DisplayName,  location FROM muser WHERE  muser.Status=0 and muser.isactive=true and  muser.UserName='" + username + "' AND muser.Password='" + password + "'";

                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Userlogin usr = new Userlogin();
                        while (reader.Read())
                        {

                            usr.Username = reader.GetString("UserName");
                            usr.Location = reader.GetString("Location");
                            usr.Roleid = int.Parse(reader.GetString("Roleid"));
                            usr.Id = int.Parse(reader.GetString("Id"));

                            HttpCookie myCookieobj = new HttpCookie("location");
                            myCookieobj.Value = usr.Location.ToString();

                            Session["UserName"] = reader.GetString("UserName");
                            Session["location"] = reader.GetString("Location");
                            Session["DisplayName"] = reader.GetString("DisplayName");
                            // Session["RoleId"] = reader.GetString("RoleId");
                            System.Web.HttpContext.Current.Session["RoleId"] = reader.GetString("RoleId");
                            Session["UserId"] = reader.GetString("Id");

                            var serializer = new JavaScriptSerializer();
                            string userData = serializer.Serialize(usr);

                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, reader.GetString("UserName"), DateTime.Now, DateTime.Now.AddDays(30), true, userData, FormsAuthentication.FormsCookiePath);

                            // Encrypt the ticket.
                            string encTicket = FormsAuthentication.Encrypt(ticket);

                            // Create the cookie.
                            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.LoginStatus = "Sorry! Login credential is not valid.";
                        return View("LoginPage");
                    }
                }
            }
            catch (Exception ex)
            {

                //ViewBag.LoginStatus ="test";
                // ViewBag.LoginStatus = ex;
                ViewBag.LoginStatus = "Login issue";

                return View("LoginPage");
            }
        }

        //public string SaveChangepassword(PasswordModel model)
        //{
        //    string PassMessage = "";
        //    int userId;

        //    User Model = new User();
        //    int.TryParse(Session["Userid"].ToString(), out userId);
        //    Model.Id = userId;
        //    string ActionParam = "SELECT";

        //    //string Result = CommonHelper.GetSyncData("GetCommonMasters/" + userId.ToString() + "/" + ActionParam + "/" + "GetUser", ConfigurationManager.AppSettings["UserManageServiceUrl"].ToString());
        //    //if (Result != "" && Result != "[]")
        //    //{

        //    //    Model = JArray.Parse(Result).First.ToObject<User>();
        //    //}

        //    string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
        //    string Command = "SELECT muser.Id ,muser.Username,muser.password,muser.Firstname,muser.LastName,muser.EmailId from muser where muser.Id=" + userId;
        //    using (MySqlConnection mConnection = new MySqlConnection(connString))
        //    {
        //        mConnection.Open();
        //        MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
        //        DataSet dataSet = new DataSet();
        //        dataSet.Tables.Add(new DataTable());
        //        adapter.Fill(dataSet.Tables[0]);
        //        DataTable dtt = dataSet.Tables[0];
        //        Model = dtt.DataTableToList<User>().First();

        //    }

        //    if (model.OldPassword == Model.Password)
        //    {
        //        Model.Password = CommonHelper.SHA1HashStringForUTF8String(model.NewPassword);

        //        String UpdateResult = CommonHelper.post("UpdateUserPassword", ConfigurationManager.AppSettings["UserManageServiceUrl"].ToString(), JsonConvert.SerializeObject(Model));
        //        PassMessage = "Password changed successfuly";
        //    }
        //    else
        //    {
        //        PassMessage = "Old Password doesn't match";
        //    }
        //    return PassMessage.ToString();

        //}

        [AllowAnonymous]
        public string SaveChangepassword(PasswordModel model)
        {
            string PassMessage = "";
            int userId;

            User Model = new User();
            int.TryParse(Session["Userid"].ToString(), out userId);
            Model.Id = userId;
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT muser.Id ,muser.Username,muser.password,muser.Firstname,muser.LastName,muser.EmailId from muser where muser.Id=" + userId;
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(new DataTable());
                adapter.Fill(dataSet.Tables[0]);
                DataTable dtt = dataSet.Tables[0];
                Model = dtt.DataTableToList<User>().First();

            }

            if (model.OldPassword == Model.Password)
            {
                string pass = model.NewPassword;

                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = mConnection;
                    cmd.CommandText = "update muser set password='" + pass + "' where muser.Id=" + userId;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }

                PassMessage = "Password changed successfuly";
            }
            else
            {
                PassMessage = "Old Password doesn't match";
            }
            return PassMessage.ToString();

        }

        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetUserPassword(LoginModel model)
        {
            LoginModel Model = new LoginModel();
            string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
            string Command = "SELECT muser.Id,muser.UserName from muser WHERE   muser.EmailId='" + model.EmailId + "'";
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                mConnection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command, mConnection);
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(new DataTable());
                adapter.Fill(dataSet.Tables[0]);
                DataTable dtt = dataSet.Tables[0];
                if (dtt.Rows.Count > 0)
                {
                    Model = dtt.DataTableToList<LoginModel>().First();
                }
                //else
                //{

                //}

            }

            if (Model.Id != 0 && Model.Id.ToString() != string.Empty)
            {

                //var guid = Guid.NewGuid();
                string EncPassword = CreateRandomPassword(8);

                string subject = "Your changed password";

                string body = "<b>Please find the  Reset Password . </b><br/>" + EncPassword;

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = new MailAddress("mistool@sblinfo.org ");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(model.EmailId));
                model.password = EncPassword;
                model.Id = Model.Id;

                SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;

                smtp.Host = "relay-hosting.secureserver.net";
                smtp.Port = 25;
                smtp.EnableSsl = false;

                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = mail.From.Address;
                NetworkCred.Password = "x@VDl12639d6";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;

                smtp.Send(mail);

                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    mConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = mConnection;
                    cmd.CommandText = "update muser set password='" + EncPassword + "' where muser.Id=" + model.Id;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }

            }

            return View("LoginPage", model);

        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    Exception ex = filterContext.Exception;
        //    filterContext.ExceptionHandled = true;

        //    var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

        //    filterContext.Result = new ViewResult()
        //    {
        //        ViewName = "Error",
        //        ViewData = new ViewDataDictionary(model)
        //    };

        //}

        public bool ReadData(string username, string password)
        {
            try
            {

                string connString = ConfigurationManager.ConnectionStrings["MySQLConnString"].ConnectionString;
                string Command = "SELECT muser.Id,muser.UserName,muser.Password,muser.Roleid, CONCAT(muser.FirstName,' ',COALESCE(muser.LastName,'')) as DisplayName FROM muser WHERE  muser.Status=0 and muser.UserName='" + username + "' AND muser.Password='" + password + "'";
                using (MySqlConnection mConnection = new MySqlConnection(connString))
                {
                    MySqlCommand cmd = new MySqlCommand(Command, mConnection);
                    mConnection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Session["UserName"] = reader.GetString("UserName");
                            Session["DisplayName"] = reader.GetString("DisplayName");
                            // Session["RoleId"] = reader.GetString("RoleId");
                            System.Web.HttpContext.Current.Session["RoleId"] = reader.GetString("RoleId");
                            Session["UserId"] = reader.GetString("Id");
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
