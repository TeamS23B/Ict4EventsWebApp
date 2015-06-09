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
    public partial class Check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                com.CommandText = "SELECT reservering.betaald FROM polsbandje INNER JOIN reservering_polsbandje ON polsbandje.id = reservering_polsbandje.polsbandje_id INNER JOIN reservering ON reservering_polsbandje.reservering_id = reservering.id WHERE polsbandje.barcode = :1";
                AddParameterWithValue(com, "barc", (string)tbBarcode.Text);

                lblBarcodeObject.Text = tbBarcode.Text;
                try
                {
                    if ((short)com.ExecuteScalar() == 0)
                    {
                        lblBetaaldobject.Text = "Niet betaald";

                    }
                    else
                    {
                        lblBetaaldobject.Text = "Betaald";
                    }
                }
                catch (NullReferenceException)
                {
                    lblBarcodeObject.Text = "Barcode is niet gevonden";
                }
            }
        }
    }
}