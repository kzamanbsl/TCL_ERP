using KGERP.Data.CustomModel;
using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web;

namespace KGERP.Service.Implementation
{
    public class ChequeRegisterService : IChequeRegisterService
    {
        private readonly ERPEntities _context;
        public ChequeRegisterService(ERPEntities context)
        {
            _context = context;
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
                                                            RequisitionId = (int)t1.RequisitionMasterId,
                                                            RequisitionNo = t2.BillRequisitionNo ?? "N/A",
                                                            ProjectId = t1.ProjectId,
                                                            ProjectName = t4.Name,
                                                            SupplierId = (int)t1.SupplierId,
                                                            SupplierName = t3.Name,
                                                            SupplierCode = t3.Code,
                                                            PayTo = t1.PayTo,
                                                            IssueDate = t1.IssueDate,
                                                            ChequeDate = t1.ChequeDate,
                                                            ChequeNo = t1.ChequeNo,
                                                            Amount = t1.Amount,
                                                            ClearingDate = t1.ClearingDate,
                                                            Remarks = t1.Remarks,
                                                            IsSigned = t1.IsSigned,
                                                            CreatedBy = t1.CreatedBy,
                                                            CreatedDate = t1.CreatedOn,
                                                            ModifiedBy = t1.ModifiedBy,
                                                            ModifiedDate = t1.ModifiedOn,
                                                            IsActive = t1.IsActive,
                                                            CompanyFK = 21,
                                                        }).ToListAsync();
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
                                                      RequisitionId = (int)t1.RequisitionMasterId,
                                                      RequisitionNo = t2.BillRequisitionNo ?? "N/A",
                                                      ProjectId = t1.ProjectId,
                                                      ProjectName = t4.Name,
                                                      SupplierId = (int)t1.SupplierId,
                                                      SupplierName = t3.Name,
                                                      SupplierCode = t3.Code,
                                                      PayTo = t1.PayTo,
                                                      IssueDate = t1.IssueDate,
                                                      ChequeDate = t1.ChequeDate,
                                                      ChequeNo = t1.ChequeNo,
                                                      Amount = t1.Amount,
                                                      ClearingDate = t1.ClearingDate,
                                                      Remarks = t1.Remarks,
                                                      IsSigned = t1.IsSigned,
                                                      CreatedBy = t1.CreatedBy,
                                                      CreatedDate = t1.CreatedOn,
                                                      ModifiedBy = t1.ModifiedBy,
                                                      ModifiedDate = t1.ModifiedOn,
                                                      IsActive = t1.IsActive,
                                                      CompanyFK = 21,
                                                  }).FirstOrDefaultAsync();
            return sendData;
        }


        public async Task<bool> Add(ChequeRegisterModel model)
        {
            bool sendData = false;

            if(model != null)
            {
                ChequeRegister data = new ChequeRegister();
                data.RequisitionMasterId = model.RequisitionId;
                data.ProjectId = model.ProjectId;
                data.SupplierId = model.SupplierId;
                data.PayTo = model.PayTo;
                data.IssueDate = model.IssueDate;
                data.ChequeDate = model.ChequeDate;
                data.ChequeNo = model.ChequeNo;
                data.Amount = model.Amount;
                data.ClearingDate = model.ClearingDate;
                data.Remarks = model.Remarks;
                data.IsSigned = false;
                data.IsActive = true;
                data.CreatedBy = HttpContext.Current.User.Identity.Name;
                data.CreatedOn = DateTime.Now;

                _context.ChequeRegisters.Add(data);
                if (await _context.SaveChangesAsync() > 0)
                {
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
                var result = await _context.ChequeRegisters.FirstOrDefaultAsync(x => x.ChequeRegisterId == model.ID);
                if (result != null)
                {
                    result.RequisitionMasterId = model.RequisitionId;
                    result.ProjectId = model.ProjectId;
                    result.SupplierId = model.SupplierId;
                    result.PayTo = model.PayTo;
                    result.IssueDate = model.IssueDate;
                    result.ChequeDate = model.ChequeDate;
                    result.ChequeNo = model.ChequeNo;
                    result.Amount = model.Amount;
                    result.ClearingDate = model.ClearingDate;
                    result.IsSigned = result.IsSigned;
                    result.Remarks = model.Remarks;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name.ToString();
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
                var result = await _context.ChequeRegisters.FirstOrDefaultAsync(x => x.ChequeRegisterId == model.ChequeRegisterId);
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

        public async Task<bool> ChequeSign(long chequeRegisterId)
        {
            bool sendData = false;
            if (chequeRegisterId > 0)
            {
                var result = await _context.ChequeRegisters.FirstOrDefaultAsync(x => x.ChequeRegisterId == chequeRegisterId);
                if (result != null)
                {
                    result.IsSigned = true;
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
    }
}
