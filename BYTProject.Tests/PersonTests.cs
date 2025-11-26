using NUnit.Framework;
using BYTProject.Core.Models;
using System;

namespace BYTProject.Tests
{
    public class PersonTests
    {
        // -----------------------------------------
        // Helper concrete class (Person is abstract)
        // -----------------------------------------
        private class TestPerson : Person
        {
            public TestPerson(string name, string surname, DateTime birthDate, string email)
                : base(name, surname, birthDate, email) { }
        }

        // -----------------------------------------
        // TEST 1: Name cannot be empty
        // -----------------------------------------
        [Test]
        public void Person_NameEmpty_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var p = new TestPerson("", "Doe", DateTime.Now.AddYears(-20), "test@mail.com");
            });
        }

        // -----------------------------------------
        // TEST 2: BirthDate cannot be in the future
        // -----------------------------------------
        [Test]
        public void Person_FutureBirthDate_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var p = new TestPerson("John", "Doe", DateTime.Now.AddDays(3), "test@mail.com");
            });
        }

        // -----------------------------------------
        // TEST 3: GetFullName works correctly
        // -----------------------------------------
        [Test]
        public void Person_GetFullName_ReturnsCorrectString()
        {
            var birth = DateTime.Now.AddYears(-25);

            var p = new TestPerson("John", "Doe", birth, "test@mail.com");

            Assert.AreEqual("John Doe", p.GetFullName());
        }
    }
}