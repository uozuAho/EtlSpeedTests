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
            // TODO: LoadIndividualActivityTarget
            LoadIndividualPropertyTarget();
            LoadActivityPropertyTarget();
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

        private void LoadIndividualPropertyTarget()
        {
            const string cmdtext =
              @"with indvProp as (
                    select
                        pt.Id as PropertyTypeId,
                        bp.PersonId as IndividualId,
                        bp.Value as Value
                    from BulkEtl_Property bp
                    join PropertyType pt on bp.PropertyType = pt.Value
                    where bp.PersonId is not null
                )
                merge into Property dest
                using indvProp as source
                    on  source.IndividualId = dest.IndividualId
                    and source.PropertyTypeId = dest.PropertyTypeId
                when matched then
                    update set dest.Value = source.Value
                when not matched by target then
                    insert (IndividualId, PropertyTypeId, Value)
	                values (source.IndividualId, source.PropertyTypeId, source.Value);";
            using (var con = new SqlConnection(_connstring))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = cmdtext;
                cmd.ExecuteNonQuery();
            }
        }

        private void LoadActivityPropertyTarget()
        {
            const string cmdtext =
                @"with hobbyProp as (
                    select
                        pt.Id as PropertyTypeId,
                        act.Id as ActivityId,
                        bp.Value as Value
                    from BulkEtl_Property bp
                    join PropertyType pt on bp.PropertyType = pt.Value
	                join Activity act on act.HobbyId = bp.HobbyId
                )
                merge into Property dest
                using hobbyProp as source
                    on  source.ActivityId = dest.ActivityId
                    and source.PropertyTypeId = dest.PropertyTypeId
                when matched then
                    update set dest.Value = source.Value
                when not matched by target then
                    insert (ActivityId, PropertyTypeId, Value)
	                values (source.ActivityId, source.PropertyTypeId, source.Value);";
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
