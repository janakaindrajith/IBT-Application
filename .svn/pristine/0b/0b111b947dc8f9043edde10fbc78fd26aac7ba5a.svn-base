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
    public partial class IBT_Filtering : System.Web.UI.Page
    {

        public DataTable DtRules = new DataTable();
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

                 try
                 {
                     if (!IsPostBack)
                     {
                         RuleDataTable();
                         IBTAccountsFor_ddl();
                         IBTColumns();
                         IBTRulesFor_ddl();
                         LoadColumns();
                     }
                 }
                 catch (Exception ex)
                 {
                     CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                     call_error_msg(false);
                     lblResult.Text = ex.InnerException.ToString();
                 }
             }
            else
             {
                 Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                 Context.ApplicationInstance.CompleteRequest();
             }
        }

        private void RuleDataTable()
        {
            try
            {
                DtRules.Dispose();
                DtRules.Clear();
                DtRules.Columns.Add("ID");
                DtRules.Columns.Add("Description");

                Session["DtRules"] = DtRules;
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }

        }

        public void IBTAccountsFor_ddl()//Data binding for dropdownlists
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();//IBT Accounts
                cmd.CommandText = " select t.acc_no || '-' || t.acc_name as accWithName, t.acc_no from fas_ibt_accounts t " +
                                  " where t.Inactive = 0";
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                ddlIbtAcc.DataTextField = "accWithName";
                ddlIbtAcc.DataValueField = "acc_no";
                ddlIbtAcc.DataSource = dt;
                ddlIbtAcc.DataBind();

                ddlIbtAcc.Items.Insert(0, new ListItem("---Please Select---", "0"));
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        public void IBTRulesFor_ddl()//Data binding for dropdownlists
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();//IBT Accounts
                cmd.CommandText = "select t.ID, t.DESCRIPTION  from fas_ibt_rules t where t.TYPE = '1' and t.Inactive = 0";
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                ddlIBTRules.DataTextField = "DESCRIPTION";
                ddlIBTRules.DataValueField = "ID";
                ddlIBTRules.DataSource = dt;
                ddlIBTRules.DataBind();

                ddlIBTRules.Items.Insert(0, new ListItem("---Please Select---", "0"));
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        public void IBTColumns()
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();//IBT Accounts
                cmd.CommandText = "select t.Id, t.description from fas_ibt_columns t";

                //GridView1.DataSource = cmd.ExecuteReader();
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        private void LoadColumns()
        {
            try
            {
                //this.GrdRules.Rows[0].Cell[0].value = 1;

                //lblResult.Text = "";

                //GrdRules.DataSource = null;
                //GrdRules.DataBind();

                //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                //OracleCommand cmd = new OracleCommand();
                //conn.Open();
                //cmd.Connection = conn;

                //cmd.CommandText = "select t.ID,t.Description from fas_ibt_columns t where t.ID =1";

                //OracleDataReader myOleDbDataReader = cmd.ExecuteReader();
                //if (myOleDbDataReader.HasRows == true)
                //{
                //    GrdRules.DataSource = myOleDbDataReader;
                //    GrdRules.DataBind();
                //}

                //conn.Close();
                //myOleDbDataReader.Close();

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();//IBT Accounts
                cmd.CommandText = "select t.ID,t.Description from fas_ibt_columns t where t.Inactive = 0";
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                DDLRules.DataTextField = "Description";
                DDLRules.DataValueField = "ID";
                DDLRules.DataSource = dt;
                DDLRules.DataBind();

                DDLRules.Items.Insert(0, new ListItem("---Please Select---", "0"));

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void cmdSaveRule_Click(object sender, EventArgs e)
        {
            try
            {
                //--------------Validations----------------------------
                if (ddlIBTRules.SelectedValue == "0")
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select the Rule...";
                    return;
                }
                if (ddlIbtAcc.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select the Account...";
                    return;
                }
                if (ddlTrans.SelectedValue == "0")
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select the Transaction...";
                    return;
                }
                //----------------------------------------------------

                //CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
                string UserName = usrName;// clsCom.getCurentUser();

                String SQLWhere = "";

                if (ddlIbtAcc.SelectedIndex != 0)
                {
                    SQLWhere = SQLWhere + "ACCOUNT_NO = " + ddlIbtAcc.SelectedValue;
                }

                if (ddlTrans.SelectedIndex == 2)//Debit
                {
                    SQLWhere = SQLWhere + " AND DEBIT > 0";
                }

                if (ddlTrans.SelectedIndex == 3)//Debit
                {
                    SQLWhere = SQLWhere + " AND CREDIT > 0";
                }

                //SQLWhere = SQLWhere;

                foreach (GridViewRow row in GrdRules.Rows)
                {

                    DropDownList DDLOPENBrackets = row.Cells[0].FindControl("ddlOpenBracket") as DropDownList;
                    TextBox TXTColID = row.Cells[1].FindControl("txtcolID") as TextBox;
                    TextBox TXTColDesc = row.Cells[2].FindControl("txtDescription") as TextBox;
                    DropDownList DDLOperator = row.Cells[3].FindControl("ddlOperator") as DropDownList;
                    TextBox TXTValue = row.Cells[4].FindControl("txtValue") as TextBox;
                    DropDownList DDLCLOSEBrackets = row.Cells[4].FindControl("ddlCloseBracket") as DropDownList;
                    DropDownList DDLConditions = row.Cells[5].FindControl("ddlConditions") as DropDownList;


                    OracleConnection OraCon = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());


                    if (DDLOperator.SelectedIndex != 0)
                    {
                        OracleCommand objCmd = new OracleCommand();
                        OraCon.Open();
                        objCmd.Connection = OraCon;
                        objCmd.CommandText = "SP_FAS_IBT_BIFURCATION_RULES";
                        objCmd.CommandType = CommandType.StoredProcedure;

                        objCmd.Parameters.Add("RULE_ID", OracleType.Number).Value = Convert.ToInt16(ddlIBTRules.SelectedValue);
                        objCmd.Parameters.Add("OPEN_BRACKET_ID", OracleType.Number).Value = Convert.ToInt16(DDLOPENBrackets.SelectedValue);
                        objCmd.Parameters.Add("DESCRIPTION", OracleType.VarChar).Value = TXTColDesc.Text;
                        objCmd.Parameters.Add("ACC_ID", OracleType.VarChar).Value = ddlIbtAcc.SelectedValue;
                        objCmd.Parameters.Add("TYPE_ID", OracleType.Number).Value = Convert.ToInt64(ddlTrans.SelectedValue);
                        objCmd.Parameters.Add("COLUMN_ID", OracleType.Number).Value = Convert.ToInt64(TXTColID.Text);
                        objCmd.Parameters.Add("OPERATOR_ID", OracleType.Number).Value = Convert.ToInt64(DDLOperator.SelectedValue);
                        objCmd.Parameters.Add("VALUE", OracleType.VarChar).Value = TXTValue.Text;
                        objCmd.Parameters.Add("CLOSE_BRACKET_ID", OracleType.Number).Value = Convert.ToInt64(DDLCLOSEBrackets.SelectedValue);

                        objCmd.Parameters.Add("CONDITION", OracleType.Number).Value = Convert.ToInt16(DDLConditions.SelectedValue);

                        SQLWhere = SQLWhere + CreateSQLWhere(SQLWhere, DDLOPENBrackets.SelectedItem.Text, TXTColDesc.Text, Convert.ToInt16(DDLOperator.SelectedValue), TXTValue.Text, DDLCLOSEBrackets.SelectedItem.Text, DDLConditions.SelectedItem.Text);

                        //objCmd.Parameters.Add("CREATEDBY", OracleType.VarChar).Value = UserName;

                        objCmd.ExecuteNonQuery();

                        
                    }
                }

                //Rule Header Update
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                cmd.CommandText = "SP_FAS_IBT_BIFURCATE_RULES_HDR";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("RULE_ID", OracleType.Int32).Value = Convert.ToInt64(ddlIBTRules.SelectedValue);
                cmd.Parameters.AddWithValue("ACC_ID", OracleType.VarChar).Value = ddlIbtAcc.SelectedValue;
                cmd.Parameters.AddWithValue("TRANS_ID", OracleType.Int32).Value = Convert.ToInt64(ddlTrans.SelectedValue);
                cmd.Parameters.AddWithValue("DESCRIPTION", OracleType.VarChar).Value = SQLWhere;

                cmd.Parameters.AddWithValue("CREATEDBY", OracleType.VarChar).Value = UserName;

                cmd.ExecuteNonQuery();
                conn.Close();



                ddlIbtAcc.SelectedIndex = -1;
                ddlIBTRules.SelectedIndex = -1;
                ddlTrans.SelectedIndex = -1;
                DDLRules.SelectedIndex = -1;

                GrdRules.DataSource = null;
                GrdRules.DataBind();
                txtViewRule.Text = "";

                this.SetFocus(cmdViewRule);

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        private String CreateSQLWhere(String SQL, String OpenningBrackets, String Column, Int16 OperatorID, String Value, String ClosingBrackets, String Condition)
        {
            String Temp = "";
            try
            {

                String Operator = "";

                if (OpenningBrackets == "--") OpenningBrackets = "";
                if (ClosingBrackets == "--") ClosingBrackets = "";
                if (Condition == "--") Condition = "";

                if (OperatorID == 1) Operator = "=";
                if (OperatorID == 2) Operator = "<>";
                if (OperatorID == 3) Operator = "<";
                if (OperatorID == 4) Operator = "<=";
                if (OperatorID == 5) Operator = ">";
                if (OperatorID == 6) Operator = ">=";
                if (OperatorID == 7) Operator = "Like";
                if (OperatorID == 8) Operator = "Not Like";
                if (OperatorID == 9) Operator = "In";
                if (OperatorID == 10) Operator = "Not In";


                Temp = " " + Condition + " " + OpenningBrackets + " " + Column + " " + Operator + " " + Value + " " + ClosingBrackets;

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }

            return Temp;
         }

        protected void cmdViewRule_Click(object sender, EventArgs e)
        {

            try
            {

                if (ddlIbtAcc.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select the Account...";
                    return;
                }
                if (ddlIBTRules.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select the Rule...";
                    return;
                }
                if (ddlTrans.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select the Transaction...";
                    return;
                }



                String SQLWhere = "";

                if (ddlIbtAcc.SelectedIndex != 0)//account no
                    SQLWhere = SQLWhere + "ACCOUNT_NO = " + ddlIbtAcc.SelectedValue;

                if (ddlTrans.SelectedIndex == 2)//Debit
                    SQLWhere = SQLWhere + " AND DEBIT > 0";

                if (ddlTrans.SelectedIndex == 3)//Debit
                    SQLWhere = SQLWhere + " AND CREDIT > 0";


                //SQLWhere = SQLWhere + " AND ";

                //if (GrdRules.Rows.Count >0)
                //{
                //    SQLWhere = " AND " + SQLWhere;
                //}

                foreach (GridViewRow row in GrdRules.Rows)
                {
                    DropDownList DDLOPENBrackets = row.Cells[0].FindControl("ddlOpenBracket") as DropDownList;
                    TextBox TXTColID = row.Cells[1].FindControl("txtcolID") as TextBox;
                    TextBox TXTColDesc = row.Cells[2].FindControl("txtDescription") as TextBox;
                    DropDownList DDLOperator = row.Cells[3].FindControl("ddlOperator") as DropDownList;
                    TextBox TXTValue = row.Cells[4].FindControl("txtValue") as TextBox;
                    DropDownList DDLCLOSEBrackets = row.Cells[4].FindControl("ddlCloseBracket") as DropDownList;
                    DropDownList DDLConditions = row.Cells[5].FindControl("ddlConditions") as DropDownList;

                    if (DDLOperator.SelectedIndex != 0)
                    {
                        SQLWhere = SQLWhere + CreateSQLWhere(SQLWhere, DDLOPENBrackets.SelectedItem.Text, TXTColDesc.Text, Convert.ToInt16(DDLOperator.SelectedValue), TXTValue.Text, DDLCLOSEBrackets.SelectedItem.Text, DDLConditions.SelectedItem.Text);
                    }
                }

                txtViewRule.Text = SQLWhere;

                this.SetFocus(cmdViewRule);

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void cmdAddRule_Click(object sender, EventArgs e)
        {
            try
            {
                if(page_result.Visible == true)
                {
                    page_result.Visible = false;
                }

                if (DDLRules.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select a Column And Try Again...!";
                    return;
                }

                //GrdRules.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                DtRules = (DataTable)Session["DtRules"];

                DataRow Row = DtRules.NewRow();
                Row["ID"] = Convert.ToString(DDLRules.SelectedValue);
                Row["Description"] = Convert.ToString(DDLRules.SelectedItem);
                DtRules.Rows.Add(Row);

                GrdRules.DataSource = DtRules;
                GrdRules.DataBind();

                lblCount.Text = DtRules.Rows.Count.ToString();

                this.SetFocus(cmdViewRule);



            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void GrdRules_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
        }

        protected void ddlIBTRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                txtDescription.Text = "";

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select rls.description as Rule, hdr.desctription from fas_ibt_bifurcation_rules_hdr hdr " +
                "inner join fas_ibt_rules rls on hdr.rule_id = rls.id where rls.id = " + ddlIBTRules.SelectedValue;

                OracleDataReader odr = cmd.ExecuteReader();
                while (odr.Read())
                {
                    txtDescription.Text = odr["desctription"].ToString().Trim();
                }


                if (odr.HasRows)
                {
                    cmdSaveRule.Enabled = false;
                    cmdViewRule.Enabled = false;
                }
                else
                {
                    cmdSaveRule.Enabled = true;
                    cmdViewRule.Enabled = true;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }

        }

        protected void GrdRules_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //DataTable dt = (DataTable)GrdRules.DataSource;
            DtRules = (DataTable)Session["DtRules"];
            DtRules.Rows[e.RowIndex].Delete();
            GrdRules.DataSource = DtRules;
            GrdRules.DataBind();

            lblCount.Text = DtRules.Rows.Count.ToString();

            this.SetFocus(cmdViewRule);
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

                //this.page_result.Style.Add("background-color", "#FF0000");
                //this.page_result.Style.Add("border-color", "#FF0000");
                lblResult.Visible = true;
                this.lblResult.Style.Add("color", "#ffff");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//
    }
}