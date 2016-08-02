using System.Data.SqlClient;

namespace EfEtl.Test
{
    public class TestDb
    {
        private const string ConnString = @"Data Source=localhost\sqlexpress2014;Initial Catalog=EtlSpeedTests; Integrated Security=SSPI;";

        /// <summary>
        /// Clear all but required data from the test db.
        /// </summary>
        public static void Clear()
        {
            using (var con = new SqlConnection(ConnString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText =
                   @"delete from Property;
                     delete from Individual;
                     delete from Activity;
                     truncate table IndividualActivity;
                     truncate table EfEtl_Person;
                     truncate table EfEtl_Hobby;";
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
