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
    
    public partial class PaymentMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PaymentMaster()
        {
            this.Payments = new HashSet<Payment>();
        }
    
        public int PaymentMasterId { get; set; }
        public int CompanyId { get; set; }
        public int VendorId { get; set; }
        public string ReferenceNo { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string MoneyReceiptNo { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsFinalized { get; set; }
        public string PaymentNo { get; set; }
        public Nullable<System.DateTime> MoneyReceiptDate { get; set; }
        public Nullable<int> BankChargeHeadGLId { get; set; }
        public decimal BankCharge { get; set; }
        public Nullable<int> PaymentToHeadGLId { get; set; }
        public int VendorTypeId { get; set; }
        public Nullable<long> CGID { get; set; }
        public Nullable<int> BatchPaymentDetailId { get; set; }
        public bool IsAdjust { get; set; }
        public Nullable<int> BankAccountInfoId { get; set; }
    
        public virtual BatchPaymentDetail BatchPaymentDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
