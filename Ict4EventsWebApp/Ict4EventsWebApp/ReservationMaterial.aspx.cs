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

namespace Ict4EventsWebApp
{
    public partial class ReservationMaterial : System.Web.UI.Page
    {
        private Party party { get; set; }

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
                lbGroupMembers.Items.Clear();
                foreach (Person person in party.Members)
                {
                    lbGroupMembers.Items.Add(person.ToString());
        }
            }
        }
        
        protected void Button1_Click(object sender, EventArgs e)
        {
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
        }

        protected void btnNextStep_Click(object sender, EventArgs e)
                {

            pnlMap.Visible = true;
            pnlRegistration.Visible = false;

                }
        protected void btRMAterialVerder_Click(object sender, EventArgs e)
                {
            pnlOverview.Visible = true;
            pnlMaterial.Visible = false;
        }

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

        protected void btCMaterialVerder_Click(object sender, EventArgs e)
        {
            string a = XValue.Value;
            string b = YValue.Value;
            string persoonId;
            string accountId;
            string reserveringId;

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

                string plekid = com.Parameters["PlekId"].Value.ToString();
            }
            //using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            //{
            //    DbCommand com = OracleClientFactory.Instance.CreateCommand();
            //    com.CommandType = System.Data.CommandType.StoredProcedure;
            //    com.CommandText = "INSERT_PERSOONLEIDER";

            //    AddParameterWithValue(com, "voornaam", tbFirstName.Text);
            //    AddParameterWithValue(com, "tussenvoegsel", tbInfix.Text);
            //    AddParameterWithValue(com, "achternaam", tbSurname.Text);
            //    AddParameterWithValue(com, "straat", tbStreet.Text);
            //    AddParameterWithValue(com, "huisnr", tbHouseNr.Text);
            //    AddParameterWithValue(com, "postcode", tbPostalCode.Text);
            //    AddParameterWithValue(com, "banknr", tbIban.Text);

            //    var q = com.CreateParameter();
            //    q.DbType = DbType.Decimal;
            //    q.ParameterName = "insertGelukt";
            //    q.Direction = ParameterDirection.Output;
            //    com.Parameters.Add(q);

            //    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
            //    con.Open();
            //    com.Connection = con;
            //    com.ExecuteNonQuery();

            //    string result = com.Parameters["insertGelukt"].Value.ToString();

            //    if (result == "0")
            //    {
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald')</script>");
            //    }
            //}

            //PERSOONID
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

            //ACCOUNT
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
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald')</script>");
                }
                accountId = com.Parameters["accountId"].Value.ToString();
            }

            

            //RESERVERING
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
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald')</script>");
                }
                reserveringId = com.Parameters["reserveringIdOUT"].Value.ToString();
            }
            //reservering polsbandje
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

                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                string result = com.Parameters["insertGelukt"].Value.ToString();
                if (result == "0")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Database insert gefaald')</script>");
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

        protected void btnRemove_Click(object sender, EventArgs e)
        {

        }
    }
}