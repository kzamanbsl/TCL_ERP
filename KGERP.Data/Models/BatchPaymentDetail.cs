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
    
    public partial class BatchPaymentDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BatchPaymentDetail()
        {
            this.PaymentMasters = new HashSet<PaymentMaster>();
        }
    
        public int BatchPaymentDetailId { get; set; }
        public int BatchPaymentMasterId { get; set; }
        public int CompanyId { get; set; }
        public int VendorId { get; set; }
        public int VendorTypeId { get; set; }
        public string ReferenceNo { get; set; }
        public string MoneyReceiptNo { get; set; }
        public Nullable<System.DateTime> MoneyReceiptDate { get; set; }
        public decimal InAmount { get; set; }
        public decimal OutAmount { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual BatchPaymentMaster BatchPaymentMaster { get; set; }
        public virtual Vendor Vendor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentMaster> PaymentMasters { get; set; }
    }
}
