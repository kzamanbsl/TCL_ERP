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
    
    public partial class VendorDeed
    {
        public int VendorDeedId { get; set; }
        public int CompanyId { get; set; }
        public int VendorId { get; set; }
        public Nullable<int> MonthlyTarget { get; set; }
        public Nullable<int> YearlyTarget { get; set; }
        public Nullable<int> CreditRatioFrom { get; set; }
        public Nullable<int> CreditRatioTo { get; set; }
        public Nullable<int> CreditLimit { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<decimal> Transport { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public Nullable<int> ExtraCondition1 { get; set; }
        public Nullable<decimal> ExtraBenifite { get; set; }
        public Nullable<decimal> DepositRate { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
