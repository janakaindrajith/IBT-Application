﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Web.UI.WebControls;
//using Excel = Microsoft.Office.Interop.Excel;
//using ExcelAutoFormat = Microsoft.Office.Interop.Excel.XlRangeAutoFormat;
using System.Reflection;


namespace BranchMIS.IBT
{

    public partial class IBT_Edit_Uploads : System.Web.UI.Page
    {

        OracleConnection myConnectionUse = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        private static int Export_Count = 0;//Excell File Export Count
        private static string qry = "";//Main Search Query
        private static string qry_orderby = "";//Main Search Query Order By ID/SERIAL
        //private static string temp_qry = "";//Main Query For Data Rebing Bind 
        private string usrName = "";
        public int usr_role = 0;
        int manuallyReceipted_validation = 0;

        public DataTable dtPageControls = new DataTable();

        //--------------------Page Controls Validation-------------------//

        string controller_permission = "";
        bool validto_search = false;
        bool enalbleBulkReceiptBtn = true;

        int Dep_Rule_ID = 0;
        int Bifurcate_Rule_ID = 0;
        int Matching_Rule_ID = 0;
        string DepartmentExisting = "";
        string IBTValueExisting = "";
        string MatchingStatusExisting = "";
        //private static bool IsBulkReceipt = false;


        protected void Page_Load(object sender, EventArgs e)//---------------------------------------------Page Load Event-------------------------------------------------//
        {
            if ((Session["IBT_UserName"] == null) || (Session["IBT_Company"] == null))
            {
                string usrValid = "SessionExpired";
                Response.Redirect("~/FAS_Home.aspx?usr=" + usrValid, false);
                return;
            }


            OracleCommand cmd_pageid = myConnectionUse.CreateCommand();
            cmd_pageid.CommandText = "";

            CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
            //usrName = clsCom.getCurentUser();
            usrName = Session["IBT_UserName"].ToString();

            string pageName = (System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath));

            dtPageControls = clsCom.getUserRoleAndPageControls(usrName, pageName);
            Session["PagePermissions"] = dtPageControls;



            bool x = clsCom.getPermissions(usrName, pageName);

            if (x == true)
            {

                //temp_clickCount = 0;****

                //getIBTColumns(); 
                //getMatchingStatus();



                if (!IsPostBack)
                {
                    getMatchingStatus();
                    LoadReceiptStatus();
                    getProduct();
                    getDataCategory();
                    Export_Count = 0;

                }

                this.SetFocus(ddlHistory);
                //this.SetFocus(btnSetFocusTop);
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }


        //-----------------------------------------------------------------------Search Function---------------------------------------------------------------------------//

        protected void btnSearch_Click(object sender, EventArgs e)//---------------------------------------Search Button Click Event---------------------------------------//
        {
            try
            {
                String SQLWhere = "";
                string selected_to_date = "";
                string selected_from_date = "";
                string temp_usr_role = "";
                qry = "";
                qry_orderby = "";

                //---------------------------------------------------------User Permissions Validate---------------------------------------------------------//

                //OracleConnection conn_get_searchPermissions = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                //conn_get_searchPermissions.Open();
                myConnectionUse.Close();
                myConnectionUse.Open();

                OracleCommand cmd_get_searchPermissions = myConnectionUse.CreateCommand();//conn_get_searchPermissions.CreateCommand();
                cmd_get_searchPermissions.CommandText = "select ra.user_name, ur.role_id,ra.inactive, ur.role_description, rc.department_id from fas_ibt_role_assigned ra " +
                                                        "inner join fas_ibt_usr_roles ur on ra.role_id = ur.role_id " +
                                                        "inner join fas_ibt_rolecat rc on ur.role_id = rc.role_id " +
                                                        "where ra.user_name = '" + usrName + "'";

                OracleCommand cmd_formControl_Validation = myConnectionUse.CreateCommand();//conn_get_searchPermissions.CreateCommand();
                cmd_formControl_Validation.CommandText = "select count (*) as formCtrlCount from fas_ibt_pagecontrols pc inner join fas_ibt_role_assigned ra on ra.role_id = pc.role_id " +
                                                            "where ra.user_name = '" + usrName + "'";

                OracleDataReader odr_formControl_Validation = cmd_formControl_Validation.ExecuteReader();

                int formControlsCount = 0;

                while (odr_formControl_Validation.Read())
                {
                    formControlsCount = int.Parse(odr_formControl_Validation["formCtrlCount"].ToString());
                }

                if (formControlsCount == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "No Form Controls Available For Active User , Please Contact System Administrator...!";
                    return;
                }

                DataTable dt_SearchPermission = new DataTable();
                OracleDataAdapter oda_getSearchPermission = new OracleDataAdapter(cmd_get_searchPermissions);
                oda_getSearchPermission.Fill(dt_SearchPermission);

                OracleDataReader odr_getSearchPermission = cmd_get_searchPermissions.ExecuteReader();

                while (odr_getSearchPermission.Read())
                {
                    temp_usr_role = odr_getSearchPermission["role_description"].ToString();//assign user role description to variable
                }
                if (temp_usr_role == "")
                {
                    call_error_msg(false);
                    lblResult.Text = "No Data Categories Available For Active User , Please Contact System Administrator...!";
                    return;
                }
                if (temp_usr_role != "SUPER_USER")
                {
                    //if (Session["Division"].ToString() == "LIFE")
                    if (Session["IBT_Company"].ToString() == "LIFE")
                    {
                        if ((ddlProduct.SelectedIndex == 0) || (ddlProduct.SelectedIndex == 1) || (ddlProduct.SelectedIndex == 3))
                        {
                            call_error_msg(false);
                            lblResult.Text = "Search Result Not Available. Please Select 'Life' Product And Try Again...!";

                            grdSearchData.DataSource = null;
                            grdSearchData.DataBind();

                            return;
                        }
                        else
                        {
                            validto_search = true;
                        }
                    }

                    else if (Session["IBT_Company"].ToString() == "GENERAL")
                    {
                        if ((ddlProduct.SelectedIndex == 0) || (ddlProduct.SelectedIndex == 2) || (ddlProduct.SelectedIndex == 4))
                        {
                            call_error_msg(false);
                            lblResult.Text = "Search Result Not Available. Please Select 'General' Product And Try Again...!";

                            grdSearchData.DataSource = null;
                            grdSearchData.DataBind();
                            return;
                        }
                        else
                        {
                            validto_search = true;
                        }
                    }
                    else
                    {
                        call_error_msg(false);
                        lblResult.Text = "Search Result Not Available...!";

                        grdSearchData.DataSource = null;
                        grdSearchData.DataBind();

                        return;
                    }
                }

                //----------------------------------Data Category Validation---------------------------//


                int usr_selectedDataCatVal = -1;
                usr_selectedDataCatVal = int.Parse(ddlDataCategory.SelectedValue);//SelectedIndex;//user input


                List<Int32> dataCat_data = new List<Int32>();

                foreach (DataRow row in dt_SearchPermission.Rows)
                {
                    dataCat_data.Add((Int32)Convert.ToInt32(row["DEPARTMENT_ID"]));
                }

                Array department_id = dataCat_data.ToArray();



                if (temp_usr_role == "SUPER_USER")
                {
                    validto_search = true;
                }

                if (temp_usr_role != "SUPER_USER")
                {
                    if ((Array.IndexOf(department_id, usr_selectedDataCatVal) != -1))
                    {
                        validto_search = true;
                    }
                    else
                    {
                        validto_search = false;
                        call_error_msg(false);
                        lblResult.Text = "Invalid Search, Please Check The Data Category And Try Again...!";

                        grdSearchData.DataSource = null;
                        grdSearchData.DataBind();
                    }
                    //foreach (int i in department_id)
                    //{
                    //    if (usr_selectedDataCatVal == i)
                    //    {
                    //        validto_search = true;
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        validto_search = false;
                    //        call_error_msg(false);
                    //        lblResult.Text = "Invalid Search, Please Check The Data Category And Try Again...!";

                    //        grdSearchData.DataSource = null;
                    //        grdSearchData.DataBind();
                    //        //return;
                    //    }
                    //}
                }

                //-------------------------------------------------------------------------------------------------//

                if (validto_search == true)
                {

                    string CurrentDate = System.DateTime.Today.Date.ToShortDateString();

                    //----------------------Date---------------------//
                    if (ddlDate.SelectedIndex == 0)
                    {
                        SQLWhere = "WHERE 1 = 1";
                    }
                    else if (ddlDate.SelectedIndex == 1)
                    {
                        SQLWhere = "WHERE (trunc(transaction_date)) = to_date ('" + CurrentDate + "','DD/MM/RRRR')";
                    }
                    else if (ddlDate.SelectedIndex == 2)
                    {
                        SQLWhere = "WHERE (trunc(transaction_date)) <> to_date ('" + CurrentDate + "','DD/MM/RRRR')";
                    }
                    else if (ddlDate.SelectedIndex == 3)
                    {
                        if ((txtToDate.Text == "") || (txtFromDate.Text == ""))
                        {
                            call_error_msg(false);
                            lblResult.Text = "Date Fields Can not be empty...!";
                            return;
                        }
                        else
                        {
                            DateTime to_d = Convert.ToDateTime(txtToDate.Text);
                            DateTime from_d = Convert.ToDateTime(txtFromDate.Text);


                            if ((from_d > to_d) || (to_d < from_d))
                            {
                                call_error_msg(false);
                                lblResult.Text = "Date Range Error...!";
                                return;
                            }
                            else
                            {
                                SQLWhere = "WHERE (trunc(transaction_date)) BETWEEN to_date('" + from_d.ToShortDateString() + "', 'DD/MM/RRRR') AND to_date('" + to_d.ToShortDateString() + "', 'DD/MM/RRRR')";
                            }
                        }
                    }

                    //----------------------------------------------------//


                    //----------------------IBT---------------------------//

                    if (ddlIBT.SelectedIndex == 0)
                    {
                        SQLWhere = SQLWhere + " AND 1 = 1";
                    }
                    else if (ddlIBT.SelectedIndex == 1)
                    {
                        SQLWhere = SQLWhere + " AND IBT_STATUS = 1";
                    }
                    else if (ddlIBT.SelectedIndex == 2)
                    {
                        SQLWhere = SQLWhere + " AND IBT_STATUS = 0";
                    }
                    //----------------------------------------------------//


                    //---------------------Table Cols--------------------//

                    if (ddlIbtColumns.SelectedIndex != 0)
                    {
                        if (txtIbtColumn.Text.Trim() != "")
                        {
                            SQLWhere = SQLWhere + " AND " + ddlIbtColumns.SelectedItem + " LIKE '%" + txtIbtColumn.Text.Trim() + "%'";
                        }
                        else
                        {
                            call_error_msg(false);
                            lblResult.Text = "Search Value Can Not Be Empty...!";

                            grdSearchData.DataSource = null;
                            grdSearchData.DataBind();
                            summaryDiv.Visible = false;

                            return;
                        }

                    }


                    //if (txtIbtColumn.Text.Trim() != "")
                    //{
                    //    if (ddlIbtColumns.SelectedIndex != 0)
                    //    {
                    //        SQLWhere = SQLWhere + " AND " + ddlIbtColumns.SelectedItem + " LIKE '%" + txtIbtColumn.Text.Trim() + "%'";
                    //    }
                    //}


                    //---------------------------------------------------//


                    //---------History (Effective Date Filter)----------//

                    if (ddlHistory.SelectedIndex == 0)
                    {
                        SQLWhere = SQLWhere + " AND effective_end_date IS NULL";
                        qry_orderby = "serial_no";
                    }
                    else
                    {
                        qry_orderby = "id";
                    }
                    //---------------------------------------------------------------------//


                    //--------------------------------Matched -----------------------------//

                    if (ddlMatching.SelectedIndex == 0)
                    {
                        SQLWhere = SQLWhere + " AND 1 = 1";//All
                    }
                    if (ddlMatching.SelectedIndex == 1)
                    {
                        SQLWhere = SQLWhere + " AND matching_status = 0";//Unmatched
                    }
                    if (ddlMatching.SelectedIndex == 2)
                    {
                        SQLWhere = SQLWhere + " AND matching_status = 1";//possible
                    }
                    if (ddlMatching.SelectedIndex == 3)
                    {
                        SQLWhere = SQLWhere + " AND matching_status = 2";//exact
                    }
                    if (ddlMatching.SelectedIndex == 4)
                    {
                        SQLWhere = SQLWhere + " AND matching_status = 3";//Confirmed
                    }
                    if (ddlMatching.SelectedIndex == 5)
                    {
                        SQLWhere = SQLWhere + " AND matching_status = 4";//Manually Matched
                    }
                    if (ddlMatching.SelectedIndex == 6)
                    {
                        SQLWhere = SQLWhere + " AND matching_status = 5";
                    }

                    //---------------------------------------------------------------------//


                    //----------------------Receipt Status---------------------------------//

                    if (ddlreceipt_status.SelectedIndex == 0)
                    {
                        SQLWhere = SQLWhere + " AND 1 = 1 ";//All
                    }
                    if (ddlreceipt_status.SelectedIndex == 1)
                    {
                        SQLWhere = SQLWhere + " AND RECEIPT_STATUS = 1";//Pending FeedBack
                    }
                    if (ddlreceipt_status.SelectedIndex == 2)
                    {
                        SQLWhere = SQLWhere + " AND RECEIPT_STATUS = 2";//Error - Excess
                    }
                    if (ddlreceipt_status.SelectedIndex == 3)
                    {
                        SQLWhere = SQLWhere + " AND RECEIPT_STATUS = 3";//Error - No Debit Found
                    }
                    if (ddlreceipt_status.SelectedIndex == 4)
                    {
                        SQLWhere = SQLWhere + " AND RECEIPT_STATUS = 4";//Error - Reinstate
                    }
                    if (ddlreceipt_status.SelectedIndex == 5)
                    {
                        SQLWhere = SQLWhere + " AND RECEIPT_STATUS = 5";//Successfully Receipted
                    }
                    if (ddlreceipt_status.SelectedIndex == 6)
                    {
                        SQLWhere = SQLWhere + " AND RECEIPT_STATUS = 6";//Receipt Reversed
                    }
                    if (ddlreceipt_status.SelectedIndex == 7)
                    {
                        SQLWhere = SQLWhere + " AND RECEIPT_STATUS = 7";//Receipt Error - TCS
                    }
                    if (ddlreceipt_status.SelectedIndex == 8)
                    {
                        SQLWhere = SQLWhere + " AND RECEIPT_STATUS = 8";//No Receipt Value '-'
                    }
                    if (ddlreceipt_status.SelectedIndex == 9)
                    {
                        SQLWhere = SQLWhere + " AND RECEIPT_STATUS = 9";//Manually Receipted
                    }

                    //----------------------------------------------------------------------//

                    //---------------------------Bulk Receipt Status------------------------//

                    if (ddlBulkReceiptStatus.SelectedIndex == 0)
                    {
                        SQLWhere = SQLWhere + " AND 1 = 1 ";
                    }
                    if (ddlBulkReceiptStatus.SelectedIndex == 1)
                    {
                        SQLWhere = SQLWhere + " AND bulk_receipt_ind = '1' ";
                    }
                    if (ddlBulkReceiptStatus.SelectedIndex == 2)
                    {
                        SQLWhere = SQLWhere + " AND bulk_receipt_ind = '0' ";
                    }
                    //----------------------------------------------------------------------//


                    //---------------------------Product------------------------------------//

                    if (ddlProduct.SelectedIndex == 0)
                    {
                        SQLWhere = SQLWhere + " AND 1 = 1 ";//All
                    }
                    if (ddlProduct.SelectedIndex == 1)
                    {
                        SQLWhere = SQLWhere + " AND PRODUCT = 1";//Conventional - General
                    }
                    if (ddlProduct.SelectedIndex == 2)
                    {
                        SQLWhere = SQLWhere + " AND PRODUCT = 2";//Conventional - Life
                    }
                    if (ddlProduct.SelectedIndex == 3)
                    {
                        SQLWhere = SQLWhere + " AND PRODUCT = 3";//Takaful - General
                    }
                    if (ddlProduct.SelectedIndex == 4)
                    {
                        SQLWhere = SQLWhere + " AND PRODUCT = 4";//Takaful - Life
                    }

                    //----------------------------------------------------------------------------------//


                    //------------------------------Data Category(Department)---------------------------//

                    int dataCatID = -1;

                    dataCatID = int.Parse(ddlDataCategory.SelectedValue);//Index

                    //if ((dataCatID == 0) && (dataCatID != -1))
                    if (dataCatID == -1)
                    {
                        SQLWhere = SQLWhere + " AND 1 = 1 ";//All
                    }

                    else
                    {
                        SQLWhere = SQLWhere + " AND DEPARTMENT = " + dataCatID;
                    }

                    //---------------------------------------------------------------------------------//

                    //Main Search Query
                    qry = "select account_no,serial_no, to_char(TRANSACTION_DATE, 'DD/MM/RRRR') AS transaction_date,description,policy_no,bank_ref,bank_branch,cheque_no,debit,credit,ibt_status, id, balance, openning_bal, clossing_bal, createdby, createddate, effective_end_date, matching_status, user_comment, type_comment, product, receipt_status, RECEIPT_NO, CASH_ACC_NO, FORMAT_ID, POLICY_NO_UPDATED,RECEIPT_NARRATION, DEPARTMENT, MANUALLY_MATCHED_IND, bulk_receipt_ind,FAS_IBT_GET_SERIAL_WISE_COUNT(a.serial_no) as SerailCount  from fas_ibt_uploaded_dtl a " + SQLWhere + " order by " + qry_orderby; //id";//serial_no

                    //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                    //conn.Open();

                    myConnectionUse.Close();
                    myConnectionUse.Open();

                    OracleCommand cmd = myConnectionUse.CreateCommand();//conn.CreateCommand();
                    cmd.CommandText = qry;
                    //temp_qry = qry;

                    OracleDataReader odr = cmd.ExecuteReader();


                    if (odr.HasRows == true)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(odr);

                        //dt.Load(odr);

                        DataView dv = new DataView(dt);
                        DataTable dt_new = dv.ToTable(true, "id", "account_no", "serial_no", "transaction_date", "policy_no", "balance", "matching_status", "effective_end_date", "DEPARTMENT", "bulk_receipt_ind", "bank_ref", "description", "createdby", "SerailCount");

                        grdSearchData.DataSource = dt_new;
                        grdSearchData.DataBind();

                        // grdSearchData.Columns[4].HeaderStyle.Width = 2;

                        Session["GrdData"] = dt;

                        call_error_msg(true);
                        lblResult.Text = "Search Results...!";

                        getSummary();
                    }
                    else
                    {
                        call_error_msg(false);
                        lblResult.Text = "No Data Records Found...!";

                        grdSearchData.DataSource = null;
                        grdSearchData.DataBind();

                        getSummary();
                    }

                    this.SetFocus(cmdViewRule);

                    return;
                }
                else//end of valid to search
                {
                    return;
                }

            }

            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "----IBT Uploads Search Error---- Search By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                //throw;
            }

            finally
            {

            }
        }

        public void getIBTColumns()//----------------------------------------------------------------------Get IBT columns from table (method not used yet)----------------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();

            myConnectionUse.Close();
            myConnectionUse.Open();
            OracleCommand cmd = myConnectionUse.CreateCommand();//conn.CreateCommand();
            cmd.CommandText = "select t.* from fas_ibt_columns t";

            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);

            ddlIbtColumns.DataTextField = "description";
            ddlIbtColumns.DataValueField = "id";
            //ddlIbtColumns.Items.Insert(0, new ListItem("--- Please Select ---","0"));
            ddlIbtColumns.DataSource = dt;
            ddlIbtColumns.DataBind();

        }

        protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)//--------------------------Dropdownlist (date search) Selected Index Change----------------//
        {
            if (ddlDate.SelectedIndex == 3)
            {
                cal_div.Visible = true;
                cal_div2.Visible = true;
            }
            else
            {
                cal_div.Visible = false;
                cal_div2.Visible = false;
                txtFromDate.Text = "";
                txtToDate.Text = "";
            }
        }

        public void getMatchingStatus()//------------------------------------------------------------------Get Matching Status From Table (For Search Dropdownlist)--------//
        {
            try
            {
                //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                //conn.Open();

                myConnectionUse.Close();
                myConnectionUse.Open();

                OracleCommand cmd2 = myConnectionUse.CreateCommand();//conn.CreateCommand();
                cmd2.CommandText = "select * from fas_ibt_matchingstatus t order by t.value";

                OracleDataAdapter oda2 = new OracleDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                oda2.Fill(dt2);

                ddlMatching.DataTextField = "MATCHING_DESCRIPTION";
                ddlMatching.DataValueField = "value";
                ddlMatching.DataSource = dt2;
                ddlMatching.DataBind();

                ddlMatching.Items.Insert(0, new ListItem("All", "-1"));

                myConnectionUse.Close();
                //conn.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void getProduct()//-------------------------------------------------------------------------Get Products For ddlProduct Dropdownlist------------------------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();
            myConnectionUse.Close();
            myConnectionUse.Open();

            OracleCommand cmd2 = myConnectionUse.CreateCommand();//conn.CreateCommand();
            cmd2.CommandText = "select * from fas_ibt_products t order by t.value ";//new33333333333

            OracleDataAdapter oda2 = new OracleDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            oda2.Fill(dt2);

            ddlProduct.DataTextField = "PRODUCT_DESCRIPTION";
            ddlProduct.DataValueField = "VALUE";
            ddlProduct.DataSource = dt2;
            ddlProduct.DataBind();

            ddlProduct.Items.Insert(0, new ListItem("All", "-1"));
            myConnectionUse.Close();
            //conn.Close();
        }

        public void LoadReceiptStatus()//------------------------------------------------------------------Get Receipt Status For ddlreceipt_status Dropdownlist-----------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();
            myConnectionUse.Close();
            myConnectionUse.Open();

            OracleCommand cmd = myConnectionUse.CreateCommand();//conn.CreateCommand();
            cmd.CommandText = "select * from fas_ibt_receipt_status k order by k.value ";
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);

            ddlreceipt_status.DataTextField = "RECEIPTSTATUS_DESCRIPTION";
            ddlreceipt_status.DataValueField = "VALUE";
            ddlreceipt_status.DataSource = dt;
            ddlreceipt_status.DataBind();

            ddlreceipt_status.Items.Insert(0, new ListItem("---Please Select---", "0"));


        }

        public void getDataCategory()//--------------------------------------------------------------------Get Data Category (Departments)---------------------------------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());*************SPECIAL CHANGE**************
            //conn.Open();

            myConnectionUse.Close();
            myConnectionUse.Open();


            OracleCommand cmd2 = myConnectionUse.CreateCommand();//conn.CreateCommand();
            //cmd2.CommandText = "select d.value, d.department_description from fas_ibt_departments d order by d.value";

            cmd2.CommandText = "select k.value, k.department_description from fas_ibt_departments k where k.value in(select d.department_id  " +
                               "from fas_ibt_rolecat d where d.role_id = (select t.role_id from fas_ibt_role_assigned t where t.user_name = '" + usrName + "'))";

            OracleDataAdapter oda = new OracleDataAdapter(cmd2);
            DataTable dt = new DataTable();

            oda.Fill(dt);

            ddlDataCategory.DataTextField = "department_description";
            ddlDataCategory.DataValueField = "value";

            ddlDataCategory.DataSource = dt;
            ddlDataCategory.DataBind();

            ddlDataCategory.Items.Insert(0, new ListItem("All", "-1"));
            myConnectionUse.Close();
            //conn.Close();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //------------------------------------------------------------------------Search Result Grid-----------------------------------------------------------------------//

        public void page_controls_authentication(bool isHistoryRecord, bool isManuallyReceipted, bool isConfirmed)//--Popup Color / Controllers Handle---------------------//
        {

            //-------------------HISTORY RECORD-----------------//

            if (isHistoryRecord == true)
            {
                btn_UpdateRecord.Enabled = false;
                txt_Description.Enabled = false;
                txt_PolicyNo.Enabled = false;
                ddl_IBTStatus.Enabled = false;
                ddl_IBTStatus.Enabled = false;
                ddl_MatchingStatus.Enabled = false;
                ddl_Produt.Enabled = false;
                ddl_DataCategory.Enabled = false;
                ddl_ReceiptedMethod.Enabled = false;
                txt_Rec_No.Enabled = false;
                txt_Rec_Narration.Enabled = false;
                txt_usrComment.Enabled = false;

                //popupHeader  modalPopup
                this.popupHeader.Style.Add("background-color", "#FF0000");
                this.popupHeader.Style.Add("border-color", "#FF0000");
                this.pnlPopup.Style.Add("background-color", "#FFEFD5");

            }
            else
            {
                btn_UpdateRecord.Enabled = true;
                txt_Description.Enabled = true;
                txt_PolicyNo.Enabled = true;
                ddl_IBTStatus.Enabled = true;
                ddl_IBTStatus.Enabled = true;
                ddl_MatchingStatus.Enabled = true;
                ddl_Produt.Enabled = true;
                ddl_DataCategory.Enabled = true;
                ddl_ReceiptedMethod.Enabled = true;
                txt_Rec_No.Enabled = true;
                txt_Rec_Narration.Enabled = true;
                txt_usrComment.Enabled = true;

                //popupHeader
                this.popupHeader.Style.Add("background-color", "#18919b");
                this.popupHeader.Style.Add("border-color", "#18919b");
                this.pnlPopup.Style.Add("background-color", "#FFFFFF");

                //-------------------MANUALLY RECEIPTED-------------//

                if (isManuallyReceipted == true)
                {
                    btn_UpdateRecord.Enabled = false;
                    txt_Description.Enabled = false;
                    txt_PolicyNo.Enabled = false;
                    ddl_IBTStatus.Enabled = false;
                    ddl_IBTStatus.Enabled = false;
                    ddl_MatchingStatus.Enabled = false;
                    ddl_Produt.Enabled = false;
                    ddl_DataCategory.Enabled = false;
                    ddl_ReceiptedMethod.Enabled = false;
                    txt_Rec_No.Enabled = false;
                    txt_Rec_Narration.Enabled = false;
                    txt_usrComment.Enabled = false;

                    //popupHeader  modalPopup
                    this.popupHeader.Style.Add("background-color", "#984A84");
                    this.popupHeader.Style.Add("border-color", "#984A84");
                    this.pnlPopup.Style.Add("background-color", "#FFFFFF");
                }

                if (isConfirmed == true)
                {
                    btn_UpdateRecord.Enabled = false;
                    txt_Description.Enabled = false;
                    txt_PolicyNo.Enabled = false;
                    ddl_IBTStatus.Enabled = false;
                    ddl_IBTStatus.Enabled = false;
                    ddl_MatchingStatus.Enabled = false;
                    ddl_Produt.Enabled = false;
                    ddl_DataCategory.Enabled = false;
                    ddl_ReceiptedMethod.Enabled = false;
                    txt_Rec_No.Enabled = false;
                    txt_Rec_Narration.Enabled = false;
                    txt_usrComment.Enabled = false;

                    //popupHeader  modalPopup
                    this.popupHeader.Style.Add("background-color", "#ffcc00");
                    this.popupHeader.Style.Add("border-color", "#ffcc00");
                    this.pnlPopup.Style.Add("background-color", "#FFFFFF");
                }

            }

            validateCommon_controls(isHistoryRecord);//disable controls
        }



        public void IBT_HistoryUpdate(string type, string product, string parameter, string value, string SerilaNo)
        {
            try
            {
                myConnectionUse.Close();
                myConnectionUse.Open();
                {


                    OracleCommand cmd = myConnectionUse.CreateCommand();
                    cmd.CommandText = "SP_FAS_IBT_POSSIBLE_AUTO";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vType", OracleType.VarChar).Value = type;
                    cmd.Parameters.Add("vProduct", OracleType.VarChar).Value = product;
                    cmd.Parameters.Add("vParameter", OracleType.VarChar).Value = parameter;
                    cmd.Parameters.Add("vValue", OracleType.VarChar).Value = value;
                    cmd.Parameters.Add("VSerialNo", OracleType.VarChar).Value = SerilaNo;

                    cmd.ExecuteNonQuery();

                    lblResult.Text = "Command Execution Successfull..!";

                    myConnectionUse.Close();


                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Boolean CheckRecordUpdateByOutSide (String SerialNo, Double PrevCount)
        {
            Boolean isUpdatable = false;
            try
            {
                Double NewCount = 0;

                OracleConnection conn_GetUserRole = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn_GetUserRole.Open();

                OracleCommand cmd = conn_GetUserRole.CreateCommand();
                cmd.CommandText = "select count(dd.id) as NewCount from fas_ibt_uploaded_dtl dd where dd.serial_no = '" + SerialNo + "'";

                OracleDataAdapter oda = new OracleDataAdapter(cmd);

                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();

                oda.Fill(dt);

                NewCount = Convert.ToDouble(dt.Rows[0]["NewCount"].ToString());

                if (NewCount == PrevCount)
                {
                    isUpdatable = true;
                }
                else
                {
                    isUpdatable = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return isUpdatable;
        }


        protected void btn_UpdateRecord_Click(object sender, EventArgs e)//---------------------------------------Update Selected Record-----------------------------------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();

            //validation for same 06/10/2017
            if (!CheckRecordUpdateByOutSide(lbl_Serial.Text,Convert.ToDouble(lblSerial_Count.Text)))
            {
                call_error_msg(false);
                lblResult.Text = "Some one is trying to update the same record please refresh the page and try to update your record again...";
                return;
            }



            if (ddl_AddHistory.SelectedValue == "1")
            {
                //11-09-2017
                //IBT_HistoryUpdate("Other", "Life", "", txt_PolicyNo.Text.Trim(), lbl_Serial.Text);

                IBT_HistoryUpdate("Other", "Life", txt_Description.Text.Trim(), txt_PolicyNo.Text.Trim(), lbl_Serial.Text);
            }


            myConnectionUse.Open();

            try
            {
                int update_upload_dtl_id = int.Parse(lbl_mainID.Text.Trim());//-------------------------------------------------ID
                string account_no = lbl_AccountNo.Text;//-----------------------------------------------------------------------Account Number
                string serial_no = lbl_Serial.Text;//---------------------------------------------------------------------------Serial number
                string update_trans_date = lbl_TransactionDate.Text;//----------------------------------------------------------Transaction Date
                string update_description = txt_Description.Text.Trim();//------------------------------------------------------Description
                string update_policy_no = txt_PolicyNo.Text.Trim();//-----------------------------------------------------------Policy Number
                string update_bankRef = lbl_BankRef.Text;//---------------------------------------------------------------------Bank Ref
                string update_bankBranch = lbl_BranchCode.Text;//---------------------------------------------------------------Bank Branch
                string update_chqNo = lbl_Chequeno.Text;//----------------------------------------------------------------------Chq number
                double update_debit = double.Parse(lbl_Debit.Text);//-----------------------------------------------------------Debit
                double update_credit = double.Parse(lbl_Credit.Text);//---------------------------------------------------------Credit
                double update_balance = double.Parse(lbl_RunningTot.Text);//----------------------------------------------------Running total
                double update_openning_balance = double.Parse(lbl_OpenningBal.Text);//------------------------------------------Opening Balance
                double update_clossing_balance = double.Parse(lbl_ClossingBal.Text);//------------------------------------------Closing Balance
                string update_created_by = usrName;//---------------------------------------------------------------------------Created By
                string update_created_date = lbl_Created_Date.Text;//-----------------------------------------------------------Created Date
                int update_ibtStatus = int.Parse(ddl_IBTStatus.SelectedValue);//------------------------------------------------IBT Status
                string update_effective_end_date = null;//----------------------------------------------------------------------Effective End Date Value For New Record
                int update_matching_status = int.Parse(ddl_MatchingStatus.SelectedValue);//-------------------------------------Matching Status
                string update_user_comment = txt_usrComment.Text.Trim();//------------------------------------------------------User Comment
                string update_type_comment = "";//------------------------------------------------------------------------------Type Comment
                int update_product_status = int.Parse(ddl_Produt.SelectedValue);//----------------------------------------------Product
                string update_Receipt_No = txt_Rec_No.Text.Trim();//------------------------------------------------------------Receipt Number
                string receipt_status_value = lbl_Rec_Status.Text;//------------------------------------------------------------Receipt Status For View
                string update_CashAcc_No = lbl_CashAccount.Text;//--------------------------------------------------------------Cash Account Number
                int update_Format_ID = -1;//------------------------------------------------------------------------------------Referred Format For Statement Upload
                int update_PolicyNoUpdated = 2;//-------------------------------------------------------------------------------Policy Number Updating Indicator (2 For User)
                string update_Receipt_Narration = txt_Rec_Narration.Text.Trim();//----------------------------------------------Receipt Narration
                int update_Data_Category = int.Parse(ddl_DataCategory.SelectedValue);//-----------------------------------------Department
                int update_Receipted_Method = int.Parse(ddl_ReceiptedMethod.SelectedValue);//-----------------------------------ManuallyMatched Indicator
                int update_Bulk_Receipt_Ind = int.Parse(ddl_bulkReceiptStatus.SelectedValue);//---------------------------------Bulk Receipt Indicator
                string effective_end_date_history = "";//-----------------------------------------------------------------------Ef End Date For Excel Export with History Records
                int update_receipt_status = int.Parse(lbl_ReceiptStatusVal.Text);//0;//-----------------------------------------Receipt Status for table (Integer Value)



                OracleCommand cmd = myConnectionUse.CreateCommand();//conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_Uploaded_dtl_update";
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("vIDToUpdate", OracleType.Int32).Value = update_upload_dtl_id;
                cmd.Parameters.AddWithValue("vAccount_No", OracleType.VarChar).Value = account_no;
                cmd.Parameters.AddWithValue("vSerial_No", OracleType.VarChar).Value = serial_no;
                cmd.Parameters.AddWithValue("vTransDate", OracleType.DateTime).Value = Convert.ToDateTime(update_trans_date);
                //statementdate
                //valuedate
                cmd.Parameters.AddWithValue("vDescription", OracleType.VarChar).Value = update_description;
                cmd.Parameters.AddWithValue("vPolicyNo", OracleType.VarChar).Value = update_policy_no;
                cmd.Parameters.AddWithValue("vBankRef", OracleType.VarChar).Value = update_bankRef;
                cmd.Parameters.AddWithValue("vBankBranch", OracleType.VarChar).Value = update_bankBranch;
                cmd.Parameters.AddWithValue("vChequeNo", OracleType.VarChar).Value = update_chqNo;
                cmd.Parameters.AddWithValue("vDebit", OracleType.Double).Value = update_debit;
                cmd.Parameters.AddWithValue("vCredit", OracleType.Double).Value = update_credit;
                cmd.Parameters.AddWithValue("vBalance", OracleType.Double).Value = update_balance;
                cmd.Parameters.AddWithValue("vOpenningBal", OracleType.Double).Value = update_openning_balance;
                cmd.Parameters.AddWithValue("vClossingBal", OracleType.Double).Value = update_clossing_balance;
                cmd.Parameters.AddWithValue("vCreatedBy", OracleType.VarChar).Value = update_created_by;
                cmd.Parameters.AddWithValue("vCreatedDate", OracleType.DateTime).Value = Convert.ToDateTime(update_created_date);
                cmd.Parameters.AddWithValue("vIbtStatus", OracleType.VarChar).Value = update_ibtStatus;
                cmd.Parameters.AddWithValue("vEffectiveEndDate", OracleType.DateTime).Value = Convert.ToDateTime(update_effective_end_date);
                //cmd.Parameters.AddWithValue("vMatchingStatus", OracleType.Int32).Value = update_matching_status; //matching status value
                cmd.Parameters.AddWithValue("vUserComment", OracleType.VarChar).Value = update_user_comment;
                cmd.Parameters.AddWithValue("vTypeComment", OracleType.VarChar).Value = update_type_comment;
                cmd.Parameters.AddWithValue("vProduct", OracleType.Int32).Value = update_product_status;
                cmd.Parameters.AddWithValue("vReceiptNo", OracleType.VarChar).Value = update_Receipt_No;
                //cmd.Parameters.AddWithValue("vReceiptStatus", OracleType.Int32).Value = update_receipt_status; //receipt_status_value;
                cmd.Parameters.AddWithValue("vCashAccNo", OracleType.VarChar).Value = update_CashAcc_No;
                cmd.Parameters.AddWithValue("vFormatId", OracleType.Int32).Value = update_Format_ID;
                cmd.Parameters.AddWithValue("vPolicyNoUpdated", OracleType.Int32).Value = update_PolicyNoUpdated;
                cmd.Parameters.AddWithValue("vReceiptNarration", OracleType.VarChar).Value = update_Receipt_Narration;
                cmd.Parameters.AddWithValue("vDepartment", OracleType.Int32).Value = update_Data_Category;
                cmd.Parameters.AddWithValue("vManuallyMatched_Ind", OracleType.Int32).Value = update_Receipted_Method;
                cmd.Parameters.AddWithValue("vBulkReceipt_Ind", OracleType.Int32).Value = update_Bulk_Receipt_Ind;

                //------------------------------Rules IDs Validation--------------------------------//

                if (DepartmentExisting != update_Data_Category.ToString())
                {
                    //change dep rule id
                    Dep_Rule_ID = 2;
                }

                if (IBTValueExisting != update_ibtStatus.ToString())
                {
                    //change bifuercation rule id
                    Bifurcate_Rule_ID = -1;
                }

                if (MatchingStatusExisting != update_matching_status.ToString())
                {
                    //change matching rule id
                    Matching_Rule_ID = -1;
                }

                cmd.Parameters.AddWithValue("vDep_RuleID", OracleType.Int32).Value = Dep_Rule_ID;
                cmd.Parameters.AddWithValue("vBifercate_RuleID", OracleType.Int32).Value = Bifurcate_Rule_ID;
                cmd.Parameters.AddWithValue("vMatchingRuleID", OracleType.Int32).Value = Matching_Rule_ID;


                bool valid_to_update_dataCat = false;
                bool valid_to_update_manuallyMatched = false;
                bool valid_to_update_bulkReceipt_Ind = false;
                bool valid_to_update_regularReceipt = false;

                valid_to_update_dataCat = chk_dataCategoryValue_forUpdate(update_upload_dtl_id, update_Data_Category);//Departments Validation (Active / Inactive)
                valid_to_update_manuallyMatched = chk_manuallyMatched_forUpdate(update_upload_dtl_id, update_Receipted_Method, update_Receipt_No, update_Receipt_Narration);//Receipting Validation
                valid_to_update_bulkReceipt_Ind = chk_bulkReceipt_forUpdate(update_upload_dtl_id, update_Bulk_Receipt_Ind);//Bulk Receipt Validation
                valid_to_update_regularReceipt = chk_regularReceipt_forUpdate(update_upload_dtl_id, update_Bulk_Receipt_Ind);

                //Main Validation For Update
                //Receipt Status 1 = Pending FeedBack; 5 = Successfully Receipted; Matching Status 3 = Confirmed 
                if ((effective_end_date_history.Length == 0) && (receipt_status_value != "Pending FeedBack") && (receipt_status_value != "Successfully Receipted") && (update_matching_status != 3) && (valid_to_update_dataCat == true) && (valid_to_update_manuallyMatched == true) && (valid_to_update_bulkReceipt_Ind == true) && (valid_to_update_regularReceipt == true))//Main Validation For Update
                {
                    if (manuallyReceipted_validation == 1)//manually receipted true
                    {
                        update_receipt_status = 9;
                        update_matching_status = 4;
                        cmd.Parameters.AddWithValue("vReceiptStatus", OracleType.Int32).Value = update_receipt_status;
                        cmd.Parameters.AddWithValue("vMatchingStatus", OracleType.Int32).Value = update_matching_status;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("vReceiptStatus", OracleType.Int32).Value = update_receipt_status;
                        cmd.Parameters.AddWithValue("vMatchingStatus", OracleType.Int32).Value = update_matching_status;
                    }
                    //***********************NEW Change****************************
                    if (myConnectionUse.State.ToString() == "Closed")
                    {
                        myConnectionUse.Open();
                    }
                    //**************************************************************
                    cmd.ExecuteNonQuery();

                    manuallyReceipted_validation = 0;

                    call_error_msg(true);
                    lblResult.Text = "Update Successfull...!";
                    CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----IBT Record Updated! ID: " + update_upload_dtl_id + "---- Active User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));

                    OracleCommand cmd_afterUpdateDataBind = new OracleCommand();
                    cmd_afterUpdateDataBind.Connection = myConnectionUse;//conn;
                    cmd_afterUpdateDataBind.CommandText = qry;//temp_qry;//qry;

                    OracleDataReader odr_afterUpdateDataBind = cmd_afterUpdateDataBind.ExecuteReader();

                    if (odr_afterUpdateDataBind.HasRows == true)
                    {
                        DataTable dt2 = new DataTable();
                        dt2.Load(odr_afterUpdateDataBind);

                        grdSearchData.DataSource = null;
                        grdSearchData.DataBind();

                        DataView dv = new DataView(dt2);
                        DataTable dt_new = dv.ToTable(true, "id", "account_no", "serial_no", "transaction_date", "policy_no", "balance", "matching_status", "effective_end_date", "DEPARTMENT", "bulk_receipt_ind", "bank_ref", "description", "createdby", "SerailCount");


                        grdSearchData.DataSource = dt_new;
                        grdSearchData.DataBind();

                        //btnSearch_Click(sender, e);//Calling Click Event

                        call_error_msg(true);
                        lblResult.Text = "Update Successfull...!";
                    }
                    else
                    {
                        grdSearchData.DataSource = null;
                        grdSearchData.DataBind();

                        call_error_msg(true);
                        lblResult.Text = "Update Successfull...!, No Data Records Available For Previous Filters!";
                    }
                    myConnectionUse.Close();
                    //conn.Close();
                    odr_afterUpdateDataBind.Close();

                    //this.SetFocus(cmdViewRule);

                }
                else
                {
                    string ErrorMsg = "";

                    ErrorMsg = "Update Fail...!";

                    call_error_msg(false);

                    if (valid_to_update_dataCat == false)
                    {
                        ErrorMsg = ErrorMsg + " [Data Category Error] ";
                    }

                    if (valid_to_update_manuallyMatched == false)
                    {
                        ErrorMsg = ErrorMsg + " [Manual Receipting Error] ";
                    }

                    if (valid_to_update_bulkReceipt_Ind == false)
                    {
                        ErrorMsg = ErrorMsg + " [Trying To Update 'Bulk - Pending Confirmation' Record...!] ";
                    }

                    if (valid_to_update_regularReceipt == false)
                    {
                        ErrorMsg = ErrorMsg + " [Trying To Update 'Regular - Pending Confirmation' Record...!] ";
                    }

                    lblResult.Text = ErrorMsg;

                }

                Dep_Rule_ID = 0;
                Bifurcate_Rule_ID = 0;
                Matching_Rule_ID = 0;
                DepartmentExisting = "";
                IBTValueExisting = "";
                MatchingStatusExisting = "";
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Update Fail...!" + ex.ToString();
                //throw;
            }
            finally
            {
                //if (conn != null)
                //{
                //    conn.Close();
                //    conn.Dispose();
                //}
                if (myConnectionUse != null)
                {
                    myConnectionUse.Close();
                    // myConnectionUse.Dispose();
                }
            }
        }

        protected void grdSearchData_RowCommand(object sender, GridViewCommandEventArgs e)//----------------------Search Result Details Popup------------------------------//
        {
            //OracleConnection conn_getData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn_getData.Open();
            myConnectionUse.Close();
            myConnectionUse.Open();

            try
            {
                if (e.CommandName == "BulkReceipt")
                {
                    Int64 Bulk_MainID = -1;
                    string Bulk_TransDate = "";
                    string Bulk_Serial = "";
                    string Bulk_PolicyNo = "";
                    decimal Bulk_Credit = 0;

                    //bool parseToBulkReceiptDetails = false;

                    grdBulkReceiptDetails.DataSource = null;
                    grdBulkReceiptDetails.DataBind();

                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    Int64 selectedRowID = Int64.Parse(grdSearchData.Rows[rowIndex].Cells[2].Text);

                    OracleCommand cmd_getSelectedRowData = myConnectionUse.CreateCommand();//conn_getData.CreateCommand();
                    cmd_getSelectedRowData.CommandText = "select t.id, t.serial_no,t.policy_no, t.transaction_date, t.credit " +
                                                         "from fas_ibt_uploaded_dtl t where t.id='" + selectedRowID + "' order by t.id";

                    OracleDataReader odr_getSelectedRowData = cmd_getSelectedRowData.ExecuteReader();
                    while (odr_getSelectedRowData.Read())
                    {
                        Bulk_MainID = selectedRowID;
                        Bulk_TransDate = odr_getSelectedRowData["transaction_date"].ToString();
                        Bulk_Serial = odr_getSelectedRowData["serial_no"].ToString();
                        Bulk_PolicyNo = odr_getSelectedRowData["policy_no"].ToString();
                        Bulk_Credit = Convert.ToDecimal(odr_getSelectedRowData["credit"].ToString());
                    }



                    //Send details to bulk receipt page as Cookie
                    HttpCookie cookie = new HttpCookie("BulkDetailVariables");
                    cookie.Values.Add("DtlID", Bulk_MainID.ToString());
                    cookie.Values.Add("DtlTransDate", Bulk_TransDate.ToString());
                    cookie.Values.Add("DtlSerialNo", Bulk_Serial.ToString());
                    cookie.Values.Add("DtlPolicyNo", Bulk_PolicyNo.ToString());
                    cookie.Values.Add("DtlTotalAmount", Bulk_Credit.ToString());

                    //aCookie.Value = DateTime.Now.ToString();
                    //aCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);
                    //---------------



                    myConnectionUse.Close();
                    //conn_getData.Close();
                    odr_getSelectedRowData.Close();

                    Response.Redirect("~/IBT/IBT_Bulk_Receipting.aspx", false);

                    return;
                }

                if (e.CommandName == "Select")
                {
                    bool isHistoryRecord = false;
                    bool isManuallyReceipted = false;
                    bool isConfirmedRecord = false;//New

                    int MainID = -1;
                    string AccountNo = "";
                    string Serial = "";
                    string CashAccount = "";
                    string PolicyNo = "";
                    string TransDate = "";
                    string BankRef = "";
                    string BankCode = "";
                    string ChqNo = "";
                    string Debit = "";
                    string Credit = "";
                    string RunningTot = "";
                    string OpenningBal = "";
                    string ClossingBal = "";
                    string Description = "";
                    string Product = "";
                    string IbtStatus = "";
                    string MatchingStatus = "";
                    string DataCategory = "";
                    string ReceiptStatus = "";
                    string ReceiptNo = "";
                    string ReceiptNarration = "";
                    string ReceiptMatchingMethod = "";//Manually
                    string BulkReceiptInd = "";//Bulk

                    string CreatedBy = "";
                    string CreatedDate = "";
                    string EffectiveEndDate = "";
                    string UserComment = "";
                    double OB_Calculated = 0;

                    double SerialCount = 0;



                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    int selectedRowID = int.Parse(grdSearchData.Rows[rowIndex].Cells[2].Text);

                    SerialCount = int.Parse(grdSearchData.Rows[rowIndex].Cells[15].Text);

                    OracleCommand cmd_getSelectedRowData = myConnectionUse.CreateCommand();//conn_getData.CreateCommand();
                    cmd_getSelectedRowData.CommandText = "select t.id, t.account_no,t.serial_no,t.policy_no, t.cash_acc_no, t.transaction_date, t.receipt_status, " +
                                                            "t.bank_ref, t.bank_branch, t.Cheque_No, t.debit, t.credit, t.balance, t.openning_bal, t.clossing_bal, " +
                                                            "t.Description, t.product, t.Ibt_Status, t.Matching_Status, t.department, " +
                                                            "t.Receipt_No,t.receipt_narration,t.manually_matched_ind, bulk_receipt_ind, " +
                                                            "t.createdby, t.createddate, t.effective_end_date, t.user_comment, " +
                                                            "t.department_rule_id, t.bifurcate_rule_id,t.matching_rule_id " +
                                                            "from fas_ibt_uploaded_dtl t where t.id='" + selectedRowID + "' order by t.id";

                    OracleDataReader odr_getSelectedRowData = cmd_getSelectedRowData.ExecuteReader();
                    while (odr_getSelectedRowData.Read())
                    {
                        MainID = int.Parse(odr_getSelectedRowData["id"].ToString());
                        AccountNo = odr_getSelectedRowData["account_no"].ToString();
                        Serial = odr_getSelectedRowData["serial_no"].ToString();
                        CashAccount = odr_getSelectedRowData["cash_acc_no"].ToString();
                        PolicyNo = odr_getSelectedRowData["policy_no"].ToString();
                        TransDate = odr_getSelectedRowData["transaction_date"].ToString();
                        BankRef = odr_getSelectedRowData["bank_ref"].ToString();
                        BankCode = odr_getSelectedRowData["bank_branch"].ToString();
                        ChqNo = odr_getSelectedRowData["Cheque_No"].ToString();
                        Debit = odr_getSelectedRowData["debit"].ToString();
                        Credit = odr_getSelectedRowData["credit"].ToString();
                        RunningTot = odr_getSelectedRowData["balance"].ToString();
                        OpenningBal = odr_getSelectedRowData["openning_bal"].ToString();
                        ClossingBal = odr_getSelectedRowData["clossing_bal"].ToString();
                        Description = odr_getSelectedRowData["Description"].ToString();
                        Product = odr_getSelectedRowData["product"].ToString();
                        IbtStatus = odr_getSelectedRowData["Ibt_Status"].ToString();
                        MatchingStatus = odr_getSelectedRowData["Matching_Status"].ToString();
                        DataCategory = odr_getSelectedRowData["department"].ToString();
                        ReceiptStatus = odr_getSelectedRowData["receipt_status"].ToString();
                        ReceiptNo = odr_getSelectedRowData["Receipt_No"].ToString();
                        ReceiptNarration = odr_getSelectedRowData["receipt_narration"].ToString();
                        ReceiptMatchingMethod = odr_getSelectedRowData["manually_matched_ind"].ToString();
                        BulkReceiptInd = odr_getSelectedRowData["bulk_receipt_ind"].ToString();
                        CreatedBy = odr_getSelectedRowData["createdby"].ToString();
                        CreatedDate = odr_getSelectedRowData["createddate"].ToString();
                        EffectiveEndDate = odr_getSelectedRowData["effective_end_date"].ToString();
                        UserComment = odr_getSelectedRowData["user_comment"].ToString();

                        Dep_Rule_ID = int.Parse(odr_getSelectedRowData["department_rule_id"].ToString());
                        Bifurcate_Rule_ID = int.Parse(odr_getSelectedRowData["bifurcate_rule_id"].ToString());
                        Matching_Rule_ID = int.Parse(odr_getSelectedRowData["matching_rule_id"].ToString());
                        DepartmentExisting = DataCategory;
                        IBTValueExisting = IbtStatus;
                        MatchingStatusExisting = MatchingStatus;

                    }

                    lbl_mainID.Text = MainID.ToString();
                    lbl_AccountNo.Text = AccountNo;
                    lbl_Serial.Text = Serial;
                    lbl_CashAccount.Text = CashAccount;
                    lbl_PolicyNo.Text = PolicyNo;
                    lbl_TransactionDate.Text = TransDate;
                    lbl_BankRef.Text = BankRef;
                    lbl_BranchCode.Text = BankCode;
                    lbl_Chequeno.Text = ChqNo;
                    lbl_Debit.Text = Debit;
                    lbl_Credit.Text = Credit;
                    lbl_RunningTot.Text = RunningTot;
                    lbl_OpenningBal.Text = OpenningBal;
                    lbl_ClossingBal.Text = ClossingBal;
                    txt_Description.Text = Description;
                    txt_PolicyNo.Text = PolicyNo;
                    lbl_Created_Date.Text = CreatedDate;
                    lbl_CreatedBy.Text = CreatedBy;
                    lbl_Ef_End_Date.Text = EffectiveEndDate;

                    lblSerial_Count.Text = Convert.ToString(SerialCount.ToString());

                    txt_usrComment.Text = UserComment;

                    //--------------------------Set Opening Balance-----------------------------//

                    OB_Calculated = calculate_openningBalance(RunningTot, Debit, Credit);
                    lbl_CalCulatedOB.Text = OB_Calculated.ToString();

                    //-------------------------------------------------------------------------//
                    get_popupDropdownlistValues(Product, MatchingStatus, DataCategory, IbtStatus);
                    getReceiptingDetails(ReceiptStatus, ReceiptNo, ReceiptNarration, ReceiptMatchingMethod, BulkReceiptInd);

                    if ((EffectiveEndDate != null) && (EffectiveEndDate != ""))
                    {
                        isHistoryRecord = true;
                    }

                    if (ReceiptMatchingMethod == "1")
                    {
                        isManuallyReceipted = true;
                    }
                    if (MatchingStatusExisting == "3")
                    {
                        isConfirmedRecord = true;
                    }
                    page_controls_authentication(isHistoryRecord, isManuallyReceipted, isConfirmedRecord);

                    //----------------New----------------//

                    if (BulkReceiptInd == "1")
                    {
                        btn_bulkDetails.Enabled = true;
                    }
                    else
                    {
                        btn_bulkDetails.Enabled = false;
                    }
                    mpe.Show();

                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Row Command Error...!" + ex.ToString();
                //throw;
            }
            finally
            {

                if (myConnectionUse != null)
                {
                    myConnectionUse.Close();

                }
            }
        }

        protected void grdSearchData_PageIndexChanging(object sender, GridViewPageEventArgs e)//------------------Grid View Paging-----------------------------------------//
        {
            grdSearchData.PageIndex = e.NewPageIndex;
            DataTable dt = (DataTable)Session["GrdData"];

            DataView dv = new DataView(dt);
            DataTable dt_new = dv.ToTable(true, "id", "account_no", "serial_no", "transaction_date", "policy_no", "balance", "matching_status", "effective_end_date", "DEPARTMENT", "bulk_receipt_ind", "bank_ref", "description", "createdby", "SerailCount");
            grdSearchData.DataSource = dt_new;
            grdSearchData.DataBind();
            this.SetFocus(cmdViewRule);
        }

        protected void grdSearchData_RowDataBound(object sender, GridViewRowEventArgs e)//------------------------Grid View Data Row Bound---------------------------------//
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Hidden Columns

                e.Row.Cells[2].CssClass = "HiddenCol";
                e.Row.Cells[9].CssClass = "HiddenCol";
                e.Row.Cells[7].CssClass = "HiddenCol";
                e.Row.Cells[11].CssClass = "HiddenCol";

                //Set Headers

                e.Row.Cells[3].Text = "Account No";
                e.Row.Cells[4].Text = "Serial No";
                e.Row.Cells[5].Text = "Trans Date";
                e.Row.Cells[6].Text = "Policy No";
                e.Row.Cells[8].Text = "Status";
                e.Row.Cells[10].Text = "Category";
                e.Row.Cells[12].Text = "DC No";
                e.Row.Cells[13].Text = "Description";
                e.Row.Cells[14].Text = "Created By";

                //Set Width Of Each Column

                e.Row.Cells[0].Attributes["width"] = "50px";//Select Button
                e.Row.Cells[1].Attributes["width"] = "80px";//Bulk Receipt Button
                e.Row.Cells[2].Attributes["width"] = "80px";//ID
                e.Row.Cells[3].Attributes["width"] = "110px";//Account No
                e.Row.Cells[4].Attributes["width"] = "110px";//Serial
                e.Row.Cells[5].Attributes["width"] = "80px";//Transaction Date
                e.Row.Cells[6].Attributes["width"] = "290px";//Policy No
                e.Row.Cells[7].Attributes["width"] = "150px";//Balance
                e.Row.Cells[8].Attributes["width"] = "135px";//State
                e.Row.Cells[9].Attributes["width"] = "150px";//Ef Date
                e.Row.Cells[10].Attributes["width"] = "150px";//Data Category
                e.Row.Cells[11].Attributes["width"] = "10px";//Bulk Receipt Ind
                e.Row.Cells[12].Attributes["width"] = "150px";//Bank Ref
                e.Row.Cells[13].Attributes["width"] = "350px";//Description
                e.Row.Cells[14].Attributes["width"] = "150px";//Created By
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //--------------Items Align---------------//

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[14].HorizontalAlign = HorizontalAlign.Left;


                //----------------------------------------//

                //validateCommon_controls();********************OLD

                //**********************************************NEW
                string EndEfDate = e.Row.Cells[9].Text;

                bool x = false;

                if (EndEfDate != "&nbsp;")
                {
                    x = true;
                }

                validateCommon_controls(x);

                //************************************************


                e.Row.Cells[2].CssClass = "HiddenCol";
                e.Row.Cells[9].CssClass = "HiddenCol";
                e.Row.Cells[7].CssClass = "HiddenCol";
                e.Row.Cells[11].CssClass = "HiddenCol";

                //e.Row.Cells[4].Attributes["width"] = "700px";


                foreach (TableCell cell in e.Row.Cells)//Default Background Color For Grid
                {
                    cell.BackColor = Color.White;
                }


                string MatchingStatusSearchGrd = e.Row.Cells[8].Text;
                string bulkReceiptIndicator = e.Row.Cells[11].Text;
                string dataCategory = e.Row.Cells[10].Text;

                LinkButton btnBulkReceipting = (LinkButton)e.Row.Cells[1].Controls[0];

                //----------------------Disable Bulk Receipting Button------------------//

                if (EndEfDate != "&nbsp;")//For History Record
                {
                    btnBulkReceipting.Enabled = false;

                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.BackColor = Color.PapayaWhip;
                    }
                }

                if (enalbleBulkReceiptBtn == false)//Chk
                {
                    btnBulkReceipting.Enabled = false;
                }

                if (dataCategory == "1")
                {
                    btnBulkReceipting.Enabled = false;
                }



                if (MatchingStatusSearchGrd == "0")//For Unmatched Record 
                {
                    MatchingStatusSearchGrd = "Unmatched";
                    e.Row.Cells[8].Text = MatchingStatusSearchGrd;
                    e.Row.Cells[8].BackColor = Color.LightSalmon;
                }

                if (MatchingStatusSearchGrd == "1")//For Possible Record
                {
                    MatchingStatusSearchGrd = "Possible";
                    e.Row.Cells[8].Text = MatchingStatusSearchGrd;//LavenderBlush
                    e.Row.Cells[8].BackColor = Color.Azure;
                }

                if (MatchingStatusSearchGrd == "2")//For Exactly Matched Record
                {
                    MatchingStatusSearchGrd = "Exact";
                    e.Row.Cells[8].Text = MatchingStatusSearchGrd;
                    btnBulkReceipting.Enabled = false;

                    e.Row.Cells[8].BackColor = Color.LemonChiffon;
                    //foreach (TableCell cell in e.Row.Cells)
                    //{
                    //    cell.BackColor = Color.LemonChiffon;
                    //}
                }

                if (MatchingStatusSearchGrd == "3")//For Confirmed Record
                {
                    MatchingStatusSearchGrd = "Confirmed";
                    e.Row.Cells[8].Text = MatchingStatusSearchGrd;
                    btnBulkReceipting.Enabled = false;

                    e.Row.Cells[8].BackColor = Color.Gold;
                }

                if (MatchingStatusSearchGrd == "4")//Manually Matched
                {
                    MatchingStatusSearchGrd = "ManuallyMatched";
                    e.Row.Cells[8].Text = MatchingStatusSearchGrd;
                    btnBulkReceipting.Enabled = false;
                    e.Row.Cells[8].BackColor = Color.MistyRose;
                }

                if (MatchingStatusSearchGrd == "5")
                {
                    MatchingStatusSearchGrd = "Pending Confirmation";
                    e.Row.Cells[8].Text = MatchingStatusSearchGrd;
                    btnBulkReceipting.Enabled = false;
                    e.Row.Cells[8].BackColor = Color.AntiqueWhite;
                }

                if ((MatchingStatusSearchGrd == "Pending Confirmation") && (bulkReceiptIndicator == "1"))//Pending Confirmation Bulk
                {
                    MatchingStatusSearchGrd = "Pending Confirmation";//"Pending Confirmation";
                    e.Row.Cells[8].Text = MatchingStatusSearchGrd;
                    btnBulkReceipting.Enabled = true;
                    e.Row.Cells[8].BackColor = Color.Thistle;
                }

                if ((MatchingStatusSearchGrd == "Pending Confirmation") && (bulkReceiptIndicator == "0"))//Pending Confirmation Regular
                {
                    MatchingStatusSearchGrd = "Pending Confirmation";
                    e.Row.Cells[8].Text = MatchingStatusSearchGrd;
                    btnBulkReceipting.Enabled = false;
                    e.Row.Cells[8].BackColor = Color.Thistle;
                }

                if ((MatchingStatusSearchGrd == "Pending Confirmation") && (bulkReceiptIndicator == "1") && (EndEfDate != "&nbsp;"))
                {
                    btnBulkReceipting.Enabled = false;
                }

                //---------------------------------------------------------------------------------------// 

                //-------------------------Swap Data Category Value To Data Category Description---------//

                string data_cat_value = e.Row.Cells[10].Text;
                string data_cat_description = "";

                myConnectionUse.Close();
                myConnectionUse.Open();

                OracleCommand cmd_getDataCatDescription = myConnectionUse.CreateCommand();
                cmd_getDataCatDescription.CommandText = "select d.department_description, d.value from fas_ibt_departments d where d.value ='" + data_cat_value + "'";
                OracleDataReader odr_getDataCatDescription = cmd_getDataCatDescription.ExecuteReader();

                while (odr_getDataCatDescription.Read())
                {
                    data_cat_description = odr_getDataCatDescription["department_description"].ToString();
                }

                e.Row.Cells[10].Text = data_cat_description;

                odr_getDataCatDescription.Close();
                myConnectionUse.Close();

                //---------------------------------------------------------------------------------------//

            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //------------------------------------------------------------------------Popup Data Bindings And Update Validations-----------------------------------------------//

        public void get_popupDropdownlistValues(string ProductValue, string MatchingStatusVal, string DataCatVal, string IBTValue)//--Dropdownlist Values Inside Popup-----//
        {
            //OracleConnection conn_getddlData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn_getddlData.Open();

            myConnectionUse.Close();
            myConnectionUse.Open();

            try
            {
                //-------------------------------------------------PRODUCTS-----------------------------------------------//

                OracleCommand cmd_getddlProductData = myConnectionUse.CreateCommand();//conn_getddlData.CreateCommand();
                cmd_getddlProductData.CommandText = "select p.product_description, p.value from fas_ibt_products p";

                OracleDataAdapter oda_ddlProducts = new OracleDataAdapter(cmd_getddlProductData);

                DataTable dt_ddlProducts = new DataTable();

                oda_ddlProducts.Fill(dt_ddlProducts);

                ddl_Produt.DataTextField = "product_description";
                ddl_Produt.DataValueField = "value";

                ddl_Produt.DataSource = dt_ddlProducts;
                ddl_Produt.DataBind();

                ddl_Produt.ClearSelection();
                ddl_Produt.Items.FindByValue(ProductValue).Selected = true;

                //--------------------------------------------------------------------------------------------------------//

                //-------------------------------------------------MATCHING STATUS----------------------------------------//

                OracleCommand cmd_getddlMatchingStatus = myConnectionUse.CreateCommand();//conn_getddlData.CreateCommand();
                cmd_getddlMatchingStatus.CommandText = "select m.matching_description, m.value from fas_ibt_matchingstatus m";

                OracleDataAdapter oda_ddlMatchingStatus = new OracleDataAdapter(cmd_getddlMatchingStatus);

                DataTable dt_ddlMatchingStatus = new DataTable();

                oda_ddlMatchingStatus.Fill(dt_ddlMatchingStatus);

                ddl_MatchingStatus.DataTextField = "matching_description";
                ddl_MatchingStatus.DataValueField = "value";

                ddl_MatchingStatus.DataSource = dt_ddlMatchingStatus;
                ddl_MatchingStatus.DataBind();

                ddl_MatchingStatus.ClearSelection();
                ddl_MatchingStatus.Items.FindByValue(MatchingStatusVal).Selected = true;

                //--------------------------------------------------------------------------------------------------------//

                //-------------------------------------------------DATA CATEGORY------------------------------------------//

                OracleCommand cmd_getddlDataCategory = myConnectionUse.CreateCommand();//conn_getddlData.CreateCommand();
                cmd_getddlDataCategory.CommandText = "select d.department_description, d.value, d.inactive from fas_ibt_departments d";

                OracleDataAdapter oda_ddlDataCategory = new OracleDataAdapter(cmd_getddlDataCategory);

                DataTable dt_ddlDataCategory = new DataTable();

                oda_ddlDataCategory.Fill(dt_ddlDataCategory);

                ddl_DataCategory.DataTextField = "department_description";
                ddl_DataCategory.DataValueField = "value";

                ddl_DataCategory.DataSource = dt_ddlDataCategory;
                ddl_DataCategory.DataBind();

                ddl_DataCategory.ClearSelection();
                ddl_DataCategory.Items.FindByValue(DataCatVal).Selected = true;

                //--------------------------------------------------------------------------------------------------------//

                //-------------------------------------------------IBT / NON IBT------------------------------------------//
                ddl_IBTStatus.Items.Clear();
                ddl_IBTStatus.Items.Clear();
                ddl_IBTStatus.Items.Add(new ListItem("Non IBT", "0"));
                ddl_IBTStatus.Items.Add(new ListItem("IBT", "1"));

                ddl_IBTStatus.DataBind();

                ddl_IBTStatus.ClearSelection();
                ddl_IBTStatus.Items.FindByValue(IBTValue).Selected = true; ;

                //--------------------------------------------------------------------------------------------------------//

                oda_ddlDataCategory.Dispose();
                oda_ddlMatchingStatus.Dispose();
                oda_ddlProducts.Dispose();
                myConnectionUse.Close();//conn_getddlData.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Dropdown Lists Data Binding Fail...!" + ex.ToString();
                //throw;
            }
            finally
            {
                //if (conn_getddlData != null)
                //{
                //    conn_getddlData.Dispose();
                //}
                if (myConnectionUse != null)
                {
                    myConnectionUse.Close();
                    //myConnectionUse.Dispose();
                }
            }
        }

        public void getReceiptingDetails(string ReceiptStatus, string ReceiptNo, string ReceiptNarration, string ReceiptMatchingMethod, string BulkReceiptInd)//--Receipting Details For Popup----//
        {
            //OracleConnection conn_getddlData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn_getddlData.Open();

            //myConnectionUse.Close();
            myConnectionUse.Open();

            try
            {
                txt_Rec_No.Text = ReceiptNo;
                txt_Rec_Narration.Text = ReceiptNarration;

                //-------------------------------------------------BULK----------------------------------------------------------//

                ddl_bulkReceiptStatus.Items.Clear();
                ddl_bulkReceiptStatus.Items.Add(new ListItem("No", "0"));
                ddl_bulkReceiptStatus.Items.Add(new ListItem("Yes", "1"));

                ddl_bulkReceiptStatus.DataBind();

                ddl_bulkReceiptStatus.ClearSelection();
                ddl_bulkReceiptStatus.Items.FindByValue(BulkReceiptInd).Selected = true;

                //-------------------------------------------------RECEITED METHOD (Manual)--------------------------------------//

                ddl_ReceiptedMethod.Items.Clear();
                ddl_ReceiptedMethod.Items.Clear();
                ddl_ReceiptedMethod.Items.Add(new ListItem("No", "0"));
                ddl_ReceiptedMethod.Items.Add(new ListItem("Yes", "1"));

                ddl_ReceiptedMethod.DataBind();

                ddl_ReceiptedMethod.ClearSelection();
                ddl_ReceiptedMethod.Items.FindByValue(ReceiptMatchingMethod).Selected = true;

                //--------------------------------------------------------------------------------------------------------------//


                //-------------------------------------------------SUPPRESS METHOD (Manual)--------------------------------------//

                ddl_AddHistory.Items.Clear();
                ddl_AddHistory.Items.Clear();
                ddl_AddHistory.Items.Add(new ListItem("No", "0"));
                ddl_AddHistory.Items.Add(new ListItem("Yes", "1"));

                ddl_AddHistory.DataBind();

                ddl_AddHistory.ClearSelection();
                ddl_AddHistory.Items.FindByValue(ReceiptMatchingMethod).Selected = true;

                //--------------------------------------------------------------------------------------------------------------//


                //-------------------------------------------------RECEIPTED STATUS---------------------------------------------//

                string receipt_status_for_print = "";

                OracleCommand cmd_getReceiptStatus = myConnectionUse.CreateCommand();//conn_getddlData.CreateCommand();
                cmd_getReceiptStatus.CommandText = "select r.receiptstatus_description, r.value from fas_ibt_receipt_status r where r.value = '" + ReceiptStatus + "'";

                OracleDataReader odr_getReceiptStatus = cmd_getReceiptStatus.ExecuteReader();

                while (odr_getReceiptStatus.Read())
                {
                    receipt_status_for_print = odr_getReceiptStatus[0].ToString();
                }
                lbl_ReceiptStatusVal.Text = ReceiptStatus.ToString();//New
                lbl_Rec_Status.Text = receipt_status_for_print;

                odr_getReceiptStatus.Close();
                myConnectionUse.Close();//conn_getddlData.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Get Receipting Details Method Fail...!" + ex.ToString();
                //throw;
            }

            finally
            {
                //if (conn_getddlData != null)
                //{
                //    conn_getddlData.Dispose();
                //}
                if (myConnectionUse != null)
                {
                    myConnectionUse.Close();
                    //myConnectionUse.Dispose();
                }
            }

            //--------------------------------------------------------------------------------------------------------------//

        }

        public void validateCommon_controls(bool isHistoryRecord)//-------------------------------------------User Controls (Inside Popup And Grid) Enable / Disable-------//
        {
            try
            {
                if (isHistoryRecord == true)
                {
                    txt_Description.Enabled = false;
                    txt_PolicyNo.Enabled = false;
                    ddl_IBTStatus.Enabled = false;
                    ddl_MatchingStatus.Enabled = false;
                    ddl_Produt.Enabled = false;
                    ddl_DataCategory.Enabled = false;
                    ddl_ReceiptedMethod.Enabled = false;
                    txt_Rec_No.Enabled = false;
                    txt_Rec_Narration.Enabled = false;
                    enalbleBulkReceiptBtn = false;
                }
                else
                {
                    DataTable dt_controls = new DataTable();

                    dt_controls = (DataTable)Session["PagePermissions"];

                    txt_Description.Enabled = false;
                    txt_PolicyNo.Enabled = false;
                    ddl_IBTStatus.Enabled = false;
                    ddl_MatchingStatus.Enabled = false;
                    ddl_Produt.Enabled = false;
                    ddl_DataCategory.Enabled = false;
                    ddl_ReceiptedMethod.Enabled = false;
                    txt_Rec_No.Enabled = false;
                    txt_Rec_Narration.Enabled = false;
                    enalbleBulkReceiptBtn = false;


                    foreach (DataRow usr_row in dt_controls.Rows)
                    {
                        controller_permission = usr_row["field_name"].ToString();
                        // usr_ibt_status = usr_row["IBT_STATUS"].ToString();

                        if (controller_permission == "DESCRIPTION")
                        {
                            txt_Description.Enabled = true;
                            txt_Description.ReadOnly = true;
                            //txt_Description.Attributes.Add("readonly", "readonly");
                        }
                        if (controller_permission == "POLICY_NO")
                        {
                            txt_PolicyNo.Enabled = true;
                        }
                        if (controller_permission == "IBT_STATUS")
                        {
                            ddl_IBTStatus.Enabled = true;
                        }
                        if (controller_permission == "MATCHING_STATUS")
                        {
                            ddl_MatchingStatus.Enabled = true;
                        }
                        if (controller_permission == "PRODUCT")
                        {
                            ddl_Produt.Enabled = true;
                        }
                        if (controller_permission == "DEPARTMENT")//data category
                        {
                            ddl_DataCategory.Enabled = true;
                        }
                        if (controller_permission == "RECEIPTED_METHOD")
                        {
                            ddl_ReceiptedMethod.Enabled = true;
                        }
                        if (controller_permission == "RECEIPT_N0")
                        {
                            txt_Rec_No.Enabled = true;
                        }
                        if (controller_permission == "RECEIPT_NARRATION")
                        {
                            txt_Rec_Narration.Enabled = true;
                        }
                        if (controller_permission == "BULK_RECEIPTING")
                        {
                            enalbleBulkReceiptBtn = true;
                        }
                        controller_permission = "";
                    }
                }

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Validation Common Controls Fail...!" + ex.ToString();
                //throw;
            }
        }

        public bool chk_dataCategoryValue_forUpdate(int update_upload_dtl_id, int update_Data_Category)//-----Check Data Category Status (Active / Inactive)---------------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();            
            myConnectionUse.Close();
            myConnectionUse.Open();

            try
            {
                int dataCatValue_existing = -1;
                int dataCatValue_Updated = -1;
                int status_of_updatedDataCatValue = -1;
                bool valid_to_update = false;

                OracleCommand cmd_get_dataCatValue_existing = myConnectionUse.CreateCommand();//conn.CreateCommand();
                cmd_get_dataCatValue_existing.CommandText = "select t.department from fas_ibt_uploaded_dtl t where t.id =" + update_upload_dtl_id;

                OracleDataReader odr_get_dataCatValue_existing = cmd_get_dataCatValue_existing.ExecuteReader();

                while (odr_get_dataCatValue_existing.Read())
                {
                    dataCatValue_existing = int.Parse(odr_get_dataCatValue_existing[0].ToString());
                }

                dataCatValue_Updated = update_Data_Category;

                if (dataCatValue_existing == dataCatValue_Updated)
                {
                    valid_to_update = true;
                }
                else
                {
                    OracleCommand cmd_status_of_updatedDataCatValue = myConnectionUse.CreateCommand();//conn.CreateCommand();
                    cmd_status_of_updatedDataCatValue.CommandText = "select d.inactive from fas_ibt_departments d where d.value =" + dataCatValue_Updated;

                    OracleDataReader odr_status_of_updatedDataCatValue = cmd_status_of_updatedDataCatValue.ExecuteReader();

                    while (odr_status_of_updatedDataCatValue.Read())
                    {
                        status_of_updatedDataCatValue = int.Parse(odr_status_of_updatedDataCatValue[0].ToString());
                    }

                    if (status_of_updatedDataCatValue == 0)
                    {
                        valid_to_update = true;
                    }
                    else
                    {
                        valid_to_update = false;
                    }
                }

                return valid_to_update;

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Data Category (Department) Validation Fail...!" + ex.ToString();
                return false;
            }
            finally
            {
                //if(conn!=null)
                //{
                //    conn.Dispose();
                //}
                if (myConnectionUse != null)
                {
                    myConnectionUse.Close();
                    //myConnectionUse.Dispose();
                }
            }


        }

        public bool chk_manuallyMatched_forUpdate(int update_upload_dtl_id, int manuallyMatched_ind, string update_Receipt_No, string update_Receipt_Narration)//----------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();
            myConnectionUse.Close();
            myConnectionUse.Open();

            try
            {
                bool x = false;
                int manuallyMatched_existing = -1;
                int manuallyMatched_updated = -1;
                string receiptNo_existing = null;
                string receiptNo_updated = null;
                string receipt_narration_existing = null;
                string receipt_narration_updated = null;


                OracleCommand cmd_ManuallyMatched_existing = myConnectionUse.CreateCommand();//conn.CreateCommand();

                cmd_ManuallyMatched_existing.CommandText = "select t.manually_matched_ind, t.Receipt_No, t.receipt_narration from fas_ibt_uploaded_dtl t where t.id = '" + update_upload_dtl_id + "'";

                OracleDataReader odr_manuallyMatched_existing = cmd_ManuallyMatched_existing.ExecuteReader();

                while (odr_manuallyMatched_existing.Read())
                {
                    manuallyMatched_existing = int.Parse(odr_manuallyMatched_existing[0].ToString());
                    receiptNo_existing = odr_manuallyMatched_existing[1].ToString();
                    receipt_narration_existing = odr_manuallyMatched_existing[2].ToString();
                }

                manuallyMatched_updated = manuallyMatched_ind;
                receiptNo_updated = update_Receipt_No;
                receipt_narration_updated = update_Receipt_Narration;

                if (manuallyMatched_existing == manuallyMatched_updated)
                {
                    if ((receiptNo_updated == receiptNo_existing) && (receipt_narration_updated == receipt_narration_existing))
                    {
                        //valid to update
                        x = true;
                        manuallyReceipted_validation = 0;//receipt status not changed
                    }
                    else
                    {
                        x = false;
                        manuallyReceipted_validation = 0;//receipt status not changed
                    }
                }
                else
                {
                    if ((receiptNo_updated != receiptNo_existing) && (receipt_narration_updated != receipt_narration_existing) && ((receiptNo_updated != "") || (receiptNo_updated != null)) && ((receipt_narration_updated != "") || (receipt_narration_updated != null)))
                    {
                        //valid to update
                        x = true;
                        manuallyReceipted_validation = 1;//receipt status changed
                    }
                    else
                    {
                        x = false;
                        manuallyReceipted_validation = 0;//receipt status not changed
                    }
                }

                return x;
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Manually Matched Validation Fail...!" + ex.ToString();
                return false;
                //throw;
            }
            finally
            {
                //if(conn!=null)
                //{
                //    conn.Dispose();
                //}
                if (myConnectionUse != null)
                {
                    myConnectionUse.Close();
                    //myConnectionUse.Dispose();
                }
            }

        }

        public bool chk_bulkReceipt_forUpdate(int update_upload_dtl_id, int bulk_receipt_ind)//---------------Bulk Receipt Validation For Record Update--------------------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();

            myConnectionUse.Close();
            myConnectionUse.Open();

            try
            {
                int bulkIndicator = -1;
                bool validToUpdate = false;

                OracleCommand cmd_get_bulkReceiptInd = myConnectionUse.CreateCommand();//conn.CreateCommand();
                cmd_get_bulkReceiptInd.CommandText = "select t.bulk_receipt_ind, t.receipt_status from fas_ibt_uploaded_dtl t where t.id =" + update_upload_dtl_id;

                OracleDataReader odr_get_bulkReceiptInd = cmd_get_bulkReceiptInd.ExecuteReader();

                while (odr_get_bulkReceiptInd.Read())
                {
                    bulkIndicator = int.Parse(odr_get_bulkReceiptInd["bulk_receipt_ind"].ToString());
                }

                if (bulkIndicator == 0)
                {
                    validToUpdate = true;
                }
                else
                {
                    validToUpdate = false;
                }

                myConnectionUse.Close();//conn.Close();
                odr_get_bulkReceiptInd.Close();

                return validToUpdate;
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Bulk Receipt Indicator Validation Fail...!" + ex.ToString();
                return false;
            }
            finally
            {
                //if(conn!=null)
                //{
                //    conn.Close();
                //}
                if (myConnectionUse != null)
                {
                    myConnectionUse.Close();
                    //myConnectionUse.Dispose();
                }
            }

        }

        public bool chk_regularReceipt_forUpdate(int update_upload_dtl_id, int bulk_receipt_ind)//------------Regular Receipt Validation For Record Update-----------------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();

            myConnectionUse.Close();
            myConnectionUse.Open();

            try
            {
                int current_matchingStatus = -1;
                bool validToUpdate = false;

                OracleCommand cmd_get_matchingStatus = myConnectionUse.CreateCommand();//conn.CreateCommand();
                cmd_get_matchingStatus.CommandText = "select t.bulk_receipt_ind, t.matching_status from fas_ibt_uploaded_dtl t where t.id =" + update_upload_dtl_id;

                OracleDataReader odr_get_matchingStatus = cmd_get_matchingStatus.ExecuteReader();

                while (odr_get_matchingStatus.Read())
                {
                    current_matchingStatus = int.Parse(odr_get_matchingStatus["matching_status"].ToString());
                }

                if ((current_matchingStatus == 5) && (bulk_receipt_ind == 0))//Pending Confirmation Record 
                {
                    validToUpdate = false;
                }
                else
                {
                    validToUpdate = true;
                }

                return validToUpdate;
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "Current User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = "Regular Receipt Matching Status Validation Fail...!" + ex.ToString();
                return false;
            }
            finally
            {
                //if (conn != null)
                //{
                //    conn.Close();
                //}
                if (myConnectionUse != null)
                {
                    myConnectionUse.Close();
                    //myConnectionUse.Dispose();
                }
            }
        }

        public double calculate_openningBalance(string RunningTotal, string Debit, string Credit)//-----------Calculate Opening Balance For A Transaction------------------//
        {
            double OpenningBal = double.Parse(RunningTotal);

            if (Debit != "0")
            {
                double debit = double.Parse(Debit);
                OpenningBal = OpenningBal + debit;
            }
            if (Credit != "0")
            {
                double credit = double.Parse(Credit);
                OpenningBal = OpenningBal - credit;
            }

            return OpenningBal;
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

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //------------------------------------------------------------------------Search Result Summary And Export To Excel File-------------------------------------------//

        public void getSummary()//------------------------------------------------------------------------Get Searched Result Summary--------------------------------------//
        {
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();
            myConnectionUse.Close();//******new addition
            myConnectionUse.Open();
            OracleCommand cmd = myConnectionUse.CreateCommand();//conn.CreateCommand();

            /*SELECT  COUNT(case when t.ibt_status=1 then t.id ELSE null END) IBT ,
        COUNT(case when t.ibt_status=0 then t.id ELSE null END) NONIBT, 
        count(*)*/

            cmd.CommandText = "SELECT " +
                              "COUNT (*) All_Data," +
                              "COUNT (case when t.ibt_status=1 then t.id ELSE null END) IBT," +
                              "COUNT (case when t.ibt_status=0 then t.id ELSE null END) NONIBT," +
                              "COUNT (case when t.matching_status = 0 then t.id ELSE null END) Unmatched," +
                              "COUNT (case when t.matching_status = 1 then t.id ELSE null END) Possible," +
                              "COUNT (case when t.matching_status = 2 then t.id ELSE null END) Exact," +
                              "COUNT (case when t.matching_status = 3 then t.id ELSE null END) Confirmed," +
                              "COUNT (case when t.product = 1 then t.id ELSE null END) Conventional_General," +
                              "COUNT (case when t.product = 2 then t.id ELSE null END) Conventional_Life," +
                              "COUNT (case when t.product = 3 then t.id ELSE null END) Takaful_General," +
                              "COUNT (case when t.product = 4 then t.id ELSE null END) Takaful_Life, " +
                              "COUNT (case when t.department = 1 then t.id ELSE null END) OTHER, " +
                              "COUNT (case when t.department = 2 then t.id ELSE null END) MRP, " +
                              "COUNT (case when t.department = 3 then t.id ELSE null END) MCR, " +
                              "COUNT (case when t.department = 4 then t.id ELSE null END) MOTOR, " +
                              "COUNT (case when t.department = 5 then t.id ELSE null END) NON_MOTOR, " +
                              "COUNT (case when t.department = 6 then t.id ELSE null END) NEW_BUSINESS, " +
                            "COUNT (case when t.bulk_receipt_ind = 1 then t.id ELSE null END)BULK_AVAILABLE " +
                              "FROM (" + qry + ") t ";

            OracleDataReader odr = cmd.ExecuteReader();

            if (odr.Read())
            {
                if (odr.HasRows)
                {
                    lblSummaryTotal.Text = "Total Records: " + odr[0].ToString();
                    lblSummaryIBT.Text = "IBT: " + odr[1].ToString();
                    lblSummaryNonIBT.Text = "Non IBT: " + odr[2].ToString();
                    lblSummaryUnmatched.Text = "Unmatched: " + odr[3].ToString();
                    lblSummaryPossible.Text = "Possible: " + odr[4].ToString();
                    lblSummaryExact.Text = "Exact: " + odr[5].ToString();
                    lblSummaryConfirmed.Text = "Confirmed: " + odr[6].ToString();
                    lblSummaryC_General.Text = "C.General: " + odr[7].ToString();
                    lblSummaryC_Life.Text = "C.Life: " + odr[8].ToString();
                    lblSummaryT_General.Text = "T.General: " + odr[9].ToString();
                    lblSummaryT_Life.Text = "T.Life: " + odr[10].ToString();
                    lblSummary_OTHER.Text = "Other: " + odr[11].ToString();
                    lblSummary_MRP.Text = "MRP: " + odr[12].ToString();
                    lblSummary_MCR.Text = "MCR: " + odr[13].ToString();
                    lblSummary_MOTOR.Text = "Motor: " + odr[14].ToString();
                    lblSummary_NON_MOTOR.Text = "Non Motor: " + odr[15].ToString();
                    lblSummary_NEW_BUSINESS.Text = "New Business: " + odr[16].ToString();
                    lblSummary_BULK.Text = "Bulk Records: " + odr[17].ToString();

                    summaryDiv.Visible = true;
                }
                else
                {
                    summaryDiv.Visible = false;
                }

            }

            odr.Close();
            myConnectionUse.Close();//conn.Close();
        }

        private void dataToReport()//---------------------------------------------------------------------Data To Excel Export SSRS Report---------------------------------//
        {
            try
            {
                string whereToExcel = "";
                string searchedWhere = qry.Split(new string[] { "WHERE" }, StringSplitOptions.None).Last();//original
                whereToExcel = "where " + searchedWhere;


                myConnectionUse.Open();

                OracleCommand cmd = myConnectionUse.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_SEARCH_RESULTS";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("vSQL_WHERE", OracleType.VarChar).Value = whereToExcel;
                cmd.Parameters.Add("vDataExportedBy", OracleType.VarChar).Value = usrName;

                cmd.ExecuteNonQuery();
                myConnectionUse.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (myConnectionUse.State != null)
                {
                    myConnectionUse.Close();
                }
            }

        }

        protected void ibtnExcelReport_Click(object sender, ImageClickEventArgs e)//----------------------Search Results Export Click Event--------------------------------//
        {
            if (grdSearchData.Rows.Count != 0)
            {
                ClearExportTable();
                dataToReport();
                Response.Redirect("~/IBT/IBT_Reports.aspx", false);
            }
            else
            {
                call_error_msg(false);
                lblResult.Text = "No Data Records Available For Generate Report...!";
            }
        }


        private void ClearExportTable()
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                OracleCommand cmdUploadData = new OracleCommand();
                cmdUploadData.Connection = conn;
                conn.Open();
                cmdUploadData.CommandText = "DELETE FROM FAS_IBT_EXCEL_EXPORT WHERE DATA_EXPORTED_BY = '" +usrName+"'";
                OracleDataReader myOleDbDataReaderUpload = cmdUploadData.ExecuteReader();
                conn.Close();
                myOleDbDataReaderUpload.Close();
            }
            catch (Exception)
            {
                
                throw;
            }

        }


        protected void btn_bulkDetails_Click(object sender, EventArgs e)//--------------------------------View Bulk Information Button Click Event-------------------------//
        {
            string serial_bulk = lbl_Serial.Text;//lbl_mainID.Text;
            get_bulkInfo(serial_bulk);
            mpe2.Show();
        }

        public void get_bulkInfo(string serial_bulk)//----------------------------------------------------Get Bulk Receipting Details For Popup----------------------------//
        {
            try
            {
                if (myConnectionUse.State != null)
                {
                    myConnectionUse.Close();
                    myConnectionUse.Open();

                    OracleCommand cmd_get_bulkInfo = myConnectionUse.CreateCommand();
                    cmd_get_bulkInfo.CommandText = "SELECT BD.ID, BD.DTL_ID,BD.SERIAL_NO,BD.CREATEDBY,BD.AMOUNT, BD.RECEIPT_NO,RS.RECEIPTSTATUS_DESCRIPTION,BD.CREATEDDATE, BD.EFFECTIVE_END_DATE " +
                                                    "FROM FAS_IBT_BULK_RECEIPT_DTL BD INNER JOIN FAS_IBT_RECEIPT_STATUS RS ON BD.RECEIPT_STATUS = RS.VALUE " +
                                                    "WHERE BD.SERIAL_NO ='" + serial_bulk + "' ORDER BY BD.ID";//TRUNC(TO_DATE(BD.CREATEDDATE,'DD/MM/RRRR'))AS CREATEDDATE  BD.EFFECTIVE_END_DATE IS NULL AND

                    OracleDataAdapter oda_bulkDetails = new OracleDataAdapter(cmd_get_bulkInfo);
                    DataTable dt_bulk = new DataTable();
                    oda_bulkDetails.Fill(dt_bulk);

                    grdBulkReceiptDetails.DataSource = dt_bulk;
                    grdBulkReceiptDetails.DataBind();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)//-------------------------Bulk Receipt Details Popup Close---------------------------------//
        {
            mpe.Show();

        }

        protected void grdBulkReceiptDetails_RowDataBound(object sender, GridViewRowEventArgs e)//--------Bulk Receipt Details Grid View Data Row Bound--------------------//
        {
            foreach (TableCell cell in e.Row.Cells)//Default Background Color For Grid
            {
                cell.BackColor = Color.White;
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                //BD.ID, BD.DTL_ID,BD.SERIAL_NO,BD.CREATEDBY,BD.AMOUNT, BD.RECEIPT_NO,RS.RECEIPTSTATUS_DESCRIPTION,BD.CREATEDDATE, BD.EFFECTIVE_END_DATE
                //Hidden Columns
                e.Row.Cells[0].CssClass = "HiddenCol";

                //Set Headers
                e.Row.Cells[1].Text = "Detail ID";
                e.Row.Cells[2].Text = "Serial";
                e.Row.Cells[3].Text = "Created By";
                e.Row.Cells[4].Text = "Amount";
                e.Row.Cells[5].Text = "Receipt No";
                e.Row.Cells[6].Text = "Receipt Narration";
                e.Row.Cells[7].Text = "Created Date";
                e.Row.Cells[8].Text = "Effective End Date";

                //Set Column Width
                e.Row.Cells[1].Attributes["width"] = "50px";
                e.Row.Cells[2].Attributes["width"] = "115px";
                e.Row.Cells[3].Attributes["width"] = "110px";
                e.Row.Cells[4].Attributes["width"] = "70px";
                e.Row.Cells[5].Attributes["width"] = "105px";
                e.Row.Cells[6].Attributes["width"] = "145px";
                e.Row.Cells[7].Attributes["width"] = "145px";
                e.Row.Cells[8].Attributes["width"] = "145px";

                foreach (TableCell cell in e.Row.Cells)//Default Background Color For Grid
                {
                    cell.BackColor = Color.Teal;
                }

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].CssClass = "HiddenCol";

                //--------------Items Align---------------//

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Left;



                string EndEfDate = e.Row.Cells[8].Text;
                string Current_Status = e.Row.Cells[6].Text;

                if (EndEfDate != "&nbsp;")
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.BackColor = Color.PapayaWhip;
                    }
                }

                if ((Current_Status == "Manually Receipted") || (Current_Status == "Successfully Receipted"))
                {
                    e.Row.Cells[6].BackColor = Color.Gold;
                }
            }


        }
    }
}

