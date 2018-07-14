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
    public partial class IBT_Format_Configuration : System.Web.UI.Page
    {
        public static int tempID = 0;
        public static int tempRowCount = 0;
        public static int formatID = 0;
        public static int colID = 0;
        public static bool Filter = false;
        public static string pageLoad_Error = "";
        private string usrName = "";
        public int usr_role = 0;

        protected void Page_Load(object sender, EventArgs e)//-----------------------------Page Load----------------------------------------------------//
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


                lblResult.Text = "";
                if (pageLoad_Error != "")
                {
                    call_error_msg(false);
                    lblResult.Text = pageLoad_Error;
                    pageLoad_Error = "";
                }


                if (!IsPostBack)
                {
                    LoadIBTFormats();//dropdownlist data bind method call

                    if (tempID != 0)
                    {
                        this.BindGrid(tempID);
                    }
                }

                if (ddlIBTFormat.SelectedIndex == -1)
                {
                    grdInsert.DataSource = null;
                    grdInsert.DataBind();
                    //tblHdr.Visible = false;//Header Fixed
                    btnUpdate.Visible = false;
                    btnCancel.Visible = false;//new
                    btnInsert.Visible = false;//new
                }

                if (ddlIBTFormat.SelectedIndex == 0)
                {
                    //tblHdr.Visible = false;//Header Fixed
                    btnUpdate.Visible = false;
                    btnCancel.Visible = false;//new
                    btnInsert.Visible = false;//new
                    grdInsert.DataSource = null;
                    grdInsert.DataBind();
                }
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private void BindGrid(int fid)//---------------------------------------------------Grid View Data Bind------------------------------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                //OracleCommand cmd = conn.CreateCommand();

                int row_count = getRowCount();

                if (row_count > 0)
                {
                    tempRowCount = row_count;

                    OracleCommand cmd = new OracleCommand("select * from fas_ibt_upload_formats t inner join fas_ibt_columns tt on t.column_id = tt.id where t.format_id =" + fid, conn);
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);

                    grdInsert.DataSource = dt;
                    grdInsert.DataBind();

                    //tblHdr.Visible = true;//Header Fixed
                    btnUpdate.Visible = true;
                    btnCancel.Visible = true;
                    if (btnInsert.Visible == true)
                    {
                        btnInsert.Visible = false;
                    }
                    //Session["status"] = "Update";
                }
                else
                {
                    tempRowCount = row_count;

                    OracleCommand cmd2 = new OracleCommand("select t.id as column_id ,t.description as DESCRIPTION,  '0' as ROW_INDEX, '0' as COLUMNS_INDEX, '0' as COLUMN_ORDER, '0' as START_INDEX,'1' as Filter , '0' as END_INDEX, :formatid as Format_ID from fas_ibt_columns t", conn); //where t.format_id =" + fid);
                    cmd2.Parameters.AddWithValue("formatid", OracleType.Int32).Value = tempID;

                    OracleDataAdapter oda = new OracleDataAdapter(cmd2);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);

                    grdInsert.DataSource = dt;
                    grdInsert.DataBind();

                    //tblHdr.Visible = true;//Header Fixed
                    btnInsert.Visible = true;
                    btnCancel.Visible = true;
                    if (btnUpdate.Visible == true)
                    {
                        btnUpdate.Visible = false;
                    }
                    //Session["status"] = "Insert";
                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                pageLoad_Error = ex.ToString();
                
                //throw;
            }
        }

        public void LoadIBTFormats()//-----------------------------------------------------Drop down list data bind (IBT Formats)-----------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from fas_ibt_formats t where t.inactive = 0 order by t.id";
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                ddlIBTFormat.DataTextField = "description";
                ddlIBTFormat.DataValueField = "id";
                ddlIBTFormat.DataSource = dt;
                ddlIBTFormat.DataBind();

                ddlIBTFormat.Items.Insert(0, new ListItem("---Please Select---", "0"));
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                pageLoad_Error = ex.ToString();
                //throw;
            }
        }       

        protected void ddlIBTFormat_SelectedIndexChanged(object sender, EventArgs e)//-----Drop down list selected index changed (IBT Formats)----------//
        {
            if(ddlIBTFormat.SelectedIndex == 0 || ddlIBTFormat.SelectedIndex == -1)
            {
                btnCancel.Visible = false;
                btnInsert.Visible = false;
                btnUpdate.Visible = false;
            }
            if (page_result.Visible == true)
            {
                this.page_result.Visible = false;
            }

            if (ddlIBTFormat.SelectedIndex != 0)
            {
                int formatId = int.Parse(ddlIBTFormat.SelectedValue.ToString());

                tempID = formatId;

                if (tempRowCount > 0)
                {
                    tempRowCount = 0;
                    this.BindGrid(tempID);
                    //tblHdr.Visible = true;//Header Fixed
                }
                else if (tempRowCount == 0)
                {
                    this.BindGrid(tempID);
                   // tblHdr.Visible = true;//Header Fixed/////
                }
            }
            else
            {
                //tblHdr.Visible = false;//Header Fixed
                call_error_msg(false);
                lblResult.Text = "Please Select IBT Format First!";
            }
        }

        public int getRowCount()//---------------------------------------------------------Check Records availability-----------------------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select count(t.format_id) as rowCount from fas_ibt_upload_formats t where t.format_id=" + tempID;
                OracleDataReader odr = cmd.ExecuteReader();
                int rowCount = 0;
                while (odr.Read())
                {
                    rowCount = int.Parse(odr["rowCount"].ToString());
                }

                return rowCount;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)//-----------------------Insert Button Click Event------------------------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                OracleTransaction myTrans;
                conn.Open();
                myTrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                foreach (GridViewRow row in grdInsert.Rows)
                {
                    DropDownList ddlColIndex = (row.FindControl("ddlCol_Index") as DropDownList);
                    string a = ddlColIndex.SelectedItem.Text;

                    if (row.RowType == DataControlRowType.DataRow)
                    {
                       
                        //conn.Open();
                        OracleCommand cmd = new OracleCommand();//conn.CreateCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = myTrans;

                        cmd.CommandText = "SP_FAS_IBT_UploadFormatsUpdate";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("vFormatID", OracleType.Int32).Value = tempID;
                        cmd.Parameters.AddWithValue("vProcessType", OracleType.VarChar).Value = "tblInsert";
                        cmd.Parameters.AddWithValue("vColID", OracleType.Int32).Value = int.Parse(row.Cells[1].Text);
                        cmd.Parameters.AddWithValue("vDescription", OracleType.VarChar).Value = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                        cmd.Parameters.AddWithValue("vRowIndex", OracleType.Int32).Value = int.Parse(row.Cells[4].Controls.OfType<TextBox>().FirstOrDefault().Text);
                        cmd.Parameters.AddWithValue("vColumnsIndex", OracleType.Int32).Value = int.Parse(row.Cells[3].Controls.OfType<DropDownList>().FirstOrDefault().Text);
                        cmd.Parameters.AddWithValue("vColumnOrder", OracleType.VarChar).Value = a;
                        cmd.Parameters.AddWithValue("vStartIndex", OracleType.Int32).Value = int.Parse(row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text);
                        cmd.Parameters.AddWithValue("vEndIndex", OracleType.Int32).Value = int.Parse(row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Text);

                        cmd.ExecuteNonQuery();
                        //conn.Close();

                    }
                }
                myTrans.Commit();
                call_error_msg(true);            
                lblResult.Text = "Data Insertion Successfull...!";

                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----IBT Format Configured (Insert)---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));

                grdInsert.DataSource = null;
                grdInsert.DataBind();                
                ddlIBTFormat.SelectedIndex = -1;

                //tblHdr.Visible = false;//Header Fixed
                btnInsert.Visible = false;
                btnCancel.Visible = false;
                tempID = 0;
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Data Insertion Fail...!" + ex.ToString();
            }
        }

        protected void grdInsert_RowDataBound(object sender, GridViewRowEventArgs e)//-----Grid Row Data Bound------------------------------------------//
        {
            try
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;

                //GridViewRow row = grdInsert.Se;

                string description = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DESCRIPTION"));//row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                

                if (description == "TransactionRow")//Disable controls in row editing
                {
                    DropDownList ddlTrans = (e.Row.FindControl("ddlCol_Index") as DropDownList);
                    ddlTrans.Enabled = false;

                    TextBox txtStartIndex = (e.Row.FindControl("txtStartIndex") as TextBox);
                    txtStartIndex.Enabled = false;

                    TextBox txtEndIndex = (e.Row.FindControl("txtEndIndex") as TextBox);
                    txtEndIndex.Enabled = false;
                }

                if(description == "StartingRow")
                {
                    DropDownList ddlTrans = (e.Row.FindControl("ddlCol_Index") as DropDownList);
                    ddlTrans.Enabled = false;

                    TextBox txtStartIndex = (e.Row.FindControl("txtStartIndex") as TextBox);
                    txtStartIndex.Enabled = false;

                    TextBox txtEndIndex = (e.Row.FindControl("txtEndIndex") as TextBox);
                    txtEndIndex.Enabled = false;
                }


                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                String a_test = "";

                if (e.Row.DataItemIndex == -1)
                {
                    return;
                }

                if (tempRowCount > 0)
                {

                    formatID = int.Parse(e.Row.Cells[0].Text.ToString());//Convert.ToInt32((e.Row.FindControl("FORMAT_ID") as Label).Text);
                    colID = int.Parse(e.Row.Cells[1].Text.ToString()); //Convert.ToInt32((e.Row.FindControl("lblcolID") as Label).Text);

                    OracleCommand cmd = new OracleCommand();
                    conn.Close();
                    conn.Open();
                    cmd.Connection = conn;

                    cmd.CommandText = "select t.columns_index as cindex, t.column_order as corder from fas_ibt_upload_formats t where t.column_id = :colID and t.format_id = :formatID";
                    cmd.Parameters.AddWithValue("colID", OracleType.Int32).Value = colID;
                    cmd.Parameters.AddWithValue("formatID", OracleType.Int32).Value = formatID;
                    OracleDataReader odr = cmd.ExecuteReader();

                    while (odr.Read())
                    {
                        a_test = (odr["cindex"].ToString());
                    }
                    conn.Close();
                    odr.Close();

                    DropDownList ddlCol = (e.Row.FindControl("ddlCol_Index") as DropDownList);
                    if (a_test != "")
                    {
                        ddlCol.Items.FindByValue(a_test).Selected = true;
                    }

                    
                }

            }
            catch (Exception ex)
            {
                pageLoad_Error = "grdInsert_RowDataBound Error...!" + ex.ToString();
                throw ex;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)//-----------------------Update Button Click Event------------------------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());            
                OracleTransaction myTrans; 
                conn.Open();
                myTrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                foreach (GridViewRow row in grdInsert.Rows)
                {
                    DropDownList ddlColIndex = (row.FindControl("ddlCol_Index") as DropDownList);
                    string b = ddlColIndex.SelectedItem.Text;

                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        OracleCommand cmd = new OracleCommand();//conn.CreateCommand();
                        cmd.Connection = conn;                        

                        cmd.Transaction = myTrans;

                        cmd.CommandText = "SP_FAS_IBT_UploadFormatsUpdate";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("vFormatID", OracleType.Int32).Value = tempID;
                        cmd.Parameters.AddWithValue("vProcessType", OracleType.VarChar).Value = "tblUpdate";
                        cmd.Parameters.AddWithValue("vColID", OracleType.Int32).Value = int.Parse(row.Cells[1].Text);
                        cmd.Parameters.AddWithValue("vDescription", OracleType.VarChar).Value = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                        cmd.Parameters.AddWithValue("vRowIndex", OracleType.Int32).Value = int.Parse(row.Cells[4].Controls.OfType<TextBox>().FirstOrDefault().Text);
                        cmd.Parameters.AddWithValue("vColumnsIndex", OracleType.Int32).Value = int.Parse(row.Cells[3].Controls.OfType<DropDownList>().FirstOrDefault().Text);
                        cmd.Parameters.AddWithValue("vColumnOrder", OracleType.VarChar).Value = b;
                        cmd.Parameters.AddWithValue("vStartIndex", OracleType.Int32).Value = int.Parse(row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text);
                        cmd.Parameters.AddWithValue("vEndIndex", OracleType.Int32).Value = int.Parse(row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Text);

                        cmd.ExecuteNonQuery();
                    }
                }
                myTrans.Commit();

                call_error_msg(true);                
                lblResult.Text = "Update Successfull...!";

                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----IBT Format Configured (Update)---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));

                grdInsert.DataSource = null;
                grdInsert.DataBind();

                ddlIBTFormat.SelectedIndex = -1;
                //tblHdr.Visible = false;//Header Fixed
                btnUpdate.Visible = false;
                btnCancel.Visible = false;

                tempID = 0;
                conn.Close();
            }
            catch (Exception ex)
            {
               // myTrans.Rollback();
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Update Fail...!" + ex.ToString();
            }
        }

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
                //this.lblResult.Style.Add("color", "#ffff");
                //this.page_result.Style.Add("background-color", "#FF0000");
                //this.page_result.Style.Add("border-color", "#FF0000");
                lblResult.Visible = true;
                this.lblResult.Style.Add("color", "#ffff");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)//-----------------------Cancel Button Click Event------------------------------------//
        {
            ddlIBTFormat.SelectedIndex = -1;
            grdInsert.DataSource = null;
            grdInsert.DataBind();
            //tblHdr.Visible = false;//Header Fixed
            btnCancel.Visible = false;
            btnInsert.Visible = false;
            btnUpdate.Visible = false;
        }        
    }
}