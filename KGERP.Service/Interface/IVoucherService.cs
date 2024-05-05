using KGERP.Service.Implementation.Accounting;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Service.Interface
{
    public interface IVoucherService : IDisposable
    {
        string GetVoucherNo(int voucherTypeId, int companyId, DateTime voucherDate);
        List<VoucherModel> GetVouchers(int companyId, string searchDate, string searchText);
        Task<VoucherModel> GetVouchersList(int companyId, DateTime? fromDate, DateTime? toDate, bool? vStatus, int? voucherTypeId);
        Task<VoucherModel> GetRequisitionVouchersList(int companyId, DateTime? fromDate, DateTime? toDate, bool? vStatus, int? voucherTypeId);
        Task<VoucherModel> GetRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate,/* bool? vStatus,*/ int? voucherTypeId);
        Task<VoucherModel> InitiatorGetRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate,/* bool? vStatus,*/ int? voucherTypeId);
        Task<VoucherModel> RequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate,/* bool? vStatus,*/ int? voucherTypeId); // New
        Task<VoucherModel> CheckerGetRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate,/* bool? vStatus,*/ int? voucherTypeId);
        Task<VoucherModel> CheckerRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate,/* bool? vStatus,*/ int? voucherTypeId); // New
        Task<VoucherModel> ApproverGetRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate,/* bool? vStatus,*/ int? voucherTypeId);
        Task<VoucherModel> ApproverRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate,/* bool? vStatus,*/ int? voucherTypeId); // New

        Task<VMJournalSlave> GetVoucherRequisitionMapDetailsWithApproval(int companyId, int voucherId);
        Task<VoucherModel> GetStockVouchersList(int companyId);

        Task<VoucherModel> GetVouchersList(VoucherModel voucherModel);

        Task<List<VoucherModel>> GetAllVouchersList(int companyId,int? voucherId, DateTime? fromDate, DateTime? toDate);

        VoucherModel GetVoucher(int companyId, long id);
        //VoucherModel CreateTempVoucher(VoucherModel voucher);
        bool SaveVoucher(VoucherModel voucher, out string message);
        //VoucherModel RemoveVoucherItem(long id);
        object GetVoucherNoAutoComplete(string prefix, int companyId);
        //int InsertFeedDada(int companyId);
        //int UpdateAccGLSheet

        Task<long> CheckerVoucherRequisitionApproval(VMJournalSlave vmJournalSlave);
        Task<long> ApproverVoucherRequisitionApproval(VMJournalSlave vmJournalSlave);
    }
}
