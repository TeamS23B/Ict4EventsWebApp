using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Configuration;

namespace Ict4EventsWebApp
{
    public partial class Check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //protected void BtnLogin_Click(object sender, EventArgs e)
        //{
        //    if (Authentication.Instance.IsAuthenticated(tbUsername.Text, tbPassword.Text))
        //    {
        //        lblUsername.Text = "Success!";
        //    }
        //    else
        //    {
        //        lblUsername.Text = "False!";
        //    }
        //}

        private void AddParameterWithValue(DbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.DbType = System.Data.DbType.AnsiString;
            parameter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {

            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                if (con == null)
                {
                    //return "Error! No Connection";
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                if (com == null)
                {
                    //return "Error! No Command";
                }
                com.Connection = con;
                com.CommandText = "SELECT reservering.betaald FROM polsbandje INNER JOIN reservering_polsbandje ON polsbandje.id = reservering_polsbandje.polsbandje_id INNER JOIN reservering ON reservering_polsbandje.reservering_id = reservering.id WHERE polsbandje.barcode = :1";
                AddParameterWithValue(com, "barc", (string)tbBarcode.Text);

                lblBarcodeObject.Text = tbBarcode.Text;
                try
        {
                    if ((short)com.ExecuteScalar() == 0)
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
                        lblBetaaldobject.Text = "Betaald";
                    }
                }
                catch (NullReferenceException)
                {
                    lblBarcodeObject.Text = "Barcode is niet gevonden";
                }
            }
        }
    }
}