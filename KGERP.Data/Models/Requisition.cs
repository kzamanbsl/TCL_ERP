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
    
    public partial class Requisition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requisition()
        {
            this.Stores = new HashSet<Store>();
            this.RequisitionItems = new HashSet<RequisitionItem>();
        }
    
        public int RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public string RequisitionBy { get; set; }
        public Nullable<System.DateTime> RequisitionDate { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string RequisitionStatus { get; set; }
        public string DeliveredBy { get; set; }
        public Nullable<System.DateTime> DeliveredDate { get; set; }
        public string DeliveryNo { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> RequisitionType { get; set; }
        public Nullable<int> FromRequisitionId { get; set; }
        public Nullable<int> ToRequisitionId { get; set; }
        public int OrderDetailsId { get; set; }
        public bool IsSubmitted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Store> Stores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequisitionItem> RequisitionItems { get; set; }
    }
}
