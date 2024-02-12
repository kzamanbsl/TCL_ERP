using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class ConsumptionService : IConsumptionService
    {
        private readonly ERPEntities _context;
        private readonly ConfigurationService _configurationService;
        public ConsumptionService(ERPEntities erpEntities, ConfigurationService configurationService)
        {
            _context = erpEntities;
            _configurationService = configurationService;
        }

        public async Task<long> CreateConsumptionMaster(ConsumptionModel consumption)
        {
            var result = -1;
            if (consumption is null)
            {
                return await Task.Run(() => result);
            }



            ConsumptionMaster consumptionMaster = new ConsumptionMaster()
            {

                BoqItemId = consumption.BOQItemId,
                StoreId = consumption?.StockInfoId ?? 0,
                DivisionId = consumption.DivisionId,
                IsActive=true,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreatedOn = DateTime.Now
            };
            try
            {
                _context.ConsumptionMasters.Add(consumptionMaster);
                var count = _context.SaveChanges();
                if (count > 0)
                {
                    return consumptionMaster.ConsumptionMasterId;
                }
            }
            catch (Exception e)
            {
                return result;
            }


            return result;
        }
        public async Task<bool> CreateConsumptionDetail(ConsumptionModel consumption)
        {
            if (consumption is null || consumption.ConsumptionMasterId <= 0)
            {
                return await Task.Run(() => false);
            }



            try
            {
                ConsumptionDetail consumptionDetail = new ConsumptionDetail()
                {
                    ConsumptionMasterId = consumption?.ConsumptionMasterId ?? 0,
                    //ProductSubtypeId = consumption.DetailModel?.ProductSubtypeId ?? 0,
                    ProductId = consumption.DetailModel?.ProductId ?? 0,
                    UnitPrice = consumption.DetailModel.UnitPrice,
                    ConsumedQty = consumption.DetailModel.ConsumedQty,
                    RemainingQty = consumption.DetailModel.RemainingQty,
                    TotalAmount = (consumption.DetailModel.UnitPrice * consumption.DetailModel.ConsumedQty),
                    IsActive=true,
                    CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                    CreatedOn = DateTime.Now
                };
                _context.ConsumptionDetails.Add(consumptionDetail);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);

            }

            return false;
        }
        public async Task<bool> DeleteConsumption(ConsumptionModel consumption)
        {
            if (consumption is null || consumption.ConsumptionMasterId <= 0)
            {
                return await Task.Run(() => false);
            }
            var consumptionMaster = await _context.ConsumptionMasters.FirstOrDefaultAsync(c => c.ConsumptionMasterId == consumption.ConsumptionMasterId);

            return false;
        }

        public async Task<ConsumptionDetailModel> ConsumptionDetailGetById(int? Id)
        {
            if (!Id.HasValue || Id.Value == 0)
            {
                return new ConsumptionDetailModel();
            }

            try
            {
                var consumption = await _context.ConsumptionDetails.FirstOrDefaultAsync(c => c.ConsumptionDetailsId == Id);

                if (consumption == null || consumption.ProductId <= 0) return new ConsumptionDetailModel();
                var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == consumption.ProductId);

                var consumptionDetail = new ConsumptionDetailModel()
                {
                    ConsumptionDetailsId = (int)consumption.ConsumptionDetailsId,
                    ConsumptionMasterId = (int)consumption.ConsumptionMasterId,
                    ProductName = product.ProductName,
                    UnitPrice = (decimal)consumption.UnitPrice,
                    ConsumedQty = (int)consumption.ConsumedQty,
                    StoredQty = (int)consumption.StoredQty,
                    RemainingQty = (int)consumption.RemainingQty,
                    TotalPrice = (int)consumption.TotalAmount
                };
                return consumptionDetail;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            return new ConsumptionDetailModel();
        }

        public async Task<bool> UpdateConsumptionDetail(ConsumptionModel consumption)
        {
            if (consumption is null || consumption.ConsumptionMasterId <= 0)
            {
                return await Task.Run(() => false);
            }
            var detail = _context.ConsumptionDetails.FirstOrDefault(c => c.ConsumptionMasterId == consumption.ConsumptionMasterId && c.ConsumptionDetailsId == consumption.ConsumptionDetailsId);
            if (detail is null)
            {
                return await Task.Run(() => false);
            }


            try
            {

                //detail.ProductSubtypeId = consumption.DetailModel?.ProductSubtypeId ?? 0;
                detail.ProductId = consumption.DetailModel?.ProductId ?? 0;
                detail.UnitPrice = consumption.DetailModel.UnitPrice;
                detail.ConsumedQty = consumption.DetailModel.ConsumedQty;
                detail.StoredQty = consumption.DetailModel?.StoredQty;
                detail.RemainingQty = consumption.DetailModel?.RemainingQty;
                detail.TotalAmount = consumption.DetailModel?.TotalPrice;
                detail.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                detail.ModifiedOn = DateTime.Now;
                var count = _context.SaveChanges();
                if (count > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }


            return false;
        }
        public async Task<ConsumptionModel> GetConsumptionMasterDetail(long consumptionMasterId = 0)
        {
            ConsumptionModel consumptionMasterModel = new ConsumptionModel();


            consumptionMasterModel = await Task.Run(() => (from t1 in _context.ConsumptionMasters.Where(x => x.ConsumptionMasterId == consumptionMasterId)

                                                           join t2 in _context.Stores on t1.StoreId > 0 ? t1.StoreId : 0 equals t2.StoreId > 0 ? t2.StoreId : 0 into t2_Join
                                                           from t2 in t2_Join.DefaultIfEmpty()
                                                           join t3 in _context.BoQDivisions on t1.DivisionId > 0 ? t1.DivisionId : 0 equals t3.BoQDivisionId > 0 ? t3.BoQDivisionId : 0 into t3_Join
                                                           from t3 in t3_Join.DefaultIfEmpty()
                                                           join t4 in _context.BillBoQItems on (int)t1.BoqItemId > 0 ? t1.BoqItemId : 0 equals t4.BoQItemId > 0 ? t4.BoQItemId : 0 into t4_Join
                                                           from t4 in t4_Join.DefaultIfEmpty()
                                                           join t5 in _context.Employees on t1.CreatedBy equals t5.EmployeeId into t5_Join
                                                           from t5 in t5_Join.DefaultIfEmpty()
                                                           join t6 in _context.Accounting_CostCenter on t3.ProjectId > 0 ? t3.ProjectId : 0 equals t6.CostCenterId > 0 ? t6.CostCenterId : 0 into t6_Join
                                                           from t6 in t6_Join.DefaultIfEmpty()
                                                           join t7 in _context.Accounting_CostCenterType on t6.CostCenterTypeId > 0 ? t6.CostCenterTypeId : 0 equals t7.CostCenterTypeId > 0 ? t7.CostCenterTypeId : 0 into t7_Join
                                                           from t7 in t7_Join.DefaultIfEmpty()

                                                           select new ConsumptionModel
                                                           {
                                                               ConsumptionMasterId = t1.ConsumptionMasterId,
                                                               ProjectTypeId = t7.CostCenterTypeId > 0 ? t7.CostCenterTypeId : 0,
                                                               ProjectTypeName = t7.Name,
                                                               CostCenterId = t6.CostCenterId > 0 ? t6.CostCenterId : 0,
                                                               CostCenterName = t6.Name,
                                                               StockInfoId = t2.StoreId > 0 ? t2.StoreId : 0,
                                                               BoQDivisionId = t3.BoQDivisionId > 0 ? t3.BoQDivisionId : 0,
                                                               BoQDivisionName = t3.Name,
                                                               BOQItemId = t4.BoQItemId > 0 ? t4.BoQItemId : 0,
                                                               BOQItemName = t4.Name,
                                                               CreatedDate = t1.CreatedOn,
                                                               CreatedBy = t5.Name

                                                           }).FirstOrDefault());

            var consumptionDetailList = await Task.Run(() => (from t1 in _context.ConsumptionDetails.Where(x => x.ConsumptionMasterId == consumptionMasterId)
                                                              join t2 in _context.ConsumptionMasters on t1.ConsumptionMasterId equals t2.ConsumptionMasterId into t2_Join
                                                              from t2 in t2_Join.DefaultIfEmpty()
                                                              join t3 in _context.Products on t1.ProductId equals t3.ProductId into t3_Join
                                                              from t3 in t3_Join.DefaultIfEmpty()
                                                                  //join t4 in _context.Units on t1.UnitId equals t4.UnitId into t4_Join
                                                                  //from t4 in t4_Join.DefaultIfEmpty()
                                                                  //join t5 in _context.BillBoQItems on t2.BoqItemId equals t5.BoQItemId into t5_Join
                                                                  //from t5 in t5_Join.DefaultIfEmpty()
                                                                  //join t6 in _context.BoQDivisions on t5.BoQDivisionId equals t6.BoQDivisionId into t6_Join
                                                                  //from t6 in t6_Join.DefaultIfEmpty()
                                                              join t7 in _context.ProductSubCategories on t3.ProductSubCategoryId equals t7.ProductSubCategoryId into t7_Join
                                                              from t7 in t7_Join.DefaultIfEmpty()
                                                              select new ConsumptionDetailModel
                                                              {
                                                                  ConsumptionMasterId = t1.ConsumptionMasterId,
                                                                  ConsumptionDetailsId = t1.ConsumptionDetailsId,
                                                                  ProductSubTypeId = t7.ProductSubCategoryId,
                                                                  ProductSubTypeName = t7.Name,
                                                                  ProductId = t3.ProductId,
                                                                  ProductName = t3.ProductName,
                                                                  UnitPrice = t1.UnitPrice,
                                                                  TotalPrice = t1.TotalAmount > 0 ? t1.TotalAmount : t1.ConsumedQty * t1.UnitPrice,
                                                                  StoredQty = t1.StoredQty,
                                                                  ConsumedQty = t1.ConsumedQty,
                                                                  RemainingQty = t1.RemainingQty

                                                              }).OrderByDescending(x => x.ConsumptionDetailsId).AsEnumerable());
            consumptionMasterModel.DetailList = consumptionDetailList.ToList();
            consumptionMasterModel.TotalAmount = consumptionMasterModel.DetailList.Select(x => x.TotalPrice).Sum();

            return consumptionMasterModel;
        }

    }
}
