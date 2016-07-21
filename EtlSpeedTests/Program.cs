using Etl;

namespace EtlSpeedTests
{
    class Program
    {
        const int NumPeople = 5;
        const int NumHobbies = 1;
        const string TargetDbConnString = @"Data Source=localhost\sqlexpress2014;Initial Catalog=EtlSpeedTests; Integrated Security=SSPI;";

        static void Main(string[] args)
        {
            // efetl tool's connstring is in the etl's app.config
            var efEtl = new EfEtl.EfEtlTool(
                DataGenerator.CreatePersonRecords(NumPeople), 
                DataGenerator.CreateHobbyRecords(NumHobbies));
            efEtl.Run();
        }
    }
}
