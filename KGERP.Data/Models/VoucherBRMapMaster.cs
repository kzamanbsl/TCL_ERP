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
    
    public partial class VoucherBRMapMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VoucherBRMapMaster()
        {
            this.VoucherBRMapMasterApprovals = new HashSet<VoucherBRMapMasterApproval>();
        }
    
        public long VoucherBRMapMasterId { get; set; }
        public long BillRequsitionMasterId { get; set; }
        public long VoucherId { get; set; }
        public Nullable<int> CostCenterId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> ApprovalStatusId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public bool IsRequisitionVoucher { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual BillRequisitionMaster BillRequisitionMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VoucherBRMapMasterApproval> VoucherBRMapMasterApprovals { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}
