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
    public partial class IBT_Rules : System.Web.UI.Page
    {
        public static int temp_tid = 0;
        public static string ora_error = "";
        public static string inner_error = "";
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

            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnInsertRule_Click(object sender, EventArgs e)//Insert Rule Button Click
        {
            try
            {
                if ((ddlRuleType.SelectedIndex != 0) && (txtRule_Description.Text != ""))
                {
                    int rule_type = int.Parse(ddlRuleType.SelectedValue.ToString());
                    string rule_description = txtRule_Description.Text;
                    string company = ddl_company.SelectedValue.ToString();

                    int inactive = 0;

                    bool x = false;

                    x = insertNewRule(rule_description, rule_type, inactive);
                    if (x == true)
                    {
                        this.page_result.Visible = true;
                        this.page_result.Style.Clear();
                        page_result.Attributes["class"] = "alert alert-success";
                        //this.page_result.Style.Add("background-color", "#32CD32");
                        //this.page_result.Style.Add("border-color", "#32CD32");

                        lblResult.Visible = true;
                        lblResult.Text = "Rule Insertion Successfull...!";
                    }
                    else
                    {
                        if (ora_error != "")
                        {
                            this.page_result.Visible = true;
                            this.page_result.Style.Clear();
                            page_result.Attributes["class"] = "alert alert-danger";
                            this.lblResult.Style.Add("color", "#ffff");
                            lblResult.Visible = true;
                            lblResult.Text = ora_error + inner_error;//changed
                        }
                        else
                        {
                            this.page_result.Visible = true;
                            this.page_result.Style.Clear();
                            page_result.Attributes["class"] = "alert alert-danger";
                            this.lblResult.Style.Add("color", "#ffff");
                            lblResult.Visible = true;
                            lblResult.Text = ora_error;//"Rule Insertion Fail...!";
                        }

                    }
                }
                else
                {
                    this.page_result.Visible = true;
                    this.page_result.Style.Clear();
                    page_result.Attributes["class"] = "alert alert-danger";
                    this.lblResult.Style.Add("color", "#ffff");
                    lblResult.Visible = true;
                    lblResult.Text = "Please Input Rule Type, Description And Try Again!";
                }
            }
            catch (Exception ex)
            {                
                //CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                inner_error = ex.ToString();

            }          
        }

        public bool insertNewRule(string a, int b, int c)//Insert Rule Method
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_NewIBT_Rule";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("vDescription", OracleType.VarChar).Value = a.ToUpper();//rule description;
                cmd.Parameters.Add("vType", OracleType.Int32).Value = b;//rule type;
                cmd.Parameters.Add("vCompany", OracleType.VarChar).Value = Session["IBT_Company"].ToString().ToUpper();//company;
                cmd.Parameters.Add("vInactive", OracleType.Int32).Value = c;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                //CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));

                if(ex.Message.Contains("ORA-00001"))
                {
                    ora_error = "Rule name already exists in the database...!";
                }
                else
                {
                    inner_error = ex.InnerException.ToString();
                }
                return false;
                //throw;
            }      

        }

        public void getAvailableRules(int tid)//Data Bind For Gridview
        {
            try
            {
                string SQL_Query = "";
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                

                if (tid == 1)
                {

                    SQL_Query = "select t.id, t.description, t.inactive, tt.rule_id, tt.desctription as rule, 0 as product_id, 0 as dept_id from fas_ibt_rules t inner join fas_ibt_bifurcation_rules_hdr tt on tt.rule_id = t.id where (t.type = 1) order by t.id ";
                    
                }
                else if(tid == 2)
                {
                    SQL_Query = "select t.id, t.description, t.inactive, tt.rule_id, tt.desctription as rule, 0 as product_id, 0 as dept_id from fas_ibt_rules t inner join fas_ibt_matching_rules_hdr tt on t.id = tt.rule_id where (t.type = 2) order by t.id";                   
                }

                else
                {
                    SQL_Query = "select t.id, t.description, t.inactive, tt.rule_id, tt.DESCRIPTION as rule, tt.product_id, tt.dept_id from fas_ibt_rules t inner join fas_ibt_mcpmcr_hdr tt on t.id = tt.rule_id where (t.type = 3) order by t.id"; //product rules test           
                }

                cmd.CommandText = SQL_Query;

                OracleDataReader odr = cmd.ExecuteReader();
                while (odr.Read())
                {
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    grdRules.DataSource = dt;
                    grdRules.DataBind();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
            }
        }

        protected void grdRules_RowDataBound(object sender, GridViewRowEventArgs e)//Gridview Row Data Bound
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int status_value = int.Parse(DataBinder.Eval(e.Row.DataItem, "Inactive").ToString());//Active Inactive Value In Grid
                int product_value = int.Parse(DataBinder.Eval(e.Row.DataItem, "product_id").ToString());//Product ID Value In Grid
                int dataCat_value = int.Parse(DataBinder.Eval(e.Row.DataItem, "dept_id").ToString());//Data Category Value (Department) In Grid
                string dataCatIn_String = "";

                string ID = (DataBinder.Eval(e.Row.DataItem, "ID").ToString());

                if (status_value == 0)
                {
                    CheckBox chk = e.Row.FindControl("chkStatus") as CheckBox;
                    chk.Checked = true;                    
                }


                //--------------------------------------Products----------------------------------//

                
                if(product_value == 0)
                {
                    e.Row.Cells[3].Text = "Null";
                }
                if(product_value == 1)
                {
                    e.Row.Cells[3].Text = "Conventional-General";
                }
                if (product_value == 2)
                {
                    e.Row.Cells[3].Text = "Conventional-Life";
                }
                if (product_value == 3)
                {
                    e.Row.Cells[3].Text = "Takaful-General";
                }
                if (product_value == 4)
                {
                    e.Row.Cells[3].Text = "Takaful-Life";
                }


                //--------------------------------------Departments-------------------------------//

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Close();
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                cmd.CommandText = "select d.department_description, d.value from fas_ibt_departments d where d.value = " + dataCat_value;

                OracleDataReader odr = cmd.ExecuteReader();

                while(odr.Read())
                {
                    dataCatIn_String = odr["department_description"].ToString();
                }

                if((dataCatIn_String != "")&&(odr.HasRows == true))
                {
                    e.Row.Cells[4].Text = dataCatIn_String;
                }
                else
                {
                    e.Row.Cells[4].Text = "Null";
                }

                odr.Close();
                conn.Close();
                //-------------------//
            }
        }

        protected void chkStatus_CheckedChanged(object sender, EventArgs e)//Check Change Event (Update Inactive / Active)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();

                CheckBox chkbox = (CheckBox)sender;
                GridViewRow grd_rules = (GridViewRow)chkbox.NamingContainer;

                int v_Status = -1;
                string temp_ID = grd_rules.Cells[0].Text.Trim();

                if (((CheckBox)sender).Checked)
                {
                    v_Status = 0;
                }
                else
                {
                    v_Status = 1;
                }

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update fas_ibt_rules set inactive=" + v_Status + "where ID =" + temp_ID;
                cmd.ExecuteNonQuery();
                getAvailableRules(temp_tid);

                this.SetFocus(btnSetFocus);
            }
            catch (Exception ex)
            {
               CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));

            }
     
        }

        protected void ddlRuleTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (page_result.Visible == true)
                {
                    this.page_result.Visible = false;
                }

                if (ddlRuleTypeSearch.SelectedIndex != 0)
                {
                    int ruleType = int.Parse(ddlRuleTypeSearch.SelectedValue.ToString());

                    temp_tid = ruleType;

                    getAvailableRules(temp_tid);

                }
                else
                {
                    grdRules.DataSource = null;
                    grdRules.DataBind();
                }

                this.SetFocus(btnSetFocus);
            }
            catch (Exception ex)
            {
              CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));

            }

        }       

    }
}