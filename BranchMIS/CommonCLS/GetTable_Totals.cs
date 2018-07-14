using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace BranchMIS.CommonCLS
{
    public abstract class GetTable_Totals
    {
        public int _result { get; set; }
        public string _RecordType { get; set; }
        public string _DtlSerialNo { get; set; }
              
        public virtual int getTotal()
        {
            OracleConnection conn_getData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn_getData.Open();

            OracleCommand cmd_getCount = conn_getData.CreateCommand();
            cmd_getCount.CommandText = "SP_FAS_IBT_COUNT_CONFIRMATION";
            cmd_getCount.CommandType = CommandType.StoredProcedure;
            cmd_getCount.Parameters.Add("vSerial_No", OracleType.VarChar).Value = _DtlSerialNo;
            cmd_getCount.Parameters.Add("vTotalRecordsCount", OracleType.Int32).Direction = ParameterDirection.Output;
            cmd_getCount.Parameters.Add("vRecordType", OracleType.VarChar).Value = _RecordType;

            cmd_getCount.ExecuteReader();

            this._result = int.Parse(cmd_getCount.Parameters["vTotalRecordsCount"].Value.ToString());
            return _result;
        }
    }

    public class get_Total_At_PageLoad : GetTable_Totals
    {
        int record_Count_When_PageLoad = 0;

        public override int getTotal()
        {
            record_Count_When_PageLoad = _result;
            return record_Count_When_PageLoad;
        }
    }

    public class get_LiveTotal : GetTable_Totals
    {
        int Live_Count = 0;
        //public override int getTotal()
        //{
        //    Live_Count = _result;
        //    return Live_Count;
        //}
    }

}