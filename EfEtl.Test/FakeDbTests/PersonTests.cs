using EfEtl.Models;
using EfEtl.Test.Fakes;
using Etl.Data.Input;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EfEtl.Test.FakeDbTests
{
    [TestFixture]
    public class PersonTests
    {
        [Test]
        public void asdf()
        {
            var people = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Address = "1",
                    FirstName = "a",
                    LastName = "b",
                    Gender = "M",
                    HobbyId = 1,
                    Ph = "1234"
                }
            };

            var db = new FakeEtlSpeedTestsContext();
            db.PropertyTypes.AddRange(new List<PropertyType>
            {
                new PropertyType { Value = "Address" },
                new PropertyType { Value = "Ph." },
                new PropertyType { Value = "Hobby name" },
                new PropertyType { Value = "Hobby Id" },
                new PropertyType { Value = "Hobby Type" }
            });

            var efetl = new EfEtlTool(people, new List<Hobby>(), db);
            efetl.Run();

            // TODO: asserts
        }
    }
}
