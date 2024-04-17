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
    
    public partial class PurchaseOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseOrder()
        {
            this.MaterialReceives = new HashSet<MaterialReceive>();
            this.PurchaseOrderDetails = new HashSet<PurchaseOrderDetail>();
            this.VendorDepositHistories = new HashSet<VendorDepositHistory>();
        }
    
        public long PurchaseOrderId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<long> DemandId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public Nullable<int> WorkOrderForId { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public Nullable<int> ModeOfPurchaseId { get; set; }
        public string Days { get; set; }
        public Nullable<int> ProductOriginId { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<long> EmpId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public Nullable<System.Guid> ReferenceNo { get; set; }
        public bool IsActive { get; set; }
        public int CompletionStatus { get; set; }
        public int Status { get; set; }
        public int SupplierPaymentMethodEnumFK { get; set; }
        public string DeliveryAddress { get; set; }
        public string TermsAndCondition { get; set; }
        public bool IsCancel { get; set; }
        public bool IsHold { get; set; }
        public string PINo { get; set; }
        public string ShippedBy { get; set; }
        public string PortOfLoading { get; set; }
        public Nullable<int> FinalDestinationCountryFk { get; set; }
        public string PortOfDischarge { get; set; }
        public decimal FreightCharge { get; set; }
        public decimal OtherCharge { get; set; }
        public string LCNo { get; set; }
        public decimal LCValue { get; set; }
        public string InsuranceNo { get; set; }
        public decimal PremiumValue { get; set; }
        public bool IsOpening { get; set; }
        public Nullable<long> OrderMasterId { get; set; }
        public Nullable<int> LCHeadGLId { get; set; }
        public Nullable<int> StockInfoId { get; set; }
        public Nullable<long> BillRequisitionMasterId { get; set; }
        public string DocPath { get; set; }
    
        public virtual DropDownItem DropDownItem { get; set; }
        public virtual DropDownItem DropDownItem1 { get; set; }
        public virtual Demand Demand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialReceive> MaterialReceives { get; set; }
        public virtual StockInfo StockInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorDepositHistory> VendorDepositHistories { get; set; }
        public virtual BillRequisitionMaster BillRequisitionMaster { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
