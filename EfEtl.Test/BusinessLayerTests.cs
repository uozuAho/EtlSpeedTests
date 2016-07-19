using EfEtl.BusinessLayer;
using EfEtl.Models;
using NUnit.Framework;

namespace EfEtl.Test
{
    [TestFixture]
    class BusinessLayerTests
    {
        [Test]
        public void DefaultPerson()
        {
            var person = GetDefaultTestPerson();
            var indv = PersonToIndividual.NewIndividual(person);
            Assert.AreEqual(1, indv.Id);
            Assert.AreEqual("a b", indv.Name);
            Assert.AreEqual("M", indv.Sex);
        }

        private static EfEtl_Person GetDefaultTestPerson()
        {
            return new EfEtl_Person
            {
                Id = 1,
                FirstName = "a",
                LastName = "b",
                Gender = "M",
                Address = "1",
                Ph = "1234",
                HobbyId = 1
            };
        }
    }
}
