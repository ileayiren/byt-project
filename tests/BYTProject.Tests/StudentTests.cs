using NUnit.Framework;
using BYTProject.Models;
using System;
using System.IO;
using System.Collections.Generic;

namespace BYTProject.Tests
{
    [TestFixture]
    public class StudentTests
    {
        // Her test öncesi student extent sıfırlama
        [SetUp]
        public void Setup()
        {
            typeof(Student)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Student>());
        }

        // ============================================================
        // HELPERS
        // ============================================================
        private Student CreateValidStudent()
        {
            return new Student(
                "John",
                "Doe",
                new DateTime(2000, 1, 1),
                "john@example.com",
                123,          // studentNumber
                200,          // balance
                3,            // yearOfStudy
                2.5,          // GPA
                2,            // semester
                90            // attendance
            );
        }

        // ============================================================
        // CONSTRUCTOR TEST
        // ============================================================
        [Test]
        public void Constructor_ShouldCreateStudent_WhenValidData()
        {
            var s = CreateValidStudent();

            Assert.That(s.StudentNumber, Is.EqualTo(123));
            Assert.That(s.YearOfStudy, Is.EqualTo(3));
            Assert.That(s.GPA, Is.EqualTo(2.5));
            Assert.That(s.CurrentSemester, Is.EqualTo(2));
            Assert.That(s.Attendance, Is.EqualTo(90));

            Assert.That(s.Name, Is.EqualTo("John"));
            Assert.That(s.Surname, Is.EqualTo("Doe"));
            Assert.That(s.Email, Is.EqualTo("john@example.com"));
        }

        // ============================================================
        // VALIDATION TESTS
        // ============================================================
        [Test]
        public void StudentNumber_ShouldThrow_WhenInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Student("A", "B", DateTime.Today, "a@b.com", -5, 100, 1, 2.0, 1, 80);
            });
        }

        [Test]
        public void GPA_ShouldThrow_WhenOutOfRange()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Student("A", "B", DateTime.Today, "a@b.com", 100, 100, 1, 5.0, 1, 80);
            });
        }

        [Test]
        public void YearOfStudy_ShouldThrow_WhenInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Student("A", "B", DateTime.Today, "a@b.com", 100, 100, 0, 2.0, 1, 80);
            });
        }

        [Test]
        public void Attendance_ShouldThrow_WhenInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Student("A", "B", DateTime.Today, "a@b.com", 100, 100, 3, 2.0, 1, -10);
            });
        }

        // ============================================================
        // EXTENT TESTS
        // ============================================================
        [Test]
        public void Constructor_ShouldAddStudentToExtent()
        {
            var s = CreateValidStudent();
            Assert.That(Student.Extent.Count, Is.EqualTo(1));
        }

        [Test]
        public void Extent_ShouldBeEncapsulated()
        {
            var s = CreateValidStudent();
            var extent = Student.Extent;

            Assert.Throws<NotSupportedException>(() =>
            {
                ((ICollection<Student>)extent).Add(CreateValidStudent());
            });
        }

        // ============================================================
        // INHERITANCE TEST
        // ============================================================
        [Test]
        public void Student_ShouldBeAssignableToPerson()
        {
            Person p = CreateValidStudent();
            Assert.That(p.GetFullName(), Is.EqualTo("John Doe"));
        }

        // ============================================================
        // PERSISTENCY TESTS (TXT)
        // ============================================================
        [Test]
        public void SaveAndLoadExtent_ShouldWorkCorrectly()
        {
            string path = "students_test.txt";

            // Bir student oluştur
            var s = CreateValidStudent();

            // TXT Olarak yaz
            Student.Save(path);

            // Extenti sıfırla
            typeof(Student)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Student>());

            Assert.That(Student.Extent.Count, Is.EqualTo(0));

            // TXT’den geri yükle
            Student.Load(path);

            Assert.That(Student.Extent.Count, Is.EqualTo(1));
            var loaded = Student.Extent[0];

            Assert.That(loaded.StudentNumber, Is.EqualTo(123));
            Assert.That(loaded.GPA, Is.EqualTo(2.5));
            Assert.That(loaded.YearOfStudy, Is.EqualTo(3));
            Assert.That(loaded.Name, Is.EqualTo("John"));

            File.Delete(path);
        }
    }
}
