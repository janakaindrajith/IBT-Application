﻿//using Microsoft.SqlServer.Management.Common;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Collections;


namespace BranchMIS.IBT
{


    public partial class IBT_Confirmation : System.Web.UI.Page
    {
        //public static string usrName = "";
        //public static int usr_role = 0;

        private string usrName = "";
        public int usr_role = 0;

        //static Server server;
        //static Job job;

        public DataTable dtPageControls = new DataTable();

        protected void Page_Load(object sender, EventArgs e)//-----------------------------------------Page Load Events---------------------------------------------------//
        {
            if (Session["IBT_UserName"] == null)
            {
                string usrValid = "SessionExpired";
                Response.Redirect("~/FAS_Home.aspx?usr=" + usrValid, false);
                return;
            }


            try
            {

                CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();

                usrName = Session["IBT_UserName"].ToString();

                Session["PagePermissions"] = null;
                string pageName = (System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath));

                dtPageControls = clsCom.getUserRoleAndPageControls(usrName, pageName);
                Session["PagePermissions"] = dtPageControls;
                

                bool x = clsCom.getPermissions(usrName, pageName);

                if (x == true)
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            disableBatchButtons();
                            formControlsAuthentication();
                            getDataCategory("-");
                        }
                        //LoadData(Convert.ToString(ddlCategory.SelectedValue));

                    }
                    catch (Exception ex)
                    {
                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                        lblResult.Text = ex.InnerException.ToString();
                    }
                }
                else
                {
                    Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "View Data Co-", Server.MapPath("~/IBTLogFiles/Log_New.txt"));
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
            
        }


        public DataTable GetUnmatchedDetails()
        {
            try
            {

                DataTable Dt = new DataTable();

                OracleConnection conn_GetUserRole = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn_GetUserRole.Open();

                OracleCommand cmd = conn_GetUserRole.CreateCommand();

                cmd.CommandText = " select Dtl.Serial_No, Dept.department_description as DeptDesc, Dept.value " +
                                  " from fas_ibt_departments Dept " +
                                  " inner join fas_ibt_uploaded_dtl Dtl  on Dept.Value = Dtl.Department " +
                                  " where Dept.mrp_batch_process = 1 ";

                OracleDataAdapter oda = new OracleDataAdapter(cmd);

                oda.Fill(Dt);

                return Dt;

            }
            catch (Exception)
            {              
                throw;
            }

        }

        //private void UpdateMRPMCR()//----------------Update MRP MCR-------------------------------//
        //{
        //    try
        //    {

        //        DataTable DtTemp = GetUnmatchedDetails();

        //        String StrMRP = "";
        //        String StrMCR = "";

        //        foreach (DataRow Dr in DtTemp.Rows)
        //        {
        //            if (Dr["DeptDesc"].ToString() == "MRP")
        //            {
        //                StrMRP = StrMRP + "'" + Dr["serial_NO"].ToString() + "',";
        //            }
        //            if (Dr["DeptDesc"].ToString() == "MCR")
        //            {
        //                StrMCR = StrMCR + "'" + Dr["serial_NO"].ToString() + "',";
        //            }               
        //        }

        //        StrMRP = StrMRP.Remove(StrMRP.Length - 1, 1);
        //        StrMCR = StrMCR.Remove(StrMCR.Length - 1, 1);



        //        //------------------------------Get MRP/MCR Details---------------------------------
        //        var dtMRP = new DataTable();
        //        var dtMCR = new DataTable();

        //        SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlconnForMRP"].ConnectionString);
        //        SqlCommand sqlcmd = new SqlCommand();
        //        sqlcmd.CommandType = System.Data.CommandType.Text;


        //        sqlcmd.CommandText = " SELECT REPLACE(C.PropNo,'/','') AS PROPOSALNO,UPPER(C.Name) AS CLIENTNAME, " +
        //                             " C.Premiumpolicyfee AS PREMIUM,UPPER(SUBSTRING(C.Code,1,4)) AS HNBCODE,  " +
        //                             " C.Ibtdate AS IBTDATE,UPPER(REPLACE(REPLACE(C.ACCOUNT,' ',''),'-','')) AS CASHACCOUNT," +
        //                             " 'MRP' AS PRODUCT,BAN.Branchname,C.BNKCode, 'Count' as Count  FROM   Chequedetail C,BANK BAN  " +
        //                             " WHERE  BAN.BNKCode = C.Code  " + 
        //                             " --AND Convert(datetime,C.ibtDate,103) >= '05/08/2016'" +
        //                             " --AND Convert(datetime,C.ibtDate,103) <= '11/08/2016' " +
        //                             " AND REPLACE(REPLACE(C.ACCOUNT,' ',''),'-','') IN (" + StrMRP + ") " +
        //                             " UNION  " +
        //                             " SELECT REPLACE(C.PropNo,'/','') AS PROPOSALNO,UPPER(C.Name) AS CLIENTNAME," +
        //                             " C.Premiumpolicyfee AS PREMIUM,UPPER(SUBSTRING(C.Code,1,4)) AS HNBCODE,  " +
        //                             " C.Ibtdate AS IBTDATE,UPPER(REPLACE(REPLACE(C.ACCOUNT,' ',''),'-','')) AS CASHACCOUNT, " +
        //                             " 'MCR' AS PRODUCT,BAN.Branchname,C.BNKCode, 'Count' as Count   FROM   ChequedetailMicro C,BANK BAN  " +
        //                             " WHERE  BAN.BNKCode = C.Code   " +
        //                             " --AND Convert(datetime,C.ibtDate,103) >= '05/08/2016' " +
        //                             " --AND Convert(datetime,C.ibtDate,103) <= '11/08/2016' " +
        //                             " AND REPLACE(REPLACE(C.ACCOUNT,' ',''),'-','') IN (" + StrMCR + ") ";


        //        sqlcmd.Connection = sqlconn;
        //        sqlconn.Open();

        //        var dataReader = sqlcmd.ExecuteReader();
        //        dtMRP.Load(dataReader);


        //        DataView dv = dtMRP.DefaultView;
        //        dv.Sort = "IBTDATE desc";
        //        dtMRP = dv.ToTable();

        //        sqlconn.Close();
        //        sqlconn.Dispose();
        //        dataReader.Close();
        //        sqlcmd.Dispose();



        //        DataTable DtRecCount= new DataTable();
        //        DtRecCount.Columns.Add("SerialNo", typeof(string));
        //        DtRecCount.Columns.Add("Count", typeof(int));

        //        ArrayList arrRegular = new ArrayList();
        //        ArrayList arrBulk= new ArrayList();


        //        foreach (DataRow item in dtMRP.Rows)
        //        {
        //            String CASHACCOUNT = item["CASHACCOUNT"].ToString();

        //            DataRow[] DrTemp1 = dtMRP.Select("CASHACCOUNT = '" + CASHACCOUNT + "'");

        //            if (DrTemp1.Count() > 1)
        //            {
        //                if (!arrBulk.Contains(CASHACCOUNT))
        //                {
        //                    if (!(CASHACCOUNT == "" || CASHACCOUNT == " " || CASHACCOUNT == null))
        //                    {
        //                        arrBulk.Add(CASHACCOUNT);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (!(CASHACCOUNT == "" || CASHACCOUNT == " " || CASHACCOUNT == null))
        //                {
        //                    arrRegular.Add(CASHACCOUNT);
        //                }
        //            }
        //        }


        //        //------------------------- MRP / MCR Receipts Updation---------------------
        //        using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString()))
        //        {
        //            conn.Open();
        //            OracleCommand command = conn.CreateCommand();
        //            OracleTransaction transaction;

        //            transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
        //            command.Transaction = transaction;


        //            foreach (var item in arrRegular)
        //            {
        //                String CASHACCOUNT = item.ToString();
        //                DataRow Dr = dtMRP.Select("CASHACCOUNT = '" + CASHACCOUNT + "'").Single();

        //                //-----------------------Regular Receipting-------------------------------------
        //                OracleCommand cmd = new OracleCommand();//conn.CreateCommand();
        //                cmd.Connection = conn;

        //                String SerialNo = Dr["CASHACCOUNT"].ToString();
        //                String PolicyNo = Dr["PROPOSALNO"].ToString();
        //                String Premium = Dr["PREMIUM"].ToString();

        //                cmd.CommandText = "SP_FAS_UPDATE_MRPMCR_DTL";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.AddWithValue("vSerialNo", OracleType.NVarChar).Value = SerialNo;
        //                cmd.Parameters.AddWithValue("vPolicyNo", OracleType.NVarChar).Value = PolicyNo;
        //                cmd.Parameters.AddWithValue("vAmount", OracleType.Number).Value = Premium;

        //                cmd.Transaction = transaction;
        //                cmd.ExecuteNonQuery();
        //                //-------------------------------------------------------------------------------
        //            }



        //            //------------------------Bulk Receipting----------------------------------------
        //            Int16 SubNo = 1;
        //            foreach (var item in arrBulk)
        //            {
        //                String CASHACCOUNT = item.ToString();
        //                DataRow[] Dr = dtMRP.Select("CASHACCOUNT = '" + CASHACCOUNT + "'");

        //                DataTable dtTemp = Dr.CopyToDataTable();

        //                foreach (DataRow row in dtTemp.Rows)
        //                {

        //                    String SerialNo = row["cashaccount"].ToString();
        //                    String RefNumber = row["proposalno"].ToString();
        //                    String Amount = row["Premium"].ToString();

        //                    OracleCommand cmdBulk = new OracleCommand();//conn.CreateCommand();
        //                    cmdBulk.Connection = conn;

        //                    cmdBulk.CommandText = "SP_FAS_UPDATE_MRPMCR_DTL_BULK";
        //                    cmdBulk.CommandType = CommandType.StoredProcedure;

        //                    //cmdBulk.Parameters.AddWithValue("vID", OracleType.Int32).Value = DtlID;
        //                    cmdBulk.Parameters.AddWithValue("vSerialNo", OracleType.VarChar).Value = SerialNo;
        //                    cmdBulk.Parameters.AddWithValue("vSubNo", OracleType.VarChar).Value = SubNo;
        //                    cmdBulk.Parameters.AddWithValue("vRefNumber", OracleType.VarChar).Value = RefNumber;
        //                    cmdBulk.Parameters.AddWithValue("vAmount", OracleType.Number).Value = Convert.ToDouble(Amount);
        //                    cmdBulk.Parameters.AddWithValue("vCreatedBy", OracleType.Number).Value = "MRP";

        //                    SubNo = Convert.ToInt16(SubNo + 1);

        //                    cmdBulk.Transaction = transaction;

        //                    cmdBulk.ExecuteNonQuery();

        //                }

        //            }
        //            //-------------------------------------------------------------------------------


        //            transaction.Commit();
        //            conn.Close();
        //        }
        //        //----------------------------------------------------------------


        //    }
        //    catch (Exception ex)
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(false);
        //        lblResult.Text = ex.InnerException.ToString();
        //    }
        //}

        public void getDataCategory(String Type)//-----------------------------------------------------Get Data Category (Departments)------------------------------------//
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();
            OracleCommand cmd2 = conn.CreateCommand();

            //cmd2.CommandText = "select d.value, d.department_description from fas_ibt_departments  d order by d.value";
            cmd2.CommandText = "select k.value, k.department_description from fas_ibt_departments k where k.value in(select d.department_id  " +
                               "from fas_ibt_rolecat d where d.role_id = (select t.role_id from fas_ibt_role_assigned t where t.user_name = '" + usrName + "'))";

            OracleDataAdapter oda = new OracleDataAdapter(cmd2);
            DataTable dt = new DataTable();

            oda.Fill(dt);

            ddlCategory.DataTextField = "department_description";
            ddlCategory.DataValueField = "value";

            ddlCategory.DataSource = dt;
            ddlCategory.DataBind();

            //ddlCategory.Items.Insert(0, new ListItem("All", "-1"));
            conn.Close();
        }
        
        private void LoadData(String CatID)//----------------------------------------------------------Load Data To Grid View---------------------------------------------//
        {
            try
            {

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();

                String Where = "";


                if (Session["IBT_Company"] == null)
                {
                    return;
                }


                //Life General Bifurcation according to the logged user.
                if (Session["IBT_Company"].ToString() == "GENERAL")
                {
                    Where = " AND (P.VALUE = 1 or P.VALUE = 3)";
                }

                if (Session["IBT_Company"].ToString() == "LIFE")
                {
                    Where = " AND (P.VALUE = 2 or P.VALUE = 4)";
                }

                if (Session["IBT_Company"].ToString() == "ALL")
                {
                    Where = "";
                    //Where = " AND P.VALUE = -1";
                }

                Where = Where + " AND T.DEPARTMENT = " + CatID;

                //OracleCommand cmd = new OracleCommand(" SELECT T.ID, T.ACCOUNT_NO,T.SERIAL_NO,to_char(T.TRANSACTION_DATE, 'DD/MM/RRRR') AS TRAN_DATE,T.DESCRIPTION,T.POLICY_NO,T.CREDIT,P.PRODUCT_DESCRIPTION,P.VALUE " +
                //                      " FROM FAS_IBT_UPLOADED_DTL T INNER JOIN FAS_IBT_PRODUCTS P ON " +
                //                      " P.VALUE = T.PRODUCT  WHERE T.MATCHING_STATUS = 2 AND T.DEPARTMENT <> 1 " + Where, conn);


                OracleCommand cmd = new OracleCommand(" SELECT T.ID, T.ACCOUNT_NO,T.SERIAL_NO,to_char(T.TRANSACTION_DATE, 'DD/MM/RRRR') AS TRAN_DATE,T.DESCRIPTION,T.POLICY_NO,T.CREDIT,P.PRODUCT_DESCRIPTION,P.VALUE " +
                                                      " FROM FAS_IBT_UPLOADED_DTL T INNER JOIN FAS_IBT_PRODUCTS P ON " +
                                                      " P.VALUE = T.PRODUCT  WHERE T.MATCHING_STATUS = 2 AND t.effective_end_date is null AND T.DEPARTMENT <> 1" + Where, conn);

                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                //Session["Dt"]

                //call_error_msg(true);
                lblReceiptStatus.Text = "     - Total Records - " + Convert.ToString(dt.Rows.Count);

                GrdIBTConfirmation.DataSource = dt;
                GrdIBTConfirmation.DataBind();

                Session["ConfirmedData"] = dt;

                conn.Close();

            }
            catch (Exception)
            {
                
                throw;
            }


        }

        protected void cmdConfirm_Click(object sender, EventArgs e)//----------------------------------Confirmed Selected Records Click Event-----------------------------//
        {
            try
            {
                LoadExeptionData();

                ConfirmRecords(false);
                LoadData(Convert.ToString(ddlCategory.SelectedValue));
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }

        }

        private Int32 GetUnreceiptedCount()
        {
            Int32 Count = 0;

            try
            {

                OracleConnection conn_GetUserRole = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn_GetUserRole.Open();

                OracleCommand cmd = conn_GetUserRole.CreateCommand();
                cmd.CommandText = "select count(t.serial) as Count from iims_acc.temp_life_receipt_batch t where t.status is null";
                
                //OracleDataReader odr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                //dt.Load(odr);

                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);

                Count = Convert.ToInt32(dt.Rows[0]["Count"].ToString());

                return Count;
            }
            catch (Exception)
            {

                throw;
            }
        }




        private void ConfirmRecords(Boolean SelectAll)//-----------------------------------------------Confirm All Records------------------------------------------------//
        {
            try
            {

                //Int32 RecCount = GetUnreceiptedCount();

                //if (RecCount > 499)
                //{
                //    call_error_msg(true);
                //    lblResult.Text = "Please Run the relavant receipt batch and do the other confirmations...!";
                //    return;
                //}

                CommonCLS.IBTBatches IBTBatch = new CommonCLS.IBTBatches();
                String BatchNo = IBTBatch.GetBatchNo("BATCH_NO");


                if (GrdIBTConfirmation.Rows.Count > 0)
                {
                    using (OracleConnection OraCon = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString()))
                    {

                        OraCon.Open();
                        OracleCommand command = OraCon.CreateCommand();
                        OracleTransaction transaction;

                        // Start a local transaction
                        transaction = OraCon.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                        // Assign transaction object for a pending local transaction
                        command.Transaction = transaction;

                        try
                        {

                            DataTable Dt = (DataTable)Session["ConfirmedData"];

                            //when user select all the records to be confirmed
                            if (SelectAll)
                            {
                                Int16 Count = 0;

                                foreach (DataRow item in Dt.Rows)
                                {

                                    //if (Count == 500)
                                    //{
                                    //    call_error_msg(true);
                                    //    lblResult.Text = "Please Run the relavant receipt batch and do the other confirmations...!";
                                    //    break;
                                    //}

                                     OracleCommand cmd = OraCon.CreateCommand();
                                     cmd.CommandText = "SP_FAS_IBT_CONFIRMATION";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("vPolicyNo", OracleType.VarChar).Value = item["POLICY_NO"].ToString();
                                        cmd.Parameters.Add("vProduct", OracleType.VarChar).Value = item["VALUE"].ToString();
                                        cmd.Parameters.Add("vAmount", OracleType.Number).Value = item["CREDIT"].ToString();
                                        cmd.Parameters.Add("vID", OracleType.Number).Value = Convert.ToInt64(item["ID"].ToString());
                                        cmd.Parameters.Add("vCreatedBy", OracleType.VarChar).Value = usrName;
                                        cmd.Parameters.Add("vBatchNo", OracleType.VarChar).Value = BatchNo;

                                        cmd.Transaction = transaction;

                                        cmd.ExecuteNonQuery();//***********************************************************************************************Test

                                        Count ++;
                                }
                            }
                            else
                            {
                                foreach (GridViewRow row in GrdIBTConfirmation.Rows)//Selected Records
                                {
                                    CheckBox chk = (row.Cells[0].FindControl("chkboxselect") as CheckBox);
                                    DropDownList Commision = row.Cells[1].FindControl("ddlCommision") as DropDownList;


                                    if ((chk != null && chk.Checked) || SelectAll)
                                    {

                                        OracleCommand cmd = OraCon.CreateCommand();
                                        cmd.CommandText = "SP_FAS_IBT_CONFIRMATION";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("vPolicyNo", OracleType.VarChar).Value = row.Cells[6].Text.ToString();
                                        cmd.Parameters.Add("vProduct", OracleType.VarChar).Value = row.Cells[9].Text.ToString();
                                        cmd.Parameters.Add("vAmount", OracleType.Number).Value = Convert.ToDouble(row.Cells[7].Text);
                                        cmd.Parameters.Add("vID", OracleType.Number).Value = Convert.ToInt64(row.Cells[1].Text);
                                        cmd.Parameters.Add("vCreatedBy", OracleType.VarChar).Value = usrName;
                                        cmd.Parameters.Add("vBatchNo", OracleType.VarChar).Value = BatchNo;

                                        cmd.Transaction = transaction;

                                        cmd.ExecuteNonQuery();//***********************************************************************************************Test

                                        //--------Send Data To TCS for Receipting---------
                                        // Exess 
                                        // Partial
                                        // When Two Debit Returns 
                                        // When PPW Cancelled
                                        //------------------------------------------------
                                    }

                                }
                            }
                            


                            transaction.Commit();
                            OraCon.Close();

                            GrdIBTConfirmation.DataSource = null;
                            GrdIBTConfirmation.DataBind();



                            //--------Load New business and renewal system detected exeptions---------
                            LoadExeptionData();

                            //------------------------------------------------------------------------



                            call_error_msg(true);
                            lblResult.Text = "Confirmation Successfull...!";
                            CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----IBT Confirmation Successfull ---- Active User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));

                            //LoadData();
                        }

                        catch (Exception ex)
                        {
                            CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                            call_error_msg(false);
                            lblResult.Text = ex.InnerException.ToString();
                        }

                    }

                    
                }

                else
                {
                    call_error_msg(false);
                    lblResult.Text = "No Matching Records Available!";
                }

            }

            catch (Exception)
            {
                throw;
            }
        }

        private static void ClearExeptionData(String usrName)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "sp_fas_ibt_clear_Exception";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("vUserID", OracleType.VarChar).Value = usrName;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void LoadExeptionData()
        {
            try
            {

                DataTable DtTemp = new DataTable();

                OracleConnection conn_GetUserRole = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn_GetUserRole.Open();

                OracleCommand cmd = conn_GetUserRole.CreateCommand();

                cmd.CommandText = " select t.serial_no as Policy_No from fas_ibt_new_renewal_validate t where t.createdby = '" + usrName + "'";

                OracleDataAdapter oda = new OracleDataAdapter(cmd);

                oda.Fill(DtTemp);

                if (DtTemp.Rows.Count > 0)
                {
                    grdExceptions.DataSource = DtTemp;
                    grdExceptions.DataBind();

                    mpe.Show();
                }
                else
                {
                    return;
                }
            }
            catch (Exception)
            {            
                throw;
            }
        }
       
        //private Boolean CheckReceiptStatus(String PolicyNo,Double IBTAmt)
        //{
        //    try
        //    {

        //        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        //        conn.Open();
        //        OracleCommand cmd = conn.CreateCommand();
        //        cmd.CommandText = "SELECT thu.unadjusted_Amt FROM iims_uwr.t_report_dncn_gwp_thu thu where thu.pol_no = '" + PolicyNo + "' and thu.unadjusted_Amt = " + IBTAmt;
        //        OracleDataReader odr = cmd.ExecuteReader();

        //        while (odr.Read())
        //        {
        //            Double clossing_bal = 0;
        //            clossing_bal = Convert.ToDouble(odr["unadjusted_Amt"].ToString());
        //        }

        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        protected void GrdIBTConfirmation_RowDataBound(object sender, GridViewRowEventArgs e)//--------Grid View Row Data Bound-------------------------------------------//
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Width = new Unit("40px");
                e.Row.Cells[1].CssClass = "HiddenCol";
                e.Row.Cells[9].CssClass = "HiddenCol";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = new Unit("40px");
                e.Row.Cells[1].CssClass = "HiddenCol";
                e.Row.Cells[9].CssClass = "HiddenCol";
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

        protected void GrdIBTConfirmation_PageIndexChanging(object sender, GridViewPageEventArgs e)//--Grid View Paging---------------------------------------------------//
        {
            try
            {
                GrdIBTConfirmation.PageIndex = e.NewPageIndex;
                LoadData(ddlCategory.SelectedValue);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //protected void cmdLifebatch_Click(object sender, EventArgs e)//--------------------------------Life Batch Click Event---------------------------------------------//
        //{
        //    try
        //    {
        //        Thread thread = new Thread(new ThreadStart(LifeBatchExecute));
        //        thread.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(false);
        //        lblResult.Text = ex.InnerException.ToString();
        //    }
        //}

        //private void LifeBatchExecute()//--------------------------------------------------------------Life Batch Execute-------------------------------------------------//
        //{
        //    try
        //    {
        //        BranchMIS.CommonCLS.IBTBatches.LifeBatch();
        //        BranchMIS.CommonCLS.IBTEmails.SendEmails("Life", ConfigurationManager.ConnectionStrings["ORAWF_ACC"].ToString(), ConfigurationManager.ConnectionStrings["sqlconn"].ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //protected void cmdGeneralbatch_Click(object sender, EventArgs e)//-----------------------------General Bacth Click Event------------------------------------------//
        //{
        //    try
        //    {
        //        Thread thread = new Thread(new ThreadStart(GeneralBatchExecute));
        //        thread.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(false);
        //        lblResult.Text = ex.InnerException.ToString();
        //    }

        //}

        //private void GeneralBatchExecute()//-----------------------------------------------------------General Batch Execute----------------------------------------------//
        //{
        //    try
        //    {
        //        BranchMIS.CommonCLS.IBTBatches.GeneralBatch();
        //        BranchMIS.CommonCLS.IBTEmails.SendEmails("General", ConfigurationManager.ConnectionStrings["ORAWF_ACC"].ToString(), ConfigurationManager.ConnectionStrings["sqlconn"].ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //protected void cmdNonTCSBatch_Click(object sender, EventArgs e)//------------------------------Non TCS Batch Click Event------------------------------------------//
        //{
        //    try
        //    {
        //        Thread thread = new Thread(new ThreadStart(NonTCSBatchExecute));
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        call_error_msg(false);
        //        lblResult.Text = ex.InnerException.ToString();
        //    }
        //}

        //private void NonTCSBatchExecute()//------------------------------------------------------------Non TCS Batch Execute----------------------------------------------//
        //{
        //    try
        //    {
        //        BranchMIS.CommonCLS.IBTBatches.NonTCSBatch();
        //        BranchMIS.CommonCLS.IBTEmails.SendEmails("NonTCS", ConfigurationManager.ConnectionStrings["ORAWF_ACC"].ToString(), ConfigurationManager.ConnectionStrings["sqlconn"].ToString());
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        protected void GrdIBTConfirmation_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Width = 100; // hides the first column
        }

        protected void cmdConfirmAll_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmRecords(true);
                LoadData(Convert.ToString(ddlCategory.SelectedValue));
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }

        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadData(Convert.ToString(ddlCategory.SelectedValue));
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void formControlsAuthentication()//--------------------------------------------------Form Authentications (Enable User Controls)------------------------//
        {
            //DataTable dt_formControls = (DataTable)Session["PagePermissions"];

            DataRow[] result = dtPageControls.Select("page_id =10");

            foreach (DataRow row in result)
            {
                //string pageid = row[0].ToString();
                string field_name = row[1].ToString();

                if (field_name == "CONFIRM_ALL_RECORDS")
                {
                    cmdConfirmAll.Enabled = true;
                }
                else if (field_name == "CONFIRM_SELECTED_RECORDS")
                {
                    cmdConfirm.Enabled = true;
                }
                //else if (field_name == "GENERAL_RECEIPT")
                //{
                //    cmdGeneralbatch.Enabled = true;
                //}
                //else if(field_name == "LIFE_RECEIPT")
                //{
                //    cmdLifebatch.Enabled = true;
                //}
                //else if(field_name == "NON_TCS_RECEIPT")
                //{
                //    cmdNonTCSBatch.Enabled = true;
                //}
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
            cmdConfirmAll.Enabled = false;
            cmdConfirm.Enabled = false;
            //cmdGeneralbatch.Enabled = false;
            //cmdLifebatch.Enabled = false;
            //cmdNonTCSBatch.Enabled = false;
        }

        

        protected void cmd_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearExeptionData(usrName);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void Cmd_Continue_Click(object sender, EventArgs e)
        {
            try
            {
                Process();
                ClearExeptionData(usrName);
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        private void Process()//Change the category from NewBusiness to Renewal if the user confirms
        {
            try
            {
                using (OracleConnection OraCon = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString()))
                {
                    OraCon.Open();
                    OracleCommand command = OraCon.CreateCommand();
                    OracleTransaction transaction;

                    transaction = OraCon.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    command.Transaction = transaction;

                    foreach (GridViewRow row in grdExceptions.Rows)
                    {
                        String SerialNo = row.Cells[0].Text.ToString();

                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = OraCon;
                        cmd.CommandText = "sp_fas_ibt_UPLD_INSERT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("vUserID", OracleType.VarChar).Value = usrName;
                        cmd.Parameters.AddWithValue("vSerialNo", OracleType.VarChar).Value = SerialNo;

                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    OraCon.Close();

                    LoadData(Convert.ToString(ddlCategory.SelectedValue));

                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }


    }
}