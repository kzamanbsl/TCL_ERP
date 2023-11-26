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
    
    public partial class Accounting_CostCenter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Accounting_CostCenter()
        {
            this.CostCenterManagerMaps = new HashSet<CostCenterManagerMap>();
            this.BillRequisitionMasters = new HashSet<BillRequisitionMaster>();
            this.BoQBudgetMasters = new HashSet<BoQBudgetMaster>();
            this.BoQDivisions = new HashSet<BoQDivision>();
        }
    
        public int CostCenterId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public Nullable<int> CostCenterTypeId { get; set; }
    
        public virtual Accounting_CostCenterType Accounting_CostCenterType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CostCenterManagerMap> CostCenterManagerMaps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillRequisitionMaster> BillRequisitionMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BoQBudgetMaster> BoQBudgetMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BoQDivision> BoQDivisions { get; set; }
    }
}
