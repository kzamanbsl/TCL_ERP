using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static KGERP.Service.ServiceModel.ProductionMasterModel;

namespace KGERP.Service.ServiceModel
{
    public class BatchPaymentMasterModel : BaseVM
    {
        public int BatchPaymentMasterId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public decimal BankCharge { get; set; }
        public decimal TotalAmount { get; set; }// without bank charge sum of payments
        public Nullable<int> BankChargeHeadGLId { get; set; }
        public Nullable<int> PaymentToHeadGLId { get; set; }
        public bool IsSubmitted { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public string PaymentFromHeadGLName { get; set; }
        public string BankChargeHeadGLName { get; set; }
        public string PaymentToHeadGLName { get; set; }
        public int? Accounting_BankOrCashParantId { get; set; }
        public int? Accounting_BankOrCashId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string StrFromDate { get; set; }
        public string StrToDate { get; set; }

        public SelectList BankOrCashParantList { get; set; } = new SelectList(new List<object>());
        public SelectList BankOrCashGLList { get; set; } = new SelectList(new List<object>());
        public virtual ICollection<BatchPaymentDetail> BatchPaymentDetails { get; set; }
        public BatchPaymentDetailModel batchPaymentDetailModel { get; set; } = new BatchPaymentDetailModel();
        public IEnumerable<BatchPaymentMasterModel> DataList { get; set; } = new List<BatchPaymentMasterModel>();
        public IEnumerable<BatchPaymentDetailModel> DetailList { get; set; } = new List<BatchPaymentDetailModel>();

       
    }
    public class BatchPaymentDetailModel
    {
        public int BatchPaymentDetailId { get; set; }
        public int BatchPaymentMasterId { get; set; }
        public int CompanyId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int VendorTypeId { get; set; }
        public string ReferenceNo { get; set; }
        public string MoneyReceiptNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? MoneyReceiptDate { get; set; }
        public decimal InAmount { get; set; }
        public decimal OutAmount { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public int? SubZoneFk { get; set; }
        public SelectList SubZoneList { get; set; } = new SelectList(new List<object>());
        public SelectList CustomerList { get; set; } = new SelectList(new List<object>());
        public virtual Company Company { get; set; }
        public virtual BatchPaymentMaster BatchPaymentMaster { get; set; }
        public virtual Vendor Vendor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentMaster> PaymentMasters { get; set; }

    }
}
