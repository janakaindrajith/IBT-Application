//using Microsoft.SqlServer.Management.Common;
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


namespace BranchMIS.CommonCLS
{
    public class MRPMCRDetails
    {


        private string usrName = "";
        public int usr_role = 0;

        //static Server server;
        //static Job job;

        private DataTable GetUnmatchedDetails()
        {
            try
            {

                DataTable Dt = new DataTable();

                OracleConnection conn_GetUserRole = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn_GetUserRole.Open();

                OracleCommand cmd = conn_GetUserRole.CreateCommand();

                cmd.CommandText = " select Dtl.Serial_No, Dept.department_description as DeptDesc, Dept.value " +
                                  " from fas_ibt_departments Dept " +
                                  " inner join fas_ibt_uploaded_dtl Dtl on Dept.Value = Dtl.Department " +
                                  " where Dept.mrp_batch_process = 1 and Dtl.effective_end_date is null and Dtl.matching_status = 0  and Dtl.Receipt_Status = 8";

                OracleDataAdapter oda = new OracleDataAdapter(cmd);

                oda.Fill(Dt);

                return Dt;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public Boolean ValidateTotals(String SerailNo, Double Total)
        {
            Boolean Status = false;

            try
            {

                OracleConnection conn_GetUserRole = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn_GetUserRole.Open();

                OracleCommand cmd = conn_GetUserRole.CreateCommand();
                cmd.CommandText = "select count(ID) as Count from fas_ibt_uploaded_dtl dtl where dtl.serial_no = '" + SerailNo + "' and dtl.effective_end_date is null and dtl.credit = " + Total + " ";

                OracleDataAdapter oda = new OracleDataAdapter(cmd);

                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();

                oda.Fill(dt);

                Int16 Count = Convert.ToInt16(dt.Rows[0]["Count"].ToString());

                if (Count > 0)
                {
                    Status = true;
                }

                return Status;
            }
            catch (Exception)
            {

                throw;
            }


        }


        public void UpdateMRPMCR(string Type)//----------------Update MRP MCR-------------------------------//
        {
            try
            {

                DataTable DtTemp = GetUnmatchedDetails();

                String StrMRP = "";
                String StrMCR = "";
                ArrayList UnmatchedList = new ArrayList();


                if (DtTemp.Rows.Count == 0)
                {
                    return;
                }

                foreach (DataRow Dr in DtTemp.Rows)
                {
                    if (Dr["DeptDesc"].ToString() == "MRP")
                    {
                        StrMRP = StrMRP + "'" + Dr["serial_NO"].ToString() + "',";
                    }
                    if (Dr["DeptDesc"].ToString() == "MCR")
                    {
                        StrMCR = StrMCR + "'" + Dr["serial_NO"].ToString() + "',";
                    }
                }

                if (StrMCR != "")
                {
                    StrMCR = StrMCR.Remove(StrMCR.Length - 1, 1);
                }
                else
                {
                    StrMCR = "'xxxxxxxxxxxxxxxxx'";
                }

                if (StrMRP != "")
                {
                    StrMRP = StrMRP.Remove(StrMRP.Length - 1, 1);
                }
                else
                {
                    StrMRP = "'xxxxxxxxxxxxxxxxx'";
                }

                //------------------------------Get MRP/MCR Details---------------------------------
                var dtMRP = new DataTable();
                var dtMCR = new DataTable();

                SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlconnForMRP"].ConnectionString);
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.CommandType = System.Data.CommandType.Text;

                //sqlcmd.CommandText = " SELECT REPLACE(C.PropNo,'/','') AS PROPOSALNO,UPPER(C.Name) AS CLIENTNAME, " +
                //     " C.Premiumpolicyfee AS PREMIUM,UPPER(SUBSTRING(C.Code,1,4)) AS HNBCODE,  " +
                //     " C.Ibtdate AS IBTDATE,UPPER(REPLACE(REPLACE(C.ACCOUNT,' ',''),'-','')) AS CASHACCOUNT," +
                //     " 'MRP' AS PRODUCT,BAN.Branchname,C.BNKCode, 'Count' as Count  FROM   Chequedetail C,BANK BAN  " +
                //     " WHERE  BAN.BNKCode = C.Code  " +
                //     " --AND Convert(datetime,C.ibtDate,103) >= '05/08/2016'" +
                //     " --AND Convert(datetime,C.ibtDate,103) <= '11/08/2016' " +
                //     " AND REPLACE(REPLACE(C.ACCOUNT,' ',''),'-','') IN (" + StrMRP + ") " +
                //     " UNION  " +
                //     " SELECT REPLACE(C.PropNo,'/','') AS PROPOSALNO,UPPER(C.Name) AS CLIENTNAME," +
                //     " C.Premiumpolicyfee AS PREMIUM,UPPER(SUBSTRING(C.Code,1,4)) AS HNBCODE,  " +
                //     " C.Ibtdate AS IBTDATE,UPPER(REPLACE(REPLACE(C.ACCOUNT,' ',''),'-','')) AS CASHACCOUNT, " +
                //     " 'MCR' AS PRODUCT,BAN.Branchname,C.BNKCode, 'Count' as Count   FROM   ChequedetailMicro C,BANK BAN  " +
                //     " WHERE  BAN.BNKCode = C.Code   " +
                //     " --AND Convert(datetime,C.ibtDate,103) >= '05/08/2016' " +
                //     " --AND Convert(datetime,C.ibtDate,103) <= '11/08/2016' " +
                //     " AND REPLACE(REPLACE(C.ACCOUNT,' ',''),'-','') IN (" + StrMCR + ") ";

                //25-09-2017
                //sqlcmd.CommandText = " SELECT REPLACE(C.PropNo,'/','') AS PROPOSALNO,UPPER(C.Name) AS CLIENTNAME, " +
                //                     " C.Premium AS PREMIUM, C.Policyfee AS POLICYFEE, UPPER(SUBSTRING(C.Code,1,4)) AS HNBCODE,  " +
                //                     " C.Ibtdate AS IBTDATE,C.ACCOUNT AS CASHACCOUNT," +
                //                     " 'MRP' AS PRODUCT,BAN.Branchname,C.BNKCode, 'Count' as Count  FROM   Chequedetail C,BANK BAN  " +
                //                     " WHERE  BAN.BNKCode = C.Code  " +
                //                     " AND C.ACCOUNT IN (" + StrMRP + ") " +
                //                     " UNION  " +
                //                     " SELECT REPLACE(C.PropNo,'/','') AS PROPOSALNO,UPPER(C.Name) AS CLIENTNAME," +
                //                     " C.Premium AS PREMIUM, C.Policyfee AS POLICYFEE, UPPER(SUBSTRING(C.Code,1,4)) AS HNBCODE,  " +
                //                     " C.Ibtdate AS IBTDATE,C.ACCOUNT AS CASHACCOUNT, " +
                //                     " 'MCR' AS PRODUCT,BAN.Branchname,C.BNKCode, 'Count' as Count   FROM   ChequedetailMicro C,BANK BAN  " +
                //                     " WHERE  BAN.BNKCode = C.Code   " +
                //                     " AND C.ACCOUNT IN (" + StrMCR + ") order by PRODUCT";

                if (Type == "MRP")
                {
                    sqlcmd.CommandText = " SELECT REPLACE(C.PropNo,'/','') AS PROPOSALNO,UPPER(C.Name) AS CLIENTNAME, " +
                                         " C.Premium AS PREMIUM, C.Policyfee AS POLICYFEE, UPPER(SUBSTRING(C.Code,1,4)) AS HNBCODE,  " +
                                         " C.Ibtdate AS IBTDATE,C.ACCOUNT AS CASHACCOUNT," +
                                         " 'MRP' AS PRODUCT,BAN.Branchname,C.BNKCode, 'Count' as Count,Premiumpolicyfee  FROM   Chequedetail C,BANK BAN  " +
                                         " WHERE  BAN.BNKCode = C.Code  " +
                                         " AND C.ACCOUNT IN (" + StrMRP + ") ";
                }
                if (Type == "MCR")
                {
                    sqlcmd.CommandText = " SELECT REPLACE(C.PropNo,'/','') AS PROPOSALNO,UPPER(C.Name) AS CLIENTNAME," +
                                         " C.Premium AS PREMIUM, C.Policyfee AS POLICYFEE, UPPER(SUBSTRING(C.Code,1,4)) AS HNBCODE,  " +
                                         " C.Ibtdate AS IBTDATE,C.ACCOUNT AS CASHACCOUNT, " +
                                         " 'MCR' AS PRODUCT,BAN.Branchname,C.BNKCode, 'Count' as Count,Premiumpolicyfee   FROM   ChequedetailMicro C,BANK BAN  " +
                                         " WHERE  BAN.BNKCode = C.Code   " +
                                         " AND C.ACCOUNT IN (" + StrMCR + ") order by PRODUCT";
                }


                sqlcmd.Connection = sqlconn;
                sqlconn.Open();

                var dataReader = sqlcmd.ExecuteReader();
                dtMRP.Load(dataReader);


                DataView dv = dtMRP.DefaultView;
                dv.Sort = "IBTDATE desc";
                dtMRP = dv.ToTable();

                sqlconn.Close();
                sqlconn.Dispose();
                dataReader.Close();
                sqlcmd.Dispose();



                DataTable DtRecCount = new DataTable();
                DtRecCount.Columns.Add("SerialNo", typeof(string));
                DtRecCount.Columns.Add("Count", typeof(int));

                ArrayList arrRegular = new ArrayList();
                ArrayList arrBulk = new ArrayList();


                foreach (DataRow item in dtMRP.Rows)
                {
                    String CASHACCOUNT = item["CASHACCOUNT"].ToString();

                    DataRow[] DrTemp1 = dtMRP.Select("CASHACCOUNT = '" + CASHACCOUNT + "'");

                    if (DrTemp1.Count() > 1)
                    {
                        if (!arrBulk.Contains(CASHACCOUNT))
                        {
                            if (!(CASHACCOUNT == "" || CASHACCOUNT == " " || CASHACCOUNT == null))
                            {
                                arrBulk.Add(CASHACCOUNT);
                            }
                        }
                    }
                    else
                    {
                        if (!(CASHACCOUNT == "" || CASHACCOUNT == " " || CASHACCOUNT == null))
                        {
                            arrRegular.Add(CASHACCOUNT);
                        }
                    }
                }



                CommonCLS.IBTBatches IBTBatch = new CommonCLS.IBTBatches();
                String BatchNo = IBTBatch.GetBatchNo("BATCH_NO");


                //------------------------- MRP / MCR Receipts Updation---------------------
                using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString()))
                {
                    conn.Open();
                    OracleCommand command = conn.CreateCommand();
                    OracleTransaction transaction;

                    transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    command.Transaction = transaction;


                    foreach (var item in arrRegular)
                    {
                        String CASHACCOUNT = item.ToString();
                        DataRow Dr = dtMRP.Select("CASHACCOUNT = '" + CASHACCOUNT + "'").Single();

                        //-----------------------Regular Receipting-------------------------------------
                        //OracleCommand cmd = new OracleCommand();//conn.CreateCommand();
                        //cmd.Connection = conn;
                        //cmd.CommandText = "SP_FAS_UPDATE_MRPMCR_DTL";
                        //cmd.CommandType = CommandType.StoredProcedure;



                        OracleCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SP_FAS_IBT_UPD_MRPMCR_DTL";
                        cmd.CommandType = CommandType.StoredProcedure;


                        String SerialNo = Dr["CASHACCOUNT"].ToString();
                        String PolicyNo = Dr["PROPOSALNO"].ToString();
                        String Premium = Dr["PREMIUM"].ToString();
                        String PolicyFee = Dr["POLICYFEE"].ToString();
                        String MRPNarration = Dr["HNBCODE"].ToString() + '-' + Dr["CASHACCOUNT"].ToString() + ';' + PolicyNo + ':' + Dr["CLIENTNAME"].ToString();
                        String MRPPayingParty = Dr["BRANCHNAME"].ToString();

                        String Premiumpolicyfee = Dr["Premiumpolicyfee"].ToString();


                        cmd.Parameters.AddWithValue("vSerialNo", OracleType.VarChar).Value = SerialNo;
                        cmd.Parameters.AddWithValue("vPolicyNo", OracleType.VarChar).Value = PolicyNo;
                        cmd.Parameters.AddWithValue("vAmount", OracleType.Number).Value = Premium;
                        cmd.Parameters.AddWithValue("vMRPNarration", OracleType.VarChar).Value = MRPNarration.Trim();
                        cmd.Parameters.AddWithValue("vMRPPayingParty", OracleType.VarChar).Value = "Hatton National Bank PLC-" + MRPPayingParty.Trim();
                        cmd.Parameters.AddWithValue("vMRPPolicyFee", OracleType.Number).Value = PolicyFee;
                        cmd.Parameters.AddWithValue("vBatchNo", OracleType.Number).Value = BatchNo;

                        //23-11-2017
                        cmd.Parameters.AddWithValue("vCreatedBy", OracleType.Number).Value = Type;// "MRP";
                        cmd.Parameters.AddWithValue("vPremiumpolicyfee", OracleType.Number).Value = Premiumpolicyfee;

                        cmd.Parameters.Add("vResult", OracleType.VarChar,100).Direction = ParameterDirection.Output;


                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                        string vResult = cmd.Parameters["vResult"].Value.ToString();

                        if (vResult != "Updated")
                        {
                            UnmatchedList.Add(vResult);
                        }

                        //UnmatchedList.Add(vResult);

                        //-------------------------------------------------------------------------------
                    }



                    //------------------------Bulk Receipting----------------------------------------
                    Int16 SubNo = 1;
                    foreach (var item in arrBulk)
                    {
                        String CASHACCOUNT = item.ToString();
                        DataRow[] Dr = dtMRP.Select("CASHACCOUNT = '" + CASHACCOUNT + "'");

                        DataTable dtTemp = Dr.CopyToDataTable();


                        //---------Validate MRP Divivion Entered Total With Statement Total--------
                        Double TempMRPTotal = 0;
                        foreach (DataRow row in dtTemp.Rows)
                        {
                            TempMRPTotal = TempMRPTotal + (Convert.ToDouble(row["Premium"].ToString()) + Convert.ToDouble(row["POLICYFEE"].ToString())); 
                        }

                        Boolean Temp = ValidateTotals(item.ToString(), TempMRPTotal);
                        if (Temp != true)
                        {
                            UnmatchedList.Add(item.ToString());
                            continue;
                        }
                        //-------------------------------------------------------------------------

                        Int16 Count = 1;

                        foreach (DataRow row in dtTemp.Rows)
                        {

                            String SerialNo = row["cashaccount"].ToString();
                            String RefNumber = row["proposalno"].ToString();
                            String Amount = row["Premium"].ToString();
                            String PolicyFee = row["POLICYFEE"].ToString();
                            String MRPNarration = row["HNBCODE"].ToString() + '-' + row["CASHACCOUNT"].ToString() + ';' + RefNumber + ':' + row["CLIENTNAME"].ToString();
                            String MRPPayingParty = row["BRANCHNAME"].ToString();

                            OracleCommand cmdBulk = new OracleCommand();//conn.CreateCommand();
                            cmdBulk.Connection = conn;

                            cmdBulk.CommandText = "SP_FAS_IBT_UPD_MRPMCR_DTL_BULK";
                            cmdBulk.CommandType = CommandType.StoredProcedure;

                            //cmdBulk.Parameters.AddWithValue("vID", OracleType.Int32).Value = DtlID;
                            cmdBulk.Parameters.AddWithValue("vSerialNo", OracleType.VarChar).Value = SerialNo;
                            cmdBulk.Parameters.AddWithValue("vSubNo", OracleType.VarChar).Value = SubNo;
                            cmdBulk.Parameters.AddWithValue("vRefNumber", OracleType.VarChar).Value = RefNumber;
                            cmdBulk.Parameters.AddWithValue("vAmount", OracleType.Number).Value = Convert.ToDouble(Amount);
                            cmdBulk.Parameters.AddWithValue("vCreatedBy", OracleType.Number).Value = Type;// "MRP";
                            cmdBulk.Parameters.AddWithValue("vCount", OracleType.Number).Value = Count;
                            cmdBulk.Parameters.AddWithValue("vMRPNarration", OracleType.VarChar).Value = MRPNarration.Trim();
                            cmdBulk.Parameters.AddWithValue("vMRPPayingParty", OracleType.VarChar).Value = "Hatton National Bank PLC-" + MRPPayingParty.Trim();
                            cmdBulk.Parameters.AddWithValue("vMRPPolicyFee", OracleType.Number).Value = PolicyFee;
                            cmdBulk.Parameters.AddWithValue("vBatchNo", OracleType.Number).Value = BatchNo;

                            SubNo = Convert.ToInt16(SubNo + 1);

                            cmdBulk.Transaction = transaction;

                            cmdBulk.ExecuteNonQuery();                          

                            Count++;

                        }

                        SubNo = 1;
                    }


                    //-------------------------------------------------------------------------------


                    transaction.Commit();
                    conn.Close();


                    //Emailling The Figure -- Unmatched List(Total check with uploaded record total if not tally reject)
                    if (UnmatchedList.Count > 0)
                    {
                        CommonCLS.IBTEmails.EmailAlertCommon(9, UnmatchedList,"");
                    }


                }
                //----------------------------------------------------------------


            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}