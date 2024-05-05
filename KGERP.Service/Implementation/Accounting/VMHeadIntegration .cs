using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;

namespace KGERP.Service.Implementation.Accounting
{
    public class VMHeadIntegration : BaseVM
    {
        public int ParentId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public int OrderNo { get; set; }
        public int? LayerNo { get; set; }
        public bool IsIncomeHead { get; set; }
        public string ProductType { get; set; }

    }
    public class VMJournal : BaseVM
    {
        public string VoucherNo { get; set; }
        public DateTime? Date { get; set; }
        public string Title { get; set; }
        public string Narration { get; set; }
        public bool Finalized { get; set; } = false;
        public bool Approved { get; set; } = false;
        public string ChqName { get; set; }
        public decimal BillofExchangeValue { get; set; }
        public decimal dollarRate { get; set; }

        public int? Accounting_CostCenterFK { get; set; }
        public long? BillRequisitionId { get; set; }
        public bool IsRequisitionVoucher { get; set; } = false;

        public long RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public string RequisitionInitiator { get; set; }
        public int Accounting_BankOrCashParantId { get; set; }

        public int? Accounting_BankOrCashId { get; set; }
        public string BankOrCashNane { get; set; }

        public string Accounting_CostCenterName { get; set; }

        public decimal BDT_Value { get; set; }
        public decimal ForeignCurrency_Value { get; set; }
        public IEnumerable<VMJournal> DataList { get; set; }


        public decimal ForeignCurrencyAmount { get; set; } = 0;
        public decimal ConversionRateToBDT { get; set; } = 0;

        public bool isRealization { get; set; } = false;
        public bool IsVirtual { get; set; } = false;

        //public string Coce { get; set; }

        public int ERAccounting_HeadFK { get; set; }

    }

    public class VMJournalSlave : VMJournal
    {
        public long? VoucherDetailId { get; set; }
        public long VoucherId { get; set; }


        public VMJournal vMJournal { get; set; }

        public string Status { get; set; }
        [Required(ErrorMessage = "The Accounts Head Field Is Required")]
        public int Accounting_HeadFK { get; set; }
        [Required]
        public string AccountingHeadName { get; set; }
        public string Particular { get; set; }

        [Required]
        public int VoucherTypeId { get; set; }
        public int? RequisitionMaterialId { get; set; }
        public string MaterialName { get; set; }
        public int? SupplierId { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public int BankBranchId { get; set; }
        public string BankBranchName { get; set; }
        public long BankAccountInfoId { get; set; }
        public string AccountName { get; set; }
        public long AccountNumber { get; set; }
        public long ChequeBookId { get; set; }
        public int ChequeBookNo { get; set; }

        public double Debit { get; set; } = 0;
        public double Credit { get; set; } = 0;
        public decimal TotalDebit { get; set; } = 0;
        public decimal TotalCredit { get; set; } = 0;
        public IEnumerable<VMJournalSlave> DataListDetails { get; set; }

        public List<VMJournalSlave> DataListSlave { get; set; }

        public List<BRVoucherApprovalModel> ApprovalList { get; set; } = new List<BRVoucherApprovalModel>();
        public string message { get; set; }
        public string ChqNo { get; set; }
        public bool IsStock { get; set; }
        public bool IsSubmit { get; set; }

        public decimal? ApprovedQty { get; set; }
        public decimal? UnitRate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal TotalCredited { get; set; } = 0;
        public decimal TotalDebited { get; set; } = 0;
        public string Reason { get; set; }


        public DateTime? ChqDate { get; set; }
        public bool isDocumentPayment { get; set; }
        public SelectList VoucherTypesList { get; set; } = new SelectList(new List<object>());
        public SelectList CostCenterList { get; set; } = new SelectList(new List<object>());
        public SelectList BankOrCashParantList { get; set; } = new SelectList(new List<object>());
        public SelectList BankOrCashGLList { get; set; } = new SelectList(new List<object>());
        public SelectList Requisitions { get; set; } = new SelectList(new List<object>());
        public SelectList MaterialItemList { get; set; } = new SelectList(new List<Product>());
        public SelectList BankList { get; set; } = new SelectList(new List<object>());
        public SelectList BankBranchList { get; set; } = new SelectList(new List<object>());
        public SelectList AccountNoList { get; set; } = new SelectList(new List<object>());
        public SelectList ChequeBookList { get; set; } = new SelectList(new List<object>());
        public int VoucherFor { get; set; }
        public SelectList VoucherForList { get; set; } = new SelectList(Enum.GetValues(typeof(EnumVoucherFor)).Cast<EnumVoucherFor>().Select(e => new SelectListItem { Text = e.ToString(), Value = ((int)e).ToString() }), "Value", "Text");

        public int AprrovalStatusId { get; set; }
        public string AprrovalStatusName { get { return BaseFunctionalities.GetEnumDescription((EnumBillRequisitionStatus)AprrovalStatusId); } }

        public int CheckerAprrovalStatusId { get; set; }
        public string CheckerAprrovalStatusName { get { return BaseFunctionalities.GetEnumDescription((EnumBillRequisitionStatus)CheckerAprrovalStatusId); } }
        public int ApproverAprrovalStatusId { get; set; }
        public string ApproverAprrovalStatusName { get { return BaseFunctionalities.GetEnumDescription((EnumBillRequisitionStatus)ApproverAprrovalStatusId); } }


    }
    public partial class BRVoucherApprovalModel
    {
        public long VoucherBRMapMasterApprovalId { get; set; }
        public Nullable<long> VoucherBRMapMasterId { get; set; }
        public Nullable<long> VoucherId { get; set; }
        public int AprrovalStatusId { get; set; }
        public string AprrovalStatusName { get { return BaseFunctionalities.GetEnumDescription((EnumBillRequisitionStatus)AprrovalStatusId); } }
        public Nullable<long> EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int SignatoryId { get; set; }
        public string SignatoryName { get { return BaseFunctionalities.GetEnumDescription((EnumVoucherRequisitionSignatory)SignatoryId); } }

        public int PriorityNo { get; set; }
        public bool IsSupremeApproved { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }

}
