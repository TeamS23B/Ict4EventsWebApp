using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ict4EventsWebApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((bool) (Session["loggedIn"]??false))//logout if the user is already logged in
            {
                lblError.Text = "Uitgelogd";
                Session.Clear();
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            if (Authentication.Instance.IsAuthenticated(tbUsername.Text, tbPassword.Text))
            {
                Session["loggedIn"] = true;
                Session["username"] = tbUsername.Text;
                Session["token"] = SmsConnect.Instance.AddToken(tbUsername.Text);
                if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                {

// ReSharper disable once AssignNullToNotNullAttribute
                    Response.Redirect(Server.UrlDecode(Request.QueryString["returnUrl"]));//return to returnUrl with server.UrlDecode
                }
                else
                {
                    Response.Redirect("/index.html");//return to index.html
                }
            }
            else
            {
                lblError.Text = "Could not log in user, username or password incorrect!";
            }
        }
    }
}