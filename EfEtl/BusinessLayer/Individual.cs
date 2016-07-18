using EfEtl.Models;
using System;

namespace EfEtl.BusinessLayer
{
    // TODO: this is probably the wrong name
    public class Individual
    {
        public static Models.Individual NewIndividual(EfEtl_Person person)
        {
            return new Models.Individual
            {
                Name = person.FirstName + ' ' + person.LastName,
                Sex = GetSex(person.Gender)
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
