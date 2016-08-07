using System.Data;
using System.Data.SqlClient;

namespace BulkEtl.InputLoader
{
    static class InputUtils
    {
        /// <summary>
        /// Copy a datatable to a database, assuming the table name and column names are all equal
        /// </summary>
        public static void BulkCopyToDb(DataTable dt, string connstring)
        {
            using (var sbc = new SqlBulkCopy(connstring))
            {
                sbc.DestinationTableName = dt.TableName;
                foreach (DataColumn col in dt.Columns)
                    sbc.ColumnMappings.Add(new SqlBulkCopyColumnMapping(col.ColumnName, col.ColumnName));
                sbc.WriteToServer(dt);
            }
        }

        /// <summary>
        /// Copy datatable to server and clear the datatable, if the row count is >= the threshold
        /// </summary>
        public static void TransferIfOverThresh(DataTable dt, string connstring, int threshold)
        {
            if (dt.Rows.Count >= threshold)
            {
                BulkCopyToDb(dt, connstring);
                dt.Clear();
            }
        }

        public static DataTable CreateBulkEtlPropertyDataTable()
        {
            var dt = new DataTable("BulkEtl_Property");
            dt.Columns.Add(new DataColumn { ColumnName = "PropertyType", DataType = typeof(string), MaxLength = 20, AllowDBNull = false });
            dt.Columns.Add(new DataColumn { ColumnName = "PersonId", DataType = typeof(int) });
            dt.Columns.Add(new DataColumn { ColumnName = "HobbyId", DataType = typeof(int) });
            dt.Columns.Add(new DataColumn { ColumnName = "Value", DataType = typeof(string), MaxLength = 20 });
            return dt;
        }
    }
}
