using Etl.Data.Input;
using System.Collections.Generic;
using System.Data;

namespace BulkEtl.InputLoader
{
    static class PersonLoader
    {
        public static void Load(IEnumerable<Person> people, string connstring)
        {
            const int bulkCopyThreshold = 10000;
            var peopleDt = CreateBulkEtlPersonTable();
            var peoplePropDt = InputUtils.CreateBulkEtlPropertyDataTable();
            foreach (var person in people)
            {
                AddPersonRow(peopleDt, person);
                AddPersonPropertyRows(peoplePropDt, person);
                InputUtils.TransferIfOverThresh(peopleDt, connstring, bulkCopyThreshold);
                InputUtils.TransferIfOverThresh(peoplePropDt, connstring, bulkCopyThreshold);
            }
            // write remaining rows to database
            InputUtils.TransferIfOverThresh(peopleDt, connstring, 1);
            InputUtils.TransferIfOverThresh(peoplePropDt, connstring, 1);
        }

        private static void AddPersonRow(DataTable dt, Person person)
        {
            var row = dt.NewRow();
            row["Id"] = person.Id;
            row["FirstName"] = person.FirstName;
            row["LastName"] = person.LastName;
            row["Gender"] = person.Gender;
            row["Address"] = person.Address;
            row["Ph"] = person.Ph;
            row["HobbyId"] = person.HobbyId;
            dt.Rows.Add(row);
        }

        private static void AddPersonPropertyRows(DataTable dt, Person person)
        {
            AddAddressProperty(dt, person);
            AddPhProperty(dt, person);
        }

        private static void AddAddressProperty(DataTable dt, Person person)
        {
            var row = dt.NewRow();
            row["PropertyType"] = "Address";
            row["PersonId"] = person.Id;
            row["Value"] = person.Address;
            dt.Rows.Add(row);
        }

        private static void AddPhProperty(DataTable dt, Person person)
        {
            var row = dt.NewRow();
            row["PropertyType"] = "Ph.";
            row["PersonId"] = person.Id;
            row["Value"] = person.Ph;
            dt.Rows.Add(row);
        }

        private static DataTable CreateBulkEtlPersonTable()
        {
            var dt = new DataTable("BulkEtl_Person");
            dt.Columns.Add(new DataColumn { ColumnName = "Id", DataType = typeof(int) });
            dt.Columns.Add(new DataColumn { ColumnName = "FirstName", DataType = typeof(string), MaxLength = 20 });
            dt.Columns.Add(new DataColumn { ColumnName = "LastName", DataType = typeof(string), MaxLength = 20 });
            dt.Columns.Add(new DataColumn { ColumnName = "Gender", DataType = typeof(string), MaxLength = 6 });
            dt.Columns.Add(new DataColumn { ColumnName = "Address", DataType = typeof(string), MaxLength = 50 });
            dt.Columns.Add(new DataColumn { ColumnName = "Ph", DataType = typeof(string), MaxLength = 10 });
            dt.Columns.Add(new DataColumn { ColumnName = "HobbyId", DataType = typeof(int) });
            return dt;
        }
    }
}
