using EfEtl.Models;
using System.Data.Entity;

namespace EfEtl.Test.Fakes
{
    class FakeEtlSpeedTestsContext
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

        public IDbSet<Activity> Activities { get; set; }
        public IDbSet<EfEtl_Hobby> EfEtl_Hobby { get; set; }
        public IDbSet<EfEtl_Person> EfEtl_Person { get; set; }
        public IDbSet<Individual> Individuals { get; set; }
        public IDbSet<IndividualActivity> IndividualActivities { get; set; }
        public IDbSet<Property> Properties { get; set; }
        public IDbSet<PropertyType> PropertyTypes { get; set; }

        public int SaveChanges()
        {
            return 0;
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

    }

    public class FakeIndividualActivitySet : FakeDbSet<IndividualActivity>
    {

    }

    public class FakePropertySet : FakeDbSet<Property>
    {

    }

    public class FakePropertyTypeSet : FakeDbSet<PropertyType>
    {

    }
}
