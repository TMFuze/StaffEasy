﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StaffEasy.AppFiles.DataBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HRDatabaseEntities : DbContext
    {
        public HRDatabaseEntities()
            : base("name=HRDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Education> Education { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<MedicalRecords> MedicalRecords { get; set; }
        public virtual DbSet<Vacancies> Vacancies { get; set; }
        public virtual DbSet<WorkExperience> WorkExperience { get; set; }
        public virtual DbSet<Archive> Archive { get; set; }
        public virtual DbSet<CountryCode> CountryCode { get; set; }
        public virtual DbSet<DocType> DocType { get; set; }
        public virtual DbSet<EducationForm> EducationForm { get; set; }
        public virtual DbSet<EducationLevel> EducationLevel { get; set; }
        public virtual DbSet<FormOfEducationTermination> FormOfEducationTermination { get; set; }
        public virtual DbSet<SourceOfFinancing> SourceOfFinancing { get; set; }
        public virtual DbSet<SpecializationCode> SpecializationCode { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
