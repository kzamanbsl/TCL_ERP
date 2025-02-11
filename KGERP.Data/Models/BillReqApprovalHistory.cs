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
    
    public partial class BillReqApprovalHistory
    {
        public long BillReqApprovalHistoryId { get; set; }
        public long BillRequisitionMasterId { get; set; }
        public Nullable<long> BillRequisitionDetailId { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<decimal> DemandQty { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> ReceivedSoFar { get; set; }
        public Nullable<decimal> RemainingQty { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
