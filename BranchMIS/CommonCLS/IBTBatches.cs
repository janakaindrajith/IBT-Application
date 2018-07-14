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
//using Microsoft.SqlServer.Management.Common;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Smo.Agent;
using System.Web.Configuration;



namespace BranchMIS.CommonCLS
{
    public class IBTBatches
    {
        //static Server server;
        //static Job job;


        //--07/10/2017
        public static void LifeBatch_OLD()
        {
            try
            {

                //---------------------------------------------Batch Run------------------------------------------------
                OracleConnection conn1 = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

                OracleCommand ora_cmd = new OracleCommand("SP_FAS_IBT_BATCHES", conn1);
                ora_cmd.CommandType = CommandType.StoredProcedure;

                ora_cmd.Parameters.Add("v_user_name", OracleType.VarChar).Value = "UPLD";
                ora_cmd.Parameters.Add("v_request_id", OracleType.VarChar).Value = "01";
                ora_cmd.Parameters.Add("vfile", OracleType.VarChar).Value = "";
                ora_cmd.Parameters.Add("VBatchType", OracleType.VarChar).Value = "Life";

                conn1.Open();
                ora_cmd.ExecuteNonQuery();
                conn1.Close();
                //-------------------------------------------------------------------------------------------------------

                using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString()))
                {
                    conn.Open();
                    OracleCommand command = conn.CreateCommand();
                    OracleTransaction transaction;

                    transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    command.Transaction = transaction;


                    //----------------Feed back - Detail Update---------------
                    OracleCommand cmdBulk = new OracleCommand("sp_fas_ibt_rv_feedback_life", conn);
                    cmdBulk.CommandType = CommandType.StoredProcedure;
                    cmdBulk.Parameters.Add("StatusRet", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                    //conn.Open();

                    cmdBulk.Transaction = transaction;

                    cmdBulk.ExecuteNonQuery();
                    string Status = cmdBulk.Parameters["StatusRet"].Value.ToString();//Get Feedback updated serial no list
                    //conn.Close();
                    //--------------------------------------------------------

                    if (Status == "")
                    {
                        transaction.Commit();
                        conn.Close();
                        return;
                    }
                    //End of normal receipts

                    Status = Status.Remove(0, 1);
                    Status = Status.Replace(",", "','");
                    Status = "'" + Status + "'";


                    OracleCommand cmdGetBulkDTL = conn.CreateCommand();
                    cmdGetBulkDTL.CommandText = " select SUBSTR(bdtl.dtl_id,0,INSTR(bdtl.dtl_id,'.')-1),dtl.serial_no, bdtl.receipt_status,dtl.id  from fas_ibt_bulk_receipt_dtl bdtl " +
                                                " inner join fas_ibt_uploaded_dtl dtl on  SUBSTR(bdtl.dtl_id,0,INSTR(bdtl.dtl_id,'.')-1) = dtl.id and  bdtl.effective_end_date is null and " +
                                                " dtl.serial_no in (" + Status + ") ";



                    OracleDataAdapter oda = new OracleDataAdapter(cmdGetBulkDTL);
                    DataTable dtTemp = new DataTable();

                    cmdGetBulkDTL.Transaction = transaction;

                    oda.Fill(dtTemp);

                    string[] split = Status.Split(',');

                    split = split.Distinct().ToArray();


                    foreach (string item in split)
                    {
                        Console.WriteLine(item);
                        DataRow[] DrAll = dtTemp.Select("serial_no = " + item + "");
                        DataRow[] DrSuccess = dtTemp.Select("serial_no = " + item + " and (receipt_status = 5 or receipt_status = 9)");

                        if (DrAll.Length == DrSuccess.Length)
                        {
                            //Success
                            OracleCommand oraSuccess = new OracleCommand("SP_FAS_IBT_UPLD_DTL_INSERT", conn);
                            oraSuccess.CommandType = CommandType.StoredProcedure;

                            oraSuccess.Parameters.Add("vreceipt_status", OracleType.Int16).Value = 5;
                            oraSuccess.Parameters.Add("vmatching_status", OracleType.Int16).Value = 3;
                            oraSuccess.Parameters.Add("vBULK_RECEIPT_IND", OracleType.Int16).Value = 1;
                            oraSuccess.Parameters.Add("vCREATEDBY", OracleType.VarChar).Value = "TCS";
                            oraSuccess.Parameters.Add("vSerialNo", OracleType.VarChar).Value = item.ToString().Replace("'", "");

                            //conn.Open();

                            oraSuccess.Transaction = transaction;

                            oraSuccess.ExecuteNonQuery();
                            //conn.Close();

                            continue;
                        }
                        else
                        {
                            //Fail
                            OracleCommand oraFail = new OracleCommand("SP_FAS_IBT_UPLD_DTL_INSERT", conn);
                            oraFail.CommandType = CommandType.StoredProcedure;

                            oraFail.Parameters.Add("vreceipt_status", OracleType.Int16).Value = 7;
                            oraFail.Parameters.Add("vmatching_status", OracleType.Int16).Value = 1;
                            oraFail.Parameters.Add("vBULK_RECEIPT_IND", OracleType.Int16).Value = 1;
                            oraFail.Parameters.Add("vCREATEDBY", OracleType.VarChar).Value = "TCS";
                            oraFail.Parameters.Add("vSerialNo", OracleType.VarChar).Value = item.ToString().Replace("'", "");

                            //conn.Open();

                            oraFail.Transaction = transaction;

                            oraFail.ExecuteNonQuery();
                            //conn.Close();

                            continue;

                        }
                    }


                    transaction.Commit();
                    conn.Close();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        //--07/10/2017
        public static void GeneralBatch_OLD()
        {
            try
            {

                OracleConnection conn1 = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

                OracleCommand ora_cmd = new OracleCommand("SP_FAS_IBT_BATCHES", conn1);
                ora_cmd.CommandType = CommandType.StoredProcedure;

                ora_cmd.Parameters.Add("v_user_name", OracleType.VarChar).Value = "UPLD";
                ora_cmd.Parameters.Add("v_request_id", OracleType.VarChar).Value = "01";
                ora_cmd.Parameters.Add("vfile", OracleType.VarChar).Value = "";
                ora_cmd.Parameters.Add("VBatchType", OracleType.VarChar).Value = "General";

                conn1.Open();
                ora_cmd.ExecuteNonQuery();
                conn1.Close();


                using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString()))
                {
                    conn.Open();
                    OracleCommand command = conn.CreateCommand();
                    OracleTransaction transaction;

                    transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    command.Transaction = transaction;


                    //----------------Feed back - Detail Update---------------
                    OracleCommand cmdBulk = new OracleCommand("sp_fas_ibt_rv_feedback_life", conn);
                    cmdBulk.CommandType = CommandType.StoredProcedure;
                    cmdBulk.Parameters.Add("StatusRet", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                    //conn.Open();

                    cmdBulk.Transaction = transaction;

                    cmdBulk.ExecuteNonQuery();
                    string Status = cmdBulk.Parameters["StatusRet"].Value.ToString();//Get Feedback updated serial no list
                    //conn.Close();
                    //--------------------------------------------------------

                    if (Status == "")
                    {
                        transaction.Commit();
                        conn.Close();
                        return;
                    }
                    //End of normal receipts

                    Status = Status.Remove(0, 1);
                    Status = Status.Replace(",", "','");
                    Status = "'" + Status + "'";


                    OracleCommand cmdGetBulkDTL = conn.CreateCommand();
                    cmdGetBulkDTL.CommandText = " select SUBSTR(bdtl.dtl_id,0,INSTR(bdtl.dtl_id,'.')-1),dtl.serial_no, bdtl.receipt_status,dtl.id  from fas_ibt_bulk_receipt_dtl bdtl " +
                                                " inner join fas_ibt_uploaded_dtl dtl on  SUBSTR(bdtl.dtl_id,0,INSTR(bdtl.dtl_id,'.')-1) = dtl.id and  bdtl.effective_end_date is null and " +
                                                " dtl.serial_no in (" + Status + ") ";



                    OracleDataAdapter oda = new OracleDataAdapter(cmdGetBulkDTL);
                    DataTable dtTemp = new DataTable();

                    cmdGetBulkDTL.Transaction = transaction;

                    oda.Fill(dtTemp);

                    string[] split = Status.Split(',');

                    split = split.Distinct().ToArray();


                    foreach (string item in split)
                    {
                        Console.WriteLine(item);
                        DataRow[] DrAll = dtTemp.Select("serial_no = " + item + "");
                        DataRow[] DrSuccess = dtTemp.Select("serial_no = " + item + " and (receipt_status = 5 or receipt_status = 9)");

                        if (DrAll.Length == DrSuccess.Length)
                        {
                            //Success
                            OracleCommand oraSuccess = new OracleCommand("SP_FAS_IBT_UPLD_DTL_INSERT", conn);
                            oraSuccess.CommandType = CommandType.StoredProcedure;

                            oraSuccess.Parameters.Add("vreceipt_status", OracleType.Int16).Value = 5;
                            oraSuccess.Parameters.Add("vmatching_status", OracleType.Int16).Value = 3;
                            oraSuccess.Parameters.Add("vBULK_RECEIPT_IND", OracleType.Int16).Value = 1;
                            oraSuccess.Parameters.Add("vCREATEDBY", OracleType.VarChar).Value = "TCS";
                            oraSuccess.Parameters.Add("vSerialNo", OracleType.VarChar).Value = item.ToString().Replace("'", "");

                            //conn.Open();

                            oraSuccess.Transaction = transaction;

                            oraSuccess.ExecuteNonQuery();
                            //conn.Close();

                            continue;
                        }
                        else
                        {
                            //Fail
                            OracleCommand oraFail = new OracleCommand("SP_FAS_IBT_UPLD_DTL_INSERT", conn);
                            oraFail.CommandType = CommandType.StoredProcedure;

                            oraFail.Parameters.Add("vreceipt_status", OracleType.Int16).Value = 7;
                            oraFail.Parameters.Add("vmatching_status", OracleType.Int16).Value = 1;
                            oraFail.Parameters.Add("vBULK_RECEIPT_IND", OracleType.Int16).Value = 1;
                            oraFail.Parameters.Add("vCREATEDBY", OracleType.VarChar).Value = "TCS";
                            oraFail.Parameters.Add("vSerialNo", OracleType.VarChar).Value = item.ToString().Replace("'", "");

                            //conn.Open();

                            oraFail.Transaction = transaction;

                            oraFail.ExecuteNonQuery();
                            //conn.Close();

                            continue;

                        }
                    }


                    transaction.Commit();
                    conn.Close();
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        //--07/10/2017
        public static void NonTCSBatch_OLD()
        {
            try
            {

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

                OracleCommand ora_cmd = new OracleCommand("SP_FAS_IBT_BATCHES", conn);
                ora_cmd.CommandType = CommandType.StoredProcedure;

                ora_cmd.Parameters.Add("v_user_name", OracleType.VarChar).Value = "UPLD";
                ora_cmd.Parameters.Add("v_request_id", OracleType.VarChar).Value = "01";
                ora_cmd.Parameters.Add("vfile", OracleType.VarChar).Value = "";
                ora_cmd.Parameters.Add("VBatchType", OracleType.VarChar).Value = "NonTCS";

                conn.Open();
                ora_cmd.ExecuteNonQuery();
                conn.Close();



                //----------------Feed back - Detail Update---------------
                OracleCommand cmdBulk = new OracleCommand("sp_fas_ibt_rv_feedback_nontcs", conn);
                cmdBulk.CommandType = CommandType.StoredProcedure;
                cmdBulk.Parameters.Add("StatusRet", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                conn.Open();
                cmdBulk.ExecuteNonQuery();
                string Status = cmdBulk.Parameters["StatusRet"].Value.ToString();
                conn.Close();


                if (Status == "")
                {
                    //transaction.Commit();
                    //conn.Close();
                    return;
                }


                Status = Status.Remove(0, 1);
                Status = Status.Replace(",", "','");
                Status = "'" + Status + "'";

                OracleCommand cmdGetBulkDTL = conn.CreateCommand();
                cmdGetBulkDTL.CommandText = " select SUBSTR(bdtl.dtl_id,0,INSTR(bdtl.dtl_id,'.')-1),dtl.serial_no, bdtl.receipt_status,dtl.id  from fas_ibt_bulk_receipt_dtl bdtl " +
                                            " inner join fas_ibt_uploaded_dtl dtl on  SUBSTR(bdtl.dtl_id,0,INSTR(bdtl.dtl_id,'.')-1) = dtl.id and  bdtl.effective_end_date is null and " +
                                            " dtl.serial_no in (" + Status + ") ";



                OracleDataAdapter oda = new OracleDataAdapter(cmdGetBulkDTL);
                DataTable dtTemp = new DataTable();
                oda.Fill(dtTemp);

                //--------------------------------------------------------
                string[] split = Status.Split(',');

                split = split.Distinct().ToArray();

                foreach (string item in split)
                {
                    Console.WriteLine(item);
                    DataRow[] DrAll = dtTemp.Select("serial_no = " + item + "");
                    DataRow[] DrSuccess = dtTemp.Select("serial_no = " + item + " and (receipt_status = 5 or receipt_status = 9)");

                    if (DrAll.Length == DrSuccess.Length)
                    {
                        //Success
                        OracleCommand oraSuccess = new OracleCommand("SP_FAS_IBT_UPLD_DTL_INSERT", conn);
                        oraSuccess.CommandType = CommandType.StoredProcedure;

                        oraSuccess.Parameters.Add("vreceipt_status", OracleType.Int16).Value = 5;
                        oraSuccess.Parameters.Add("vmatching_status", OracleType.Int16).Value = 3;
                        oraSuccess.Parameters.Add("vBULK_RECEIPT_IND", OracleType.Int16).Value = 1;
                        oraSuccess.Parameters.Add("vCREATEDBY", OracleType.VarChar).Value = "TCS";
                        oraSuccess.Parameters.Add("vSerialNo", OracleType.VarChar).Value = item.ToString().Replace("'", "");

                        conn.Open();
                        oraSuccess.ExecuteNonQuery();
                        conn.Close();

                        continue;
                    }
                    else
                    {
                        //Fail
                        OracleCommand oraFail = new OracleCommand("SP_FAS_IBT_UPLD_DTL_INSERT", conn);
                        oraFail.CommandType = CommandType.StoredProcedure;

                        oraFail.Parameters.Add("vreceipt_status", OracleType.Int16).Value = 7;
                        oraFail.Parameters.Add("vmatching_status", OracleType.Int16).Value = 1;
                        oraFail.Parameters.Add("vBULK_RECEIPT_IND", OracleType.Int16).Value = 1;
                        oraFail.Parameters.Add("vCREATEDBY", OracleType.VarChar).Value = "TCS";
                        oraFail.Parameters.Add("vSerialNo", OracleType.VarChar).Value = item.ToString().Replace("'", "");

                        conn.Open();
                        oraFail.ExecuteNonQuery();
                        conn.Close();

                        continue;

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public String GetBatchNo(String Type)
        {

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SP_FAS_IBT_BATCH_UPDATE";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("vBATCH_TYPE", OracleType.VarChar).Value = Type;
            cmd.Parameters.Add("vBATCH_ID", OracleType.Int32).Direction = ParameterDirection.Output;

            cmd.ExecuteReader();

            return cmd.Parameters["vBATCH_ID"].Value.ToString();

        }

    }
}