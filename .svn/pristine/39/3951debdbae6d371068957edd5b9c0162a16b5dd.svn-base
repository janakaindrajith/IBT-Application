using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;

namespace BranchMIS.IBT
{
    public partial class IBT_Upload_Config : System.Web.UI.Page
    {
        public static string ora_error = "";
        public static string pageLoad_error = "";
        private string usrName = "";
        public int usr_role = 0;

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


            string pageName = (System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath));

            bool x = clsCom.getPermissions(usrName, pageName);

            if (x == true)
            {
                if (pageLoad_error != "")
                {
                    call_error_msg(false);
                    lblResult.Text = "Page Load Error! " + " " + pageLoad_error;
                    pageLoad_error = "";
                }
                if (!IsPostBack)
                {
                    IBTAccountsFor_ddl();
                    getAvailableIBTFormats();
                    getAvailableIBTAccounts();
                    getAssignedFormats();

                }
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }

        }

        //----------------------------------------------------------IBT Formats------------------------------------------------------------//

        protected void btnInsertFormat_Click(object sender, EventArgs e)//Add New IBT Format Button Click Event
        {
            try
            {
                if (txtFormatDescription.Text != "")
                {
                    //string activeUsr = usrName;
                    string y = txtFormatDescription.Text;
                    int AutoDetect = 0;

                    if(cbPolNoAuto.Checked == true)
                    {
                        AutoDetect = 1;
                    }
                                       

                    bool valid = false;
                    valid = insertNewFormat(y, AutoDetect);

                    if (valid != false)
                    {                       
                        call_error_msg(true);
                        //lblResult.ForeColor = System.Drawing.Color.Green;
                        lblResult.Text = "IBT Format Insertion Successfull...!";
                        ora_error = "";
                        getAvailableIBTFormats();
                        IBTAccountsFor_ddl();
                        ClearInputs(Page.Controls);         
                        cbPolNoAuto.Checked = false;
                        //txtFormatDescription.Text = "";
                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name,"----New IBT Format Inserted---- By: "+ usrName , Server.MapPath("~/IBTLogFiles/Log.txt"));
                    }
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "Fields Can not be empty!";
                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                if (ex.Message.Contains("ORA-00001"))
                {
                    ora_error = "IBT Format already exists in the database...!";
                    call_error_msg(false);
                    lblResult.Text = ora_error;
                    ora_error = "";
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "Error Occured...!";
                }
            }
        }

        public bool insertNewFormat(string description, int auto_detect_polNo)//Add New IBT Format
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_NewIBTFormat";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("vDescription", OracleType.VarChar).Value = description.ToUpper();
                cmd.Parameters.AddWithValue("vInactive", OracleType.Int32).Value = 0;
                cmd.Parameters.AddWithValue("vPolNo_Auto", OracleType.Int32).Value = auto_detect_polNo;
                cmd.Parameters.Add("vUserName", OracleType.VarChar).Value = usrName;

                cmd.ExecuteNonQuery();


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void getAvailableIBTFormats()//Data Bind - IBT Formats (Grid View ID :grdUpdateFormat)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select t.description, t.id, t.inactive from FAS_IBT_FORMATS t"; //where t.inactive = 0";

                OracleDataReader odr = cmd.ExecuteReader();
                while (odr.Read())
                {
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    grdUpdateFormats.DataSource = dt;
                    grdUpdateFormats.DataBind();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                pageLoad_error = ex.ToString();
                //throw;
            }

        }

        protected void grdUpdateFormats_RowDataBound(object sender, GridViewRowEventArgs e)//IBT Formats Active/ Inactive Row Data Bound (Grid View ID :grdUpdateFormat)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int status_value = int.Parse(DataBinder.Eval(e.Row.DataItem, "Inactive").ToString());
                int f_id = int.Parse(DataBinder.Eval(e.Row.DataItem, "ID").ToString());

                if (status_value == 0)
                {
                    CheckBox chk = e.Row.FindControl("chkStatus") as CheckBox;
                    chk.Checked = true;
                }
            }
        }

        protected void chkStatus_CheckedChanged(object sender, EventArgs e)//IBT Formats Active/ Inactive (Grid View ID :grdUpdateFormat)
        {
            try
            {
                if(page_result.Visible == true)
                {
                    page_result.Visible = false;
                }

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();

                CheckBox chkbox = (CheckBox)sender;
                GridViewRow grd = (GridViewRow)chkbox.NamingContainer;

                int v_Status = -1;
                int temp_id = Convert.ToInt32(grd.Cells[0].Text);
                
                //Label description = (Label)grd.Cells[1].FindControl("lblDescription");
                //string temp_description = description.Text;

                if (((CheckBox)sender).Checked)
                {
                    v_Status = 0;
                }
                else
                {
                    v_Status = 1;
                }

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update fas_ibt_formats t set inactive=" + v_Status + "where ID =" + temp_id;
                //cmd.CommandText = "SP_FAS_IBT_Update_IBTFormat";
                cmd.ExecuteNonQuery();

                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----IBT Format Current State Changed...! (Active / Inactive) ---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                getAvailableIBTFormats();
                IBTAccountsFor_ddl();
                getAssignedFormats();//new
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                pageLoad_error = "Check Box Check Changed Event Error...!" + ex.ToString();
                //throw;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------//

        //----------------------------------------------------------IBT Accounts------------------------------------------------------------//

        protected void btnInsertAccount_Click(object sender, EventArgs e)//Add New IBT Account Button Click Event
        {
            try
            {
                if ((txtAccountNo.Text != "") && (txtAccountName.Text != "")&&(ddlProduct.SelectedIndex!=0) && (txtCashAcc.Text!= ""))
                {
                    string x = txtAccountNo.Text;
                    string y = txtAccountName.Text;
                    string cashAcc = txtCashAcc.Text;
                    int product = int.Parse(ddlProduct.SelectedValue);

                    bool valid = false;
                    valid = insertNewAccount(x, y, cashAcc, product);

                    if (valid != false)
                    {
                        call_error_msg(true);
                        lblResult.Text = "IBT Account Insertion Successfull...!";
                        getAvailableIBTAccounts();
                        IBTAccountsFor_ddl();
                        ClearInputs(Page.Controls);
                        //txtAccountName.Text = "";
                        //txtAccountNo.Text = "";
                        //txtCashAcc.Text = "";
                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----New IBT Account Inserted---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                    }                    
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "Fields Can not be empty!";
                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                if (ex.Message.Contains("ORA-00001"))
                {
                    ora_error = "IBT Account already exists in the database...!";
                    call_error_msg(false);
                    lblResult.Text = ora_error;
                    ora_error = "";
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "Error Occured...!";
                }
            }


        }

        public bool insertNewAccount(string accNo, string accName, string cashAcc, int product)//Add New IBT Account
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_newIbtAccount";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("vAccNo", OracleType.VarChar).Value = accNo.ToUpper().Trim();
                cmd.Parameters.Add("vAccName", OracleType.VarChar).Value = accName.ToUpper();
                cmd.Parameters.AddWithValue("vInactive", OracleType.Int32).Value = 0;
                cmd.Parameters.Add("vCashAcc", OracleType.VarChar).Value = cashAcc.ToUpper().Trim();
                cmd.Parameters.Add("vProduct", OracleType.Int32).Value = product;
                cmd.Parameters.Add("vUserName", OracleType.VarChar).Value = usrName;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IBTAccountsFor_ddl()//Data binding for dropdownlists
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();//IBT Accounts
                cmd.CommandText = "select t.acc_no || '-' || t.acc_name  as accWithName, t.acc_no from fas_ibt_accounts t where t.inactive = 0";//|| '-' || t.product
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                ddlAccounts.DataTextField = "accWithName";
                ddlAccounts.DataValueField = "acc_no";
                ddlAccounts.DataSource = dt;
                ddlAccounts.DataBind();

                ddlAccounts.Items.Insert(0, new ListItem("---Please Select---", "0"));


                OracleCommand cmd2 = conn.CreateCommand();//IBT Formats
                cmd2.CommandText = "select * from fas_ibt_formats where inactive = 0";
                OracleDataAdapter oda2 = new OracleDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                oda2.Fill(dt2);

                ddlFormats.DataTextField = "description";
                ddlFormats.DataValueField = "id";
                ddlFormats.DataSource = dt2;
                ddlFormats.DataBind();

                ddlFormats.Items.Insert(0, new ListItem("---Please Select---", "0"));


                OracleCommand cmd3 = conn.CreateCommand();//IBT Products
                cmd3.CommandText = "select * from fas_ibt_products";
                OracleDataAdapter oda3 = new OracleDataAdapter(cmd3);
                DataTable dt3 = new DataTable();
                oda3.Fill(dt3);

                ddlProduct.DataTextField = "Product_Description";
                ddlProduct.DataValueField = "value";
                ddlProduct.DataSource = dt3;
                ddlProduct.DataBind();

                ddlProduct.Items.Insert(0, new ListItem("---Please Select---", "0"));


                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                pageLoad_error = ex.ToString();
                //throw;
            }
        }

        public void getAvailableIBTAccounts()//Data Bind - IBT Accounts (Grid View ID :grdIBTAccounts)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select t.acc_no, t.acc_name,t.inactive, t.cash_account, tt.product_description from fas_ibt_accounts t inner join fas_ibt_products tt on t.product = tt.value order by t.acc_name"; //"select t.acc_no, t.acc_name,t.inactive, t.cash_account, t.product from fas_ibt_accounts t order by t.acc_name";//where t.inactive = 0";

                OracleDataReader odr = cmd.ExecuteReader();
                while (odr.Read())
                {
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    grdIBTAccounts.DataSource = dt;
                    grdIBTAccounts.DataBind();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                pageLoad_error = ex.ToString();
                //throw;
            }
        }

        protected void grdIBTAccounts_RowDataBound(object sender, GridViewRowEventArgs e)//IBT Accounts Active/ Inactive Row Data Bound (Grid View ID :grdIBTAccounts)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int status_value = int.Parse(DataBinder.Eval(e.Row.DataItem, "Inactive").ToString());
                string acc_no = (DataBinder.Eval(e.Row.DataItem, "ACC_NO").ToString());

                if (status_value == 0)
                {
                    CheckBox chk = e.Row.FindControl("chkChangeStatus") as CheckBox;
                    chk.Checked = true;
                }
            }
        }

        protected void chkChangeStatus_CheckedChanged(object sender, EventArgs e)//IBT Accounts Active/ Inactive (Grid View ID :grdIBTAccounts)
        {
            try
            {
                if(page_result.Visible == true)
                {
                    page_result.Visible = false;
                }

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();

                CheckBox chkbox = (CheckBox)sender;
                GridViewRow grd_acc = (GridViewRow)chkbox.NamingContainer;

                int v_Status = -1;
                string temp_accNo = grd_acc.Cells[0].Text.Trim();

                if (((CheckBox)sender).Checked)
                {
                    v_Status = 0;
                }
                else
                {
                    v_Status = 1;
                }

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update fas_ibt_accounts t set inactive=" + v_Status + "where ACC_NO = " + "'" + temp_accNo + "'";
                cmd.ExecuteNonQuery();
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----IBT Account Current State Changed...! (Active / Inactive) ---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                getAvailableIBTAccounts();
                IBTAccountsFor_ddl();
                getAssignedFormats();//new
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                pageLoad_error = "Check Box Changed Event Error" + ex.ToString();
                //throw;
            }
        }

        
        //----------------------------------------------------------------------------------------------------------------------------------//

        //--------------------------------------------------Assign Format To IBT Accounts---------------------------------------------------//      

        protected void btnAssign_Click(object sender, EventArgs e)//Assign IBT Format Click Event
        {
            try
            {
                if ((ddlAccounts.SelectedIndex != 0) && (ddlFormats.SelectedIndex != 0))
                {
                    string x = ddlAccounts.SelectedValue.ToString();//acc no
                    int y = int.Parse(ddlFormats.SelectedValue.ToString());//format id

                    bool valid = false;
                    valid = assignFormats(x, y);

                    if (valid == true)
                    {
                        call_error_msg(true);
                        lblResult.Text = "IBT Format Assigned Successfully...!";
                        getAssignedFormats();
                        ClearInputs(Page.Controls);
                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----IBT Format Assigned to IBT Account---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                    }
                   
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "Both Account Number and IBT Format Should be selected!";
                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                if (ex.Message.Contains("ORA-00001"))
                {
                    ora_error = "Format Already Assigned To Selected Account...! Unique Constraint Violated.";
                    call_error_msg(false);
                    lblResult.Text = ora_error;
                    ora_error = "";
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "Error Occured...!";
                }
            }

        }

        public bool assignFormats(string accNo, int formatId)//Assign IBT Format to IBT Account
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_AssignFormat";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("vAccNo", OracleType.VarChar).Value = accNo;
                cmd.Parameters.Add("vFormatId", OracleType.VarChar).Value = formatId;
                cmd.Parameters.AddWithValue("vInactive", OracleType.Int32).Value = 0;
                cmd.Parameters.Add("vUserName", OracleType.VarChar).Value = usrName;

                cmd.ExecuteNonQuery();
                conn.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void getAssignedFormats()//Data Bind - IBT Assigned Formats (Grid View ID :grdAssignedFormats)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                //cmd.CommandText = "select t.acc_no, t.format_id, tt.description from fas_ibt_accformats t inner join fas_ibt_formats tt on t.format_id = tt.id where t.effective_end_date is null";

                cmd.CommandText = "select t.acc_no, t.format_id, tt.description  from fas_ibt_accformats t " +
                                  "inner join fas_ibt_formats tt on t.format_id = tt.id " +
                                  "inner join fas_ibt_accounts a on a.acc_no = t.acc_no " +
                                  "where t.effective_end_date is null and tt.inactive = 0 and a.inactive = 0";

                OracleDataReader odr = cmd.ExecuteReader();
                while (odr.Read())
                {
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    grdAssignedFormats.DataSource = dt;
                    grdAssignedFormats.DataBind();
                }
                odr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                pageLoad_error = ex.ToString();
                //throw;
            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------//      

        //--------------------------------------------------Reset All Controls--------------------------------------------------------------//
        private void ClearInputs(ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                else if (ctrl is DropDownList)
                    ((DropDownList)ctrl).ClearSelection();
               

                ClearInputs(ctrl.Controls);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//  

        //--------------------------------------------------Error Msg Setings--------------------------------------------------------------//
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
        //----------------------------------------------------------------------------------------------------------------------------------// 
    }
}
