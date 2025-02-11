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
    
    public partial class BillBoQItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillBoQItem()
        {
            this.BoQItemProductMaps = new HashSet<BoQItemProductMap>();
        }
    
        public int BoQItemId { get; set; }
        public long BoQDivisionId { get; set; }
        public string Name { get; set; }
        public string BoQNumber { get; set; }
        public int BoqUnitId { get; set; }
        public decimal BoqQuantity { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BoQItemProductMap> BoQItemProductMaps { get; set; }
    }
}
