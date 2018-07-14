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
    public partial class IBT_Mails : System.Web.UI.Page
    {
        OracleConnection myConnectionUse = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());


        private string usrName = "";
        public int usr_role = 0;
        public DataTable dtPageControls = new DataTable();


        protected void Page_Load(object sender, EventArgs e)//Page Load Events
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
                if (!IsPostBack)
                {
                    getMailDescription();
                }
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                myConnectionUse.Close();
                myConnectionUse.Open();
                int mail_ID = -1;
                string mail_To = "";
                string mail_CC = "";
                string mail_DisplayName = "";
                string mail_Subject = "";
                string mail_body = "";
                string created_by = usrName;//"deshapriya.sooriya";

                if ((ddl_description.SelectedIndex != 0) && ((txt_CC.Text != "") || (txt_To.Text != null)) && (txt_displayName.Text != "") && (txt_subject.Text != "") && (txt_body.Text != ""))
                {
                    mail_ID = int.Parse(ddl_description.SelectedValue);
                    mail_To = txt_To.Text.Trim();
                    mail_CC = txt_CC.Text.Trim();
                    mail_DisplayName = txt_displayName.Text.Trim();
                    mail_Subject = txt_subject.Text.Trim();
                    mail_body = txt_body.Text.Trim();
                    //created_by = ""; 

                    OracleCommand cmd_addEmailDetails = myConnectionUse.CreateCommand();
                    cmd_addEmailDetails.CommandText = "SP_FAS_IBT_EMAILS";
                    cmd_addEmailDetails.CommandType = CommandType.StoredProcedure;

                    cmd_addEmailDetails.Parameters.Add("vEmailID", OracleType.Int32).Value = mail_ID;
                    cmd_addEmailDetails.Parameters.Add("vEmailToList", OracleType.VarChar).Value = mail_To;
                    cmd_addEmailDetails.Parameters.Add("vEmailCCList", OracleType.VarChar).Value = mail_CC;
                    cmd_addEmailDetails.Parameters.Add("vEmailDisplayName", OracleType.VarChar).Value = mail_DisplayName;
                    cmd_addEmailDetails.Parameters.Add("vEmailSubject", OracleType.VarChar).Value = mail_Subject;
                    cmd_addEmailDetails.Parameters.Add("vEmailBody", OracleType.VarChar).Value = mail_body;
                    cmd_addEmailDetails.Parameters.Add("vCreatedBy", OracleType.VarChar).Value = created_by;

                    cmd_addEmailDetails.ExecuteNonQuery();

                    call_error_msg(true);
                    lblResult.Text = "Command Execution Successfull..!";

                    myConnectionUse.Close();

                    ddl_description.ClearSelection();
                    txt_To.Text = "";
                    txt_CC.Text = "";
                    txt_displayName.Text = "";
                    txt_subject.Text = "";
                    txt_body.Text = "";
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "Command Execution Fail";
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
            
        }

        private void getMailDescription()
        {
            myConnectionUse.Open();
            OracleCommand cmd_getData = myConnectionUse.CreateCommand();
            cmd_getData.CommandText = "select d.id,d.email_desc from fas_ibt_email d order by d.id";

            DataTable dt = new DataTable();

            OracleDataAdapter oda = new OracleDataAdapter(cmd_getData);

            oda.Fill(dt);

            ddl_description.DataValueField = "id";
            ddl_description.DataTextField = "email_desc";

            ddl_description.DataSource = dt;
            ddl_description.DataBind();

            ddl_description.Items.Insert(0, new ListItem("--Please Select--", "-1"));
        }

        protected void ddl_description_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (page_result.Visible == true)
            {
                page_result.Visible = false;
            }
            else
            {
                page_result.Visible = false;
            }


            try
            {
                int email_id = -1;
                string _To = "";
                string _CC = "";
                string _displayName = "";
                string _subject = "";
                string _body = "";

                myConnectionUse.Close();
                myConnectionUse.Open();

                if (ddl_description.SelectedIndex != 0)
                {
                    email_id = int.Parse(ddl_description.SelectedValue);
                    OracleCommand cmd_getData = myConnectionUse.CreateCommand();
                    cmd_getData.CommandText = "select ea.email_to_list, ea.email_cc_list, ea.email_display_name, ea.email_subject, " +
                                                "ea.email_body from fas_ibt_email_alerts ea where ea.effective_end_date is null and ea.email_id = " + email_id;

                    OracleDataReader odr_getData = cmd_getData.ExecuteReader();

                    while (odr_getData.Read())
                    {
                        _To = odr_getData["email_to_list"].ToString();
                        _CC = odr_getData["email_cc_list"].ToString();
                        _displayName = odr_getData["email_display_name"].ToString();
                        _subject = odr_getData["email_subject"].ToString();
                        _body = odr_getData["email_body"].ToString();
                    }

                    txt_To.Text = _To;
                    txt_CC.Text = _CC;
                    txt_displayName.Text = _displayName;
                    txt_subject.Text = _subject;
                    txt_body.Text = _body;
                    //mpe.Show();
                }
                else
                {
                    return;
                }

                myConnectionUse.Close();
            }
            catch (Exception)
            {
                
                throw;
            }
            finally 
            {
                if(myConnectionUse.State!= null)
                {
                    myConnectionUse.Close();
                }
            }
            
        }

        //protected void btnMailInformation_Click(object sender, EventArgs e)
        //{
        //    mpe.Show();
        //}

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
    }
}