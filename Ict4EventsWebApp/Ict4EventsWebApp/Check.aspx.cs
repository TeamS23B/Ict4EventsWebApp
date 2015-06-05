using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace Ict4EventsWebApp
{
    public partial class Check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            if (Authentication.Instance.IsAuthenticated(tbUsername.Text, tbPassword.Text))
            {
                Session["LoggedIn"] = true;
                Session["Username"] = tbUsername.Text;
                Session["Permissions"] = 0;
                using (var dbcon = OracleClientFactory.Instance.CreateConnection())
                {
                    dbcon.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    dbcon.Open();
                    var com = OracleClientFactory.Instance.CreateCommand();
                    com.Connection = dbcon;
                    com.CommandText = "SELECT Permissie FROM account WHERE gebruikersnaam = :1";
                    var param = com.CreateParameter();
                    param.DbType = DbType.AnsiString;
                    param.Direction= ParameterDirection.Input;
                    param.ParameterName = "Gebruikersnaam";
                    param.Value = tbUsername.Text;
                    com.Parameters.Add(param);
                    Session["Permissions"] = (int)com.ExecuteScalar();
                    com.Dispose();
                }
            }
            else
            {
                lblUsername.Text = "Kon niet inloggen!";
            }
        }
    }
}