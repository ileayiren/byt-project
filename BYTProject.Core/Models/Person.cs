using System;
using System.Collections.Generic;
using System.IO;

namespace BYTProject.Core.Models
{
    public abstract class Person
    {
        // -----------------------------
        // STATIC ATTRIBUTE (UML)
        // -----------------------------
        public static string SchoolName { get; private set; } = "pjatk";

        public static void SetSchoolName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("School name cannot be empty.");
            SchoolName = newName;
        }

        // -----------------------------
        // INSTANCE ATTRIBUTES
        // -----------------------------
        private string _name;
        private string? _middleName;
        private string _surname;
        private string _email;
        private DateTime _birthDate; // /age derived from this

        // -----------------------------
        // PROPERTIES
        // -----------------------------
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                _name = value;
            }
        }

        public string? MiddleName
        {
            get => _middleName;
            set
            {
                if (value != null && value.Trim() == "")
                    throw new ArgumentException("Middle name cannot be empty string.");
                _middleName = value;
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Surname cannot be empty.");
                _surname = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email cannot be empty.");
                _email = value;
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Birth date cannot be in the future.");
                _birthDate = value;
            }
        }

        // -----------------------------
        // DERIVED ATTRIBUTE: /age
        // -----------------------------
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        // -----------------------------
        // METHODS (UML)
        // -----------------------------
        public string GetFullName()
        {
            return MiddleName == null
                ? $"{Name} {Surname}"
                : $"{Name} {MiddleName} {Surname}";
        }

        public string GetEmail() => Email;

        // -----------------------------
        // CONSTRUCTORS
        // -----------------------------
        protected Person() { }

        protected Person(string name, string surname, DateTime birthDate, string email)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Email = email;

            AddToExtent(this);
        }

        // -----------------------------
        // EXTENT
        // -----------------------------
        private static List<Person> _extent = new();

        private static void AddToExtent(Person p)
        {
            if (p == null)
                throw new ArgumentException("Person cannot be null.");

            _extent.Add(p);
        }

        public static IReadOnlyList<Person> GetExtent() => _extent.AsReadOnly();

        // -----------------------------
        // PERSISTENCY (TXT - NO LIBRARIES)
        // -----------------------------
        public static void Save(string path = "persons.txt")
        {
            var lines = new List<string>();

            foreach (var p in _extent)
            {
                string line = $"{p.Name};{p.MiddleName};{p.Surname};{p.Email};{p.BirthDate:yyyy-MM-dd}";
                lines.Add(line);
            }

            File.WriteAllLines(path, lines);
        }

        public static void Load(string path = "persons.txt")
        {
            _extent = new List<Person>();

            if (!File.Exists(path))
                return;

            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                var parts = line.Split(';');

                string name = parts[0];
                string? middle = parts[1] == "" ? null : parts[1];
                string surname = parts[2];
                string email = parts[3];
                DateTime birth = DateTime.Parse(parts[4]);

                // Person abstract olduğu için bir concrete class gerekecek.
                // Bu generic load sadece subclass üzerinden kullanılacak.
            }
        }
    }
}
