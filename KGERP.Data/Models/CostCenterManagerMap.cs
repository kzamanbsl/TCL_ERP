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
    using System.ComponentModel.DataAnnotations;

    public partial class CostCenterManagerMap
    {
        public int CostCenterManagerMapId { get; set; }
        public int CostCenterId { get; set; }
        public long ManagerId { get; set; }
        public bool IsMapActive { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Accounting_CostCenter Accounting_CostCenter { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
