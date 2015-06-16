using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Configuration;

namespace Ict4EventsWebApp
{
    public partial class MateriaalVerhuur : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //todo add everything to rent
                lbProducten.Items.Clear();
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
                    com.CommandText = "SELECT DISTINCT Merk, serie, prijs FROM PRODUCT INNER JOIN productexemplaar ON product.id = productexemplaar.product_id WHERE productexemplaar.id NOT IN (select productexemplaar_id FROM verhuur)";
                    AddParameterWithValue(com, "prodBrand", tbBarcode.Text);
                    DbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        lbProducten.Items.Add(reader[0].ToString() + "  " + reader[1].ToString());
                    }

                }
            }
            
        }

        protected void btSubmit_Click(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Session["aapje"] = 0;
            }
            Session["aapje"]=(int)Session["aapje"]+1;
            lbProducten.Items.Add(clEndDate.SelectedDate.ToString());
        }
        private void AddParameterWithValue(DbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.DbType = System.Data.DbType.AnsiString;
            parameter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }

        protected void BtnConfirm_Click(object sender, EventArgs e)
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
                com.CommandText = "Select Reservering_polsbandje.id, persoon.voornaam FROM POLSBANDJE INNER JOIN RESERVERING_POLSBANDJE ON POLSBANDJE.id = RESERVERING_POLSBANDJE.polsbandje_id INNER JOIN RESERVERING ON RESERVERING_POLSBANDJE.RESERVERING_ID = RESERVERING.id INNER JOIN PERSOON ON RESERVERING.PERSOON_id = PERSOON.id WHERE POLSBANDJE.Barcode = :1";
                AddParameterWithValue(com, "prodBrand", tbBarcode.Text);
                DbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Session["ResPbId"] = Convert.ToInt32(reader[0]);
                    lbNameAfterBarcode.Text = reader[1].ToString();
                }

            }
        }

        protected void lbProducten_SelectedIndexChanged(object sender, EventArgs e)
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
                com.CommandText = "Select typenummer, prijs FROM Product WHERE merk = :1 AND rownum = 1";
                AddParameterWithValue(com, "prodBrand", lbProducten.SelectedValue.ToString().Substring(0,lbProducten.SelectedValue.ToString().IndexOf("  ")));
                DbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    lbDetail.Text = reader[0].ToString();
                    lblRentCost.Text = reader[1].ToString();
                }

            }
        }
    }
}