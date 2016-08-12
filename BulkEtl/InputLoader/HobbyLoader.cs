using Etl.Data.Input;
using System.Collections.Generic;
using System.Data;

namespace BulkEtl.InputLoader
{
    static class HobbyLoader
    {
        public static void Load(IEnumerable<Hobby> hobbies, string connstring)
        {
            const int bulkCopyThreshold = 10000;
            var hobbiesDt = CreateBulkEtlHobbyTable();
            var hobbyPropDt = InputUtils.CreateBulkEtlPropertyDataTable();
            foreach (var hobby in hobbies)
            {
                AddHobbyRow(hobbiesDt, hobby);
                AddHobbyPropertyRows(hobbyPropDt, hobby);
                InputUtils.TransferIfOverThresh(hobbiesDt, connstring, bulkCopyThreshold);
                InputUtils.TransferIfOverThresh(hobbyPropDt, connstring, bulkCopyThreshold);
            }
            // write remaining rows to database
            InputUtils.TransferIfOverThresh(hobbiesDt, connstring, 1);
            InputUtils.TransferIfOverThresh(hobbyPropDt, connstring, 1);
        }

        private static void AddHobbyRow(DataTable dt, Hobby hobby)
        {
            var row = dt.NewRow();
            row["Id"] = hobby.Id;
            row["Name"] = hobby.Name;
            row["Type"] = hobby.Type;
            dt.Rows.Add(row);
        }

        private static void AddHobbyPropertyRows(DataTable dt, Hobby hobby)
        {
            AddHobbyIdProperty(dt, hobby);
            AddHobbyTypeProperty(dt, hobby);
        }

        private static void AddHobbyIdProperty(DataTable dt, Hobby hobby)
        {
            var row = dt.NewRow();
            row["PropertyType"] = "Hobby Id";
            row["HobbyId"] = hobby.Id;
            row["Value"] = hobby.Id;
            dt.Rows.Add(row);
        }

        private static void AddHobbyTypeProperty(DataTable dt, Hobby hobby)
        {
            var row = dt.NewRow();
            row["PropertyType"] = "Hobby Type";
            row["HobbyId"] = hobby.Id;
            row["Value"] = hobby.Type;
            dt.Rows.Add(row);
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
