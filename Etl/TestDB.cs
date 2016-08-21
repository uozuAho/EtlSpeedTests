using System.Data.SqlClient;

namespace EfEtl.Test
{
    public class TestDb
    {
        /// <summary>
        /// Clear all but required data from the test db.
        /// </summary>
        public static void Clear(string connstring)
        {
            using (var con = new SqlConnection(connstring))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText =
                   @"delete from Property;
                    truncate table IndividualActivity;
                    delete from Individual;
                    delete from Activity;
                    truncate table EfEtl_Person;
                    truncate table EfEtl_Hobby;
                    truncate table BulkEtl_Person;
                    truncate table BulkEtl_Hobby;
                    truncate table BulkEtl_Property;";
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
