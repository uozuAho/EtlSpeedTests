using Etl.Data.Input;
using NUnit.Framework;
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

            var efetl = new EfEtlTool(people, new List<Hobby>());
            efetl.Run();
        }
    }
}
