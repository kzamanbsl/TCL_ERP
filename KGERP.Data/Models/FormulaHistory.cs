//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KGERP.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FormulaHistory
    {
        public System.Guid Id { get; set; }
        public Nullable<int> RequisitionId { get; set; }
        public Nullable<int> RequisitionItemId { get; set; }
        public Nullable<int> FProductId { get; set; }
        public Nullable<int> ProductFormulaId { get; set; }
        public Nullable<int> RProductId { get; set; }
        public Nullable<decimal> RQty { get; set; }
        public Nullable<System.DateTime> FormulaDate { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
