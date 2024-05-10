using KGERP.Data.Models;
using KGERP.Service.Implementation.Accounting;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Configuration;
using System.Threading.Tasks;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace KGERP.Service.Implementation
{
    public class ChequeRegisterService : IChequeRegisterService
    {
        private readonly ERPEntities _context;
        private readonly ConfigurationService _configurationService;
        public ChequeRegisterService(ERPEntities context, ConfigurationService configurationService)
        {
            _context = context;
            _configurationService = configurationService;
        }

        public async Task<bool> Add(ChequeRegisterModel model)
        {
            bool sendData = false;

            if (model is null)
            {
                return sendData;
            }

            if (model.SupplierId > 0)
            {
                ChequeRegister data = new ChequeRegister()
                {
                    ChequeBookId = model.ChequeBookId,
                    ProjectId = model.ProjectId,
                    SupplierId = model.SupplierId,
                    PayTo = model.PayTo,
                    IssueDate = model.IssueDate,
                    ChequeDate = model.ChequeDate,
                    ChequeNo = model.ChequeNo,
                    Amount = model.Amount,
                    ClearingDate = model.ClearingDate,
                    Remarks = model.Remarks,
                    IsSigned = false,
                    IsCanceled = false,
                    HasPDF = false,
                    IsCancelRequest = false,
                    PrintCount = 0,
                    IsActive = true,
                    CreatedBy = HttpContext.Current.User.Identity.Name,
                    CreatedOn = DateTime.Now
                };
                if (model.RequisitionId > 0)
                {
                    data.RequisitionMasterId = model.RequisitionId;

                }

                _context.ChequeRegisters.Add(data);
                if (await _context.SaveChangesAsync() > 0)
                {
                    if (model.ChequeBookId > 0)
                    {
                        var chequeBook = _context.ChequeBooks.FirstOrDefault(x => x.ChequeBookId == model.ChequeBookId);
                        chequeBook.UsedBookPage = ++chequeBook.UsedBookPage;
                        chequeBook.ModifiedBy = data.CreatedBy;
                        chequeBook.ModifiedOn = DateTime.Now;
                        _context.SaveChanges();
                    }
                    sendData = true;
                }
            }
            else
            {
                ChequeRegister data = new ChequeRegister()
                {
                    ChequeBookId = model.ChequeBookId,
                    ProjectId = model.ProjectId,
                    PayTo = model.PayTo,
                    IssueDate = model.IssueDate,
                    ChequeDate = model.ChequeDate,
                    ChequeNo = model.ChequeNo,
                    Amount = model.Amount,
                    ClearingDate = model.ClearingDate,
                    Remarks = model.Remarks,
                    IsSigned = false,
                    IsCanceled = false,
                    HasPDF = false,
                    IsCancelRequest = false,
                    PrintCount = 0,
                    IsActive = true,
                    CreatedBy = HttpContext.Current.User.Identity.Name,
                    CreatedOn = DateTime.Now
                };
                if (model.RequisitionId > 0)
                {
                    data.RequisitionMasterId = model.RequisitionId;
                }

                _context.ChequeRegisters.Add(data);
                if (await _context.SaveChangesAsync() > 0)
                {
                    if (model.ChequeBookId > 0)
                    {
                        var chequeBook = _context.ChequeBooks.FirstOrDefault(x => x.ChequeBookId == model.ChequeBookId);
                        chequeBook.UsedBookPage = ++chequeBook.UsedBookPage;
                        chequeBook.ModifiedBy = data.CreatedBy;
                        chequeBook.ModifiedOn = DateTime.Now;
                        _context.SaveChanges();
                    }
                    sendData = true;
                }
            }

            return sendData;
        }

        public async Task<bool> Edit(ChequeRegisterModel model)
        {
            bool sendData = false;
            if (model.ID > 0)
            {
                var result = _context.ChequeRegisters.FirstOrDefault(x => x.ChequeRegisterId == model.ID);
                if (result != null)
                {
                    result.ChequeBookId = model.ChequeBookId;
                    result.RequisitionMasterId = model.RequisitionId;
                    result.ProjectId = model.ProjectId;
                    result.SupplierId = model.SupplierId;
                    result.PayTo = model.PayTo;
                    result.IssueDate = model.IssueDate;
                    result.ChequeDate = model.ChequeDate;
                    result.ChequeNo = model.ChequeNo;
                    result.Amount = model.Amount;
                    result.ClearingDate = model.ClearingDate;
                    result.Remarks = model.Remarks;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    result.ModifiedOn = DateTime.Now;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }

        public async Task<bool> Delete(ChequeRegisterModel model)
        {
            bool sendData = false;
            if (model.ChequeRegisterId > 0)
            {
                var result = _context.ChequeRegisters.FirstOrDefault(x => x.ChequeRegisterId == model.ChequeRegisterId);
                if (result != null)
                {
                    result.IsActive = false;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    result.ModifiedOn = DateTime.Now;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }

        public async Task<bool> ChequeSignRequest(long chequeRegisterId)
        {
            bool sendData = false;
            if (chequeRegisterId > 0)
            {
                var result = _context.ChequeRegisters.FirstOrDefault(x => x.ChequeRegisterId == chequeRegisterId);
                if (result != null)
                {
                    if (!(bool)result.IsCanceled)
                    {
                        result.IsSigned = true;
                        result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                        result.ModifiedOn = DateTime.Now;

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            if (result.RequisitionMasterId > 0)
                            {
                                var requisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(x => x.BillRequisitionMasterId == result.RequisitionMasterId);
                                requisitionMaster.PaymentStatus = true;
                                requisitionMaster.ModifiedBy = HttpContext.Current.User.Identity.Name;
                                requisitionMaster.ModifiedDate = DateTime.Now;
                                _context.SaveChanges();
                            }
                            sendData = true;
                        }
                    }
                }
            }

            return sendData;
        }

        public async Task<bool> ChequeCancelRequest(long chequeRegisterId)
        {
            bool sendData = false;
            if (chequeRegisterId > 0)
            {
                var result = _context.ChequeRegisters.FirstOrDefault(x => x.ChequeRegisterId == chequeRegisterId);
                if (result != null)
                {
                    if (!result.IsSigned)
                    {
                        result.IsCanceled = true;
                        result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                        result.ModifiedOn = DateTime.Now;

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            sendData = true;
                        }
                    }
                    else if ((bool)result.IsCancelRequest)
                    {
                        result.IsCanceled = true;
                        result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                        result.ModifiedOn = DateTime.Now;

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            sendData = true;
                        }
                    }
                }
            }

            return sendData;
        }

        public async Task<bool> MakePdf(long chequeRegisterId)
        {
            bool sendData = false;
            if (chequeRegisterId > 0)
            {
                var result = _context.ChequeRegisters.FirstOrDefault(x => x.ChequeRegisterId == chequeRegisterId);
                if (result != null)
                {
                    result.HasPDF = true;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    result.ModifiedOn = DateTime.Now;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }

        public async Task<bool> PrintCount(long chequeRegisterId)
        {
            bool sendData = false;
            if (chequeRegisterId > 0)
            {
                var result = _context.ChequeRegisters.FirstOrDefault(x => x.ChequeRegisterId == chequeRegisterId);
                int printCount = result.PrintCount ?? 0;
                if (result != null)
                {
                    result.PrintCount = ++printCount;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    result.ModifiedOn = DateTime.Now;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }

        public async Task<ChequeRegisterModel> GetChequeRegisterById(long chequeRegisterId)
        {
            ChequeRegisterModel sendData = await (from t1 in _context.ChequeRegisters
                                                  join t2 in _context.BillRequisitionMasters on t1.RequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                  from t2 in t2_Join.DefaultIfEmpty()
                                                  join t3 in _context.Vendors on t1.SupplierId equals t3.VendorId into t3_Join
                                                  from t3 in t3_Join.DefaultIfEmpty()
                                                  join t4 in _context.Accounting_CostCenter on t1.ProjectId equals t4.CostCenterId into t4_Join
                                                  from t4 in t4_Join.DefaultIfEmpty()
                                                  where t1.ChequeRegisterId == chequeRegisterId
                                                  select new ChequeRegisterModel
                                                  {
                                                      ChequeRegisterId = t1.ChequeRegisterId,
                                                      RegisterFor = t1.RequisitionMasterId == null ? (int)EnumChequeRegisterFor.General : (int)EnumChequeRegisterFor.Requisition,
                                                      RequisitionId = (int)(t1.RequisitionMasterId == null ? 0 : t1.RequisitionMasterId),
                                                      RequisitionNo = t2.BillRequisitionNo,
                                                      ProjectId = t1.ProjectId,
                                                      ProjectName = t4.Name,
                                                      SupplierId = (int)t1.SupplierId,
                                                      SupplierName = t3.Name,
                                                      SupplierCode = t3.Code,
                                                      PayTo = t1.PayTo,
                                                      IssueDate = t1.IssueDate,
                                                      ChequeDate = t1.ChequeDate,
                                                      ChequeNo = (int)t1.ChequeNo,
                                                      Amount = t1.Amount,
                                                      ClearingDate = t1.ClearingDate,
                                                      Remarks = t1.Remarks,
                                                      IsSigned = t1.IsSigned,
                                                      HasPDF = t1.HasPDF,
                                                      IsCancelRequest = (bool)(t1.IsCancelRequest == null ? false : t1.IsCancelRequest),
                                                      IsCanceled = (bool)(t1.IsCanceled == null ? false : t1.IsCanceled),
                                                      PrintCount = t1.PrintCount ?? 0,
                                                      CreatedBy = t1.CreatedBy,
                                                      CreatedDate = t1.CreatedOn,
                                                      ModifiedBy = t1.ModifiedBy,
                                                      ModifiedDate = t1.ModifiedOn,
                                                      IsActive = t1.IsActive,
                                                      CompanyFK = 21,
                                                  }).FirstOrDefaultAsync();
            return sendData;
        }

        public List<object> RegisteredRequisitionList(int projectId)
        {
            var getData = (from t1 in _context.ChequeRegisters.Where(x => x.IsActive)
                           join t2 in _context.BillRequisitionMasters on t1.RequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                           from t2 in t2_Join.DefaultIfEmpty()
                           where t2.CostCenterId == projectId
                           select new
                           {
                               Text = t2.BillRequisitionNo,
                               Value = t2.BillRequisitionMasterId
                           }).ToList();
            return getData.Cast<object>().ToList();

        }

        public List<object> ChequePageNo()
        {
            var getData = (from t1 in _context.ChequeRegisters
                           where t1.IsCanceled == false && t1.IsActive
                           select new
                           {
                               ChequeNo = t1.ChequeNo,
                               IssueDate = t1.IssueDate,
                               ChequeRegisterId = t1.ChequeRegisterId
                           }).AsEnumerable()
                           .Select(t1 => new
                           {
                               Text = t1.ChequeNo + " => Issue Date: " + t1.IssueDate.ToString("yyyy-MM-dd"),
                               Value = t1.ChequeRegisterId
                           }).ToList();
            return getData.Cast<object>().ToList();
        }

        public async Task<string> GetPayeeNameBySupplierId(int supplierId)
        {
            var getData = await _context.Vendors.FirstOrDefaultAsync(x => x.VendorId == supplierId && x.IsActive);
            return getData.Name;

        }
        public async Task<List<ChequeRegisterModel>> GetChequeRegisterList(int companyId)
        {
            List<ChequeRegisterModel> sendData = await (from t1 in _context.ChequeRegisters
                                                        join t2 in _context.BillRequisitionMasters on t1.RequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                        join t3 in _context.Vendors on t1.SupplierId equals t3.VendorId into t3_Join
                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                        join t4 in _context.Accounting_CostCenter on t1.ProjectId equals t4.CostCenterId into t4_Join
                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                        where t1.IsActive
                                                        select new ChequeRegisterModel
                                                        {
                                                            ChequeRegisterId = t1.ChequeRegisterId,
                                                            RegisterFor = t1.RequisitionMasterId == null ? (int)EnumChequeRegisterFor.General : (int)EnumChequeRegisterFor.Requisition,
                                                            RequisitionId = (int)(t1.RequisitionMasterId == null ? 0 : t1.RequisitionMasterId),
                                                            RequisitionNo = t2.BillRequisitionNo,
                                                            ProjectId = t1.ProjectId,
                                                            ProjectName = t4.Name,
                                                            SupplierId = t1.SupplierId == null ? 0 : (int)t1.SupplierId,
                                                            SupplierName = t3.Name ?? "N/A",
                                                            SupplierCode = t3.Code,
                                                            PayTo = t1.PayTo,
                                                            IssueDate = t1.IssueDate,
                                                            ChequeDate = t1.ChequeDate,
                                                            ChequeNo = (int)t1.ChequeNo,
                                                            Amount = t1.Amount,
                                                            ClearingDate = t1.ClearingDate,
                                                            Remarks = t1.Remarks,
                                                            IsSigned = t1.IsSigned,
                                                            HasPDF = t1.HasPDF,
                                                            IsCancelRequest = (bool)(t1.IsCancelRequest == null ? false : t1.IsCancelRequest),
                                                            IsCanceled = (bool)(t1.IsCanceled == null ? false : t1.IsCanceled),
                                                            PrintCount = t1.PrintCount ?? 0,
                                                            CreatedBy = t1.CreatedBy,
                                                            CreatedDate = t1.CreatedOn,
                                                            ModifiedBy = t1.ModifiedBy,
                                                            ModifiedDate = t1.ModifiedOn,
                                                            IsActive = t1.IsActive,
                                                            CompanyFK = 21,
                                                        }).ToListAsync();
            return sendData;
        }

        public List<ChequeRegisterModel> CanceledChequeRegisterList()
        {
            List<ChequeRegisterModel> sendData = (from t1 in _context.ChequeRegisters
                                                  join t2 in _context.BillRequisitionMasters on t1.RequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                  from t2 in t2_Join.DefaultIfEmpty()
                                                  join t3 in _context.Vendors on t1.SupplierId equals t3.VendorId into t3_Join
                                                  from t3 in t3_Join.DefaultIfEmpty()
                                                  join t4 in _context.Accounting_CostCenter on t1.ProjectId equals t4.CostCenterId into t4_Join
                                                  from t4 in t4_Join.DefaultIfEmpty()
                                                  join t5 in _context.Employees on t1.RequestedBy equals t5.EmployeeId into t5_Join
                                                  from t5 in t5_Join.DefaultIfEmpty()
                                                  where t1.IsActive && t1.IsCancelRequest == true
                                                  select new ChequeRegisterModel
                                                  {
                                                      ChequeRegisterId = t1.ChequeRegisterId,
                                                      RegisterFor = t1.RequisitionMasterId == null ? (int)EnumChequeRegisterFor.General : (int)EnumChequeRegisterFor.Requisition,
                                                      RequisitionId = (int)(t1.RequisitionMasterId == null ? 0 : t1.RequisitionMasterId),
                                                      RequisitionNo = t2.BillRequisitionNo,
                                                      ProjectId = t1.ProjectId,
                                                      ProjectName = t4.Name,
                                                      SupplierId = t1.SupplierId == null ? 0 : (int)t1.SupplierId,
                                                      SupplierName = t3.Name ?? "N/A",
                                                      SupplierCode = t3.Code,
                                                      PayTo = t1.PayTo,
                                                      IssueDate = t1.IssueDate,
                                                      ChequeDate = t1.ChequeDate,
                                                      ChequeNo = (int)t1.ChequeNo,
                                                      Amount = t1.Amount,
                                                      ClearingDate = t1.ClearingDate,
                                                      Remarks = t1.Remarks,
                                                      IsSigned = t1.IsSigned,
                                                      HasPDF = t1.HasPDF,
                                                      IsCancelRequest = (bool)(t1.IsCancelRequest == null ? false : t1.IsCancelRequest),
                                                      IsCanceled = (bool)(t1.IsCanceled == null ? false : t1.IsCanceled),
                                                      PrintCount = t1.PrintCount ?? 0,
                                                      CreatedBy = t1.CreatedBy,
                                                      CreatedDate = t1.CreatedOn,
                                                      ModifiedBy = t1.ModifiedBy,
                                                      ModifiedDate = t1.ModifiedOn,
                                                      IsActive = t1.IsActive,
                                                      CompanyFK = 21,
                                                      CancelReason = t1.CancelReason ?? "N/A",
                                                      RequestedBy = t1.RequestedBy + " - " + t5.Name ?? "N/A",
                                                      RequestedOn = (DateTime)t1.RequestedOn
                                                  }).ToList();
            return sendData;
        }

        public async Task<List<ChequeRegisterModel>> GetSignedChequeList(int companyId)
        {
            List<ChequeRegisterModel> sendData = await (from t1 in _context.ChequeRegisters
                                                        join t2 in _context.BillRequisitionMasters on t1.RequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                        join t3 in _context.Vendors on t1.SupplierId equals t3.VendorId into t3_Join
                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                        join t4 in _context.Accounting_CostCenter on t1.ProjectId equals t4.CostCenterId into t4_Join
                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                        where t1.IsActive && t1.IsSigned
                                                        select new ChequeRegisterModel
                                                        {
                                                            ChequeRegisterId = t1.ChequeRegisterId,
                                                            RegisterFor = t1.RequisitionMasterId == null ? (int)EnumChequeRegisterFor.General : (int)EnumChequeRegisterFor.Requisition,
                                                            RequisitionId = (int)(t1.RequisitionMasterId == null ? 0 : t1.RequisitionMasterId),
                                                            RequisitionNo = t2.BillRequisitionNo,
                                                            ProjectId = t1.ProjectId,
                                                            ProjectName = t4.Name,
                                                            SupplierId = t1.SupplierId == null ? 0 : (int)t1.SupplierId,
                                                            SupplierName = t3.Name ?? "N/A",
                                                            SupplierCode = t3.Code,
                                                            PayTo = t1.PayTo,
                                                            IssueDate = t1.IssueDate,
                                                            ChequeDate = t1.ChequeDate,
                                                            ChequeNo = (int)t1.ChequeNo,
                                                            Amount = t1.Amount,
                                                            ClearingDate = t1.ClearingDate,
                                                            Remarks = t1.Remarks,
                                                            IsSigned = t1.IsSigned,
                                                            HasPDF = t1.HasPDF,
                                                            IsCancelRequest = (bool)(t1.IsCancelRequest == null ? false : t1.IsCancelRequest),
                                                            IsCanceled = (bool)(t1.IsCanceled == null ? false : t1.IsCanceled),
                                                            PrintCount = t1.PrintCount ?? 0,
                                                            CreatedBy = t1.CreatedBy,
                                                            CreatedDate = t1.CreatedOn,
                                                            ModifiedBy = t1.ModifiedBy,
                                                            ModifiedDate = t1.ModifiedOn,
                                                            IsActive = t1.IsActive,
                                                            CompanyFK = 21,
                                                        }).ToListAsync();
            return sendData;
        }

        public async Task<List<ChequeRegisterModel>> GetGeneratedChequeList(int companyId)
        {
            List<ChequeRegisterModel> sendData = await (from t1 in _context.ChequeRegisters
                                                        join t2 in _context.BillRequisitionMasters on t1.RequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                        join t3 in _context.Vendors on t1.SupplierId equals t3.VendorId into t3_Join
                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                        join t4 in _context.Accounting_CostCenter on t1.ProjectId equals t4.CostCenterId into t4_Join
                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                        where t1.IsActive && t1.HasPDF
                                                        select new ChequeRegisterModel
                                                        {
                                                            ChequeRegisterId = t1.ChequeRegisterId,
                                                            RegisterFor = t1.RequisitionMasterId == null ? (int)EnumChequeRegisterFor.General : (int)EnumChequeRegisterFor.Requisition,
                                                            RequisitionId = (int)(t1.RequisitionMasterId == null ? 0 : t1.RequisitionMasterId),
                                                            RequisitionNo = t2.BillRequisitionNo,
                                                            ProjectId = t1.ProjectId,
                                                            ProjectName = t4.Name,
                                                            SupplierId = t1.SupplierId == null ? 0 : (int)t1.SupplierId,
                                                            SupplierName = t3.Name ?? "N/A",
                                                            SupplierCode = t3.Code,
                                                            PayTo = t1.PayTo,
                                                            IssueDate = t1.IssueDate,
                                                            ChequeDate = t1.ChequeDate,
                                                            ChequeNo = (int)t1.ChequeNo,
                                                            Amount = t1.Amount,
                                                            ClearingDate = t1.ClearingDate,
                                                            Remarks = t1.Remarks,
                                                            IsSigned = t1.IsSigned,
                                                            HasPDF = t1.HasPDF,
                                                            IsCancelRequest = (bool)(t1.IsCancelRequest == null ? false : t1.IsCancelRequest),
                                                            IsCanceled = (bool)(t1.IsCanceled == null ? false : t1.IsCanceled),
                                                            PrintCount = t1.PrintCount ?? 0,
                                                            CreatedBy = t1.CreatedBy,
                                                            CreatedDate = t1.CreatedOn,
                                                            ModifiedBy = t1.ModifiedBy,
                                                            ModifiedDate = t1.ModifiedOn,
                                                            IsActive = t1.IsActive,
                                                            CompanyFK = 21,
                                                        }).ToListAsync();
            return sendData;
        }

        public async Task<List<ChequeRegisterModel>> GetChequeRegisterListByDate(ChequeRegisterModel model)
        {
            List<ChequeRegisterModel> sendData = await (from t1 in _context.ChequeRegisters
                                                        join t2 in _context.BillRequisitionMasters on t1.RequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                        join t3 in _context.Vendors on t1.SupplierId equals t3.VendorId into t3_Join
                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                        join t4 in _context.Accounting_CostCenter on t1.ProjectId equals t4.CostCenterId into t4_Join
                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                        where t1.IssueDate >= model.StrFromDate && t1.IssueDate <= model.StrToDate
                                                        select new ChequeRegisterModel
                                                        {
                                                            ChequeRegisterId = t1.ChequeRegisterId,
                                                            RegisterFor = t1.RequisitionMasterId == null ? (int)EnumChequeRegisterFor.General : (int)EnumChequeRegisterFor.Requisition,
                                                            RequisitionId = (int)t1.RequisitionMasterId,
                                                            RequisitionNo = t2.BillRequisitionNo,
                                                            ProjectId = t1.ProjectId,
                                                            ProjectName = t4.Name,
                                                            SupplierId = (int)t1.SupplierId,
                                                            SupplierName = t3.Name,
                                                            SupplierCode = t3.Code,
                                                            PayTo = t1.PayTo,
                                                            IssueDate = t1.IssueDate,
                                                            ChequeDate = t1.ChequeDate,
                                                            ChequeNo = (int)t1.ChequeNo,
                                                            Amount = t1.Amount,
                                                            ClearingDate = t1.ClearingDate,
                                                            Remarks = t1.Remarks,
                                                            IsSigned = t1.IsSigned,
                                                            HasPDF = t1.HasPDF,
                                                            IsCancelRequest = (bool)(t1.IsCancelRequest == null ? false : t1.IsCancelRequest),
                                                            IsCanceled = (bool)(t1.IsCanceled == null ? false : t1.IsCanceled),
                                                            PrintCount = t1.PrintCount ?? 0,
                                                            CreatedBy = t1.CreatedBy,
                                                            CreatedDate = t1.CreatedOn,
                                                            ModifiedBy = t1.ModifiedBy,
                                                            ModifiedDate = t1.ModifiedOn,
                                                            IsActive = t1.IsActive,
                                                            CompanyFK = 21,
                                                        }).ToListAsync();
            return sendData;
        }

        public async Task<bool> Add(ChequeBookModel model)
        {
            bool sendData = false;

            if (model != null)
            {
                ChequeBook data = new ChequeBook();
                data.BankAccountInfoId = model.BankAccountInfoId;
                data.ChequeBookNo = model.ChequeBookNo;
                data.BookFirstPageNumber = model.BookFirstPageNumber;
                data.BookLastPageNumber = model.BookLastPageNumber;
                data.TotalBookPage = model.TotalBookPage;
                data.UsedBookPage = 0;
                data.Remarks = model.Remarks;
                data.CreatedBy = HttpContext.Current.User.Identity.Name;
                data.CreatedOn = DateTime.Now;
                data.IsActive = true;

                _context.ChequeBooks.Add(data);
                if (await _context.SaveChangesAsync() > 0)
                {
                    sendData = true;
                }
            }

            return sendData;
        }

        public async Task<bool> Edit(ChequeBookModel model)
        {
            bool sendData = false;
            if (model.ID > 0)
            {
                var result = _context.ChequeBooks.FirstOrDefault(x => x.ChequeBookId == model.ID);
                if (result != null)
                {
                    result.BankAccountInfoId = (int)model.BankAccountInfoId;
                    result.ChequeBookNo = model.ChequeBookNo;
                    result.BookFirstPageNumber = model.BookFirstPageNumber;
                    result.BookLastPageNumber = model.BookLastPageNumber;
                    result.TotalBookPage = model.TotalBookPage;
                    result.Remarks = model.Remarks;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    result.ModifiedOn = DateTime.Now;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }

        public async Task<bool> Delete(ChequeBookModel model)
        {
            bool sendData = false;
            if (model.ChequeBookId > 0)
            {
                var result = _context.ChequeBooks.FirstOrDefault(x => x.ChequeBookId == model.ChequeBookId);
                if (result != null)
                {
                    result.IsActive = false;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    result.ModifiedOn = DateTime.Now;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }

        public async Task<ChequeBookModel> GetChequeBookById(long chequeBookId)
        {
            ChequeBookModel sendData = await (from t1 in _context.ChequeBooks.Where(x => x.ChequeBookId == chequeBookId && x.IsActive)
                                              join t2 in _context.BankAccountInfoes on t1.BankAccountInfoId equals t2.BankAccountInfoId into t2_Join
                                              from t2 in t2_Join.DefaultIfEmpty()
                                              join t3 in _context.BankBranches on t2.BranchId equals t3.BankBranchId into t3_Join
                                              from t3 in t3_Join.DefaultIfEmpty()
                                              join t4 in _context.Banks on t2.BankId equals t4.BankId into t4_Join
                                              from t4 in t4_Join.DefaultIfEmpty()
                                              select new ChequeBookModel
                                              {
                                                  ChequeBookId = t1.ChequeBookId,
                                                  BankAccountInfoId = t2.BankAccountInfoId,
                                                  JournalType = t2.AccountTypeId,
                                                  AccountName = t2.AccountName,
                                                  AccountNumber = (long)t2.AccountNumber,
                                                  ChequeBookNo = t1.ChequeBookNo,
                                                  BankId = t4.BankId,
                                                  BankName = t4.Name,
                                                  BankBranchId = t3.BankBranchId,
                                                  BankBranchName = t3.Name,
                                                  BookFirstPageNumber = t1.BookFirstPageNumber,
                                                  BookLastPageNumber = t1.BookLastPageNumber,
                                                  TotalBookPage = t1.TotalBookPage,
                                                  UsedBookPage = t1.UsedBookPage,
                                                  Remarks = t1.Remarks,
                                                  CreatedBy = t1.CreatedBy,
                                                  CreatedDate = t1.CreatedOn,
                                                  ModifiedBy = t1.ModifiedBy,
                                                  ModifiedDate = t1.ModifiedOn,
                                                  IsActive = t1.IsActive,
                                                  CompanyFK = 21,
                                              }).FirstOrDefaultAsync();
            return sendData;
        }

        public async Task<List<ChequeBookModel>> GetChequeBookList(int companyId)
        {
            List<ChequeBookModel> sendData = await (from t1 in _context.ChequeBooks.Where(x => x.IsActive)
                                                    join t2 in _context.BankAccountInfoes on t1.BankAccountInfoId equals t2.BankAccountInfoId into t2_Join
                                                    from t2 in t2_Join.DefaultIfEmpty()
                                                    join t3 in _context.BankBranches on t2.BranchId equals t3.BankBranchId into t3_Join
                                                    from t3 in t3_Join.DefaultIfEmpty()
                                                    join t4 in _context.Banks on t2.BankId equals t4.BankId into t4_Join
                                                    from t4 in t4_Join.DefaultIfEmpty()
                                                    select new ChequeBookModel
                                                    {
                                                        ChequeBookId = t1.ChequeBookId,
                                                        BankAccountInfoId = t2.BankAccountInfoId,
                                                        JournalType = t2.AccountTypeId,
                                                        AccountName = t2.AccountName,
                                                        AccountNumber = (long)t2.AccountNumber,
                                                        ChequeBookNo = t1.ChequeBookNo,
                                                        BankId = t4.BankId,
                                                        BankName = t4.Name,
                                                        BankBranchId = t3.BankBranchId,
                                                        BankBranchName = t3.Name,
                                                        BookFirstPageNumber = t1.BookFirstPageNumber,
                                                        BookLastPageNumber = t1.BookLastPageNumber,
                                                        TotalBookPage = t1.TotalBookPage,
                                                        UsedBookPage = t1.UsedBookPage,
                                                        Remarks = t1.Remarks,
                                                        CreatedBy = t1.CreatedBy,
                                                        CreatedDate = t1.CreatedOn,
                                                        ModifiedBy = t1.ModifiedBy,
                                                        ModifiedDate = t1.ModifiedOn,
                                                        IsActive = t1.IsActive,
                                                        CompanyFK = 21,
                                                    }).ToListAsync();
            return sendData;
        }

        public bool Add(BankAccountInfoModel model)
        {
            bool sendData = false;

            if (model != null)
            {
                BankAccountInfo data = new BankAccountInfo();

                data.BankId = model.BankId;
                data.BranchId = model.BankBranchId;
                data.AccountTypeId = model.AccountTypeId;
                data.AccountName = model.AccountName;
                data.AccountNumber = model.AccountNumber;
                data.Remarks = model.Remarks;
                data.CompanyId = (int)model.CompanyFK;
                data.CreatedBy = HttpContext.Current.User.Identity.Name;
                data.CreatedOn = DateTime.Now;
                data.IsActive = true;

                _context.BankAccountInfoes.Add(data);

                if (_context.SaveChanges() > 0)
                {
                    int headGlParentId = 0;
                    if (data.BankId > 0)
                    {
                        int head5Id = 0;

                        switch (data.AccountTypeId)
                        {
                            case (int)EnumBankAccountType.Current:
                                head5Id = _context.Banks.FirstOrDefault(x => x.BankId == data.BankId)?.CurrentAccountingHeadId ?? 0;
                                break;
                            case (int)EnumBankAccountType.Saving:
                                head5Id = _context.Banks.FirstOrDefault(x => x.BankId == data.BankId)?.SavingAccountingHeadId ?? 0;
                                break;
                            case (int)EnumBankAccountType.Current_JV:
                                head5Id = _context.Banks.FirstOrDefault(x => x.BankId == data.BankId)?.CurrentJVAccountingHeadId ?? 0;
                                break;
                            case (int)EnumBankAccountType.SND:
                                head5Id = _context.Banks.FirstOrDefault(x => x.BankId == data.BankId)?.SNDAccountingHeadId ?? 0;
                                break;
                            case (int)EnumBankAccountType.FDR:
                                head5Id = _context.Banks.FirstOrDefault(x => x.BankId == data.BankId)?.FDRAccountingHeadId ?? 0;
                                break;
                            default:
                                break;
                        }

                        if (head5Id > 0)
                        {
                            headGlParentId = head5Id;
                        }
                    }

                    VMHeadIntegration integration = new VMHeadIntegration
                    {
                        AccName = data.AccountName + "(" + data.AccountNumber + ")",
                        ParentId = headGlParentId,
                        LayerNo = 6,
                        Remarks = "GL Layer",
                        IsIncomeHead = true,
                        CompanyFK = data.CompanyId,
                        CreatedBy = data.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    int headGl = _configurationService.AccHeadGlPush(integration);

                    if (headGl > 0)
                    {
                        var productForAssets = _context.BankAccountInfoes.SingleOrDefault(x => x.BankAccountInfoId == data.BankAccountInfoId);
                        productForAssets.AccountingHeadId = headGl;
                        productForAssets.ModifiedBy = data.CreatedBy;
                        productForAssets.ModifiedOn = DateTime.Now;
                        _context.SaveChanges();
                        ; sendData = true;
                    }
                }
            }

            return sendData;
        }

        public async Task<bool> Edit(BankAccountInfoModel model)
        {
            bool sendData = false;
            if (model.ID > 0)
            {
                var result = _context.BankAccountInfoes.FirstOrDefault(x => x.BankAccountInfoId == model.ID);
                if (result != null)
                {
                    result.BankId = model.BankId;
                    result.BranchId = model.BankBranchId;
                    result.AccountTypeId = model.AccountTypeId;
                    result.AccountName = model.AccountName;
                    result.AccountNumber = model.AccountNumber;
                    result.Remarks = model.Remarks;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    result.ModifiedOn = DateTime.Now;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }

        public async Task<bool> Delete(BankAccountInfoModel model)
        {
            bool sendData = false;
            if (model.BankAccountInfoId > 0)
            {
                var result = _context.BankAccountInfoes.FirstOrDefault(x => x.BankAccountInfoId == model.BankAccountInfoId);
                if (result != null)
                {
                    result.IsActive = false;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    result.ModifiedOn = DateTime.Now;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }

        public async Task<BankAccountInfoModel> GetBankAccountInfoById(long bankAccountInfoId)
        {
            BankAccountInfoModel sendData = await (from t1 in _context.BankAccountInfoes.Where(x => x.BankAccountInfoId == bankAccountInfoId && x.IsActive)
                                                   join t2 in _context.BankBranches on t1.BranchId equals t2.BankBranchId into t2_Join
                                                   from t2 in t2_Join.DefaultIfEmpty()
                                                   join t3 in _context.Banks on t2.BankId equals t3.BankId into t3_Join
                                                   from t3 in t3_Join.DefaultIfEmpty()
                                                   select new BankAccountInfoModel
                                                   {
                                                       BankAccountInfoId = t1.BankAccountInfoId,
                                                       BankId = t3.BankId,
                                                       BankName = t3.Name,
                                                       BankBranchId = t2.BankBranchId,
                                                       BankBranchName = t2.Name,
                                                       AccountTypeId = t1.AccountTypeId,
                                                       AccountName = t1.AccountName,
                                                       AccountNumber = t1.AccountNumber,
                                                       Remarks = t1.Remarks,
                                                       CreatedBy = t1.CreatedBy,
                                                       CreatedDate = t1.CreatedOn,
                                                       ModifiedBy = t1.ModifiedBy,
                                                       ModifiedDate = t1.ModifiedOn,
                                                       IsActive = t1.IsActive,
                                                   }).FirstOrDefaultAsync();
            return sendData;
        }

        public async Task<List<BankAccountInfoModel>> GetBankAccountInfoList(int companyId)
        {
            List<BankAccountInfoModel> sendData = await (from t1 in _context.BankAccountInfoes.Where(x => x.CompanyId == companyId && x.IsActive)
                                                         join t2 in _context.BankBranches on t1.BranchId equals t2.BankBranchId into t2_Join
                                                         from t2 in t2_Join.DefaultIfEmpty()
                                                         join t3 in _context.Banks on t2.BankId equals t3.BankId into t3_Join
                                                         from t3 in t3_Join.DefaultIfEmpty()
                                                         select new BankAccountInfoModel
                                                         {
                                                             BankAccountInfoId = t1.BankAccountInfoId,
                                                             BankId = t3.BankId,
                                                             BankName = t3.Name,
                                                             BankBranchId = t2.BankBranchId,
                                                             BankBranchName = t2.Name,
                                                             AccountTypeId = t1.AccountTypeId,
                                                             AccountName = t1.AccountName,
                                                             AccountNumber = t1.AccountNumber,
                                                             Remarks = t1.Remarks,
                                                             CreatedBy = t1.CreatedBy,
                                                             CreatedDate = t1.CreatedOn,
                                                             ModifiedBy = t1.ModifiedBy,
                                                             ModifiedDate = t1.ModifiedOn,
                                                             IsActive = t1.IsActive,
                                                         }).ToListAsync();
            return sendData;
        }

        public async Task<List<ChequeBookModel>> GetChequeBookListByAccountInfo(int bankAccountInfoId)
        {
            List<ChequeBookModel> sendData = await (from t1 in _context.ChequeBooks.Where(x => x.IsActive)
                                                    join t2 in _context.BankAccountInfoes on t1.BankAccountInfoId equals t2.BankAccountInfoId into t2_Join
                                                    from t2 in t2_Join.DefaultIfEmpty()
                                                    join t3 in _context.BankBranches on t2.BranchId equals t3.BankBranchId into t3_Join
                                                    from t3 in t3_Join.DefaultIfEmpty()
                                                    join t4 in _context.Banks on t2.BankId equals t4.BankId into t4_Join
                                                    from t4 in t4_Join.DefaultIfEmpty()
                                                    where t1.BankAccountInfoId == bankAccountInfoId
                                                    select new ChequeBookModel
                                                    {
                                                        ChequeBookId = t1.ChequeBookId,
                                                        BankAccountInfoId = t2.BankAccountInfoId,
                                                        JournalType = t2.AccountTypeId,
                                                        AccountName = t2.AccountName,
                                                        AccountNumber = (long)t2.AccountNumber,
                                                        ChequeBookNo = t1.ChequeBookNo,
                                                        BankId = t4.BankId,
                                                        BankName = t4.Name,
                                                        BankBranchId = t3.BankBranchId,
                                                        BankBranchName = t3.Name,
                                                        BookFirstPageNumber = t1.BookFirstPageNumber,
                                                        BookLastPageNumber = t1.BookLastPageNumber,
                                                        TotalBookPage = t1.TotalBookPage,
                                                        UsedBookPage = t1.UsedBookPage,
                                                        Remarks = t1.Remarks,
                                                        CreatedBy = t1.CreatedBy,
                                                        CreatedDate = t1.CreatedOn,
                                                        ModifiedBy = t1.ModifiedBy,
                                                        ModifiedDate = t1.ModifiedOn,
                                                        IsActive = t1.IsActive,
                                                        CompanyFK = 21,
                                                    }).ToListAsync();
            return sendData;
        }

        public async Task<object> GetChequeBookInfo(long chequeBookId)
        {
            object sendData = await (from t1 in _context.ChequeBooks.Where(x => x.ChequeBookId == chequeBookId && x.IsActive)
                                     join t2 in _context.BankAccountInfoes on t1.BankAccountInfoId equals t2.BankAccountInfoId into t2_Join
                                     from t2 in t2_Join.DefaultIfEmpty()
                                     join t3 in _context.BankBranches on t2.BranchId equals t3.BankBranchId into t3_Join
                                     from t3 in t3_Join.DefaultIfEmpty()
                                     join t4 in _context.Banks on t2.BankId equals t4.BankId into t4_Join
                                     from t4 in t4_Join.DefaultIfEmpty()
                                     select new
                                     {
                                         ChequeBookId = t1.ChequeBookId,
                                         BookFirstPageNumber = t1.BookFirstPageNumber,
                                         BookLastPageNumber = t1.BookLastPageNumber,
                                         TotalBookPage = t1.TotalBookPage,
                                         UsedBookPage = t1.UsedBookPage,
                                     }).FirstOrDefaultAsync();
            return sendData;
        }

        public async Task<bool> SendRequest(ChequeRegisterModel model)
        {
            bool sendData = false;

            if (model != null)
            {
                var getCheque = _context.ChequeRegisters.FirstOrDefault(x => x.ChequeRegisterId == model.ChequeRegisterId);
                if (getCheque != null)
                {
                    getCheque.IsCancelRequest = true;
                    getCheque.CancelReason = model.CancelReason;
                    getCheque.RequestedBy = HttpContext.Current.User.Identity.Name;
                    getCheque.RequestedOn = DateTime.Now;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }

        public bool CheckIsSignOrNot(long chequeRegisterId)
        {
            bool sendData = false;

            if (chequeRegisterId > 0)
            {
                var getCheque = _context.ChequeRegisters.FirstOrDefault(x => x.ChequeRegisterId == chequeRegisterId);
                if (getCheque.IsSigned)
                {
                    sendData = true;
                }
            }

            return sendData;
        }

        public object ChequeCancelationInfo(long chequeRegisterId)
        {
            object sendData = new { IsCancelRequest = false };
            if (chequeRegisterId > 0)
            {
                var getCheque = _context.ChequeRegisters.FirstOrDefault(x => x.ChequeRegisterId == chequeRegisterId && x.IsActive);
                if (getCheque != null)
                {
                    var employeeName = "";
                    if (getCheque.IsCancelRequest == true)
                    {
                        employeeName = _context.Employees.FirstOrDefault(x => x.EmployeeId == getCheque.RequestedBy).Name;
                        string requestedOnString = getCheque.RequestedOn.HasValue ? getCheque.RequestedOn.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "";
                        sendData = new
                        {
                            IsSigned = getCheque.IsSigned,
                            HasPDF = getCheque.HasPDF,
                            IsCancelRequest = (bool)(getCheque.IsCancelRequest == null ? false : getCheque.IsCancelRequest),
                            IsCanceled = (bool)(getCheque.IsCanceled == null ? false : getCheque.IsCanceled),
                            PrintCount = getCheque.PrintCount ?? 0,
                            CompanyFK = 21,
                            CancelReason = getCheque.CancelReason ?? "N/A",
                            RequestedBy = getCheque.RequestedBy + " - " + employeeName,
                            RequestedOn = requestedOnString,
                        };
                    }
                }
            }

            return sendData;
        }
    }
}