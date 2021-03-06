//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Soccen.Models
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public customer()
        {
            this.customersocialtypes = new ObservableCollection<customersocialtype>();
            this.serviceexecutions = new ObservableCollection<serviceexecution>();
        }
    
        public int IdCustomer { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string City { get; set; }
        public string House { get; set; }
        public Nullable<int> Apartment { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Passport { get; set; }
        public string Identification { get; set; }
        public string Pension { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public Nullable<sbyte> LiveStatus { get; set; }
        public Nullable<sbyte> OtgStatus { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public Nullable<System.DateTime> DeathDate { get; set; }
        public Nullable<int> StreetId { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public string Other { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<customersocialtype> customersocialtypes { get; set; }
        public virtual street street { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<serviceexecution> serviceexecutions { get; set; }
    }
}
