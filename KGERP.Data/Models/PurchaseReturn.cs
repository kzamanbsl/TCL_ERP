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
    
    public partial class PurchaseReturn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseReturn()
        {
            this.PurchaseReturnDetails = new HashSet<PurchaseReturnDetail>();
        }
    
        public long PurchaseReturnId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public string ProductType { get; set; }
        public string ReturnNo { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string ReturnReason { get; set; }
        public Nullable<int> StockInfoId { get; set; }
        public Nullable<long> ReturnBy { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool Active { get; set; }
        public bool IsSubmited { get; set; }
        public long MaterialReceiveId { get; set; }
    
        public virtual StockInfo StockInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
        public virtual MaterialReceive MaterialReceive { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
