using EfEtl.Models;
using System;
using System.Collections.Generic;

namespace EfEtl.BusinessLayer
{
    public class PersonToIndividual
    {
        public static Individual NewIndividual(EfEtl_Person person)
        {
            return new Individual
            {
                Id = person.Id == null ? 0 : person.Id.Value,
                Name = person.FirstName + ' ' + person.LastName,
                Sex = GetSex(person.Gender)
            };
        }

        public static IEnumerable<Property> GetIndividualProperties(EfEtl_Person person)
        {
            if (person.Id == null)
                throw new ArgumentException("Person must have id for properties to be created");
            var targetData = TargetDbData.GetInstance();
            yield return new Property
            {
                IndividualId = person.Id,
                PropertyTypeId = targetData.PropertyTypes["Address"].Id,
                Value = person.Address
            };
            yield return new Property
            {
                IndividualId = person.Id,
                PropertyTypeId = targetData.PropertyTypes["Ph."].Id,
                Value = person.Ph
            };
        }

        private static string GetSex(string gender)
        {
            switch (gender)
            {
                case "M":
                case "Male":
                    return "M";
                case "F":
                case "Female":
                    return "F";
                default:
                    throw new ArgumentException("Unhandled gender " + gender);
            }
        }
    }
}
