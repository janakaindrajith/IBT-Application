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
    public partial class IBT_HistoryUpdate : System.Web.UI.Page
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
                    LoadData();
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

                string Type = "";
                string Product = "";
                string Parameter = "";
                string Value = "";


                if ((ddl_Type.SelectedIndex != 0) && (ddl_product.SelectedIndex != 0) && ((txtParameter.Text != "") || (txtValue.Text != null)))
                {
                    Type = ddl_Type.SelectedItem.ToString();
                    Product = ddl_product.SelectedItem.ToString();
                    Parameter = txtParameter.Text.Trim();
                    Value = txtValue.Text.Trim();


                    OracleCommand cmd = myConnectionUse.CreateCommand();
                    cmd.CommandText = "SP_FAS_IBT_POSSIBLE_INSERT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vType", OracleType.VarChar).Value = Type;
                    cmd.Parameters.Add("vProduct", OracleType.VarChar).Value = Product;
                    cmd.Parameters.Add("vParameter", OracleType.VarChar).Value = Parameter;
                    cmd.Parameters.Add("vValue", OracleType.VarChar).Value = Value;

                    cmd.ExecuteNonQuery();

                    call_error_msg(true);
                    lblResult.Text = "Command Execution Successfull..!";

                    myConnectionUse.Close();

                    ddl_Type.ClearSelection();
                    ddl_product.ClearSelection();
                    txtParameter.Text = "";
                    txtValue.Text = "";
   
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

        private void LoadData()
        {
            //Create DataTable this can be from database also.
            DataTable dtName = new DataTable();

            //Add Columns to Table
            dtName.Columns.Add(new DataColumn("ID"));
            dtName.Columns.Add(new DataColumn("DESC"));

            //Now Add Values
            dtName.Rows.Add("SELECT", "0");
            dtName.Rows.Add("MY FUND", "1");
            dtName.Rows.Add("PROPOSAL", "2");
            dtName.Rows.Add("RANMAGA", "3");
            dtName.Rows.Add("Scholar, My Life,Saviya,Ranaswa", "4");


            //At Last Bind datatable to dropdown.
            ddl_Type.DataSource = dtName;
            ddl_Type.DataTextField = dtName.Columns["ID"].ToString();
            ddl_Type.DataValueField = dtName.Columns["DESC"].ToString();
            ddl_Type.DataBind();



            //Create DataTable this can be from database also.
            DataTable dtProduct = new DataTable();

            //Add Columns to Table
            dtProduct.Columns.Add(new DataColumn("ID"));
            dtProduct.Columns.Add(new DataColumn("DESC"));

            //Now Add Values
            dtProduct.Rows.Add("SELECT", "0");
            dtProduct.Rows.Add("LIFE", "1");
            dtProduct.Rows.Add("GENERAL", "2");


            //At Last Bind datatable to dropdown.
            ddl_product.DataSource = dtProduct;
            ddl_product.DataTextField = dtProduct.Columns["ID"].ToString();
            ddl_product.DataValueField = dtProduct.Columns["DESC"].ToString();
            ddl_product.DataBind();

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
    }
}