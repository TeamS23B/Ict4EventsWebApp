using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ict4EventsWebApp
{
    public partial class MateriaalVerhuur : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //todo add everything to rent
            lbProducten.Items.Clear();
            lbProducten.Items.Add("Test Item");
            lbProducten.Items.Add("Test Item");
            lbProducten.Items.Add("Test Item");
            lbProducten.Items.Add("Test Item");
            lbProducten.Items.Add("Test Item");
        }

        protected void btSubmit_Click(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Session["aapje"] = 0;
            }
            lblBar.Text = ((int)Session["aapje"]).ToString();
            Session["aapje"]=(int)Session["aapje"]+1;
            lbProducten.Items.Add(clEndDate.SelectedDate.ToString());
        }
    }
}