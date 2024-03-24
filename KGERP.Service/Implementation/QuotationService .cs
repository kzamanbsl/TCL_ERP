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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

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
                                                             QuotationDate = t1.QutationDate,
                                                             QuotationName = t1.QuotationName,
                                                             SupplierId = t1.SupplierId,
                                                             SupplierName = "[" + t3.Code + "] " + t3.Name,
                                                             RequisitionId = t1.BillRequisitionMasterId,
                                                             RequisitionNo = t2.BillRequisitionNo,
                                                             QuotationFor = (int)(EnumQuotationFor)t1.QuotationForId,
                                                             Description = t1.Description ?? "N/A",
                                                             StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                             CreatedDate = t1.CreatedOn,
                                                             CreatedBy = t1.CreatedBy,
                                                             EmployeeName = t1.CreatedBy + " - " + t4.Name,
                                                             CompanyFK = 21
                                                         }).FirstOrDefault());

            quotationMasterModel.DetailList = await Task.Run(() => (from t1 in _context.QuotationDetails.Where(x => x.IsActive && x.QuotationMasterId == quotationMasterId)
                                                                    join t2 in _context.QuotationMasters on t1.QuotationMasterId equals t2.QuotationMasterId into t2_Join
                                                                    from t2 in t2_Join.DefaultIfEmpty()
                                                                    join t3 in _context.Products on t1.MaterialId equals t3.ProductId into t3_Join
                                                                    from t3 in t3_Join.DefaultIfEmpty()
                                                                    join t4 in _context.ProductSubCategories on t3.ProductSubCategoryId equals t4.ProductSubCategoryId into t4_Join
                                                                    from t4 in t4_Join.DefaultIfEmpty()
                                                                    join t5 in _context.ProductCategories on t4.ProductCategoryId equals t5.ProductCategoryId into t5_Join
                                                                    from t5 in t5_Join.DefaultIfEmpty()
                                                                    join t6 in _context.Units on t3.UnitId equals t6.UnitId into t6_Join
                                                                    from t6 in t6_Join.DefaultIfEmpty()

                                                                    select new QuotationDetailModel
                                                                    {
                                                                        QuotationDetailId = t1.QuotationDetailId,
                                                                        QuotationMasterId = t1.QuotationMasterId,
                                                                        MaterialId = t1.MaterialId,
                                                                        MaterialName = t3.ProductName,
                                                                        MaterialSubtypeId = t4.ProductSubCategoryId,
                                                                        MaterialSubtypeName = t4.Name,
                                                                        MaterialTypeId = t5.ProductCategoryId,
                                                                        MaterialTypeName = t5.Name,
                                                                        UnitId = t6.UnitId,
                                                                        UnitName = t6.Name,
                                                                        MaterialQualityId = (int)(EnumMaterialQuality)t1.MaterialQuality,
                                                                        Quantity = t1.Quantity,
                                                                        UnitPrice = t1.UnitPrice,
                                                                        TotalAmount = t1.TotalAmount,
                                                                        Remarks = t1.Remarks ?? "N/A",
                                                                        CompanyFK = 21
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
                int getLastRowId = _context.QuotationMasters.Where(c => c.QutationDate == model.QuotationDate).Count();

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

        public long QuotationDetailEdit(QuotationMasterModel model)
        {
            long result = -1;
            decimal totalPrice = model.DetailModel.Quantity * model.DetailModel.UnitPrice;
            QuotationDetail quotationDetail = _context.QuotationDetails.FirstOrDefault(x => x.QuotationDetailId == model.DetailModel.QuotationDetailId);
            quotationDetail.QuotationMasterId = quotationDetail.QuotationMasterId;
            quotationDetail.MaterialId = (int)model.DetailModel.MaterialId;
            quotationDetail.MaterialQuality = model.DetailModel.MaterialQualityId;
            quotationDetail.Quantity = model.DetailModel.Quantity;
            quotationDetail.UnitPrice = model.DetailModel.UnitPrice;
            quotationDetail.TotalAmount = totalPrice;
            quotationDetail.Remarks = model.DetailModel.Remarks;
            quotationDetail.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            quotationDetail.ModifiedOn = DateTime.Now;

            if (_context.SaveChanges() > 0)
            {
                result = quotationDetail.QuotationMasterId;
            }

            return result;
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

        public async Task<long> QuotationDetailDelete(long id)
        {
            long result = -1;

            QuotationDetail quotationDetail = _context.QuotationDetails.FirstOrDefault(x => x.QuotationDetailId == id);
            quotationDetail.IsActive = false;
            quotationDetail.ModifiedBy = HttpContext.Current.User.Identity.Name;
            quotationDetail.ModifiedOn = DateTime.Now;

            if (await _context.SaveChangesAsync() > 0)
            {
                result = quotationDetail.QuotationMasterId;
            }

            return result;
        }

        public async Task<QuotationDetailModel> QuotationDetailBbyId(long id)
        {
            var data = await Task.Run(() => (from t1 in _context.QuotationDetails.Where(x => x.IsActive && x.QuotationDetailId == id)
                                             join t2 in _context.QuotationMasters on t1.QuotationMasterId equals t2.QuotationMasterId into t2_Join
                                             from t2 in t2_Join.DefaultIfEmpty()
                                             join t3 in _context.Products on t1.MaterialId equals t3.ProductId into t3_Join
                                             from t3 in t3_Join.DefaultIfEmpty()
                                             join t4 in _context.ProductSubCategories on t3.ProductSubCategoryId equals t4.ProductSubCategoryId into t4_Join
                                             from t4 in t4_Join.DefaultIfEmpty()
                                             join t5 in _context.ProductCategories on t4.ProductCategoryId equals t5.ProductCategoryId into t5_Join
                                             from t5 in t5_Join.DefaultIfEmpty()
                                             join t6 in _context.Units on t3.UnitId equals t6.UnitId into t6_Join
                                             from t6 in t6_Join.DefaultIfEmpty()
                                             select new QuotationDetailModel
                                             {
                                                 QuotationDetailId = t1.QuotationDetailId,
                                                 QuotationMasterId = t1.QuotationMasterId,
                                                 MaterialId = t1.MaterialId,
                                                 MaterialName = t3.ProductName,
                                                 MaterialSubtypeId = t4.ProductSubCategoryId,
                                                 MaterialSubtypeName = t4.Name,
                                                 MaterialTypeId = t5.ProductCategoryId,
                                                 MaterialTypeName = t5.Name,
                                                 UnitId = t6.UnitId,
                                                 UnitName = t6.Name,
                                                 MaterialQualityId = (int)(EnumMaterialQuality)t1.MaterialQuality,
                                                 Quantity = t1.Quantity,
                                                 UnitPrice = t1.UnitPrice,
                                                 TotalAmount = t1.TotalAmount,
                                                 Remarks = t1.Remarks,
                                                 CompanyFK = 21,
                                             }).FirstOrDefaultAsync());
            return data;
        }

        public async Task<QuotationMasterModel> GetQuotationList()
        {
            QuotationMasterModel quotationMasterModel = new QuotationMasterModel();
            var totalPrice = (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
                              join t2 in _context.QuotationDetails on t1.QuotationMasterId equals t2.QuotationMasterId into t2_Join
                              from t2 in t2_Join.DefaultIfEmpty()
                              where t2.IsActive
                              select new
                              {
                                  QuotationMasterId = t1.QuotationMasterId,
                                  TotalAmount = t2.Quantity * t2.UnitPrice,
                              }).AsQueryable();

            quotationMasterModel.DataList = await Task.Run(() => (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
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
                                                                      QuotationDate = t1.QutationDate,
                                                                      QuotationName = t1.QuotationName,
                                                                      SupplierId = t1.SupplierId,
                                                                      SupplierName = "[" + t3.Code + "] " + t3.Name,
                                                                      RequisitionId = t1.BillRequisitionMasterId,
                                                                      RequisitionNo = t2.BillRequisitionNo,
                                                                      QuotationFor = (int)(EnumQuotationFor)t1.QuotationForId,
                                                                      Description = t1.Description ?? "N/A",
                                                                      StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                                      CreatedDate = t1.CreatedOn,
                                                                      CreatedBy = t1.CreatedBy,
                                                                      EmployeeName = t1.CreatedBy + " - " + t4.Name,
                                                                      TotalAmount = totalPrice
                                                                                    .Where(x => x.QuotationMasterId == t1.QuotationMasterId)
                                                                                    .Select(x => (decimal?)x.TotalAmount)
                                                                                    .Sum() ?? 0,
                                                                      CompanyFK = 21
                                                                  }).ToListAsync());

            return quotationMasterModel;
        }

        public async Task<bool> QuotationMasterDelete(long id)
        {
            bool result = false;

            QuotationMaster quotationMaster = _context.QuotationMasters.FirstOrDefault(x => x.QuotationMasterId == id);
            quotationMaster.IsActive = false;
            quotationMaster.ModifiedBy = HttpContext.Current.User.Identity.Name;
            quotationMaster.ModifiedOn = DateTime.Now;

            if (await _context.SaveChangesAsync() > 0)
            {
                result = true;
            }

            return result;
        }

        public async Task<QuotationMasterModel> GetQuotationListByDate(QuotationMasterModel model)
        {
            QuotationMasterModel quotationMasterModel = new QuotationMasterModel();
            var totalPrice = (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
                              join t2 in _context.QuotationDetails on t1.QuotationMasterId equals t2.QuotationMasterId into t2_Join
                              from t2 in t2_Join.DefaultIfEmpty()
                              where t2.IsActive
                              select new
                              {
                                  QuotationMasterId = t1.QuotationMasterId,
                                  TotalAmount = t2.Quantity * t2.UnitPrice,
                              }).AsQueryable();

            quotationMasterModel.DataList = await Task.Run(() => (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
                                                                  join t2 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                                  from t2 in t2_Join.DefaultIfEmpty()
                                                                  join t3 in _context.Vendors on t1.SupplierId equals t3.VendorId into t3_Join
                                                                  from t3 in t3_Join.DefaultIfEmpty()
                                                                  join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                                                  from t4 in t4_Join.DefaultIfEmpty()
                                                                  where t1.QutationDate >= model.QuotationFromDate && t1.QutationDate <= model.QuotationToDate
                                                                  select new QuotationMasterModel
                                                                  {
                                                                      QuotationMasterId = t1.QuotationMasterId,
                                                                      QuotationNo = t1.QuotationNo,
                                                                      QuotationDate = t1.QutationDate,
                                                                      QuotationName = t1.QuotationName,
                                                                      SupplierId = t1.SupplierId,
                                                                      SupplierName = "[" + t3.Code + "] " + t3.Name,
                                                                      RequisitionId = t1.BillRequisitionMasterId,
                                                                      RequisitionNo = t2.BillRequisitionNo,
                                                                      QuotationFor = (int)(EnumQuotationFor)t1.QuotationForId,
                                                                      Description = t1.Description ?? "N/A",
                                                                      StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                                      CreatedDate = t1.CreatedOn,
                                                                      CreatedBy = t1.CreatedBy,
                                                                      EmployeeName = t1.CreatedBy + " - " + t4.Name,
                                                                      TotalAmount = totalPrice
                                                                                    .Where(x => x.QuotationMasterId == t1.QuotationMasterId)
                                                                                    .Select(x => (decimal?)x.TotalAmount)
                                                                                    .Sum() ?? 0,
                                                                      CompanyFK = 21
                                                                  }).ToListAsync());
            quotationMasterModel.CompanyFK = 21;

            return quotationMasterModel;
        }

        public async Task<QuotationCompareModel> GetComparedQuotation(long quotationIdOne, long quotationIdTwo)
        {
            QuotationCompareModel quotationCompareModel = new QuotationCompareModel();

            QuotationMasterModel masterDataOne = await GetQuotationMaster(quotationIdOne);
            QuotationMasterModel masterDataTwo = await GetQuotationMaster(quotationIdTwo);

            quotationCompareModel.QuotationMasterModel.Add(masterDataOne);
            quotationCompareModel.QuotationMasterModel.Add(masterDataTwo);

            return quotationCompareModel;
        }

        private async Task<QuotationMasterModel> GetQuotationMaster(long quotationId)
        {
            var masterData = await (from t1 in _context.QuotationMasters
                                    where t1.IsActive && t1.QuotationMasterId == quotationId
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
                                        QuotationDate = t1.QutationDate,
                                        QuotationName = t1.QuotationName,
                                        SupplierId = t1.SupplierId,
                                        SupplierName = "[" + t3.Code + "] " + t3.Name,
                                        RequisitionId = t1.BillRequisitionMasterId,
                                        RequisitionNo = t2.BillRequisitionNo,
                                        QuotationFor = (int)(EnumQuotationFor)t1.QuotationForId,
                                        Description = t1.Description ?? "N/A",
                                        StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                        CreatedDate = t1.CreatedOn,
                                        CreatedBy = t1.CreatedBy,
                                        EmployeeName = t1.CreatedBy + " - " + t4.Name,
                                        CompanyFK = 21
                                    }).FirstOrDefaultAsync();

            if (masterData != null)
            {
                var detailData = await (from t1 in _context.QuotationDetails
                                        where t1.QuotationMasterId == quotationId
                                        join t2 in _context.Products on t1.MaterialId equals t2.ProductId into t2_Join
                                        from t2 in t2_Join.DefaultIfEmpty()
                                        join t3 in _context.ProductSubCategories on t2.ProductSubCategoryId equals t3.ProductSubCategoryId into t3_Join
                                        from t3 in t3_Join.DefaultIfEmpty()
                                        join t4 in _context.ProductCategories on t3.ProductCategoryId equals t4.ProductCategoryId into t4_Join
                                        from t4 in t4_Join.DefaultIfEmpty()
                                        join t5 in _context.Units on t2.UnitId equals t5.UnitId into t5_Join
                                        from t5 in t5_Join.DefaultIfEmpty()
                                        select new QuotationDetailModel
                                        {
                                            QuotationDetailId = t1.QuotationDetailId,
                                            QuotationMasterId = t1.QuotationMasterId,
                                            MaterialId = t1.MaterialId,
                                            MaterialName = t2.ProductName,
                                            MaterialSubtypeId = t3.ProductSubCategoryId,
                                            MaterialSubtypeName = t3.Name,
                                            MaterialTypeId = t4.ProductCategoryId,
                                            MaterialTypeName = t4.Name,
                                            UnitId = t5.UnitId,
                                            UnitName = t5.Name,
                                            MaterialQualityId = (int)(EnumMaterialQuality)t1.MaterialQuality,
                                            Quantity = t1.Quantity,
                                            UnitPrice = t1.UnitPrice,
                                            TotalAmount = t1.TotalAmount,
                                            Remarks = t1.Remarks,
                                            CompanyFK = 21
                                        }).ToListAsync();

                masterData.DetailDataList = detailData;
            }

            return masterData;
        }

        public async Task<List<QuotationMasterModel>> GetQuotationListWithNameAndNo()
        {
            List<QuotationMasterModel> senddata = await Task.Run(() => (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
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
                                                                            QuotationDate = t1.QutationDate,
                                                                            QuotationName = t1.QuotationName,
                                                                            QuotationNameWitNo = "[" + t1.QuotationNo + "] " + t1.QuotationName,
                                                                            SupplierId = t1.SupplierId,
                                                                            SupplierName = "[" + t3.Code + "] " + t3.Name,
                                                                            RequisitionId = t1.BillRequisitionMasterId,
                                                                            RequisitionNo = t2.BillRequisitionNo,
                                                                            QuotationFor = (int)(EnumQuotationFor)t1.QuotationForId,
                                                                            Description = t1.Description ?? "N/A",
                                                                            StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                                            CreatedDate = t1.CreatedOn,
                                                                            CreatedBy = t1.CreatedBy,
                                                                            EmployeeName = t1.CreatedBy + " - " + t4.Name,
                                                                            CompanyFK = 21
                                                                        }).ToListAsync());
            return senddata;
        }

        #region  Project Type

        public async Task<List<QuotationTypeModel>> GetQuotationTypeList(int companyId)
        {
            var projectTypes = await _context.Accounting_CostCenterType
                .Where(c => c.CompanyId == companyId && c.IsActive)
                .ToListAsync();

            var returnData = projectTypes.Select(projectType => new QuotationTypeModel
            {
                QuotationTypeId = projectType.CostCenterTypeId,
                Name = projectType.Name,
                CompanyFK = projectType.CompanyId,
                CreatedBy = projectType.CreatedBy,
                CreatedDate = projectType.CreatedDate,
                ModifiedBy = projectType.ModifiedBy,
            }).ToList();

            return returnData;
        }

        public async Task<bool> Add(QuotationTypeModel model)
        {
            if (model != null)
            {
                QuotationTypeModel data = new QuotationTypeModel()
                {
                    Name = model.Name,
                    CompanyFK = (int)model.CompanyFK,
                    IsActive = true,
                    CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                    CreatedDate = DateTime.Now,
                };

                //_context.Accounting_CostCenterType.Add(data);
                var count = await _context.SaveChangesAsync();

                return count > 0;
            }
            return false;
        }

        public async Task<bool> Edit(QuotationTypeModel model)
        {
            if (model != null)
            {
                var findCostCenterType = await _context.Accounting_CostCenterType.FirstOrDefaultAsync(c => c.CostCenterTypeId == model.ID);

                if (findCostCenterType != null)
                {
                    findCostCenterType.Name = model.Name;
                    findCostCenterType.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findCostCenterType.ModifiedDate = DateTime.Now.ToString();
                    var count = await _context.SaveChangesAsync();

                    model.CompanyFK = findCostCenterType.CompanyId;

                    return count > 0;
                }
            }
            return false;
        }

        public async Task<bool> Delete(QuotationTypeModel model)
        {
            if (model.QuotationTypeId > 0)
            {
                var findCostCenterType = await _context.Accounting_CostCenterType.FirstOrDefaultAsync(c => c.CostCenterTypeId == model.QuotationTypeId);

                if (findCostCenterType != null)
                {
                    findCostCenterType.IsActive = false;
                    findCostCenterType.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findCostCenterType.ModifiedDate = DateTime.Now.ToString();
                    var count = await _context.SaveChangesAsync();

                    return count > 0;
                }
            }
            return false;
        }

        #endregion
    }
}
