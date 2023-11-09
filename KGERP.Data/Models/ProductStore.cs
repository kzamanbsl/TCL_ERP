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
    
    public partial class ProductStore
    {
        public long ProductStoreId { get; set; }
        public string ReceiveCode { get; set; }
        public System.DateTime ReceiveDate { get; set; }
        public int StockInfoId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal InQty { get; set; }
        public decimal OutQty { get; set; }
        public Nullable<decimal> CogsPrice { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        public int CompanyId { get; set; }
        public string Flag { get; set; }
        public Nullable<decimal> TpPrice { get; set; }
        public Nullable<decimal> BalanceQty { get; set; }
        public Nullable<decimal> AvgRate { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
}
