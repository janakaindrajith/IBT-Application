using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Transactions;
using System.Collections;
using System.Web.Configuration;
using System.Threading;



namespace BranchMIS.IBT
{


    public partial class IBT_Bulk_Receipting : System.Web.UI.Page
    {
        DataTable dt = new DataTable();

        //----------------From Other Pages------------------
        public Int64 DtlID;
        public String DtlTransDate;
        public String DtlSerialNo;
        public String DtlPolicyNo;
        public Decimal DtlTotalAmount;
        //--------------------------------------------------

        private string receiptNo_update = "";//For Manual Receipting
        private string receiptNarration_update = "";//For Manual Receipting
        public Decimal TempTotal;
        public ArrayList arrRemoveListRV = new ArrayList();
        private string usrName = "";
        public int usr_role = 0;
        public DataTable dtPageControls = new DataTable();//For Page Controls Authentication
        public int TotalRecords_WhenPageLoad = 0;//For Lock Record Validation
        public int LiveRecord_Count = 0;//For Lock Record Validation
        int running_count = 0;//For Lock Record Validation


        protected void Page_Load(object sender, EventArgs e)//-----------------------------------------Page Load Events---------------------------------------------------//
        {
            if (Session["IBT_UserName"] == null)
            {
                string usrValid = "SessionExpired";
                Response.Redirect("~/FAS_Home.aspx?usr=" + usrValid, false);
                return;
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies["BulkDetailVariables"];

            if (cookie == null)
            {
                string usrValid = "SessionExpired";
                Response.Redirect("~/FAS_Home.aspx?usr=" + usrValid, false);
                return;
            }

            DtlID = Convert.ToInt64(Request.Cookies["BulkDetailVariables"]["DtlID"]);
            DtlTransDate = Request.Cookies["BulkDetailVariables"]["DtlTransDate"];
            DtlSerialNo = Request.Cookies["BulkDetailVariables"]["DtlSerialNo"];
            DtlPolicyNo = Request.Cookies["BulkDetailVariables"]["DtlPolicyNo"];
            DtlTotalAmount = Convert.ToDecimal(Request.Cookies["BulkDetailVariables"]["DtlTotalAmount"]);

            CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();

            usrName = Session["IBT_UserName"].ToString();

            string pageName = (System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath));

            bool x = clsCom.getPermissions(usrName, pageName);
            Session["PagePermissions"] = null;
            dtPageControls = clsCom.getUserRoleAndPageControls(usrName, pageName);
            Session["PagePermissions"] = dtPageControls;

            if (x == true)
            {
                if (!IsPostBack)
                {
                    //hf_selectedRowID.Value = null;

                    disableBatchButtons();
                    txtSerialNo.Text = DtlSerialNo;
                    txtTotal.Text = DtlTotalAmount.ToString("#,##0.00");
                    txtAvailableBal.Text = DtlTotalAmount.ToString("#,##0.00");

                    TempTotal = DtlTotalAmount;
                    LoadData(DtlID);


                    //----------------------------------ORIGINAL CODE (NEW)----------------------------//
                    if (running_count == 0)
                    {
                        get_TotalRecords_WhenPageLoad();
                        running_count = running_count + 1;
                    }
                    get_LiveRecords_Total();

                    //--------------------------------------------------------------------------------//

                    formControlsAuthentication();
                }
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)//--------------------------------------Add Click----------------------------------------------------------//
        {
            try
            {
                CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();

                if (page_result.Visible == true)
                {
                    page_result.Visible = false;
                }

                grdTransList.DataSource = Session["TransList"];
                grdTransList.DataBind();


                Decimal TotalAmount = 0;
                Decimal Amount = 0;

                if (txtRefNo.Text.Trim().Length == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Reference Number Can Not Be Empty...!";
                    return;
                }

                if (txtPerc.Text.Trim().Length == 0 && txtAmount.Text.Trim().Length == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Percentage and Amount both can not have Empty values at the same time...!";
                    return;
                }

                if (txtPerc.Text.Trim().Length > 0 && txtAmount.Text.Trim().Length > 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Percentage and Amount both can not have values at the same time...!";
                    return;
                }


                if (txtAmount.Text.Trim() != "")
                {
                    if (Convert.ToDecimal(txtAvailableBal.Text.Trim()) < Convert.ToDecimal(txtAmount.Text.Trim()))
                    {
                        call_error_msg(false);
                        lblResult.Text = "You can not exceed the available amount...!";
                        return;
                    }
                }


                DataTable dtTemp = new DataTable();
                dtTemp = (DataTable)Session["TransList"];

                //---------------Calculate Start

                TotalAmount = Convert.ToDecimal(txtAvailableBal.Text);

                if (txtPerc.Text.Trim().Length > 0)
                {
                    Amount = TotalAmount * Convert.ToDecimal(Convert.ToDecimal(txtPerc.Text) / 100);
                }

                if (txtAmount.Text.Trim().Length > 0)
                {
                    Amount = Convert.ToDecimal(txtAmount.Text);
                }


                if (Amount == 0)
                {
                    call_error_msg(false);
                    lblResult.Text = "Zero value not allowed...!";
                    return;
                }


                TotalAmount = TotalAmount - Amount;

                TempTotal = TempTotal - Amount;

                dtTemp.Rows.Add(-1, txtRefNo.Text, Amount.ToString("#,##0.00"), usrName, System.DateTime.Now, "New", "");

                txtAvailableBal.Text = TotalAmount.ToString("#,##0.00");

                Session["TransList"] = dtTemp;

                grdTransList.DataSource = Session["TransList"];
                grdTransList.DataBind();


                Double TempTot = 0;
                foreach (DataRow item in dtTemp.Rows)
                {
                    TempTot = TempTot + Convert.ToDouble(item["Amount"].ToString());
                }


                txtRefNo.Text = "";
                txtPerc.Text = "";
                txtAmount.Text = "";



            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void cmdConfirm_Click(object sender, EventArgs e)//----------------------------------Records Confirm Click Event----------------------------------------//
        {
            //double dr_total = 0;


            if (page_result.Visible == true)
            {
                page_result.Visible = false;
            }
            get_LiveRecords_Total();

            if (int.Parse(Session["TotalRecords_WhenPageLoad"].ToString()) == LiveRecord_Count)
            {


                //Check Totals-------------
                Double TempTot = 0;
                DataTable dtTemp = (DataTable)Session["TransList"];


                if (dtTemp == null)
                {
                    return;
                }

                foreach (DataRow item in dtTemp.Rows)
                {
                    TempTot = TempTot + Convert.ToDouble(item["Amount"].ToString());
                }



                //*****************************************-----New Fn-----**********************************//

                double real_total = double.Parse(txtTotal.Text);
                double calculation_differance = TempTot - real_total;
                

                if ((Math.Abs(calculation_differance) <= 0.1) || (calculation_differance == 0))
                {
                    if (calculation_differance > 0)
                    {
                        calculation_differance = (-1) * (calculation_differance);
                    }
                    else
                    {
                        calculation_differance = (1) * (calculation_differance);
                    }
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "Calculation Differance Is Greater than 0.1 ";
                    return;
                }

                //********************************************************************************************//

                CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
                DataTable Dt = (DataTable)Session["TransList"];

                Int16 SubNo = 1;


                CommonCLS.IBTBatches IBTBatch = new CommonCLS.IBTBatches();
                String BatchNo = IBTBatch.GetBatchNo("BATCH_NO");

                using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString()))
                {
                    conn.Open();
                    OracleCommand command = conn.CreateCommand();
                    OracleTransaction transaction;

                    transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    // Assign transaction object for a pending local transaction
                    command.Transaction = transaction;

                    try
                    {
                        Int16 Count = 1;
                        foreach (DataRow item in Dt.Rows)
                        {
                            if (item["Status"].ToString().Trim() != "New")
                            {
                                continue;
                            }

                            OracleCommand cmd = new OracleCommand();//conn.CreateCommand();
                            cmd.Connection = conn;

                            cmd.CommandText = "sp_fas_ibt_BulkReceipt";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("vID", OracleType.Int32).Value = DtlID;
                            cmd.Parameters.AddWithValue("vSubNo", OracleType.VarChar).Value = SubNo;
                            cmd.Parameters.AddWithValue("vRefNumber", OracleType.VarChar).Value = item["Ref_no"].ToString();
                            //cmd.Parameters.AddWithValue("vAmount", OracleType.Number).Value = double.Parse(item["Amount"].ToString());


                            cmd.Parameters.AddWithValue("vAmount", OracleType.Number).Value = double.Parse(item["Amount"].ToString()) + (calculation_differance);
                            calculation_differance = 0;





                            cmd.Parameters.AddWithValue("vCreatedBy", OracleType.Number).Value = usrName;
                            cmd.Parameters.AddWithValue("vCount", OracleType.Number).Value = Count;
                            cmd.Parameters.AddWithValue("vBatchNo", OracleType.Number).Value = BatchNo;

                            SubNo = Convert.ToInt16(SubNo + 1);

                            cmd.Transaction = transaction;

                            cmd.ExecuteNonQuery();

                            Count++;

                        }


                        arrRemoveListRV = (ArrayList)Session["arrRemoveListRV"];

                        if (arrRemoveListRV != null)
                        {
                            foreach (Object obj in arrRemoveListRV)
                            {
                                OracleCommand cmd = new OracleCommand();//conn.CreateCommand();
                                cmd.Connection = conn;
                                cmd.CommandText = "sp_fas_ibt_bulk_rv_Remove";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("BulkRV_ID", OracleType.Int32).Value = Convert.ToInt32(obj);

                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----Bulk Receipt Confirmed!---- By: " + usrName + " Base Record ID :" + DtlID, Server.MapPath("~/IBTLogFiles/Log.txt"));
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                        call_error_msg(false);
                        lblResult.Text = ex.InnerException.ToString();
                    }
                }


                ClearControls();
                running_count = 0;
                if (running_count == 0)
                {
                    get_TotalRecords_WhenPageLoad();
                    running_count = running_count + 1;//New
                }

                call_error_msg(true);
                lblResult.Text = "Records Confirmed For Receipt...!";
            }

            else
            {
                running_count = 0;

                call_error_msg(false);
                lblResult.Text = "Record Locked By Another User...! Please Refresh And Try Again!";
            }
        }

        private void ClearControls()//-----------------------------------------------------------------Clear Page Controls------------------------------------------------//
        {
            grdTransList.DataSource = null;
            grdTransList.DataBind();

            Session["TransList"] = null;

            txtAmount.Text = "";
            txtAvailableBal.Text = "";
            txtPerc.Text = "";
            txtRefNo.Text = "";
            txtTotal.Text = "";
            txtSerialNo.Text = "";
        }

        protected void grdTransList_RowDeleting(object sender, GridViewDeleteEventArgs e)//------------Grid View Row Deleting Event---------------------------------------//
        {
            try
            {
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void grdTransList_RowDataBound(object sender, GridViewRowEventArgs e)//--------------Grid View Row Data Bound-------------------------------------------//
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //e.Row.Cells[1].CssClass = "HiddenCol";
                }


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                    string Status = e.Row.Cells[7].Text;

                    if (Status == "Receipted")
                    {
                        LinkButton Remove = (LinkButton)e.Row.Cells[0].Controls[0];
                        Remove.Enabled = false;
                    }

                    if (Status == "New" || Status == "Pending FeedBack" || Status == "Successfully Receipted" || Status == "Manually Receipted")
                    {
                        LinkButton ManualReceipt = (LinkButton)e.Row.Cells[1].Controls[0];
                        ManualReceipt.Enabled = false;

                        if (Status == "Pending FeedBack")
                        {
                            LinkButton Remove = (LinkButton)e.Row.Cells[0].Controls[0];
                            Remove.Enabled = false;
                            //confirm_btn.Enabled = false;
                        }
                        if (Status == "Successfully Receipted")
                        {
                            LinkButton Remove = (LinkButton)e.Row.Cells[0].Controls[0];
                            Remove.Enabled = false;
                            //confirm_btn.Enabled = false;
                        }

                        if (Status == "Manually Receipted")
                        {
                            LinkButton Remove = (LinkButton)e.Row.Cells[0].Controls[0];
                            Remove.Enabled = false;
                            //confirm_btn.Enabled = false;
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        private void LoadData(Int64 SelectedID)//------------------------------------------------------Grid View Data Bind------------------------------------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd1 = new OracleCommand("SELECT SERIAL_NO FROM fas_ibt_uploaded_dtl WHERE ID = '" + SelectedID + "'", conn);

                OracleDataAdapter oda1 = new OracleDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                oda1.Fill(dt1);

                String SERIAL_NO = dt1.Rows[0]["SERIAL_NO"].ToString();

                OracleCommand cmd = new OracleCommand("select d.ID, d.ref_no, TO_CHAR(d.amount, '999,999,999,999.00') AS Amount, d.createdby, d.createddate, " +
                                                        "rs.receiptstatus_description as  Status, d.receipt_narration  from fas_ibt_bulk_receipt_dtl d " +
                                                        "inner join fas_ibt_receipt_status rs on d.receipt_status = rs.value  where d.serial_no  = '" + SERIAL_NO + "' AND d.effective_end_date is null", conn);

                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                grdTransList.DataSource = dt;
                grdTransList.DataBind();

                Session["TransList"] = dt;

                Double TempTot = 0;
                foreach (DataRow item in dt.Rows)
                {
                    TempTot = TempTot + Convert.ToDouble(item["Amount"].ToString());
                }

                txtAvailableBal.Text = Convert.ToDouble(Convert.ToDouble(txtTotal.Text) - TempTot).ToString("#,##0.00");

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }

        }

        protected void grdTransList_RowCommand(object sender, GridViewCommandEventArgs e)//------------Grid View Row Command----------------------------------------------// h
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    int BulkRV_ID = int.Parse(grdTransList.Rows[rowIndex].Cells[2].Text);

                    //Removed RV List

                    arrRemoveListRV.Add(BulkRV_ID);
                    Session["arrRemoveListRV"] = arrRemoveListRV;

                    if (page_result.Visible == true)
                    {
                        page_result.Visible = false;
                    }

                    DataTable Dt = (DataTable)Session["TransList"];

                    Dt.Rows.RemoveAt(rowIndex);
                    grdTransList.DataSource = Dt;
                    grdTransList.DataBind();

                    Double TempTot = 0;
                    Session["TransList"] = Dt;
                    foreach (DataRow item in Dt.Rows)
                    {
                        TempTot = TempTot + Convert.ToDouble(item["Amount"].ToString());
                    }

                    //txtAvailableBal.Text = Convert.ToDouble(TempTot).ToString("#,##0.00");
                    txtAvailableBal.Text = Convert.ToDouble(Convert.ToDouble(txtTotal.Text) - TempTot).ToString("#,##0.00");

                }

                if (e.CommandName == "Manual_Receipt")
                {
                    OracleConnection conn_getData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                    conn_getData.Open();

                    string ref_no = "";

                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    //selectedRowID = int.Parse(grdTransList.Rows[rowIndex].Cells[2].Text);//original code
                    hf_selectedRowID.Value = grdTransList.Rows[rowIndex].Cells[2].Text;

                    OracleCommand cmd_getDetails = conn_getData.CreateCommand();
                    cmd_getDetails.CommandText = "select t.id, t.ref_no, t.amount, t.createdby, t.createddate, t.receipt_no, t.receipt_narration, t.dtl_id, t.receipt_status, t.effective_end_date from fas_ibt_bulk_receipt_dtl t where t.id='" + hf_selectedRowID.Value + "'";
                    OracleDataReader odr_getDetails = cmd_getDetails.ExecuteReader();

                    while (odr_getDetails.Read())
                    {
                        ref_no = odr_getDetails["ref_no"].ToString();
                    }

                    if (odr_getDetails.HasRows == true)
                    {
                        lbl_refNo.Text = ref_no;
                    }

                    mpe.Show();
                }
            }
            catch (Exception)
            {
                call_error_msg(false);
                lblResult.Text = "Something went wrong, Please Try Again...!";
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----IBT Bulk Receipting grdTransList_RowCommand Error ---- Active User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                throw;
            }
        }

        protected void btn_UpdateRecord_Click(object sender, EventArgs e)//----------------------------Manual Receipt Click Event-----------------------------------------//
        {
            try
            {
                get_LiveRecords_Total();

                if (int.Parse(Session["TotalRecords_WhenPageLoad"].ToString()) == LiveRecord_Count)//if (TotalRecords_WhenPageLoad == LiveRecord_Count)
                {
                    OracleConnection conn_getData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                    conn_getData.Open();

                    receiptNo_update = txt_Rec_No.Text;
                    receiptNarration_update = txt_Rec_Narration.Text;

                    OracleCommand cmd_manualReceipt = conn_getData.CreateCommand();
                    cmd_manualReceipt.CommandText = "sp_fas_ibt_BulkReceiptManual";
                    cmd_manualReceipt.CommandType = CommandType.StoredProcedure;

                    cmd_manualReceipt.Parameters.Add("vID", OracleType.Int32).Value = hf_selectedRowID.Value;
                    cmd_manualReceipt.Parameters.Add("vRECEIPT_NO", OracleType.VarChar).Value = receiptNo_update;
                    cmd_manualReceipt.Parameters.Add("vRECEIPT_NARRATION", OracleType.VarChar).Value = receiptNarration_update;
                    cmd_manualReceipt.Parameters.Add("vRECEIPT_STATUS", OracleType.Int32).Value = 9;
                    cmd_manualReceipt.Parameters.Add("vCreatedBy", OracleType.VarChar).Value = usrName;
                    cmd_manualReceipt.Parameters.Add("vExisting_Record_Count", OracleType.Int32).Value = int.Parse(Session["TotalRecords_WhenPageLoad"].ToString());//TotalRecords_WhenPageLoad;//Existing_Record_Count;
                    cmd_manualReceipt.Parameters.Add("vSerial_No", OracleType.VarChar).Value = DtlSerialNo;
                    cmd_manualReceipt.Parameters.Add("vResult", OracleType.VarChar, 100).Direction = ParameterDirection.Output;

                    cmd_manualReceipt.ExecuteNonQuery();

                    string Error_Messege = cmd_manualReceipt.Parameters["vResult"].Value.ToString();

                    LoadData(DtlID);
                    conn_getData.Close();

                    get_LiveRecords_Total();//New


                    if (Error_Messege == "All Records Successfull")
                    {
                        call_error_msg(true);
                        lblResult.Text = "Manaul Receipt Successfull...! [Base Record Confirmed]";
                    }
                    else if (Error_Messege == "Manual Receipt Successfull")
                    {
                        call_error_msg(true);
                        lblResult.Text = "Manaul Receipt Successfull...! [Base Record Not Confirmed]";
                    }
                    else
                    {
                        call_error_msg(false);
                        lblResult.Text = "Manaul Receipt Fail Please Refresh And Try Again...!, [Record Locked By Another User]";
                    }


                    txt_Rec_No.Text = "";
                    txt_Rec_Narration.Text = "";
                    txt_usrComment.Text = "";


                    running_count = 0;
                    if (running_count == 0)
                    {
                        get_TotalRecords_WhenPageLoad();
                        running_count = running_count + 1;//New
                    }
                }
                else
                {
                    running_count = 0;

                    txt_Rec_No.Text = "";
                    txt_Rec_Narration.Text = "";
                    txt_usrComment.Text = "";

                    call_error_msg(false);
                    lblResult.Text = "Manaul Receipt Fail Please Refresh And Try Again...!, [Record Locked By Another User]";
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                throw;
            }
        }

        protected void formControlsAuthentication()//--------------------------------------------------Form Authentications (Enable User Controls)------------------------//
        {
            //DataTable dt_formControls = (DataTable)Session["PagePermissions"];

            DataRow[] result = dtPageControls.Select("page_id =120");

            foreach (DataRow row in result)
            {
                string field_name = row[1].ToString();

                if (field_name == "CONFIRM_BULK")
                {
                    cmdConfirm.Enabled = true;
                }
                else
                {
                    disableBatchButtons();
                    return;
                }

                field_name = "";
            }
        }

        protected void disableBatchButtons()//---------------------------------------------------------Disable All Batch Buttons------------------------------------------//
        {
            cmdConfirm.Enabled = false;
        }

        public void get_TotalRecords_WhenPageLoad()//--------------------------------------------------Lock Record Validation - Get Count on page load--------------------//
        {
            CommonCLS.IBT_LockRecordValidation rlv = new CommonCLS.IBT_LockRecordValidation();
            string recordType = "Bulk";
            //TotalRecords_WhenPageLoad = rlv.get_Total_At_PageLoad(recordType, DtlSerialNo);
            Session["TotalRecords_WhenPageLoad"] = rlv.get_Total_At_PageLoad(recordType, DtlSerialNo);
        }

        public void get_LiveRecords_Total()//----------------------------------------------------------Lock Record Validation - Get live record count---------------------//
        {
            CommonCLS.IBT_LockRecordValidation rlv = new CommonCLS.IBT_LockRecordValidation();
            string recordType = "Bulk";
            LiveRecord_Count = rlv.get_LiveTotal(recordType, DtlSerialNo);
        }

        public void call_error_msg(bool x)//-----------------------------------------------------------System Messeges----------------------------------------------------//
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