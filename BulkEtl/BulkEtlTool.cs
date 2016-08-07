using BulkEtl.InputLoader;
using Etl;
using Etl.Data.Input;
using System.Collections.Generic;
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
            PersonLoader.Load(_people, _connstring);
            HobbyLoader.Load(_hobbies, _connstring);
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
