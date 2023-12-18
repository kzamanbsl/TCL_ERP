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
    
    public partial class BillRequisitionApproval
    {
        public long BRApprovalId { get; set; }
        public long BillRequisitionMasterId { get; set; }
        public int AprrovalStatusId { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public int SignatoryId { get; set; }
        public int PriorityNo { get; set; }
        public bool IsSupremeApproved { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
