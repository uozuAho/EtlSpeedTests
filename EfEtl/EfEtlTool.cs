using EfEtl.BusinessLayer;
using EfEtl.Models;
using Etl;
using Etl.Data.Input;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace EfEtl
{
    public class EfEtlTool : IEtl, IDisposable
    {
        private readonly IEnumerable<Person> _people;
        private readonly IEnumerable<Hobby> _hobbies;

        private readonly EtlSpeedTestsEntities _db;

        public EfEtlTool(IEnumerable<Person> people, IEnumerable<Hobby> hobbies)
        {
            _people = people;
            _hobbies = hobbies;
            _db = new EtlSpeedTestsEntities();
            // 
            TargetDbData.Initialise(_db);
        }

        public void Run()
        {
            LoadInputTables();
            LoadTarget();
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
            EfEtl_Person person;
            while ((person = _db.EfEtl_Person.FirstOrDefault(p => p.ProcessingState == 0)) != null)
            {
                _db.Individuals.AddOrUpdate(PersonToIndividual.NewIndividual(person));
                foreach (var property in PersonToIndividual.GetIndividualProperties(person))
                {
                    _db.Properties.AddOrUpdate(property);
                }
                person.ProcessingState = (int) ProcessingState.Processed;
                _db.SaveChanges();
            }
        }
    }
}
