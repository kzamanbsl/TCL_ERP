using KGERP.Data.Models;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class BillRequisitionService : IBillRequisitionService
    {
        private readonly ERPEntities _context;
        public BillRequisitionService(ERPEntities context)
        {
            _context = context;
        }
        public int GetRequisitionNo()
        {
            int requisitionId = 0;
            //var value = _context.Requisitions.OrderByDescending(x => x.RequisitionId).FirstOrDefault();
            //if (value != null)
            //{
            //    requisitionId = value.RequisitionId + 1;
            //}
            //else
            //{
            //    requisitionId = requisitionId + 1;
            //}
            return requisitionId;
        }

        #region Bill Requisition Item

        public List<BillRequisitionItem> GetBillRequisitionItemList()
        {
            List<BillRequisitionItem> billRequisitionItems = new List<BillRequisitionItem>();
            var getBillRequisitionItems = _context.BillRequisitionItems.Where(c => c.IsActive == true).ToList();
            foreach (var item in getBillRequisitionItems)
            {
                var data = new BillRequisitionItem()
                {
                    BillRequisitionItemId = item.BillRequisitionItemId,
                    Name = item.Name,
                    Description = item.Description
                };
                billRequisitionItems.Add(data);
            }
            return billRequisitionItems;
        }

        public bool Add(BillRequisitionItemModel model)
        {
            if (model != null)
            {
                try
                {
                    BillRequisitionItem data = new BillRequisitionItem()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        //CompanyId = (int)model.CompanyFK,
                        CompanyId = 21,
                        IsActive = true,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CreateDate = DateTime.Now
                    };
                    _context.BillRequisitionItems.Add(data);
                    var count = _context.SaveChanges();
                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public bool Edit(BillRequisitionItemModel model)
        {
            if (model != null)
            {
                try
                {
                    var findBillRequisitionItem = _context.BillRequisitionItems.FirstOrDefault(c => c.BillRequisitionItemId == model.BillRequisitionItemId);

                    findBillRequisitionItem.Name = model.Name;
                    findBillRequisitionItem.Description = model.Description;
                    findBillRequisitionItem.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBillRequisitionItem.ModifiedDate = DateTime.Now;
                    var count = _context.SaveChanges();
                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public bool Delete(BillRequisitionItemModel model)
        {
            if (model.BillRequisitionItemId > 0 || model.BillRequisitionItemId != null)
            {
                try
                {
                    var findBillRequisitionItem = _context.BillRequisitionItems.FirstOrDefault(c => c.BillRequisitionItemId == model.BillRequisitionItemId);

                    findBillRequisitionItem.IsActive = false;
                    findBillRequisitionItem.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBillRequisitionItem.ModifiedDate = DateTime.Now;
                    var count = _context.SaveChanges();

                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region Bill Requisition Type

        public List<BillRequisitionType> GetBillRequisitionTypeList()
        {
            List<BillRequisitionType> billRequisitionTypes = new List<BillRequisitionType>();
            var getBillRequisitionTypes = _context.BillRequisitionTypes.Where(c => c.IsActive == true).ToList();
            foreach (var item in getBillRequisitionTypes)
            {
                var data = new BillRequisitionType()
                {
                    BillRequisitionTypeId = item.BillRequisitionTypeId,
                    Name = item.Name,
                    Description = item.Description
                };
                billRequisitionTypes.Add(data);
            }
            return billRequisitionTypes;
        }

        public bool Add(BillRequisitionTypeModel model)
        {
            if (model != null)
            {
                try
                {
                    BillRequisitionType data = new BillRequisitionType()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        //CompanyId = (int)model.CompanyFK,
                        CompanyId = 21,
                        IsActive = true,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CreateDate = DateTime.Now
                    };
                    _context.BillRequisitionTypes.Add(data);
                    var count = _context.SaveChanges();
                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public bool Edit(BillRequisitionTypeModel model)
        {
            if (model != null)
            {
                try
                {
                    var findBillRequisitionType = _context.BillRequisitionTypes.FirstOrDefault(c => c.BillRequisitionTypeId == model.BillRequisitionTypeId);

                    findBillRequisitionType.Name = model.Name;
                    findBillRequisitionType.Description = model.Description;
                    findBillRequisitionType.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBillRequisitionType.ModifiedDate = DateTime.Now;
                    var count = _context.SaveChanges();
                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public bool Delete(BillRequisitionTypeModel model)
        {
            if (model.BillRequisitionTypeId > 0 || model.BillRequisitionTypeId != null)
            {
                try
                {
                    var findBillRequisitionType = _context.BillRequisitionTypes.FirstOrDefault(c => c.BillRequisitionTypeId == model.BillRequisitionTypeId);

                    findBillRequisitionType.IsActive = false;
                    findBillRequisitionType.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBillRequisitionType.ModifiedDate = DateTime.Now;
                    var count = _context.SaveChanges();

                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region Cost Center Manager Map

        public List<Accounting_CostCenter> GetProjectList()
        {
            List<Accounting_CostCenter> projects = new List<Accounting_CostCenter>();
            var getProject = _context.Accounting_CostCenter.Where(c => c.IsActive == true).ToList();
            foreach (var project in getProject)
            {
                var data = new Accounting_CostCenter()
                {
                    CompanyId = project.CompanyId,
                    CostCenterId = project.CostCenterId,
                    Name = project.Name
                };
                projects.Add(data);
            }
            return projects;
        }

        public List<Employee> GetEmployeeList()
        {
            List<Employee> employees = new List<Employee>();
            var getEmployee = _context.Employees.Where(c => c.Active == true).ToList();
            foreach (var emp in getEmployee)
            {
                var data = new Employee()
                {
                    Id = emp.Id,
                    EmployeeId = emp.EmployeeId,
                    Name = emp.Name
                };
                employees.Add(data);
            }
            return employees;
        }

        public List<CostCenterManagerMap> GetCostCenterManagerMapList()
        {
            List<CostCenterManagerMap> costCenterManagerMap = new List<CostCenterManagerMap>();
            var getCostCenterManagerMaps = _context.CostCenterManagerMaps.Where(c => c.IsActive == true).ToList();
            foreach (var item in getCostCenterManagerMaps)
            {
                var data = new CostCenterManagerMap()
                {
                    CostCenterManagerMapId = item.CostCenterManagerMapId,
                    CostCenterId = item.CostCenterId,
                    ManagerId = item.ManagerId,
                };
                costCenterManagerMap.Add(data);
            }
            return costCenterManagerMap;
        }

        public bool Add(CostCenterManagerMapModel model)
        {
            if (model != null)
            {
                try
                {
                    CostCenterManagerMap data = new CostCenterManagerMap()
                    {
                        CostCenterId = model.ProjectId,
                        ManagerId = model.EmployeeRowId,
                        //CompanyId = (int)model.CompanyFK,
                        CompanyId = 21,
                        IsMapActive = true,
                        IsActive = true,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CreateDate = DateTime.Now
                    };
                    _context.CostCenterManagerMaps.Add(data);
                    var count = _context.SaveChanges();
                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public bool Edit(CostCenterManagerMapModel model)
        {
            if (model != null)
            {
                try
                {
                    var findCostCenterManagerMap = _context.CostCenterManagerMaps.FirstOrDefault(c => c.CostCenterManagerMapId == model.CostCenterManagerMapId);

                    findCostCenterManagerMap.CostCenterId = model.ProjectId;
                    findCostCenterManagerMap.ManagerId = model.EmployeeRowId;
                    findCostCenterManagerMap.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findCostCenterManagerMap.ModifiedDate = DateTime.Now;
                    var count = _context.SaveChanges();
                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public bool Delete(CostCenterManagerMapModel model)
        {
            if (model.CostCenterManagerMapId > 0 || model.CostCenterManagerMapId != null)
            {
                try
                {
                    var findCostCenterManagerMap = _context.CostCenterManagerMaps.FirstOrDefault(c => c.CostCenterManagerMapId == model.CostCenterManagerMapId);

                    findCostCenterManagerMap.IsActive = false;
                    findCostCenterManagerMap.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findCostCenterManagerMap.ModifiedDate = DateTime.Now;
                    var count = _context.SaveChanges();

                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region BillRequisition Master Detail
        public async Task<BillRequisitionMasterModel> GetBillRequisitionMasterDetail(int companyId, long billRequisitionMasterId)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();


            billRequisitionMasterModel = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive && x.BillRequisitionMasterId == billRequisitionMasterId)

                                                               join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                               from t2 in t2_Join.DefaultIfEmpty()
                                                               join t3 in _context.BillRequisitionTypes on t1.BillRequisitionTypeId equals t3.BillRequisitionTypeId into t3_Join
                                                               from t3 in t3_Join.DefaultIfEmpty()

                                                               select new BillRequisitionMasterModel
                                                               {
                                                                   BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                   BillRequisitionTypeId = t1.BillRequisitionTypeId,
                                                                   BRTypeName = t3.Name,
                                                                   CostCenterId = t1.CostCenterId,
                                                                   CostCenterName = t2.Name,
                                                                   Description = t1.Description,
                                                                   BRDate = t1.BRDate,
                                                                   BillRequisitionNo = t1.BillRequisitionNo,
                                                                   StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                                                   CompanyFK = t1.CompanyId,
                                                                   CreatedDate = t1.CreateDate,
                                                                   CreatedBy = t1.CreatedBy,

                                                               }).FirstOrDefault());
            billRequisitionMasterModel.DetailList = await Task.Run(() => (from t1 in _context.BillRequisitionDetails.Where(x => x.IsActive && x.BillRequisitionMasterId == billRequisitionMasterId)
                                                                          join t2 in _context.BillRequisitionMasters.Where(x => x.IsActive) on t1.BillRequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                                          from t2 in t2_Join.DefaultIfEmpty()
                                                                          join t3 in _context.BillRequisitionItems.Where(x => x.IsActive) on t1.BillRequisitionItemId equals t3.BillRequisitionItemId into t3_Join
                                                                          from t3 in t3_Join.DefaultIfEmpty()

                                                                          select new BillRequisitionDetailModel
                                                                          {
                                                                              BillRequisitionDetailId = t1.BillRequisitionDetailId,
                                                                              BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                              BillRequisitionItemId = t1.BillRequisitionItemId,
                                                                              ItemName = t3.Name,
                                                                              Qty = t1.Qty,
                                                                              Description = t1.Description,
                                                                              UnitPrice = t1.UnitPrice,
                                                                              TotalAmount = t1.TotalAmount,
                                                                          }).OrderByDescending(x => x.BillRequisitionDetailId).AsEnumerable());


            return billRequisitionMasterModel;
        }

        public async Task<long> BillRequisitionMasterAdd(BillRequisitionMasterModel model)
        {
            long result = -1;

            try
            {
                BillRequisitionMaster billRequisitionMaster = new BillRequisitionMaster
                {
                    BillRequisitionMasterId = model.BillRequisitionMasterId,
                    BRDate = model.BRDate,
                    BillRequisitionTypeId = model.BillRequisitionTypeId,
                    CostCenterId = model.CostCenterId,
                    Description = model.Description,
                    BillRequisitionNo = GetUniqueRequisitionNo(),
                    StatusId = (int)model.StatusId,
                    CompanyId = (int)model.CompanyFK,
                    CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                    CreateDate = DateTime.Now,
                    IsActive = true
                };

                _context.BillRequisitionMasters.Add(billRequisitionMaster);

                if (await _context.SaveChangesAsync() > 0)
                {
                    result = billRequisitionMaster.BillRequisitionMasterId;
                }
                return result;
            }
            catch (Exception error)
            {
                return result;
            }

            // Generate unique bill requisition number
            string GetUniqueRequisitionNo()
            {
                #region Generate Unique Requisition Number With Last Id

                int getLastRowId = _context.BillRequisitionMasters.Where(c => c.CreateDate == DateTime.Today).Count();

                string setZeroBeforeLastId(int lastRowId, int length)
                {
                    string totalDigit = "";

                    for (int i = (length - lastRowId.ToString().Length); 0 < i; i--)
                    {
                        totalDigit += "0";
                    }
                    return totalDigit + lastRowId.ToString();
                }

                string prefix = "REQ";

                string generatedNumber = $"{prefix.ToUpper()}-{DateTime.Now:yyMMdd}-{setZeroBeforeLastId(++getLastRowId, 4)}";

                return generatedNumber;

                #endregion

                #region Generate Unique Requisition Number With Guid

                //string prefix = "REQ";
                //string uniqueIdentifier = Guid.NewGuid().ToString("N").Substring(0, 5);
                //string generatedNumber = $"{prefix.ToUpper()}-{DateTime.Now:yyMMdd}-{uniqueIdentifier}";
                //return generatedNumber;

                #endregion
            }
        }

        public async Task<long> BillRequisitionDetailAdd(BillRequisitionMasterModel model)
        {
            long result = -1;
            BillRequisitionDetail demageDetail = new BillRequisitionDetail
            {
                BillRequisitionMasterId = model.BillRequisitionMasterId,
                BillRequisitionDetailId = model.DetailModel.BillRequisitionDetailId,
                BillRequisitionItemId = model.DetailModel.BillRequisitionItemId,
                UnitPrice = model.DetailModel.UnitPrice,
                Qty = model.DetailModel.Qty,
                Description = model.DetailModel.Description,
                CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                CreateDate = DateTime.Now,
                IsActive = true,

            };
            _context.BillRequisitionDetails.Add(demageDetail);

            if (await _context.SaveChangesAsync() > 0)
            {
                result = demageDetail.BillRequisitionMasterId;
            }

            return result;
        }
        public async Task<long> BillRequisitionDetailEdit(BillRequisitionMasterModel model)
        {
            long result = -1;
            BillRequisitionDetail demageDetail = await _context.BillRequisitionDetails.FindAsync(model.DetailModel.BillRequisitionDetailId);
            if (demageDetail == null) throw new Exception("Sorry! item not found!");

            demageDetail.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            demageDetail.ModifiedDate = DateTime.Now;

            demageDetail.BillRequisitionMasterId = model.BillRequisitionMasterId;
            demageDetail.BillRequisitionDetailId = model.DetailModel.BillRequisitionDetailId;
            demageDetail.BillRequisitionItemId = model.DetailModel.BillRequisitionItemId;
            demageDetail.UnitPrice = model.DetailModel.UnitPrice;
            demageDetail.Qty = model.DetailModel.Qty;
            demageDetail.Description = model.DetailModel.Description;
            demageDetail.CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString();
            demageDetail.CreateDate = DateTime.Now;
            demageDetail.IsActive = true;

            demageDetail.IsActive = true;
            if (await _context.SaveChangesAsync() > 0)
            {
                result = demageDetail.BillRequisitionDetailId;
            }

            return result;
        }
        public async Task<long> SubmitBillRequisitionMaster(long? id = 0)
        {
            long result = -1;

            BillRequisitionMaster billRequisitionMaster = await _context.BillRequisitionMasters.FindAsync(id);

            if (billRequisitionMaster == null)
            {
                throw new Exception("Sorry! item not found!");
            }

            if (billRequisitionMaster.StatusId == (int)EnumBillRequisitionStatus.Draft)
            {
                billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Submitted;
            }
            else
            {
                billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Draft;
            }

            billRequisitionMaster.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            if (await _context.SaveChangesAsync() > 0)
            {
                result = billRequisitionMaster.BillRequisitionMasterId;
            }
            return result;
        }
        public async Task<long> BillRequisitionMasterEdit(BillRequisitionMasterModel model)
        {
            long result = -1;
            BillRequisitionMaster billRequisitionMaster = await _context.BillRequisitionMasters.FindAsync(model.BillRequisitionMasterId);
            billRequisitionMaster.BRDate = model.BRDate;
            billRequisitionMaster.BillRequisitionNo = model.BillRequisitionNo;
            billRequisitionMaster.BillRequisitionTypeId = model.BillRequisitionTypeId;
            billRequisitionMaster.CostCenterId = model.CostCenterId;
            billRequisitionMaster.Description = model.Description;
            billRequisitionMaster.StatusId = (int)model.StatusId;
            billRequisitionMaster.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            billRequisitionMaster.ModifiedDate = DateTime.Now;
            if (await _context.SaveChangesAsync() > 0)
            {
                result = billRequisitionMaster.BillRequisitionMasterId;
            }

            return result;
        }
        public async Task<BillRequisitionMasterModel> GetBillRequisitionMasterById(long billRequisitionMasterId)
        {

            var v = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive && x.BillRequisitionMasterId == billRequisitionMasterId)

                                          join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                          from t2 in t2_Join.DefaultIfEmpty()
                                          join t3 in _context.BillRequisitionTypes on t1.BillRequisitionTypeId equals t3.BillRequisitionTypeId into t3_Join
                                          from t3 in t3_Join.DefaultIfEmpty()

                                          select new BillRequisitionMasterModel
                                          {
                                              BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                              BillRequisitionTypeId = t1.BillRequisitionTypeId,
                                              BRTypeName = t3.Name,
                                              CostCenterId = t1.CostCenterId,
                                              CostCenterName = t2.Name,
                                              Description = t1.Description,
                                              BRDate = t1.BRDate,
                                              BillRequisitionNo = t1.BillRequisitionNo,
                                              StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                              CompanyFK = t1.CompanyId,
                                              CreatedDate = t1.CreateDate,
                                              CreatedBy = t1.CreatedBy,
                                          }).FirstOrDefault());
            return v;
        }
        public async Task<long> BillRequisitionDetailDelete(long id)
        {
            long result = -1;

            BillRequisitionDetail demageDetail = await _context.BillRequisitionDetails.FindAsync(id);
            if (demageDetail == null)
            {
                throw new Exception("Sorry! Order not found!");
            }

            demageDetail.IsActive = false;

            if (await _context.SaveChangesAsync() > 0)
            {
                result = demageDetail.BillRequisitionDetailId;
            }

            return result;
        }
        public async Task<long> BillRequisitionMasterDelete(long id)
        {
            long result = -1;
            BillRequisitionMaster billRequisitionMaster = await _context.BillRequisitionMasters.FindAsync(id);
            if (billRequisitionMaster == null)
            {
                throw new Exception("Sorry! item not found!");
            }

            billRequisitionMaster.IsActive = false;
            billRequisitionMaster.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            billRequisitionMaster.ModifiedDate = DateTime.Now;
            if (await _context.SaveChangesAsync() > 0)
            {
                result = billRequisitionMaster.BillRequisitionMasterId;
            }


            return result;
        }
        public async Task<BillRequisitionDetailModel> GetSingleBillRequisitionDetails(long id)
        {
            var v = await Task.Run(() => (from t1 in _context.BillRequisitionDetails.Where(x => x.IsActive && x.BillRequisitionDetailId == id)
                                          join t2 in _context.BillRequisitionMasters.Where(x => x.IsActive) on t1.BillRequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                          from t2 in t2_Join.DefaultIfEmpty()
                                          join t3 in _context.BillRequisitionItems.Where(x => x.IsActive) on t1.BillRequisitionItemId equals t3.BillRequisitionItemId into t3_Join
                                          from t3 in t3_Join.DefaultIfEmpty()

                                          select new BillRequisitionDetailModel
                                          {
                                              BillRequisitionDetailId = t1.BillRequisitionDetailId,
                                              BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                              BillRequisitionItemId = t1.BillRequisitionItemId,
                                              ItemName = t3.Name,
                                              Qty = t1.Qty,
                                              Description = t1.Description,
                                              UnitPrice = t1.UnitPrice,
                                              TotalAmount = t1.TotalAmount,
                                          }).FirstOrDefault());
            return v;
        }
        public async Task<BillRequisitionMasterModel> GetBillRequisitionMasterList(int companyId, DateTime? fromDate, DateTime? toDate, int? statusId)
        {
            BillRequisitionMasterModel BillRequisitionMasterModel = new BillRequisitionMasterModel();
            BillRequisitionMasterModel.CompanyFK = companyId;
            BillRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
                                                          && x.CompanyId == companyId)
                                                                        join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                                        join t3 in _context.BillRequisitionTypes on t1.BillRequisitionTypeId equals t3.BillRequisitionTypeId into t3_Join
                                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                                        select new BillRequisitionMasterModel
                                                                        {
                                                                            BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                            BillRequisitionTypeId = t1.BillRequisitionTypeId,
                                                                            BRTypeName = t3.Name,
                                                                            CostCenterId = t1.CostCenterId,
                                                                            CostCenterName = t2.Name,
                                                                            Description = t1.Description,
                                                                            BRDate = t1.BRDate,
                                                                            BillRequisitionNo = t1.BillRequisitionNo,
                                                                            StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                                                            CompanyFK = t1.CompanyId,
                                                                            CreatedDate = t1.CreateDate,
                                                                            CreatedBy = t1.CreatedBy,
                                                                        }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

            if (statusId != -1 && statusId != null)
            {
                BillRequisitionMasterModel.DataList = BillRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)statusId);
            }
            return BillRequisitionMasterModel;
        }

        #endregion


        #region 1.2   BillRequisition received circle

        //public async Task<long> DealerBillRequisitionReceived(BillRequisitionMasterModel BillRequisitionMasterModel)
        //{
        //    long result = -1;
        //    if (BillRequisitionMasterModel.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
        //    if (BillRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

        //    var userName = System.Web.HttpContext.Current.User.Identity.Name;

        //    BillRequisitionMaster BillRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == BillRequisitionMasterModel.BillRequisitionMasterId);
        //    BillRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Received;

        //    BillRequisitionMaster.ModifiedBy = userName;
        //    BillRequisitionMaster.ModifiedDate = DateTime.Now;

        //    List<BillRequisitionDetail> details = _context.BillRequisitionDetails.Where(c => c.BillRequisitionMasterId == BillRequisitionMasterModel.BillRequisitionMasterId && c.IsActive == true).ToList();
        //    if (details?.Count() <= 0) throw new Exception("Sorry! BillRequisition  not found to Receive!");

        //    List<BillRequisitionDetailHistory> history = new List<BillRequisitionDetailHistory>();
        //    foreach (var item in details)
        //    {
        //        history.Add(new BillRequisitionDetailHistory
        //        {
        //            BillRequisitionDetailHistoryId = 0,
        //            BillRequisitionMasterId = item.BillRequisitionMasterId,
        //            BillRequisitionDetailId = item.BillRequisitionDetailId,
        //            BillRequisitionTypeId = item.BillRequisitionTypeId,
        //            ProductId = item.ProductId,
        //            BillRequisitionQty = item.BillRequisitionQty,
        //            UnitPrice = item.UnitPrice,
        //            TotalPrice = item.TotalPrice,
        //            Remarks = item.Remarks,
        //            CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
        //            CreateDate = DateTime.Now,
        //            IsActive = true,
        //        });
        //    }

        //    foreach (var dt in details)
        //    {
        //        var obj = BillRequisitionMasterModel.DetailDataList.FirstOrDefault(c => c.BillRequisitionDetailId == dt.BillRequisitionDetailId);
        //        dt.BillRequisitionQty = obj.BillRequisitionQty;
        //        dt.UnitPrice = obj.UnitPrice;
        //        dt.Remarks = obj.Remarks;
        //        dt.ModifiedBy = userName;
        //        dt.ModifiedDate = DateTime.Now;
        //    }

        //    using (var scope = _context.Database.Beglongransaction())
        //    {
        //        _context.BillRequisitionDetailHistories.AddRange(history);
        //        if (await _context.SaveChangesAsync() > 0)
        //        {
        //            result = BillRequisitionMasterModel.BillRequisitionMasterId;
        //        }
        //        scope.Commit();
        //    }
        //    return result;
        //}
        //public async Task<BillRequisitionMasterModel> GetDealerBillRequisitionMasterReceivedList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        //{
        //    BillRequisitionMasterModel BillRequisitionMasterModel = new BillRequisitionMasterModel();
        //    BillRequisitionMasterModel.CompanyFK = companyId;

        //    BillRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
        //                                                  && x.CompanyId == companyId
        //                                                  && x.BillRequisitionFromId == (long)EnumBillRequisitionFrom.Dealer
        //                                                  && x.ToDeportId != null
        //                                                  && x.FromDealerId != null
        //                                                  && x.OperationDate >= fromDate && x.OperationDate <= toDate
        //                                                  && x.StatusId <= (int)EnumBillRequisitionStatus.Received
        //                                                  && x.StatusId != (int)EnumBillRequisitionStatus.Draft)
        //                                                                join t3 in _context.Vendors on t1.FromDealerId equals t3.VendorId into t3_Join
        //                                                                from t3 in t3_Join.DefaultIfEmpty()
        //                                                                join t4 in _context.Vendors on t1.ToDeportId equals t4.VendorId into t4_Join
        //                                                                from t4 in t4_Join.DefaultIfEmpty()
        //                                                                select new BillRequisitionMasterModel
        //                                                                {
        //                                                                    BillRequisitionMasterId = t1.BillRequisitionMasterId,
        //                                                                    OperationDate = t1.OperationDate,

        //                                                                    DealerName = t3.Name,
        //                                                                    DealerAddress = t3.Address,
        //                                                                    DealerEmail = t3.Email,
        //                                                                    DealerPhone = t3.Phone,

        //                                                                    DeportName = t4.Name,
        //                                                                    DeportEmail = t4.Email,
        //                                                                    DeportPhone = t4.Phone,
        //                                                                    DeportAddress = t4.Address,

        //                                                                    BillRequisitionFromId = (EnumBillRequisitionFrom)t1.BillRequisitionFromId,
        //                                                                    FromCustomerId = t1.FromCustomerId,
        //                                                                    FromDealerId = t1.FromDealerId,
        //                                                                    FromDeportId = t1.FromDeportId,
        //                                                                    ToDealerId = t1.ToDealerId,
        //                                                                    ToDeportId = t1.ToDeportId,
        //                                                                    ToStockInfoId = t1.ToStockInfoId,
        //                                                                    StatusId = (EnumBillRequisitionStatus)t1.StatusId,
        //                                                                    CompanyFK = t1.CompanyId,
        //                                                                    CompanyId = t1.CompanyId,
        //                                                                    CreatedDate = t1.CreateDate,
        //                                                                    CreatedBy = t1.CreatedBy,

        //                                                                }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());
        //    if (vStatus != -1 && vStatus != null)
        //    {
        //        BillRequisitionMasterModel.DataList = BillRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)vStatus);
        //    }
        //    return BillRequisitionMasterModel;
        //}

        //public async Task<BillRequisitionMasterModel> GetCustomerBillRequisitionMasterReceivedList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        //{
        //    BillRequisitionMasterModel BillRequisitionMasterModel = new BillRequisitionMasterModel();
        //    BillRequisitionMasterModel.CompanyFK = companyId;

        //    BillRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
        //                                                  && x.CompanyId == companyId
        //                                                  && x.BillRequisitionFromId == (long)EnumBillRequisitionFrom.Customer
        //                                                  && x.ToDealerId != null
        //                                                  && x.FromCustomerId != null
        //                                                  && x.OperationDate >= fromDate && x.OperationDate <= toDate
        //                                                  && x.StatusId <= (int)EnumBillRequisitionStatus.Received
        //                                                  && x.StatusId != (int)EnumBillRequisitionStatus.Draft)
        //                                                                join t2 in _context.Vendors on t1.FromCustomerId equals t2.VendorId into t2_Join
        //                                                                from t2 in t2_Join.DefaultIfEmpty()
        //                                                                join t3 in _context.Vendors on t1.ToDealerId equals t3.VendorId into t3_Join
        //                                                                from t3 in t3_Join.DefaultIfEmpty()
        //                                                                select new BillRequisitionMasterModel
        //                                                                {
        //                                                                    BillRequisitionMasterId = t1.BillRequisitionMasterId,
        //                                                                    OperationDate = t1.OperationDate,
        //                                                                    DealerName = t3.Name,
        //                                                                    DealerAddress = t3.Address,
        //                                                                    DealerEmail = t3.Email,
        //                                                                    DealerPhone = t3.Phone,
        //                                                                    CustomerName = t2.Name,
        //                                                                    CustomerEmail = t2.Email,
        //                                                                    CustomerPhone = t2.Phone,
        //                                                                    CustomerAddress = t2.Address,
        //                                                                    BillRequisitionFromId = (EnumBillRequisitionFrom)t1.BillRequisitionFromId,
        //                                                                    FromCustomerId = t1.FromCustomerId,
        //                                                                    FromDealerId = t1.FromDealerId,
        //                                                                    FromDeportId = t1.FromDeportId,
        //                                                                    ToDealerId = t1.ToDealerId,
        //                                                                    ToDeportId = t1.ToDeportId,
        //                                                                    ToStockInfoId = t1.ToStockInfoId,
        //                                                                    StatusId = (EnumBillRequisitionStatus)t1.StatusId,
        //                                                                    CompanyFK = t1.CompanyId,
        //                                                                    CompanyId = t1.CompanyId,
        //                                                                    CreatedDate = t1.CreateDate,
        //                                                                    CreatedBy = t1.CreatedBy,

        //                                                                }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());
        //    if (vStatus != -1 && vStatus != null)
        //    {
        //        BillRequisitionMasterModel.DataList = BillRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)vStatus);
        //    }
        //    return BillRequisitionMasterModel;
        //}
        #endregion

    }
}
