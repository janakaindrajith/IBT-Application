using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace BranchMIS.IBT
{
    public partial class IBT_Reports : System.Web.UI.Page
    {
        private string usrName = "";
        public int usr_role = 0;
        public DataTable dtPageControls = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IBT_UserName"] == null)
            {
                string usrValid = "SessionExpired";
                Response.Redirect("~/FAS_Home.aspx?usr=" + usrValid, false);
                return;
            }


            try
            {
                CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
                //usrName = clsCom.getCurentUser();
                usrName = Session["IBT_UserName"].ToString();


                Session["PagePermissions"] = null;
                string pageName = (System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath));

                dtPageControls = clsCom.getUserRoleAndPageControls(usrName, pageName);
                Session["PagePermissions"] = dtPageControls;

                bool x = clsCom.getPermissions(usrName, pageName);

                if (x == true)
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            //disableBatchButtons();
                            //formControlsAuthentication();

                            string ReportName = "FAS_IBT_Excel_Export";//FAS_IBT_Excel_Export.rdl
                            string UserName = usrName;// "deshapriya.sooriya";
                            //ltrlExcelExport.Text = "<iframe src=\"http://sourcesafe:42000/ReportServer_STNDDBSQL/Pages/ReportViewer.aspx?%2fHNBA_MIS%2f" + ReportName + "&rs:Command=Render&UserID=" + UserName + "\" width=\"100%\" height=\"600\" frameborder=\"0\" id=\"IFRAME1\">";
                            
                            //Working
                            //ltrlExcelExport.Text = "<iframe src=\"http://sourcesafe:42000/reportserver_stnddbsql/pages/reportviewer.aspx?%2fhnba_mis%2f" + ReportName + "&rs:command=render\" width=\"100%\" height=\"600\" frameborder=\"0\" id=\"iframe1\">";


                            ltrlExcelExport.Text = "<iframe src=\"http://sourcesafe:42000/reportserver_stnddbsql/pages/reportviewer.aspx?%2fhnba_mis%2f" + ReportName + "&rs:command=render&UserID=" + UserName + "\" width=\"100%\" height=\"600\" frameborder=\"0\" id=\"iframe1\">";

                        }


                    }
                    catch (Exception ex)
                    {
                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                        //lblResult.Text = ex.InnerException.ToString();
                    }
                }
                else
                {
                    Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }


            catch (Exception ex)
            {

                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                throw;
            }
        }
    }
}
