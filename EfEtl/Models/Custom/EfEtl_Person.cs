using Etl.Data.Input;

namespace EfEtl.Models
{
    public partial class EfEtl_Person
    {
        public static EfEtl_Person New(Person person)
        {
            return new EfEtl_Person
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Gender = person.Gender,
                Address = person.Address,
                Ph = person.Ph,
                HobbyId = person.HobbyId
            };
        }
    }
}
