using System;
using System.Data.Entity;

namespace EfEtl.Models
{
    public interface IEtlSpeedTestsContext : IDisposable
    {
        DbSet<Activity> Activities { get; }
        DbSet<EfEtl_Hobby> EfEtl_Hobby { get; }
        DbSet<EfEtl_Person> EfEtl_Person { get; }
        DbSet<Individual> Individuals { get; }
        DbSet<IndividualActivity> IndividualActivities { get; }
        DbSet<Property> Properties { get; }
        DbSet<PropertyType> PropertyTypes { get; }

        int SaveChanges();
    }
}
