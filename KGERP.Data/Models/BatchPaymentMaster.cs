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
    
    public partial class BatchPaymentMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BatchPaymentMaster()
        {
            this.BatchPaymentDetails = new HashSet<BatchPaymentDetail>();
        }
    
        public int BatchPaymentMasterId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public decimal BankCharge { get; set; }
        public Nullable<int> BankChargeHeadGLId { get; set; }
        public Nullable<int> PaymentToHeadGLId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsSubmitted { get; set; }
        public int CompanyId { get; set; }
        public string BatchPaymentNo { get; set; }
    
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatchPaymentDetail> BatchPaymentDetails { get; set; }
    }
}
