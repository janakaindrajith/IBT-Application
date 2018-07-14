using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.DirectoryServices;
using System.Net.Mail;
using System.IO;
using System.Data.OracleClient;
using System.Transactions;
using System.Data.OleDb;


namespace BranchMIS.IBT
{
    public partial class IBTUpload : System.Web.UI.Page
    {
        String AccountNo = "";
        String SerialNo = "";
        String TransID = "";
        String TransDate = "";
        String CheckNo = "";
        String Description = "";
        String PolicyNo = "";
        String Debit = "";
        String Credit = "";
        String Balance = "";
        String OpenningBalance = "";
        String ClosingBalance = "";
        String StatementDate = "";
        String ValueDate = "";

        String BankRef = "";
        String BankBranch = "";
        String ChequeNo = "";
        String FormatID = "";

        DataTable dtUpload = new DataTable();

        Int64 TotalRowUploaded = 0;
        Int64 TotalAccountsUploaded = 0;
        Int64 TotalAccountsSelected = 0;

        public static String CashAccountNo = "";
        public static String Product = "";

        public static string usrName = "";
        public static string CurrentDate = "";

        //public static string usrName = "";
        public int usr_role = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IBT_UserName"] == null)
            {
                string usrValid = "SessionExpired";
                Response.Redirect("~/FAS_Home.aspx?usr=" + usrValid, false);
                return;
            }


            usrName = Session["IBT_UserName"].ToString();
            string pageName = (System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath));

            CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
            bool x = clsCom.getPermissions(usrName, pageName);

            if (x == true)
            {
                try
                {
                    if (!IsPostBack)
                    {
                        DateTime s_date = System.DateTime.Now;
                        CurrentDate = s_date.ToShortDateString();
                        //txtFromDate.Text = s_date.ToShortDateString();          
                        UploadingStatusViewLog(Convert.ToDateTime(CurrentDate));

                    }
                    //CommonCLS.CommonFunctions.IBT_Bifurcate();
                    //CommonCLS.CommonFunctions.IBT_Matching();
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

        private DataTable MT940_Upload(String FilePath)
        {

            try
            {
                cmdUpload.Enabled = true;

                string TempTransDate = "0";

                string text = System.IO.File.ReadAllText(FilePath);

                System.Console.WriteLine("Contents of WriteText.txt = {0}", text);

                string[] lines = System.IO.File.ReadAllLines(FilePath);

                Int16 Count = 0;

                Boolean isFirstTime = true;

                Product = "0"; OpenningBalance = "0"; TransDate = "0"; Debit = "0"; Credit = "0";

                foreach (string line in lines)
                {



                    //if (line.StartsWith(":20:"))//Statement Date


                    if (line.StartsWith(":25:"))
                        Product = line.Substring(4, line.Length - 4);

                    //if (line.StartsWith(":28C:"))//Statement No


                    if (line.StartsWith(":60F:"))
                        OpenningBalance = line.Substring(line.IndexOf("LKR"), line.Length - line.IndexOf("LKR")).Replace(",", ".").Replace("LKR", "");

                    if (line.StartsWith(":61:"))
                    {

                        //Value Date
                        TransDate = line.Substring(4, 6);
                        TransDate = TransDate.Substring(4, 2) + "/" + TransDate.Substring(2, 2) + "/" + TransDate.Substring(0, 2);
                        TransDate = Convert.ToDateTime(TransDate).ToShortDateString();






                        //janaka
                        if (TransDate.Length > 6 && isFirstTime != true)
                        {
                            if (TransDate != TempTransDate)
                            {
                                dtUpload = null;
                                call_error_msg(false);
                                lblResult.Text = "Please check file contain multiple dates";
                                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "MT940 file exists multiple dates...", Server.MapPath("~/IBTLogFiles/Log.txt"));
                                //return null;
                                cmdUpload.Enabled = false;
                            }
                        }
                        TempTransDate = Convert.ToDateTime(TransDate).ToShortDateString();

                        isFirstTime = false;





                        if (line.Substring(14, 1) == "C")
                        {
                            Credit = line.Substring(line.IndexOf("C"), line.IndexOf(",") + 3 - line.IndexOf("C")).Replace(",", ".");
                            Credit = Credit.Replace("C", "");
                        }

                        if (line.Substring(14, 1) == "D")
                        {
                            Debit = line.Substring(line.IndexOf("D"), line.IndexOf(",") + 3 - line.IndexOf("D")).Replace(",", ".");
                            Debit = Debit.Replace("D", "");
                        }
                    }

                    if (line.StartsWith(":86:"))//Value Date
                    {
                        Description = line.Substring(4, line.Length - 4);
                    }



                    if (TransDate.Length > 1 && (Credit.Length > 1 || Debit.Length > 1) && Description.Length > 1)
                    {
                        //FormatID = -1(this is just assigned. Beacsues policy no auto filterring function needs format ID)
                        //AddNewRow(Product, OpenningBalance, ClosingBalance, SerialNo, TransDate, Description, "", Debit, Credit, Balance, BankRef, BankBranch, CheckNo, FormatID, CashAccountNo); 

                        AddNewRow(Product, OpenningBalance, ClosingBalance, SerialNo, TransDate, Description, "", Debit, Credit, Balance, BankRef, BankBranch, CheckNo, "-1", CashAccountNo);
                        TransDate = ""; Credit = ""; Debit = ""; Description = "";
                        Count++;
                    }

                    if (!(TransDate.Length > 1 || Credit.Length > 1 || Debit.Length > 1 || Description.Length > 1))
                    {
                        //TransDate = ""; Credit = ""; Debit = ""; Description = "";
                    }

                    if (line.StartsWith(":62F:"))//Value Date
                    {
                        ClosingBalance = line.Substring(line.IndexOf("LKR"), line.Length - line.IndexOf("LKR"));
                        ClosingBalance = ClosingBalance.Replace("LKR", "").Replace(",", ".");
                    }

                }





            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));

                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }

            return null;
        }

        private DataTable TEXT_Upload(DataTable Dt)
        {
            return null;
        }

        private DataTable CSV_Upload(DataTable Dt)
        {
            String Date = DateTime.Now.ToShortDateString().Replace("/", "-");

            string directoryPath = Server.MapPath("~/IBTUploadFiles/" + Date + "/");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }


            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/IBTUploadFiles/" + Date + "/") + fileName);

            //Reading All text  
            string CSVFilePathName = Server.MapPath("~/IBTUploadFiles/" + Date + "/") + fileName;


            //string CSVFilePathName = @"F:\Finance Automation\IBT\HNB_CSV.csv";
            string[] Lines = File.ReadAllLines(CSVFilePathName);
            string[] Fields;
            Fields = Lines[0].Split(new char[] { ',' });
            int Cols = Fields.GetLength(0);
            DataTable dt = new DataTable();
            //1st row must be column names; force lower case to ensure matching later on.
            for (int i = 0; i < Cols; i++)
                dt.Columns.Add(Fields[i].ToLower(), typeof(string));
            DataRow Row;
            for (int i = 1; i < Lines.GetLength(0); i++)
            {
                Fields = Lines[i].Split(new char[] { ',' });
                Row = dt.NewRow();
                for (int f = 0; f < Cols; f++)
                    Row[f] = Fields[f];
                dt.Rows.Add(Row);
            }

            foreach (DataRow dr in dt.Rows)
            {
                String a = dr[0].ToString();
                int index = dt.Rows.IndexOf(dr);

                if (index == 0)
                {
                    Product = a.Substring(a.IndexOf("S5") + 3, 12);
                    OpenningBalance = a.Substring(a.IndexOf("S6") + 3, a.IndexOf(" ") + 3);
                    ClosingBalance = a.Substring(a.IndexOf("S7") + 3);
                }

                if (index >= 6)
                {
                    SerialNo = dr[0].ToString();
                    TransDate = dr[1].ToString();
                    Description = dr[2].ToString();
                    Debit = dr[5].ToString();
                    Credit = dr[6].ToString();
                    Balance = dr[7].ToString();

                    //AddNewRow(AccountNo, OpenningBalance, ClosingBalance, SerialNo, TransDate, Description, Debit, Credit, Balance);

                }
            }

            GrdUpload.DataSource = dtUpload;
            GrdUpload.DataBind();

            return null;
        }

        private void SetPath(DataTable Dt)
        {
            try
            {

                if (FileUpload1.HasFiles)
                {

                    CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "View Data Started-", Server.MapPath("~/IBTLogFiles/Log_New.txt"));


                    TotalAccountsSelected = FileUpload1.PostedFiles.Count;


                    foreach (var file in FileUpload1.PostedFiles)
                    {
                        String Date = DateTime.Now.ToShortDateString().Replace("/", "-");

                        string directoryPath = Server.MapPath("~/IBTUploadFiles/" + Date + "/");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string fileName = Path.GetFileName(file.FileName);

                        if (!(fileName.Contains("_")))
                        {
                            CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "Wrong File Name - " + fileName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                            continue;
                        }

                        String FileType = fileName.Substring(fileName.IndexOf("."), fileName.Length - fileName.IndexOf(".")).Replace(".", "");

                        fileName = fileName.Substring(0, fileName.IndexOf("_"));

                        file.SaveAs(Server.MapPath("~/IBTUploadFiles/" + Date + "/") + fileName);

                        string FilePathName = Server.MapPath("~/IBTUploadFiles/" + Date + "/") + fileName;

                        string con = "";


                        if (FileType == "xls")
                        {
                            con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + FilePathName + "';" + @"Extended Properties='Excel 8.0;HDR=NO;IMEX=1;'";
                        }
                        else
                        {
                            con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePathName + ";Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;MAXSCANROWS=0;\"";
                        }


                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "View Started-" + fileName, Server.MapPath("~/IBTLogFiles/Log.txt"));

                        Boolean RecFound = false;

                        RecFound = UploadData(Dt, con, fileName, FileType, FilePathName);

                        //Account Detail Separate Row

                        if (RecFound)
                        {
                            AddNewRow("--", "0", "0", "0", null, "", "", "0", "0", "0", "", "", "", "", "");
                            TotalAccountsUploaded++;
                        }

                        //---------------------------

                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "View Done   -" + fileName, Server.MapPath("~/IBTLogFiles/Log.txt"));

                        //TotalAccountsUploaded++;

                    }
                }

                //Data Bind for Grid view
                LoadUploadDataToGrid();


                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "View Data Completed", Server.MapPath("~/IBTLogFiles/Log.txt"));

            }
            catch (Exception ex)
            {
                //CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));

                call_error_msg(false);
                lblResult.Text = ex.Message.ToString();
                throw ex;
            }
        }

        private Boolean UploadData(DataTable Dt, String Con, String FileName, String FileType, String FilePath)
        {
            try
            {

                if (FileType == "txt")
                {
                    DataTable Temp = MT940_Upload(FilePath);
                    if (Temp == null)
                    {
                        //call_error_msg(false);
                        //lblResult.Text = "File Contain multiple dates";
                        //CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "File Contain multiple dates...", Server.MapPath("~/IBTLogFiles/Log.txt"));

                        //cmdUpload.Enabled = false;

                        //return false;

                    }

                    if (dtUpload != null)
                    {
                        if (dtUpload.Rows.Count == 0)
                        {
                            cmdUpload.Enabled = false;

                            return false;
                        }
                    }

                }
                else
                {
                    DataTable DtConfig = GetConfig(FileName.Replace(".xls", "").Replace(".xlsx", "").Trim());

                    if (DtConfig.Rows.Count == 0)
                    {
                        call_error_msg(false);
                        lblResult.Text = "Format Configuration Error...";
                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "Format Error - Please check the format...", Server.MapPath("~/IBTLogFiles/Log.txt"));
                        return false;
                    }

                    string TempFormatID = DtConfig.Rows[0][1].ToString();

                    DataTable DtConfigTrans = DtConfig;

                    DataRow[] DrTemp1 = DtConfig.Select("ROW_INDEX<>" + -1);
                    DtConfig = DrTemp1.CopyToDataTable();




                    DataTable DtAccConfig = GetAccountConfig();
                    DataRow DrAccConfig = DtAccConfig.Select("ACC_NO = '" + FileName + "'").Single();

                    String vProduct = DrAccConfig["PRODUCT"].ToString();

                    String vCashAcc = DrAccConfig["CASH_ACCOUNT"].ToString();


                    using (OleDbConnection connection = new OleDbConnection(Con))
                    {
                        connection.Close();
                        connection.Open();


                        DataRow DrStartingRow = DtConfig.Select("COLUMN_ID=15").Single();
                        Int16 RowIndex = Convert.ToInt16(DrStartingRow["ROW_INDEX"].ToString());
                        OleDbCommand commandHDR = new OleDbCommand("select * from [Sheet0$A" + RowIndex + ":M]", connection);
                        OleDbCommand commandDTL = new OleDbCommand("select * from [Sheet0$A" + RowIndex + ":M]", connection);


                        using (OleDbDataReader drHDR = commandHDR.ExecuteReader())
                        {


                            //Row index should equal to defined starting index in excel.

                            //DataRow DrStartingRow = DtConfig.Select("COLUMN_ID=15").Single();
                            //Int16 RowIndex = Convert.ToInt16(DrStartingRow["ROW_INDEX"].ToString());

                            if (RowIndex == 0)
                            {
                                RowIndex = 1;
                            }

                            //Int16 RowIndex = 1;
                            Boolean TransRowFound = false;


                            //================================DETAIL=========================================

                            using (OleDbDataReader dr = commandDTL.ExecuteReader())
                            {

                                //DataRow[] Drxx = DtConfigTrans.Select();
                                //DataTable dtxx = dr.HasRows;

                                while (dr.Read())
                                {

                                    String RowData = dr[0].ToString();
                                    DataRow DrConfig = null;

                                    if (DtConfig.Select("ROW_INDEX=" + RowIndex).Count() != 0)
                                    {

                                        if (RowIndex == 0)
                                        {
                                            RowIndex++;
                                            continue;
                                        }


                                        DrConfig = DtConfig.Select("ROW_INDEX=" + RowIndex + " AND [STATUS] = '-'").Single();

                                        //DrConfig = DtConfig.Select("ROW_INDEX=" + RowIndex).Single();


                                        Int16 ROW_INDEX = Convert.ToInt16(DrConfig["ROW_INDEX"].ToString());
                                        Int16 COLUMNS_INDEX = Convert.ToInt16(DrConfig["COLUMNS_INDEX"]);
                                        COLUMNS_INDEX = Convert.ToInt16(COLUMNS_INDEX - 1);
                                        String COLUMN_ORDER = Convert.ToString(DrConfig["COLUMN_ORDER"]);
                                        Int16 START_INDEX = Convert.ToInt16(DrConfig["START_INDEX"].ToString());
                                        //ISURU
                                        START_INDEX = Convert.ToInt16(START_INDEX - 1);

                                        Int16 END_INDEX = Convert.ToInt16(DrConfig["END_INDEX"].ToString());

                                        if (ROW_INDEX == 0)
                                        {
                                            RowIndex++;
                                            continue;
                                        }


                                        if (DrConfig != null)
                                        {

                                            if (TransRowFound == false)
                                            {
                                                if (DrConfig["COLUMN_ID"].ToString() == "1")//Account Number
                                                {

                                                    FormatID = TempFormatID;

                                                    AccountNo = dr[COLUMNS_INDEX].ToString();

                                                    if (START_INDEX != 0 && END_INDEX != 0)
                                                        AccountNo = AccountNo.Substring(START_INDEX, END_INDEX);
                                                }

                                                if (DrConfig["COLUMN_ID"].ToString() == "3")//OP Balance
                                                {
                                                    OpenningBalance = dr[COLUMNS_INDEX].ToString();

                                                    if (START_INDEX != 0 && END_INDEX != 0)
                                                        OpenningBalance = OpenningBalance.Substring(START_INDEX, END_INDEX);
                                                }
                                            }


                                            if (DrConfig["COLUMN_ID"].ToString() == "13")
                                            {
                                                TransRowFound = true;
                                                //RowIndex++;
                                                //continue;
                                                ROW_INDEX = 0;
                                                COLUMNS_INDEX = 0;
                                                COLUMN_ORDER = "0";
                                                START_INDEX = 0;
                                                END_INDEX = 0;

                                                //RowIndex = -1;

                                            }


                                            if (TransRowFound == true && AccountNo.Length > 0)
                                            {
                                                if (TransRowFound)
                                                {

                                                    DataRow[] DrTemp = DtConfigTrans.Select("ROW_INDEX=" + -1);

                                                    DataTable dt = DrTemp.CopyToDataTable();


                                                    Int16 i = 0;
                                                    foreach (DataRow row in dt.Rows)
                                                    {

                                                        Int16 StartIndex = Convert.ToInt16(row[7].ToString());

                                                        StartIndex = Convert.ToInt16(StartIndex - 1);

                                                        Int16 EndIndex = Convert.ToInt16(row[8].ToString());


                                                        Int16 TempIndex = Convert.ToInt16(row[5].ToString());
                                                        TempIndex = Convert.ToInt16(TempIndex - 1);


                                                        if (row[2].ToString() == "5")//Date
                                                        {
                                                            TransDate = dr[TempIndex].ToString();
                                                            if (TransDate != "")
                                                            {
                                                                TransDate = Convert.ToDateTime(TransDate).ToShortDateString();
                                                            }
                                                            i++;
                                                            continue;
                                                        }
                                                        if (row[2].ToString() == "6")//Description
                                                        {
                                                            Description = dr[TempIndex].ToString();

                                                            //Do this in Upload detail SP

                                                            if (vProduct != "2" || vProduct != "4")
                                                            {
                                                                if ((StartIndex + EndIndex) <= Description.Length && (StartIndex != -1))
                                                                {
                                                                    PolicyNo = Description.Substring(StartIndex, EndIndex);
                                                                }
                                                                else
                                                                {
                                                                    PolicyNo = Description;
                                                                }
                                                            }



                                                            i++;
                                                            continue;
                                                        }
                                                        if (row[2].ToString() == "7")//Bank Ref
                                                        {
                                                            BankRef = dr[TempIndex].ToString();
                                                            i++;
                                                            continue;
                                                        }
                                                        if (row[2].ToString() == "8")//Bank branch
                                                        {
                                                            BankBranch = dr[TempIndex].ToString();
                                                            i++;
                                                            continue;
                                                        }
                                                        if (row[2].ToString() == "9")//Cheque No
                                                        {
                                                            ChequeNo = dr[TempIndex].ToString();

                                                            //16-09-2017
                                                            if (StartIndex > 0)
                                                            {
                                                                if ((StartIndex + EndIndex) <= ChequeNo.Length)
                                                                {
                                                                    ChequeNo = ChequeNo.Substring(StartIndex, EndIndex);
                                                                }
                                                            }

                                                            i++;
                                                            continue;
                                                        }
                                                        //if (row[2].ToString() == "9")//debit
                                                        //{
                                                        //    Debit = dr[TempIndex].ToString();
                                                        //    i++;
                                                        //    continue;
                                                        //}
                                                        if (row[2].ToString() == "10")//debit
                                                        {
                                                            Debit = dr[TempIndex].ToString();
                                                            i++;
                                                            continue;
                                                        }
                                                        if (row[2].ToString() == "11")//credit
                                                        {
                                                            Credit = dr[TempIndex].ToString();
                                                            i++;



                                                            if (Credit != "")
                                                            {
                                                                if (Convert.ToDouble(Credit) < 0)
                                                                {
                                                                    Debit = Convert.ToString(-(Convert.ToDouble(Credit)));
                                                                    Credit = "";
                                                                }
                                                                else
                                                                {
                                                                    Debit = "";
                                                                }
                                                            }

                                                            //Origin 14-06-2016
                                                            ////--------when figures comming as [ + or - ]-------
                                                            //if (Credit != "")
                                                            //{
                                                            //    if (Convert.ToDouble(Credit) < 0)
                                                            //    {
                                                            //        Double TCredit = Math.Abs(Convert.ToDouble(Credit));
                                                            //        Debit = Convert.ToString(TCredit);
                                                            //        Credit = "";
                                                            //    }
                                                            //}
                                                            ////-------------------------------------------------



                                                            continue;
                                                        }



                                                    }

                                                    if (TransDate == "")
                                                    {
                                                        continue;
                                                    }

                                                    AddNewRow(AccountNo, OpenningBalance, ClosingBalance, SerialNo, TransDate, Description, PolicyNo, Debit, Credit, Balance, BankRef, BankBranch, ChequeNo, FormatID, vCashAcc);

                                                    PolicyNo = "";
                                                }

                                                continue;//07/12/2015
                                            }

                                        }

                                    }
                                    else
                                    {
                                        RowIndex++;
                                        continue;
                                    }

                                    RowIndex++;

                                }
                            }


                        }
                    }

                    //===============================================================================================

                    //LoadUploadDataToGrid();
                }


                return true;

            }
            catch (Exception ex)
            {
                //CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.Message.ToString();
                throw ex;
            }
        }

        private DataTable GetConfig(String AccNo)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                OracleCommand cmd = new OracleCommand();
                conn.Open();
                cmd.Connection = conn;

                //cmd.CommandText = "select a.acc_no, t.format_id,t.column_id,t.description,t.row_index,t.columns_index,t.column_order,t.start_index , " +
                //                  "t.end_index,t.Status from fas_ibt_upload_formats t inner join fas_ibt_accformats a on t.format_id = a.format_id where a.acc_no ='" + AccNo.Trim() + "' and t.Inactive = 0 order by t.row_index";


                cmd.CommandText = " select a.acc_no, t.format_id,t.column_id,t.description,t.row_index,t.columns_index,t.column_order, " +
                                  " t.start_index ,t.end_index,t.Status from fas_ibt_upload_formats t " +
                                  " inner join fas_ibt_accformats a on t.format_id = a.format_id " +
                                  " inner join fas_ibt_formats f on t.format_id = f.id " +
                                  " inner join fas_ibt_accounts acc on a.acc_no= acc.acc_no " +
                                  " where a.acc_no ='" + AccNo.Trim() + "'  and f.inactive = 0 and acc.inactive = 0 and a.EFFECTIVE_END_DATE is null" +
                                  " order by t.row_index";

                OracleDataReader myOleDbDataReader = cmd.ExecuteReader();

                OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                dt.Load(dr);
                conn.Close();

                myOleDbDataReader.Close();

                return dt;

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
                return null;
            }
        }

        private DataTable GetAccountConfig()
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                OracleCommand cmd = new OracleCommand();
                conn.Open();
                cmd.Connection = conn;

                cmd.CommandText = "select t.ACC_NO,t.product,t.cash_account from fas_ibt_accounts t ";

                OracleDataReader myOleDbDataReader = cmd.ExecuteReader();

                OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                dt.Load(dr);
                conn.Close();

                myOleDbDataReader.Close();

                return dt;

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
                return null;
            }
        }

        private void LoadUploadDataToGrid()
        {
            try
            {
                if (dtUpload == null)
                {
                    return;
                }

                string[] selectedColumns = new[] { "AccountNo", "TransDate", "Description", "BankRef", "Debit", "Credit" };
                DataTable dt = new DataView(dtUpload).ToTable(false, selectedColumns);
                GrdUpload.DataSource = dt;
                GrdUpload.DataBind();

                Session["dtUpload"] = dtUpload;

                Session["dtUploadAll"] = dtUpload;

                if (dtUpload.Rows.Count < 2)
                {
                    return;
                }

                //Distinct Account Numbers
                DataView view = new DataView(dtUpload);
                DataTable distinctValues = view.ToTable(true, "AccountNo");

                DataRow[] DrTemp = distinctValues.Select("AccountNo<>'--'");

                distinctValues = DrTemp.CopyToDataTable();


                DDLAccounts.DataTextField = "AccountNo";
                DDLAccounts.DataValueField = "AccountNo";
                DDLAccounts.DataSource = distinctValues;
                DDLAccounts.DataBind();

                DDLAccounts.Items.Insert(0, new ListItem("---All---", "0"));


                //foreach (GridViewRow row in GrdUpload.Rows)
                //{
                //    row.Cells[6].Attributes.CssStyle["text-align"] = "right";
                //}


            }
            catch (Exception ex)
            {
                //CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
                throw ex;
            }
        }

        private string GetPath(String FilePath)
        {
            try
            {
                String Date = DateTime.Now.ToShortDateString().Replace("/", "-");

                string directoryPath = Server.MapPath("~/IBTUploadFiles/" + Date + "/");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }


                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

                if (fileName == "")
                {
                    return "";
                }
                //string fileName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);

                FileUpload1.PostedFile.SaveAs(Server.MapPath("~/IBTUploadFiles/" + Date + "/") + fileName);

                //Reading All text  
                string FilePathName = Server.MapPath("~/IBTUploadFiles/" + Date + "/") + fileName;

                string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + FilePathName + "';" + @"Extended Properties='Excel 8.0;HDR=NO;'";

                return con;
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
                return "";
            }

        }

        private void AddNewRow(String AccountNo, String OpenningBalance, String ClosingBalance, String SerialNo, String TransDate, String Description, String PolicyNo, String Debit, String Credit, String Balance, String BankRef, String BankBranch, String ChequeNo, String FormatID, String vCashAcc)
        {
            try
            {
                if (dtUpload == null)
                {
                    return;
                }

                DataRow Row = dtUpload.NewRow();
                Row["AccountNo"] = AccountNo;
                Row["OpenningBalance"] = OpenningBalance;
                Row["ClosingBalance"] = ClosingBalance;
                Row["SerialNo"] = SerialNo;
                Row["TransDate"] = TransDate;
                Row["Description"] = Description.Replace("'", "");
                Row["PolicyNo"] = PolicyNo;
                Row["Debit"] = Debit;
                Row["Credit"] = Credit;
                Row["Balance"] = Balance;
                Row["OpenningBalance"] = OpenningBalance;
                Row["ClosingBalance"] = ClosingBalance;
                Row["BankRef"] = BankRef;
                Row["BankBranch"] = BankBranch;
                Row["ChequeNo"] = ChequeNo;
                Row["Product"] = Product;
                Row["CashAccNo"] = vCashAcc;
                Row["FormatID"] = FormatID;

                dtUpload.Rows.Add(Row);



                TotalRowUploaded++;



                Session["DtUpload"] = dtUpload;
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));

                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void cmdShowData_Click(object sender, EventArgs e)
        {
            try
            {
                cmdUpload.Enabled = true;

                page_result.Visible = false;
                lblResult.Text = "";

                if (!(FileUpload1.HasFiles))
                {
                    return;
                }


                dtUpload.Clear();
                dtUpload.Columns.Add("AccountNo");
                dtUpload.Columns.Add("SerialNo");
                dtUpload.Columns.Add("TransID");
                dtUpload.Columns.Add("TransDate");
                dtUpload.Columns.Add("CheckNo");
                dtUpload.Columns.Add("Description");
                dtUpload.Columns.Add("PolicyNo");
                dtUpload.Columns.Add("Debit");
                dtUpload.Columns.Add("Credit");
                dtUpload.Columns.Add("Balance");
                dtUpload.Columns.Add("OpenningBalance");
                dtUpload.Columns.Add("ClosingBalance");
                dtUpload.Columns.Add("BankRef");
                dtUpload.Columns.Add("BankBranch");
                dtUpload.Columns.Add("ChequeNo");
                dtUpload.Columns.Add("Product");
                dtUpload.Columns.Add("CashAccNo");
                dtUpload.Columns.Add("FormatID");


                SetPath(dtUpload);

                if (dtUpload == null)
                {
                    return;
                }

                DataRow[] DrTemp = dtUpload.Select("AccountNo='--'");

                if (DrTemp.Length > 0)
                {
                    DataTable DtTemp = DrTemp.CopyToDataTable();
                    TotalRowUploaded = TotalRowUploaded - DtTemp.Rows.Count;
                }


                if (dtUpload.Rows.Count >= 2)
                {
                    if (TotalAccountsSelected == TotalAccountsUploaded)
                    {
                        call_error_msg(true);
                        lblResult.Text = "Viewed Summery..         " + "[ACCOUNTS SELECTED = " + TotalAccountsSelected + "]     [ACCOUNTS UPLOADED = " + TotalAccountsUploaded + "]     [TOTAL TRANSACTIONS = " + TotalRowUploaded + "]";

                        cmdUpload.Enabled = true;
                    }
                    else
                    {
                        if (lblResult.Text != "Please check file contain multiple dates")
                        {
                            call_error_msg(false);
                            lblResult.Text = "Viewed Summery..         " + "[ACCOUNTS SELECTED = " + TotalAccountsSelected + "]     [ACCOUNTS UPLOADED = " + TotalAccountsUploaded + "]     [TOTAL TRANSACTIONS = " + TotalRowUploaded + "]";

                            cmdUpload.Enabled = false;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.Message.ToString();
            }
        }

        protected void GrdUpload_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdUpload.EditIndex = e.NewEditIndex;

            string[] selectedColumns = new[] { "AccountNo", "SerialNo", "TransDate", "Description", "PolicyNo", "BankRef", "Debit", "Credit" };

            DataTable dt = new DataView((DataTable)Session["DtUpload"]).ToTable(false, selectedColumns);
            //GrdUpload.EditIndex = 3;
            GrdUpload.DataSource = dt;
            GrdUpload.DataBind();
        }

        protected void GrdUpload_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void GrdUpload_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GrdUpload.EditIndex = -1;

                string[] selectedColumns = new[] { "AccountNo", "SerialNo", "TransDate", "Description", "PolicyNo", "BankRef", "Debit", "Credit" };

                DataTable dt = new DataView((DataTable)Session["DtUpload"]).ToTable(false, selectedColumns);
                //GrdUpload.EditIndex = 3;
                GrdUpload.DataSource = dt;
                GrdUpload.DataBind();
            }
            catch (Exception ex)
            {
                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        private Boolean OpenningBalnceValidate(Double OP_BAL, String ACC_NO)
        {
            try
            {
                Boolean Correct = false;

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                //cmd.CommandText = "select t.clossing_bal from fas_ibt_uploaded_hdr t where t.account_no=" + ACC_NO + " and rownum = 1 order by t.transaction_date desc"; //Original Code

                cmd.CommandText = "select t.clossing_bal from fas_ibt_uploaded_hdr t where t.account_no=" + ACC_NO + " and t.createddate = (select max(u.createddate) from fas_ibt_uploaded_hdr u where u.account_no= " + ACC_NO + ") and rownum = 1 order by t.transaction_date,t.createddate desc";

                OracleDataReader odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    Double clossing_bal = 0;
                    clossing_bal = Convert.ToDouble(odr["clossing_bal"].ToString());
                    if (clossing_bal == OP_BAL)
                    {
                        Correct = true;
                    }
                    else
                    {
                        Correct = false;
                    }
                }

                if (!(odr.HasRows))
                {
                    Correct = true;
                }


                return Correct;
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));

                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
                return false;
            }

        }

        private void UploadLog(String ACC_NO, String Status, String Desc, Boolean Insert)
        {
            try
            {
                //Transaction Upload LOG
                //CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
                //String UserName = clsCom.getCurentUser();

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Close();
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_UPLOAD_STATUS";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("vAccNo", OracleType.VarChar).Value = ACC_NO;
                cmd.Parameters.Add("vDate", OracleType.DateTime).Value = DateTime.Now.ToShortDateString();
                cmd.Parameters.Add("vStatus", OracleType.VarChar).Value = Status;
                cmd.Parameters.Add("vDesc", OracleType.VarChar).Value = Desc;
                cmd.Parameters.Add("vCreatedBy", OracleType.VarChar).Value = usrName;//UserName;
                cmd.Parameters.Add("vCreatedDate", OracleType.DateTime).Value = DateTime.Now;

                if (Insert == true)
                    cmd.Parameters.Add("vInsert", OracleType.Number).Value = "1";
                else
                    cmd.Parameters.Add("vInsert", OracleType.Number).Value = "0";



                cmd.ExecuteNonQuery();
                //----------------------
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cmdUpload_Click(object sender, System.EventArgs e)
        {
            try
            {
                page_result.Visible = false;
                lblResult.Text = "";

                if (Session["dtUpload"] == null)
                {
                    CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "Error - No Records for upload....", Server.MapPath("~/IBTLogFiles/Log.txt"));
                    return;
                }



                //----Log all the selected accounts numbers-----
                UploadLog("--", "--", "Uploading Started", true);
                DataView view = new DataView((DataTable)Session["dtUpload"]);
                DataTable distinctValues = view.ToTable(true, "AccountNo", "TransDate");
                foreach (DataRow row in distinctValues.Rows)
                {
                    if (row["AccountNo"].ToString() == "--")
                    {
                        continue;
                    }

                    UploadLog(row["AccountNo"].ToString(), "", "", true);
                }
                //----------------------------------------------

                if (GrdUpload.Rows.Count == 0)
                {
                    return;
                }

                UploadLog("--", "--", "Uploading End", true);

                foreach (DataRow row in distinctValues.Rows)
                {
                    if (row["AccountNo"].ToString() == "--") continue;
                    DataToOracle(row["AccountNo"].ToString().Trim(), row["TransDate"].ToString().Trim());
                }




                //--------------------------------

                CommonCLS.CommonFunctions.IBT_Bifurcate(Session["IBT_Company"].ToString());//Check IBT or NonIBT and update accordingly

                CommonCLS.CommonFunctions.IBT_DEPT_DECOMPOSE(Session["IBT_Company"].ToString());//Decompose MCR / MRP / NON MOTOR / MOTOR

                CommonCLS.CommonFunctions.IBT_Matching(Session["IBT_Company"].ToString());//Check Matching status Exact / Possible No / Unmatch


                //PolicyNoUpdate();//Update policy no using description

                //--------------------------------

            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
                lblResult.Text = ex.Message.ToString();
            }
        }

        //private void PolicyNoUpdate()
        //{
        //    try
        //    {
        //        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        //        conn.Open();
        //        OracleCommand cmd = conn.CreateCommand();
        //        cmd.CommandText = "SP_FAS_IBT_POLNO_FILTER";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
        //        lblResult.Text = ex.InnerException.ToString();
        //        throw ex;
        //    }
        //}

        private void DataToOracle(String AccNo, String TransDate)
        {
            //CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
            //String UserName = "Test-User";// clsCom.getCurentUser();

            Double OP_BAL = 0; String ACC_NO = ""; DateTime T_DATE = DateTime.Now;

            DataTable Dt = (DataTable)Session["dtUpload"];

            using (OracleConnection OraCon = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString()))
            {
                Boolean OP_BAL_SUCCESS = false;

                OraCon.Open();
                OracleCommand command = OraCon.CreateCommand();
                OracleTransaction transaction;

                // Start a local transaction
                transaction = OraCon.BeginTransaction(System.Data.IsolationLevel.Serializable);
                // Assign transaction object for a pending local transaction
                command.Transaction = transaction;



                try
                {

                    Double RunningTotal = 0;
                    Double DebitTotal = 0;
                    Double CreditTotal = 0;

                    String TempACC = "";


                    //------Transaction Date Validation-----------------------------------------------------------------
                    DataRow[] DrTemp11 = Dt.Select("AccountNo='" + AccNo + "'");
                    DataTable Dt11 = DrTemp11.CopyToDataTable();
                    DataView view = new DataView(Dt11);
                    DataTable distinctValues = view.ToTable(true, "AccountNo", "TransDate");
                    if (distinctValues.Rows.Count > 1)
                    {
                        //Error
                        call_error_msg(false);
                        lblResult.Text = "Transaction Date Error. - " + AccNo;

                        UploadLog(AccNo, "Error", "Transaction Date Error.. ", false);
                        CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "Transaction Date Error - " + AccNo, Server.MapPath("~/IBTLogFiles/Log.txt"));
                        return;
                    }
                    //===================================================================================================




                    DataRow[] DrTemp1 = Dt.Select("AccountNo='" + AccNo + "' AND TransDate='" + TransDate + "'");
                    Dt = DrTemp1.CopyToDataTable();
                    //Account Detail Separate Row
                    DataRow Row = Dt.NewRow();
                    Row["AccountNo"] = "--";
                    Row["OpenningBalance"] = 0;
                    Row["ClosingBalance"] = 0;
                    Row["SerialNo"] = "";
                    Row["TransDate"] = "";
                    Row["Description"] = "";
                    Row["PolicyNo"] = "";
                    Row["Debit"] = 0;
                    Row["Credit"] = 0;
                    Row["Balance"] = 0;
                    Row["OpenningBalance"] = 0;
                    Row["ClosingBalance"] = 0;
                    Row["BankRef"] = "";
                    Row["BankBranch"] = "";
                    Row["ChequeNo"] = "";
                    Row["FormatID"] = "";

                    Dt.Rows.Add(Row);



                    //-------Get Config---------
                    DataTable DtAccConfig = GetAccountConfig();
                    DataRow DrAccConfig = DtAccConfig.Select("ACC_NO = '" + AccNo + "'").Single();
                    //--------------------------



                    foreach (DataRow row in Dt.Rows)
                    {
                        OracleCommand objCmdD = OraCon.CreateCommand();

                        objCmdD.Transaction = transaction;

                        objCmdD.CommandText = "SP_FAS_IBT_UPLOAD";
                        objCmdD.CommandType = CommandType.StoredProcedure;


                        if (row["TransDate"].ToString() == "")
                        {
                            if (row["TransDate"].ToString() != "")
                            {
                                UploadLog(TempACC, "Error", "Transaction Date Is Empty", false);
                                break;
                            }

                        }

                        ACC_NO = row["AccountNo"].ToString();

                        if (row["OpenningBalance"].ToString() != "")
                        {
                            if (OP_BAL == 0)
                            {
                                OP_BAL = Convert.ToDouble(row["OpenningBalance"].ToString());
                            }
                        }


                        //Account No separate------
                        if (RunningTotal == 0)
                        {
                            RunningTotal = OP_BAL;
                        }

                        if (ACC_NO == "--")
                        {

                            UpdateHeader(usrName, OP_BAL, TempACC, T_DATE, RunningTotal);
                            //UploadLog(ACC_NO, "Success", "Sucessfully Uploaded", false);

                            RunningTotal = 0;
                            OP_BAL = 0;
                            continue;
                        }
                        //-------------------------


                        T_DATE = Convert.ToDateTime(row["TransDate"].ToString());


                        //Openning Balance check with last time clossing Balance
                        if (OP_BAL_SUCCESS == false)
                        {
                            if (OpenningBalnceValidate(OP_BAL, ACC_NO))
                            {
                                OP_BAL_SUCCESS = true;
                            }
                            else
                            {
                                //Error
                                call_error_msg(false);
                                lblResult.Text = "Openning Balance Error. - " + ACC_NO;

                                UploadLog(row["AccountNo"].ToString(), "Error", "Openning Balance Error.", false);
                                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "Openning Balance Error - " + ACC_NO, Server.MapPath("~/IBTLogFiles/Log.txt"));
                                return;
                                //break;
                            }
                        }



                        //-----------------------------------------------------
                        objCmdD.Parameters.Add("FORMAT_ID", OracleType.VarChar).Value = row["FormatID"].ToString();
                        objCmdD.Parameters.Add("ACCOUNT_NO", OracleType.VarChar).Value = row["AccountNo"].ToString();
                        objCmdD.Parameters.Add("TRANSACTION_DATE", OracleType.DateTime).Value = Convert.ToDateTime(row["TransDate"].ToString());
                        objCmdD.Parameters.Add("DESCRIPTION", OracleType.VarChar).Value = row[5].ToString().Trim();

                        objCmdD.Parameters.Add("POLICYNO", OracleType.VarChar).Value = row[6].ToString().Trim();

                        objCmdD.Parameters.Add("BANK_REF", OracleType.VarChar).Value = row["BankRef"].ToString();
                        objCmdD.Parameters.Add("BANK_BRANCH", OracleType.VarChar).Value = row["BankBranch"].ToString();
                        objCmdD.Parameters.Add("CHEQUE_NO", OracleType.VarChar).Value = row["ChequeNo"].ToString();



                        if (row["Debit"].ToString() != "")
                        {
                            objCmdD.Parameters.Add("DEBIT", OracleType.Double).Value = Convert.ToDouble(row["Debit"].ToString());
                            RunningTotal = RunningTotal - Convert.ToDouble(row["Debit"].ToString());
                            DebitTotal = DebitTotal + Convert.ToDouble(row["Debit"].ToString());
                        }
                        else
                            objCmdD.Parameters.Add("DEBIT", OracleType.Double).Value = 0;

                        if (row["Credit"].ToString() != "")
                        {
                            objCmdD.Parameters.Add("CREDIT", OracleType.Double).Value = Convert.ToDouble(row["Credit"].ToString());
                            RunningTotal = RunningTotal + Convert.ToDouble(row["Credit"].ToString());
                            CreditTotal = CreditTotal + Convert.ToDouble(row["Credit"].ToString());
                        }
                        else
                            objCmdD.Parameters.Add("CREDIT", OracleType.Double).Value = 0;



                        if (row["Balance"].ToString() != "")
                            objCmdD.Parameters.Add("BALANCE", OracleType.Double).Value = RunningTotal;//Running Total
                        else
                            objCmdD.Parameters.Add("BALANCE", OracleType.Double).Value = RunningTotal;

                        if (row["OpenningBalance"].ToString() != "")
                            objCmdD.Parameters.Add("OPENNING_BAL", OracleType.Double).Value = Convert.ToDouble(row["OpenningBalance"].ToString());
                        else
                            objCmdD.Parameters.Add("OPENNING_BAL", OracleType.Double).Value = 0;

                        if (row["ClosingBalance"].ToString() != "")
                            objCmdD.Parameters.Add("CLOSSING_BAL", OracleType.Double).Value = Convert.ToDouble(row["ClosingBalance"].ToString());
                        else
                            objCmdD.Parameters.Add("CLOSSING_BAL", OracleType.Double).Value = 0;


                        objCmdD.Parameters.Add("CASH_ACC_NO", OracleType.VarChar).Value = DrAccConfig["CASH_ACCOUNT"].ToString();


                        objCmdD.Parameters.Add("PRODUCT", OracleType.VarChar).Value = DrAccConfig["PRODUCT"].ToString();


                        objCmdD.Parameters.Add("CREATEDBY", OracleType.VarChar).Value = usrName;//UserName;

                        objCmdD.Transaction = transaction;

                        objCmdD.ExecuteNonQuery();
                        //string ReturnValueD = Convert.ToString(objCmdD.Parameters["P_RETVAL"].Value);



                        //-------------------------Update Header----------------------------
                        if (TempACC != ACC_NO)
                        {
                            //OP_BAL = Convert.ToDouble(row["OpenningBalance"].ToString());
                            //UpdateHeader(UserName, OP_BAL, ACC_NO, T_DATE, RunningTotal);

                            //OP_BAL = 0;

                            //RunningTotal = 0;

                            UploadLog(ACC_NO, "Success", "Sucessfully Uploaded", false);
                        }

                        TempACC = ACC_NO;
                        //------------------------------------------------------------------

                    }



                    OP_BAL_SUCCESS = false;

                    UploadingStatusViewLog(System.DateTime.Now);

                    transaction.Commit();



                    //Reset
                    DDLAccounts.SelectedIndex = -1;
                    GrdUpload.DataSource = null;
                    GrdUpload.DataBind();


                    this.SetFocus(grdUploadResult);


                }
                catch (Exception ex)
                {
                    UploadLog(AccNo, "Error", ex.Message.ToString(), false);
                    transaction.Rollback();
                    throw;
                }

            }
            call_error_msg(true);
            lblResult.Text = "Successfully Uploaded..";

            //01-12-2017
            //CommonCLS.IBTEmails.EmailAlertCommon(5, null,"");
            CommonCLS.IBTEmails.EmailAlertCommon(5, null, AccNo);

        }

        private void UploadingStatusViewLog(DateTime search_date)
        {
            //DateTime current_date = search_date;//System.DateTime.Now;
            //Uploading Status---------------------------------------------------------------------------------------
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            //cmd.CommandText = "select t.account_no,t.trans_date,t.status,t.description,t.createdby,t.createddate " +
            //                  " from fas_ibt_upload_status t order by t.ID";

            cmd.CommandText = "select t.account_no,t.trans_date,t.status,t.description,t.createdby,t.createddate from fas_ibt_upload_status t WHERE (trunc(t.createddate)) = to_date('" + search_date.ToShortDateString() + "', 'DD/MM/RRRR')  order by t.ID";

            OracleDataReader odr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            //while (odr.Read())
            //{
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            //    //grdUploadResult.DataSource = dt;
            //    //grdUploadResult.DataBind();
            //}
            if (dt.Rows.Count > 0)
            {
                grdUploadResult.DataSource = dt;
                grdUploadResult.DataBind();
            }
            else
            {
                grdUploadResult.DataSource = null;
                grdUploadResult.DataBind();
            }
            conn.Close();
            //-------------------------------------------------------------------------------------------------------
        }

        private static void UpdateHeader(String UserName, Double OP_BAL, String ACC_NO, DateTime T_DATE, Double RunningTotal)
        {
            try
            {
                //Transaction Upload LOG
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_UPLOAD_HDR";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("TRANSACTION_DATE", OracleType.DateTime).Value = T_DATE;
                cmd.Parameters.Add("ACCOUNT_NO", OracleType.VarChar).Value = ACC_NO;
                cmd.Parameters.Add("OPENNING_BAL", OracleType.Double).Value = OP_BAL;
                cmd.Parameters.Add("CLOSSING_BAL", OracleType.Double).Value = RunningTotal;
                cmd.Parameters.Add("CREATEDBY", OracleType.VarChar).Value = UserName;
                cmd.Parameters.Add("CREATEDDATE", OracleType.DateTime).Value = DateTime.Now;

                cmd.ExecuteNonQuery();
                //----------------------
            }
            catch (Exception ex)
            {
                //CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                //lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void GrdUpload_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[3].Text = "TOTAL";

                //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                //e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            }

            //e.Row.ToolTip = e.Row.Cells[1].Text + e.Row.Cells[2].Text + e.Row.Cells[3].Text;

            //e.Row.Cells[6].Attributes.CssStyle["text-align"] = "right";

        }

        protected void cmdClearAll_Click(object sender, EventArgs e)
        {
            try
            {

                GrdUpload.DataSource = null;
                GrdUpload.DataBind();

                DDLAccounts.Items.Clear();
                DDLAccounts.SelectedIndex = -1;

                lblResult.Text = "";
                if (page_result.Visible == true)
                {
                    page_result.Visible = false;
                }
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));

                call_error_msg(false);
                lblResult.Text = ex.InnerException.ToString();
            }
        }

        protected void DDLAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] selectedColumns = new[] { "AccountNo", "TransDate", "Description", "PolicyNo", "BankRef", "Debit", "Credit" };
                DataTable dt = new DataView((DataTable)Session["DtUploadAll"]).ToTable(false, selectedColumns);

                if (DDLAccounts.SelectedIndex != 0)
                {
                    String ACC = DDLAccounts.SelectedItem.ToString().Trim();
                    DataRow[] DrTemp1 = dt.Select("AccountNo = '" + ACC + "'");

                    if (DrTemp1.Count() == 0)
                    {
                        return;
                    }

                    dt = DrTemp1.CopyToDataTable();
                }

                Session["DtUpload"] = dt;

                GrdUpload.DataSource = dt;
                GrdUpload.DataBind();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), Server.MapPath("~/IBTLogFiles/Log.txt"));
                call_error_msg(false);
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

        protected void btnSearchLog_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Text != "")
            {
                CurrentDate = txtFromDate.Text;
                UploadingStatusViewLog(Convert.ToDateTime(CurrentDate));
            }
            else
            {
                return;
                //grdUploadResult.DataSource = null;
                //grdUploadResult.DataBind();
            }

            SetFocus(lbSetFocus);
        }

        protected void GrdUpload_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdUpload.PageIndex = e.NewPageIndex;

                string[] selectedColumns = new[] { "AccountNo", "TransDate", "Description", "PolicyNo", "BankRef", "Debit", "Credit" };
                DataTable dt = new DataView((DataTable)Session["DtUpload"]).ToTable(false, selectedColumns);

                GrdUpload.DataSource = dt;
                GrdUpload.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}