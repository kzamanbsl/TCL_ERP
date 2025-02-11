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
    
    public partial class ChequeRegister
    {
        public long ChequeRegisterId { get; set; }
        public Nullable<long> RequisitionMasterId { get; set; }
        public int ProjectId { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public long ChequeBookId { get; set; }
        public string PayTo { get; set; }
        public System.DateTime IssueDate { get; set; }
        public System.DateTime ChequeDate { get; set; }
        public Nullable<int> ChequeNo { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime ClearingDate { get; set; }
        public string Remarks { get; set; }
        public bool IsSigned { get; set; }
        public bool HasPDF { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> PrintCount { get; set; }
        public Nullable<bool> IsCanceled { get; set; }
        public Nullable<bool> IsCancelRequest { get; set; }
        public string CancelReason { get; set; }
        public string RequestedBy { get; set; }
        public Nullable<System.DateTime> RequestedOn { get; set; }
    
        public virtual Accounting_CostCenter Accounting_CostCenter { get; set; }
        public virtual BillRequisitionMaster BillRequisitionMaster { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
