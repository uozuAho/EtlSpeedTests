using Etl.Data.Input;

namespace EfEtl.Models
{
    public partial class EfEtl_Hobby
    {
        public static EfEtl_Hobby New(Hobby hobby)
        {
            return new EfEtl_Hobby
            {
                Id = hobby.Id,
                Name = hobby.Name,
                Type = hobby.Type
            };
        }
    }
}
