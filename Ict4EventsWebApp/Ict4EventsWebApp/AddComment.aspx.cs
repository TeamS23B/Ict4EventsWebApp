using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ict4EventsWebApp
{
    public partial class AddComment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(bool) (Session["loggedIn"] ?? false))
            {
                Response.Redirect("index.aspx");//to index and to login
            }
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                lblError.Text = "No ID!";
                send.Enabled = false;
            }
        }

        protected void send_Click(object sender, EventArgs e)
        {
            if (rbFile.Checked)
            {
                lblError.Text = "WIP";
                return;
            }
            else if (rbText.Checked)
            {
                //check if data is correct
                if (tbText.Text.Length < 10 || tbTitle.Text.Length < 3)
                {
                    lblError.Text = "Not enough characters!";
                    return;//not enough text
                }

                //insert a new post
                using (var con = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection())
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandText = "INSERT ALL " +
                                          "INTO bijdrage VALUES (null,(SELECT id FROM account WHERE gebruikersnaam = :usrn), SYSDATE, 'bericht') " +
                                          "INTO bericht VALUES ((SELECT max(id) FROM bijdrage),:pTitle,:pText) " +
                                          "INTO BIJDRAGE_BERICHT VALUES (:catid, (SELECT max(id) FROM bijdrage)) " +
                                          "SELECT * FROM DUAL";
                        var pUsrn = com.CreateParameter();
                        pUsrn.DbType = DbType.String;
                        pUsrn.Direction = ParameterDirection.Input;
                        pUsrn.ParameterName = "usrn";
                        pUsrn.Value = "admin";//tijdelijk Session["username"];

                        var pTitle = com.CreateParameter();
                        pTitle.DbType=DbType.String;
                        pTitle.Direction=ParameterDirection.Input;
                        pTitle.ParameterName = "pTitle";
                        pTitle.Value = tbTitle.Text;

                        var pText = com.CreateParameter();
                        pText.DbType = DbType.String;
                        pText.Direction = ParameterDirection.Input;
                        pText.ParameterName = "pTitle";
                        pText.Value = tbText.Text;

                        var pCat = com.CreateParameter();
                        pCat.DbType = DbType.Int32;
                        pCat.Direction = ParameterDirection.Input;
                        pCat.ParameterName = "catid";
                        pCat.Value = Convert.ToInt32(Request.QueryString["id"]);


                        com.Parameters.Add(pUsrn);
                        com.Parameters.Add(pTitle);
                        com.Parameters.Add(pText);
                        com.Parameters.Add(pCat);

                        if (com.ExecuteNonQuery() < 1)
                        {
                            lblError.Text = "Somthing went wrong!";
                        }
                        else
                        {
                            Response.Redirect("index.aspx?id="+Request.QueryString["id"]);
                        }
                    }
                }
            }
            else
            {
                lblError.Text = "Nothing selected!";
                //error
            }
        }
    }
}