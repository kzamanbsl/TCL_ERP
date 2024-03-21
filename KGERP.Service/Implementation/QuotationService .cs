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

namespace KGERP.Service.Implementation
{
    public class QuotationService : IQuotationService
    {
        private readonly ERPEntities _context;
        private readonly ConfigurationService _configurationService;
        public QuotationService(ERPEntities context, ConfigurationService configurationService)
        {
            _context = context;
            _configurationService = configurationService;
        }

        public async Task<QuotationMasterModel> GetQuotationMasterDetail(int companyId, long quotationMasterId)
        {
            QuotationMasterModel quotationMasterModel = new QuotationMasterModel();


            quotationMasterModel = await Task.Run(() => (from t1 in _context.QuotationMasters.Where(x => x.IsActive && x.QuotationMasterId == quotationMasterId)
                                                         join t2 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                         from t2 in t2_Join.DefaultIfEmpty()
                                                         join t3 in _context.Vendors on t1.SupplierId equals t3.VendorId into t3_Join
                                                         from t3 in t3_Join.DefaultIfEmpty()
                                                         join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                                         from t4 in t4_Join.DefaultIfEmpty()
                                                         select new QuotationMasterModel
                                                         {
                                                             QuotationMasterId = t1.QuotationMasterId,
                                                             QuotationNo = t1.QuotationNo,
                                                             QuotationName = t1.QuotationName,
                                                             SupplierId = t1.SupplierId,
                                                             SupplierName = t3.ContactName,
                                                             RequisitionId = t1.BillRequisitionMasterId,
                                                             RequisitionNo = t2.BillRequisitionNo,
                                                             QuotationFor = (int)(EnumQuotationFor)t1.QuotationForId,
                                                             Description = t1.Description,
                                                             StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                             CreatedDate = t1.CreatedOn,
                                                             CreatedBy = t1.CreatedBy,
                                                             EmployeeName = t1.CreatedBy + " - " + t4.Name

                                                         }).FirstOrDefault());

            quotationMasterModel.DetailList = await Task.Run(() => (from t1 in _context.QuotationDetails.Where(x => x.IsActive && x.QuotationMasterId == quotationMasterId)
                                                                    join t2 in _context.QuotationMasters on t1.QuotationMasterId equals t2.QuotationMasterId into t2_Join
                                                                    from t2 in t2_Join.DefaultIfEmpty()
                                                                    join t3 in _context.Products on t1.MaterialId equals t3.ProductId into t3_Join
                                                                    from t3 in t3_Join.DefaultIfEmpty()
                                                                    join t4 in _context.Units on t3.UnitId equals t4.UnitId into t4_Join
                                                                    from t4 in t4_Join.DefaultIfEmpty()
                                                                    join t5 in _context.ProductSubCategories on t3.ProductSubCategoryId equals t5.ProductSubCategoryId into t5_Join
                                                                    from t5 in t5_Join.DefaultIfEmpty()
                                                                    select new QuotationDetailModel
                                                                    {
                                                                        QuotationDetailId = t1.QuotationDetailId,
                                                                        QuotationMasterId = t1.QuotationMasterId,
                                                                        MaterialId = t1.MaterialId,
                                                                        MaterialName = t3.ProductName,
                                                                        UnitId = t4.UnitId,
                                                                        UnitName = t4.Name,
                                                                        MaterialQualityId = (int)(EnumMaterialQuality)t1.MaterialQuality,
                                                                        Quantity = t1.Quantity,
                                                                        UnitPrice = t1.UnitPrice,
                                                                        TotalAmount = t1.Quantity * t1.UnitPrice,
                                                                        Remarks = t1.Remarks,
                                                                    }).OrderByDescending(x => x.QuotationDetailId).AsEnumerable());

            quotationMasterModel.TotalAmount = quotationMasterModel.DetailList.Select(x => x.TotalAmount).Sum();

            return quotationMasterModel;
        }

        public async Task<long> QuotationMasterAdd(QuotationMasterModel model)
        {
            long result = -1;

            if (model.StatusId == 0)
            {
                model.StatusId = (int)EnumQuotationStatus.Draft;
            }

            QuotationMaster quotationMaster = new QuotationMaster
            {
                QutationDate = model.QuotationDate,
                QuotationNo = GetUniqueQuotationNo(),
                QuotationName = model.QuotationName,
                SupplierId = model.SupplierId,
                QuotationForId = model.QuotationFor,
                BillRequisitionMasterId = model.RequisitionId,
                Description = model.Description,
                StatusId = (int)model.StatusId,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            _context.QuotationMasters.Add(quotationMaster);

            if (await _context.SaveChangesAsync() > 0)
            {
                result = quotationMaster.QuotationMasterId;
            }
            return result;

            #region Generate Unique Requisition Number With Last Id

            // Generate Unique Quotation Number
            string GetUniqueQuotationNo()
            {
                int getLastRowId = _context.BillRequisitionMasters.Where(c => c.BRDate == model.QuotationDate).Count();

                string setZeroBeforeLastId(int lastRowId, int length)
                {
                    string totalDigit = "";

                    for (int i = (length - lastRowId.ToString().Length); 0 < i; i--)
                    {
                        totalDigit += "0";
                    }
                    return totalDigit + lastRowId.ToString();
                }

                string generatedNumber = $"QUO-{model.QuotationDate:yyMMdd}-{setZeroBeforeLastId(++getLastRowId, 4)}";
                return generatedNumber;
            }

            #endregion
        }

        public async Task<long> QuotationDetailAdd(QuotationMasterModel model)
        {
            long result = -1;
            decimal totalPrice = (decimal)model.DetailModel.Quantity * (decimal)model.DetailModel.UnitPrice;

            QuotationDetail quotationDetail = new QuotationDetail
            {
                QuotationMasterId = model.QuotationMasterId,
                MaterialId = (int)model.DetailModel.MaterialId,
                MaterialQuality = model.DetailModel.MaterialQualityId,
                Quantity = model.DetailModel.Quantity,
                UnitPrice = model.DetailModel.UnitPrice,
                TotalAmount = totalPrice,
                Remarks = model.DetailModel.Remarks,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreatedOn = DateTime.Now,
                IsActive = true,
            };

            _context.QuotationDetails.Add(quotationDetail);

            if (await _context.SaveChangesAsync() > 0)
            {
                result = quotationDetail.QuotationMasterId;
            }
            return result;
        }

        public Task<long> QuotationDetailEdit(QuotationMasterModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<long> SubmitQuotationMaster(long quotationMasterId)
        {
            long result = -1;

            if (quotationMasterId == 0)
            {
                throw new Exception("Sorry! Quotation not found!");
            }
            else
            {
                QuotationMaster quotationMaster = _context.QuotationMasters.FirstOrDefault(x => x.QuotationMasterId == quotationMasterId);
                quotationMaster.StatusId = (int)EnumQuotationStatus.Submitted;
                quotationMaster.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                quotationMaster.ModifiedOn = DateTime.Now;

                if (await _context.SaveChangesAsync() > 0)
                {
                    result = quotationMasterId;
                }
            }
            return result;
        }
    }
}
