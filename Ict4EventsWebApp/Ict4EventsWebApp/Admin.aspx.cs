using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ict4EventsWebApp
{
    public partial class Admin : System.Web.UI.Page
    {
        private void AddParameterWithValue(DbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.DbType = System.Data.DbType.AnsiString;
            parameter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //GET LOCATIONS FROM EVENT
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
                com.CommandText = "SELECT * FROM LOCATIE";
                DbDataReader reader = com.ExecuteReader();
                try
                {
                    ddlLocation.Items.Clear();
                    while (reader.Read())
                    {
                        ddlLocation.Items.Add(reader[0].ToString() + ". " + reader[1].ToString());
                    }
                }
                catch (NullReferenceException)
                {
                    
                }
            }
            //GET ALL USERS
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
                com.CommandText = "SELECT gebruikersnaam FROM account";
                DbDataReader reader = com.ExecuteReader();
                try
                {
                    lbUsers.Items.Clear();
                    while (reader.Read())
                    {
                        lbUsers.Items.Add(reader[0].ToString());
                    }
                }
                catch (NullReferenceException)
                {

                }
            }
        }

        protected void btnAddEvent_Click(object sender, EventArgs e)
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                if (con == null)
                {
                    
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                if (com == null)
                {
                    
                }
                com.Connection = con;
                com.CommandText = "INSERT INTO EVENT(locatie_id, naam, datumStart, datumEinde, maxBezoekers) VALUES(:1, :2, to_date(:3, 'DD-MM-YYYY HH24:MI:SS'), to_date(:4, 'DD-MM-YYYY HH24:MI:SS'), :5)";
                string location = ddlLocation.SelectedValue.ToString();
                int locationId = Convert.ToInt32(location.Substring(0, location.IndexOf(".")));
                AddParameterWithValue(com, "locId", locationId);
                AddParameterWithValue(com, "naam", tbName.Text);
                AddParameterWithValue(com, "datumStart", clStartDate.SelectedDate.ToString());
                AddParameterWithValue(com, "datumEinde", clEndDate.SelectedDate.ToString());
                AddParameterWithValue(com, "maxBezoekers", Convert.ToInt32(tbMaxVis.Text));
                com.ExecuteNonQuery();
            }
        }

        protected void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string user = lbUsers.SelectedValue.ToString();
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
                com.CommandText = "SELECT titel, inhoud FROM bericht INNER JOIN bijdrage ON bijdrage.id = bericht.bijdrage_id INNER JOIN account ON account.id = bijdrage.account_id WHERE account.gebruikersnaam = :1";
                AddParameterWithValue(com, "gebruiker", user);
                DbDataReader reader = com.ExecuteReader();
                try
                {
                    lbUsers.Items.Clear();
                    while (reader.Read())
                    {
                        lbUsers.Items.Add(reader[0].ToString());
                    }
                }
                catch (NullReferenceException)
                {

                }
            }
        }
    }
}