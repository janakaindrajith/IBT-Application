﻿using System;
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

namespace BranchMIS.IBT
{
    public partial class IBT_Records_Suppress : System.Web.UI.Page
    {
        OracleConnection myConnectionUse = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void get_unmatched_records_for_suppress()
        {
            myConnectionUse.Close();
            myConnectionUse.Open();

            OracleCommand cmd_getData = myConnectionUse.CreateCommand();

            cmd_getData.CommandText = "SELECT T.ID, T.SERIAL_NO, T.ACCOUNT_NO, T.TRANSACTION_DATE,T.POLICY_NO, T.BANK_REF, T.DEBIT, T.CREDIT, P.PRODUCT_DESCRIPTION, " +
                                        "D.DEPARTMENT_DESCRIPTION, M.MATCHING_DESCRIPTION, R.RECEIPTSTATUS_DESCRIPTION, T.CREATEDDATE, T.CREATEDBY " +
                                        "FROM FAS_IBT_UPLOADED_DTL T, FAS_IBT_PRODUCTS P, FAS_IBT_MATCHINGSTATUS M, FAS_IBT_RECEIPT_STATUS R,FAS_IBT_DEPARTMENTS D " +
                                        "WHERE T.MATCHING_STATUS = M.VALUE AND T.PRODUCT = P.VALUE AND T.RECEIPT_STATUS = R.VALUE AND T.DEPARTMENT = D.VALUE AND " +
                                        "T.EFFECTIVE_END_DATE IS NULL AND T.IBT_STATUS = 1 AND T.MATCHING_STATUS IN(0,1)AND T.BULK_RECEIPT_IND = 0 AND T.SUPPRESS_IND = 0 " +
                                        "AND T.TRANSACTION_DATE <= ADD_MONTHS(SYSDATE,-6) ORDER BY T.TRANSACTION_DATE";

            OracleDataReader odr_getData = cmd_getData.ExecuteReader();

            DataTable dt_getData = new DataTable();
            dt_getData.Load(odr_getData);

            grd_suppressData.DataSource = dt_getData;
            grd_suppressData.DataBind();

            odr_getData.Close();
            myConnectionUse.Close();
        }

        protected void grd_suppressData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].CssClass = "HiddenCol";
                //Set Headers
                e.Row.Cells[2].Text = "Serial";
                e.Row.Cells[3].Text = "Account No";
                e.Row.Cells[4].Text = "Transaction Date";
                e.Row.Cells[5].Text = "Policy No";
                e.Row.Cells[6].Text = "Bank Referance";
                e.Row.Cells[7].Text = "Debit";
                e.Row.Cells[8].Text = "Credit";
                e.Row.Cells[9].Text = "Product";
                e.Row.Cells[10].Text = "Data Category";
                e.Row.Cells[11].Text = "Matching Status";
                e.Row.Cells[12].Text = "Receipt Status";
                e.Row.Cells[13].Text = "Create Date";
                e.Row.Cells[14].Text = "Created By";

                //Set Column Width
                e.Row.Cells[2].Attributes["width"] = "115px";
                e.Row.Cells[3].Attributes["width"] = "115px";
                e.Row.Cells[4].Attributes["width"] = "155px";
                e.Row.Cells[5].Attributes["width"] = "155px";
                e.Row.Cells[6].Attributes["width"] = "105px";
                e.Row.Cells[7].Attributes["width"] = "100px";
                e.Row.Cells[8].Attributes["width"] = "100px";
                e.Row.Cells[9].Attributes["width"] = "145px";
                e.Row.Cells[10].Attributes["width"] = "145px";
                e.Row.Cells[11].Attributes["width"] = "145px";
                e.Row.Cells[12].Attributes["width"] = "145px";
                e.Row.Cells[13].Attributes["width"] = "145px";
                e.Row.Cells[14].Attributes["width"] = "145px";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].CssClass = "HiddenCol";
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        protected void btn_getData_Click1(object sender, EventArgs e)
        {
            get_unmatched_records_for_suppress();
        }

        protected void btn_suppressed_Click(object sender, EventArgs e)
        {
            ArrayList arr = new ArrayList();

            myConnectionUse.Close();
            myConnectionUse.Open();

            OracleTransaction transaction;
            transaction = myConnectionUse.BeginTransaction(System.Data.IsolationLevel.Serializable);

            if (txtUserComment.Text != "")
            {
                foreach (GridViewRow row in grd_suppressData.Rows)
                {
                    CheckBox chk = (row.Cells[0].FindControl("chkboxselect") as CheckBox);
                    OracleCommand cmd = myConnectionUse.CreateCommand();

                    cmd.Transaction = transaction;

                    if (chk != null && chk.Checked)
                    {
                        cmd.CommandText = "SP_FAS_IBT_SUPPRESS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("V_SERIAL_NO", OracleType.VarChar).Value = row.Cells[2].Text.ToString();
                        cmd.Parameters.Add("V_ACCOUNT_NO", OracleType.VarChar).Value = row.Cells[3].Text.ToString();
                        cmd.Parameters.Add("V_POLICY_NO", OracleType.VarChar).Value = row.Cells[5].Text.ToString();
                        cmd.Parameters.Add("V_USER_COMMENT", OracleType.VarChar).Value = txtUserComment.Text.ToString();
                        cmd.Parameters.Add("V_CREATED_USER", OracleType.VarChar).Value = Session["IBT_UserName"].ToString(); //"deshapriya.sooriya2";
                        cmd.Parameters.Add("V_EXICUTING_TYPE", OracleType.Number).Value = 1; //2 Value depends on the user role

                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                        arr.Add(row.Cells[2].Text.ToString() + " - " + row.Cells[3].Text.ToString() + " - " + row.Cells[5].Text.ToString() + " - " + Session["IBT_UserName"].ToString() + " - " + txtUserComment.Text.ToString());
                    
                    }
                }
                transaction.Commit();
                myConnectionUse.Close();

                CommonCLS.IBTEmails.EmailAlertCommon(12, arr, "SUPPRESS_REQUEST");

                get_unmatched_records_for_suppress();
            }
            else
            {
                return;//Error
            }
        }
    }
}