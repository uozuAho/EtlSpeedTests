namespace EtlSpeedTests
{
    class Program
    {
        static void Main(string[] args)
        {
            CsvGenerator.CreateFiles();

            // push data into target tables, ensuring correctness
            // tables:
            // individual (person)
            // activity (hobby)
            // properties (for individual and activity)
        }
    }
}
