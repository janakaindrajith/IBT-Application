using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using BranchMIS.CommonCLS;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;

namespace BranchMIS
{
    public partial class FAS_Home : System.Web.UI.Page
    {
        OracleConnection myConnectionMain = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        private string UserName = "";
        private string UserCompany = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonFunctions cmnFunctions = new CommonFunctions();
            UserName = Context.User.Identity.Name;//Get AD Name

            if(UserName.Contains("HNBGI"))
            {
                UserName = cmnFunctions.Right(UserName, (UserName.Length) - 6);
            }
            else
            {
                UserName = cmnFunctions.Right(UserName, (UserName.Length) - 5);
            }

            
            
            //lbl_UserName.Text = UserName;
            CreateUser_Sessions();

            if(!IsPostBack)
            {
                string x = "";
                string y = "";

                x = Request.QueryString["valid"];
                y = Request.QueryString["usr"];

                if ((x == null) || (x == ""))
                {
                    if ((y == null) || (y == ""))
                    {
                        systemMessege.Visible = false;
                    }
                    else if (y == "SessionExpired")
                    {
                        systemMessege.Visible = true;
                        lblLoginError.Text = "User Session Expired...!";
                    }
                    else
                    {
                        systemMessege.Visible = false;
                    }

                    
                }
                else if (x == "False")
                {
                    systemMessege.Visible = true;
                    lblLoginError.Text = "Access Denied...!";
                }
                else
                {
                    systemMessege.Visible = true;
                    lblLoginError.Text = "Access Denied...!";
                }
                


                

                //if((y==null)||(y==""))
                //{
                //    systemMessege.Visible = false;
                //}
                //else if (y == "SessionExpired")
                //{
                //    systemMessege.Visible = true;
                //    lblLoginError.Text = "User Session Expired...!";
                //}
                //else
                //{
                //    systemMessege.Visible = true;
                //    lblLoginError.Text = "Access Denied...!";
                //}




                if(Session["IBT_UserName"]!=null)
                {
                    string EmailPrifix = WebConfigurationManager.AppSettings["EmailSubjectPrifix"].ToString();

                    lbl_ADName.Text = Session["IBT_UserName"].ToString() + " - " + EmailPrifix;
                }





                get_availablePages();
            }
            
        }

        public void CreateUser_Sessions()
        {
            if(UserName!="")
            {
                myConnectionMain.Close();
                myConnectionMain.Open();

                OracleCommand cmd_get_UserCompany = myConnectionMain.CreateCommand();
                cmd_get_UserCompany.CommandText = "SELECT UR.COMPANY FROM FAS_IBT_USR_ROLES UR WHERE UR.ROLE_ID =(SELECT R.ROLE_ID FROM FAS_IBT_ROLE_ASSIGNED R WHERE R.USER_NAME = '"+ UserName +"')";

                OracleDataReader odr_get_UserCompany = cmd_get_UserCompany.ExecuteReader();
                while(odr_get_UserCompany.Read())
                {
                    UserCompany = odr_get_UserCompany["COMPANY"].ToString();
                }

                Session["IBT_UserName"] = UserName;
                Session["IBT_Company"] = UserCompany;
            }
        }

        private void get_availablePages()
        {
            myConnectionMain.Close();
            myConnectionMain.Open();

            OracleCommand cmd_getPages = myConnectionMain.CreateCommand();
            cmd_getPages.CommandText = "SELECT U.PER_ID FROM FAS_IBT_USER_PERMISSIONS U WHERE U.ROLE_ID = (SELECT R.ROLE_ID FROM FAS_IBT_ROLE_ASSIGNED R WHERE R.USER_NAME = '" + Session["IBT_UserName"].ToString() + "')";


            OracleCommand cmd_AllPages = myConnectionMain.CreateCommand();
            cmd_AllPages.CommandText = "select id from fas_ibt_permissions t";

            OracleDataReader odr_AllPages = cmd_AllPages.ExecuteReader();
            DataTable dt_AllPages = new DataTable();

            if (odr_AllPages.HasRows == true)
            {
                dt_AllPages.Load(odr_AllPages);
            }

            string[] page_url_ids = { "h10", "h20", "h30", "h40", "h50", "h60", "h70", "h80", "h90", "h100", "h110", "h120", "h130", "h140", "h150", "h160", "h170", "h180", "h190", "h200", "h210" };

            foreach (DataRow rows in dt_AllPages.Rows)
            {
                string page_id = rows["ID"].ToString();
                page_id = "h" + page_id;

                HyperLink tb = (HyperLink)Page.FindControl(page_id);
                if (tb == null)
                {
                    continue;
                }
                else
                {
                    tb.Enabled = false;
                    tb.Visible = false;
                }
            }
            

            OracleDataReader odr_getPages = cmd_getPages.ExecuteReader();
            DataTable dt_Allowed_pages = new DataTable();

            if (odr_getPages.HasRows == true)
            {
                dt_Allowed_pages.Load(odr_getPages);
            }

            foreach (DataRow allowed_ids in dt_Allowed_pages.Rows)
            {
                string allowed_page_id = allowed_ids["PER_ID"].ToString();
                allowed_page_id = "h" + allowed_page_id;

                HyperLink tb_l = (HyperLink)Page.FindControl(allowed_page_id);
                if (tb_l == null)
                {
                    continue;
                }
                else
                {
                    tb_l.Enabled = true;
                    tb_l.Visible = true;
                }
               
            }

        }
    }
}