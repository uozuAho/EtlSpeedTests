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
    /// ETL application implemented using Entity Framework (EF).
    /// 
    /// To update the EF model:
    /// - Try 'update from database' from menu after right clicking on the edmx diagram. This usually fails though.
    /// - Delete EtlSpeedTestsModel.edmx
    /// - Remove the connection string from App.config
    /// - Right click on Models directory, add new ADO.NET Entity model
    /// - Name EtlSpeedTestsModel
    /// - Continue with defaults
    /// - Once done, add the IEtlSpeedTestsContext interface to EtlSpeedTestsModel.Context.cs
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
            LoadActivityTarget();
            CreateIndividualActivityLinks();
        }

        private void LoadIndividualTarget()
        {
            EfEtl_Person person;
            while ((person = _db.EfEtl_Person.FirstOrDefault(p => p.ProcessingState == (int)ProcessingState.Default)) != null)
            {
                var indv = PersonToIndividual.NewIndividual(person);
                _db.Individuals.AddOrUpdate(indv);
                foreach (var property in PersonToIndividual.GetIndividualProperties(person))
                {
                    _db.Properties.AddOrUpdate(property);
                }
                person.ProcessingState = (int)ProcessingState.InsertedToTarget;
                _db.SaveChanges();
            }
        }

        private void LoadActivityTarget()
        {
            EfEtl_Hobby hobby;
            while ((hobby = _db.EfEtl_Hobby.FirstOrDefault(p => p.ProcessingState == (int)ProcessingState.Default)) != null)
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
                hobby.ProcessingState = (int)ProcessingState.InsertedToTarget;
                _db.SaveChanges();
            }
        }

        private void CreateIndividualActivityLinks()
        {
            EfEtl_Person person;
            while ((person = _db.EfEtl_Person.FirstOrDefault(p => p.ProcessingState == (int)ProcessingState.InsertedToTarget)) != null)
            {
                var act = _db.Activities.Where(a => a.HobbyId == person.HobbyId).SingleOrDefault();
                if (act != null)
                {
                    var existing = _db.IndividualActivities
                        .Where(ia => ia.ActivityId == act.Id && ia.IndividualId == person.Id)
                        .SingleOrDefault();
                    if (existing == null)
                        _db.IndividualActivities.Add(new IndividualActivity { ActivityId = act.Id, IndividualId = person.Id.Value });
                }
                person.ProcessingState = (int)ProcessingState.ActivityLinkInserted;
                _db.SaveChanges();
            }
        }
    }
}
