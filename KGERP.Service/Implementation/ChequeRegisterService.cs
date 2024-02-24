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
        private readonly ERPEntities _Context;
        public ChequeRegisterService(ERPEntities context)
        {
            _Context = context;
        }

        public async Task<List<ChequeRegisterModel>> GetChequeRegisterList(int companyId)
        {
            List<ChequeRegisterModel> sendData = await (from t1 in _Context.ChequeRegisters
                                                        join t2 in _Context.BillRequisitionMasters on t1.RequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                        join t3 in _Context.Vendors on t1.SupplierId equals t3.VendorId into t3_Join
                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                        join t4 in _Context.Accounting_CostCenter on t1.ProjectId equals t4.CostCenterId into t4_Join
                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                        where t1.IsActive
                                                        select new ChequeRegisterModel
                                                        {
                                                            ChequeRegisterId = t1.ChequeRegisterId,
                                                            RegisterFor = t1.RequisitionMasterId == null ? (int)EnumChequeRegisterFor.General: (int)EnumChequeRegisterFor.Requisition,
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
                                                            IsActive = t1.IsActive
                                                        }).ToListAsync();
            return sendData;
        }

        public Task<ChequeRegisterModel> GetChequeRegisterById(long chequeRegisterId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(ChequeRegisterModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(ChequeRegisterModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(long chequeRegisterId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChequeSign(long chequeRegisterId)
        {
            bool sendData = false;
            if(chequeRegisterId > 0)
            {
                var result = await _Context.ChequeRegisters.FirstOrDefaultAsync(x => x.ChequeRegisterId == chequeRegisterId);
                if( result != null)
                {
                    result.IsSigned = true;
                    result.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    result.ModifiedOn = DateTime.Now;

                    if (await _Context.SaveChangesAsync() > 0)
                    {
                        sendData = true;
                    }
                }
            }

            return sendData;
        }
    }
}
