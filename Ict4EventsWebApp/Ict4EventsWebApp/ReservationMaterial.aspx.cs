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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlMaterial.Visible = false;
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {

                
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "Verdubbel";
                var p = com.CreateParameter();
                p.DbType = DbType.Decimal;
                p.ParameterName = "teVerdubbelen";
                p.Value = Convert.ToInt32(voorbeeld.Text);
                com.Parameters.Add(p);

                var q = com.CreateParameter();
                q.DbType = DbType.Decimal;
                q.ParameterName = "resultaat";
                q.Direction = ParameterDirection.Output;
                com.Parameters.Add(q);

                //com.Parameters.Add("teVerdubbelen", OracleDbType.Decimal).value = Convert.ToInt32(TextBox1.Text);
                //com.Parameters.Add("resultaat", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                try
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    con.Open();
                    com.Connection = con;
                    com.ExecuteNonQuery(); //Voert de stored procedure uit

                    //Label1.Text = "De verdubbelde waarde is: " + com.Parameters["resultaat"].Value;
                    //Tonen van de waarde in de resultaat parameter na uitvoeren van de stored procedure
                    //label1.   .Show("De verdubbelde waarde is: " + objCmd.Parameters["resultaat"].Value);
                }
                catch (Exception ex)
                {
                    //Voor het geval "iets" mis gaat, de letterlijke foutmelding tonen (doe je natuurlijk niet in een "echte" applicatie)
                    //MessageBox.Show("De volgende fout is opgetreden: " + ex.ToString());
                }
                //Verbinding sluiten (waarschijnlijk doe je dit in je applicatie niet per database commando)
                con.Close();
            }
        }

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            
        }

        protected void btCMaterialVerder_Click(object sender, EventArgs e)
        {
            string a = XValue.Value;
            string b = YValue.Value;





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

                leel.Text = "plek: " + com.Parameters["PlekId"].Value;
            }
        }
    }
}