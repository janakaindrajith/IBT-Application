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
    public partial class IBT_TypeOfProductRule : System.Web.UI.Page
    {
        public DataTable DtRules = new DataTable();

        private string usrName = "";
        //private static int userRole = 0;
        public static int usr_role = 1;

        protected void Page_Load(object sender, EventArgs e)//------------------------------Page Load Events--------------------------------------------------------------//
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

                if (!IsPostBack)
                {
                    RuleDataTable();
                    getProducts();
                    getDepartments();
                    getRules();
                    getColumns();
                    getAvailableDepartments();
                }
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }


        //----------------------------------------------------------------------------------Dropdownlists-----------------------------------------------------------------//

        public void getProducts()//---------------------------------------------------------Get products to dropdownlist (ddlAllProducts)---------------------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from fas_ibt_products t order by t.value ";

                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                ddlAllProducts.DataTextField = "PRODUCT_DESCRIPTION";
                ddlAllProducts.DataValueField = "VALUE";
                ddlAllProducts.DataSource = dt;
                ddlAllProducts.DataBind();

                ddlAllProducts.Items.Insert(0, new ListItem("---Please Select---", "-1"));
                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                //call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        public void getDepartments()//------------------------------------------------------Get departments (data category) to dropdownlist (ddlDepartment)---------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from fas_ibt_departments t where t.inactive = 0 order by t.value ";

                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                ddlDepartment.DataTextField = "department_description";
                ddlDepartment.DataValueField = "value";
                ddlDepartment.DataSource = dt;
                ddlDepartment.DataBind();

                ddlDepartment.Items.Insert(0, new ListItem("---Please Select---", "-1"));
                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                //call_error_msg(false);
                //lblResult.Text = ex.InnerException.ToString();
            }
        }

        public void getRules()//------------------------------------------------------------Get product rules to dropdownlist (ddlIBTRules)-------------------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();//IBT Accounts
                cmd.CommandText = "select t.ID, t.DESCRIPTION  from fas_ibt_rules t where t.TYPE = '3' and t.Inactive = 0";
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

        public void getColumns()//----------------------------------------------------------Get columns from ibt columns table to dropdownlist (DDLRules)-----------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();//IBT Accounts
                cmd.CommandText = "select t.ID,t.Description from fas_ibt_columns t where t.Inactive = 0 and t.id <> 15";
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
                //call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }        

        protected void ddlIBTRules_SelectedIndexChanged1(object sender, EventArgs e)//------Selected index change event for view available rule---------------------------//
        {
            try
            {

                txtDescription.Text = "";

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                cmd.CommandText = " select rls.description as Rule, hdr.description from fas_ibt_mcpmcr_hdr hdr " +
                                  " inner join fas_ibt_rules rls on hdr.rule_id = rls.id where rls.id = " + ddlIBTRules.SelectedValue;

                OracleDataReader odr = cmd.ExecuteReader();
                while (odr.Read())
                {
                    txtDescription.Text = odr["description"].ToString().Trim();
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

                this.SetFocus("cmdViewRule");

                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------//


         
        //-----------------------------------------------------------------------------------Data Category (Department) Events--------------------------------------------//



        protected void grdDataCategory_RowDataBound(object sender, GridViewRowEventArgs e)//----------------Grid View Row Data Bound (Gird Id: grdDataCategory)----------//
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int status_value = int.Parse(DataBinder.Eval(e.Row.DataItem, "INACTIVE").ToString());
                    //string acc_no = (DataBinder.Eval(e.Row.DataItem, "ACC_NO").ToString());

                    int datacategory_value = int.Parse(DataBinder.Eval(e.Row.DataItem, "VALUE").ToString());

                    if (status_value == 0)
                    {
                        CheckBox chk = e.Row.FindControl("chkStatus") as CheckBox;
                        chk.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----grdDataCategory row databound fail...!---- Active User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                //throw;
            }
        }

        protected void chkStatus_CheckedChanged(object sender, EventArgs e)//-------------------------------Active Inactive Data Category (Departments)------------------//
        {
            try
            {
                if (page_result.Visible == true)
                {
                    page_result.Visible = false;
                }

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();

                CheckBox chkbox = (CheckBox)sender;
                GridViewRow grd_acc = (GridViewRow)chkbox.NamingContainer;

                int v_Status = -1;
                //string temp_accNo = grd_acc.Cells[0].Text.Trim();
                int datacategory_value = int.Parse(grd_acc.Cells[0].Text.Trim());

                if (((CheckBox)sender).Checked)
                {
                    v_Status = 0;
                }
                else
                {
                    v_Status = 1;
                }

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update fas_ibt_departments t set inactive=" + v_Status + "where VALUE = " + "'" + datacategory_value + "'";
                cmd.ExecuteNonQuery();
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----Data Category(Department) State Changed...! (Active / Inactive) ---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                getDepartments();//dropdownlistredatabound

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                //pageLoad_error = "Check Box Changed Event Error" + ex.ToString();
                //throw;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------//

        

        //---------------------------------------------------------------------------------Creating The Query------------------------------------------------------------//


        private String CreateSQLWhere(String SQL, String OpenningBrackets, String Column, Int16 OperatorID, String Value, string userInput, String ClosingBrackets, String Condition, int department)//method for create sql where clause
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

                    Temp = " " + Condition + " " + OpenningBrackets + " SUBSTR(" + Column + ", " + From + "," + To + ") " + Operator + " " + "'" + userInput + "'" + " " + ClosingBrackets;

                }

                if (Value.Contains("LEFT"))
                {
                    String TempStr = Value;
                    TempStr = TempStr.Replace("LEFT(", "").Replace(")", "");
                    Int16 Index = Convert.ToInt16(TempStr);

                    Temp = " " + Condition + " " + OpenningBrackets + " SUBSTR(" + Column + ",0," + Index + ") " + Operator + " " + "'" + userInput + "'" + " " + ClosingBrackets;
  
                }

                if (Value.Contains("RIGHT"))
                {
                    String TempStr = Value;
                    TempStr = TempStr.Replace("RIGHT(", "").Replace(")", "");
                    Int16 Index = Convert.ToInt16(TempStr);

                    Index = Convert.ToInt16(-1 * Index);

                    Temp = " " + Condition + " " + OpenningBrackets + " SUBSTR(" + Column + "," + Index + ") " + Operator + " " + "'" + userInput + "'" + " " + ClosingBrackets;
                    
                }

                if (OperatorID == 7)
                {
                    Temp = " " + Condition + " " + OpenningBrackets + " " + Column + " LIKE '%" + userInput + "%'" + ClosingBrackets;
                }

                if((!Value.Contains("LEFT")) && (!Value.Contains("RIGHT")) && (!Value.Contains("MID")) && (OperatorID != 7))
                {
                    Temp = " " + Condition + " " + OpenningBrackets + Column + Operator + "'" + userInput + "'" + ClosingBrackets;
                }

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }

            return Temp;
        }

        public void getAvailableDepartments()//-------------------------Data Bind - IBT Departments (Data Category) (Grid View ID :grdDataCategory)---------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                //cmd.CommandText = "select d.value, d.department_description, d.inactive from fas_ibt_departments d order by d.value";
                cmd.CommandText = "select d.value, d.department_description, case when d.type_id = 1 THEN 'New Business' when d.type_id = 2 THEN 'Renewal' " +
                                    "when d.type_id = 3 THEN 'Non TCS' when d.type_id = 4 THEN 'Other' end as Type ,d.transaction_code, d.policy_fee ,d.inactive " +
                                    "from fas_ibt_departments d order by d.value ";

                OracleDataReader odr = cmd.ExecuteReader();
                while (odr.Read())
                {
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    grdDataCategory.DataSource = dt;
                    grdDataCategory.DataBind();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                //pageLoad_error = ex.ToString();
                //throw;
            }
        }

        public void IBTColumns()//---------------------------------------------------------------------------Get columns from ibt columns (All data)--------------------//
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
        
        private void RuleDataTable()//-----------------------------------------------------------------------Store Data Table Inside A Session--------------------------//
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

        protected void GrdRules_RowDataBound(object sender, GridViewRowEventArgs e)//------------------------Row Data Bound (grid view: GrdRules)-----------------------//
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                    if (Convert.ToInt16(e.Row.RowIndex) == 0)
                    {
                        DropDownList ddlTrans = (e.Row.FindControl("ddlConditions") as DropDownList);
                        ddlTrans.Enabled = false;
                    }

                    //DropDownList ddlCondition = (DropDownList)e.Row.FindControl("ddlOperator");
                    //TextBox txtStringCondition = (TextBox)e.Row.FindControl("txtValue");

                    //if(ddlCondition.SelectedItem.Text == "Like")
                    //{
                    //    txtStringCondition.Enabled = false;
                    //}

                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                //throw;
            }
        }

        protected void GrdRules_RowDeleting(object sender, GridViewDeleteEventArgs e)//----------------------Row Delete (grid view: GrdRules)---------------------------//
        {
            //DataTable dt = (DataTable)GrdRules.DataSource;
            DtRules = (DataTable)Session["DtRules"];
            DtRules.Rows[e.RowIndex].Delete();
            GrdRules.DataSource = DtRules;
            GrdRules.DataBind();

        }

        private string Reverse(string str)//Not Used
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------//




        //---------------------------------------------------------------------------Button Click Events----------------------------------------------------------------//
        

        protected void btnInsert_Click(object sender, EventArgs e)//----------------------------------------Insert Department-------------------------------------------//
        {
            try
            {
                if ((txtDepartment.Text != "") && (ddlType.SelectedIndex != 0) && (txtTransactionCode.Text != ""))
                {
                    OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                    conn.Open();
                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SP_FAS_IBT_NewDepartment";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vDescription", OracleType.VarChar).Value = txtDepartment.Text;
                    cmd.Parameters.Add("vCreatedBy", OracleType.VarChar).Value = usrName;
                    cmd.Parameters.Add("vInactive", OracleType.Int32).Value = 0;
                    cmd.Parameters.Add("vType_ID", OracleType.Int32).Value = ddlType.SelectedIndex;
                    cmd.Parameters.Add("vTransCode", OracleType.VarChar).Value = txtTransactionCode.Text.Trim();

                    if (txtPolicyFee.Text.Trim() == "" || txtPolicyFee.Text.Trim() == null)
                    {
                        txtPolicyFee.Text = "0";
                    }

                    cmd.Parameters.Add("vPolicyFee", OracleType.Double).Value = Convert.ToDouble(txtPolicyFee.Text.Trim());

                    cmd.ExecuteNonQuery();

                    conn.Close();

                    call_error_msg(true);
                    lblResult.Text = "New Data Category Inserted Successfully...!";

                    CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----New IBT Data Category (Department) Created ---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));

                    txtDepartment.Text = "";
                    ddlType.SelectedIndex = 0;
                    txtTransactionCode.Text = "";
                    getDepartments();//ddl
                    getAvailableDepartments(); //grd

                    this.SetFocus("btnInsert");
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "All Fields Are Mandatory...!";
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString() + "Active User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                throw ex;
            }
        }

        protected void cmdAddRule_Click(object sender, EventArgs e)//---------------------------------------Add New Row To The Gridview---------------------------------//
        {
            try
            {
                if (page_result.Visible == true)
                {
                    page_result.Visible = false;
                }

                call_error_msg(true);
                lblResult.Text = "";


                if (ddlIBTRules.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select A Rule And Try Again...!";
                    return;
                }
                if (ddlAllProducts.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select A Product And Try Again...!";
                    return;
                }
                if (ddlDepartment.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select A Product And Try Again...!";
                    return;
                }
                if (DDLRules.SelectedIndex == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select A Column And Try Again...!";
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

                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----New Column Added To Grid View (GrdRules) ---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));

                this.SetFocus(cmdViewRule);

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString() + "Active User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void cmdViewRule_Click(object sender, EventArgs e)//--------------------------------------View Rule---------------------------------------------------//
        {
            try
            {
                if ((ddlDepartment.SelectedIndex == 0)&&(ddlAllProducts.SelectedIndex == 0))//new
                {
                    call_error_msg(false);
                    lblResult.Text = "Product And Category Fields Are Mandentory, Please Try Again...!";
                    return;
                }
                else
                {

                    if (page_result.Visible == true)
                    {
                        page_result.Visible = false;
                    }

                    String SQLWhere = "";


                    foreach (GridViewRow row in GrdRules.Rows)
                    {
                        DropDownList DDLOPENBrackets = row.Cells[0].FindControl("ddlOpenBracket") as DropDownList;//Openning Brackets
                        TextBox TXTColID = row.Cells[1].FindControl("txtcolID") as TextBox;
                        TextBox TXTColDesc = row.Cells[2].FindControl("txtDescription") as TextBox;//Statement Details (Acc_no, Description, Date...)
                        DropDownList DDLOperator = row.Cells[3].FindControl("ddlOperator") as DropDownList;//Operator (Eqaul To, Non Eqaual To, Less Than...)
                        TextBox TXTValue = row.Cells[4].FindControl("txtValue") as TextBox;//Value (LEFT, RIGHT, MID)
                        //DropDownList DDLPolicyDTL = row.Cells[3].FindControl("ddlPolicyDtl") as DropDownList;

                        TextBox TXTUserInput = row.Cells[5].FindControl("txtUsrInput") as TextBox; 
                        DropDownList DDLCLOSEBrackets = row.Cells[6].FindControl("ddlCloseBracket") as DropDownList;//Clossing Brackets
                        DropDownList DDLConditions = row.Cells[7].FindControl("ddlConditions") as DropDownList;//Conditions (AND, OR)

                        if (DDLOperator.SelectedIndex != 0)
                        {
                            //SQLWhere = SQLWhere + CreateSQLWhere(SQLWhere, DDLOPENBrackets.SelectedItem.Text, TXTColDesc.Text, Convert.ToInt16(DDLOperator.SelectedValue), TXTValue.Text, DDLPolicyDTL.SelectedItem.Text, DDLCLOSEBrackets.SelectedItem.Text, DDLConditions.SelectedItem.Text, ddlDepartment.SelectedItem.Text);
                            SQLWhere = SQLWhere + CreateSQLWhere(SQLWhere, DDLOPENBrackets.SelectedItem.Text, TXTColDesc.Text, Convert.ToInt16(DDLOperator.SelectedValue), TXTValue.Text, TXTUserInput.Text , DDLCLOSEBrackets.SelectedItem.Text, DDLConditions.SelectedItem.Text, int.Parse(ddlDepartment.SelectedValue.ToString()));
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

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString() + "Active User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void cmdSaveRule_Click(object sender, EventArgs e)//--------------------------------------Save New Rule-----------------------------------------------//
        {
            try
            {
                if ((ddlIBTRules.SelectedIndex == 0) || (ddlAllProducts.SelectedIndex == 0) || (ddlDepartment.SelectedIndex == 0))
                {
                    call_error_msg(false);
                    lblResult.Text = "IBT Rule, Product And Department Fields Are Mandentory, Please Try Again...!";
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
                        //DropDownList DDLPolicyDTL = row.Cells[3].FindControl("ddlPolicyDtl") as DropDownList;
                        TextBox TXTUserInput = row.Cells[5].FindControl("txtUsrInput") as TextBox; 
                        DropDownList DDLCLOSEBrackets = row.Cells[6].FindControl("ddlCloseBracket") as DropDownList;
                        DropDownList DDLConditions = row.Cells[7].FindControl("ddlConditions") as DropDownList;

                        if (DDLOperator.SelectedIndex != 0)
                        {
                            //SQLWhere = SQLWhere + CreateSQLWhere(SQLWhere, DDLOPENBrackets.SelectedItem.Text, TXTColDesc.Text, Convert.ToInt16(DDLOperator.SelectedValue), TXTValue.Text, DDLPolicyDTL.SelectedItem.Text, DDLCLOSEBrackets.SelectedItem.Text, DDLConditions.SelectedItem.Text, ddlDepartment.SelectedItem.Text);
                            SQLWhere = SQLWhere + CreateSQLWhere(SQLWhere, DDLOPENBrackets.SelectedItem.Text, TXTColDesc.Text, Convert.ToInt16(DDLOperator.SelectedValue), TXTValue.Text, TXTUserInput.Text, DDLCLOSEBrackets.SelectedItem.Text, DDLConditions.SelectedItem.Text, int.Parse(ddlDepartment.SelectedValue.ToString()));
                        }
                    }

                    txtViewRule.Text = SQLWhere;



                    //Rule Header Update
                    OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                    conn.Open();

                    OracleCommand cmd = new OracleCommand();//conn.CreateCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "SP_FAS_IBT_MCPMCR_RULES_HDR";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("RULE_ID", OracleType.Int32).Value = Convert.ToInt32(ddlIBTRules.SelectedValue);//ToInt64
                    cmd.Parameters.AddWithValue("DESCRIPTION", OracleType.VarChar).Value = SQLWhere.ToUpper();
                    cmd.Parameters.AddWithValue("product_id", OracleType.Int32).Value = ddlAllProducts.SelectedValue;
                    cmd.Parameters.AddWithValue("mcpmcr_value", OracleType.Int32).Value = ddlDepartment.SelectedItem.Value;
                    cmd.Parameters.AddWithValue("vCreatedby", OracleType.VarChar).Value = usrName;

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----New Type Of Product Rule Saved---- By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));

                    call_error_msg(true);
                    lblResult.Text = "Rule Inserted Successfully...!";

                    this.SetFocus(cmdViewRule);
                    ClearInputs(Page.Controls);
                    GrdRules.DataSource = null;
                    GrdRules.DataBind();
                    
                }

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString() + "---Type Of Product Rule Insertion Fail...! - Active User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }
               
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------//


        
        //--------------------------------------------------------------------------------------------------Common Controls---------------------------------------------//

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

        private void ClearInputs(ControlCollection ctrls)//-------------------------------------------------Reset Page Controls-----------------------------------------//
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
        
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------//       
    }
}