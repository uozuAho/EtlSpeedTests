using BulkEtl;
using EfEtl.Test;
using Etl;
using System;
using System.Diagnostics;

namespace EtlSpeedTests
{
    class Program
    {
        const int NumPeople = 100;
        const int NumHobbies = 50;
        const string TargetDbConnString = @"Data Source=localhost\sqlexpress2014;Initial Catalog=EtlSpeedTests; Integrated Security=SSPI;";

        static void Main(string[] args)
        {
            Console.WriteLine($"Etl speed tests. Num people: {NumPeople}, num hobbies: {NumHobbies}");
            // efetl tool's connstring is in the etl's app.config
            //RunImport("EF etl", new EfEtl.EfEtlTool(
            //    DataGenerator.CreatePersonRecords(NumPeople),
            //    DataGenerator.CreateHobbyRecords(NumHobbies)));

            RunImport("Bulk etl", new BulkEtlTool(
                DataGenerator.CreatePersonRecords(NumPeople),
                DataGenerator.CreateHobbyRecords(NumHobbies),
                TargetDbConnString));

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void RunImport(string name, IEtl etl)
        {
            Console.WriteLine($"{name}: clearing test db");
            TestDb.Clear();
            Console.WriteLine($"{name}: start");
            var sw = Stopwatch.StartNew();
            etl.Run();
            var ms = sw.ElapsedMilliseconds;
            Console.WriteLine($"{name}: stop. Time (ms): {ms}");
        }
    }
}
