using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class BankAccountInfoModel : BaseVM
    {
        public long BankAccountInfoId { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public int BankBranchId { get; set; }
        public string BankBranchName { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountName { get; set; }
        public decimal AccountNumber { get; set; }
        public SelectList AccountTypeList { get { return new SelectList(BaseFunctionalities.GetEnumList<EnumBankAccountType>(), "Value", "Text"); } }
        public SelectList BankList { get; set; } = new SelectList(new List<object>());
        public SelectList BankBranchList { get; set; } = new SelectList(new List<object>());
        public IEnumerable<BankAccountInfoModel> BankAccountInfoList { get; set; } = new List<BankAccountInfoModel>();
    }

    public class ChequeBookModel : BaseVM
    {
        public long ChequeBookId { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public int BankBranchId { get; set; }
        public string BankBranchName { get; set; }
        public int BankAccountInfoId { get; set; }
        public string AccountName { get; set; }
        public long AccountNumber { get; set; }
        public string ChequeBookNo { get; set; }
        public int BookFirstPageNumber { get; set; }
        public int BookLastPageNumber { get; set; }
        public int TotalBookPage { get; set; }
        public int UsedBookPage { get; set; }
        public SelectList BankList { get; set; } = new SelectList(new List<object>());
        public SelectList BankBranchList { get; set; } = new SelectList(new List<object>());
        public SelectList AccountNoList { get; set; } = new SelectList(new List<object>());
        public IEnumerable<ChequeBookModel> ChequeBookList { get; set; } = new List<ChequeBookModel>();
    }

    public class ChequeRegisterModel : BaseVM
    {
        public long ChequeRegisterId { get; set; }
        public int RegisterFor { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public int BankBranchId { get; set; }
        public string BankBranchName { get; set; }
        public long BankAccountInfoId { get; set; }
        public string AccountName { get; set; }
        public long AccountNumber { get; set; }
        public long ChequeBookId { get; set; }
        public int ChequeBookNo { get; set; }
        public int RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ChequeDate { get; set; }
        public int ChequeNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime ClearingDate { get; set; }
        public string PayTo { get; set; }
        public bool IsSigned { get; set; }
        public bool IsPrinted { get; set; }
        public bool IsCanceled { get; set; }
        public int PrintCount { get; set; }
        public DateTime StrFromDate { get; set; }
        public DateTime StrToDate { get; set; }
        public string ReportType { get; set; }
        public string CancelReason { get; set; }
        public SelectList RegisterForList { get { return new SelectList(BaseFunctionalities.GetEnumList<EnumChequeRegisterFor>(), "Value", "Text"); } }
        public SelectList RequisitionList { get; set; } = new SelectList(new List<object>());
        public SelectList ProjectList { get; set; } = new SelectList(new List<object>());
        public SelectList SupplierList { get; set; } = new SelectList(new List<object>());
        public SelectList BankList { get; set; } = new SelectList(new List<object>());
        public SelectList BankBranchList { get; set; } = new SelectList(new List<object>());
        public SelectList AccountNoList { get; set; } = new SelectList(new List<object>());
        public SelectList ChequeBookList { get; set; } = new SelectList(new List<object>());
        public SelectList ChequeNumberList { get; set; } = new SelectList(new List<object>());
        public IEnumerable<ChequeRegisterModel> ChequeRegisterList { get; set; } = new List<ChequeRegisterModel>();
       
    }
}
