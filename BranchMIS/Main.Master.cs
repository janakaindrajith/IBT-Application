using BranchMIS.CommonCLS;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SignalR;
using SignalR.Hosting.AspNet;
using SignalR.Infrastructure;


namespace BranchMIS
{
    public partial class Main : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //ltrlStyle.Text = "<link id=\"style_color\" href=\"../assets/css/themes/default.css\" rel=\"stylesheet\" type=\"text/css\">";

            if (Session["IBT_UserName"]!= null)
            {
                lbl_ADName.Text = Session["IBT_UserName"].ToString();
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx?usr=SessionExpired" , false);
                lbl_ADName.Text = "Authentication Fail...!";
            }

        }
    }             
}
