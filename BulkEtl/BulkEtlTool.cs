using Etl;
using Etl.Data.Input;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BulkEtl
{
    /// <summary>
    /// Loads target database using bulk copies and SQL commands for data conversion.
    /// Intended to provide an absolute best case time estimate - doesn't satisfy a
    /// number of requirements in readme.txt.
    /// </summary>
    public class BulkEtlTool : IEtl
    {
        private readonly IEnumerable<Person> _people;
        private readonly IEnumerable<Hobby> _hobbies;

        private readonly string _connstring;

        public BulkEtlTool(IEnumerable<Person> people, IEnumerable<Hobby> hobbies, string connstring)
        {
            _people = people;
            _hobbies = hobbies;
            _connstring = connstring;
        }

        public void Run()
        {
            LoadInputTables();
            LoadTarget();
        }

        private void LoadInputTables()
        {
            LoadPersonInputTable();
            LoadHobbyInputTable();
        }

        private void LoadPersonInputTable()
        {
            var dt = CreateBulkEtlPersonTable();
            var peopleEn = _people.GetEnumerator();
            while (FillPersonDataTable(dt, peopleEn) > 0)
            {
                BulkCopyToDb(dt, _connstring);
                dt.Clear();
            }
        }

        private static int FillPersonDataTable(DataTable dt, IEnumerator<Person> people, int maxRows = 10000)
        {
            // TODO: properties here
            while (people.MoveNext())
            {
                var person = people.Current;
                var row = dt.NewRow();
                row["Id"] = person.Id;
                row["FirstName"] = person.FirstName;
                row["LastName"] = person.LastName;
                row["Gender"] = person.Gender;
                row["Address"] = person.Address;
                row["Ph"] = person.Ph;
                row["HobbyId"] = person.HobbyId;
                dt.Rows.Add(row);
                if (dt.Rows.Count == maxRows)
                    break;
            }
            return dt.Rows.Count;
        }

        private void LoadHobbyInputTable()
        {
            var dt = CreateBulkEtlHobbyTable();
            var hobbyEn = _hobbies.GetEnumerator();
            while (FillHobbyDataTable(dt, hobbyEn) > 0)
            {
                BulkCopyToDb(dt, _connstring);
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

        private static DataTable CreateBulkEtlHobbyTable()
        {
            var dt = new DataTable("BulkEtl_Hobby");
            dt.Columns.Add(new DataColumn { ColumnName = "Id", DataType = typeof(int) });
            dt.Columns.Add(new DataColumn { ColumnName = "Name", DataType = typeof(string), MaxLength = 20 });
            dt.Columns.Add(new DataColumn { ColumnName = "Type", DataType = typeof(string), MaxLength = 20 });
            return dt;
        }

        /// <summary>
        /// Copy a datatable to a database, assuming the table name and column names are all equal
        /// </summary>
        private static void BulkCopyToDb(DataTable dt, string connstring)
        {
            using (var sbc = new SqlBulkCopy(connstring))
            {
                sbc.DestinationTableName = dt.TableName;
                foreach (DataColumn col in dt.Columns)
                    sbc.ColumnMappings.Add(new SqlBulkCopyColumnMapping(col.ColumnName, col.ColumnName));
                sbc.WriteToServer(dt);
            }
        }

        private void LoadTarget()
        {
            LoadIndividualTarget();
            LoadActivityTarget();
        }

        private void LoadIndividualTarget()
        {
            const string cmdtext =
              @"merge into Individual indv
                using BulkEtl_Person as pers
                    on indv.Id = pers.Id
                when matched then
                    update set
                        indv.Name = pers.FirstName + ' ' + pers.LastName,
                        indv.Sex = pers.Gender
                when not matched by target then
                    insert (Id, Name, Sex)
                    values (pers.Id, pers.FirstName + ' ' + pers.LastName, pers.Gender);";
            using (var con = new SqlConnection(_connstring))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = cmdtext;
                cmd.ExecuteNonQuery();
            }
        }

        private void LoadActivityTarget()
        {
            const string cmdtext =
              @"merge into Activity act
                using BulkEtl_Hobby as hob
                    on act.HobbyId = hob.Id
                when matched then
                    update set
                        act.Name = hob.Name
                when not matched by target then
                    insert (Name, HobbyId)
                    values (hob.Name, hob.Id);";
            using (var con = new SqlConnection(_connstring))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = cmdtext;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
