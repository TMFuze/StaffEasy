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
    
    public partial class Vacancies
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vacancies()
        {
            this.Applications = new HashSet<Applications>();
            this.Employees = new HashSet<Employees>();
        }
    
        public int VacancyID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string Status { get; set; }
        public Nullable<int> DepartmentID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Applications> Applications { get; set; }
        public virtual Departments Departments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
