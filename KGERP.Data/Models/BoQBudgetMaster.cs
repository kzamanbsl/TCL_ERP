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
    
    public partial class BoQBudgetMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BoQBudgetMaster()
        {
            this.BoQBudgetDetails = new HashSet<BoQBudgetDetail>();
        }
    
        public long BoQBudgetMasterId { get; set; }
        public int ProjectId { get; set; }
        public long BoQDivisionId { get; set; }
        public int BillBoQItemId { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BoQBudgetDetail> BoQBudgetDetails { get; set; }
        public virtual Accounting_CostCenter Accounting_CostCenter { get; set; }
    }
}
