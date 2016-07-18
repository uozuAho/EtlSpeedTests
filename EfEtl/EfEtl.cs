using EfEtl.Models;
using Etl;
using Etl.Data.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EfEtl
{
    public class EfEtl : IEtl, IDisposable
    {
        private readonly IEnumerable<Person> _people;
        private readonly IEnumerable<Hobby> _hobbies;
        private readonly string _targetDbConnstring;

        private readonly EtlSpeedTestsEntities _db;

        public EfEtl(IEnumerable<Person> people, IEnumerable<Hobby> hobbies, string targetDbConnString)
        {
            _people = people;
            _hobbies = hobbies;
            _targetDbConnstring = targetDbConnString;
            _db = new EtlSpeedTestsEntities(targetDbConnString);
        }

        public void Run()
        {
            LoadInputTables();
        }

        public void Dispose()
        {
            if (_db != null)
                _db.Dispose();
        }

        private void LoadInputTables()
        {
            foreach (var person in _people)
            {
                _db.EfEtl_Person.Add(EfEtl_Person.New(person));
            }
            _db.SaveChanges();
        }

        private void LoadTarget()
        {
            //foreach (var person in _db.EfEtl_Person.Where(p => p.is))
            //{

            //}
        }
    }
}
