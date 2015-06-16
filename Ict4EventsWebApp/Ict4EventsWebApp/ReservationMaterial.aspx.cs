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
        }

        

        protected void Button1_Click(object sender, EventArgs e)
        {
            pnlMaterial.Visible = true;

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
            string a = XValue.Value;
            string b = YValue.Value;

            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                pnlMap.Visible = true;
            }
        }

        protected void btRMAterialVerder_Click(object sender, EventArgs e)
        {
            pnlOverview.Visible = true;
        }
    }
}