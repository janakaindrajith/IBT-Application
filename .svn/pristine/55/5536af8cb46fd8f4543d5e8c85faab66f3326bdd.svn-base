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
    public partial class IBT_MatchingRule : System.Web.UI.Page
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
                DtRules.Clear();
                DtRules.Columns.Add("ID");
                DtRules.Columns.Add("Description");
                DtRules.Columns.Add("CreateDate");//new

                Session["DtRules"] = DtRules;
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
                cmd.CommandText = "select t.ID, t.DESCRIPTION  from fas_ibt_rules t where t.TYPE = '2' and t.Inactive = 0";
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
                cmd.CommandText = "select t.Id, t.description from fas_ibt_columns t where t.Inactive = 0";


                //GridView1.DataSource = cmd.ExecuteReader();
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        private void LoadColumns()
        {
            try
            {

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
                if (ddlIBTRules.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select A Rule And Try Again...!";
                    return;
                }
                else
                {
                    CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
                    String UserName = usrName;// clsCom.getCurentUser();

                    String SQLWhere = "";


                    foreach (GridViewRow row in GrdRules.Rows)
                    {
                        DropDownList DDLOPENBrackets = row.Cells[0].FindControl("ddlOpenBracket") as DropDownList;
                        TextBox TXTColID = row.Cells[1].FindControl("txtcolID") as TextBox;
                        TextBox TXTColDesc = row.Cells[2].FindControl("txtDescription") as TextBox;
                        DropDownList DDLOperator = row.Cells[3].FindControl("ddlOperator") as DropDownList;
                        TextBox TXTValue = row.Cells[4].FindControl("txtValue") as TextBox;
                        DropDownList DDLPolicyDTL = row.Cells[3].FindControl("ddlPolicyDtl") as DropDownList;
                        DropDownList DDLCLOSEBrackets = row.Cells[4].FindControl("ddlCloseBracket") as DropDownList;
                        DropDownList DDLConditions = row.Cells[5].FindControl("ddlConditions") as DropDownList;

                        if (DDLOperator.SelectedIndex != 0)
                        {
                            SQLWhere = SQLWhere + CreateSQLWhere(SQLWhere, DDLOPENBrackets.SelectedItem.Text, TXTColDesc.Text, Convert.ToInt16(DDLOperator.SelectedValue), TXTValue.Text, DDLPolicyDTL.SelectedItem.Text, DDLCLOSEBrackets.SelectedItem.Text, DDLConditions.SelectedItem.Text);
                        }
                    }

                    txtViewRule.Text = SQLWhere;



                    //Rule Header Update
                    OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                    conn.Open();

                    OracleCommand cmd = new OracleCommand();//conn.CreateCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "SP_FAS_IBT_MATCHING_RULES_HDR";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("RULE_ID", OracleType.Int32).Value = Convert.ToInt64(ddlIBTRules.SelectedValue);
                    cmd.Parameters.AddWithValue("DESCRIPTION", OracleType.VarChar).Value = SQLWhere.ToUpper();
                    cmd.Parameters.AddWithValue("CREATEDBY", OracleType.VarChar).Value = UserName; //"ss";

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    call_error_msg(true);
                    lblResult.Text = "Matching Rule Inserted Successfully...!";

                    this.SetFocus(cmdViewRule);
                    ClearInputs(Page.Controls);
                }

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        private String CreateSQLWhere(String SQL, String OpenningBrackets, String Column, Int16 OperatorID, String Value, String PolicyDetail, String ClosingBrackets, String Condition)
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

                //if (PolicyDetail == "Proposal No") Operator = "PRO_NO";
                //if (PolicyDetail == "Policy No") Operator = "POL_NO";
                //if (PolicyDetail == "NIC") Operator = "POL_NIC";
                //if (PolicyDetail == "Vehicle No") Operator = "POL_REG_NO";


                if (Value.Contains("MID"))
                {
                    String TempStr = Value;

                    string s = TempStr.Replace("MID", "").Replace("(", "").Replace(")", "");
                    string[] values = s.Split(',');
                    Int32 From = Convert.ToInt32(values[0].ToString());
                    Int32 To = Convert.ToInt32(values[1].ToString());

                    Temp = " " + Condition + " " + OpenningBrackets + " SUBSTR(" + Column + ", " + From + "," + To + ") " + Operator + " " + PolicyDetail + " " + ClosingBrackets;
                }

                if (Value.Contains("LEFT"))
                {
                    //String TempStr = Value;
                    //Int32 From = TempStr.IndexOf("(");
                    //Int32 To = TempStr.IndexOf(")");
                    //Int16 Index = Convert.ToInt16(TempStr.Substring(From + 1, TempStr.Length - To));

                    String TempStr = Value;
                    TempStr = TempStr.Replace("LEFT(", "").Replace(")", "");
                    Int16 Index = Convert.ToInt16(TempStr);

                    Temp = " " + Condition + " " + OpenningBrackets + " SUBSTR(" + Column + ",0," + Index + ") " + Operator + " " + PolicyDetail + " " + ClosingBrackets;
                }

                if (Value.Contains("RIGHT"))
                {
                    //String TempStr = Value;
                    //Int32 From = TempStr.IndexOf("(");
                    //Int32 To = TempStr.IndexOf(")");
                    //Int16 Index = Convert.ToInt16(TempStr.Substring(From + 1, TempStr.Length - To));

                    String TempStr = Value;
                    TempStr = TempStr.Replace("RIGHT(", "").Replace(")", "");
                    Int16 Index = Convert.ToInt16(TempStr);

                    Index = Convert.ToInt16(-1 * Index);

                    Temp = " " + Condition + " " + OpenningBrackets + " SUBSTR(" + Column + "," + Index + ") " + Operator + " " + PolicyDetail + " " + ClosingBrackets;
                }

                if (Value == "")
                {
                    Temp = " " + Condition + " " + OpenningBrackets + Column + Operator + " " + PolicyDetail + " " + ClosingBrackets;
                }

                //Temp = " " + Condition + " " + OpenningBrackets + " " + Column + " " + Operator + " " + Value + " " + ClosingBrackets;
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }



            return Temp;
        }

        private string Reverse(string str)
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        protected void cmdViewRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (page_result.Visible == true)
                {
                    page_result.Visible = false;
                }

                String SQLWhere = "";


                foreach (GridViewRow row in GrdRules.Rows)
                {
                    DropDownList DDLOPENBrackets = row.Cells[0].FindControl("ddlOpenBracket") as DropDownList;
                    TextBox TXTColID = row.Cells[1].FindControl("txtcolID") as TextBox;
                    TextBox TXTColDesc = row.Cells[2].FindControl("txtDescription") as TextBox;
                    DropDownList DDLOperator = row.Cells[3].FindControl("ddlOperator") as DropDownList;
                    TextBox TXTValue = row.Cells[4].FindControl("txtValue") as TextBox;
                    DropDownList DDLPolicyDTL = row.Cells[3].FindControl("ddlPolicyDtl") as DropDownList;
                    DropDownList DDLCLOSEBrackets = row.Cells[4].FindControl("ddlCloseBracket") as DropDownList;
                    DropDownList DDLConditions = row.Cells[5].FindControl("ddlConditions") as DropDownList;

                    if (DDLOperator.SelectedIndex != 0)
                    {
                        SQLWhere = SQLWhere + CreateSQLWhere(SQLWhere, DDLOPENBrackets.SelectedItem.Text, TXTColDesc.Text, Convert.ToInt16(DDLOperator.SelectedValue), TXTValue.Text, DDLPolicyDTL.SelectedItem.Text, DDLCLOSEBrackets.SelectedItem.Text, DDLConditions.SelectedItem.Text);
                    }
                }

                if (SQLWhere == "")//new
                {
                    call_error_msg(false);
                    lblResult.Text = "No Rules Available For View...!";
                }
                else
                {
                    txtViewRule.Text = SQLWhere;
                    this.SetFocus(cmdViewRule);
                }

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
                if (page_result.Visible == true)
                {
                    page_result.Visible = false;
                }

                if (DDLRules.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select A Coulumn And Try Again...!";
                    return;
                }

                //GrdRules.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                DtRules = (DataTable)Session["DtRules"];
                string currentDate = System.DateTime.Now.ToString();

                DataRow Row = DtRules.NewRow();
                Row["ID"] = Convert.ToString(DDLRules.SelectedValue);
                Row["Description"] = Convert.ToString(DDLRules.SelectedItem);
                Row["CreateDate"] = currentDate;
                DtRules.Rows.Add(Row);

                GrdRules.DataSource = DtRules;
                GrdRules.DataBind();

                this.SetFocus(cmdViewRule);

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void GrdRules_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                if (Convert.ToInt16(e.Row.RowIndex) == 0)
                {
                    DropDownList ddlTrans = (e.Row.FindControl("ddlConditions") as DropDownList);
                    ddlTrans.Enabled = false;
                }

            }
        }

        protected void ddlIBTRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                txtDescription.Text = "";

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select rls.description as Rule, hdr.desctription from fas_ibt_matching_rules_hdr hdr " +
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
                lblResult.Text = ex.InnerException.ToString();
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

                //this.page_result.Style.Add("background-color", "#FF0000");
                //this.page_result.Style.Add("border-color", "#FF0000");
                lblResult.Visible = true;
                this.lblResult.Style.Add("color", "#ffff");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//

        protected void GrdRules_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //DataTable dt = (DataTable)GrdRules.DataSource;
            DtRules = (DataTable)Session["DtRules"];
            DtRules.Rows[e.RowIndex].Delete();
            GrdRules.DataSource = DtRules;
            GrdRules.DataBind();

        }

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

    }
}