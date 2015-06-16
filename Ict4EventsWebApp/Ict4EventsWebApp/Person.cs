using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ict4EventsWebApp
{
    /// <summary>
    /// This class contains all required data to create a party member.
    /// </summary>
    public class Person
    {
        
        public string Name { get; set; }
        public string Infix { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="infix"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        public Person(string name, string infix, string surname, string email)
        {
            this.Name = name;
            this.Infix = infix;
            this.Surname = surname;
            this.Email = email;
        }

        /// <summary>
        /// Return person's full name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + " " + Surname;
        }
    }
}