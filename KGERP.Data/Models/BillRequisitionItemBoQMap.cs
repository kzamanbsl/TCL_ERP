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
    
    public partial class BillRequisitionItemBoQMap
    {
        public long BillRequisitionItemBoQMapId { get; set; }
        public int BoQItemId { get; set; }
        public int BillRequisitionItemId { get; set; }
        public decimal EstimatedQty { get; set; }
        public decimal EstimatedAmount { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual BillBoQItem BillBoQItem { get; set; }
        public virtual BillRequisitionItem BillRequisitionItem { get; set; }
    }
}
