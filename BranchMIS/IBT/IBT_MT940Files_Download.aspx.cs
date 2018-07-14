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

namespace BranchMIS.IBT
{
    public partial class MT940_Files_Download : System.Web.UI.Page
    {
        private string usrName = "";
        public int usr_role = 0;

        protected void Page_Load(object sender, EventArgs e)//---------------------------------Page load events----------------------------------------------------//
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
                    getAccounts();
                    getDownloadHistory();

                }
            }
            else
            {
                Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        public void getAccounts()//------------------------------------------------------------Load data to dropdownlist (ddlAccounts)-----------------------------//
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select distinct(t.account_no) from fas_ibt_uploaded_dtl t";

            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();

            oda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                ddlAccounts.DataTextField = "account_no";
                ddlAccounts.DataValueField = "account_no";

                ddlAccounts.DataSource = dt;
                ddlAccounts.DataBind();

                ddlAccounts.Items.Insert(0, new ListItem("---Please Select---", "0"));
            }
            else
            {
                ddlAccounts.Items.Insert(0, new ListItem("---Please Select---", "0"));
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)//-------------------------Download button click event-----------------------------------------//
        {
            try
            {
                string SQLWhere = "";
                string accNo = "";
                double openningBal_usr = 0.00;
                string FileName = "";
                if (lblResult.Text != "")
                {
                    page_result.Visible = false;
                }

                //--------------------------------------------------------------------------------Date Range Search---------------------------------------------------//

                if ((txtFromDate.Text == "") && (txtToDate.Text == "") && (txtSerialFrom.Text == "") && (txtSerialTo.Text == "")) // && (ddlAccounts.SelectedIndex == 0))
                {
                    call_error_msg(false);
                    lblResult.Text = "Fields Can Not Be Empty...!";
                    return;
                }

                if (((txtFromDate.Text != "") && (txtToDate.Text == "")) || ((txtFromDate.Text == "") && (txtToDate.Text != "")))
                {
                    call_error_msg(false);
                    lblResult.Text = "Date Fields Can Not Be Empty...!";
                    return;
                }

                if ((txtFromDate.Text != "") && (txtToDate.Text != ""))
                {
                    DateTime to_d = Convert.ToDateTime(txtToDate.Text);
                    DateTime from_d = Convert.ToDateTime(txtFromDate.Text);

                    if (((from_d > to_d) || (to_d < from_d)) || ((from_d > System.DateTime.Now) || (to_d > System.DateTime.Now)))
                    {
                        call_error_msg(false);
                        lblResult.Text = "Date Range Error...!";
                        return;
                    }
                    else
                    {
                        SQLWhere = " (trunc(t.transaction_date)) BETWEEN to_date('" + from_d.ToShortDateString() + "', 'DD/MM/RRRR') AND to_date('" + to_d.ToShortDateString() + "', 'DD/MM/RRRR')"; //AND t.effective_end_date is null";
                    }
                }
                else
                {
                    SQLWhere = " 1 = 1 ";
                }

                //--------------------------------------------------------------------------------Serial Range Search-----------------------------------------------------------//

                if (((txtSerialFrom.Text != "") && (txtSerialTo.Text != "")) && ((txtFromDate.Text != "") && (txtToDate.Text != "")))
                {
                    string serial_from = txtSerialFrom.Text;
                    string serial_to = txtSerialTo.Text;

                    SQLWhere = SQLWhere + " AND t.serial_no between '" + serial_from + "' and '" + serial_to + "'";
                }
                else if (((txtSerialFrom.Text != "") && (txtSerialTo.Text == "")) || ((txtSerialFrom.Text == "") && (txtSerialTo.Text != "")))
                {
                    call_error_msg(false);
                    lblResult.Text = "Serial Fields Can Not Be Empty...!";
                    return;
                }
                else if ((txtSerialFrom.Text != "") && (txtSerialTo.Text != ""))
                {
                    string serial_from = txtSerialFrom.Text;
                    string serial_to = txtSerialTo.Text;

                    double serial_from_trimed = double.Parse(serial_from.Trim().Remove(11, 4));
                    double serial_to_trimed = double.Parse(serial_to.Trim().Remove(11, 4));

                    if(serial_from_trimed > serial_to_trimed)
                    {
                        call_error_msg(false);

                        lblResult.Text = "Serial From Value Is Greater than Serial To Value...!";
                        return;
                    }
                    

                    SQLWhere = SQLWhere + " AND t.serial_no between '" + serial_from + "' and '" + serial_to + "'";
                }

                else
                {
                    SQLWhere = SQLWhere + " AND 1 = 1 ";
                }

                //--------------------------------------------------------------------------------Account Number Validation-----------------------------------------------------------//

                if ((ddlAccounts.SelectedIndex != 0) && (txtOpenningBal.Text != ""))
                {
                    accNo = ddlAccounts.SelectedValue.ToString();
                    openningBal_usr = double.Parse(txtOpenningBal.Text.Trim().Replace(" ",""));
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "Please Select An Account Number And Try Again...!";
                    return;
                }

                //string qry = "select t.transaction_date, t.account_no, t.serial_no, t.openning_bal, t.credit, t.debit, t.policy_no, t.bank_ref, t.description, t.clossing_bal, t.balance from fas_ibt_uploaded_dtl t WHERE " + SQLWhere + " AND t.effective_end_date is null AND t.account_no = " + accNo; 

                string err = getData(accNo, SQLWhere, openningBal_usr);//calling get data method


                //--------------------------------------------------------------------------------Error Masseges-----------------------------------------------------------//

                if (err == "Success")
                {
                    call_error_msg(true);
                    lblResult.Text = "Process Completed Successfully...!";
                    //return;
                }
                else if (err == "No Data Records Found")
                {
                    call_error_msg(false);
                    lblResult.Text = "No Data Records Available...!";
                    return;
                }
                else if (err == "Openning Balance Error")
                {
                    call_error_msg(false);
                    lblResult.Text = "Openning Balance Not Matched...!";
                    return;
                }
                else if (err == "MT940 File Created Successfully")
                {
                    call_error_msg(true);
                    lblResult.Text = "MT940 File Created Successfully...!";
                    //return;

                    FileName = "MT940-" + accNo + "-Download";

                    CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, "----MT940 File Created Successfully---- [File Name : " + FileName + ".txt] , Active User: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                                        
                    bool x = fileDownload(FileName);//Calling MT940 Download Method 

                    if(x == true)
                    {
                        createDownloadHistory(FileName, SQLWhere);//Calling Method For Create Download History Log
                        call_error_msg(true);
                        lblResult.Text = "Process Completed Successfully...!";


                        string url = "~/IBT/IBT_MT940Files_Download.aspx";
                        getDownloadHistory();//History Grid Data Rebind
                        //Response.Write("<script type='text/javascript'>");
                        //Response.Write("window.location = '" + url + "'</script>");
                        //Response.Flush();

                        //Response.Redirect(url, false);
                    }
                    else
                    {
                        call_error_msg(false);
                        lblResult.Text = "File Download Error...!";
                    }

                }
                else if (err == "MT940 File Creation Fail")
                {
                    call_error_msg(false);
                    lblResult.Text = "MT940 File Creation Fail...!";
                    return;
                }
            }

           catch (System.Threading.ThreadAbortException)
            {
            // To Handle HTTP Exception "Cannot redirect after HTTP headers have been sent".
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/IBT/IBT_MT940Files_Download.aspx", false);
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name,ex.Message.ToString() + "----MT940 File Download Error---- Active User: "+ usrName , Server.MapPath("~/IBTLogFiles/Log.txt"));
                //throw;
            }


            //FileName = "MT940-" + accNo + "-Download";
            //bool x = fileDownload(FileName);

        }              

        public string getData(string accNo, string where, double usr_opening_bal)//------------Search data from db for selected account no-------------------------//
        {
            try
            {
                string error_type = "";
                int dataRecords_Count = 0;
                bool closing_bal_valid = false;
                string openningBal_C_D_value_toPrint = "";
                string clossingBal_C_D_value_toPrint = "";
                

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText =   "select t.id, t.transaction_date, t.account_no, t.serial_no, t.openning_bal, t.credit, t.debit, " +
                                    "t.policy_no, t.bank_ref, t.description, t.clossing_bal, t.balance, t.bank_branch, t.CHEQUE_NO, case when(t.ibt_status=1) then t.serial_no else t.bank_ref end as Serial_For_Non_IBT,t.ibt_status " +
                                    "from fas_ibt_uploaded_dtl t WHERE " + where + " AND t.effective_end_date is null AND t.account_no = " + accNo + " order by t.serial_no";


                //cmd.CommandText = "select t.id, t.transaction_date, t.account_no, t.serial_no, t.openning_bal, t.credit, t.debit, " +
                //                    "t.policy_no, t.bank_ref, t.description, t.clossing_bal, t.balance, t.bank_branch, t.CHEQUE_NO, case when(t.ibt_status=1) then t.serial_no else t.bank_ref end as Serial_For_Non_IBT " +
                //                    "from fas_ibt_uploaded_dtl t where t.id in (" +
                //                    "select min(id) from fas_ibt_uploaded_dtl t where " + where + " and t.account_no = " + accNo + " group by serial_no ) order by serial_no";

                double openningBal_tbl = 0.00;
                double clossingBal_tbl = 0.00;
                double credits_tbl = 0.00;
                double debits_tbl = 0.00;
                double running_bal = 0.00;

                bool openningBal_valid = false;

                OracleDataAdapter oda = new OracleDataAdapter(cmd);

                DataTable dt = new DataTable();
                oda.Fill(dt);


                object maxDate = dt.Compute("MAX(TRANSACTION_DATE)", null);



                if (dt.Rows.Count > 0)//openning balance and record count validation
                {
                    DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
                    clossingBal_tbl = double.Parse(lastRow["balance"].ToString());


                    //openningBal_tbl = Convert.ToDouble(dt.Rows[0]["balance"]);
                    running_bal = Convert.ToDouble(dt.Rows[0]["balance"]);

                    credits_tbl = Convert.ToDouble(dt.Rows[0]["credit"]);
                    debits_tbl = Convert.ToDouble(dt.Rows[0]["debit"]);

                    if ((credits_tbl != null)&&(credits_tbl != 0.0))
                    {
                        openningBal_tbl = running_bal - credits_tbl;
                    }
                    else if ((debits_tbl != null)&&(debits_tbl != 0.0))
                    {
                        openningBal_tbl = running_bal + debits_tbl;
                    }
                    

                    if (usr_opening_bal == openningBal_tbl)
                    {
                        openningBal_valid = true;
                    }
                    else
                    {
                        error_type = "Openning Balance Error";
                        return error_type;
                    }
                }

                else
                {
                    error_type = "No Data Records Found";
                    return error_type;
                }

                //-------------------------------------------------------------------Check MT940 File Exists Or Not-----------------------------------------------------------//

                if (File.Exists(Server.MapPath("~/IBTFilesDownload/MT940-" + ddlAccounts.SelectedItem.Text + "-Download.txt")))
                {
                    File.Delete(Server.MapPath("~/IBTFilesDownload/MT940-" + ddlAccounts.SelectedItem.Text + "-Download.txt"));
                    File.Create(Server.MapPath("~/IBTFilesDownload/MT940-" + ddlAccounts.SelectedItem.Text + "-Download.txt")).Dispose();
                }
                else
                {
                    File.Create(Server.MapPath("~/IBTFilesDownload/MT940-" + ddlAccounts.SelectedItem.Text + "-Download.txt")).Dispose();
                }

                //-----------------------------------------------------------------------------------------------------------------------------------------------------------//

                if (openningBal_valid == true)
                {
                    OracleDataReader odr = cmd.ExecuteReader();

                    while (odr.Read())
                    {
                        dataRecords_Count = dataRecords_Count + 1;
                        //DateTime testDate = Convert.ToDateTime(getDate.ToString("dd/mm/yy"));

                        DateTime getDate = Convert.ToDateTime(odr["transaction_date"].ToString());

                        string accountNo_toPrint = odr["account_no"].ToString();
                        string serialNo_toPrint = odr["serial_no"].ToString();

                        //string  a =  Convert.ToDecimal(odr["credit"]).ToString("#,##0.00");
                        //inputValue = Math.Round(inputValue, 2);
                        //var cre = Math.Round(Convert.ToDouble(odr["credit"].ToString()), 2);

                        string openningBal = odr["openning_bal"].ToString();
                        string credit = Convert.ToDecimal(odr["credit"]).ToString("###0.00"); //odr["credit"].ToString();
                        string debit = Convert.ToDecimal(odr["debit"]).ToString("###0.00"); //odr["debit"].ToString();
                        string polNo = odr["policy_no"].ToString();
                        string bank_ref = odr["bank_ref"].ToString();
                        string description = odr["description"].ToString();
                        string clossing_bal = odr["clossing_bal"].ToString();
                        string bank_branch = odr["bank_branch"].ToString();
                        double balance = double.Parse(odr["balance"].ToString());
                        string cheque_no = odr["CHEQUE_NO"].ToString();
                        string serial_for_nonibt = odr["Serial_For_Non_IBT"].ToString();

                        string ibt_status = odr["ibt_status"].ToString();//****


                        string path = Server.MapPath("~/IBTFilesDownload/MT940-" + accountNo_toPrint + "-Download.txt");
                        //trans_Date = trans_Date.Replace("/", string.Empty);

                        //-----------------------------Date----------------------------------//

                        string t_date = getDate.Day.ToString();//config date

                        if (t_date.Length < 2)
                        {
                            t_date = "0" + t_date;
                        }
                        string t_month = getDate.Month.ToString();//config month

                        if (t_month.Length < 2)
                        {
                            t_month = "0" + t_month;
                        }

                        string t_year = getDate.Year.ToString();//config year
                        t_year = t_year.Remove(0, 2);

                        string trans_Date_toPrint = t_year + t_month + t_date;//parsing full date
                        string trans_monthAndDate_toPrint = t_month + t_date;//parsing month and date
                        //------------------------------------------------------------------//

                        //-----------------------Openning Balance---------------------------//

                        //if((!openningBal.Contains(".")) || (!openningBal.Contains(",")))//new
                        //{
                        //    openningBal = openningBal + ".00";//new
                        //}

                        openningBal = openningBal.Replace(".", ",");

                        string openningBal_toPrint = openningBal_tbl.ToString().Replace(".", ",");

                        //if(!openningBal_toPrint.Contains('.'))//new
                        //{
                        //    openningBal_toPrint = openningBal_toPrint + ",00"; //new
                        //}

                        if (openningBal_tbl > 0)
                        {
                            openningBal_C_D_value_toPrint = "C";
                        }
                        else
                        {
                            openningBal_C_D_value_toPrint = "D";
                        }

                        //-----------------------------------------------------------------//

                        //-----------------------Credit / Debit----------------------------//

                        if ((credit == "") || (credit == "0.00"))
                        {
                            credit = "0,00";
                        }
                        else
                        {
                            credit = credit.Replace(".", ",");
                        }

                        if ((debit == "") || (debit == "0.00"))
                        {
                            debit = "0,00";
                        }
                        else
                        {
                            debit = debit.Replace(".", ",");
                        }

                        if (credit == "0,00")
                        {
                            credit = "";
                        }
                        if (debit == "0,00")
                        {
                            debit = "";
                        }

                        //-----------------------------Customer Refference (Bank Branch)---------------------//

                        if (bank_branch == "")
                        {
                            //Need Discussion
                        }
                        else
                        {
                            //Ori Br010-Utility Payment : HNBALIFE1

                            //janaka - 12-09-2017
                            //////////bank_branch = bank_branch.Trim().Remove(0, 6);
                            //////////int testIndex = bank_branch.LastIndexOf("-") + 1;
                            //////////if (testIndex > 0)
                            //////////{
                            //////////    bank_branch = bank_branch.Substring(0, testIndex);
                            //////////}
                        }

                        //-----------------------------Clossing Balance Validation-----------------------------//
                        
                        string clossingBal_to_Print = clossingBal_tbl.ToString().Replace(".", ",");
                        
                        if(!clossingBal_to_Print.Contains(','))
                        {
                            clossingBal_to_Print = clossingBal_to_Print + ",00"; //new
                        }

                        //23-11-2017 - Commented By Janaka
                        //if (clossingBal_tbl == balance)
                        //{
                        //    closing_bal_valid = true;
                        //}


                        //from 16/11/2017 HNBA has decided to fix the clossing balance for all the IBT accounts so from now Openning & 
                        //Clossing balance same so below validation get the MAX Date of the selected date range and print the clossing 
                        //Balance just after the last record of the Last selected date.
                        if (clossingBal_tbl == balance && getDate.ToString() == maxDate.ToString())
                        {
                            closing_bal_valid = true;
                        }



                        if (clossingBal_tbl > 0)
                        {
                            clossingBal_C_D_value_toPrint = "C";
                        }
                        else
                        {
                            clossingBal_C_D_value_toPrint = "D";
                        }

                        //-------------------------------------------------------------------------------------//


                        //-------------------------------Cheque Number Validation------------------------------//

                        string chq_no_to_Print = "";

                        //OLD - 17-09-2017
                        //if (cheque_no != "")
                        //{
                        //    chq_no_to_Print = cheque_no;
                        //}


                        //NEW
                        if (ibt_status != "1")
                        {
                            chq_no_to_Print = cheque_no;
                        }

                        //-------------------------------------------------------------------------------------//


                        error_type = "Data Read Success";

                        error_type = WriteData(trans_Date_toPrint, accountNo_toPrint, serialNo_toPrint, openningBal, credit, debit, polNo, bank_ref, description, openningBal_toPrint, trans_monthAndDate_toPrint, clossingBal_tbl, bank_branch, path, dataRecords_Count, closing_bal_valid, openningBal_C_D_value_toPrint, clossingBal_C_D_value_toPrint, clossingBal_to_Print, chq_no_to_Print,serial_for_nonibt);//write data in to text file

                        closing_bal_valid = false;

                    }
                }

                return error_type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-------------------------------------------------------------------------------------Format data and create text file in mt940 format--------------------//

        //date :20: (yymmdd) (1st date)
        //account no :25: + account no
        //statement no :28c: + account no + "_" + serial no
        //openning bal :60F: + "C/D" + date + "LKR" + openning bal (. --> , )
        //statement line :61: + date + "month and date" + "C/D" (for credit/ debit) + credit/ debit value (. --> , ) + identification code + customer ref (bank branch) + bank ref
        
        
        //information to account owner at transaction level :86: + customer ref (bank branch) + bank ref + "@" + description + "/" Original
        //information to account owner at transaction level :86: + serial no + "@" + customer ref (bank branch) + "/" + bank ref + description + "/" New
        //information to account owner at transaction level :86: + serial no + "@" + customer ref (bank branch) + "/" + bank ref + "@"+ description + "/" New req
        
        //closing balance :62M: + "C" + date (yymmdd) (last date) + "LKR" + clossing bal (. --> , )


        public static string ValidateChequeNo(string print_cheque_no)
        {

            //if (print_cheque_no == "53190106/7135/089/31-08-2017")
            //{
            //    string aa = "";
            //}

            string TXT = "";

            if (print_cheque_no.Substring(0, 1) == "L" || print_cheque_no.Substring(0, 1) == "G")
            {
                TXT = print_cheque_no.Replace("/","");
                return TXT;
            }
            else
            {
                TXT = print_cheque_no;

                if (TXT.Length >= 8)
                {
                    //when cheque number visible end of the text
                    TXT = TXT.Substring(TXT.Length - 8, 8);

                    //when cheque number visible beginning of the text
                    if (!TXT.All(char.IsDigit))
                    {
                        TXT = print_cheque_no.Substring(0, 8);
                    }

                    if (TXT.All(char.IsDigit))
                    {
                        if (TXT.Length == 8)
                        {
                            TXT = TXT.Remove(0, 2);
                        }
                        if (TXT.Length == 7)
                        {
                            TXT = TXT.Remove(0, 1);
                        }

                    }
                    else
                    {
                        TXT = "000000";
                    }
                }
                else
                {
                    if (TXT.All(char.IsDigit))
                    {
                        TXT = String.Format("{0:000000}", Convert.ToDouble(TXT));
                    }
                    else
                    {
                        TXT = "000000";
                    }
                }

                if (TXT == "000000")
                {
                    return "";
                }
                else
                {
                    return TXT;
                }


            }
 
        }


        public string WriteData(string print_date, string print_acc_no, string print_serial, string print_openning_bal, string print_credits, string print_debits, string print_pol_no, string print_bank_ref, string print_description, string print_openningBal_tbl, string print_monthDate, double print_closingBal, string print_bank_branch, string Path, int print_dataRecords_Count, bool print_clossingBal_valid, string print_cd_value_openning, string print_cd_value_clossing, string print_clossingBal_to_Print, string print_cheque_no, string serial_for_nonibt)
        {
            try
            {
                string err_msg = "";

                string chq_no_half = "";
                string chq_no_full = "";

                string path = Path;

                TextWriter tw = new StreamWriter(path, true);

                if(!print_openningBal_tbl.Contains(','))
                {
                    print_openningBal_tbl = print_openningBal_tbl + ",00";//new
                }

                if(!print_clossingBal_to_Print.Contains(','))
                {
                    print_clossingBal_to_Print = print_clossingBal_to_Print + ",00"; //new
                }

                if (print_dataRecords_Count == 1)
                {
                    tw.WriteLine(":20:" + print_date + " ");//Full Date (yy-mm-dd)
                    tw.WriteLine(":25:" + print_acc_no + " ");//Account Number
                    tw.WriteLine(":28C:" + print_acc_no + "_" + print_serial + " ");//Account Number + Serial
                    tw.WriteLine(":60F:" + print_cd_value_openning + print_date + "LKR" + print_openningBal_tbl + " ");//openning Balance
                    //tw.WriteLine("");

                    string creditOrDebit = "";

                    if (print_credits == "")
                    {
                        creditOrDebit = "D";
                    }
                    if (print_debits == "")
                    {
                        creditOrDebit = "C";
                    }

                    string identification_code = "";

                    if (creditOrDebit == "C")
                    {
                        identification_code = "NMSC";
                    }
                    if (creditOrDebit == "D")
                    {
                        identification_code = "NMSD";
                    }

                    if (print_cheque_no == "")
                    {
                        //tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + print_bank_branch + print_bank_ref);Original
                        //tw.WriteLine(":86:" + print_bank_branch + print_bank_ref + "@" + print_description + "/");Original
                        //tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + print_serial + " ");
                        //tw.WriteLine(":86:" + print_serial + "@" + print_bank_branch + print_bank_ref + "@" +print_description + "/" + " ");

                        tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + serial_for_nonibt + " ");
                        tw.WriteLine(":86:" + print_serial + "@" + print_bank_branch + print_bank_ref + "@" + print_description + "/" + " ");
                    }
                    else
                    {                        
                        chq_no_full = print_cheque_no.Trim();
                        //chq_no_half = print_cheque_no.Trim().Remove(0,2); 

                        chq_no_half = ValidateChequeNo(print_cheque_no);

                        //tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + chq_no_half);Original
                        //tw.WriteLine(":86:" + print_bank_branch + print_bank_ref + "@" + print_description + "/" + chq_no_full);Original
                        //tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + print_serial + chq_no_half + " ");
                        //tw.WriteLine(":86:" + print_serial + "@" + print_bank_branch + print_bank_ref + "@" + print_description + "/" + chq_no_full + " ");

                        tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + serial_for_nonibt + chq_no_half + " ");
                        tw.WriteLine(":86:" + print_serial + "@" + print_bank_branch + print_bank_ref + "@" + print_description + "/" + chq_no_full + " ");
                    }

                    //tw.WriteLine("");
                    //tw.Close();
                    err_msg = "MT940 File Created Successfully";
                    //return err_msg;
                }

                if (print_dataRecords_Count > 1)
                {
                    string creditOrDebit = "";

                    if (print_credits == "")
                    {
                        creditOrDebit = "D";
                    }
                    if (print_debits == "")
                    {
                        creditOrDebit = "C";
                    }

                    string identification_code = "";

                    if (creditOrDebit == "C")
                    {
                        identification_code = "NMSC";
                    }
                    if (creditOrDebit == "D")
                    {
                        identification_code = "NMSD";
                    }

                    if (print_cheque_no == "")
                    {
                        //tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + print_bank_branch + print_bank_ref);//Original
                        //tw.WriteLine(":86:" + print_bank_branch + print_bank_ref + "@" + print_description + "/");//Original
                        tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + print_serial + " ");
                        tw.WriteLine(":86:" + print_serial + "@" + print_bank_branch + print_bank_ref + "@" +print_description + "/" + " ");
                    }
                    else
                    {
                        chq_no_full = print_cheque_no.Trim();
                        //chq_no_half = print_cheque_no.Trim().Remove(0, 2);


                        chq_no_half = ValidateChequeNo(print_cheque_no);

                        

                        //tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + chq_no_half);//Original
                        //tw.WriteLine(":86:" + print_bank_branch + print_bank_ref + "@" + print_description + "/" + chq_no_full);//Original

                        //Original - 17-09-2017
                        //tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + chq_no_half + print_serial + " ");
                        //tw.WriteLine(":86:" + print_serial + "@" + print_bank_branch + print_bank_ref + "@" + print_description + "/" + chq_no_full + " ");


                        tw.WriteLine(":61:" + print_date + print_monthDate + creditOrDebit + print_credits + print_debits + identification_code + chq_no_half + " ");
                        tw.WriteLine(":86:" + print_serial + "@" + print_bank_branch + print_bank_ref + "@" + print_description + "/" + chq_no_full + " ");

                    
                    }


                    //tw.WriteLine("");

                    //tw.Close();

                    err_msg = "MT940 File Created Successfully";

                    //return err_msg;
                }

                if (print_clossingBal_valid == true)
                {
                    tw.WriteLine(":62M:" + print_cd_value_clossing + print_date + "LKR" + print_clossingBal_to_Print + " ");
                }


                tw.Close();
                return err_msg;

            }
            catch (Exception ex)
            {                
                throw ex;
            }

        }

        private string ChequeNoFormat(string ChequeNo)
        {
            return "";
        }

        public bool fileDownload(string filename)//--------------------------------------------Download File-------------------------------------------------------//
        {
            try
            {
                string FileName = filename;
                string fileSavingName = filename + ".txt";
                bool valid = false;

                System.IO.FileStream fs = null;

                if (File.Exists(Server.MapPath("~/IBTFilesDownload/" + fileSavingName)))
                {
                    fs = System.IO.File.Open(Server.MapPath("~/IBTFilesDownload/" + FileName + ".txt"), System.IO.FileMode.Open);
                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    Response.AddHeader("Content-disposition", "attachment; filename=" + fileSavingName);
                    Response.ContentType = "text/plain";//"application/octet-stream";
                    Response.BinaryWrite(btFile);

                    Response.Flush();
                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                    //Response.End();

                    valid = true;
                }

                return valid;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            
        }

        public void createDownloadHistory(string fileName_to_log, string sqlwhere_to_log)//----Create Download History Log-----------------------------------------//
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SP_FAS_IBT_Download_History";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("vFileName", OracleType.VarChar).Value = fileName_to_log + ".txt";
            cmd.Parameters.Add("vSQLWhere", OracleType.VarChar).Value = sqlwhere_to_log.Trim();
            cmd.Parameters.Add("vActiveUser", OracleType.VarChar).Value = usrName;

            cmd.ExecuteNonQuery();
            conn.Close();

        }

        public void getDownloadHistory()//-----------------------------------------------------View Download History-----------------------------------------------//
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                cmd.CommandText = "select  t.id, t.filename, t.date_and_time, t.activeuser, t.sqlwhere from fas_ibt_mt940_downloadhistory t  where ((EXTRACT(MONTH from t.date_and_time) = EXTRACT(MONTH FROM SYSDATE)) AND (EXTRACT(YEAR from t.date_and_time) = EXTRACT(YEAR FROM SYSDATE))) order by t.id desc";
                OracleDataAdapter oda = new OracleDataAdapter(cmd);

                DataTable dt = new DataTable();
                oda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    grdDownloadHistory.DataSource = dt;
                    grdDownloadHistory.DataBind();
                }


                //odr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                CommonCLS.CommonFunctions.Logger(System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath), System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString() + "----MT 940 File Download History Table Load Error---- Searched By: " + usrName, Server.MapPath("~/IBTLogFiles/Log.txt"));
                throw;
            }

        }

        public void call_error_msg(bool x)//---------------------------------------------------System Messeges-----------------------------------------------------//
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

        private void ClearInputs(ControlCollection ctrls)//------------------------------------Clear Controls------------------------------------------------------//
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

    }
}