using StaffEasy.AppFiles.DataBase;
using System;
using System.Collections.Generic;

namespace StaffEasy.Pages.User
{
    public class EmployeeFullViewModel
    {
        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Gender { get; set; }
        public string Citizenship { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssuedBy { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public string INN { get; set; }
        public string SNILS { get; set; }
        public string MilitaryID { get; set; }
        public string MilitaryDocumentType { get; set; }
        public string BirthCertificate { get; set; }
        public MedicalRecords MedicalRecord { get; set; }
        public List<WorkExperience> WorkExperiences { get; set; }
        public List<Education> Educations { get; set; }
    }
}