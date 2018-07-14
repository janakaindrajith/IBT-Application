//using Microsoft.SqlServer.Management.Common;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Collections;

namespace BranchMIS.IBT
{
    public partial class IBT_RuleProcessing : System.Web.UI.Page
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
                        disableBatchButtons();
                        formControlsAuthentication();
                    }
                    //LoadData(Convert.ToString(ddlCategory.SelectedValue));

                }
                catch (Exception ex)
                {
                    CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                    lblResult.Text = ex.InnerException.ToString();
                }
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }


        //-----------------------------------------------------------------------Rule Engine Execute----------------------------------------------------------------------//

        protected void cmdBifurcation_Click(object sender, EventArgs e)//------------------------------Execute IBT Bifurcation Rules--------------------------------------//
        {
            try
            {
                CommonCLS.CommonFunctions.IBT_Bifurcate(Session["IBT_Company"].ToString());//Check IBT or NonIBT and update accordingly
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----Bifurcation Rules Executed---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(true);
                //page_result.Visible = true;
                lblResult.Text = "Successfully Birfurcated...";
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                //page_result.Visible = true;
                lblResult.Text = ex.Message.ToString();
            }
        }

        protected void cmdMatching_Click(object sender, EventArgs e)//---------------------------------Execute Matching Rules---------------------------------------------//
        {
            try
            {
                CommonCLS.CommonFunctions.IBT_Matching(Session["IBT_Company"].ToString());//Check Matching status Exact / Possible No / Unmatch
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----Matching Rules Executed---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(true);
                lblResult.Text = "Successfully Matched...!";
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.Message.ToString();
            }
        }

        protected void cmdDeptDecompose_Click(object sender, EventArgs e)//----------------------------Execute Department Decompose Rules---------------------------------//
        {
            try
            {
                CommonCLS.CommonFunctions.IBT_DEPT_DECOMPOSE(Session["IBT_Company"].ToString());//Decompose MCR / MRP / NON MOTOR / MOTOR
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----Decomposed Rules Executed---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(true);
                //page_result.Visible = true;
                lblResult.Text = "Successfully Decomposed...";
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                //page_result.Visible = true;
                lblResult.Text = ex.Message.ToString();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //-----------------------------------------------------------------------Receipt Batches Execute------------------------------------------------------------------//
        
        //protected void cmdGeneralbatch_Click(object sender, EventArgs e)//-----------------------------General Bacth Click Event------------------------------------------//
        //{
        //    try
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----General Batch Executed---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        Thread thread = new Thread(new ThreadStart(GeneralBatchExecute));
        //        thread.Start();
        //    }
        //    catch (Exception ex)
        //    {

        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(false);
        //        lblResult.Text = ex.InnerException.ToString();
        //    }
        //}

        //private void GeneralBatchExecute()//-----------------------------------------------------------General Batch Execute----------------------------------------------//
        //{
        //    try
        //    {
        //        BranchMIS.CommonCLS.IBTBatches.GeneralBatch();
        //        BranchMIS.CommonCLS.IBTEmails.SendEmails("General");
                
        //        CommonCLS.IBTEmails.EmailAlertCommon(8,null);

        //        //CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----General Batch Executed---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(true);
        //        lblResult.Text = "General Batch Executed...";

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //protected void cmdLifebatch_Click(object sender, EventArgs e)//--------------------------------Life Batch Click Event---------------------------------------------//
        //{
        //    try
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----Life Batch Executed---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        Thread thread = new Thread(new ThreadStart(LifeBatchExecute));
        //        thread.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(false);
        //        lblResult.Text = ex.InnerException.ToString();
        //    }
        //}

        //private void LifeBatchExecute()//--------------------------------------------------------------Life Batch Execute-------------------------------------------------//
        //{
        //    try
        //    {
        //        BranchMIS.CommonCLS.IBTBatches.LifeBatch();
        //        //BranchMIS.CommonCLS.IBTEmails.SendEmails("Life");
                
        //        CommonCLS.IBTEmails.EmailAlertCommon(6,null);

        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----Life Batch Executed---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(true);
        //        lblResult.Text = "Life Batch Executed...";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //protected void cmdNonTCSBatch_Click(object sender, EventArgs e)//------------------------------Non TCS Batch Click Event------------------------------------------//
        //{
        //    try
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----NON TCS Batch Executed---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        Thread thread = new Thread(new ThreadStart(NonTCSBatchExecute));
        //        thread.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(false);
        //        lblResult.Text = ex.InnerException.ToString();
        //    }
        //}

        //private void NonTCSBatchExecute()//------------------------------------------------------------Non TCS Batch Execute----------------------------------------------//
        //{
        //    try
        //    {
        //        BranchMIS.CommonCLS.IBTBatches.NonTCSBatch();
        //        //BranchMIS.CommonCLS.IBTEmails.SendEmails("NonTCS");
                
        //        CommonCLS.IBTEmails.EmailAlertCommon(7,null);

        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----NonTCS Batch Executed---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(true);
        //        lblResult.Text = "NonTCS Batch Executed...";

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //----------------------------------------------------------------------Common Controls---------------------------------------------------------------------------//

        public void call_error_msg(bool x)//------------------------------------------------------------------System Messeges----------------------------------------------//
        {
            if (x == true)
            {
                this.page_result.Visible = true;
                this.page_result.Style.Clear();
                page_result.Attributes["class"] = "alert alert-success";

                //this.page_result.Style.Add("background-color", "#32CD32");
                //this.page_result.Style.Add("border-color", "#32CD32");
                lblResult.Visible = true;
            }
            else
            {
                this.page_result.Visible = true;
                this.page_result.Style.Clear();
                page_result.Attributes["class"] = "alert alert-danger";

                //this.page_result.Style.Add("background-color", "#FF0000");
                //this.page_result.Style.Add("border-color", "#FF0000");
                lblResult.Visible = true;
                this.lblResult.Style.Add("color", "#ffff");
            }
        }

        protected void disableBatchButtons()//---------------------------------------------------------Disable All Batch Buttons------------------------------------------//
        {
            //cmdGeneralbatch.Enabled = false;
            //cmdLifebatch.Enabled = false;
            //cmdNonTCSBatch.Enabled = false;
        }

        protected void formControlsAuthentication()//--------------------------------------------------Form Authentications (Enable User Controls)------------------------//
        {
            //DataTable dt_formControls = (DataTable)Session["PagePermissions"];

            DataRow[] result = dtPageControls.Select("page_id =70");

            foreach (DataRow row in result)
            {
                //string pageid = row[0].ToString();
                string field_name = row[1].ToString();

                if (field_name == "GENERAL_RECEIPT")
                {
                    //cmdGeneralbatch.Enabled = true;
                }
                else if (field_name == "LIFE_RECEIPT")
                {
                    //cmdLifebatch.Enabled = true;
                }
                else if (field_name == "NON_TCS_RECEIPT")
                {
                    //cmdNonTCSBatch.Enabled = true;
                }
                else
                {
                    disableBatchButtons();
                    return;
                }

                field_name = "";
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------//
        
    }
}