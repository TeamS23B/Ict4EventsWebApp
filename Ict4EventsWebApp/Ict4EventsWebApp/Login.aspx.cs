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
                SmsConnect.Instance.RemoveToken((string)Session["username"]);
                lblError.Text = "Uitgelogd";
                Session.Clear();
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (Authentication.Instance.IsAuthenticated(tbUsername.Text, tbPassword.Text))
                {
                    Session["loggedIn"] = true;
                    if (lblUsername.Text == "Administrator")
                    {
                        Session["username"] = "admin";
                    }
                    else
                    {
                        Session["username"] = tbUsername.Text;    
                    }
                    
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
            catch (Exception)
            {
                //if there is an exception then the user can't login
                lblError.Text = "Could not log in user, username or password incorrect!<Exception>";
            }
            
        }
    }
}