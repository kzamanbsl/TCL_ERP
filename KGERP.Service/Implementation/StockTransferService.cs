﻿using KGERP.Data.Models;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class StockTransferService : IStockTransferService
    {
        private readonly ERPEntities _context;
        private readonly IManagerProductMapService _managerProductMapService;
        public StockTransferService(ERPEntities context, IManagerProductMapService managerProductMapService)
        {
            this._context = context;
            _managerProductMapService = managerProductMapService;
        }

        public List<StockTransferDetailModel> GetStockTransferDetail(int stockTransferId)
        {
            List<StockTransferDetailModel> stockTransferDetails = _context.Database.SqlQuery<StockTransferDetailModel>("EXEC Feed_GetStockTransferItems {0}", stockTransferId).ToList();
            return stockTransferDetails;
        }

        public async Task<StockTransferModel> GetGcclRmTransfer(int companyId, DateTime? fromDate, DateTime? toDate)
        {
            StockTransferModel stockTransferModel = new StockTransferModel();
            stockTransferModel.CompanyId = companyId;
            stockTransferModel.DataList = await Task.Run(() => (from t1 in _context.StockTransfers.Where(q => q.IsActive)
                                                                join t2 in _context.StockInfoes on t1.StockIdFrom equals t2.StockInfoId
                                                                join t3 in _context.StockInfoes on t1.StockIdTo equals t3.StockInfoId
                                                                where t1.CompanyId == companyId
                                                                && t1.TransferDate >= fromDate
                                                                && t1.TransferDate <= toDate

                                                                select new StockTransferModel
                                                                {
                                                                    CompanyId = t1.CompanyId,
                                                                    TransferDate = t1.TransferDate,
                                                                    StockFrom = t2.Name,
                                                                    StockTo = t3.Name,
                                                                    StockIdTo = t1.StockIdTo,
                                                                    StockIdFrom = t1.StockIdFrom,
                                                                    ChallanNo = t1.ChallanNo,
                                                                    VehicleNo = t1.VehicleNo,
                                                                    IsReceived = t1.IsReceived,
                                                                    StockTransferId = t1.StockTransferId,
                                                                    IsActive = t1.IsActive,
                                                                    IsSubmitted = t1.IsSubmitted

                                                                }).OrderByDescending(o => o.TransferDate).AsEnumerable());

            return stockTransferModel;

        }

        public async Task<StockTransferModel> GetStockTransfer(int companyId, DateTime? fromDate, DateTime? toDate)
        {
            StockTransferModel stockTransferModel = new StockTransferModel();
            stockTransferModel.CompanyId = companyId;
            stockTransferModel.DataList = await Task.Run(() => (from t1 in _context.StockTransfers.Where(q => q.IsActive)
                                                                join t2 in _context.StockInfoes on t1.StockIdFrom equals t2.StockInfoId into t2_Join
                                                                from t2 in t2_Join.DefaultIfEmpty()
                                                                join t3 in _context.StockInfoes on t1.StockIdTo equals t3.StockInfoId into t3_Join
                                                                from t3 in t3_Join.DefaultIfEmpty()
                                                                where t1.CompanyId == companyId
                                                                && t1.TransferDate >= fromDate
                                                                && t1.TransferDate <= toDate

                                                                select new StockTransferModel
                                                                {
                                                                    CompanyId = t1.CompanyId,
                                                                    TransferDate = t1.TransferDate,
                                                                    StockFrom = t2.Name,
                                                                    StockTo = t3.Name,
                                                                    StockIdTo = t1.StockIdTo,
                                                                    StockIdFrom = t1.StockIdFrom,
                                                                    ChallanNo = t1.ChallanNo,
                                                                    VehicleNo = t1.VehicleNo,
                                                                    IsReceived = t1.IsReceived,
                                                                    StockTransferId = t1.StockTransferId,
                                                                    IsActive = t1.IsActive,
                                                                    IsSubmitted = t1.IsSubmitted

                                                                }).OrderByDescending(o => o.TransferDate).AsEnumerable());

            return stockTransferModel;

        }

        public async Task<StockTransferModel> GetStockTransferApproveList(int companyId, DateTime? fromDate, DateTime? toDate, int status)
        {
            StockTransferModel stockTransferModel = new StockTransferModel();
            stockTransferModel.CompanyId = companyId;
            stockTransferModel.DataList = await Task.Run(() => (from t1 in _context.StockTransfers.Where(q => q.IsActive)
                                                                join t2 in _context.StockInfoes on t1.StockIdFrom equals t2.StockInfoId
                                                                join t3 in _context.StockInfoes on t1.StockIdTo equals t3.StockInfoId
                                                                where t1.CompanyId == companyId
                                                                && t1.TransferDate >= fromDate
                                                                && t1.TransferDate <= toDate
                                                                && t1.Status >= status

                                                                select new StockTransferModel
                                                                {
                                                                    CompanyId = t1.CompanyId,
                                                                    TransferDate = t1.TransferDate,
                                                                    StockFrom = t2.Name,
                                                                    StockTo = t3.Name,
                                                                    StockIdTo = t1.StockIdTo,
                                                                    StockIdFrom = t1.StockIdFrom,
                                                                    ChallanNo = t1.ChallanNo,
                                                                    VehicleNo = t1.VehicleNo,
                                                                    IsReceived = t1.IsReceived,
                                                                    StockTransferId = t1.StockTransferId,
                                                                    IsActive = t1.IsActive,
                                                                    IsSubmitted = t1.IsSubmitted

                                                                }).OrderByDescending(o => o.TransferDate).AsEnumerable());

            return stockTransferModel;

        }

        public async Task<StockTransferModel> GetStockTransferApprovalListByEmpId(int companyId, long employeeId, DateTime? fromDate, DateTime? toDate)
        {
            StockTransferModel stockTransferModel = new StockTransferModel();
            stockTransferModel.CompanyId = companyId;
            stockTransferModel.ApproveBy = employeeId;
            stockTransferModel.DataList = await Task.Run(() => (from t1 in _context.StockTransfers.Where(q => q.IsActive)
                                                                join t2 in _context.StockInfoes on t1.StockIdFrom equals t2.StockInfoId into t2_Join
                                                                from t2 in t2_Join.DefaultIfEmpty()
                                                                join t3 in _context.StockInfoes on t1.StockIdTo equals t3.StockInfoId into t3_Join
                                                                from t3 in t3_Join.DefaultIfEmpty()
                                                                join t4 in _context.StockTransferDetails on t1.StockTransferId equals t4.StockTransferId
                                                                where t1.CompanyId == companyId
                                                                      && t1.TransferDate >= fromDate
                                                                      && t1.TransferDate <= toDate

                                                                select new StockTransferModel
                                                                {
                                                                    CompanyId = t1.CompanyId,
                                                                    TransferDate = t1.TransferDate,
                                                                    StockFrom = t2.Name,
                                                                    StockTo = t3.Name,
                                                                    StockIdTo = t1.StockIdTo,
                                                                    StockIdFrom = t1.StockIdFrom,
                                                                    ChallanNo = t1.ChallanNo,
                                                                    VehicleNo = t1.VehicleNo,
                                                                    IsReceived = t1.IsReceived,
                                                                    StockTransferId = t1.StockTransferId,
                                                                    IsActive = t1.IsActive,
                                                                    IsSubmitted = t1.IsSubmitted,
                                                                    ApproveBy = employeeId,
                                                                    ProductId = t4.ProductId ?? 0,
                                                                    Status = t1.Status
                                                                }).OrderByDescending(o => o.TransferDate).AsEnumerable());

            var managerProductMapList = _managerProductMapService.GetManagerProductMapListByEmpId(companyId, employeeId);
            if (managerProductMapList == null || managerProductMapList?.Count <= 0)
            {
                stockTransferModel.DataList = new List<StockTransferModel>();
                return stockTransferModel;
            }

            var productIds = managerProductMapList.Select(o => o.ProductId).ToList();

            if (stockTransferModel?.DataList?.Count() > 0)
            {
                stockTransferModel?.DataList?.Where(c => productIds.Contains(c.ProductId));
            }

            return stockTransferModel;

        }

        public async Task<int> StockTransferAdd(StockTransferModel model)
        {
            int result = -1;
            var stockTransfers = _context.StockTransfers
                .Where(x => x.CompanyId == model.CompanyId)
                .OrderByDescending(x => x.StockTransferId)
                .Take(1);
            string challanNo = stockTransfers.ToList().FirstOrDefault()?.ChallanNo ?? "ST000000";
            string numberPart = challanNo?.Substring(2, 6);
            int lastNumberPart = Convert.ToInt32(numberPart);
            challanNo = GenerateSequenceNumber(lastNumberPart);

            StockTransfer obj = new StockTransfer
            {
                ChallanNo = challanNo,
                CompanyId = model.CompanyId,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreatedDate = DateTime.Now,
                IsActive = true,
                StockIdTo = model.StockIdTo,
                StockIdFrom = model.StockIdFrom,
                TransferDate = model.TransferDate,
                VehicleNo = model.VehicleNo,
                TruckFare = model.TruckFare,
                LabourBill = model.LabourBill,
                ReferenceNo = model.ReferenceNo,
                ReceiverPhone = model.ReceiverPhone,
                ReceivedBy = model.ReceivedBy,
                ReceivedDate = model.ReceivedDate
            };

            _context.StockTransfers.Add(obj);
            if (await _context.SaveChangesAsync() > 0)
            {
                result = obj.StockTransferId;
            }

            return result;
        }

        public async Task<int> StockTransferSlaveAdd(StockTransferModel model)
        {
            int result = -1;
            StockTransferDetail objDetail = new StockTransferDetail
            {
                StockTransferId = model.StockTransferId,
                TransferQty = model.TransferQty,

                ProductId = model.ProductId,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreateDate = DateTime.Now,
                IsActive = true
            };
            try
            {
                _context.StockTransferDetails.Add(objDetail);
                if (await _context.SaveChangesAsync() > 0)
                {
                    result = model.StockTransferId;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }

        public async Task<int> StockTransferSlaveEdit(StockTransferModel model)
        {
            var result = -1;
            StockTransferDetail objDetail = await _context.StockTransferDetails.FindAsync(model.StockTransferDetailId);

            objDetail.ProductId = model.ProductId;
            objDetail.TransferQty = model.TransferQty;
            objDetail.IsActive = true;



            objDetail.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            objDetail.ModifiedDate = DateTime.Now;
            if (await _context.SaveChangesAsync() > 0)
            {
                result = objDetail.StockTransferDetailId;
            }

            return result;
        }

        public async Task<int> StockTransferSlaveDelete(int stockTransferDetailId)
        {
            int result = -1;
            StockTransferDetail objDetail = await _context.StockTransferDetails.FindAsync(stockTransferDetailId);

            if (objDetail != null)
            {
                objDetail.IsActive = false;
                if (await _context.SaveChangesAsync() > 0)
                {
                    result = objDetail.StockTransferId ?? 0;
                }
            }
            return result;
        }

        public async Task<StockTransferDetailModel> GetSingleStockTransferSlave(int id)
        {
            var v = await Task.Run(() => (from t1 in _context.StockTransferDetails
                                          join t2 in _context.Products on t1.ProductId equals t2.ProductId
                                          join t3 in _context.Units on t2.UnitId equals t3.UnitId

                                          where t1.StockTransferDetailId == id
                                          select new StockTransferDetailModel
                                          {
                                              ProductName = t2.ProductName,
                                              ProductId = t2.ProductId,
                                              StockTransferId = t1.StockTransferId ?? 0,
                                              StockTransferDetailId = t1.StockTransferDetailId,
                                              TransferQty = t1.TransferQty ?? 0,

                                              CompanyId = t2.CompanyId ?? 0,
                                          }).FirstOrDefault());
            return v;
        }

        public async Task<StockTransferModel> GetStockTransferSlave(int companyId, int stockTransferId)
        {
            StockTransferModel model = new StockTransferModel();
            model = await Task.Run(() => (from t1 in _context.StockTransfers
                                          join t2 in _context.StockInfoes on t1.StockIdTo equals t2.StockInfoId into t2_Join
                                          from t2 in t2_Join.DefaultIfEmpty()
                                          join t3 in _context.StockInfoes on t1.StockIdFrom equals t3.StockInfoId into t3_Join
                                          from t3 in t3_Join.DefaultIfEmpty()
                                          join t4 in _context.Employees on t1.ReceivedBy equals t4.Id into t4_Join
                                          from t4 in t4_Join.DefaultIfEmpty()
                                          where t1.StockTransferId == stockTransferId
                                          && t1.CompanyId == companyId
                                          && t1.IsActive
                                          select new StockTransferModel
                                          {
                                              StockTransferId = t1.StockTransferId,
                                              CompanyId = t1.CompanyId,
                                              ChallanNo = t1.ChallanNo,
                                              TransferDate = t1.TransferDate,
                                              StockTo = t2.Name,
                                              StockFrom = t3.Name,
                                              VehicleNo = t1.VehicleNo,
                                              TruckFare = t1.TruckFare,
                                              LabourBill = t1.LabourBill,
                                              ReceivePerson = t4.Name,
                                              ReferenceNo = t1.ReferenceNo,
                                              ReceiverPhone = t1.ReceiverPhone,
                                              Status = t1.Status,
                                              IsActive = t1.IsActive,
                                              IsReceived = t1.IsReceived,
                                              IsSubmitted = t1.IsSubmitted,
                                              StockIdFrom = t1.StockIdFrom,
                                              StockIdTo = t1.StockIdTo

                                          }).FirstOrDefault());

            model.Items = await Task.Run(() => (from t1 in _context.StockTransferDetails
                                                join t2 in _context.Products on t1.ProductId equals t2.ProductId
                                                join t3 in _context.ProductSubCategories on t2.ProductSubCategoryId equals t3.ProductSubCategoryId
                                                join t4 in _context.ProductCategories on t3.ProductCategoryId equals t4.ProductCategoryId
                                                join t5 in _context.Units on t2.UnitId equals t5.UnitId
                                                where t1.StockTransferId == stockTransferId
                                                && t1.IsActive
                                                select new StockTransferDetailModel
                                                {
                                                    StockTransferId = t1.StockTransferId,
                                                    CompanyId = companyId,
                                                    StockTransferDetailId = t1.StockTransferDetailId,
                                                    ProductId = t1.ProductId,
                                                    ProductName = t4.Name + " " + t3.Name + "" + t2.ProductName,
                                                    UnitName = t5.Name,
                                                    TransferQty = t1.TransferQty,
                                                    IsRecieved = t1.IsReceived,
                                                    ReceivedDate = t1.ReceivedDate,
                                                    ReceivedQty = t1.ReceivedQty,
                                                    IsItemChecked = t1.IsReceived == 1 ? true : false
                                                }
                                           ).OrderByDescending(o => o.StockTransferDetailId)
                                           .ToListAsync());

            return model;
        }

        public async Task<int> StockTransferSubmit(int stockTransferId)
        {
            int result = -1;
            StockTransfer model = await _context.StockTransfers.FindAsync(stockTransferId);
            if (model != null)
            {
                if (model.Status == (int)StockTransferStatusEnum.Draft)
                {
                    model.Status = (int)StockTransferStatusEnum.Submitted;
                    model.IsSubmitted = true;
                }
                else
                {
                    model.Status = (int)StockTransferStatusEnum.Draft;
                    model.IsSubmitted = false;

                }
                model.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                model.ModifiedDate = DateTime.Now;

                if (await _context.SaveChangesAsync() > 0)
                {
                    result = model.StockTransferId;
                }
            }
            return result;
        }

        public object GetProductAutoComplete(int companyId, string prefix, string productType)
        {
            var result = (from t1 in _context.Products
                          join t2 in _context.ProductSubCategories on t1.ProductSubCategoryId equals t2.ProductSubCategoryId
                          join t3 in _context.ProductCategories on t2.ProductCategoryId equals t3.ProductCategoryId
                          join t4 in _context.Units on t1.UnitId equals t4.UnitId into t4_join
                          from t4 in t4_join.DefaultIfEmpty()
                          where t3.CompanyId == companyId && t1.IsActive && t3.ProductType == productType
                          && (t1.ProductName.ToLower().Contains(prefix) || t2.Name.ToLower().Contains(prefix) || t3.Name.ToLower().Contains(prefix))
                          select new
                          {
                              label = t3.Name + " - " + t2.Name + " - " + t1.ProductName,
                              val = t1.ProductId,
                              unit=t4.Name
                          }).Take(20).ToList();
            return result;
        }

        public ProductCurrentStockModel GetStockAvailableQuantity(int companyId, int productId, int stockFrom, string selectedDate)
        {

            var sql = $"exec sp_FinishGoodStockByProductDepoWise '{selectedDate}',{companyId},{stockFrom},{productId}";
            ProductCurrentStockModel data = _context.Database.SqlQuery<ProductCurrentStockModel>(sql).FirstOrDefault();
            return data;
        }
        public ProductCurrentStockModel GetStockAvailableQuantityByStockProduct( int productId, int stockFrom)
        {

            var sql = $"exec sp_MaterialStockByProductStockWise '',{stockFrom},{productId}";
            ProductCurrentStockModel data = _context.Database.SqlQuery<ProductCurrentStockModel>(sql).FirstOrDefault();
            return data;
        }

        public ProductCurrentStockModel GetRmAvailableQuantity(int companyId, int productId, int stockFrom, string selectedDate)
        {

            var sql = $"exec sp_FinishGoodStockByProductDepoWise '{selectedDate}',{companyId},{stockFrom},{productId}";
            ProductCurrentStockModel data = _context.Database.SqlQuery<ProductCurrentStockModel>(sql).FirstOrDefault();
            return data;
        }

        public List<StockTransferModel> GetStockTransferInfo(int companyId, DateTime? searchDate, string searchText)
        {
            IQueryable<StockTransferModel> models = _context.Database.SqlQuery<StockTransferModel>("exec spGetStockTransfer {0}", companyId).AsQueryable();

            if (searchDate == null)
            {
                return models.Where(x => (x.StockTo.ToLower().Contains(searchText.ToLower()) || String.IsNullOrEmpty(searchText)) ||
                         (x.ChallanNo.ToLower().Contains(searchText.ToLower()) || String.IsNullOrEmpty(searchText)) ||
                          (x.VehicleNo.ToLower().Contains(searchText.ToLower()) || String.IsNullOrEmpty(searchText))
                             ).OrderByDescending(x => x.TransferDate).ToList();
            }


            if (string.IsNullOrWhiteSpace(searchText) && searchDate != null)
            {
                return models.Where(x => x.TransferDate.Value.Date == searchDate.Value.Date).OrderByDescending(x => x.TransferDate).ToList();
            }

            return models.Where(x => x.TransferDate.Value.Date == searchDate.Value.Date && (x.StockTo.ToLower().Contains(searchText.ToLower()) || String.IsNullOrEmpty(searchText)) ||
                       (x.ChallanNo.ToLower().Contains(searchText.ToLower()) || String.IsNullOrEmpty(searchText)) ||
                        (x.VehicleNo.ToLower().Contains(searchText.ToLower()) || String.IsNullOrEmpty(searchText))
                           ).OrderByDescending(x => x.TransferDate).ToList();
        }

        public async Task<StockTransferModel> GetStockTransferById(int companyId, int stockTransferId)
        {
            StockTransferModel model = new StockTransferModel();
            model = await Task.Run(() => (from t1 in _context.StockTransfers
                                          join t2 in _context.StockInfoes on t1.StockIdTo equals t2.StockInfoId into t2_Join
                                          from t2 in t2_Join.DefaultIfEmpty()
                                          join t3 in _context.StockInfoes on t1.StockIdFrom equals t3.StockInfoId into t3_Join
                                          from t3 in t3_Join.DefaultIfEmpty()
                                          join t4 in _context.Employees on t1.ReceivedBy equals t4.Id into t4_Join
                                          from t4 in t4_Join.DefaultIfEmpty()
                                          where t1.StockTransferId == stockTransferId
                                          && t1.CompanyId == companyId
                                          && t1.IsActive
                                          select new StockTransferModel
                                          {
                                              StockTransferId = t1.StockTransferId,
                                              CompanyId = t1.CompanyId,
                                              ChallanNo = t1.ChallanNo,
                                              TransferDate = t1.TransferDate,
                                              ReceivedDate = t1.ReceivedDate,
                                              StockTo = t2.Name,
                                              StockFrom = t3.Name,
                                              VehicleNo = t1.VehicleNo,
                                              TruckFare = t1.TruckFare,
                                              LabourBill = t1.LabourBill,
                                              ReceivePerson = t4.Name,
                                              ReferenceNo = t1.ReferenceNo,
                                              ReceiverPhone = t1.ReceiverPhone,
                                              Status = t1.Status,
                                              IsActive = t1.IsActive,
                                              IsReceived = t1.IsReceived,
                                              IsSubmitted = t1.IsSubmitted,
                                              StockIdFrom = t1.StockIdFrom,
                                              StockIdTo = t1.StockIdTo

                                          }).FirstOrDefault());

            model.Items = await Task.Run(() => (from t1 in _context.StockTransferDetails
                                                join t2 in _context.Products on t1.ProductId equals t2.ProductId
                                                join t3 in _context.ProductSubCategories on t2.ProductSubCategoryId equals t3.ProductSubCategoryId
                                                join t4 in _context.ProductCategories on t3.ProductCategoryId equals t4.ProductCategoryId
                                                join t5 in _context.Units on t2.UnitId equals t5.UnitId
                                                where t1.StockTransferId == stockTransferId
                                                && t1.IsActive
                                                select new StockTransferDetailModel
                                                {
                                                    StockTransferId = t1.StockTransferId,
                                                    CompanyId = companyId,
                                                    StockTransferDetailId = t1.StockTransferDetailId,
                                                    ProductId = t1.ProductId,
                                                    ProductName = t4.Name + " " + t3.Name + " " + t2.ProductName,
                                                    UnitName = t5.Name,
                                                    TransferQty = t1.TransferQty,
                                                    IsRecieved = t1.IsReceived,
                                                    ReceivedDate = t1.ReceivedDate,
                                                    ReceivedQty = t1.ReceivedQty,
                                                    IsItemChecked = t1.IsReceived == 1 ? true : false
                                                }
                                           ).OrderByDescending(o => o.StockTransferDetailId)
                                           .ToListAsync());

            return model;
        }

        public bool ConfirmStockReceive(int stockTransferDetailId, decimal receiveQty, int stockTransferId, DateTime receiveDate, int productId, int companyId)
        {

            var modifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            _context.Database.ExecuteSqlCommand("Exec Sp_ConfirmStockTransfer {0},{1},{2},{3},{4},{5}", stockTransferId, receiveQty, modifiedBy, stockTransferDetailId, receiveDate, productId);
            return true;
        }

        public StockTransferModel GetStockTransfer(int id)
        {
            int companyId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CompanyId"]);
            string challanNo = string.Empty;

            if (id <= 0)
            {
                IQueryable<StockTransfer> stockTransfers = _context.StockTransfers.Where(x => x.CompanyId == companyId);
                int count = stockTransfers.Count();
                if (count == 0)
                {
                    return new StockTransferModel()
                    {
                        ChallanNo = GenerateSequenceNumber(0),
                        CompanyId = companyId
                    };
                }

                stockTransfers = stockTransfers.Where(x => x.CompanyId == companyId).OrderByDescending(x => x.StockTransferId).Take(1);
                challanNo = stockTransfers.ToList().FirstOrDefault()?.ChallanNo ?? string.Empty;
                string numberPart = challanNo.Substring(2, 6);
                int lastNumberPart = Convert.ToInt32(numberPart);
                challanNo = GenerateSequenceNumber(lastNumberPart);
                return new StockTransferModel()
                {
                    ChallanNo = challanNo,
                    CompanyId = companyId
                };

            }

            StockTransfer stockTransfer = _context.StockTransfers.Include(x => x.StockTransferDetails).FirstOrDefault(x => x.StockTransferId == id);
            if (stockTransfer == null)
            {
                throw new Exception("Date not found");
            }

            StockTransferModel model = ObjectConverter<StockTransfer, StockTransferModel>.Convert(stockTransfer);

            return model;
        }

        private string GenerateSequenceNumber(int lastReceivedNo)
        {
            int num = ++lastReceivedNo;
            return "ST" + num.ToString().PadLeft(6, '0');
        }

        public bool DeleteStockTransfer(int stockTransferId)
        {
            StockTransfer stockTransfer = _context.StockTransfers.Include(x => x.StockTransferDetails).FirstOrDefault(x => x.StockTransferId == stockTransferId);
            if (stockTransfer == null)
            {
                return false;
            }
            if (stockTransfer.StockTransferDetails.Any())
            {
                _context.StockTransferDetails.RemoveRange(stockTransfer.StockTransferDetails);
                _context.SaveChanges();
            }
            _context.StockTransfers.Remove(stockTransfer);
            return _context.SaveChanges() > 0;
        }

        public async Task<int> StockReceivedUpdate(StockTransferModel model)
        {
            int result = -1;

            int totalItems = model.Items.Count();
            int totalCheckedItems = model.Items.Count(x => x.IsItemChecked);
            if (totalItems != totalCheckedItems)
            {
                return result;
            }
            StockTransfer stockTransfer = await _context.StockTransfers
                .Where(x => x.StockTransferId == model.StockTransferId).FirstOrDefaultAsync();

            if (stockTransfer == null)
            {
                return result;
            }
            stockTransfer.IsReceived = 1;
            stockTransfer.ReceivedDate = model.ReceivedDate;
            stockTransfer.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            stockTransfer.ModifiedDate = DateTime.Now;
            stockTransfer.Status = (int)StockTransferStatusEnum.Reveived;

            foreach (var item in model.Items)
            {
                var objDetail = await _context.StockTransferDetails
                    .SingleOrDefaultAsync(s => s.StockTransferId == stockTransfer.StockTransferId
                    && s.StockTransferDetailId == item.StockTransferDetailId);

                if (objDetail != null)
                {
                    objDetail.ReceivedQty = item.ReceivedQty;
                    objDetail.ReceivedDate = model.ReceivedDate;
                    objDetail.IsReceived = 1;
                    objDetail.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    objDetail.ModifiedDate = DateTime.Now;
                }
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                result = model.StockTransferId;
            }

            return result;
        }

        public async Task<int> StockApproveUpdate(StockTransferModel model)
        {
            int result = -1;

            int totalItems = model.Items.Count();
            int totalCheckedItems = model.Items.Count(x => x.IsItemChecked);
            if (totalItems != totalCheckedItems)
            {
                return result;
            }
            StockTransfer stockTransfer = await _context.StockTransfers
                .Where(x => x.StockTransferId == model.StockTransferId).FirstOrDefaultAsync();

            if (stockTransfer == null)
            {
                return result;
            }
            stockTransfer.IsReceived = 0;
            stockTransfer.ApproveBy = model.ApproveBy;
            stockTransfer.ApproveDate = model.ApproveDate;
            stockTransfer.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            stockTransfer.ModifiedDate = DateTime.Now;
            stockTransfer.Status = (int)StockTransferStatusEnum.Submitted;
            stockTransfer.StockIdFrom = model.StockIdFrom;

            foreach (var item in model.Items)
            {
                var objDetail = await _context.StockTransferDetails
                    .SingleOrDefaultAsync(s => s.StockTransferId == stockTransfer.StockTransferId
                    && s.StockTransferDetailId == item.StockTransferDetailId);

                if (objDetail != null)
                {
                    objDetail.TransferQty = item.TransferQty;
                    objDetail.IsReceived = 0;
                    objDetail.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    objDetail.ModifiedDate = DateTime.Now;
                }
            }

            try
            {
                if (await _context.SaveChangesAsync() > 0)
                {
                    result = model.StockTransferId;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;

        }
    }
}
