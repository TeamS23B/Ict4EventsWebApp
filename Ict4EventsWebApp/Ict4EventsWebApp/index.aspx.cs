using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ict4EventsWebApp
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if the useer isn't login id then redirect to the login page
            if(!(bool)(Session["loggedIn"]??false))
            {
                Response.Redirect("Login.aspx?returnUrl="+Server.UrlEncode(Request.Url.ToString()));
            }
        }
    }
}