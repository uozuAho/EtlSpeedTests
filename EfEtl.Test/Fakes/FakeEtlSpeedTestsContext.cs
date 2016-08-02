using EfEtl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EfEtl.Test.Fakes
{
    class FakeEtlSpeedTestsContext : IEtlSpeedTestsContext
    {
        public FakeEtlSpeedTestsContext()
        {
            Activities = new FakeActivitySet();
            EfEtl_Hobby = new FakeEfEtl_HobbySet();
            EfEtl_Person = new FakeEfEtl_PersonSet();
            Individuals = new FakeIndividualSet();
            IndividualActivities = new FakeIndividualActivitySet();
            Properties = new FakePropertySet();
            PropertyTypes = new FakePropertyTypeSet();
        }

        public static FakeEtlSpeedTestsContext NewWithPropertyTypes()
        {
            var context = new FakeEtlSpeedTestsContext();
            context.PropertyTypes.AddRange(new List<PropertyType>
            {
                new PropertyType { Value = "Address" },
                new PropertyType { Value = "Ph." },
                new PropertyType { Value = "Hobby name" },
                new PropertyType { Value = "Hobby Id" },
                new PropertyType { Value = "Hobby Type" }
            });
            return context;
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<EfEtl_Hobby> EfEtl_Hobby { get; set; }
        public DbSet<EfEtl_Person> EfEtl_Person { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<IndividualActivity> IndividualActivities { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose()
        {
        }
    }

    public class FakeActivitySet : FakeDbSet<Activity>
    {

    }

    public class FakeEfEtl_HobbySet : FakeDbSet<EfEtl_Hobby>
    {

    }

    public class FakeEfEtl_PersonSet : FakeDbSet<EfEtl_Person>
    {

    }

    public class FakeIndividualSet : FakeDbSet<Individual>
    {
        public override Individual Find(params object[] keyValues)
        {
            var id = (int)keyValues.Single();
            return this.SingleOrDefault(i => i.Id == id);
        }

        public void AddOrUpdate(params Individual[] individuals)
        {
            if (individuals.Length > 1)
                throw new NotImplementedException("I'm lazy");
            var indv = individuals.Single();
            var existing = Find(indv.Id);
            if (existing == null)
                Add(indv);
            else
            {
                existing.Name = indv.Name;
                existing.Sex = indv.Sex;
            }
        }
    }

    public class FakeIndividualActivitySet : FakeDbSet<IndividualActivity>
    {
    }

    public class FakePropertySet : FakeDbSet<Property>
    {
        public override Property Find(params object[] keyValues)
        {
            var id = (int)keyValues.Single();
            return this.SingleOrDefault(i => i.Id == id);
        }

        public void AddOrUpdate(params Property[] properties)
        {
            if (properties.Length > 1)
                throw new NotImplementedException("I'm lazy");
            var prop = properties.Single();
            var existing = Find(prop.Id);
            if (existing == null)
                Add(prop);
            else
            {
                existing.ActivityId = prop.ActivityId;
                existing.IndividualId = prop.IndividualId;
                existing.PropertyTypeId = prop.PropertyTypeId;
                existing.Value = prop.Value;
            }
        }
    }

    public class FakePropertyTypeSet : FakeDbSet<PropertyType>
    {

    }
}
