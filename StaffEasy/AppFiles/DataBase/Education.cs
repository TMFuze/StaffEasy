//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Education
    {
        public int EducationID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string EducationLevel { get; set; }
        public string EducationInstitution { get; set; }
        public string EducationSpecialty { get; set; }
        public Nullable<System.DateTime> GraduationDate { get; set; }
    
        public virtual Employees Employees { get; set; }
    }
}