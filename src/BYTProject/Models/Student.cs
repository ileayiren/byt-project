using System;
using System.Collections.Generic;
using System.IO;
using BYTProject.Core.Models;   // Person için namespace

namespace BYTProject.Models
{
    public class Student : Person
    {
        // -----------------------------
        // STATIC EXTENT (UML)
        // -----------------------------
        private static List<Student> _extent = new();

        public static IReadOnlyList<Student> Extent => _extent.AsReadOnly();

        private static void AddToExtent(Student s)
        {
            if (s == null)
                throw new ArgumentException("Student cannot be null.");

            _extent.Add(s);
        }

        // -----------------------------
        // INSTANCE ATTRIBUTES (UML)
        // -----------------------------
        private int _studentNumber;
        private decimal _accountBalance;
        private int _yearOfStudy;
        private double _gpa;
        private int _currentSemester;
        private int _attendance;

        // -----------------------------
        // PROPERTIES (VALIDATION)
        // -----------------------------
        public int StudentNumber
        {
            get => _studentNumber;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Student number must be > 0.");
                _studentNumber = value;
            }
        }

        public decimal AccountBalance
        {
            get => _accountBalance;
            set => _accountBalance = value;
        }

        public int YearOfStudy
        {
            get => _yearOfStudy;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentException("Year of study must be 1-5.");
                _yearOfStudy = value;
            }
        }

        public double GPA
        {
            get => _gpa;
            set
            {
                if (value < 0 || value > 4.0)
                    throw new ArgumentException("GPA must be 0–4.0.");
                _gpa = value;
            }
        }

        public int CurrentSemester
        {
            get => _currentSemester;
            set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentException("Semester must be 1–10.");
                _currentSemester = value;
            }
        }

        public int Attendance
        {
            get => _attendance;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Attendance must be 0–100.");
                _attendance = value;
            }
        }

        // -----------------------------
        // METHODS (UML)
        // -----------------------------
        public void ViewAvailableCourse()
        {
            // Placeholder
        }

        public void ViewInvoiceAndPayment()
        {
            // Placeholder
        }

        public void ApplyForScholarship()
        {
            // Placeholder
        }

        public void EnrollInCourse()
        {
            // Placeholder
        }

        public void ViewAssessmentAndGrades()
        {
            // Placeholder
        }

        public void DownloadMaterials()
        {
            // Placeholder
        }


        // -----------------------------
        // CONSTRUCTORS
        // -----------------------------
        // JSON veya manual load için boş constructor
        public Student() { }

        // UML constructor:
        // Person(name, surname, birthDate, email) base’de çağrılır.
        public Student(
            string name,
            string surname,
            DateTime birthDate,
            string email,
            int studentNumber,
            decimal accountBalance,
            int yearOfStudy,
            double gpa,
            int currentSemester,
            int attendance,
            string? middleName = null
        ) : base(name, surname, birthDate, email)
        {
            MiddleName = middleName;

            StudentNumber = studentNumber;
            AccountBalance = accountBalance;
            YearOfStudy = yearOfStudy;
            GPA = gpa;
            CurrentSemester = currentSemester;
            Attendance = attendance;

            AddToExtent(this);
        }

        // -----------------------------
        // TXT PERSISTENCY (NO LIBRARY)
        // -----------------------------
        public static void Save(string path = "students.txt")
        {
            var lines = new List<string>();

            foreach (var s in _extent)
            {
                string line =
                    $"{s.Name};{s.MiddleName};{s.Surname};{s.Email};{s.BirthDate:yyyy-MM-dd};" +
                    $"{s.StudentNumber};{s.AccountBalance};{s.YearOfStudy};{s.GPA};" +
                    $"{s.CurrentSemester};{s.Attendance}";

                lines.Add(line);
            }

            File.WriteAllLines(path, lines);
        }

        public static void Load(string path = "students.txt")
        {
            _extent = new List<Student>();

            if (!File.Exists(path))
                return;

            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                var p = line.Split(';');

                string name = p[0];
                string? middle = p[1] == "" ? null : p[1];
                string surname = p[2];
                string email = p[3];
                DateTime birth = DateTime.Parse(p[4]);

                int number = int.Parse(p[5]);
                decimal balance = decimal.Parse(p[6]);
                int year = int.Parse(p[7]);
                double gpa = double.Parse(p[8]);
                int sem = int.Parse(p[9]);
                int att = int.Parse(p[10]);

                new Student(name, surname, birth, email, number, balance, year, gpa, sem, att, middle);
            }
        }
    }
}
