using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
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
                var filename = string.Format(Request.MapPath("/upload")+"/{0}/{1}", Request.QueryString["id"], uploadFl.FileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filename));

                //uplod to /upload/{id}/{filename}
                var fs = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);

                fs.Write(uploadFl.FileBytes,0,uploadFl.FileBytes.Count());//upload the file to the harddisk

                //insert into the database
                using (var con = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection())
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandText =
                            "INSERT INTO bijdrage VALUES (null,(SELECT id FROM account WHERE gebruikersnaam = :usrn), SYSDATE, 'bestand')";

                        var pUsrn = com.CreateParameter();
                        pUsrn.DbType = DbType.String;
                        pUsrn.Direction = ParameterDirection.Input;
                        pUsrn.ParameterName = "usrn";
                        pUsrn.Value = Session["username"];

                        com.Parameters.Add(pUsrn);

                        com.ExecuteNonQuery();

                        com.Parameters.Clear();
                        com.CommandText = "INSERT INTO bestand VALUES ((SELECT max(id) FROM bijdrage),:catid, :pLoc, :pSize)";

                        var pCat = com.CreateParameter();
                        pCat.DbType = DbType.Int32;
                        pCat.Direction = ParameterDirection.Input;
                        pCat.ParameterName = "catid";
                        pCat.Value = Convert.ToInt32(Request.QueryString["id"]);

                        var pLoc = com.CreateParameter();
                        pLoc.DbType = DbType.String;
                        pLoc.Direction = ParameterDirection.Input;
                        pLoc.ParameterName = "pLoc";
                        pLoc.Value = string.Format("/upload/{0}/{1}",Request.QueryString["id"],uploadFl.FileName);

                        var pSize = com.CreateParameter();
                        pSize.DbType = DbType.Int32;
                        pSize.Direction=ParameterDirection.Input;
                        pSize.ParameterName = "pSize";
                        pSize.Value = uploadFl.FileBytes.Count();
                        
                        com.Parameters.Add(pCat);
                        com.Parameters.Add(pLoc);
                        com.Parameters.Add(pSize);

                        if (com.ExecuteNonQuery() < 1)
                        {
                            lblError.Text = "Somthing went wrong!";
                        }
                        else
                        {
                            Response.Redirect("index.aspx?id=" + Request.QueryString["id"]);
                        }
                    }
                }

                lblError.Text = "WIP";
                return;
            }
            else if (rbCat.Checked)
            {
                if (tbTitle.Text.Length < 4)
                {
                    lblError.Text = "Not enough characters";
                    return;
                }
                using (var con = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection())
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandText =
                            "INSERT INTO bijdrage VALUES (null,(SELECT id FROM account WHERE gebruikersnaam = :usrn), SYSDATE, 'categorie')";

                        var pUsrn = com.CreateParameter();
                        pUsrn.DbType = DbType.String;
                        pUsrn.Direction = ParameterDirection.Input;
                        pUsrn.ParameterName = "usrn";
                        pUsrn.Value = Session["username"];

                        com.Parameters.Add(pUsrn);

                        com.ExecuteNonQuery();

                        com.Parameters.Clear();
                        com.CommandText = "INSERT INTO categorie VALUES ((SELECT max(id) FROM bijdrage),:catid, :pTitle) ";

                        var pCat = com.CreateParameter();
                        pCat.DbType = DbType.Int32;
                        pCat.Direction = ParameterDirection.Input;
                        pCat.ParameterName = "catid";
                        pCat.Value = Convert.ToInt32(Request.QueryString["id"]);
                        
                        var pTitle = com.CreateParameter();
                        pTitle.DbType = DbType.String;
                        pTitle.Direction = ParameterDirection.Input;
                        pTitle.ParameterName = "pTitle";
                        pTitle.Value = tbTitle.Text;


                        com.Parameters.Add(pCat);
                        com.Parameters.Add(pTitle);

                        if (com.ExecuteNonQuery() < 1)
                        {
                            lblError.Text = "Somthing went wrong!";
                        }
                        else
                        {
                            Response.Redirect("index.aspx?id=" + Request.QueryString["id"]);
                        }
                    }
                }

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
                        com.CommandText =
                            "INSERT INTO bijdrage VALUES (null,(SELECT id FROM account WHERE gebruikersnaam = :usrn), SYSDATE, 'bericht')";

                        var pUsrn = com.CreateParameter();
                        pUsrn.DbType = DbType.String;
                        pUsrn.Direction = ParameterDirection.Input;
                        pUsrn.ParameterName = "usrn";
                        pUsrn.Value = Session["username"];

                        com.Parameters.Add(pUsrn);

                        com.ExecuteNonQuery();
                        
                        com.Parameters.Clear();
                        com.CommandText = "INSERT INTO bericht VALUES ((SELECT max(id) FROM bijdrage),:pTitle,:pText)";

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

                        com.Parameters.Add(pTitle);
                        com.Parameters.Add(pText);

                        com.ExecuteNonQuery();

                        com.Parameters.Clear();
                        com.CommandText = "INSERT INTO BIJDRAGE_BERICHT VALUES (:catid, (SELECT max(id) FROM bijdrage))";

                        var pCat = com.CreateParameter();
                        pCat.DbType = DbType.Int32;
                        pCat.Direction = ParameterDirection.Input;
                        pCat.ParameterName = "catid";
                        pCat.Value = Convert.ToInt32(Request.QueryString["id"]);

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