using CsvHelper;
using System.Collections.Generic;
using System.IO;

namespace Etl
{
    public class DataGenerator
    {
        const string Dir = "C:\\temp";
        public const string PersonFilepath = Dir + "\\Person.csv";
        public const string HobbyFilepath = Dir + "\\Hobby.csv";

        public static void CreateFiles()
        {
            DeleteExistingFiles();
            CreatePersonFile(PersonFilepath, 5);
            CreateHobbyFile(HobbyFilepath, 5);
        }

        private static void DeleteExistingFiles()
        {
            var paths = new List<string>
            {
                PersonFilepath,
                HobbyFilepath
            };
            foreach (var path in paths)
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        private static void CreatePersonFile(string path, int rows)
        {
            using (var writer = new CsvWriter(new StreamWriter(path)))
            {
                writer.WriteRecords(CreatePersonRecords(rows));
            }
        }

        public static IEnumerable<Person> CreatePersonRecords(int num)
        {
            for (var i = 0; i < num; i++)
            {
                // TODO: bad data
                yield return new Person
                {
                    Id = i,
                    FirstName = "FirstName " + i,
                    LastName = "LastName" + i,  // TODO random string
                    Gender = i % 2 == 0 ? "M" : "F",
                    Address = "Address " + i,
                    Ph = "Phone " + i, // TODO: random numeric string
                    HobbyId = i // TODO: link to valid hobby
                };
            }
        }

        private static void CreateHobbyFile(string path, int rows)
        {
            using (var writer = new CsvWriter(new StreamWriter(path)))
            {
                writer.WriteRecords(CreateHobbyRecords(rows));
            }
        }

        public static IEnumerable<Hobby> CreateHobbyRecords(int num)
        {
            for (var i = 0; i < num; i++)
            {
                yield return new Hobby
                {
                    Id = i,
                    Name = "Hobby " + i,
                    Type = "Type " + i
                };
            }
        }
    }
}
