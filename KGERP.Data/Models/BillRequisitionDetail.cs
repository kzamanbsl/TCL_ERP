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
    
    public partial class BillRequisitionDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillRequisitionDetail()
        {
            this.BillReqApprovalHistories = new HashSet<BillReqApprovalHistory>();
            this.PurchaseOrderDetails = new HashSet<PurchaseOrderDetail>();
        }
    
        public long BillRequisitionDetailId { get; set; }
        public long BillRequisitionMasterId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<decimal> EstimatedQty { get; set; }
        public Nullable<decimal> DemandQty { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> ReceivedSoFar { get; set; }
        public Nullable<decimal> RemainingQty { get; set; }
        public string Floor { get; set; }
        public string Ward { get; set; }
        public string DPP { get; set; }
        public string Chainage { get; set; }
        public string Remarks { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillReqApprovalHistory> BillReqApprovalHistories { get; set; }
        public virtual BillRequisitionMaster BillRequisitionMaster { get; set; }
        public virtual Product Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}
