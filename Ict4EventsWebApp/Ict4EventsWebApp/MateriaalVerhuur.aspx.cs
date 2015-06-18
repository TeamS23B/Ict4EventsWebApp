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
        /// <summary>
        /// If page first loads: fill list with available products
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(bool)(Session["loggedIn"] ?? false) || (string)(Session["username"] ?? "") != "admin")
            {
                Response.Redirect("index.aspx");//to index and to login
            }

            if (!Page.IsPostBack)
            {
                btSubmit.Enabled = false;
                lbProducten.Items.Clear();
                try
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
                        com.CommandText = "SELECT DISTINCT product.id, Merk, serie, prijs FROM PRODUCT INNER JOIN productexemplaar ON product.id = productexemplaar.product_id WHERE productexemplaar.id NOT IN (select productexemplaar_id FROM verhuur)";
                        AddParameterWithValue(com, "prodBrand", tbBarcode.Text);
                        DbDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            lbProducten.Items.Add(reader[0].ToString() + ". " + reader[1].ToString() + " " + reader[2].ToString());
                        }
                    }
                }
                catch (DbException ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "')</script>");
                    return;
                }
                catch (NullReferenceException ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "')</script>");
                    return;
                }
            }
        }

        /// <summary>
        /// Rent selected product until selected date for scanned person
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSubmit_Click(object sender, EventArgs e)
        {
            if (clEndDate.SelectedDate >= DateTime.Today)
            {
                try
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
                        com.CommandText = "INSERT INTO verhuur (productexemplaar_id, res_pb_id, datumIn, datumUit, Prijs, Betaald) SELECT productexemplaar.id, :1, TO_DATE(:2, 'DD-MM-YYYY HH24:MI:SS'), TO_DATE(:3, 'DD-MM-YYYY HH24:MI:SS'), prijs, 1 FROM product INNER JOIN productexemplaar ON product.id = productexemplaar.product_id WHERE product.id = :4 AND rownum = 1";
                        string respbidval = Session["ResPbId"] as string;
                        AddParameterWithValue(com, "respbId", respbidval);
                        AddParameterWithValue(com, "datumIn", DateTime.Today.ToString());
                        AddParameterWithValue(com, "datumEind", clEndDate.SelectedDate.ToString());
                        string prodIdVal = lbProducten.SelectedValue.ToString().Substring(0, lbProducten.SelectedValue.ToString().IndexOf("."));
                        AddParameterWithValue(com, "prodId", prodIdVal);

                        com.ExecuteNonQuery();
                    }
                }
                catch (DbException ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "')</script>");
                    return;
                }
                catch (NullReferenceException ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "')</script>");
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Datum klopt niet')</script>");
            }
        }

        /// <summary>
        /// Inserting SQL Parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        private void AddParameterWithValue(DbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.DbType = System.Data.DbType.AnsiString;
            parameter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// Lookup name for scanned barcode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnConfirm_Click(object sender, EventArgs e)
        {
            try
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
                        Session["ResPbId"] = reader[0].ToString();
                        lbNameAfterBarcode.Text = reader[1].ToString();
                    }

                }
            }
            catch (DbException ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "')</script>");
                return;
            }
            catch (NullReferenceException ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "')</script>");
                return;
            }
        }

        /// <summary>
        /// If product selected: lookup information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbProducten_SelectedIndexChanged(object sender, EventArgs e)
        {
            btSubmit.Enabled = true;
            try
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
                    com.CommandText = "Select typenummer, prijs FROM Product WHERE id = :1 AND rownum = 1";
                    AddParameterWithValue(com, "prodId", lbProducten.SelectedValue.ToString().Substring(0, lbProducten.SelectedValue.ToString().IndexOf(".")));
                    DbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        lbDetail.Text = reader[0].ToString();
                        lblRentCost.Text = reader[1].ToString();
                    }

                }
            }
            catch (DbException ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "')</script>");
                return;
            }
            catch (NullReferenceException ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "')</script>");
                return;
            }
        }
    }
}