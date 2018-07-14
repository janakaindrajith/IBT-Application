using System;
//using Microsoft.SqlServer.Management.Common;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Data.OracleClient;
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
using Oracle.DataAccess.Client;
using System.Data.OleDb;


namespace BranchMIS.IBTManual_Uploads
{
    public partial class IBT_General_Uploads : System.Web.UI.Page
    {
        private string usrName = "";
        public DataTable dtPageControls = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IBT_UserName"] == null)
            {
                string usrValid = "SessionExpired";
                Response.Redirect("~/FAS_Home.aspx?usr=" + usrValid, false);
                return;
            }


            CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
            usrName = Session["IBT_UserName"].ToString();

            string pageName = (System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath));

            dtPageControls = clsCom.getUserRoleAndPageControls(usrName, pageName);
            Session["PagePermissions"] = dtPageControls;

            bool y = clsCom.getPermissions(usrName, pageName);

            if (y == true)
            {
                string x = "";

                x = Request.QueryString["valid"];
                if ((x == null) || (x == ""))
                {
                    lblResult.Visible = false;
                }
                else if (x == "False")
                {
                    call_error_msg(false);
                    lblResult.Text = "Access Denied...!";
                }
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx", false);
            }
        }

        public DataTable exceldataGeneral(string location)
        {
            try
            {
                CommonCLS.IBTBatches IBTBatch = new CommonCLS.IBTBatches();
                String BatchNo = IBTBatch.GetBatchNo("BATCH_NO");

                DateTime CurrentDate = DateTime.Now;

                DataTable ds = new DataTable();

                OleDbCommand excelCommand = new OleDbCommand();
                OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter();


                string excelConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + location + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

                OleDbConnection excelConn = new OleDbConnection(excelConnStr);

                excelConn.Open();

                DataTable dtPatterns = new DataTable();

                //string usrName = "deshapriya.sooriyaTest";//Remove Please
                excelCommand = new OleDbCommand("SELECT `SERIAL`".ToString() + "as SERIAL,`TRANSACTION_CODE`".ToString() + " as TRANSACTION_CODE,`VOUCHER_DATE`".ToString() + " as VOUCHER_DATE,`PAYMENT_MODE`".ToString() + " as PAYMENT_MODE,`NARRATION`".ToString() + " as NARRATION,`DEBIT_NO`".ToString() + " as DEBIT_NO,`AMOUNT`".ToString() + " as AMOUNT,`CASH_ACCOUNT`".ToString() + " as CASH_ACCOUNT,`POLICY_BRANCH`".ToString() + " as POLICY_BRANCH,`PAYING_PARTY`".ToString() + " as PAYING_PARTY,`INSTRUMENT_NUMBER`".ToString() + " as INSTRUMENT_NUMBER,`INSTRUMENT_DATE` ".ToString() + " as INSTRUMENT_DATE,`STATUS`".ToString() + " as STATUS,'" + usrName + "' as CREATEDBY,'" + CurrentDate + "' as CREATED_DATE, '" + BatchNo + "' as BATCH_NO, 'UPLOADED' as BATCH_STATUS, 'MANUAL' as BATCH_TYPE FROM [UPLOAD$]", excelConn);

                excelDataAdapter.SelectCommand = excelCommand;

                excelDataAdapter.Fill(dtPatterns);
                return dtPatterns;
            }
            catch (Exception)
            {

                throw;
            }
        }







        protected void ibtn_Upload_Click(object sender, ImageClickEventArgs e)
        {
            OracleConnection Oracleconn = new OracleConnection(WebConfigurationManager.ConnectionStrings["ORAWF"].ConnectionString);
            Oracleconn.Open();

            using (OracleBulkCopy bulkcopy = new OracleBulkCopy(Oracleconn))
            {
                if (FileUpload2.HasFile)
                {






                    String Date = DateTime.Now.ToShortDateString().Replace("/", "-");

                    string directoryPath = Server.MapPath("~/IBTManual_UploadFiles/" + Date + "/");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    string fileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
                    string savePath = Server.MapPath("~/IBTManual_UploadFiles/" + Date + "/") + fileName;


                    //Check Duplicate file 
                    if (File.Exists(savePath))
                    {
                        call_error_msg(false);
                        lblResult.Text = "Duplicate file found, Please check the attached file.....!";
                        return;
                    }


                    FileUpload2.PostedFile.SaveAs(savePath);
                    DataTable EmployeeData = exceldataGeneral(savePath);




                    //---------------------------------------------
                    List<string> List = new List<string>();
                    foreach (DataRow row in EmployeeData.Rows)
                    {
                        string Serial = row["SERIAL"].ToString();
                        if (List.Contains(Serial))
                        {
                            call_error_msg(false);
                            lblResult.Text = "Please Check Serial Number Column.....!";
                            return;
                        }
                        List.Add(Serial);
                    }
                    //---------------------------------------------



                    //string filelocation = Convert.ToString(FileUpload2.PostedFile.FileName);

                    ////Save to path
                    //string filename = FileUpload2.FileName;
                    //string savePath = System.IO.Path.Combine(@"\\192.168.10.13\LifeClaims_WF\", filename);
                    //FileUpload2.PostedFile.SaveAs(savePath);
                    ////End Save to path

                    //DataTable EmployeeData = exceldataGeneral(savePath);


                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("SERIAL", "SERIAL"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("TRANSACTION_CODE", "TRANSACTION_CODE"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("VOUCHER_DATE", "VOUCHER_DATE"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PAYMENT_MODE", "PAYMENT_MODE"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("NARRATION", "NARRATION"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DEBIT_NO", "DEBIT_NO"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("AMOUNT", "AMOUNT"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("CASH_ACCOUNT", "CASH_ACCOUNT"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("POLICY_BRANCH", "POLICY_BRANCH"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PAYING_PARTY", "PAYING_PARTY"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("INSTRUMENT_NUMBER", "INSTRUMENT_NUMBER"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("INSTRUMENT_DATE", "INSTRUMENT_DATE"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("STATUS", "STATUS"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("CREATEDBY", "CREATEDBY"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("CREATED_DATE", "CREATED_DATE"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("BATCH_NO", "BATCH_NO"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("BATCH_STATUS", "BATCH_STATUS"));
                    bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("BATCH_TYPE", "BATCH_TYPE"));


                    bulkcopy.DestinationTableName = "FAS_IBT_TEMP_RECEIPT_GI";
                    try
                    {

                        bulkcopy.WriteToServer(EmployeeData);
                        Oracleconn.Close();
                        //LblError.Visible = true;
                        //LblError.Text = "Upload Successful...";

                        call_error_msg(true);
                        lblResult.Text = "Upload Successful........!";

                    }
                    catch (Exception ex)
                    {
                        //LblError.Text = ex.Message.ToString();
                    }
                }

            }

        }

        protected void btn_GI_Print_Click(object sender, System.EventArgs e)
        {
            //string receipt_type = "IBT_GENERAL_RECEIPT.rpt";
            //string receipt_From = "";
            //string receipt_To = "";

            if ((txt_From.Text != "") && (txt_To.Text != ""))
            {
                if (page_result.Visible == true)
                {
                    page_result.Visible = false;
                }

                //receipt_From = txt_From.Text;
                //receipt_To = txt_To.Text;

                //DataTable dt_printData = new DataTable();

                //dt_printData.Columns.Add("Req_Type");
                //dt_printData.Columns.Add("Rec_From");
                //dt_printData.Columns.Add("Rec_To");

                //dt_printData.Rows.Add(receipt_type, receipt_From, receipt_To);

                //Session["Receipt_Print_Request"] = dt_printData;

                //Response.Redirect("~/IBTReports/IBT_Receipt_Viewer.aspx", false);
                string reportName = "FAS_IBT_GI_RECEIPT";
                string parFrom = txt_From.Text;
                string parTo = txt_To.Text;

                Response.Redirect("~/IBTManual_Uploads/IBT_ReportsView.aspx?reportName=" + reportName + "&parFrom=" + parFrom + "&parTo=" + parTo + "", false);

            }
            else
            {
                call_error_msg(false);
                lblResult.Text = "Fields Can Not Be Empty!";
                return;
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

    }
}