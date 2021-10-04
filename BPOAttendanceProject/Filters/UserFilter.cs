using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BPOAttendanceProject.Filters
{
    
        public class UserFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                //if (this.IsAnonymousAction(filterContext))
                //{
                //    return;
                //}

                if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                {
                    HttpSessionStateBase session = filterContext.HttpContext.Session;
                    var user = session["Userid"];

                    if (((user == null) && (!session.IsNewSession)) || (session.IsNewSession))
                    {
                        //send them off to the login page
                        var url = new UrlHelper(filterContext.RequestContext);
                        var loginUrl = url.Content("~/Error/SessionTimedOut");

                        filterContext.HttpContext.Response.Redirect(loginUrl, true);
                    }
                }

            }
        }
    }
