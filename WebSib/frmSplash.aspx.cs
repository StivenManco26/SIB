using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webSib
{
    public partial class frmSplash : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void tmrSplash_Tick(object sender, EventArgs e)
        {
            this.tmrSplash.Enabled = false;
            Response.Redirect("frmLogin.aspx");
        }

    }
}