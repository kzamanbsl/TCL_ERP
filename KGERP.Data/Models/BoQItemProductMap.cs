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
    
    public partial class BoQItemProductMap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BoQItemProductMap()
        {
            this.BoqBNEApprovalHistroys = new HashSet<BoqBNEApprovalHistroy>();
        }
    
        public long BoQItemProductMapId { get; set; }
        public Nullable<int> BoQItemId { get; set; }
        public int ProductId { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public Nullable<decimal> EstimatedQty { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<decimal> EstimatedAmount { get; set; }
        public Nullable<int> RequisitionSubtypeId { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Product Product { get; set; }
        public virtual BillBoQItem BillBoQItem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BoqBNEApprovalHistroy> BoqBNEApprovalHistroys { get; set; }
    }
}
