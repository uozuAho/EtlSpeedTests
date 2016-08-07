using Etl.Data.Input;
using System.Collections.Generic;
using System.Data;

namespace BulkEtl.InputLoader
{
    static class HobbyLoader
    {
        public static void Load(IEnumerable<Hobby> hobbies, string connstring)
        {
            var dt = CreateBulkEtlHobbyTable();
            var hobbyEn = hobbies.GetEnumerator();
            while (FillHobbyDataTable(dt, hobbyEn) > 0)
            {
                InputUtils.BulkCopyToDb(dt, connstring);
                dt.Clear();
            }
        }

        private static int FillHobbyDataTable(DataTable dt, IEnumerator<Hobby> hobbies, int maxRows = 10000)
        {
            while (hobbies.MoveNext())
            {
                var hobby = hobbies.Current;
                var row = dt.NewRow();
                row["Id"] = hobby.Id;
                row["Name"] = hobby.Name;
                row["Type"] = hobby.Type;
                dt.Rows.Add(row);
                if (dt.Rows.Count == maxRows)
                    break;
            }
            return dt.Rows.Count;
        }

        private static DataTable CreateBulkEtlHobbyTable()
        {
            var dt = new DataTable("BulkEtl_Hobby");
            dt.Columns.Add(new DataColumn { ColumnName = "Id", DataType = typeof(int) });
            dt.Columns.Add(new DataColumn { ColumnName = "Name", DataType = typeof(string), MaxLength = 20 });
            dt.Columns.Add(new DataColumn { ColumnName = "Type", DataType = typeof(string), MaxLength = 20 });
            return dt;
        }
    }
}
