using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ict4EventsWebApp
{
    public partial class ActivateAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["username"]) || string.IsNullOrEmpty(Request.QueryString["hash"]))
            {
                //disable form if parmeters arn't provided
                ShowError("Geen gebruikersnaam of wachtwoord!");
            }
            //check if parameters are correct
            using (var con = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection())
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                using (var com = con.CreateCommand())
                {
                    //check if the user can activate
                    com.CommandText = "SELECT COUNT(*) " +
                                      "FROM account " +
                                      "WHERE gebruikersnaam = :usrn " +
                                      "AND activatiehash = :pHash " +
                                      "AND geactiveerd = 0";
                    var pUsrn = com.CreateParameter();
                    pUsrn.DbType = DbType.String;
                    pUsrn.Direction = ParameterDirection.Input;
                    pUsrn.ParameterName = "usrn";
                    pUsrn.Value = Request.QueryString["username"];

                    var pHash = com.CreateParameter();
                    pHash.DbType = DbType.String;
                    pHash.Direction = ParameterDirection.Input;
                    pHash.ParameterName = "pHash";
                    pHash.Value = Request.QueryString["hash"];

                    com.Parameters.Add(pUsrn);
                    com.Parameters.Add(pHash);

                    if(Convert.ToInt32(com.ExecuteScalar())<1)
                    {
                        ShowError("Onbekende gebruikersnaam, hash of de gebruiker is al geactiveerd");
                    }
                    //user may activate
                }
            }
        }

        private void ShowError(string message,bool disable=true)
        {
            lblError.Text = message+"<br/>";
            newPassword.Enabled = !disable;
            confirmPassword.Enabled = !disable;
            setPassword.Enabled = !disable;
        }

        protected void setPassword_Click(object sender, EventArgs e)
        {
            if (newPassword.Text != confirmPassword.Text)
            {
                ShowError("Passwords arn't the same!");
                return;
            }
            //check basic password strenght
            var pass = newPassword.Text;
            if (!Regex.IsMatch(pass,@"^(?=.*\d).{4,14}$"))
            {
                ShowError("Passwords doesn't match criteria!<br/>4-14 chars<br/>1 digit",false);
                return;
            }
            if(!EnableAccount())
                return;
            Authentication.Instance.AddUser(Request.QueryString["username"],pass);
            Response.Redirect("index.aspx");
        }

        private bool EnableAccount()
        {
            using (var con = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection())
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = "UPDATE account SET geactiveerd = 1 WHERE gebruikersnaam = :usrn";
                    var pUsrn = com.CreateParameter();
                    pUsrn.DbType=DbType.String;
                    pUsrn.Direction=ParameterDirection.Input;
                    pUsrn.ParameterName = "usrn";
                    pUsrn.Value = Request.QueryString["username"];
                    com.Parameters.Add(pUsrn);

                    if (com.ExecuteNonQuery() < 1)
                    {
                        lblError.Text = "Coulndn't activate account!";
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}