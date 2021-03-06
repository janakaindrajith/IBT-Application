﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Data;
using System.Configuration;
using System.Security;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.OracleClient;
using System.IO;
using System.Text;
using System.DirectoryServices;
using System.Collections;



namespace BranchMIS.CommonCLS
{
    public class CommonFunctions
    {
        OracleConnection myConnectionUse = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        public string getCurrentUserCode(String UserName)
        {

            String UserCode;

            try
            {
                if (Left(UserName, 4) == "HNBA")
                {
                    UserCode = Right(UserName, (UserName.Length) - 5);

                }
                else
                {
                    UserCode = Right(UserName, (UserName.Length) - 7);

                }

                return UserCode;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string getCurentUser()
        {
            string CurentUser = System.Web.HttpContext.Current.Cache["CurentUser"] as string;
            return CurentUser;
        }

        public string Left(string text, int length)
        {
            return text.Substring(0, length);
        }

        public string Right(string text, int length)
        {
            return text.Substring(text.Length - length, length);
        }

        public string Mid(string text, int start, int end)
        {
            return text.Substring(start, end);
        }

        public string Mid(string text, int start)
        {
            return text.Substring(start, text.Length - start);
        }

        public static void Logger(String PageName, String FunctionName, String Log,String Path)
        {

            string path = Path;// @"E:\AppServ\Example.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine("Welcome To Log File....");
                    tw.Close();
                }
            }
            if (File.Exists(path))
            {
                string current_ibt_usr = HttpContext.Current.Session["IBT_UserName"].ToString();
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now + " - " + current_ibt_usr + " - " + PageName + " - " + FunctionName + " - " + Log);
                tw.Close(); 
            }
        }
        
        public bool getPermissions(string a, string b)//page permissions and loggin validation
        {
            //OracleConnection conn_getPermissions = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            
            try
            {
                //conn_getPermissions.Open();
                myConnectionUse.Close();
                myConnectionUse.Open();

                if ((a == null) || (a == "") || (b == null) || (b == ""))
                {
                    return false;
                }
                else
                {
                    string userName = a; //"test.user";//"janaka.indrajith"; //a;//user name
                    string pageName = b;//page name
                    int userRole = 0;//user role id
                    int per_id = 0; //page id
                    bool valid = false;
                    int inactive = -1;

                    //get user role

                    OracleCommand cmd = myConnectionUse.CreateCommand();
                    cmd.CommandText = "select tt.role_id, tt.inactive from fas_ibt_role_assigned tt where tt.user_name = '" + userName + "'";

                    OracleDataAdapter oda = new OracleDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    oda.Fill(dt);

                    userRole = int.Parse(dt.Rows[0]["ROLE_ID"].ToString());
                    inactive = int.Parse(dt.Rows[0]["INACTIVE"].ToString());

                    if ((userRole != 0)&&((inactive != -1)&&(inactive != 1)))
                    {
                        //check page permissions to relevant user role

                        OracleCommand cmd2 = myConnectionUse.CreateCommand();
                        cmd2.CommandText = "select tt.id from fas_ibt_permissions tt where tt.description = '" + pageName + "'";

                        OracleDataAdapter oda2 = new OracleDataAdapter(cmd2);

                        DataTable dt2 = new DataTable();
                        oda2.Fill(dt2);

                        if (dt2.Rows.Count > 0)
                        {
                            per_id = int.Parse(dt2.Rows[0]["ID"].ToString());
                        }
                        else
                        {
                            return false;
                        }

                        if (per_id != 0)
                        {
                            OracleCommand cmd3 = myConnectionUse.CreateCommand();
                            cmd3.CommandText = "select count (*) from fas_ibt_user_permissions f where f.role_id = " + userRole + " AND f.per_id = " + per_id;

                            OracleDataReader odr = cmd3.ExecuteReader();

                            int rowCount = 0;

                            while (odr.Read())
                            {
                                rowCount = Convert.ToInt32(odr["COUNT(*)"]);
                            }


                            if (rowCount != 0)
                            {
                                valid = true;
                            }
                            else
                            {
                                valid = false;
                            }
                        }                        
                    }

                    myConnectionUse.Close();
                    oda.Dispose();                    
                    return valid;
                }
            }
            catch (Exception ex)
            {
                return false;
                //throw;
            }
            finally
            {
                if (myConnectionUse != null)
                {
                    myConnectionUse.Close();
                    myConnectionUse.Dispose();
                }
            }

        }

        public DataTable getUserRoleAndPageControls(string a, string b)
        {
            string userName = a;
            int userRole = 0;
            string pagename = b;//pagename
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //conn.Open();
            //myConnectionUse.Close();
            OracleConnection myConnectionUse = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());            
            myConnectionUse.Open();

            //get user role

            OracleCommand cmd = myConnectionUse.CreateCommand();
            cmd.CommandText = "select tt.role_id from fas_ibt_role_assigned tt where tt.user_name = '" + userName + "'";

            OracleDataAdapter oda = new OracleDataAdapter(cmd);

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            oda.Fill(dt);

            userRole = int.Parse(dt.Rows[0]["ROLE_ID"].ToString());

            if(userRole!= 0)
            {
                int pageid = -1;

                OracleCommand cmd2 = myConnectionUse.CreateCommand();
                OracleCommand cmd_getPageID = myConnectionUse.CreateCommand();

                cmd_getPageID.CommandText = "select ff.id from fas_ibt_permissions ff where ff.description = '"+ pagename +"'";

                OracleDataReader odr_pageid = cmd_getPageID.ExecuteReader();

                while(odr_pageid.Read())
                {
                    pageid = int.Parse(odr_pageid["id"].ToString());
                }

                cmd2.CommandText = "select f.page_id, f.field_name from fas_ibt_pagecontrols f where f.role_id = " + userRole + " and f.active = 1 and f.page_id = '" + pageid + "'";

                OracleDataAdapter oda2 = new OracleDataAdapter(cmd2);
                
                oda2.Fill(dt2);                
            }
            return dt2;
        }

        public static void IBT_Bifurcate(string Company)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_BIFURCATION_BATCH";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("vCompany", OracleType.VarChar).Value = Company.ToString().ToUpper();

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public static void IBT_DEPT_DECOMPOSE(string Company)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_DEPARTMENT_BATCH";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("vCompany", OracleType.VarChar).Value = Company.ToString().ToUpper();

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public static void IBT_Matching(string Company)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

            try
            {
                //---------------------Normal IBT matching rule execute------------------------------------------------------
                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_MATCHING_BATCH";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("vCompany", OracleType.VarChar).Value = Company.ToString().ToUpper();

                cmd.ExecuteNonQuery();
                conn.Close();
                //-----------------------------------------------------------------------------------------------------------



                return;

                //---------------ckeck sql MRP/MCP database against oracle IBT Data to get macthing policies-----------------
                ////====================== MRP / MCR ========================================================================

                var dtMRP = new DataTable();
                var dtUploadData = new DataTable();

                SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlconnForMRP"].ConnectionString);
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.CommandType = System.Data.CommandType.Text;
                //sqlcmd.CommandText = "SELECT PolicyNo as PolNo FROM  PolicyInfoMicro";
                sqlcmd.CommandText = "SELECT PolicyNo as POLNO,0 as ID, premiumFee as OriginalPremium FROM  PolicyInfoMicro union SELECT PolicyNo as POLNO,0 as ID, premiumFee as OriginalPremium FROM  PolicyInfo";
                sqlcmd.Connection = sqlconn;
                sqlconn.Open();

                var dataReader = sqlcmd.ExecuteReader();
                dtMRP.Load(dataReader);

                sqlconn.Close();
                sqlconn.Dispose();
                dataReader.Close();
                sqlcmd.Dispose();


                OracleCommand cmdUploadData = new OracleCommand();
                conn.Open();
                cmdUploadData.Connection = conn;
                cmdUploadData.CommandText = " SELECT DTL.policy_no as POLNO,DTL.ID, DTL.CREDIT AS UploadedPremium  from fas_ibt_uploaded_dtl DTL Where DTL.MATCHING_STATUS = 0 and DTL.IBT_STATUS = 1";
                OracleDataReader OledbData = cmdUploadData.ExecuteReader();
                var dataReader1 = cmdUploadData.ExecuteReader();
                var dataTable1 = new DataTable();
                dtUploadData.Load(dataReader1);

                conn.Close();

                foreach (DataRow DRUpload in dtUploadData.Rows)
                {
                    if (DRUpload["POLNO"].ToString() != "")
                    {
                        Int64 Count = (Int64)dtMRP.Select("POLNO = '" + DRUpload["POLNO"].ToString() + "' and OriginalPremium = '" + DRUpload["UploadedPremium"].ToString() + " '").Count();

                        if (Count != 0)
                        {
                            UpdateMatchingStatus(Convert.ToInt64(DRUpload["ID"].ToString()), 2);
                        }
                    }
                }
                ////=========================================================================================================
            }
            catch (Exception EX)
            {
                throw EX;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void IBT_BifurcateOLD()
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_FAS_IBT_BIFURCATION_BATCH";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception EX)
            {     
                throw EX;
            }
            //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

            //try
            //{

            //    //OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            //    OracleCommand cmdRule = new OracleCommand();
            //    conn.Open();
            //    cmdRule.Connection = conn;

            //    cmdRule.CommandText = " SELECT RLS.ID, RLS.DESCRIPTION AS RULE , RHDR.DESCTRIPTION AS RULEDESC , RLSMASTER.TYPE " +
            //                          " from fas_ibt_bifurcation_rules_hdr RHDR " +
            //                          " INNER JOIN FAS_IBT_RULES RLS ON RHDR.RULE_ID = RLS.ID " +
            //                          " INNER JOIN fas_ibt_rules RLSMASTER ON RHDR.RULE_ID = RLSMASTER.ID " +
            //                          " WHERE RLSMASTER.TYPE = 1 and RLS.Inactive = 0";

            //    OracleDataReader myOleDbDataReader = cmdRule.ExecuteReader();
            //    while (myOleDbDataReader.Read())
            //    {
            //        OracleCommand cmdUploadData = new OracleCommand();
            //        cmdUploadData.Connection = conn;

            //        String Where = "Where IBT_STATUS = 0 and " + myOleDbDataReader[2].ToString();

            //        cmdUploadData.CommandText = " select t.id,t.account_no,t.serial_no,t.transaction_date,t.statement_date,t.value_date, " +
            //                                    " t.description,t.policy_no,t.bank_ref,t.bank_branch,t.cheque_no,t.debit,t.credit,t.balance, " +
            //                                    " t.openning_bal,t.clossing_bal from fas_ibt_uploaded_dtl t " + Where;

            //        OracleDataReader myOleDbDataReaderUpload = cmdUploadData.ExecuteReader();

            //        while (myOleDbDataReaderUpload.Read())
            //        {
            //            UpdateIBTStatus(myOleDbDataReaderUpload["serial_no"].ToString());
            //        }
            //    }

            //    conn.Close();
            //    myOleDbDataReader.Close();

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    conn.Close();
            //}
        }

        public static void IBT_MatchingOLD()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

            try
            {

                OracleCommand cmdRule = new OracleCommand();
                conn.Open();
                cmdRule.Connection = conn;

                cmdRule.CommandText = " SELECT RLS.ID, RLS.DESCRIPTION AS RULE , RHDR.DESCTRIPTION AS RULEDESC , RLSMASTER.TYPE " +
                                      " from fas_ibt_matching_rules_hdr RHDR " +
                                      " INNER JOIN FAS_IBT_RULES RLS ON RHDR.RULE_ID = RLS.ID " +
                                      " INNER JOIN fas_ibt_rules RLSMASTER ON RHDR.RULE_ID = RLSMASTER.ID " +
                                      " WHERE RLSMASTER.TYPE = 2 and RLS.Inactive = 0";

                OracleDataReader myOleDbDataReader = cmdRule.ExecuteReader();
                while (myOleDbDataReader.Read())
                {


                    String Where = "";



                    //-----Checking Rules
                    //========================LIFE Policy Check================================================================
                    OracleCommand cmdUploadLIFE = new OracleCommand();
                    cmdUploadLIFE.Connection = conn;

                    Where = "";

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("PROPOSAL NO"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("PROPOSAL NO", "POL.PRO_NO");

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("POLICY NO"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("POLICY NO", "POL.Pol_No");

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("NIC"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("NIC", "POL.Pol_holder_NIC");

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("VEHICLE NO"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("VEHICLE NO", "POL.Pol_reg_no");


                    Where = Where + " Where MATCHING_STATUS = 0 and IBT_STATUS = 1";

                    cmdUploadLIFE.CommandText = " select DTL.ID,DTL.Description,pol.pol_no, pol.cus_name " +
                                                " from fas_ibt_uploaded_dtl DTL inner join crc_policy_life POL on  " + Where;

                    OracleDataReader myOleDbDataReaderUploadLIFE = cmdUploadLIFE.ExecuteReader();

                    while (myOleDbDataReaderUploadLIFE.Read())
                    {
                        UpdateMatchingStatus(Convert.ToInt64(myOleDbDataReaderUploadLIFE["ID"].ToString()), 2);//2-Exact
                    }
                    //===========================================================================================================

                    myOleDbDataReaderUploadLIFE.Close();


                    //========================MOTOR Policy Check==================================================================
                    OracleCommand cmdUploadMO = new OracleCommand();
                    cmdUploadMO.Connection = conn;

                    Where = "";

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("PROPOSAL NO"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("PROPOSAL NO", "POL.PRO_NO");

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("POLICY NO"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("POLICY NO", "POL.Pol_No");

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("NIC"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("NIC", "POL.Pol_holder_NIC");

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("VEHICLE NO"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("VEHICLE NO", "POL.Pol_reg_no");


                    Where = Where + " Where MATCHING_STATUS = 0 and IBT_STATUS = 1";

                    cmdUploadMO.CommandText = "select DTL.ID, DTL.Description,pol.pol_no" +
                                            " from fas_ibt_uploaded_dtl DTL inner join crc_policy POL on  " + Where;

                    OracleDataReader myOleDbDataReaderUploadMO = cmdUploadMO.ExecuteReader();

                    while (myOleDbDataReaderUploadMO.Read())
                    {
                        UpdateMatchingStatus(Convert.ToInt64(myOleDbDataReaderUploadMO["ID"].ToString()), 2);
                    }
                    //===========================================================================================================

                    myOleDbDataReaderUploadMO.Close();

                    //========================Non Motor Policy Check=============================================================
                    OracleCommand cmdUploadNM = new OracleCommand();
                    cmdUploadNM.Connection = conn;

                    Where = "";

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("PROPOSAL NO"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("PROPOSAL NO", "POL.PRO_NO");

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("POLICY NO"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("POLICY NO", "POL.Pol_No");

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("NIC"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("NIC", "POL.Pol_holder_NIC");

                    if (myOleDbDataReader[2].ToString().ToUpper().Contains("VEHICLE NO"))
                        Where = Where + " " + myOleDbDataReader[2].ToString().Replace("VEHICLE NO", "POL.Pol_reg_no");


                    Where = Where + " Where MATCHING_STATUS = 0 and IBT_STATUS = 1";

                    cmdUploadNM.CommandText = " select DTL.ID,DTL.Description,pol.pol_no" +
                                              " from fas_ibt_uploaded_dtl DTL inner join crc_policy_non_Motor POL on  " + Where;

                    OracleDataReader myOleDbDataReaderUploadNM = cmdUploadNM.ExecuteReader();

                    while (myOleDbDataReaderUploadNM.Read())
                    {
                        UpdateMatchingStatus(Convert.ToInt64(myOleDbDataReaderUploadNM["ID"].ToString()), 2);
                    }
                    //===========================================================================================================

                    myOleDbDataReaderUploadNM.Close();
                }







                //Possible data set
                //======================================Manual AI Data Base Check============================================
                //------------Motor-------------

                OracleCommand cmdManualDataMotor = new OracleCommand();
                cmdManualDataMotor.Connection = conn;

                cmdManualDataMotor.CommandText = " select DTL.Id from fas_ibt_uploaded_dtl DTL, fas_ibt_possible_match_data MDATA, crc_policy POL " +
                                                 " where DTL.Policy_No = MDATA.VALUE and DTL.Policy_No = POL.Pol_No";

                OracleDataReader OledbManualDataMotor = cmdManualDataMotor.ExecuteReader();

                while (OledbManualDataMotor.Read())
                {
                    UpdateMatchingStatus(Convert.ToInt64(OledbManualDataMotor["ID"].ToString()), 1);
                }
                OledbManualDataMotor.Close();
                //------------------------------



                //------------NMMotor-----------
                OracleCommand cmdManualDataNM = new OracleCommand();
                cmdManualDataNM.Connection = conn;

                cmdManualDataNM.CommandText = " select DTL.Id from fas_ibt_uploaded_dtl DTL, fas_ibt_possible_match_data MDATA, crc_policy_non_motor POL " +
                                              " where DTL.Policy_No = MDATA.VALUE and DTL.Policy_No = POL.Pol_No ";

                OracleDataReader OledbManualDataNM = cmdManualDataNM.ExecuteReader();

                while (OledbManualDataNM.Read())
                {
                    UpdateMatchingStatus(Convert.ToInt64(OledbManualDataNM["ID"].ToString()), 1);
                }

                OledbManualDataNM.Close();


                //-------------LIFE------------
                OracleCommand cmdManualDataLIFE = new OracleCommand();
                cmdManualDataLIFE.Connection = conn;

                cmdManualDataLIFE.CommandText = " select DTL.Id from fas_ibt_uploaded_dtl DTL, fas_ibt_possible_match_data MDATA, crc_policy_life POL " +
                                                " where DTL.Policy_No = MDATA.VALUE and DTL.Policy_No = POL.Pol_No";

                OracleDataReader OledbManualDataLIFE = cmdManualDataLIFE.ExecuteReader();

                while (OledbManualDataLIFE.Read())
                {
                    UpdateMatchingStatus(Convert.ToInt64(OledbManualDataLIFE["ID"].ToString()), 1);
                }
                //===========================================================================================================

                OledbManualDataLIFE.Close();
            


                //---------------ckeck sql MRP/MCP database against oracle IBT Data to get macthing policies-----------------
                ////====================== MRP / MCR ========================================================================

                var dtMRP = new DataTable();
                var dtUploadData = new DataTable();

                SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlconnForMRP"].ConnectionString);
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.CommandType = System.Data.CommandType.Text;
                //sqlcmd.CommandText = "SELECT PolicyNo as PolNo FROM  PolicyInfoMicro";
                sqlcmd.CommandText = "SELECT PolicyNo as POLNO,0 as ID FROM  PolicyInfoMicro union SELECT PolicyNo as POLNO,0 as ID FROM  PolicyInfo";
                sqlcmd.Connection = sqlconn;
                sqlconn.Open();

                var dataReader = sqlcmd.ExecuteReader();
                dtMRP.Load(dataReader);

                sqlconn.Close();
                sqlconn.Dispose();
                dataReader.Close();
                sqlcmd.Dispose();


                OracleCommand cmdUploadData = new OracleCommand();
                cmdUploadData.Connection = conn;
                cmdUploadData.CommandText = " select DTL.policy_no as POLNO,DTL.ID  from fas_ibt_uploaded_dtl DTL Where DTL.MATCHING_STATUS = 0 and DTL.IBT_STATUS = 1";
                OracleDataReader OledbData= cmdUploadData.ExecuteReader();
                var dataReader1 = cmdUploadData.ExecuteReader();
                var dataTable1 = new DataTable();
                dtUploadData.Load(dataReader1);


                conn.Close();
                myOleDbDataReader.Close();




                foreach (DataRow DRUpload in dtUploadData.Rows)
                {
                    if (DRUpload["POLNO"].ToString() != "")
                    {
                        Int64 Count = (Int64)dtMRP.Select("POLNO = '" + DRUpload["POLNO"].ToString() + "'").Count();

                        if (Count !=0)
                        {
                            UpdateMatchingStatus(Convert.ToInt64(DRUpload["ID"].ToString()), 2);
                        }
                    }
                }

                ////=========================================================================================================



            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        private static DataTable GetIBT()
        {
            try
            {
                DataTable Dt = null;

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                OracleCommand cmd = new OracleCommand();
                conn.Open();
                cmd.Connection = conn;

                cmd.CommandText = " SELECT DTL.ID,DTL.POLICY_NO FROM FAS_IBT_UPLOADED_DTL DTL  " +
                                  " INNER JOIN FAS_IBT_POSSIBLE_MATCH_DATA MDATA ON DTL.POLICY_NO = MDATA.PARAMETER " +
                                  " WHERE DTL.MATCHING_STATUS = 0 ";

                OracleDataReader myOleDbDataReader = cmd.ExecuteReader();

                OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                Dt.Load(dr);
                conn.Close();

                myOleDbDataReader.Close();

                return Dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static void UpdateMatchingStatus(Int64 ID,Int16 Status)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                OracleCommand cmdUploadData = new OracleCommand();
                cmdUploadData.Connection = conn;
                conn.Open();
                cmdUploadData.CommandText = "UPDATE fas_ibt_uploaded_dtl SET MATCHING_STATUS = " + Status + " WHERE ID =" + ID;
                OracleDataReader myOleDbDataReaderUpload = cmdUploadData.ExecuteReader();
                conn.Close();
                myOleDbDataReaderUpload.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetUserRole(String UserName)
        {
            try
            {
                String UserRole="";

                OracleConnection conn_GetUserRole = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conn_GetUserRole.Open();

                OracleCommand cmd = conn_GetUserRole.CreateCommand();
                cmd.CommandText = "select RLS.COMPANY from fas_ibt_usr_roles RLS inner join fas_ibt_role_assigned USR on RLS.Role_Id = usr.role_id where USR.USER_NAME = '" + UserName + "'";

                OracleDataAdapter oda = new OracleDataAdapter(cmd);

                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();

                oda.Fill(dt);

                UserRole = dt.Rows[0]["COMPANY"].ToString();

                return UserRole;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static void UpdateIBTStatus(String SerialNo)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                OracleCommand cmdUploadData = new OracleCommand();
                cmdUploadData.Connection = conn;
                conn.Open();
                cmdUploadData.CommandText = "UPDATE fas_ibt_uploaded_dtl SET IBT_STATUS = 1 WHERE SERIAL_NO ='" + SerialNo + "'";
                OracleDataReader myOleDbDataReaderUpload = cmdUploadData.ExecuteReader();
                conn.Close();
                myOleDbDataReaderUpload.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}