//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Soccen.Models
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class serviceexecution
    {
        public int IdServiceExecution { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> ServiceIdServiceExecution { get; set; }
        public Nullable<sbyte> Status { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string PayOrFree { get; set; }
    
        public virtual customer customer { get; set; }
        public virtual service service { get; set; }
    }
}
