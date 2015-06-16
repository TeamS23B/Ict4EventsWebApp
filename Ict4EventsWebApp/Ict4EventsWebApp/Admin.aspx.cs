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
            if (!Page.IsPostBack)
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
                    DbCommand com2 = OracleClientFactory.Instance.CreateCommand();
                    com2.Connection = con;
                    com2.CommandText = "SELECT naam FROM productcat";
                    DbDataReader reader2 = com2.ExecuteReader();
                    while (reader2.Read())
                    {
                        ddlCat.Items.Add(reader2[0].ToString());
                    }

                }
                GenerateMaterials();
            }
        }

        private void GenerateMaterials()
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                if (con == null)
                {
                    //return "Error! No Connection";
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                DbCommand com2 = OracleClientFactory.Instance.CreateCommand();
                if (com2 == null)
                {
                    //return "Error! No Command";
                }
                com2.Connection = con;
                com2.CommandText = "SELECT product.id,product.merk, product.serie, COUNT(Productexemplaar.id) FROM product, Productexemplaar WHERE product.id = Productexemplaar.product_id GROUP BY product.id, product.merk,product.serie ORDER BY product.id";
                DbDataReader reader2 = com2.ExecuteReader();
                try
                {
                    lbMaterials.Items.Clear();
                    while (reader2.Read())
                    {
                        lbMaterials.Items.Add(string.Format("{0}. {1} {2} aantal: {3}", reader2[0].ToString(), reader2[1].ToString(), reader2[2].ToString(), reader2[3].ToString()));
                    }
                    lbMaterials.Items.Add("Nieuw product");
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
                com.CommandText = "SELECT titel, inhoud, geactiveerd FROM bericht INNER JOIN bijdrage ON bijdrage.id = bericht.bijdrage_id RIGHT OUTER JOIN account ON account.id = bijdrage.account_id WHERE account.gebruikersnaam = :1";
                AddParameterWithValue(com, "gebruiker", user);
                DbDataReader reader = com.ExecuteReader();
                try
                {
                    lbMessages.Items.Clear();
                    while (reader.Read())
                    {
                        lbMessages.Items.Add(reader[0].ToString() + reader[1].ToString());
                        if ((short)reader[2] == 0)
                        {
                            cbBlock.Checked = true;
                        }
                        else
                        {
                            cbBlock.Checked = false;
                        }
                    }
                }
                catch (NullReferenceException)
                {

                }
                DbCommand com2 = OracleClientFactory.Instance.CreateCommand();
                if (com2 == null)
                {
                    //return "Error! No Command";
                }
                com2.Connection = con;
                com2.CommandText = "SELECT product.merk, product.serie, productexemplaar.id FROM product INNER JOIN productexemplaar ON product.id = productexemplaar.product_id INNER JOIN verhuur ON productexemplaar.id = verhuur.productexemplaar_id INNER JOIN reservering_polsbandje ON verhuur.res_pb_id = reservering_polsbandje.id INNER JOIN account ON reservering_polsbandje.account_id = account.id WHERE gebruikersnaam = :1";
                AddParameterWithValue(com2, "gebrnm", lbUsers.SelectedValue.ToString());
                DbDataReader reader2 = com2.ExecuteReader();
                try
                {
                    lbRentedMat.Items.Clear();
                    lbMessages.Items.Clear();
                    while (reader2.Read())
                    {
                        lbRentedMat.Items.Add(string.Format("{0} {1} {2}", reader2[0].ToString(), reader2[1].ToString(), reader2[2].ToString()));
                    }
                }
                catch (NullReferenceException)
                {

                }
            }
            cbBlock.Enabled = true;
        }

        protected void cbBlock_CheckedChanged(object sender, EventArgs e)
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
                com.CommandText = "UPDATE account SET geactiveerd = :1 WHERE gebruikersnaam = :2";
                if (cbBlock.Checked)
                {
                    AddParameterWithValue(com, "check", 0);
                }
                else
                {
                    AddParameterWithValue(com, "check", 1);
                }
                AddParameterWithValue(com, "gebrNaam", lbUsers.SelectedValue.ToString());
                com.ExecuteNonQuery();
            }
        }

        protected void btnAddCopy_Click(object sender, EventArgs e)
        {
            if (lbMaterials.SelectedValue.ToString() != "Nieuw product")
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
                    com.CommandText = "INSERT INTO productexemplaar (product_id, volgnummer, barcode) SELECT product_id, max(volgnummer)+1, max(volgnummer)+1 || :1 FROM productexemplaar WHERE product_id = :1 GROUP BY product_id";

                    string selValue = lbMaterials.SelectedValue.ToString();
                    int prodId = Convert.ToInt32(selValue.Substring(0, selValue.IndexOf(".")));
                    AddParameterWithValue(com, "prodNr", prodId);

                    com.ExecuteNonQuery();
                    GenerateMaterials();
                }
            }
        }

        protected void lbMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBrand.Enabled = true;
            tbSeries.Enabled = true;
            tbTypeNr.Enabled = true;
            tbPrice.Enabled = true;
            ddlCat.Enabled = true;
            if(lbMaterials.SelectedValue.ToString() != "Nieuw product")
            {
                string ProductId = lbMaterials.SelectedValue.Substring(0, lbMaterials.SelectedValue.IndexOf("."));
                btnAddCopy.Enabled = true;
                btnRmvCopy.Enabled = true;
                btnNew.Enabled = false;
                btnUpdate.Enabled = true;
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
                    com.CommandText = "SELECT merk,serie,typenummer,prijs FROM product WHERE product.id = :1";
                    AddParameterWithValue(com, "gebruiker", ProductId);
                    DbDataReader reader = com.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            tbBrand.Text = reader[0].ToString();
                            tbSeries.Text = reader[1].ToString();
                            tbTypeNr.Text = reader[2].ToString();
                            tbPrice.Text = Convert.ToString(reader[3]);
                        }
                    }
                    catch (NullReferenceException)
                    {

                    }
                    
                }
            }
            else
            {
                btnAddCopy.Enabled = false;
                btnRmvCopy.Enabled = false;
                btnNew.Enabled = true;
                btnUpdate.Enabled = false;
            }
        }

        protected void btnRmvCopy_Click(object sender, EventArgs e)
        {
            if(lbMaterials.SelectedValue != null)
            {
                string ProductId = lbMaterials.SelectedValue.Substring(0, lbMaterials.SelectedValue.IndexOf("."));
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
                    com.CommandText = "DELETE FROM productexemplaar WHERE id NOT IN (select productexemplaar_id FROM verhuur) AND product_id = :1 and rownum = 1";
                    AddParameterWithValue(com, "prodNr", ProductId);
                    com.ExecuteNonQuery();
                    GenerateMaterials();
                }
            }
            
            
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                string ProductId = lbMaterials.SelectedValue.Substring(0, lbMaterials.SelectedValue.IndexOf("."));
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
                com.CommandText = "UPDATE Product Set merk = :1, serie = :2, typenummer = :3, prijs = :4, productCAT_ID = (SELECT productcat.Id FROM productcat WHERE productcat.naam = :5 AND ROWNUM = 1) Where product.id = :6";
                AddParameterWithValue(com, "prodBrand", tbBrand.Text);
                AddParameterWithValue(com, "prodSerie", tbSeries.Text);
                AddParameterWithValue(com, "prodTypeNr", tbTypeNr.Text);
                AddParameterWithValue(com, "prodPrice", tbPrice.Text);
                AddParameterWithValue(com, "prodCat", ddlCat.SelectedValue.ToString());
                AddParameterWithValue(com, "prodCat", ProductId);
                com.ExecuteNonQuery();
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
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
                /*
                com.CommandText = "INSERT INTO PRODUCT(productcat_id, merk, serie, typenummer, prijs) SELECT productcat.Id, :2, :3, :4, :5 FROM productcat WHERE productcat.naam = :1 AND ROWNUM = 1";
                com.ExecuteNonQuery();
                 */
                com.CommandText = "INSERT INTO PRODUCT(productcat_id, merk, serie, typenummer, prijs) SELECT productcat.Id, :1, :2, :3, :4 FROM productcat WHERE productcat.naam = :5 AND ROWNUM = 1";
                AddParameterWithValue(com, "prodBrand", tbBrand.Text);
                AddParameterWithValue(com, "prodSerie", tbSeries.Text);
                AddParameterWithValue(com, "prodTypeNr", tbTypeNr.Text);
                AddParameterWithValue(com, "prodPrice", tbPrice.Text);
                AddParameterWithValue(com, "prodCat", ddlCat.SelectedValue.ToString());
                com.ExecuteNonQuery();
            }
        }

    }
}