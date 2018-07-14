using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace BranchMIS.CommonCLS
{
    public class IBT_LockRecordValidation
    {

        //--------------------------------FOR BULK RECORDS------------------------------//

        public int get_Total_At_PageLoad(string RecordType, string DtlSerialNo)
        {
            OracleConnection conn_getData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn_getData.Open();

            int result = -1;

            if (RecordType == "Bulk")
            {
                //--------------Check Record Count In Bulk Detail Table--------//
                int record_Count_When_PageLoad = 0;

                OracleCommand cmd_getCount = conn_getData.CreateCommand();
                cmd_getCount.CommandText = "SP_FAS_IBT_COUNT_CONFIRMATION";
                cmd_getCount.CommandType = CommandType.StoredProcedure;
                cmd_getCount.Parameters.Add("vSerial_No", OracleType.VarChar).Value = DtlSerialNo;
                cmd_getCount.Parameters.Add("vTotalRecordsCount", OracleType.Int32).Direction = ParameterDirection.Output;
                cmd_getCount.Parameters.Add("vRecordType", OracleType.VarChar).Value = RecordType;

                cmd_getCount.ExecuteReader();

                record_Count_When_PageLoad = int.Parse(cmd_getCount.Parameters["vTotalRecordsCount"].Value.ToString());

                result = record_Count_When_PageLoad;
            }
            return result;
        }

        public int get_LiveTotal(string RecordType, string DtlSerialNo)
        {
            OracleConnection conn_getData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn_getData.Open();

            int result = -1;

            if (RecordType == "Bulk")
            {
                //-----------Check Record Count In Bulk Detail Table------//
                int record_Count_When_PageLoad = 0;

                OracleCommand cmd_getCount = conn_getData.CreateCommand();
                cmd_getCount.CommandText = "SP_FAS_IBT_COUNT_CONFIRMATION";
                cmd_getCount.CommandType = CommandType.StoredProcedure;
                cmd_getCount.Parameters.Add("vSerial_No", OracleType.VarChar).Value = DtlSerialNo;
                cmd_getCount.Parameters.Add("vTotalRecordsCount", OracleType.Int32).Direction = ParameterDirection.Output;
                cmd_getCount.Parameters.Add("vRecordType", OracleType.VarChar).Value = RecordType;

                cmd_getCount.ExecuteReader();

                record_Count_When_PageLoad = int.Parse(cmd_getCount.Parameters["vTotalRecordsCount"].Value.ToString());

                result = record_Count_When_PageLoad;
            }
            return result;
        }

        //-----------------------------------------------------------------------------//


        //--------------------------------FOR SINGLE RECORD----------------------------//

        public int get_Regular_Total_At_PageLoad(string RecordType, string DtlSerialNo)
        {
            OracleConnection conn_getData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn_getData.Open();

            int result = -1;

            if (RecordType == "Regular")
            {
                //-----------Check Record Count In Bulk Detail Table------//
                int record_Count_When_PageLoad = 0;

                OracleCommand cmd_getCount = conn_getData.CreateCommand();
                cmd_getCount.CommandText = "SP_FAS_IBT_COUNT_CONFIRMATION";
                cmd_getCount.CommandType = CommandType.StoredProcedure;
                cmd_getCount.Parameters.Add("vSerial_No", OracleType.VarChar).Value = DtlSerialNo;
                cmd_getCount.Parameters.Add("vTotalRecordsCount", OracleType.Int32).Direction = ParameterDirection.Output;
                cmd_getCount.Parameters.Add("vRecordType", OracleType.VarChar).Value = RecordType;

                cmd_getCount.ExecuteReader();

                record_Count_When_PageLoad = int.Parse(cmd_getCount.Parameters["vTotalRecordsCount"].Value.ToString());

                result = record_Count_When_PageLoad;
            }
            return result;
        }

        //-----------------------------------------------------------------------------//
    }
}
