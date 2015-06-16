using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ict4EventsWebApp
{
    public class Party
    {
        public Person Leader { get; set; }
        public List<Person> Members { get; set; }

        public Party(Person leader)
        {
            this.Leader = leader;
        }

        public void AddMember(Person member)
        {
            Members.Add(member);
        }
    }
}