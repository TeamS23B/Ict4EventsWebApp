using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ict4EventsWebApp
{
    public class Person
    {
        public int IsLeader { get; set; }
        public string Name { get; set; }
        public string Infix { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string HouseNr { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        public Person (string name, string infix, string surname, string email, string street, string houseNr, string postalCode, string city)
        {
            this.IsLeader = 1;
            this.Name = name;
            this.Infix = infix;
            this.Surname = surname;
            this.Email = email;
            this.Street = street;
            this.HouseNr = houseNr;
            this.PostalCode = postalCode;
            this.City = city;
        }

        public Person(string name, string infix, string surname, string email)
        {
            this.IsLeader = 0;
            this.Name = name;
            this.Infix = infix;
            this.Surname = surname;
            this.Email = email;
        }
    }
}