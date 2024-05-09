using KGERP.Data.Models;
using KGERP.Service.Implementation.Accounting;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class VoucherService : IVoucherService
    {
        private bool disposed = false;

        private readonly ERPEntities _context;
        public VoucherService(ERPEntities context)
        {
            this._context = context;
        }
        public List<VoucherModel> GetVouchers(int companyId, string searchDate, string searchText)
        {
            DateTime? dateSearch = null;
            dateSearch = !string.IsNullOrEmpty(searchDate) ? Convert.ToDateTime(searchDate) : dateSearch;

            IQueryable<VoucherModel> vouchers = _context.Database.SqlQuery<VoucherModel>("exec spGetVoucherList {0}", companyId).AsQueryable();
            if (dateSearch == null)
            {
                return vouchers.Where(x => (x.VoucherNo.ToLower().Contains(searchText.ToLower()) || String.IsNullOrEmpty(searchText)) ||
                                    (x.Narration.ToLower().Contains(searchText.ToLower()) || String.IsNullOrEmpty(searchText))
                                    ).OrderByDescending(x => x.VoucherDate).ToList();
            }
            if (string.IsNullOrEmpty(searchText) && dateSearch != null)
            {
                return vouchers.Where(x => x.VoucherDate == dateSearch).OrderByDescending(x => x.VoucherDate).ToList();
            }


            return vouchers.Where(x => x.VoucherDate == dateSearch &&
                                (x.VoucherNo.ToLower().Contains(searchText) || String.IsNullOrEmpty(searchText)) ||
                                (x.Narration.ToLower().Contains(searchText) || String.IsNullOrEmpty(searchText))
                               ).OrderByDescending(x => x.VoucherDate).ToList();
        }

        public async Task<VoucherModel> GetVouchersList(int companyId, DateTime? fromDate, DateTime? toDate, bool? vStatus, int? voucherTypeId)
        {
            VoucherModel voucherModel = new VoucherModel();
            voucherModel.CompanyId = companyId;
            voucherModel.VoucherTypeId = voucherTypeId;
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          where t1.CompanyId == companyId && t1.IsActive
                                                          && (voucherTypeId > 0 ? t1.VoucherTypeId == voucherTypeId : t1.VoucherTypeId > 0)
                                                          && t1.VoucherDate >= fromDate && t1.VoucherDate <= toDate
                                                          && t1.IsSubmit == vStatus
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name

                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());
            return voucherModel;
        }
        public async Task<VoucherModel> GetStockVouchersList(int companyId)
        {
            VoucherModel voucherModel = new VoucherModel();
            voucherModel.CompanyId = companyId;
            //voucherModel.VoucherTypeId = voucherTypeId;
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          where t1.CompanyId == companyId && t1.IsActive && t1.IsStock

                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name

                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());
            return voucherModel;
        }

        public async Task<VoucherModel> GetVouchersList(VoucherModel voucherModel)
        {
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          where t1.CompanyId == voucherModel.CompanyId && t1.IsActive
                                                          && t1.VoucherDate > voucherModel.FromDate && t1.VoucherDate < voucherModel.ToDate
                                                          && t1.IsSubmit == voucherModel.IsSubmit
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit

                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());
            return voucherModel;
        }
        public async Task<List<VoucherModel>> GetAllVouchersList(int companyId, int? voucherTypeId, DateTime? fromDate, DateTime? toDate)
        {
            List<VoucherModel> modelList = new List<VoucherModel>();

            VoucherModel voucherModel = new VoucherModel();
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          where t1.CompanyId == voucherModel.CompanyId && t1.IsActive
                                                          && t1.VoucherDate > voucherModel.FromDate && t1.VoucherDate < voucherModel.ToDate
                                                          && t1.IsSubmit == voucherModel.IsSubmit
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit

                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());

            modelList.Add(voucherModel);

            return modelList;
        }



        public VoucherModel GetVoucher(int companyId, long id)
        {
            if (id == 0)
            {
                return new VoucherModel() { VoucherId = id };
            }
            Voucher voucher = _context.Vouchers.Find(id);
            return ObjectConverter<Voucher, VoucherModel>.Convert(voucher);
        }


        //public VoucherModel CreateTempVoucher(VoucherModel model)
        //{
        //    HeadGL headGL = context.HeadGLs.Find(model.AccountHeadId);

        //    TempVoucher tempVoucher = new TempVoucher
        //    {
        //        CompanyId = model.CompanyId,
        //        VoucherTypeId = model.VoucherTypeId,
        //        VoucherNo = model.VoucherNo,
        //        VoucherDate = model.VoucherDate,
        //        AccountHeadId = model.AccountHeadId,
        //        ChqNo = model.ChqNo,
        //        ChqName = model.ChqName,
        //        ChqDate = model.ChqDate,
        //        ProjectId = model.ProjectId,
        //        Narration = model.Narration,
        //        Particular = model.Particular,
        //        DebitAmount = model.DebitAmount ?? 0,
        //        CreditAmount = model.CreditAmount ?? 0,
        //        AccCode = headGL.AccCode,
        //        AccName = headGL.AccName,
        //        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
        //        CreateDate = DateTime.Now,
        //    };

        //    context.TempVouchers.Add(tempVoucher);
        //    context.SaveChanges();

        //    List<VoucherDetailModel> voucherDetails = new List<VoucherDetailModel>();
        //    voucherDetails = context.Database.SqlQuery<VoucherDetailModel>("exec spGetVoucherDetailGrid {0}", model.CompanyId).ToList();

        //    model.VoucherDetails = voucherDetails;
        //    return model;

        //}


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }


        public string GetVoucherNo(int voucherTypeId, int companyId, DateTime voucherDate)
        {
            //Mamun
            VoucherType voucherType = _context.VoucherTypes.FirstOrDefault(x => x.VoucherTypeId == voucherTypeId);
            string voucherNo = string.Empty;
            int vouchersCount = _context.Vouchers.Count(x => x.VoucherTypeId == voucherTypeId && x.CompanyId == companyId
                && x.VoucherDate.Value.Month == voucherDate.Month
                && x.VoucherDate.Value.Year == voucherDate.Year);

            vouchersCount++;
            //voucherNo = voucherType.Code + "-" + vouchersCount.ToString().PadLeft(4, '0');
            //voucherNo = $"{voucherType.Code}-{DateTime.Now:yy}-{DateTime.Now:MM}-{vouchersCount.ToString().PadLeft(4, '0')}";
            //voucherNo = $"{voucherType.Code}-{voucherDate.Year}-{voucherDate.Month}-{vouchersCount.ToString().PadLeft(4, '0')}";
            voucherNo = $"{voucherType.Code}-{voucherDate:yy}-{voucherDate:MM}-{vouchersCount.ToString().PadLeft(4, '0')}";

            return voucherNo;
        }

        private string GenerateVoucherNo(string lastVoucherNo)
        {
            string prefix = lastVoucherNo.Substring(0, 4);
            int code = Convert.ToInt32(lastVoucherNo.Substring(4, 6));
            int newCode = code + 1;
            return prefix + newCode.ToString().PadLeft(6, '0');
        }

        public bool SaveVoucher(VoucherModel model, out string message)
        {
            message = string.Empty;
            int noOfRowsAffected = 0;
            double sumDebitAmount = model.VoucherDetails.Sum(x => x.DebitAmount) ?? 0;
            double sumCreditAmount = model.VoucherDetails.Sum(x => x.CreditAmount) ?? 0;

            if (Math.Round(sumDebitAmount, 2) != Math.Round(sumCreditAmount, 2))
            {
                message = "Voucher posting failed. Debit and credit amount did not match.";
                return noOfRowsAffected > 0;
            }

            Voucher voucher = new Voucher()
            {
                CompanyId = model.CompanyId,
                VoucherTypeId = model.VoucherTypeId,
                VoucherNo = model.VoucherNo,
                VoucherDate = model.VoucherDate,
                ChqNo = model.ChqNo,
                ChqName = model.ChqName,
                ChqDate = model.ChqDate,
                Narration = model.Narration,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreateDate = DateTime.Now,
                Accounting_CostCenterFk = model.Accounting_CostCenterFk,
                IsActive = true,
                VoucherStatus = "A",
                IsSubmit = false,
            };

            List<VoucherDetail> voucherDetails = new List<VoucherDetail>();

            voucherDetails = model.VoucherDetails.Select(x =>
            new VoucherDetail
            {
                AccountHeadId = x.AccountHeadId,
                DebitAmount = x.DebitAmount ?? 0,
                CreditAmount = x.CreditAmount ?? 0,
                Particular = x.Particular,
                TransactionDate = model.VoucherDate,
                IsActive = true
            }).ToList();

            voucher.VoucherDetails = voucherDetails;
            _context.Vouchers.Add(voucher);
            noOfRowsAffected = _context.SaveChanges();
            if (noOfRowsAffected > 0)
            {
                _context.Database.ExecuteSqlCommand("exec spVoucerPostingToIncomeAccount {0},{1}", voucher.VoucherId, voucher.CompanyId);
                message = "Voucher posted successfully.";
            }

            return noOfRowsAffected > 0;
        }

        public object GetVoucherNoAutoComplete(string prefix, int companyId)
        {
            return _context.Vouchers.Where(x => x.CompanyId == companyId && x.VoucherNo.StartsWith(prefix)).Select(x => new
            {
                label = x.VoucherNo,
                val = x.VoucherNo
            }).OrderBy(x => x.label).Take(20).ToList();
        }

        //public VoucherModel RemoveVoucherItem(long id)
        //{
        //    var tempVoucher = context.TempVouchers.Find(id);

        //    VoucherModel model = new VoucherModel
        //    {
        //        CompanyId = tempVoucher.CompanyId,
        //        VoucherTypeId = tempVoucher.VoucherTypeId,
        //        VoucherNo = tempVoucher.VoucherNo,
        //        VoucherDate = tempVoucher.VoucherDate,
        //        AccountHeadId = tempVoucher.AccountHeadId??0,
        //        ChqNo = tempVoucher.ChqNo,
        //        ChqName = tempVoucher.ChqName,
        //        ChqDate = tempVoucher.ChqDate,
        //        ProjectId = tempVoucher.ProjectId,
        //        Narration = tempVoucher.Narration,
        //        Particular = tempVoucher.Particular,
        //        DebitAmount = tempVoucher.DebitAmount,
        //        CreditAmount = tempVoucher.CreditAmount,   
        //    };
        //    if (tempVoucher !=null)
        //    {
        //        context.TempVouchers.Remove(tempVoucher);
        //        context.SaveChanges();
        //    }

        //    List<VoucherDetailModel> voucherDetails = new List<VoucherDetailModel>();
        //    voucherDetails = context.Database.SqlQuery<VoucherDetailModel>("exec spGetVoucherDetailGrid {0}", model.CompanyId).ToList();
        //    model.VoucherDetails = voucherDetails;

        //    return model;
        //}


        #region Requisition Voucher and Approval

        public async Task<VoucherModel> GetRequisitionVouchersList(int companyId, DateTime? fromDate, DateTime? toDate, bool? vStatus, int? voucherTypeId)
        {
            VoucherModel voucherModel = new VoucherModel();
            voucherModel.CompanyId = companyId;
            voucherModel.VoucherTypeId = voucherTypeId;
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          join t4 in _context.VoucherBRMapMasters on t1.VoucherId equals t4.VoucherId into t4_Join
                                                          from t4 in t4_Join.DefaultIfEmpty()
                                                          join t5 in _context.BillRequisitionMasters on t4.BillRequsitionMasterId equals t5.BillRequisitionMasterId into t5_Join
                                                          from t5 in t5_Join.DefaultIfEmpty()
                                                          where t1.CompanyId == companyId && t1.IsActive
                                                          && (voucherTypeId > 0 ? t1.VoucherTypeId == voucherTypeId : t1.VoucherTypeId > 0)
                                                          && t1.VoucherDate >= fromDate && t1.VoucherDate <= toDate
                                                          && t1.IsSubmit == vStatus
                                                          && t5.IsActive
                                                          && t4.IsActive
                                                          && t4.IsRequisitionVoucher
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name,
                                                              RequisitionId = t4.BillRequsitionMasterId,
                                                              RequisitionNo = t5.BillRequisitionNo,
                                                              VoucherRequisitionMasterMapId = t4.VoucherBRMapMasterId



                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());
            return voucherModel;
        }

        public async Task<VoucherModel> GetRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, /*bool? vStatus,*/ int? voucherTypeId)
        {
            VoucherModel voucherModel = new VoucherModel();
            voucherModel.CompanyId = companyId;
            voucherModel.VoucherTypeId = voucherTypeId;
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          join t4 in _context.VoucherBRMapMasters on t1.VoucherId equals t4.VoucherId into t4_Join
                                                          from t4 in t4_Join.DefaultIfEmpty()
                                                          join t5 in _context.BillRequisitionMasters on t4.BillRequsitionMasterId equals t5.BillRequisitionMasterId into t5_Join
                                                          from t5 in t5_Join.DefaultIfEmpty()

                                                          join t6 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Initiator && x.IsActive) on t4.VoucherBRMapMasterId equals t6.VoucherBRMapMasterId into t6_Join
                                                          from t6 in t6_Join.DefaultIfEmpty()

                                                          join t7 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Checker && x.IsActive) on t4.VoucherBRMapMasterId equals t7.VoucherBRMapMasterId into t7_Join
                                                          from t7 in t7_Join.DefaultIfEmpty()

                                                          join t8 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Approver && x.IsActive) on t4.VoucherBRMapMasterId equals t8.VoucherBRMapMasterId into t8_Join
                                                          from t8 in t8_Join.DefaultIfEmpty()

                                                          where t1.CompanyId == companyId && t1.IsActive
                                                          && (voucherTypeId > 0 ? t1.VoucherTypeId == voucherTypeId : t1.VoucherTypeId > 0)
                                                          && t1.VoucherDate >= fromDate && t1.VoucherDate <= toDate
                                                          && t1.IsSubmit == true
                                                          && t5.IsActive
                                                          && t4.IsActive
                                                          && t4.IsRequisitionVoucher
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name,
                                                              RequisitionId = t4.BillRequsitionMasterId,
                                                              RequisitionNo = t5.BillRequisitionNo,
                                                              VoucherRequisitionMasterMapId = t4.VoucherBRMapMasterId,
                                                              InitiatorAprrovalStatusId = t6.AprrovalStatusId,
                                                              CheckerAprrovalStatusId = t7.AprrovalStatusId,
                                                              ApproverAprrovalStatusId = t7.AprrovalStatusId

                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());

            return voucherModel;
        }

        public async Task<VoucherModel> InitiatorGetRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, /*bool? vStatus,*/ int? voucherTypeId)
        {
            VoucherModel voucherModel = new VoucherModel();
            var empId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);

            voucherModel.CompanyId = companyId;
            voucherModel.VoucherTypeId = voucherTypeId;

            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          join t4 in _context.VoucherBRMapMasters on t1.VoucherId equals t4.VoucherId into t4_Join
                                                          from t4 in t4_Join.DefaultIfEmpty()
                                                          join t6 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Initiator && x.IsActive) on t4.VoucherBRMapMasterId equals t6.VoucherBRMapMasterId into t6_Join
                                                          from t6 in t6_Join.DefaultIfEmpty()

                                                          join t7 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Checker && x.IsActive) on t4.VoucherBRMapMasterId equals t7.VoucherBRMapMasterId into t7_Join
                                                          from t7 in t7_Join.DefaultIfEmpty()

                                                          join t8 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Approver && x.IsActive) on t4.VoucherBRMapMasterId equals t8.VoucherBRMapMasterId into t8_Join
                                                          from t8 in t8_Join.DefaultIfEmpty()

                                                          join t9 in _context.Employees on t6.EmployeeId equals t9.Id into t9_Join
                                                          from t9 in t9_Join.DefaultIfEmpty()

                                                          join t5 in _context.BillRequisitionMasters on t4.BillRequsitionMasterId equals t5.BillRequisitionMasterId into t5_Join
                                                          from t5 in t5_Join.DefaultIfEmpty()

                                                          where t1.CompanyId == companyId && t1.IsActive
                                                          && (voucherTypeId > 0 ? t1.VoucherTypeId == voucherTypeId : t1.VoucherTypeId > 0)
                                                          && t1.VoucherDate >= fromDate && t1.VoucherDate <= toDate
                                                          && t1.IsSubmit == true
                                                          && t4.IsActive
                                                          //&& t4.IsRequisitionVoucher
                                                          && t9.Id == empId
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name,
                                                              RequisitionId = t4.BillRequsitionMasterId,
                                                              IsRequisitionVoucher = t4.IsRequisitionVoucher,
                                                              RequisitionNo = t5.BillRequisitionNo,
                                                              VoucherRequisitionMasterMapId = t4.VoucherBRMapMasterId,
                                                              InitiatorAprrovalStatusId = t4.ApprovalStatusId ?? 0,
                                                              CheckerAprrovalStatusId = t7.AprrovalStatusId,
                                                              ApproverAprrovalStatusId = t8.AprrovalStatusId,
                                                              EmpId = empId,
                                                              EmployeeId = t9.EmployeeId

                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());

            return voucherModel;
        }

        public async Task<VoucherModel> RequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, /*bool? vStatus,*/ int? voucherTypeId)
        {
            VoucherModel voucherModel = new VoucherModel();
            voucherModel.CompanyId = companyId;
            voucherModel.VoucherTypeId = voucherTypeId;

            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          join t4 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t4.BillRequisitionMasterId into t5_Join
                                                          from t4 in t5_Join.DefaultIfEmpty()
                                                          where t1.CompanyId == companyId && t1.IsActive
                                                          && (voucherTypeId > 0 ? t1.VoucherTypeId == voucherTypeId : t1.VoucherTypeId > 0)
                                                          && t1.VoucherDate >= fromDate && t1.VoucherDate <= toDate
                                                          && t1.IsSubmit == true
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name,
                                                              RequisitionId = t1.BillRequisitionMasterId ?? 0,
                                                              IsRequisitionVoucher = t1.BillRequisitionMasterId == null ? false : true,
                                                              RequisitionNo = t4.BillRequisitionNo ?? "N/A",
                                                              AprrovalStatusId = (int)(t1.ApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.ApprovalStatusId),
                                                              CheckerAprrovalStatusId = (int)(t1.CheckerApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.CheckerApprovalStatusId),
                                                              ApproverAprrovalStatusId = (int)(t1.ApproverApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.ApproverApprovalStatusId)
                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());

            return voucherModel;
        }

        public async Task<VoucherModel> CheckerGetRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, /*bool? vStatus,*/ int? voucherTypeId)
        {
            VoucherModel voucherModel = new VoucherModel();
            voucherModel.CompanyId = companyId;
            voucherModel.VoucherTypeId = voucherTypeId;
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          join t4 in _context.VoucherBRMapMasters on t1.VoucherId equals t4.VoucherId into t4_Join
                                                          from t4 in t4_Join.DefaultIfEmpty()


                                                          join t6 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Initiator && x.IsActive) on t4.VoucherBRMapMasterId equals t6.VoucherBRMapMasterId into t6_Join
                                                          from t6 in t6_Join.DefaultIfEmpty()

                                                          join t7 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Checker && x.IsActive) on t4.VoucherBRMapMasterId equals t7.VoucherBRMapMasterId into t7_Join
                                                          from t7 in t7_Join.DefaultIfEmpty()

                                                          join t8 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Approver && x.IsActive) on t4.VoucherBRMapMasterId equals t8.VoucherBRMapMasterId into t8_Join
                                                          from t8 in t8_Join.DefaultIfEmpty()

                                                          join t5 in _context.BillRequisitionMasters on t4.BillRequsitionMasterId equals t5.BillRequisitionMasterId into t5_Join
                                                          from t5 in t5_Join.DefaultIfEmpty()

                                                          where t1.CompanyId == companyId && t1.IsActive
                                                          && (voucherTypeId > 0 ? t1.VoucherTypeId == voucherTypeId : t1.VoucherTypeId > 0)
                                                          && t1.VoucherDate >= fromDate && t1.VoucherDate <= toDate
                                                          && t1.IsSubmit == true
                                                          && t4.IsActive
                                                          //&& t4.IsRequisitionVoucher
                                                          && t6.AprrovalStatusId == (int)EnumBillRequisitionStatus.Approved
                                                          && t6.AprrovalStatusId != (int)EnumBillRequisitionStatus.Pending
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name,
                                                              RequisitionId = t4.BillRequsitionMasterId,
                                                              RequisitionNo = t5.BillRequisitionNo,
                                                              IsRequisitionVoucher = t4.IsRequisitionVoucher,
                                                              VoucherRequisitionMasterMapId = t4.VoucherBRMapMasterId,
                                                              InitiatorAprrovalStatusId = t6.AprrovalStatusId,
                                                              CheckerAprrovalStatusId = t7.AprrovalStatusId,
                                                              ApproverAprrovalStatusId = t8.AprrovalStatusId

                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());

            return voucherModel;
        }

        public async Task<VoucherModel> CheckerRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, /*bool? vStatus,*/ int? voucherTypeId)
        {
            VoucherModel voucherModel = new VoucherModel();
            voucherModel.CompanyId = companyId;
            voucherModel.VoucherTypeId = voucherTypeId;
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          join t4 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t4.BillRequisitionMasterId into t5_Join
                                                          from t4 in t5_Join.DefaultIfEmpty()
                                                          where t1.CompanyId == companyId && t1.IsActive
                                                          && (voucherTypeId > 0 ? t1.VoucherTypeId == voucherTypeId : t1.VoucherTypeId > 0)
                                                          && t1.VoucherDate >= fromDate && t1.VoucherDate <= toDate
                                                          && t1.IsSubmit == true
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name,
                                                              RequisitionId = t1.BillRequisitionMasterId ?? 0,
                                                              IsRequisitionVoucher = t1.BillRequisitionMasterId == null ? false : true,
                                                              RequisitionNo = t4.BillRequisitionNo ?? "N/A",
                                                              AprrovalStatusId = (int)(t1.ApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.ApprovalStatusId),
                                                              CheckerAprrovalStatusId = (int)(t1.CheckerApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.CheckerApprovalStatusId),
                                                              ApproverAprrovalStatusId = (int)(t1.ApproverApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.ApproverApprovalStatusId)
                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());

            return voucherModel;
        }

        public async Task<VoucherModel> ApproverGetRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, /*bool? vStatus,*/ int? voucherTypeId)
        {
            VoucherModel voucherModel = new VoucherModel();
            voucherModel.CompanyId = companyId;
            voucherModel.VoucherTypeId = voucherTypeId;
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          join t4 in _context.VoucherBRMapMasters on t1.VoucherId equals t4.VoucherId into t4_Join
                                                          from t4 in t4_Join.DefaultIfEmpty()

                                                          join t6 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Initiator && x.IsActive) on t4.VoucherBRMapMasterId equals t6.VoucherBRMapMasterId into t6_Join
                                                          from t6 in t6_Join.DefaultIfEmpty()

                                                          join t7 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Checker && x.IsActive) on t4.VoucherBRMapMasterId equals t7.VoucherBRMapMasterId into t7_Join
                                                          from t7 in t7_Join.DefaultIfEmpty()

                                                          join t8 in _context.VoucherBRMapMasterApprovals.Where(x => x.SignatoryId == (int)EnumVoucherRequisitionSignatory.Approver && x.IsActive) on t4.VoucherBRMapMasterId equals t8.VoucherBRMapMasterId into t8_Join
                                                          from t8 in t8_Join.DefaultIfEmpty()

                                                          join t5 in _context.BillRequisitionMasters on t4.BillRequsitionMasterId equals t5.BillRequisitionMasterId into t5_Join
                                                          from t5 in t5_Join.DefaultIfEmpty()


                                                          where t1.CompanyId == companyId && t1.IsActive
                                                          && (voucherTypeId > 0 ? t1.VoucherTypeId == voucherTypeId : t1.VoucherTypeId > 0)
                                                          && t1.VoucherDate >= fromDate && t1.VoucherDate <= toDate
                                                          && t1.IsSubmit == true
                                                          && t4.IsActive
                                                          //&& t4.IsRequisitionVoucher
                                                          && t6.AprrovalStatusId == (int)EnumBillRequisitionStatus.Approved
                                                          //&& t7.AprrovalStatusId == (int)EnumBillRequisitionStatus.Approved
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name,
                                                              IsRequisitionVoucher = t4.IsRequisitionVoucher,
                                                              RequisitionId = t4.BillRequsitionMasterId,
                                                              RequisitionNo = t5.BillRequisitionNo,
                                                              VoucherRequisitionMasterMapId = t4.VoucherBRMapMasterId,
                                                              InitiatorAprrovalStatusId = t6.AprrovalStatusId,
                                                              CheckerAprrovalStatusId = t7.AprrovalStatusId,
                                                              ApproverAprrovalStatusId = t8.AprrovalStatusId

                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());

            return voucherModel;
        }

        public async Task<VoucherModel> ApproverRequisitionVouchersApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, /*bool? vStatus,*/ int? voucherTypeId)
        {
            VoucherModel voucherModel = new VoucherModel();
            voucherModel.CompanyId = companyId;
            voucherModel.VoucherTypeId = voucherTypeId;
            voucherModel.DataList = await Task.Run(() => (from t1 in _context.Vouchers
                                                          join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                          join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                          join t4 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t4.BillRequisitionMasterId into t5_Join
                                                          from t4 in t5_Join.DefaultIfEmpty()
                                                          where t1.CompanyId == companyId && t1.IsActive
                                                          && (voucherTypeId > 0 ? t1.VoucherTypeId == voucherTypeId : t1.VoucherTypeId > 0)
                                                          && t1.VoucherDate >= fromDate && t1.VoucherDate <= toDate
                                                          && t1.IsSubmit == true
                                                          && t1.CheckerApprovalStatusId == (int)EnumVoucherApprovalStatus.Approved
                                                          select new VoucherModel
                                                          {
                                                              VoucherId = t1.VoucherId,
                                                              VoucherDate = t1.VoucherDate,
                                                              Narration = t1.Narration,
                                                              VoucherNo = t1.VoucherNo,
                                                              VoucherTypeId = t1.VoucherTypeId,
                                                              VoucherTypeName = t2.Name,
                                                              CompanyId = t1.CompanyId,
                                                              CreateDate = t1.CreateDate,
                                                              ChqNo = t1.ChqNo,
                                                              ChqDate = t1.ChqDate,
                                                              ChqName = t1.ChqName,
                                                              IsStock = t1.IsStock,
                                                              IsSubmit = t1.IsSubmit,
                                                              IsIntegrated = t1.IsIntegrated,
                                                              CostCenterName = t3.Name,
                                                              RequisitionId = t1.BillRequisitionMasterId ?? 0,
                                                              IsRequisitionVoucher = t1.BillRequisitionMasterId == null ? false : true,
                                                              RequisitionNo = t4.BillRequisitionNo ?? "N/A",
                                                              AprrovalStatusId = (int)(t1.ApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.ApprovalStatusId),
                                                              CheckerAprrovalStatusId = (int)(t1.CheckerApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.CheckerApprovalStatusId),
                                                              ApproverAprrovalStatusId = (int)(t1.ApproverApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.ApproverApprovalStatusId)
                                                          }).OrderByDescending(x => x.VoucherId).AsEnumerable());

            return voucherModel;
        }

        public async Task<VMJournalSlave> GetVoucherRequisitionMapDetailsWithApproval(int companyId, int voucherId)
        {
            VMJournalSlave vmJournalSlave = new VMJournalSlave();
            var reqVoucherMaster = _context.VoucherBRMapMasters.First(x => x.VoucherId == voucherId);
            if (reqVoucherMaster != null && reqVoucherMaster.IsRequisitionVoucher)
            {
                vmJournalSlave = await Task.Run(() => (from t1 in _context.Vouchers.Where(x => x.IsActive && x.VoucherId == voucherId && x.CompanyId == companyId)
                                                       join t4 in _context.VoucherTypes on t1.VoucherTypeId equals t4.VoucherTypeId
                                                       join t2 in _context.Companies on t1.CompanyId equals t2.CompanyId
                                                       join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                       //  join t5 in _db.HeadGLs on t1.VirtualHeadId equals t5.Id
                                                       join t5 in _context.VoucherBRMapMasters on t1.VoucherId equals t5.VoucherId into t5_Join
                                                       from t5 in t5_Join.DefaultIfEmpty()
                                                       join t6 in _context.BillRequisitionMasters on t5.BillRequsitionMasterId equals t6.BillRequisitionMasterId into t6_Join
                                                       from t6 in t6_Join.DefaultIfEmpty()
                                                       select new VMJournalSlave
                                                       {
                                                           VoucherId = t1.VoucherId,
                                                           Accounting_CostCenterName = t3.Name,
                                                           VoucherNo = t1.VoucherNo,
                                                           Date = t1.VoucherDate,
                                                           Narration = t1.Narration,
                                                           CompanyFK = t1.CompanyId,
                                                           Status = t1.VoucherStatus,
                                                           ChqDate = t1.ChqDate,
                                                           ChqName = t1.ChqName,
                                                           ChqNo = t1.ChqNo,
                                                           Accounting_CostCenterFK = t1.Accounting_CostCenterFk,
                                                           Accounting_BankOrCashId = t1.VirtualHeadId,
                                                           //BankOrCashNane = "[" + t5.AccCode + "] " + t5.AccName,
                                                           BillRequisitionId = t5.BillRequsitionMasterId,
                                                           RequisitionNo = t6.BillRequisitionNo,
                                                           RequisitionInitiator = t6.CreatedBy,
                                                           CompanyName = t2.Name,
                                                           IsSubmit = t1.IsSubmit,


                                                       }).FirstOrDefault());

                vmJournalSlave.DataListDetails = await Task.Run(() => (from t1 in _context.VoucherDetails.Where(x => x.IsActive && x.VoucherId == voucherId && !x.IsVirtual)
                                                                       join t2 in _context.HeadGLs on t1.AccountHeadId equals t2.Id
                                                                       join t3 in _context.VoucherBRMapDetails on t1.VoucherDetailId equals t3.VoucherDetailId into t3_Join
                                                                       from t3 in t3_Join.DefaultIfEmpty()
                                                                       join t4 in _context.Products on t3.ProductId equals t4.ProductId into t4_Join
                                                                       from t4 in t4_Join.DefaultIfEmpty()
                                                                       select new VMJournalSlave
                                                                       {
                                                                           VoucherDetailId = t1.VoucherDetailId,
                                                                           AccountingHeadName = t2.AccName,
                                                                           Code = t2.AccCode,
                                                                           Credit = t1.CreditAmount,
                                                                           Debit = t1.DebitAmount,
                                                                           Particular = t1.Particular,
                                                                           RequisitionMaterialId = t3.ProductId,
                                                                           ApprovedQty = t3.ApprovedQty,
                                                                           UnitRate = t3.ApprovedUnitRate,
                                                                           MaterialName = t4.ProductName,
                                                                       }).OrderByDescending(x => x.VoucherDetailId).AsEnumerable());
                if ((vmJournalSlave.DataListDetails?.Count() ?? 0) > 0)
                {
                    vmJournalSlave.Particular = vmJournalSlave.DataListDetails.OrderByDescending(x => x.VoucherDetailId).Select(x => x.Particular).FirstOrDefault();
                }
            }
            else
            {
                vmJournalSlave = await Task.Run(() => (from t1 in _context.Vouchers.Where(x => x.IsActive && x.VoucherId == voucherId && x.CompanyId == companyId)
                                                       join t4 in _context.VoucherTypes on t1.VoucherTypeId equals t4.VoucherTypeId
                                                       join t2 in _context.Companies on t1.CompanyId equals t2.CompanyId
                                                       join t3 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t3.CostCenterId
                                                       //  join t5 in _db.HeadGLs on t1.VirtualHeadId equals t5.Id
                                                       join t5 in _context.VoucherBRMapMasters on t1.VoucherId equals t5.VoucherId into t5_Join
                                                       from t5 in t5_Join.DefaultIfEmpty()
                                                       select new VMJournalSlave
                                                       {
                                                           VoucherId = t1.VoucherId,
                                                           Accounting_CostCenterName = t3.Name,
                                                           VoucherNo = t1.VoucherNo,
                                                           Date = t1.VoucherDate,
                                                           Narration = t1.Narration,
                                                           CompanyFK = t1.CompanyId,
                                                           Status = t1.VoucherStatus,
                                                           ChqDate = t1.ChqDate,
                                                           ChqName = t1.ChqName,
                                                           ChqNo = t1.ChqNo,
                                                           Accounting_CostCenterFK = t1.Accounting_CostCenterFk,
                                                           Accounting_BankOrCashId = t1.VirtualHeadId,
                                                           //BankOrCashNane = "[" + t5.AccCode + "] " + t5.AccName,
                                                           CreatedBy = t1.CreatedBy,

                                                           CompanyName = t2.Name,
                                                           IsSubmit = t1.IsSubmit,


                                                       }).FirstOrDefault());

                vmJournalSlave.DataListDetails = await Task.Run(() => (from t1 in _context.VoucherDetails.Where(x => x.IsActive && x.VoucherId == voucherId && !x.IsVirtual)
                                                                       join t2 in _context.HeadGLs on t1.AccountHeadId equals t2.Id
                                                                       join t3 in _context.VoucherBRMapDetails on t1.VoucherDetailId equals t3.VoucherDetailId into t3_Join
                                                                       from t3 in t3_Join.DefaultIfEmpty()
                                                                       join t4 in _context.Products on t3.ProductId equals t4.ProductId into t4_Join
                                                                       from t4 in t4_Join.DefaultIfEmpty()
                                                                       select new VMJournalSlave
                                                                       {
                                                                           VoucherDetailId = t1.VoucherDetailId,
                                                                           AccountingHeadName = t2.AccName,
                                                                           Code = t2.AccCode,
                                                                           Credit = t1.CreditAmount,
                                                                           Debit = t1.DebitAmount,
                                                                           Particular = t1.Particular,
                                                                           RequisitionMaterialId = t3.ProductId,
                                                                           ApprovedQty = t3.ApprovedQty,
                                                                           UnitRate = t3.ApprovedUnitRate,
                                                                           MaterialName = t4.ProductName,
                                                                       }).OrderByDescending(x => x.VoucherDetailId).AsEnumerable());
                if ((vmJournalSlave.DataListDetails?.Count() ?? 0) > 0)
                {
                    vmJournalSlave.Particular = vmJournalSlave.DataListDetails.OrderByDescending(x => x.VoucherDetailId).Select(x => x.Particular).FirstOrDefault();
                }
            }


            vmJournalSlave.ApprovalList = await Task.Run(() => (from t1 in _context.VoucherBRMapMasterApprovals.Where(x => x.IsActive && x.VoucherBRMapMasterId == reqVoucherMaster.VoucherBRMapMasterId)

                                                                join t3 in _context.Employees on t1.EmployeeId equals t3.Id into t3_Join
                                                                from t3 in t3_Join.DefaultIfEmpty()
                                                                select new BRVoucherApprovalModel
                                                                {
                                                                    VoucherBRMapMasterApprovalId = t1.VoucherBRMapMasterApprovalId,
                                                                    VoucherBRMapMasterId = t1.VoucherBRMapMasterId,
                                                                    VoucherId = t1.VoucherId,
                                                                    SignatoryId = t1.SignatoryId,
                                                                    AprrovalStatusId = t1.AprrovalStatusId,
                                                                    IsSupremeApproved = t1.IsSupremeApproved,
                                                                    EmployeeId = t1.EmployeeId,
                                                                    EmployeeName = t3.EmployeeId + "-" + t3.Name,
                                                                }).OrderBy(x => x.VoucherBRMapMasterApprovalId).ToList());
            return vmJournalSlave;
        }

        public async Task<VMJournalSlave> VoucherRequisitionMapDetailsWithApproval(int companyId, int voucherId)
        {
            VMJournalSlave vmJournalSlave = new VMJournalSlave();

            vmJournalSlave = await Task.Run(() => (from t1 in _context.Vouchers.Where(x => x.IsActive && x.VoucherId == voucherId && x.CompanyId == companyId)
                                                   join t2 in _context.VoucherTypes on t1.VoucherTypeId equals t2.VoucherTypeId
                                                   join t3 in _context.Companies on t1.CompanyId equals t3.CompanyId
                                                   join t4 in _context.Accounting_CostCenter on t1.Accounting_CostCenterFk equals t4.CostCenterId
                                                   join t5 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t5.BillRequisitionMasterId into t5_Join
                                                   from t5 in t5_Join.DefaultIfEmpty()
                                                   join t6 in _context.BillRequisitionApprovals
                                                   .Where(x => x.SignatoryId == (int)EnumBRequisitionSignatory.MD) on t5.BillRequisitionMasterId equals t6.BillRequisitionMasterId into t6_Join
                                                   from t6 in t6_Join.DefaultIfEmpty()
                                                   join t7 in _context.Employees on t5.CreatedBy equals t7.EmployeeId into t7_ReqInitiator
                                                   from t7 in t7_ReqInitiator.DefaultIfEmpty()
                                                   join t8 in _context.Employees on t1.CreatedBy equals t8.EmployeeId into t8_Join
                                                   from t8 in t8_Join.DefaultIfEmpty()
                                                   select new VMJournalSlave
                                                   {
                                                       VoucherId = t1.VoucherId,
                                                       Accounting_CostCenterName = t4.Name,
                                                       VoucherNo = t1.VoucherNo,
                                                       Date = t1.VoucherDate,
                                                       Narration = t1.Narration,
                                                       CompanyFK = t1.CompanyId,
                                                       CompanyName = t3.Name,
                                                       Status = t1.VoucherStatus,
                                                       ChqDate = t1.ChqDate,
                                                       ChqName = t1.ChqName,
                                                       ChqNo = t1.ChqNo,
                                                       Accounting_CostCenterFK = t1.Accounting_CostCenterFk,
                                                       Accounting_BankOrCashId = t1.VirtualHeadId,
                                                       CreatedBy = t1.CreatedBy + " - " + t8.Name,
                                                       IsSubmit = t1.IsSubmit,
                                                       RequisitionId = t1.BillRequisitionMasterId ?? 0,
                                                       BankOrCashNane = t6.PaymentMethod,
                                                       RequisitionInitiator = t5.CreatedBy + " - " + t7.Name,
                                                       IsRequisitionVoucher = t1.BillRequisitionMasterId == null ? false : true,
                                                       RequisitionNo = t5.BillRequisitionNo ?? "N/A",
                                                       AprrovalStatusId = (int)(t1.ApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.ApprovalStatusId),
                                                       CheckerAprrovalStatusId = (int)(t1.CheckerApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.CheckerApprovalStatusId),
                                                       ApproverAprrovalStatusId = (int)(t1.ApproverApprovalStatusId == null ? (int)EnumVoucherApprovalStatus.Pending : t1.ApproverApprovalStatusId)
                                                   }).FirstOrDefault());

            vmJournalSlave.DataListDetails = await Task.Run(() => (from t1 in _context.VoucherDetails.Where(x => x.IsActive && x.VoucherId == voucherId && !x.IsVirtual)
                                                                   join t2 in _context.HeadGLs on t1.AccountHeadId equals t2.Id
                                                                   select new VMJournalSlave
                                                                   {
                                                                       VoucherDetailId = t1.VoucherDetailId,
                                                                       AccountingHeadName = t2.AccName,
                                                                       Code = t2.AccCode,
                                                                       Credit = t1.CreditAmount,
                                                                       Debit = t1.DebitAmount,
                                                                       Particular = t1.Particular,
                                                                   }).OrderByDescending(x => x.VoucherDetailId).AsEnumerable());
            if ((vmJournalSlave.DataListDetails?.Count() ?? 0) > 0)
            {
                vmJournalSlave.Particular = vmJournalSlave.DataListDetails.OrderByDescending(x => x.VoucherDetailId).Select(x => x.Particular).FirstOrDefault();
            }

            return vmJournalSlave;
        }

        public async Task<long> CheckerVoucherRequisitionApproval(VMJournalSlave vmJournalSlave)
        {
            long result = -1;
            var empId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            Voucher model = _context.Vouchers.Find(vmJournalSlave.VoucherId);

            var voucherRequisitionMapMaster = _context.VoucherBRMapMasters.FirstOrDefault(s => s.VoucherId == model.VoucherId);
            var VRApproval = _context.VoucherBRMapMasterApprovals.FirstOrDefault(s => s.VoucherBRMapMasterId == voucherRequisitionMapMaster.VoucherBRMapMasterId && s.SignatoryId == (int)EnumVoucherRequisitionSignatory.Checker);
            VRApproval.VoucherId = model.VoucherId;
            VRApproval.EmployeeId = empId;
            if (vmJournalSlave.ActionId == (int)ActionEnum.UnApprove)
            {
                VRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
                voucherRequisitionMapMaster.ApprovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
                VRApproval.Reasons = vmJournalSlave.Reason;
            }
            else
            {
                VRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
            }

            VRApproval.ModifiedDate = DateTime.Now;
            VRApproval.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;

            if (await _context.SaveChangesAsync() > 0)
            {
                result = model.VoucherId;
            }

            return result;
        }

        public async Task<long> VoucherCheckerApproval(VMJournalSlave vmJournalSlave)
        {
            long result = -1;
            Voucher voucherModel = _context.Vouchers.FirstOrDefault(x => x.VoucherId == vmJournalSlave.VoucherId);

            if (vmJournalSlave.ActionId == (int)ActionEnum.UnApprove)
            {
                voucherModel.CheckerApprovalStatusId = (int)EnumVoucherApprovalStatus.Rejected;
                //voucherModel.CheckerRemarks = vmJournalSlave.Reason;
            }
            else
            {
                voucherModel.CheckerApprovalStatusId = (int)EnumVoucherApprovalStatus.Approved;
            }

            voucherModel.CheckerApprovedBy = System.Web.HttpContext.Current.User.Identity.Name;
            voucherModel.CheckerApprovedOn = DateTime.Now;

            if (await _context.SaveChangesAsync() > 0)
            {
                result = voucherModel.VoucherId;
            }

            return result;
        }

        public async Task<long> ApproverVoucherRequisitionApproval(VMJournalSlave vmJournalSlave)
        {
            long result = -1;
            var empId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            Voucher model = _context.Vouchers.Find(vmJournalSlave.VoucherId);

            var voucherRequisitionMapMaster = _context.VoucherBRMapMasters.FirstOrDefault(s => s.VoucherId == model.VoucherId);
            var VRApproval = _context.VoucherBRMapMasterApprovals.FirstOrDefault(s => s.VoucherBRMapMasterId == voucherRequisitionMapMaster.VoucherBRMapMasterId && s.SignatoryId == (int)EnumVoucherRequisitionSignatory.Approver);
            VRApproval.VoucherId = model.VoucherId;
            VRApproval.EmployeeId = empId;

            if (vmJournalSlave.ActionId == (int)ActionEnum.UnApprove)
            {
                VRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
                voucherRequisitionMapMaster.ApprovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
                VRApproval.Reasons = vmJournalSlave.Reason;
            }
            else
            {
                VRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
                VRApproval.IsSupremeApproved = true;
                voucherRequisitionMapMaster.ApprovalStatusId = (int)EnumBillRequisitionStatus.Approved;
            }

            VRApproval.ModifiedDate = DateTime.Now;
            VRApproval.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;

            model.ApprovalStatusId = (int)EnumVoucherApprovalStatus.Approved;
            model.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;

            if (await _context.SaveChangesAsync() > 0)
            {

                result = model.VoucherId;
            }

            return result;
        }

        public async Task<long> VoucherApproverApproval(VMJournalSlave vmJournalSlave)
        {
            long result = -1;
            Voucher voucherModel = _context.Vouchers.Find(vmJournalSlave.VoucherId);

            if (vmJournalSlave.ActionId == (int)ActionEnum.UnApprove)
            {
                voucherModel.ApproverApprovalStatusId = (int)EnumVoucherApprovalStatus.Rejected;
                voucherModel.ApprovalStatusId = (int)EnumVoucherApprovalStatus.Rejected;
                voucherModel.ApprovarRemarks = vmJournalSlave.Reason;
                voucherModel.ApproverApprovedBy = System.Web.HttpContext.Current.User.Identity.Name;
                voucherModel.ApproverApprovedOn = DateTime.Now;
                voucherModel.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                voucherModel.ModifiedDate = DateTime.Now;

                if (await _context.SaveChangesAsync() > 0)
                {
                    result = voucherModel.VoucherId;
                }
            }

            if (vmJournalSlave.ActionId == (int)ActionEnum.Approve)
            {
                voucherModel.ApproverApprovalStatusId = (int)EnumVoucherApprovalStatus.Approved;
                voucherModel.ApprovalStatusId = (int)EnumVoucherApprovalStatus.Approved;
                voucherModel.ApprovarRemarks = vmJournalSlave.Reason;
                voucherModel.ApproverApprovedBy = System.Web.HttpContext.Current.User.Identity.Name;
                voucherModel.ApproverApprovedOn = DateTime.Now;
                voucherModel.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                voucherModel.ModifiedDate = DateTime.Now;

                if (await _context.SaveChangesAsync() > 0)
                {
                    decimal payAmount = 0;
                    var chequeRegisterHistory = _context.VoucherPaymentChequeHistories.FirstOrDefault(x => x.VoucherId == voucherModel.VoucherId);
                    if (chequeRegisterHistory.PaymentId != null)
                    {
                        var paymentMaster = _context.PaymentMasters.FirstOrDefault(x => x.PaymentMasterId == chequeRegisterHistory.PaymentId);
                        var paymentDetail = _context.Payments.FirstOrDefault(x => x.PaymentMasterId == chequeRegisterHistory.PaymentId);
                        payAmount = (decimal)paymentDetail.OutAmount;
                    }
                    else
                    {
                        var voucherDetail = _context.VoucherDetails.Where(x => x.VoucherId == voucherModel.VoucherId);
                        payAmount = (decimal)voucherDetail.Sum(x => x.DebitAmount);
                    }

                    ChequeRegister newCheque = new ChequeRegister();

                    if (voucherModel.BillRequisitionMasterId > 0)
                    {
                        newCheque.ProjectId = (int)voucherModel.Accounting_CostCenterFk;
                        newCheque.RequisitionMasterId = voucherModel.BillRequisitionMasterId;
                        newCheque.SupplierId = chequeRegisterHistory.VendorId;
                        newCheque.ChequeBookId = chequeRegisterHistory.ChequeBookId;
                        newCheque.PayTo = chequeRegisterHistory.PayTo;
                        newCheque.ChequeDate = chequeRegisterHistory.IssueDate;
                        newCheque.IssueDate = DateTime.Now.Date;
                        newCheque.ChequeNo = int.Parse(chequeRegisterHistory.ChequeNo);
                        newCheque.Amount = payAmount;
                        newCheque.ClearingDate = DateTime.Now.Date;
                        newCheque.Remarks = "The cheque is electronically generated.";
                        newCheque.IsSigned = false;
                        newCheque.HasPDF = false;
                        newCheque.IsCanceled = false;
                        newCheque.IsCancelRequest = false;
                        newCheque.CreatedBy = voucherModel.ModifiedBy;
                        newCheque.CreatedOn = DateTime.Now;
                        newCheque.IsActive = true;
                    }
                    else
                    {
                        newCheque.ProjectId = (int)voucherModel.Accounting_CostCenterFk;
                        newCheque.SupplierId = chequeRegisterHistory.VendorId;
                        newCheque.ChequeBookId = chequeRegisterHistory.ChequeBookId;
                        newCheque.PayTo = chequeRegisterHistory.PayTo;
                        newCheque.ChequeDate = chequeRegisterHistory.IssueDate;
                        newCheque.IssueDate = DateTime.Now.Date;
                        newCheque.ChequeNo = int.Parse(chequeRegisterHistory.ChequeNo);
                        newCheque.Amount = payAmount;
                        newCheque.ClearingDate = DateTime.Now.Date;
                        newCheque.Remarks = "The cheque is electronically generated.";
                        newCheque.IsSigned = false;
                        newCheque.HasPDF = false;
                        newCheque.IsCanceled = false;
                        newCheque.IsCancelRequest = false;
                        newCheque.CreatedBy = voucherModel.ModifiedBy;
                        newCheque.CreatedOn = DateTime.Now;
                        newCheque.IsActive = true;
                    }

                    _context.ChequeRegisters.Add(newCheque);

                    if (_context.SaveChanges() > 0)
                    {
                        chequeRegisterHistory.IsRegistered = true;
                        _context.SaveChanges();

                        if (chequeRegisterHistory.IsRegistered)
                        {
                            var chequeBook = _context.ChequeBooks.FirstOrDefault(x => x.ChequeBookId == newCheque.ChequeBookId);

                            if (chequeBook != null)
                            {
                                chequeBook.UsedBookPage = ++chequeBook.UsedBookPage;
                                voucherModel.ChequeRegisterId = newCheque.ChequeRegisterId;
                                _context.SaveChanges();
                                result = voucherModel.VoucherId;
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion

    }
}
