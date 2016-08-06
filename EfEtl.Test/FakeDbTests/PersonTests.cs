using EfEtl.Test.Fakes;
using Etl.Data.Input;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace EfEtl.Test.FakeDbTests
{
    [TestFixture]
    public class PersonTests
    {
        [Test]
        public void OnePersonOneHobbyFullRun()
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

            var hobbies = new List<Hobby>
            {
                new Hobby
                {
                    Id = 1,
                    Name = "hob",
                    Type = "sport"
                }
            };

            var db = FakeEtlSpeedTestsContext.NewWithPropertyTypes();

            var efetl = new EfEtlTool(people, hobbies, db);
            efetl.Run();

            var indv = db.Individuals.Single();
            Assert.AreEqual(1, indv.Id);
            Assert.AreEqual("a b", indv.Name);
            Assert.AreEqual("M", indv.Sex); 

            var act = db.Activities.Single();
            // Assert.AreEqual(1, act.Id); ??
            Assert.AreEqual("hob", act.Name);
            Assert.AreEqual(1, act.HobbyId);
        }
    }
}
