using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Configuration;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Net.Mail;

namespace Ict4EventsWebApp
{
    public partial class ReservationMaterial : System.Web.UI.Page
    {
        private Party party { get; set; }

        /// <summary>
        /// When the page is first loaded, only make the visitor information section visible.
        /// If there is no party session available, create an empty party.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlMaterial.Visible = false;
                pnlOverview.Visible = false;
                pnlMap.Visible = false;
            }
            if (Session["party"] == null)
            {
                party = new Party();
            }
            else
            {
                party = (Party)Session["party"];

                foreach (Person person in party.Members)
                {
                    lbGroupMembers.Items.Add(person.ToString());
                }
            }

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

        /// <summary>
        /// Make only the material reservation visible and load the available materials.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {

            string a = XValue.Value;
            string b = YValue.Value;

            if (a == "" || b == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('geen plaats gekozen')</script>");
                return;
            }

            string plekId;

            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "GET_PLEKID";

                var p1 = com.CreateParameter();
                p1.DbType = DbType.Decimal;
                p1.ParameterName = "X";
                p1.Value = Convert.ToInt32(a);
                com.Parameters.Add(p1);

                var p2 = com.CreateParameter();
                p2.DbType = DbType.Decimal;
                p2.ParameterName = "Y";
                p2.Value = Convert.ToInt32(b);
                com.Parameters.Add(p2);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "PlekId";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                plekId = com.Parameters["PlekId"].Value.ToString();
            }

            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.Connection = con;
                com.CommandText = "SELECT COUNT(*) FROM plek_reservering WHERE plek_id = :plekId";

                var p1 = com.CreateParameter();
                p1.DbType = DbType.Decimal;
                p1.ParameterName = "plekId";
                p1.Value = Convert.ToInt32(plekId);
                com.Parameters.Add(p1);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;

                decimal result = (decimal)com.ExecuteScalar();

                if (result > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Plaats is al gekozen')</script>");
                    return;
                }


            }
            pnlMaterial.Visible = true;
            pnlMap.Visible = false;


            //using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            //{


            //    DbCommand com = OracleClientFactory.Instance.CreateCommand();
            //    com.CommandType = System.Data.CommandType.StoredProcedure;
            //    com.CommandText = "Verdubbel";
            //    var p = com.CreateParameter();
            //    p.DbType = DbType.Decimal;
            //    p.ParameterName = "teVerdubbelen";
            //    p.Value = Convert.ToInt32(voorbeeld.Text);
            //    com.Parameters.Add(p);

            //    var q = com.CreateParameter();
            //    q.DbType = DbType.Decimal;
            //    q.ParameterName = "resultaat";
            //    q.Direction = ParameterDirection.Output;
            //    com.Parameters.Add(q);

            //    //com.Parameters.Add("teVerdubbelen", OracleDbType.Decimal).value = Convert.ToInt32(TextBox1.Text);
            //    //com.Parameters.Add("resultaat", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            //    try
            //    {
            //        con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
            //        con.Open();
            //        com.Connection = con;
            //        com.ExecuteNonQuery(); //Voert de stored procedure uit

            //        Label1.Text = "De verdubbelde waarde is: " + com.Parameters["resultaat"].Value;
            //        //Tonen van de waarde in de resultaat parameter na uitvoeren van de stored procedure
            //        //label1.   .Show("De verdubbelde waarde is: " + objCmd.Parameters["resultaat"].Value);
            //    }
            //    catch (Exception ex)
            //    {
            //        //Voor het geval "iets" mis gaat, de letterlijke foutmelding tonen (doe je natuurlijk niet in een "echte" applicatie)
            //        //MessageBox.Show("De volgende fout is opgetreden: " + ex.ToString());
            //    }
            //    //Verbinding sluiten (waarschijnlijk doe je dit in je applicatie niet per database commando)
            //    con.Close();
            //}
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
                    DbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        lbavailableMaterial.Items.Add(reader[0].ToString() + ". " + reader[1].ToString() + " " + reader[2].ToString());
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
        //private void AddParameterWithValue(DbConnection con, string p, string plekId)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Make only the map visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNextStep_Click(object sender, EventArgs e)
        {

            pnlMap.Visible = true;
            pnlRegistration.Visible = false;

        }

        /// <summary>
        /// Make only the overview visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btRMAterialVerder_Click(object sender, EventArgs e)
        {

            string a = XValue.Value;
            string b = YValue.Value;
            string plekid;
            string persoonId;
            string accountId;
            string reserveringId;
            string resPolsId;
            #region location coordinates
            // Retrieve the to-be-reserved location coordinates
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "GET_PLEKID";

                var p1 = com.CreateParameter();
                p1.DbType = DbType.Decimal;
                p1.ParameterName = "X";
                p1.Value = Convert.ToInt32(a);
                com.Parameters.Add(p1);

                var p2 = com.CreateParameter();
                p2.DbType = DbType.Decimal;
                p2.ParameterName = "Y";
                p2.Value = Convert.ToInt32(b);
                com.Parameters.Add(p2);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "PlekId";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                plekid = com.Parameters["PlekId"].Value.ToString();
            }
            #endregion

            #region Insert group leader
            //Insert the group leader into the database.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_PERSOONLEIDER";
                string infix;
                if (tbInfix.Text == "")
                {
                    infix = " ";
                }
                else
                {
                    infix = tbInfix.Text;
                }

                AddParameterWithValue(com, "voornaam", tbFirstName.Text);
                AddParameterWithValue(com, "tussenvoegsel", infix);
                AddParameterWithValue(com, "achternaam", tbSurname.Text);
                AddParameterWithValue(com, "straat", tbStreet.Text);
                AddParameterWithValue(com, "huisnr", tbHouseNr.Text);
                AddParameterWithValue(com, "postcode", tbPostalCode.Text);
                AddParameterWithValue(com, "banknr", tbIban.Text);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();

                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_PersoonLeider')</script>");
                    return;
                }
            }
            #endregion

            #region highest ID person
            // Get the highest ID from the person list and raise it by 1 (new highest ID).
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandText = "SELECT MAX(ID) FROM PERSOON";

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;

                string c = com.ExecuteScalar().ToString();
                int d = Convert.ToInt32(c);
                persoonId = d.ToString();
            }
            #endregion

            #region Insert account
            // Insert an account into the database.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_ACCOUNT";

                AddParameterWithValue(com, "gebruikersnaam", tbFirstName.Text.ToString() + " " + tbSurname.Text.ToString());
                AddParameterWithValue(com, "email", tbEmail.Text);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                var qacc = com.CreateParameter();
                qacc.DbType = DbType.Decimal;
                qacc.ParameterName = "accountId";
                qacc.Direction = ParameterDirection.Output;
                com.Parameters.Add(qacc);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();
                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Account. Gebruikersnaam en / of email is al in gebruik.')</script>");
                    return;
                }
                accountId = com.Parameters["accountId"].Value.ToString();
            }

            //var smtpc = new SmtpClient();
            //smtpc.Host = "172.20.112.3";
            //smtpc.EnableSsl = false;
            //smtpc.UseDefaultCredentials = true;

            //var mm = new MailMessage();
            //mm.From = new MailAddress("admin@");
            //mm.To.Add(tbTarget.Text);
            //mm.Subject = tbSubject.Text;
            //mm.Body = tbText.Text;
            //smtpc.Send(mm);

            #endregion

            #region Insert reservation
            // Create a reservation for the event.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_RESERVERING";

                AddParameterWithValue(com, "persoonId", persoonId);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                var qacc = com.CreateParameter();
                qacc.DbType = DbType.Decimal;
                qacc.ParameterName = "reserveringIdOUT";
                qacc.Direction = ParameterDirection.Output;
                com.Parameters.Add(qacc);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();
                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Reservering.')</script>");
                    return;
                }
                reserveringId = com.Parameters["reserveringIdOUT"].Value.ToString();
            }
            #endregion

            #region Insert reservation plaats
            // Create a reservation for a location within an event.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_RESERVERING_PLAATS";

                AddParameterWithValue(com, "reserveringId", reserveringId);
                AddParameterWithValue(com, "plaatsId", plekid);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();
                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Reservering_Plaats.')</script>");
                    return;
                }
            }
            #endregion

            #region Insert reservation polsbandje
            // Bind a reservation to an account.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_RESERVERING_POLSBANDJE";

                AddParameterWithValue(com, "reservering_Id", reserveringId);
                AddParameterWithValue(com, "account_Id", accountId);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                var qacc = com.CreateParameter();
                qacc.DbType = DbType.Decimal;
                qacc.ParameterName = "reserveringPID";
                qacc.Direction = ParameterDirection.Output;
                com.Parameters.Add(qacc);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();
                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Reservering_Polsbandje.')</script>");
                    return;
                }
                resPolsId = com.Parameters["reserveringPID"].Value.ToString();
            }
            #endregion

            #region Insert materiaal
            // Insert all reserved materials into the database.
            while (lbMaterialToReserve.Items.Count > 0)
            {
                string s = lbMaterialToReserve.Items[0].ToString();
                s = s.Substring(0, s.IndexOf("."));

                string bedrag1 = "0";

                // Get the price of the product.
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
                    com.CommandText = "Select prijs FROM product WHERE product.id = :1 AND rownum = 1";
                    AddParameterWithValue(com, "prodId", s);
                    DbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        bedrag1 = reader[0].ToString();
                    }
                }

                // insert the rent of the product
                using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
                {
                    DbCommand com = OracleClientFactory.Instance.CreateCommand();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "INSERT_VERHUUR";

                    AddParameterWithValue(com, "PRODUCTEXEMPLAAR_ID", s);
                    AddParameterWithValue(com, "RES_PB_ID", resPolsId);
                    AddParameterWithValue(com, "PRIJS", bedrag1);
                    AddParameterWithValue(com, "BETAALD", "1");

                    var q = com.CreateParameter();
                    q.DbType = DbType.Decimal;
                    q.ParameterName = "insertGelukt";
                    q.Direction = ParameterDirection.Output;
                    com.Parameters.Add(q);

                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    com.Connection = con;
                    com.ExecuteNonQuery();

                    string result = com.Parameters["insertGelukt"].Value.ToString();
                    if (result == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Verhuur.')</script>");
                        return;
                    }
                }

                lbMaterialToReserve.Items.RemoveAt(0);
            }
            #endregion

            #region Insert party
            // If there are people in the party, insert each one of them into the database.
            foreach (Person member in party.Members)
            {
                using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
                {
                    DbCommand com = OracleClientFactory.Instance.CreateCommand();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "INSERT_PERSOONLID";
                    string infix;
                    if (member.Infix == null)
                    {
                        infix = " ";
                    }
                    else
                    {
                        infix = member.Infix;
                    }

                    AddParameterWithValue(com, "voornaam", member.Name);
                    AddParameterWithValue(com, "tussenvoegsel", infix);
                    AddParameterWithValue(com, "achternaam", member.Surname);

                    var q = com.CreateParameter();
                    q.DbType = DbType.Decimal;
                    q.ParameterName = "insertGelukt";
                    q.Direction = ParameterDirection.Output;
                    com.Parameters.Add(q);

                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    com.Connection = con;
                    com.ExecuteNonQuery();

                    string result = com.Parameters["insertGelukt"].Value.ToString();

                    if (result == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Persoonlid Member')</script>");
                        return;
                    }
                }

                // Insert an account into the database.
                using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
                {
                    DbCommand com = OracleClientFactory.Instance.CreateCommand();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "INSERT_ACCOUNT";

                    AddParameterWithValue(com, "gebruikersnaam", member.Name + " " + member.Surname);
                    AddParameterWithValue(com, "email", member.Email);

                    var q = com.CreateParameter();
                    q.DbType = DbType.Decimal;
                    q.ParameterName = "insertGelukt";
                    q.Direction = ParameterDirection.Output;
                    com.Parameters.Add(q);

                    var qacc = com.CreateParameter();
                    qacc.DbType = DbType.Decimal;
                    qacc.ParameterName = "accountId";
                    qacc.Direction = ParameterDirection.Output;
                    com.Parameters.Add(qacc);

                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    com.Connection = con;
                    com.ExecuteNonQuery();

                    string result = com.Parameters["insertGelukt"].Value.ToString();
                    if (result == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Account Member.')</script>");
                        return;
                    }
                    accountId = com.Parameters["accountId"].Value.ToString();
                }
            }
            #endregion
        }

        /// <summary>
        /// Add a new member to the party.
        /// Also update the party session with the latest party-version.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Person person = new Person(TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text);
            party.AddMember(person);
            lbGroupMembers.Items.Add(person.ToString());
            Session["party"] = party;
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";

        }

        /// <summary>
        /// To do: Remove member from party.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            string selectedgroupmember = (string)Session["SelectedMember"];

            foreach (Person member in party.Members)
            {
                if (member.ToString() == selectedgroupmember)
                {
                    party.Members.Remove(member);
                    lbGroupMembers.Items.Remove(selectedgroupmember);

                }
            }


        }

        /// <summary>
        /// Insert all data from previous registration forms into the database, or show a pop-up if anything goes wrong.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCMaterialVerder_Click(object sender, EventArgs e)
        {
            string a = XValue.Value;
            string b = YValue.Value;
            string plekid;
            string persoonId;
            string accountId;
            string reserveringId;
            string resPolsId;
            #region location coordinates
            // Retrieve the to-be-reserved location coordinates
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "GET_PLEKID";

                var p1 = com.CreateParameter();
                p1.DbType = DbType.Decimal;
                p1.ParameterName = "X";
                p1.Value = Convert.ToInt32(a);
                com.Parameters.Add(p1);

                var p2 = com.CreateParameter();
                p2.DbType = DbType.Decimal;
                p2.ParameterName = "Y";
                p2.Value = Convert.ToInt32(b);
                com.Parameters.Add(p2);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "PlekId";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                plekid = com.Parameters["PlekId"].Value.ToString();
            }
            #endregion

            #region Insert group leader
            //Insert the group leader into the database.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_PERSOONLEIDER";
                string infix;
                if (tbInfix.Text == "")
                {
                    infix = " ";
                }
                else
                {
                    infix = tbInfix.Text;
                }

                AddParameterWithValue(com, "voornaam", tbFirstName.Text);
                AddParameterWithValue(com, "tussenvoegsel", infix);
                AddParameterWithValue(com, "achternaam", tbSurname.Text);
                AddParameterWithValue(com, "straat", tbStreet.Text);
                AddParameterWithValue(com, "huisnr", tbHouseNr.Text);
                AddParameterWithValue(com, "postcode", tbPostalCode.Text);
                AddParameterWithValue(com, "banknr", tbIban.Text);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();

                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_PersoonLeider')</script>");
                    return;
                }
            }
            #endregion

            #region highest ID person
            // Get the highest ID from the person list and raise it by 1 (new highest ID).
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandText = "SELECT MAX(ID) FROM PERSOON";

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;

                string c = com.ExecuteScalar().ToString();
                int d = Convert.ToInt32(c);
                persoonId = d.ToString();
            }
            #endregion

            #region Insert account
            // Insert an account into the database.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_ACCOUNT";

                AddParameterWithValue(com, "gebruikersnaam", tbFirstName.Text.ToString() + " " + tbSurname.Text.ToString());
                AddParameterWithValue(com, "email", tbEmail.Text);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                var qacc = com.CreateParameter();
                qacc.DbType = DbType.Decimal;
                qacc.ParameterName = "accountId";
                qacc.Direction = ParameterDirection.Output;
                com.Parameters.Add(qacc);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();
                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Account. Gebruikersnaam en / of email is al in gebruik.')</script>");
                    return;
                }
                accountId = com.Parameters["accountId"].Value.ToString();
            }

            //var smtpc = new SmtpClient();
            //smtpc.Host = "172.20.112.3";
            //smtpc.EnableSsl = false;
            //smtpc.UseDefaultCredentials = true;

            //var mm = new MailMessage();
            //mm.From = new MailAddress("admin@");
            //mm.To.Add(tbTarget.Text);
            //mm.Subject = tbSubject.Text;
            //mm.Body = tbText.Text;
            //smtpc.Send(mm);

            #endregion

            #region Insert reservation
            // Create a reservation for the event.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_RESERVERING";

                AddParameterWithValue(com, "persoonId", persoonId);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                var qacc = com.CreateParameter();
                qacc.DbType = DbType.Decimal;
                qacc.ParameterName = "reserveringIdOUT";
                qacc.Direction = ParameterDirection.Output;
                com.Parameters.Add(qacc);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();
                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Reservering.')</script>");
                    return;
                }
                reserveringId = com.Parameters["reserveringIdOUT"].Value.ToString();
            }
            #endregion

            #region Insert reservation plaats
            // Create a reservation for a location within an event.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_RESERVERING_PLAATS";

                AddParameterWithValue(com, "reserveringId", reserveringId);
                AddParameterWithValue(com, "plaatsId", plekid);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();
                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Reservering_Plaats.')</script>");
                    return;
                }
            }
            #endregion

            #region Insert reservation polsbandje
            // Bind a reservation to an account.
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "INSERT_RESERVERING_POLSBANDJE";

                AddParameterWithValue(com, "reservering_Id", reserveringId);
                AddParameterWithValue(com, "account_Id", accountId);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "insertGelukt";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                var qacc = com.CreateParameter();
                qacc.DbType = DbType.Decimal;
                qacc.ParameterName = "reserveringPID";
                qacc.Direction = ParameterDirection.Output;
                com.Parameters.Add(qacc);

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();
                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Reservering_Polsbandje.')</script>");
                    return;
                }
                resPolsId = com.Parameters["reserveringPID"].Value.ToString();
            }
            #endregion

            #region Insert materiaal
            // Insert all reserved materials into the database.
            while (lbMaterialToReserve.Items.Count > 0)
            {
                string s = lbMaterialToReserve.Items[0].ToString();
                s = s.Substring(0, s.IndexOf("."));

                string bedrag1 = "0";

                // Get the price of the product.
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
                    com.CommandText = "Select prijs FROM product WHERE product.id = :1 AND rownum = 1";
                    AddParameterWithValue(com, "prodId", s);
                    DbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        bedrag1 = reader[0].ToString();
                    }
                }

                // insert the rent of the product
                using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
                {
                    DbCommand com = OracleClientFactory.Instance.CreateCommand();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "INSERT_VERHUUR";

                    AddParameterWithValue(com, "PRODUCTEXEMPLAAR_ID", s);
                    AddParameterWithValue(com, "RES_PB_ID", resPolsId);
                    AddParameterWithValue(com, "PRIJS", bedrag1);
                    AddParameterWithValue(com, "BETAALD", "1");

                    var q = com.CreateParameter();
                    q.DbType = DbType.Decimal;
                    q.ParameterName = "insertGelukt";
                    q.Direction = ParameterDirection.Output;
                    com.Parameters.Add(q);

                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    com.Connection = con;
                    com.ExecuteNonQuery();

                    string result = com.Parameters["insertGelukt"].Value.ToString();
                    if (result == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Verhuur.')</script>");
                        return;
                    }
                }

                lbMaterialToReserve.Items.RemoveAt(0);
            }
            #endregion

            #region Insert party
            // If there are people in the party, insert each one of them into the database.
            foreach (Person member in party.Members)
            {
                using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
                {
                    DbCommand com = OracleClientFactory.Instance.CreateCommand();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "INSERT_PERSOONLID";
                    string infix;
                    if (member.Infix == null)
                    {
                        infix = " ";
                    }
                    else
                    {
                        infix = member.Infix;
                    }

                    AddParameterWithValue(com, "voornaam", member.Name);
                    AddParameterWithValue(com, "tussenvoegsel", infix);
                    AddParameterWithValue(com, "achternaam", member.Surname);

                    var q = com.CreateParameter();
                    q.DbType = DbType.Decimal;
                    q.ParameterName = "insertGelukt";
                    q.Direction = ParameterDirection.Output;
                    com.Parameters.Add(q);

                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    com.Connection = con;
                    com.ExecuteNonQuery();

                    string result = com.Parameters["insertGelukt"].Value.ToString();

                    if (result == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Persoonlid Member')</script>");
                        return;
                    }
                }

                // Insert an account into the database.
                using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
                {
                    DbCommand com = OracleClientFactory.Instance.CreateCommand();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "INSERT_ACCOUNT";

                    AddParameterWithValue(com, "gebruikersnaam", member.Name + " " + member.Surname);
                    AddParameterWithValue(com, "email", member.Email);

                    var q = com.CreateParameter();
                    q.DbType = DbType.Decimal;
                    q.ParameterName = "insertGelukt";
                    q.Direction = ParameterDirection.Output;
                    com.Parameters.Add(q);

                    var qacc = com.CreateParameter();
                    qacc.DbType = DbType.Decimal;
                    qacc.ParameterName = "accountId";
                    qacc.Direction = ParameterDirection.Output;
                    com.Parameters.Add(qacc);

                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    com.Connection = con;
                    com.ExecuteNonQuery();

                    string result = com.Parameters["insertGelukt"].Value.ToString();
                    if (result == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald: Insert_Account Member.')</script>");
                        return;
                    }
                    accountId = com.Parameters["accountId"].Value.ToString();
                }
            }
            #endregion

        }

        // 




        /// <summary>
        /// Add material to the 'to rent' list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btMaterialAdd_Click(object sender, EventArgs e)
        {
            int Bedrag = Convert.ToInt32(lbPrice.Text);
            string ToAddMaterial = lbavailableMaterial.SelectedValue.ToString();
            lbMaterialToReserve.Items.Add(ToAddMaterial);
            lbavailableMaterial.Items.Remove(ToAddMaterial);
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
                    com.CommandText = "Select prijs FROM product WHERE product.id = :1 AND rownum = 1";
                    AddParameterWithValue(com, "prodId", ToAddMaterial.Substring(0, ToAddMaterial.IndexOf(".")));
                    DbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Bedrag = Bedrag + Convert.ToInt32(reader[0]);
                        lbPrice.Text = Bedrag.ToString();
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
        /// Remove materials from the 'to rent' list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btMaterialDelete_Click(object sender, EventArgs e)
        {
            string ToAddMaterial = lbMaterialToReserve.SelectedValue.ToString();
            lbavailableMaterial.Items.Add(ToAddMaterial);
            lbMaterialToReserve.Items.Remove(ToAddMaterial);
            int Bedrag = Convert.ToInt32(lbPrice.Text);
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
                    com.CommandText = "Select prijs FROM product WHERE product.id = :1 AND rownum = 1";
                    AddParameterWithValue(com, "prodId", ToAddMaterial.Substring(0, ToAddMaterial.IndexOf(".")));
                    DbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Bedrag = Bedrag - Convert.ToInt32(reader[0]);
                        lbPrice.Text = Bedrag.ToString();
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

        protected void tbFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void lbGroupMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


    }
}