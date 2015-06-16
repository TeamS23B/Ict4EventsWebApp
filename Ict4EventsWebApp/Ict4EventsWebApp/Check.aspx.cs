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
    public partial class Check1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Inserting parameters
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
        /// Check existence of barcode, check payed and change available at event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                com.CommandText = "SELECT reservering.betaald, reservering_polsbandje.aanwezig, reservering_polsbandje.id FROM polsbandje INNER JOIN reservering_polsbandje ON polsbandje.id = reservering_polsbandje.polsbandje_id INNER JOIN reservering ON reservering_polsbandje.reservering_id = reservering.id WHERE polsbandje.barcode = :1 AND rownum = 1";
                AddParameterWithValue(com, "barc", (string)tbBarcode.Text);
                DbDataReader reader = com.ExecuteReader();
                lblBarcodeObject.Text = tbBarcode.Text;
                try
                {
                    lblBetaaldobject.Text = "Barcode bestaat niet";
                    while (reader.Read())
                    {
                        if ((short)reader[0] == 0)
                        {
                            lblBetaaldobject.Text = "Niet betaald";

                        }
                        else
                        {
                            lblBetaaldobject.Text = "Betaald";
                            if ((short)reader[1] == 0)
                            {
                                updateAanwezigheid(1, (long)reader[2]);
                                lblAanwezigObject.Text = "U bent nu aanwezig";
                            }
                            else
                            {
                                updateAanwezigheid(0, (long)reader[2]);
                                lblAanwezigObject.Text = "U bent niet meer aanwezig";
                            }
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    lblBarcodeObject.Text = "Barcode is niet gevonden";
                }
            }
            tbBarcode.Text = string.Empty;
            tbBarcode.Focus();
        }

        /// <summary>
        /// Change availability in the database
        /// </summary>
        /// <param name="aanwezig"></param>
        /// <param name="resId"></param>
        /// <returns></returns>
        private int updateAanwezigheid(short aanwezig, long resId)
        {
            using (DbConnection con = OracleClientFactory.Instance.CreateConnection())
            {
                if (con == null)
                {
                    return 0;
                }
                con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                con.Open();
                DbCommand com = OracleClientFactory.Instance.CreateCommand();
                if (com == null)
                {
                    return 0;
                }
                com.Connection = con;
                com.CommandText = "UPDATE reservering_polsbandje SET aanwezig = :1 WHERE id = :2";
                AddParameterWithValue(com, "aanwezig", aanwezig);
                AddParameterWithValue(com, "resId", resId);
                return com.ExecuteNonQuery();
            }
        }
    }
}