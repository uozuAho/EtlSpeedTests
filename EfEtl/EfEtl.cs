using Etl;
using System.Collections.Generic;

namespace EfEtl
{
    public class EfEtl : IEtl
    {
        private IEnumerable<Person> _people;
        private IEnumerable<Hobby> _hobbies;
        private string _targetDbConnstring;

        public EfEtl(IEnumerable<Person> people, IEnumerable<Hobby> hobbies, string targetDbConnString)
        {
            _people = people;
            _hobbies = hobbies;
            _targetDbConnstring = targetDbConnString;
        }

        public void Run()
        {

        }
    }
}
