using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ict4EventsWebApp
{
    public class Person
    {
        
        public string Name { get; set; }
        public string Infix { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        
        public Person(string name, string infix, string surname, string email)
        {
            this.Name = name;
            this.Infix = infix;
            this.Surname = surname;
            this.Email = email;
        }
    }
}