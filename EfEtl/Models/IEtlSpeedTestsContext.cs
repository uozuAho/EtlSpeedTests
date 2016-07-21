using System.Data.Entity;

namespace EfEtl.Models
{
    interface IEtlSpeedTestsContext
    {
        IDbSet<Activity> Activities { get; }
        IDbSet<EfEtl_Hobby> EfEtl_Hobby { get; }
        IDbSet<EfEtl_Person> EfEtl_Person { get; }
        IDbSet<Individual> Individuals { get; }
        IDbSet<IndividualActivity> IndividualActivities { get; }
        IDbSet<Property> Properties { get; }
        IDbSet<PropertyType> PropertyTypes { get; }

        int SaveChanges();
    }
}
