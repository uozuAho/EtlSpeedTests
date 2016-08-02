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
    /// <summary>
    /// ETL application implemented using Entity Framework (EF)
    /// </summary>
    public class EfEtlTool : IEtl, IDisposable
    {
        private readonly IEnumerable<Person> _people;
        private readonly IEnumerable<Hobby> _hobbies;

        private readonly IEtlSpeedTestsContext _db;

        public EfEtlTool(IEnumerable<Person> people, IEnumerable<Hobby> hobbies) 
            : this(people, hobbies, new EtlSpeedTestsEntities())
        {
        }

        public EfEtlTool(IEnumerable<Person> people, IEnumerable<Hobby> hobbies, IEtlSpeedTestsContext context)
        {
            _people = people;
            _hobbies = hobbies;
            _db = context;
            TargetDbData.Initialise(context);
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
            LoadPersonInputTable();
            LoadHobbyInputTable();
        }

        private void LoadPersonInputTable()
        {
            foreach (var person in _people)
            {
                _db.EfEtl_Person.Add(EfEtl_Person.New(person));
            }
            _db.SaveChanges();
        }

        private void LoadHobbyInputTable()
        {
            foreach (var hobby in _hobbies)
            {
                _db.EfEtl_Hobby.Add(EfEtl_Hobby.New(hobby));
            }
            _db.SaveChanges();
        }

        private void LoadTarget()
        {
            LoadIndividualTarget();
        }

        private void LoadIndividualTarget()
        {
            EfEtl_Person person;
            while ((person = _db.EfEtl_Person.FirstOrDefault(p => p.ProcessingState == 0)) != null)
            {
                _db.Individuals.AddOrUpdate(PersonToIndividual.NewIndividual(person));
                foreach (var property in PersonToIndividual.GetIndividualProperties(person))
                {
                    _db.Properties.AddOrUpdate(property);
                }
                person.ProcessingState = (int)ProcessingState.Processed;
                _db.SaveChanges();
            }
        }

        private void LoadActivityTarget()
        {
            EfEtl_Hobby hobby;
            while ((hobby = _db.EfEtl_Hobby.FirstOrDefault(p => p.ProcessingState == 0)) != null)
            {
                var existing = _db.Activities.SingleOrDefault(a => a.HobbyId == hobby.Id);
                var act = HobbyToActivity.NewActivity(hobby);
                if (existing == null)
                {
                    _db.Activities.Add(act);
                    _db.SaveChanges();
                    existing = act;
                }
                else
                    HobbyToActivity.UpdateExisting(existing, act);
                foreach (var property in HobbyToActivity.GetActivityProperties(hobby, existing.Id))
                {
                    _db.Properties.AddOrUpdate(property);
                }
                hobby.ProcessingState = (int)ProcessingState.Processed;
                _db.SaveChanges();
            }
        }
    }
}
