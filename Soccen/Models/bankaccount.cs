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
    
    public partial class bankaccount
    {
        public int IdBankAccount { get; set; }
        public Nullable<int> BankId { get; set; }
        public Nullable<int> CustomerId { get; set; }
    
        public virtual bank bank { get; set; }
        public virtual customer customer { get; set; }
    }
}
