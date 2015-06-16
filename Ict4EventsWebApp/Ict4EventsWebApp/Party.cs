using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ict4EventsWebApp
{
    /// <summary>
    /// People are vessels.
    /// This is the ocean.
    /// - J. Sanders 2015
    /// </summary>
    public class Party
    {

        public List<Person> Members { get; set; }

        public Party()
        {
            Members = new List<Person>();
        }


        public void AddMember(Person member)
        {
            Members.Add(member);
        }
    }
}