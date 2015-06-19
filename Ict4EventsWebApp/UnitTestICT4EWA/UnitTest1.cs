using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ict4EventsWebApp;
using System.Collections.Generic;

namespace UnitTestICT4EWA
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreatePerson()
        {
            Person person = new Person("Jim", null, "Sanders", "jimsanders11@gmail.com");
            string personToString = person.ToString();

            Assert.AreEqual(personToString, person.ToString());
        }

        [TestMethod]
        public void CreateParty()
        {
            Party party = new Ict4EventsWebApp.Party();

            Person person1 = new Person("Jim", null, "Sanders", "jimsanders11@gmail.com");
            Person person2 = new Person("Jimbo", null, "Sandersson", "jimsandersson112@yahoo.com");

            party.AddMember(person1);
            party.AddMember(person2);

            List<Person> persons = new List<Person>();
            persons.Add(person1);
            persons.Add(person2);

            

            Assert.AreEqual(persons[0], party.Members[0]);
            Assert.AreEqual(persons[1], party.Members[1]);

            
        }

        [TestMethod]
        public void TestAuthentication()
        {
            var inst = Authentication.Instance;//get the instance
            Assert.IsTrue(inst.IsAuthenticated("Administrator","Administrator"));//test login
            try
            {
                var result = inst.IsAuthenticated("Administrator", "aapje");
                if (result)
                    Assert.Fail("Dit not fail and true Administrator aapje");
                
            }
            catch (Exception)
            {
                
            }

            try
            {
                var result = inst.IsAuthenticated("aapje", "aapje"); //should throw
                if(result)
                    Assert.Fail("Did not fail and true aapje aapje");
            }
            catch (Exception)
            {
                
            }
            
        }
        [TestMethod]
        public void TestSmsConnect()
        {
            var inst = SmsConnect.Instance;
            var token = inst.AddToken("aapje");
            Assert.IsFalse(string.IsNullOrEmpty(token));//check token

            Assert.IsTrue(inst.CheckUser("aapje", token) == 1);//check if user is logged in

            inst.RemoveToken("aapje");

            token = inst.AddToken("aapje", true);

            Assert.IsTrue(inst.CheckUser("aapje", token) == 2);//check high autherized

            inst.RemoveToken("aapje");
        }
    }
}
