using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Ict4EventsWebApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
			GlobalConfiguration.Configure(WebApiConfig.Register);    
        }

        protected void Session_OnEnd()
        {
            if (!(bool)(Session["loggedIn"] ?? false))
                SmsConnect.Instance.RemoveToken((string)Session["username"]);//remove the token if the user is logged in
        }
    }
}