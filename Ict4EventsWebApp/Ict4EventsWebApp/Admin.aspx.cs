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
        /// <summary>
        /// A method to prevent SQL-Injections 
        /// </summary>
        /// <param name="command"> The command which is sent to the databse</param>
        /// <param name="parameterName"> The name you give to the parameter (you could use that instead of numbers)</param>
        /// <param name="parameterValue"> The value that needs to be checked for sql injection</param>
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
        /// Loads all evenlocations people can select from. Fills a list with all users 
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
                //GET LOCATIONS FROM EVENT
                using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
                {
                    if (con == null)
                    {
                        ConnectieError();
                        return;
                    }
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    try
                    {
                        con.Open();
                        DbCommand com = OracleClientFactory.Instance.CreateCommand();
                        if (com == null)
                        {
                            //return "Error! No Command";
                        }
                        com.Connection = con;
                        com.CommandText = "SELECT * FROM LOCATIE";
                        DbDataReader reader = com.ExecuteReader();
                        ddlLocation.Items.Clear();
                        while (reader.Read())
                        {
                            ddlLocation.Items.Add(reader[0].ToString() + ". " + reader[1].ToString());
                        }
                        //GET ALL USERS
                        DbCommand com2 = OracleClientFactory.Instance.CreateCommand();
                        if (com2 == null)
                        {
                            //return "Error! No Command";
                        }
                        com2.Connection = con;
                        com2.CommandText = "SELECT gebruikersnaam FROM account ORDER BY gebruikersnaam";
                        DbDataReader reader2 = com2.ExecuteReader();
                        lbUsers.Items.Clear();
                        while (reader2.Read())
                        {
                            lbUsers.Items.Add(reader2[0].ToString());
                        }
                        DbCommand com3 = OracleClientFactory.Instance.CreateCommand();
                        com3.Connection = con;
                        com3.CommandText = "SELECT naam FROM productcat";
                        DbDataReader reader3 = com3.ExecuteReader();
                        while (reader3.Read())
                        {
                            ddlCat.Items.Add(reader3[0].ToString());
                        }

                    }
                    catch (NullReferenceException)
                    {
                        DatabaseError();
                        return;
                    }
                    catch (DbException)
                    {
                        ConnectieError();
                        return;
                    }
                }
                GenerateMaterials();
            }
        }
        private void DatabaseError()
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Dit staat niet in de database')</script>");
            return;
        }

        private void ConnectieError()
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Er is geen connectie')</script>");
            return;
        }
        /// <summary>
        /// Gets all Materials from the database and counts how many you have of each product
        /// </summary>
        private void GenerateMaterials()
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                if (con == null)
                {
                    ConnectieError();
                    return;
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                try
                {
                    con.Open();
                    DbCommand com2 = OracleClientFactory.Instance.CreateCommand();
                    if (com2 == null)
                    {
                        //return "Error! No Command";
                    }
                    com2.Connection = con;
                    com2.CommandText = "SELECT product.id,product.merk, product.serie, COUNT(Productexemplaar.id) FROM product LEFT OUTER JOIN Productexemplaar ON product.id = Productexemplaar.product_id GROUP BY product.id, product.merk,product.serie ORDER BY product.id";
                    DbDataReader reader2 = com2.ExecuteReader();
                    lbMaterials.Items.Clear();
                    while (reader2.Read())
                    {
                        lbMaterials.Items.Add(string.Format("{0}. {1} {2} aantal:{3}", reader2[0].ToString(), reader2[1].ToString(), reader2[2].ToString(), reader2[3].ToString()));
                    }
                    lbMaterials.Items.Add("Nieuw product");
                }
                catch (NullReferenceException)
                {
                    DatabaseError();
                    return;
                }
                catch (DbException)
                {
                    ConnectieError();
                    return;
                }
            }
        }
        /// <summary>
        /// Adds an event tot the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddEvent_Click(object sender, EventArgs e)
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                if (con == null)
                {
                    ConnectieError();
                    return;
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                try
                {
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
                catch (DbException)
                {
                    ConnectieError();
                    return;
                }

            }
        }
        /// <summary>
        /// Shows the messages the person placed, everytime a new person is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get user
            string user = lbUsers.SelectedValue.ToString();
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                if (con == null)
                {
                    ConnectieError();
                    return;
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                try
                {
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
                    lbMessages.Items.Clear();
                    while (reader.Read())
                    {
                        //Checks the Checkbox, if the person is blocked it's checked else it's unchecked
                        lbMessages.Items.Add(reader[0].ToString() + " " + reader[1].ToString());
                        if ((short)reader[2] == 0)
                        {
                            cbBlock.Checked = true;
                        }
                        else
                        {
                            cbBlock.Checked = false;
                        }
                    }
                    //Gets all messages of the selected user
                    DbCommand com2 = OracleClientFactory.Instance.CreateCommand();
                    if (com2 == null)
                    {
                        //return "Error! No Command";
                    }
                    com2.Connection = con;
                    com2.CommandText = "SELECT product.merk, product.serie, productexemplaar.id FROM product INNER JOIN productexemplaar ON product.id = productexemplaar.product_id INNER JOIN verhuur ON productexemplaar.id = verhuur.productexemplaar_id INNER JOIN reservering_polsbandje ON verhuur.res_pb_id = reservering_polsbandje.id INNER JOIN account ON reservering_polsbandje.account_id = account.id WHERE gebruikersnaam = :1";
                    AddParameterWithValue(com2, "gebrnm", lbUsers.SelectedValue.ToString());
                    DbDataReader reader2 = com2.ExecuteReader();
                    lbRentedMat.Items.Clear();
                    while (reader2.Read())
                    {
                        lbRentedMat.Items.Add(string.Format("{0} {1} {2}", reader2[0].ToString(), reader2[1].ToString(), reader2[2].ToString()));
                    }
                    cbBlock.Enabled = true;
                }
                catch (NullReferenceException)
                {
                    DatabaseError();
                    return;
                }
                catch (DbException)
                {
                    ConnectieError();
                    return;
                }
            }

        }
        /// <summary>
        /// Block/Unblocks an user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbBlock_CheckedChanged(object sender, EventArgs e)
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                if (con == null)
                {
                    ConnectieError();
                    return;
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                try
                {
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
                catch (NullReferenceException)
                {
                    DatabaseError();
                    return;
                }
                catch (DbException)
                {
                    ConnectieError();
                    return;
                }

            }
        }
        /// <summary>
        /// Adds 1 item to the selected item, for example 1 samsung tablet becomes 2 when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddCopy_Click(object sender, EventArgs e)
        {
            if (lbMaterials.SelectedValue.ToString() != "Nieuw product")
            {
                using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
                {
                    if (con == null)
                    {
                        ConnectieError();
                        return;
                    }
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    try
                    {
                        con.Open();
                        DbCommand com = OracleClientFactory.Instance.CreateCommand();
                        if (com == null)
                        {
                            //return "Error! No Command";
                        }
                        com.Connection = con;

                        string selValue = lbMaterials.SelectedValue.ToString();
                        int prodId = Convert.ToInt32(selValue.Substring(0, selValue.IndexOf(".")));

                        if (selValue.Substring(selValue.IndexOf(":") + 1) == "0")
                        {
                            com.CommandText = "INSERT INTO productexemplaar (product_id, volgnummer, barcode) VALUES (:1, 1, 1||:1)";
                        }
                        else
                        {
                            com.CommandText = "INSERT INTO productexemplaar (product_id, volgnummer, barcode) SELECT product_id, max(volgnummer)+1, max(volgnummer)+1 || :1 FROM productexemplaar WHERE product_id = :1 GROUP BY product_id";
                        }
                        AddParameterWithValue(com, "prodNr", prodId);
                        com.ExecuteNonQuery();
                        GenerateMaterials();
                    }
                    catch (NullReferenceException)
                    {
                        DatabaseError();
                        return;
                    }
                    catch (DbException)
                    {
                        ConnectieError();
                        return;
                    }

                }
            }
        }

        /// <summary>
        /// Displays more info about the selected product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBrand.Enabled = true;
            tbSeries.Enabled = true;
            tbTypeNr.Enabled = true;
            tbPrice.Enabled = true;
            ddlCat.Enabled = true;
            if (lbMaterials.SelectedValue.ToString() != "Nieuw product")
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
                        ConnectieError();
                        return;
                    }
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    try
                    {
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
                        DatabaseError();
                        return;
                    }
                    catch (DbException)
                    {
                        ConnectieError();
                        return;
                    }
                }


            }
            //If it's a new product all textboxes become empty
            else
            {
                btnAddCopy.Enabled = false;
                btnRmvCopy.Enabled = false;
                btnNew.Enabled = true;
                btnUpdate.Enabled = false;
                tbBrand.Text = string.Empty;
                tbPrice.Text = string.Empty;
                tbSeries.Text = string.Empty;
                tbTypeNr.Text = string.Empty;
            }
        }

        /// <summary>
        /// Removes one copy of the selected product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRmvCopy_Click(object sender, EventArgs e)
        {
            if (lbMaterials.SelectedValue != null)
            {
                string ProductId = lbMaterials.SelectedValue.Substring(0, lbMaterials.SelectedValue.IndexOf("."));
                using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
                {
                    if (con == null)
                    {
                        ConnectieError();
                        return;
                    }
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    try
                    {
                        con.Open();
                        //Deletes an copy of a product which isn't rent
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
                    catch (NullReferenceException)
                    {
                        DatabaseError();
                        return;
                    }
                    catch (DbException)
                    {
                        ConnectieError();
                        return;
                    }
                }
            }


        }
        /// <summary>
        /// Updates the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                string ProductId = lbMaterials.SelectedValue.Substring(0, lbMaterials.SelectedValue.IndexOf("."));
                if (con == null)
                {
                    ConnectieError();
                    return;
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                try
                {
                    con.Open();
                    //Updates the product which is selected
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
                catch (NullReferenceException)
                {
                    DatabaseError();
                    return;
                }
                catch (DbException)
                {
                    ConnectieError();
                    return;
                }
            }
        }
        /// <summary>
        /// Creates a new product which gets inserted in the table 'PRODUCT'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                if (con == null)
                {
                    ConnectieError();
                    return;
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                try
                {
                    con.Open();
                    DbCommand com = OracleClientFactory.Instance.CreateCommand();
                    if (com == null)
                    {
                        //return "Error! No Command";
                    }
                    com.Connection = con;
                    com.CommandText = "INSERT INTO PRODUCT(productcat_id, merk, serie, typenummer, prijs) SELECT productcat.Id, :1, :2, :3, :4 FROM productcat WHERE productcat.naam = :5 AND ROWNUM = 1";
                    AddParameterWithValue(com, "prodBrand", tbBrand.Text);
                    AddParameterWithValue(com, "prodSerie", tbSeries.Text);
                    AddParameterWithValue(com, "prodTypeNr", tbTypeNr.Text);
                    AddParameterWithValue(com, "prodPrice", tbPrice.Text);
                    AddParameterWithValue(com, "prodCat", ddlCat.SelectedValue.ToString());
                    com.ExecuteNonQuery();
                }
                catch (NullReferenceException)
                {
                    DatabaseError();
                    return;
                }
                catch (DbException)
                {
                    ConnectieError();
                    return;
                }
            }
        }


    }
}