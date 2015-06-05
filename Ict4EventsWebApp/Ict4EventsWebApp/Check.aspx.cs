﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ict4EventsWebApp
{
    public partial class Check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            if (Authentication.Instance.IsAuthenticated(tbUsername.Text, tbPassword.Text))
            {
                lblUsername.Text = "Success!";
            }
            else
            {
                lblUsername.Text = "False!";
            }
        }
    }
}