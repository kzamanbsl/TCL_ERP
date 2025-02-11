﻿using KGERP.Data.CustomModel;
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

            quotationMasterModel = await Task.Run(() => (from t1 in _context.QuotationMasters
                                                         where t1.IsActive && t1.QuotationMasterId == quotationMasterId
                                                         join t2 in _context.QuotationFors on t1.QuotationForId equals t2.QuotationForId into t2_Join
                                                         from t2 in t2_Join.DefaultIfEmpty()
                                                         join t3 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t3.BillRequisitionMasterId into t3_Join
                                                         from t3 in t3_Join.DefaultIfEmpty()
                                                         join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                                         from t4 in t4_Join.DefaultIfEmpty()
                                                         select new QuotationMasterModel
                                                         {
                                                             QuotationMasterId = t1.QuotationMasterId,
                                                             QuotationNo = t1.QuotationNo,
                                                             QuotationDate = t1.QuotationDate,
                                                             QuotationTypeId = (int)(EnumQuotationType)t1.QuotationTypeId,
                                                             QuotationForId = t1.QuotationForId,
                                                             QuotationForName = t2.Name,
                                                             RequisitionId = t1.BillRequisitionMasterId != null ? (long)t1.BillRequisitionMasterId : 0,
                                                             RequisitionNo = t3 != null ? t3.BillRequisitionNo ?? "N/A" : "N/A",
                                                             StartDate = t1.StartDate.HasValue ? t1.StartDate.Value : DateTime.MinValue,
                                                             EndDate = t1.EndDate.HasValue ? t1.EndDate.Value : DateTime.MinValue,
                                                             Description = t1.Description ?? "N/A",
                                                             StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                             CreatedDate = t1.CreatedOn,
                                                             CreatedBy = t1.CreatedBy,
                                                             EmployeeName = t1.CreatedBy + " - " + (t4 != null ? t4.Name : "N/A"),
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
                                                                        Remarks = t1.Remarks ?? "N/A",
                                                                        CompanyFK = 21
                                                                    }).OrderByDescending(x => x.QuotationDetailId).AsEnumerable());

            return quotationMasterModel;
        }

        public async Task<long> QuotationMasterAdd(QuotationMasterModel model)
        {
            long result = -1;

            if (model.StatusId == 0)
            {
                model.StatusId = (int)EnumQuotationStatus.Draft;
            }

            if (model.QuotationTypeId == (int)EnumQuotationType.General)
            {
                QuotationMaster quotationMaster = new QuotationMaster
                {
                    QuotationDate = model.QuotationDate,
                    QuotationNo = GetUniqueQuotationNo(),
                    QuotationTypeId = model.QuotationTypeId,
                    QuotationForId = model.QuotationForId,
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
            }
            else
            {
                QuotationMaster quotationMaster = new QuotationMaster
                {
                    QuotationDate = model.QuotationDate,
                    QuotationNo = GetUniqueQuotationNo(),
                    QuotationTypeId = model.QuotationTypeId,
                    QuotationForId = model.QuotationForId,
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
            }

            return result;

            #region Generate Unique Quotation Number With Date and Last Id

            // Generate Unique Quotation Number
            string GetUniqueQuotationNo()
            {
                int getLastRowId = _context.QuotationMasters.Where(c => c.StartDate == model.QuotationDate).Count();

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

            QuotationDetail quotationDetail = new QuotationDetail
            {
                QuotationMasterId = model.QuotationMasterId,
                MaterialId = (int)model.DetailModel.MaterialId,
                MaterialQuality = model.DetailModel.MaterialQualityId,
                Quantity = model.DetailModel.Quantity,
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
            QuotationDetail quotationDetail = _context.QuotationDetails.FirstOrDefault(x => x.QuotationDetailId == model.DetailModel.QuotationDetailId);
            quotationDetail.QuotationMasterId = quotationDetail.QuotationMasterId;
            quotationDetail.MaterialId = (int)model.DetailModel.MaterialId;
            quotationDetail.MaterialQuality = model.DetailModel.MaterialQualityId;
            quotationDetail.Quantity = model.DetailModel.Quantity;
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
                                                 Remarks = t1.Remarks,
                                                 CompanyFK = 21,
                                             }).FirstOrDefaultAsync());
            return data;
        }

        public async Task<QuotationMasterModel> GetQuotationList()
        {
            QuotationMasterModel quotationMasterModel = new QuotationMasterModel();

            quotationMasterModel.DataList = await Task.Run(() => (from t1 in _context.QuotationMasters
                                                                  join t2 in _context.QuotationFors on t1.QuotationForId equals t2.QuotationForId into t2_Join
                                                                  from t2 in t2_Join.DefaultIfEmpty()
                                                                  join t3 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t3.BillRequisitionMasterId into t3_Join
                                                                  from t3 in t3_Join.DefaultIfEmpty()
                                                                  join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                                                  from t4 in t4_Join.DefaultIfEmpty()
                                                                  where t1.IsActive
                                                                  select new QuotationMasterModel
                                                                  {
                                                                      QuotationMasterId = t1.QuotationMasterId,
                                                                      QuotationNo = t1.QuotationNo,
                                                                      QuotationTypeId = (int)(EnumQuotationType)t1.QuotationTypeId,
                                                                      QuotationForId = t1.QuotationForId,
                                                                      QuotationForName = t2.Name,
                                                                      RequisitionId = t1.BillRequisitionMasterId != null ? (long)t1.BillRequisitionMasterId : 0,
                                                                      RequisitionNo = t3 != null ? t3.BillRequisitionNo ?? "N/A" : "N/A",
                                                                      StartDate = t1.StartDate.HasValue ? t1.StartDate.Value : DateTime.MinValue,
                                                                      EndDate = t1.EndDate.HasValue ? t1.EndDate.Value : DateTime.MinValue,
                                                                      Description = t1.Description ?? "N/A",
                                                                      StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                                      CreatedDate = t1.CreatedOn,
                                                                      CreatedBy = t1.CreatedBy,
                                                                      EmployeeName = t1.CreatedBy + " - " + (t4 != null ? t4.Name : "N/A"),
                                                                      CompanyFK = 21
                                                                  }).ToListAsync());

            return quotationMasterModel;
        }

        public async Task<QuotationMasterModel> GetQuotationListFilterByStatus()
        {
            QuotationMasterModel quotationMasterModel = new QuotationMasterModel();

            quotationMasterModel.DataList = await Task.Run(() => (from t1 in _context.QuotationMasters
                                                                  join t2 in _context.QuotationFors on t1.QuotationForId equals t2.QuotationForId into t2_Join
                                                                  from t2 in t2_Join.DefaultIfEmpty()
                                                                  join t3 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t3.BillRequisitionMasterId into t3_Join
                                                                  from t3 in t3_Join.DefaultIfEmpty()
                                                                  join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                                                  from t4 in t4_Join.DefaultIfEmpty()
                                                                  where t1.IsActive && t1.StatusId == (int)EnumQuotationStatus.Opened && t1.StatusId == (int)EnumQuotationStatus.Closed
                                                                  select new QuotationMasterModel
                                                                  {
                                                                      QuotationMasterId = t1.QuotationMasterId,
                                                                      QuotationNo = t1.QuotationNo,
                                                                      QuotationTypeId = (int)(EnumQuotationType)t1.QuotationTypeId,
                                                                      QuotationForId = t1.QuotationForId,
                                                                      QuotationForName = t2.Name,
                                                                      RequisitionId = t1.BillRequisitionMasterId != null ? (long)t1.BillRequisitionMasterId : 0,
                                                                      RequisitionNo = t3 != null ? t3.BillRequisitionNo ?? "N/A" : "N/A",
                                                                      StartDate = t1.StartDate.HasValue ? t1.StartDate.Value : DateTime.MinValue,
                                                                      EndDate = t1.EndDate.HasValue ? t1.EndDate.Value : DateTime.MinValue,
                                                                      Description = t1.Description ?? "N/A",
                                                                      StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                                      CreatedDate = t1.CreatedOn,
                                                                      CreatedBy = t1.CreatedBy,
                                                                      EmployeeName = t1.CreatedBy + " - " + (t4 != null ? t4.Name : "N/A"),
                                                                      CompanyFK = 21
                                                                  }).ToListAsync());

            return quotationMasterModel;
        }

        public async Task<QuotationMasterModel> GetQuotationListByStatusId(int id)
        {
            QuotationMasterModel quotationMasterModel = new QuotationMasterModel();

            quotationMasterModel.DataList = await Task.Run(() => (from t1 in _context.QuotationMasters
                                                                  join t2 in _context.QuotationFors on t1.QuotationForId equals t2.QuotationForId into t2_Join
                                                                  from t2 in t2_Join.DefaultIfEmpty()
                                                                  join t3 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t3.BillRequisitionMasterId into t3_Join
                                                                  from t3 in t3_Join.DefaultIfEmpty()
                                                                  join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                                                  from t4 in t4_Join.DefaultIfEmpty()
                                                                  where t1.IsActive && t1.StatusId == id
                                                                  select new QuotationMasterModel
                                                                  {
                                                                      QuotationMasterId = t1.QuotationMasterId,
                                                                      QuotationNo = t1.QuotationNo,
                                                                      QuotationTypeId = (int)(EnumQuotationType)t1.QuotationTypeId,
                                                                      QuotationForId = t1.QuotationForId,
                                                                      QuotationForName = t2.Name,
                                                                      RequisitionId = t1.BillRequisitionMasterId != null ? (long)t1.BillRequisitionMasterId : 0,
                                                                      RequisitionNo = t3 != null ? t3.BillRequisitionNo ?? "N/A" : "N/A",
                                                                      StartDate = t1.StartDate.HasValue ? t1.StartDate.Value : DateTime.MinValue,
                                                                      EndDate = t1.EndDate.HasValue ? t1.EndDate.Value : DateTime.MinValue,
                                                                      Description = t1.Description ?? "N/A",
                                                                      StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                                      CreatedDate = t1.CreatedOn,
                                                                      CreatedBy = t1.CreatedBy,
                                                                      EmployeeName = t1.CreatedBy + " - " + (t4 != null ? t4.Name : "N/A"),
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

        public async Task<bool> OpenQuotationById(long id)
        {
            bool result = false;

            QuotationMaster quotationMaster = _context.QuotationMasters.FirstOrDefault(x => x.QuotationMasterId == id);
            quotationMaster.StatusId = (int)EnumQuotationStatus.Opened;
            quotationMaster.StartDate = DateTime.Now.Date;
            quotationMaster.ModifiedBy = HttpContext.Current.User.Identity.Name;
            quotationMaster.ModifiedOn = DateTime.Now;

            if (await _context.SaveChangesAsync() > 0)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> CloseQuotationById(long id)
        {
            bool result = false;

            QuotationMaster quotationMaster = _context.QuotationMasters.FirstOrDefault(x => x.QuotationMasterId == id);
            quotationMaster.StatusId = (int)EnumQuotationStatus.Closed;
            quotationMaster.EndDate = DateTime.Now.Date;
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

            quotationMasterModel.DataList = await Task.Run(() => (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
                                                                  join t2 in _context.QuotationFors on t1.QuotationForId equals t2.QuotationForId into t2_Join
                                                                  from t2 in t2_Join.DefaultIfEmpty()
                                                                  join t3 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t3.BillRequisitionMasterId into t3_Join
                                                                  from t3 in t3_Join.DefaultIfEmpty()
                                                                  join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                                                  from t4 in t4_Join.DefaultIfEmpty()
                                                                  where t1.QuotationDate >= model.QuotationFromDate && t1.QuotationDate <= model.QuotationToDate && t1.QuotationTypeId == model.QuotationTypeId
                                                                  select new QuotationMasterModel
                                                                  {
                                                                      QuotationMasterId = t1.QuotationMasterId,
                                                                      QuotationNo = t1.QuotationNo,
                                                                      QuotationTypeId = (int)(EnumQuotationType)t1.QuotationTypeId,
                                                                      QuotationForId = t1.QuotationForId,
                                                                      QuotationForName = t2.Name,
                                                                      RequisitionId = t1.BillRequisitionMasterId != null ? (long)t1.BillRequisitionMasterId : 0,
                                                                      RequisitionNo = t3 != null ? t3.BillRequisitionNo ?? "N/A" : "N/A",
                                                                      StartDate = t1.StartDate.HasValue ? t1.StartDate.Value : DateTime.MinValue,
                                                                      EndDate = t1.EndDate.HasValue ? t1.EndDate.Value : DateTime.MinValue,
                                                                      Description = t1.Description ?? "N/A",
                                                                      StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                                      CreatedDate = t1.CreatedOn,
                                                                      CreatedBy = t1.CreatedBy,
                                                                      EmployeeName = t1.CreatedBy + " - " + (t4 != null ? t4.Name : "N/A"),
                                                                      CompanyFK = 21
                                                                  }).ToListAsync());
            quotationMasterModel.CompanyFK = 21;

            return quotationMasterModel;
        }

        public async Task<QuotationMasterModel> GetQuotationListByDateAndStatus(QuotationMasterModel model)
        {
            QuotationMasterModel quotationMasterModel = new QuotationMasterModel();

            quotationMasterModel.DataList = await Task.Run(() => (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
                                                                  join t2 in _context.QuotationFors on t1.QuotationForId equals t2.QuotationForId into t2_Join
                                                                  from t2 in t2_Join.DefaultIfEmpty()
                                                                  join t3 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t3.BillRequisitionMasterId into t3_Join
                                                                  from t3 in t3_Join.DefaultIfEmpty()
                                                                  join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                                                  from t4 in t4_Join.DefaultIfEmpty()
                                                                  where t1.QuotationDate >= model.QuotationFromDate && t1.QuotationDate <= model.QuotationToDate && t1.StatusId == model.StatusId
                                                                  select new QuotationMasterModel
                                                                  {
                                                                      QuotationMasterId = t1.QuotationMasterId,
                                                                      QuotationNo = t1.QuotationNo,
                                                                      QuotationTypeId = (int)(EnumQuotationType)t1.QuotationTypeId,
                                                                      QuotationForId = t1.QuotationForId,
                                                                      QuotationForName = t2.Name,
                                                                      RequisitionId = t1.BillRequisitionMasterId != null ? (long)t1.BillRequisitionMasterId : 0,
                                                                      RequisitionNo = t3 != null ? t3.BillRequisitionNo ?? "N/A" : "N/A",
                                                                      StartDate = t1.StartDate.HasValue ? t1.StartDate.Value : DateTime.MinValue,
                                                                      EndDate = t1.EndDate.HasValue ? t1.EndDate.Value : DateTime.MinValue,
                                                                      Description = t1.Description ?? "N/A",
                                                                      StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                                      CreatedDate = t1.CreatedOn,
                                                                      CreatedBy = t1.CreatedBy,
                                                                      EmployeeName = t1.CreatedBy + " - " + (t4 != null ? t4.Name : "N/A"),
                                                                      CompanyFK = 21
                                                                  }).ToListAsync());
            quotationMasterModel.CompanyFK = 21;

            return quotationMasterModel;
        }

        public List<QuotationMasterModel> QuotationListByTypeIdAndForId(int typeId, long forId)
        {
            var QuotationList = (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
                                 join t2 in _context.QuotationFors on t1.QuotationForId equals t2.QuotationForId into t2_Join
                                 from t2 in t2_Join.DefaultIfEmpty()
                                 join t3 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t3.BillRequisitionMasterId into t3_Join
                                 from t3 in t3_Join.DefaultIfEmpty()
                                 join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                 from t4 in t4_Join.DefaultIfEmpty()
                                 where t1.QuotationTypeId == typeId && t1.QuotationForId == forId && t1.StatusId == (int)EnumQuotationStatus.Opened
                                 select new QuotationMasterModel
                                 {
                                     QuotationMasterId = t1.QuotationMasterId,
                                     QuotationNo = t1.QuotationNo,
                                     QuotationTypeId = (int)(EnumQuotationType)t1.QuotationTypeId,
                                     QuotationForId = t1.QuotationForId,
                                     QuotationForName = t2.Name,
                                     RequisitionId = t1.BillRequisitionMasterId != null ? (long)t1.BillRequisitionMasterId : 0,
                                     RequisitionNo = t3 != null ? t3.BillRequisitionNo ?? "N/A" : "N/A",
                                     StartDate = t1.StartDate ?? DateTime.MinValue,
                                     EndDate = t1.EndDate ?? DateTime.MinValue,
                                     Description = t1.Description ?? "N/A",
                                     StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                     CreatedDate = t1.CreatedOn,
                                     CreatedBy = t1.CreatedBy,
                                     EmployeeName = t1.CreatedBy + " - " + (t4 != null ? t4.Name : "N/A"),
                                     CompanyFK = 21
                                 }).ToList();

            return QuotationList;
        }

        public async Task<ComparativeStatementModel> GetComparativeStatementByQuotationId(long quotationMasterId)
        {
            ComparativeStatementModel viewData = new ComparativeStatementModel();

            viewData.QuotationSubmitDetailModelList = await (from t1 in _context.QuotationSubmitMasters
                                                             join t2 in _context.QuotationSubmitDetails on t1.QuotationSubmitMasterId equals t2.QuotationSubmitMasterId
                                                             join t3 in _context.Products on t2.MaterrialId equals t3.ProductId
                                                             join t4 in _context.Vendors on t1.SupplierId equals t4.VendorId
                                                             where t1.QuotationMasterId == quotationMasterId && t1.IsActive
                                                             select new QuotationSubmitDetailModel
                                                             {
                                                                 QuotationSubmitMasterId = t1.QuotationSubmitMasterId == null ? 0 : t1.QuotationSubmitMasterId,
                                                                 QuotationMasterId = t1.QuotationMasterId == null ? 0 : t1.QuotationMasterId,
                                                                 SubmissionDate = t1.SubmissionDate,
                                                                 QuotationSubmitDetailId = t2.QuotationSubmitDetailId == null ? 0 : t2.QuotationSubmitDetailId,
                                                                 MaterialId = t3.ProductId == null ? 0 : t3.ProductId,
                                                                 MaterialName = t3.ProductName ?? "N/A",
                                                                 SupplierId = t4.VendorId == null ? 0 : t4.VendorId,
                                                                 SupplierName = t4.Name ?? "N/A",
                                                                 Quantity = t2.Quantity == null ? 0 : t2.Quantity,
                                                                 UnitPrice = t2.UnitPrice == null ? 0 : t2.UnitPrice,
                                                                 TotalAmount = t2.TotalAmount == null ? 0 : t2.TotalAmount,
                                                             }).ToListAsync();
            viewData.CompanyFK = 21;

            return viewData;
        }

        private async Task<QuotationMasterModel> GetQuotationMaster(long quotationId)
        {
            var masterData = await (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
                                    join t2 in _context.QuotationFors on t1.QuotationForId equals t2.QuotationForId into t2_Join
                                    from t2 in t2_Join.DefaultIfEmpty()
                                    join t3 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t3.BillRequisitionMasterId into t3_Join
                                    from t3 in t3_Join.DefaultIfEmpty()
                                    join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                    from t4 in t4_Join.DefaultIfEmpty()
                                    select new QuotationMasterModel
                                    {
                                        QuotationMasterId = t1.QuotationMasterId,
                                        QuotationNo = t1.QuotationNo,
                                        QuotationTypeId = (int)(EnumQuotationType)t1.QuotationTypeId,
                                        QuotationForId = t1.QuotationForId,
                                        QuotationForName = t2.Name,
                                        RequisitionId = t1.BillRequisitionMasterId != null ? (long)t1.BillRequisitionMasterId : 0,
                                        RequisitionNo = t3 != null ? t3.BillRequisitionNo ?? "N/A" : "N/A",
                                        StartDate = t1.StartDate.HasValue ? t1.StartDate.Value : DateTime.MinValue,
                                        EndDate = t1.EndDate.HasValue ? t1.EndDate.Value : DateTime.MinValue,
                                        Description = t1.Description ?? "N/A",
                                        StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                        CreatedDate = t1.CreatedOn,
                                        CreatedBy = t1.CreatedBy,
                                        EmployeeName = t1.CreatedBy + " - " + (t4 != null ? t4.Name : "N/A"),
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
                                            Remarks = t1.Remarks,
                                            CompanyFK = 21
                                        }).ToListAsync();

                masterData.DetailDataList = detailData;
            }

            return masterData;
        }

        public async Task<List<QuotationMasterModel>> GetQuotationListWithNameAndNo()
        {
            List<QuotationMasterModel> sendData = await Task.Run(() => (from t1 in _context.QuotationMasters.Where(x => x.IsActive)
                                                                        join t2 in _context.QuotationFors on t1.QuotationForId equals t2.QuotationForId into t2_Join
                                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                                        join t3 in _context.BillRequisitionMasters on t1.BillRequisitionMasterId equals t3.BillRequisitionMasterId into t3_Join
                                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                                        join t4 in _context.Employees on t1.CreatedBy equals t4.EmployeeId into t4_Join
                                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                                        select new QuotationMasterModel
                                                                        {
                                                                            QuotationMasterId = t1.QuotationMasterId,
                                                                            QuotationDate = t1.QuotationDate,
                                                                            QuotationNo = t1.QuotationNo,
                                                                            QuotationTypeId = (int)(EnumQuotationType)t1.QuotationTypeId,
                                                                            QuotationForId = t1.QuotationForId,
                                                                            QuotationForName = t2.Name,
                                                                            RequisitionId = t1.BillRequisitionMasterId != null ? (long)t1.BillRequisitionMasterId : 0,
                                                                            RequisitionNo = t3 != null ? t3.BillRequisitionNo ?? "N/A" : "N/A",
                                                                            StartDate = t1.StartDate.HasValue ? t1.StartDate.Value : DateTime.MinValue,
                                                                            EndDate = t1.EndDate.HasValue ? t1.EndDate.Value : DateTime.MinValue,
                                                                            Description = t1.Description ?? "N/A",
                                                                            StatusId = (int)(EnumQuotationStatus)t1.StatusId,
                                                                            CreatedDate = t1.CreatedOn,
                                                                            CreatedBy = t1.CreatedBy,
                                                                            EmployeeName = t1.CreatedBy + " - " + (t4 != null ? t4.Name : "N/A"),
                                                                            CompanyFK = 21
                                                                        }).ToListAsync());
            return sendData;
        }

        #region  Project Type

        public async Task<List<QuotationForModel>> GetQuotationForList(int companyId)
        {
            List<QuotationForModel> sendData = await Task.Run(() => (from t1 in _context.QuotationFors.Where(x => x.IsActive)
                                                                     select new QuotationForModel
                                                                     {
                                                                         QuotationForId = t1.QuotationForId,
                                                                         Name = t1.Name,
                                                                         Description = t1.Description ?? "N/A",
                                                                         CreatedDate = t1.CreatedOn,
                                                                         CreatedBy = t1.CreatedBy,
                                                                         CompanyFK = 21
                                                                     }).ToListAsync());
            return sendData;
        }

        public async Task<bool> Add(QuotationForModel model)
        {
            bool result = false;
            QuotationFor data = new QuotationFor()
            {
                Name = model.Name,
                Description = model.Description,
                IsActive = true,
                CreatedBy = HttpContext.Current.User.Identity.Name,
                CreatedOn = DateTime.Now,
            };

            _context.QuotationFors.Add(data);

            if (await _context.SaveChangesAsync() > 0)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> Edit(QuotationForModel model)
        {
            bool result = false;

            var findCostCenterType = _context.QuotationFors.FirstOrDefault(c => c.QuotationForId == model.ID);
            if (findCostCenterType != null)
            {
                findCostCenterType.Name = model.Name;
                findCostCenterType.Description = model.Description;
                findCostCenterType.ModifiedBy = HttpContext.Current.User.Identity.Name;
                findCostCenterType.ModifiedOn = DateTime.Now;

                if (await _context.SaveChangesAsync() > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        public async Task<bool> Delete(QuotationForModel model)
        {
            bool result = false;

            var findCostCenterType = _context.QuotationFors.FirstOrDefault(c => c.QuotationForId == model.QuotationForId);
            if (findCostCenterType != null)
            {
                findCostCenterType.IsActive = false;
                findCostCenterType.ModifiedBy = HttpContext.Current.User.Identity.Name;
                findCostCenterType.ModifiedOn = DateTime.Now;
                var count = await _context.SaveChangesAsync();

                if (await _context.SaveChangesAsync() > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        public long AddQuotationSubmitMaster(QuotationSubmitMasterModel model)
        {
            long result = -1;

            QuotationSubmitMaster saveData = new QuotationSubmitMaster()
            {
                SubmissionDate = model.SubmissionDate,
                QuotationMasterId = model.QuotationMasterId,
                SupplierId = model.SupplierId,
                StatusId = (int)EnumQuotationSubmitStatus.Pending,
                CreatedBy = HttpContext.Current.User.Identity.Name,
                CreatedOn = DateTime.Now,
                IsActive = true
            };
            _context.QuotationSubmitMasters.Add(saveData);

            if (_context.SaveChanges() > 0)
            {
                result = saveData.QuotationSubmitMasterId;
            }
            return result;
        }

        public long AddQuotationSubmitDetail(QuotationSubmitMasterModel model)
        {
            long result = -1;
            var listOfData = new List<QuotationSubmitDetail>();

            foreach (var index in model.QuotationDetailModelList)
            {
                QuotationSubmitDetail saveData = new QuotationSubmitDetail()
                {
                    QuotationSubmitMasterId = model.QuotationSubmitMasterId,
                    MaterrialId = (int)index.MaterialId,
                    Quantity = index.Quantity,
                    UnitPrice = index.UnitPrice,
                    TotalAmount = index.Quantity * index.UnitPrice,
                    CreatedBy = HttpContext.Current.User.Identity.Name,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                listOfData.Add(saveData);
            }

            _context.QuotationSubmitDetails.AddRange(listOfData);

            if (_context.SaveChanges() > 0)
            {
                var quotationSubmitMaster = _context.QuotationSubmitMasters.FirstOrDefault(x => x.QuotationSubmitMasterId == model.QuotationSubmitMasterId);

                if (quotationSubmitMaster != null)
                {
                    quotationSubmitMaster.StatusId = (int)EnumQuotationSubmitStatus.Submitted;
                    if (_context.SaveChanges() > 0)
                    {
                        result = model.QuotationSubmitMasterId;
                    }
                }
            }
            return result;
        }

        public QuotationSubmitMasterModel GetQuotationByQuotationSubmitMasterId(long id)
        {
            QuotationSubmitMasterModel sendData = new QuotationSubmitMasterModel();
            sendData.QuotationSubmitMasterId = id;
            sendData.QuotationSubmitMaster = (from t1 in _context.QuotationSubmitMasters
                                              join t2 in _context.QuotationMasters on t1.QuotationMasterId equals t2.QuotationMasterId into t2_Join
                                              from t2 in t2_Join.DefaultIfEmpty()
                                              join t3 in _context.QuotationFors on t2.QuotationForId equals t3.QuotationForId into t3_Join
                                              from t3 in t3_Join.DefaultIfEmpty()
                                              join t4 in _context.Vendors on t1.SupplierId equals t4.VendorId into t4_Join
                                              from t4 in t4_Join.DefaultIfEmpty()
                                              where t1.IsActive && t1.QuotationSubmitMasterId == id
                                              select new QuotationSubmitMasterModel
                                              {
                                                  QuotationSubmitMasterId = t1.QuotationSubmitMasterId,
                                                  SubmissionDate = t1.SubmissionDate,
                                                  QuotationMasterId = t1.QuotationMasterId,
                                                  QuotationNo = t2.QuotationNo,
                                                  QuotationForId = t3.QuotationForId,
                                                  QuotationForName = t3.Name,
                                                  SupplierId = t4.VendorId,
                                                  SupplierName = t4.Name,
                                                  QuotationSubmitStatusId = (int)t1.StatusId,
                                                  CreatedDate = t1.CreatedOn,
                                                  CreatedBy = t1.CreatedBy,
                                                  CompanyFK = 21
                                              }).FirstOrDefault();

            sendData.QuotationDetailModelList = (from t1 in _context.QuotationSubmitMasters
                                                 join t2 in _context.QuotationMasters on t1.QuotationMasterId equals t2.QuotationMasterId into t2_Join
                                                 from t2 in t2_Join.DefaultIfEmpty()
                                                 join t3 in _context.QuotationDetails on t2.QuotationMasterId equals t3.QuotationMasterId into t3_Join
                                                 from t3 in t3_Join.DefaultIfEmpty()
                                                 join t4 in _context.Products on t3.MaterialId equals t4.ProductId into t4_Join
                                                 from t4 in t4_Join.DefaultIfEmpty()
                                                 where t1.IsActive && t1.QuotationSubmitMasterId == id
                                                 select new QuotationDetailModel
                                                 {
                                                     QuotationMasterId = t2.QuotationMasterId,
                                                     QuotationDetailId = t3.QuotationDetailId,
                                                     MaterialId = t4.ProductId,
                                                     MaterialName = t4.ProductName,
                                                     MaterialQualityId = t3.MaterialQuality,
                                                     Quantity = t3.Quantity,
                                                     CreatedDate = t1.CreatedOn,
                                                     CreatedBy = t1.CreatedBy,
                                                     CompanyFK = 21
                                                 }).ToList();
            return sendData;
        }

        public async Task<bool> CheckSupplierOnQuotation(long quotationId, int supplierId)
        {
            bool response = false;
            var isExist = await _context.QuotationSubmitMasters.FirstOrDefaultAsync(x => x.QuotationMasterId == quotationId && x.SupplierId == supplierId && x.IsActive);

            if (isExist != null)
            {
                response = true;
            }

            return response;
        }
        #endregion
    }
}
