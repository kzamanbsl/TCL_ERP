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
    public class BillRequisitionService : IBillRequisitionService
    {
        private readonly ERPEntities _context;
        private readonly ConfigurationService _configurationService;
        public BillRequisitionService(ERPEntities context, ConfigurationService configurationService)
        {
            _context = context;
            _configurationService = configurationService;
        }

        #region Employee
        public async Task<List<Employee>> GetEmployeeList(int companyId)
        {
            var employees = await _context.Employees.Where(c => c.CompanyId == companyId && c.Active).ToListAsync();
            var returnData = employees
                .Select(employee => new Employee
                {
                    Id = employee.Id,
                    EmployeeId = employee.EmployeeId,
                    Name = employee.Name,
                }).ToList();

            return returnData;
        }
        #endregion

        #region Project
        public async Task<List<Accounting_CostCenter>> GetProjectList(int companyId)
        {
            var projects = await _context.Accounting_CostCenter
                .Where(c => c.CompanyId == companyId && c.IsActive)
                .ToListAsync();

            var returnData = projects.Select(project => new Accounting_CostCenter
            {
                CostCenterTypeId = project.CostCenterTypeId,
                CostCenterId = project.CostCenterId,
                Name = project.Name,
                CompanyId = project.CompanyId,
                CreatedBy = project.CreatedBy,
                CreatedDate = project.CreatedDate,
                ModifiedBy = project.ModifiedBy,
                ModifiedDate = project.ModifiedDate,
            }).ToList();

            return returnData;
        }

        public List<Accounting_CostCenter> GetProjectListByTypeId(int id)
        {
            List<Accounting_CostCenter> projects = new List<Accounting_CostCenter>();
            var getProjects = _context.Accounting_CostCenter.Where(c => c.CompanyId == 21 && c.CostCenterTypeId == id && c.IsActive == true).ToList();
            foreach (var project in getProjects)
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

        #endregion

        #region  Project Type

        public async Task<List<Accounting_CostCenterType>> GetCostCenterTypeList(int companyId)
        {
            var projectTypes = await _context.Accounting_CostCenterType
                .Where(c => c.CompanyId == companyId && c.IsActive)
                .ToListAsync();

            var returnData = projectTypes.Select(projectType => new Accounting_CostCenterType
            {
                CostCenterTypeId = projectType.CostCenterTypeId,
                Name = projectType.Name,
                CompanyId = projectType.CompanyId,
                CreatedBy = projectType.CreatedBy,
                CreatedDate = projectType.CreatedDate,
                ModifiedBy = projectType.ModifiedBy,
                ModifiedDate = projectType.ModifiedDate,
            }).ToList();

            return returnData;
        }

        public async Task<bool> Add(CostCenterTypeModel model)
        {
            if (model != null)
            {
                try
                {
                    Accounting_CostCenterType data = new Accounting_CostCenterType()
                    {
                        Name = model.Name,
                        CompanyId = (int)model.CompanyFK,
                        IsActive = true,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CreatedDate = DateTime.Now,
                    };

                    _context.Accounting_CostCenterType.Add(data);
                    var count = await _context.SaveChangesAsync();

                    return count > 0;
                }
                catch (Exception error)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> Edit(CostCenterTypeModel model)
        {
            if (model != null)
            {
                try
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
                catch (Exception error)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> Delete(CostCenterTypeModel model)
        {
            if (model.CostCenterTypeId > 0)
            {
                try
                {
                    var findCostCenterType = await _context.Accounting_CostCenterType.FirstOrDefaultAsync(c => c.CostCenterTypeId == model.CostCenterTypeId);

                    if (findCostCenterType != null)
                    {
                        findCostCenterType.IsActive = false;
                        findCostCenterType.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                        findCostCenterType.ModifiedDate = DateTime.Now.ToString();
                        var count = await _context.SaveChangesAsync();

                        return count > 0;
                    }
                }
                catch (Exception error)
                {
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region Project Manager Assign
        public async Task<List<CostCenterManagerMapModel>> GetCostCenterManagerMapList(int companyId)
        {
            return await _context.CostCenterManagerMaps
                .Where(c => c.CompanyId == companyId && c.IsActive)
                .Select(item => new CostCenterManagerMapModel
                {
                    CostCenterManagerMapId = item.CostCenterManagerMapId,
                    ProjectId = item.CostCenterId,
                    EmployeeRowId = item.ManagerId,
                })
                .ToListAsync();
        }

        public bool Add(CostCenterManagerMapModel model)
        {
            try
            {
                if (model != null)
                {
                    var data = new CostCenterManagerMap
                    {
                        CostCenterId = model.ProjectId,
                        ManagerId = model.EmployeeRowId,
                        CompanyId = (int)model.CompanyFK,
                        IsMapActive = true,
                        IsActive = true,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CreateDate = DateTime.Now
                    };

                    _context.CostCenterManagerMaps.Add(data);
                    return _context.SaveChanges() > 0;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Edit(CostCenterManagerMapModel model)
        {
            try
            {
                if (model != null)
                {
                    var findCostCenterManagerMap = _context.CostCenterManagerMaps.Find(model.CostCenterManagerMapId);

                    if (findCostCenterManagerMap != null)
                    {
                        findCostCenterManagerMap.CostCenterId = model.ProjectId;
                        findCostCenterManagerMap.ManagerId = model.EmployeeRowId;
                        findCostCenterManagerMap.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                        findCostCenterManagerMap.ModifiedDate = DateTime.Now;

                        return _context.SaveChanges() > 0;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(CostCenterManagerMapModel model)
        {
            try
            {
                if (model.CostCenterManagerMapId > 0)
                {
                    var findCostCenterManagerMap = _context.CostCenterManagerMaps.Find(model.CostCenterManagerMapId);

                    if (findCostCenterManagerMap != null)
                    {
                        findCostCenterManagerMap.IsActive = false;
                        findCostCenterManagerMap.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                        findCostCenterManagerMap.ModifiedDate = DateTime.Now;

                        return _context.SaveChanges() > 0;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region  BoQ Division

        public List<BoQDivision> BoQDivisionList()
        {
            List<BoQDivision> BoQDivisionS = new List<BoQDivision>();
            var getBoQDivisionS = _context.BoQDivisions.Where(c => c.CompanyId == 21 && c.IsActive == true).ToList();
            foreach (var item in getBoQDivisionS)
            {
                var data = new BoQDivision()
                {
                    BoQDivisionId = item.BoQDivisionId,
                    Name = item.Name,
                    ProjectId = item.ProjectId
                };
                BoQDivisionS.Add(data);
            }
            return BoQDivisionS;
        }

        public bool Add(BoqDivisionModel model)
        {
            if (model != null)
            {
                try
                {
                    BoQDivision data = new BoQDivision()
                    {
                        Name = model.Name,
                        ProjectId = model.ProjectId,
                        CompanyId = (int)model.CompanyFK,
                        IsActive = true,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CreatedOn = DateTime.Now
                    };
                    _context.BoQDivisions.Add(data);
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

        public bool Edit(BoqDivisionModel model)
        {
            if (model != null)
            {
                try
                {
                    var findBoqDivision = _context.BoQDivisions.FirstOrDefault(c => c.BoQDivisionId == model.ID);

                    findBoqDivision.Name = model.Name;
                    findBoqDivision.ProjectId = model.ProjectId;
                    findBoqDivision.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBoqDivision.ModifiedOn = DateTime.Now;
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

        public bool Delete(BoqDivisionModel model)
        {
            if (model.BoqDivisionId > 0 || model.BoqDivisionId != null)
            {
                try
                {
                    var findBoqDivision = _context.BoQDivisions.FirstOrDefault(c => c.BoQDivisionId == model.BoqDivisionId);

                    findBoqDivision.IsActive = false;
                    findBoqDivision.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBoqDivision.ModifiedOn = DateTime.Now;
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

        #region Bill of Quotation
        public List<BillRequisitionBoqModel> GetBillOfQuotationList()
        {
            var sendData = (from t1 in _context.BillBoQItems.Where(c => c.IsActive)
                            join t2 in _context.BoQDivisions.Where(c => c.IsActive) on t1.BoQDivisionId equals t2.BoQDivisionId
                            join t3 in _context.Units.Where(c => c.IsActive) on t1.BoqUnitId equals t3.UnitId
                            join t4 in _context.Accounting_CostCenter.Where(c => c.IsActive) on t2.ProjectId equals t4.CostCenterId
                            select new BillRequisitionBoqModel()
                            {
                                BoQItemId = t1.BoQItemId,
                                BoQNumber = t1.BoQNumber,
                                Name = t1.Name,
                                BoqQuantity = (decimal)t1.BoqQuantity,
                                BoqUnitId = (int)t1.BoqUnitId,
                                BoqUnitName = t3.Name,
                                BoQDivisionId = (long)t1.BoQDivisionId,
                                BoqDivisionName = t2.Name,
                                ProjectId = t4.CostCenterId,
                                Description = t1.Description,
                            }).ToList();

            return sendData;
        }

        public List<BillBoQItem> GetBillOfQuotationListByProjectId(int id)
        {
            List<BillBoQItem> billBoQItems = new List<BillBoQItem>();
            var getBillBoQItems = _context.BillBoQItems.Where(c => c.IsActive == true).ToList();
            foreach (var item in getBillBoQItems)
            {
                var data = new BillBoQItem()
                {
                    BoQItemId = item.BoQItemId,
                    BoQNumber = item.BoQNumber,
                    Name = item.Name,
                    BoqQuantity = item.BoqQuantity,
                    BoqUnitId = item.BoqUnitId,
                    BoQDivisionId = item.BoQDivisionId,
                    Description = item.Description
                };
                billBoQItems.Add(data);
            }
            return billBoQItems;
        }

        public bool Add(BillRequisitionBoqModel model)
        {
            if (model != null)
            {
                try
                {
                    BillBoQItem data = new BillBoQItem()
                    {
                        BoQNumber = model.BoQNumber,
                        Name = model.Name,
                        BoqQuantity = model.BoqQuantity,
                        BoqUnitId = model.BoqUnitId,
                        BoQDivisionId = model.BoQDivisionId,
                        Description = model.Description,
                        IsActive = true,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CreateDate = DateTime.Now
                    };

                    _context.BillBoQItems.Add(data);

                    if (_context.SaveChanges() > 0)
                    {
                        UserLog logData = new UserLog();
                        logData.ActionType = model.ActionId;
                        logData.EmployeeId = _context.Employees.FirstOrDefault(c => c.EmployeeId == System.Web.HttpContext.Current.User.Identity.Name).Id;
                        logData.EmpUserId = System.Web.HttpContext.Current.User.Identity.Name;
                        logData.CompanyId = (int)model.CompanyFK;
                        logData.ActionTimeStamp = DateTime.Now;
                        logData.Details = $"New BoQItem is added! " +
                            $"BoQNumber: {model.BoQNumber}, " +
                            $"BoQName: {model.Name}, " +
                            $"BoqQuantity: {model.BoqQuantity}, " +
                            $"BoqUnitId: {model.BoqUnitId}, " +
                            $"BoQDivisionId: {model.BoQDivisionId}, " +
                            $"Description: {model.Description} ";

                        _ = _configurationService.UserActionLog(logData);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    UserLog logData = new UserLog();
                    logData.ActionType = model.ActionId;
                    logData.EmployeeId = _context.Employees.FirstOrDefault(c => c.EmployeeId == System.Web.HttpContext.Current.User.Identity.Name).Id;
                    logData.EmpUserId = System.Web.HttpContext.Current.User.Identity.Name;
                    logData.CompanyId = (int)model.CompanyFK;
                    logData.ActionTimeStamp = DateTime.Now;
                    logData.Details = $"Data insert failed!  Error: {ex}";

                    _ = _configurationService.UserActionLog(logData);
                    return false;
                }
            }
            return false;
        }

        public bool Edit(BillRequisitionBoqModel model)
        {
            if (model != null)
            {
                try
                {
                    var findBillRequisitionBoQ = _context.BillBoQItems.FirstOrDefault(c => c.BoQItemId == model.ID);

                    findBillRequisitionBoQ.BoQNumber = model.BoQNumber;
                    findBillRequisitionBoQ.Name = model.Name;
                    findBillRequisitionBoQ.BoqQuantity = model.BoqQuantity;
                    findBillRequisitionBoQ.BoqUnitId = model.BoqUnitId;
                    findBillRequisitionBoQ.BoQDivisionId = model.BoQDivisionId;
                    findBillRequisitionBoQ.Description = model.Description;
                    findBillRequisitionBoQ.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBillRequisitionBoQ.ModifiedDate = DateTime.Now;

                    if (_context.SaveChanges() > 0)
                    {
                        UserLog logData = new UserLog();
                        logData.ActionType = model.ActionId;
                        logData.EmployeeId = _context.Employees.FirstOrDefault(c => c.EmployeeId == System.Web.HttpContext.Current.User.Identity.Name).Id;
                        logData.EmpUserId = System.Web.HttpContext.Current.User.Identity.Name;
                        logData.CompanyId = (int)model.CompanyFK;
                        logData.ActionTimeStamp = DateTime.Now;
                        logData.Details = $"BoQItemId: {model.ID} is updated! " +
                            $"BoQNumber: {model.BoQNumber}, " +
                            $"BoQName: {model.Name}, " +
                            $"BoqQuantity: {model.BoqQuantity}, " +
                            $"BoqUnitId: {model.BoqUnitId}, " +
                            $"BoQDivisionId: {model.BoQDivisionId}, " +
                            $"Description: {model.Description} ";

                        _ = _configurationService.UserActionLog(logData);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    UserLog logData = new UserLog();
                    logData.ActionType = model.ActionId;
                    logData.EmployeeId = _context.Employees.FirstOrDefault(c => c.EmployeeId == System.Web.HttpContext.Current.User.Identity.Name).Id;
                    logData.EmpUserId = System.Web.HttpContext.Current.User.Identity.Name;
                    logData.CompanyId = (int)model.CompanyFK;
                    logData.ActionTimeStamp = DateTime.Now;
                    logData.Details = $"Data update failed! Error: {ex}";

                    _ = _configurationService.UserActionLog(logData);
                    return false;
                }
            }
            return false;
        }

        public bool Delete(BillRequisitionBoqModel model)
        {
            if (model.BoQItemId > 0)
            {
                try
                {
                    var findBillRequisitionBoQ = _context.BillBoQItems.FirstOrDefault(c => c.BoQItemId == model.BoQItemId);

                    findBillRequisitionBoQ.IsActive = false;
                    findBillRequisitionBoQ.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBillRequisitionBoQ.ModifiedDate = DateTime.Now;

                    if (_context.SaveChanges() > 0)
                    {
                        UserLog logData = new UserLog();
                        logData.ActionType = model.ActionId;
                        logData.EmployeeId = _context.Employees.FirstOrDefault(c => c.EmployeeId == System.Web.HttpContext.Current.User.Identity.Name).Id;
                        logData.EmpUserId = System.Web.HttpContext.Current.User.Identity.Name;
                        logData.CompanyId = (int)model.CompanyFK;
                        logData.ActionTimeStamp = DateTime.Now;
                        logData.Details = $"BoqItemId: {model.BoQItemId} is deleted!";

                        _ = _configurationService.UserActionLog(logData);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    UserLog logData = new UserLog();
                    logData.ActionType = model.ActionId;
                    logData.EmployeeId = _context.Employees.FirstOrDefault(c => c.EmployeeId == System.Web.HttpContext.Current.User.Identity.Name).Id;
                    logData.EmpUserId = System.Web.HttpContext.Current.User.Identity.Name;
                    logData.CompanyId = (int)model.CompanyFK;
                    logData.ActionTimeStamp = DateTime.Now;
                    logData.Details = $"Data delete failed! Error: {ex}";

                    _ = _configurationService.UserActionLog(logData);
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region Budget & Estimating

        public async Task<BoQItemProductMap> BoqMaterialBudget(long boqId, long productId)
        {
            var data = await _context.BoQItemProductMaps
                .FirstOrDefaultAsync(c => c.BoQItemId == boqId && c.ProductId == productId);

            if (data == null)
            {
                return null;
            }

            return new BoQItemProductMap
            {
                BoQItemProductMapId = data.BoQItemProductMapId,
                BoQItemId = data.BoQItemId,
                ProductId = data.ProductId,
                EstimatedQty = data.EstimatedQty,
                UnitRate = data.UnitRate,
            };
        }

        //public List<BoQItemProductMap> GetBoQProductMapList()
        //{
        //    List<BoQItemProductMap> boqMapList = new List<BoQItemProductMap>();
        //    var boqMaps = _context.BoQItemProductMaps.Where(c => c.IsActive == true).ToList();
        //    foreach (var item in boqMaps)
        //    {
        //        var data = new BoQItemProductMap()
        //        {
        //            BoQItemProductMapId = item.BoQItemProductMapId,
        //            BoQItemId = item.BoQItemId,
        //            ProductId = item.ProductId,
        //            EstimatedQty = item.EstimatedQty,
        //            UnitRate = item.UnitRate,
        //            EstimatedAmount = item.EstimatedAmount,
        //        };
        //        boqMapList.Add(data);
        //    }
        //    return boqMapList;
        //}

        public List<BillRequisitionItemBoQMapModel> GetBoQProductMapList()
        {
            var sendData = (from t1 in _context.BoQItemProductMaps.Where(c => c.IsActive)
                            join t2 in _context.Products.Where(c => c.IsActive) on t1.ProductId equals t2.ProductId
                            join t3 in _context.BillBoQItems.Where(c => c.IsActive) on t1.BoQItemId equals t3.BoQItemId
                            join t4 in _context.BoQDivisions.Where(c => c.IsActive) on t3.BoQDivisionId equals t4.BoQDivisionId
                            join t5 in _context.Accounting_CostCenter.Where(c => c.IsActive) on t4.ProjectId equals t5.CostCenterId
                            join t6 in _context.ProductSubCategories.DefaultIfEmpty() on t2.ProductSubCategoryId equals t6.ProductSubCategoryId
                            join t7 in _context.ProductCategories.DefaultIfEmpty() on t6.ProductCategoryId equals t7.ProductCategoryId
                            join t8 in _context.Accounting_CostCenterType.DefaultIfEmpty() on t5.CostCenterTypeId equals t8.CostCenterTypeId
                            select new BillRequisitionItemBoQMapModel()
                            {
                                BoQItemProductMapId = t1.BoQItemProductMapId,
                                EstimatedAmount = t1.EstimatedAmount ?? 0M,
                                EstimatedQty = t1.EstimatedQty ?? 0M,
                                UnitRate = t1.UnitRate ?? 0M,
                                MaterialItemId = t1.ProductId,
                                MaterialName = t2.ProductName ?? "N/A",
                                BoQItemId = t3.BoQItemId,
                                BoqName = t3.Name ?? "N/A",
                                BoqNumber = t3.BoQNumber ?? "0",
                                BoQDivisionId = t4.BoQDivisionId,
                                DivisionName = t4.Name ?? "N/A",
                                ProjectId = t5.CostCenterId,
                                ProjectName = t5.Name ?? "N/A",
                                BudgetTypeId = t7.ProductCategoryId,
                                MaterialTypeName = t7.Name ?? "N/A",
                                BudgetSubtypeId = t6.ProductSubCategoryId,
                                MaterialSubtypeName = t6.Name ?? "N/A",
                                ProjectTypeId = t8.CostCenterTypeId,
                                ProjectTypeName = t8.Name ?? "N/A"
                            }).ToList();

            return sendData;
        }

        public bool Add(BillRequisitionItemBoQMapModel model)
        {
            if (model != null)
            {
                var amount = model.EstimatedQty * model.UnitRate;
                try
                {
                    BoQItemProductMap data = new BoQItemProductMap()
                    {
                        BoQItemId = model.BoQItemId,
                        ProductId = model.MaterialItemId,
                        CompanyId = (int)model.CompanyFK,
                        EstimatedQty = model.EstimatedQty,
                        UnitRate = model.UnitRate,
                        EstimatedAmount = amount,
                        IsActive = true,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CreateDate = DateTime.Now
                    };
                    _context.BoQItemProductMaps.Add(data);
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

        public bool Edit(BillRequisitionItemBoQMapModel model)
        {
            if (model != null)
            {
                var amount = model.EstimatedQty * model.UnitRate;
                try
                {
                    var findBoQProductMap = _context.BoQItemProductMaps.FirstOrDefault(c => c.BoQItemProductMapId == model.ID);

                    findBoQProductMap.BoQItemId = model.BoQItemId;
                    findBoQProductMap.ProductId = model.MaterialItemId;
                    findBoQProductMap.EstimatedQty = model.EstimatedQty;
                    findBoQProductMap.UnitRate = model.UnitRate;
                    findBoQProductMap.EstimatedAmount = amount;
                    findBoQProductMap.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBoQProductMap.ModifiedDate = DateTime.Now;
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

        public bool Delete(BillRequisitionItemBoQMapModel model)
        {
            if (model.BoQItemProductMapId > 0)
            {
                try
                {
                    var findBoQProductMap = _context.BoQItemProductMaps.FirstOrDefault(c => c.BoQItemProductMapId == model.BoQItemProductMapId);

                    findBoQProductMap.IsActive = false;
                    findBoQProductMap.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBoQProductMap.ModifiedDate = DateTime.Now;
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

        #region Requisition Type

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
                        CompanyId = (int)model.CompanyFK,
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
                    var findBillRequisitionType = _context.BillRequisitionTypes.FirstOrDefault(c => c.BillRequisitionTypeId == model.ID);

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

        #region  1.1 BillRequisition Master Detail 
        public async Task<BillRequisitionMasterModel> GetBillRequisitionMasterDetail(int companyId = 21, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();


            billRequisitionMasterModel = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive && x.BillRequisitionMasterId == billRequisitionMasterId)

                                                               join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                               from t2 in t2_Join.DefaultIfEmpty()
                                                               join t3 in _context.ProductSubCategories on t1.BillRequisitionTypeId equals t3.ProductSubCategoryId into t3_Join // For overhead data is integrede
                                                               from t3 in t3_Join.DefaultIfEmpty()
                                                               join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                                               from t4 in t4_Join.DefaultIfEmpty()
                                                               join t5 in _context.BillBoQItems on t1.BOQItemId equals t5.BoQItemId into t5_Join
                                                               from t5 in t5_Join.DefaultIfEmpty()
                                                               join t6 in _context.BoQDivisions on t1.BoQDivisionId equals t6.BoQDivisionId into t6_Join
                                                               from t6 in t6_Join.DefaultIfEmpty()
                                                               join t7 in _context.ProductCategories on t3.ProductCategoryId equals t7.ProductCategoryId into t7_Join
                                                               from t7 in t7_Join.DefaultIfEmpty()

                                                               select new BillRequisitionMasterModel
                                                               {
                                                                   BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                   BillRequisitionTypeId = t7.ProductCategoryId,
                                                                   BRTypeName = t7.Name,
                                                                   BillRequisitionSubTypeId = t1.BillRequisitionTypeId,
                                                                   BRSubtypeName = t3.Name,
                                                                   ProjectTypeId = t1.ProjectTypeId,
                                                                   ProjectTypeName = t4.Name,
                                                                   BoQDivisionId = (t1.BoQDivisionId == null || t1.BoQDivisionId == 0) ? 0 : (int)t1.BoQDivisionId,
                                                                   BoQDivisionName = t6.Name,
                                                                   BOQItemId = t1.BOQItemId,
                                                                   BOQItemName = t5.Name,
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
                                                                          join t3 in _context.Products.Where(x => x.IsActive) on t1.ProductId equals t3.ProductId into t3_Join
                                                                          from t3 in t3_Join.DefaultIfEmpty()
                                                                          join t4 in _context.Units.Where(x => x.IsActive) on t1.UnitId equals t4.UnitId into t4_Join
                                                                          from t4 in t4_Join.DefaultIfEmpty()

                                                                          select new BillRequisitionDetailModel
                                                                          {
                                                                              BillRequisitionDetailId = t1.BillRequisitionDetailId,
                                                                              BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                              ProductId = t1.ProductId,
                                                                              ProductName = t3.ProductName,
                                                                              UnitId = t1.UnitId,
                                                                              UnitName = t4.Name,
                                                                              UnitRate = t1.UnitRate,
                                                                              TotalPrice = t1.UnitRate * t1.DemandQty,
                                                                              DemandQty = t1.DemandQty,
                                                                              ReceivedSoFar = t1.ReceivedSoFar,
                                                                              RemainingQty = t1.RemainingQty,
                                                                              EstimatedQty = t1.EstimatedQty,
                                                                              Floor = t1.Floor,
                                                                              Ward = t1.Ward,
                                                                              DPP = t1.DPP,
                                                                              Chainage = t1.Chainage,
                                                                              Remarks = t1.Remarks,
                                                                          }).OrderByDescending(x => x.BillRequisitionDetailId).AsEnumerable());

            billRequisitionMasterModel.TotalAmount = billRequisitionMasterModel.DetailList.Select(x => x.TotalPrice).Sum();

            return billRequisitionMasterModel;
        }

        public async Task<long> BillRequisitionMasterAdd(BillRequisitionMasterModel model)
        {
            long result = -1;

            if (model.StatusId == 0)
            {
                model.StatusId = EnumBillRequisitionStatus.Draft;
            }

            try
            {
                if (model.BoQDivisionId > 0 && model.BOQItemId > 0)
                {
                    BillRequisitionMaster billRequisitionMaster = new BillRequisitionMaster
                    {
                        BillRequisitionMasterId = model.BillRequisitionMasterId,
                        BRDate = model.BRDate,
                        BillRequisitionTypeId = model.BillRequisitionSubTypeId, // In requisition type column, insert requisition subtype id
                        BoQDivisionId = model.BoQDivisionId,
                        BOQItemId = model.BOQItemId,
                        ProjectTypeId = model.ProjectTypeId,
                        CostCenterId = model.CostCenterId,
                        Description = model.Description,
                        BillRequisitionNo = GetUniqueRequisitionNo(),
                        StatusId = (int)model.StatusId,
                        CompanyId = (int)model.CompanyFK,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
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
                else
                {
                    BillRequisitionMaster billRequisitionMaster = new BillRequisitionMaster
                    {
                        BillRequisitionMasterId = model.BillRequisitionMasterId,
                        BRDate = model.BRDate,
                        BillRequisitionTypeId = model.BillRequisitionSubTypeId, // In requisition type column, insert requisition subtype id
                        ProjectTypeId = model.ProjectTypeId,
                        CostCenterId = model.CostCenterId,
                        Description = model.Description,
                        BillRequisitionNo = GetUniqueRequisitionNo(),
                        StatusId = (int)model.StatusId,
                        CompanyId = (int)model.CompanyFK,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
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
            }
            catch (Exception error)
            {
                return result;
            }

            // Generate unique bill requisition number
            string GetUniqueRequisitionNo()
            {
                #region Generate Unique Requisition Number With Last Id

                int getLastRowId = _context.BillRequisitionMasters.Where(c => c.BRDate == DateTime.Today).Count();

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
            try
            {
                BillRequisitionDetail damageDetail = new BillRequisitionDetail
                {
                    BillRequisitionMasterId = model.BillRequisitionMasterId,
                    BillRequisitionDetailId = model.DetailModel.BillRequisitionDetailId,
                    ProductId = model.DetailModel.ProductId,
                    UnitRate = model.DetailModel.UnitRate,
                    DemandQty = model.DetailModel.DemandQty,
                    UnitId = model.DetailModel.UnitId,
                    ReceivedSoFar = model.DetailModel.ReceivedSoFar,
                    RemainingQty = model.DetailModel.RemainingQty,
                    EstimatedQty = model.DetailModel.EstimatedQty,
                    TotalPrice = model.DetailModel.TotalPrice,
                    Floor = model.DetailModel.Floor,
                    Ward = model.DetailModel.Ward,
                    DPP = model.DetailModel.DPP,
                    Chainage = model.DetailModel.Chainage,
                    Remarks = model.DetailModel.Remarks,
                    CompanyId = (int)model.CompanyFK,
                    CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                };
                _context.BillRequisitionDetails.Add(damageDetail);

                if (await _context.SaveChangesAsync() > 0)
                {
                    result = damageDetail.BillRequisitionMasterId;
                }
                return result;
            }
            catch (Exception error)
            {
                return result;
            }

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
            demageDetail.ProductId = model.DetailModel.ProductId;
            demageDetail.UnitRate = model.DetailModel.UnitRate;
            demageDetail.DemandQty = model.DetailModel.DemandQty;
            demageDetail.EstimatedQty = model.DetailModel.EstimatedQty;
            demageDetail.RemainingQty = model.DetailModel.RemainingQty;
            demageDetail.ReceivedSoFar = model.DetailModel.ReceivedSoFar;
            demageDetail.TotalPrice = model.DetailModel.TotalPrice;
            demageDetail.Ward = model.DetailModel.Ward;
            demageDetail.Floor = model.DetailModel.Floor;
            demageDetail.DPP = model.DetailModel.DPP;
            demageDetail.Chainage = model.DetailModel.Chainage;
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


            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Submitted;
            billRequisitionMaster.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();

                BillRequisitionMasterModel model = new BillRequisitionMasterModel();
                List<BillRequisitionApproval> billRequisitionApprovalList = new List<BillRequisitionApproval>();
                int priority = 0;
                var empId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
                foreach (var item in model.EnumBRSignatoryList)
                {
                    BillRequisitionApproval billRequisitionApproval = new BillRequisitionApproval();
                    billRequisitionApproval.BillRequisitionMasterId = billRequisitionMaster.BillRequisitionMasterId;
                    billRequisitionApproval.CompanyId = billRequisitionMaster.CompanyId;

                    billRequisitionApproval.SignatoryId = Convert.ToInt16(item.Value);
     
                    priority = priority + 1;
                    billRequisitionApproval.PriorityNo = priority;
                    billRequisitionApproval.IsActive = true;
                    billRequisitionApproval.IsSupremeApproved = false;

                    billRequisitionApproval.CreateDate = DateTime.Now;
                    billRequisitionApproval.CreatedBy = billRequisitionMaster.CreatedBy;

                    if (billRequisitionApproval.SignatoryId == 1)
                    {
                        billRequisitionApproval.EmployeeId = empId;
                        billRequisitionApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
                        billRequisitionApproval.ModifiedDate = DateTime.Now;
                        billRequisitionApproval.Reasons = "Requisition Created.";
                    }
                    else
                    {
                        billRequisitionApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Pending;
                    }

                    billRequisitionApprovalList.Add(billRequisitionApproval);
                }
                _context.BillRequisitionApprovals.AddRange(billRequisitionApprovalList);
                _context.SaveChanges();

                result = billRequisitionMaster.BillRequisitionMasterId;
                scope.Commit();
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
            billRequisitionMaster.ProjectTypeId = model.ProjectTypeId;
            billRequisitionMaster.BOQItemId = model.BOQItemId;
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
                                          join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                          from t4 in t4_Join.DefaultIfEmpty()
                                          join t5 in _context.BillBoQItems on t1.BOQItemId equals t5.BoQItemId into t5_Join
                                          from t5 in t5_Join.DefaultIfEmpty()

                                          select new BillRequisitionMasterModel
                                          {
                                              BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                              BillRequisitionTypeId = t1.BillRequisitionTypeId,
                                              BRTypeName = t3.Name,
                                              CostCenterId = t1.CostCenterId,
                                              ProjectTypeId = t1.ProjectTypeId,
                                              ProjectTypeName = t4.Name,
                                              BOQItemId = t1.BOQItemId,
                                              BOQItemName = t5.Name,
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
                                          join t3 in _context.Products.Where(x => x.IsActive) on t1.ProductId equals t3.ProductId into t3_Join
                                          from t3 in t3_Join.DefaultIfEmpty()

                                          select new BillRequisitionDetailModel
                                          {
                                              BillRequisitionDetailId = t1.BillRequisitionDetailId,
                                              BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                              ProductId = t1.ProductId,
                                              ProductName = t3.ProductName,
                                              UnitId = t1.UnitId,
                                              DemandQty = t1.DemandQty,
                                              EstimatedQty = t1.EstimatedQty,
                                              RemainingQty = t1.RemainingQty,
                                              ReceivedSoFar = t1.ReceivedSoFar,
                                              UnitRate = t1.UnitRate,
                                              TotalPrice = t1.TotalPrice,
                                              Ward = t1.Ward,
                                              Floor = t1.Floor,
                                              DPP = t1.DPP,
                                              Chainage = t1.Chainage,
                                              Remarks = t1.Remarks,
                                          }).FirstOrDefault());
            return v;
        }
        //public async Task<BillRequisitionMasterModel> GetBillRequisitionMasterList(int companyId, DateTime? fromDate, DateTime? toDate, int? statusId)
        //{
        //    BillRequisitionMasterModel BillRequisitionMasterModel = new BillRequisitionMasterModel();
        //    BillRequisitionMasterModel.CompanyFK = companyId;
        //    BillRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
        //                                                  && x.CompanyId == companyId)
        //                                                                join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
        //                                                                from t2 in t2_Join.DefaultIfEmpty()
        //                                                                join t3 in _context.BillRequisitionTypes on t1.BillRequisitionTypeId equals t3.BillRequisitionTypeId into t3_Join
        //                                                                from t3 in t3_Join.DefaultIfEmpty()
        //                                                                join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
        //                                                                from t4 in t4_Join.DefaultIfEmpty()
        //                                                                select new BillRequisitionMasterModel
        //                                                                {
        //                                                                    BillRequisitionMasterId = t1.BillRequisitionMasterId,
        //                                                                    BillRequisitionTypeId = t1.BillRequisitionTypeId,
        //                                                                    BOQItemId = t1.BOQItemId,
        //                                                                    ProjectTypeId = t1.ProjectTypeId,
        //                                                                    ProjectTypeName = t4.Name,
        //                                                                    BRTypeName = t3.Name,
        //                                                                    CostCenterId = t1.CostCenterId,
        //                                                                    CostCenterName = t2.Name,
        //                                                                    Description = t1.Description,
        //                                                                    BRDate = t1.BRDate,
        //                                                                    BillRequisitionNo = t1.BillRequisitionNo,
        //                                                                    StatusId = (EnumBillRequisitionStatus)t1.StatusId,
        //                                                                    CompanyFK = t1.CompanyId,
        //                                                                    CreatedDate = t1.CreateDate,
        //                                                                    CreatedBy = t1.CreatedBy,
        //                                                                }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

        //    if (statusId != -1 && statusId != null)
        //    {
        //        BillRequisitionMasterModel.DataList = BillRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)statusId);
        //    }
        //    var masterIds = BillRequisitionMasterModel.DataList.Select(x => x.BillRequisitionMasterId);

        //    return BillRequisitionMasterModel;
        //}
        public async Task<BillRequisitionMasterModel> GetBillRequisitionMasterList(int companyId, DateTime? fromDate, DateTime? toDate, int? statusId)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            billRequisitionMasterModel.CompanyFK = companyId;
            var dataQuery = (from t1 in _context.BillRequisitionMasters
                             where t1.IsActive && t1.CompanyId == companyId
                             && t1.CreatedBy == user
                             join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                             from t2 in t2_Join.DefaultIfEmpty()
                             join t3 in _context.BillRequisitionTypes on t1.BillRequisitionTypeId equals t3.BillRequisitionTypeId into t3_Join
                             from t3 in t3_Join.DefaultIfEmpty()
                             join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                             from t4 in t4_Join.DefaultIfEmpty()
                             select new BillRequisitionMasterModel
                             {
                                 BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                 BillRequisitionTypeId = t1.BillRequisitionTypeId,
                                 BOQItemId = t1.BOQItemId,
                                 ProjectTypeId = t1.ProjectTypeId,
                                 ProjectTypeName = t4.Name,
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
                                 ApprovalModelList = (from t5 in _context.BillRequisitionApprovals.Where(x => x.IsActive && x.BillRequisitionMasterId == t1.BillRequisitionMasterId)
                                                      join t6 in _context.BillRequisitionMasters.Where(x => x.IsActive) on t1.BillRequisitionMasterId equals t6.BillRequisitionMasterId into t6_Join
                                                      from t6 in t6_Join.DefaultIfEmpty()
                                                      select new BillRequisitionApprovalModel
                                                      {
                                                          BRApprovalId = t5.BRApprovalId,
                                                          BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                          SignatoryId = t5.SignatoryId,
                                                          AprrovalStatusId = t5.AprrovalStatusId,
                                                          IsSupremeApproved = t5.IsSupremeApproved,
                                                      }).OrderBy(x => x.BRApprovalId).ToList(),


                             }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable();

            if (statusId != -1 && statusId != null)
            {
                dataQuery = dataQuery.Where(q => q.StatusId == (EnumBillRequisitionStatus)statusId);
            }

            billRequisitionMasterModel.DataList = await Task.Run(() => dataQuery.ToList());

            var masterIds = billRequisitionMasterModel.DataList.Select(x => x.BillRequisitionMasterId);

            var matchingDetails = await _context.BillRequisitionDetails
                                        .Where(detail => masterIds.Contains(detail.BillRequisitionMasterId))
                                        .ToListAsync();

            foreach (var master in billRequisitionMasterModel.DataList)
            {
                var detailsForMaster = matchingDetails.Where(detail => detail.BillRequisitionMasterId == master.BillRequisitionMasterId);
                decimal? total = detailsForMaster.Sum(detail => detail.UnitRate * detail.DemandQty);
                master.TotalAmount = total;
            }

            return billRequisitionMasterModel;
        }
        public async Task<BillRequisitionMasterModel> GetBillRequisitionMasterCommonList(int companyId, DateTime? fromDate, DateTime? toDate, int? statusId)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel.CompanyFK = companyId;
            var dataQuery = (from t1 in _context.BillRequisitionMasters
                             where t1.IsActive && t1.CompanyId == companyId
                             join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                             from t2 in t2_Join.DefaultIfEmpty()
                             join t3 in _context.BillRequisitionTypes on t1.BillRequisitionTypeId equals t3.BillRequisitionTypeId into t3_Join
                             from t3 in t3_Join.DefaultIfEmpty()
                             join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                             from t4 in t4_Join.DefaultIfEmpty()
                             select new BillRequisitionMasterModel
                             {
                                 BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                 BillRequisitionTypeId = t1.BillRequisitionTypeId,
                                 BOQItemId = t1.BOQItemId,
                                 ProjectTypeId = t1.ProjectTypeId,
                                 ProjectTypeName = t4.Name,
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
                                 ApprovalModelList = (from t5 in _context.BillRequisitionApprovals.Where(x => x.IsActive && x.BillRequisitionMasterId == t1.BillRequisitionMasterId)
                                                      join t6 in _context.BillRequisitionMasters.Where(x => x.IsActive) on t1.BillRequisitionMasterId equals t6.BillRequisitionMasterId into t6_Join
                                                      from t6 in t6_Join.DefaultIfEmpty()
                                                      select new BillRequisitionApprovalModel
                                                      {
                                                          BRApprovalId = t5.BRApprovalId,
                                                          BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                          SignatoryId = t5.SignatoryId,
                                                          AprrovalStatusId = t5.AprrovalStatusId,
                                                          IsSupremeApproved = t5.IsSupremeApproved,
                                                      }).OrderBy(x => x.BRApprovalId).ToList(),


                             }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable();

            if (statusId != -1 && statusId != null)
            {
                dataQuery = dataQuery.Where(q => q.StatusId == (EnumBillRequisitionStatus)statusId);
            }

            billRequisitionMasterModel.DataList = await Task.Run(() => dataQuery.ToList());

            var masterIds = billRequisitionMasterModel.DataList.Select(x => x.BillRequisitionMasterId);

            var matchingDetails = await _context.BillRequisitionDetails
                                        .Where(detail => masterIds.Contains(detail.BillRequisitionMasterId))
                                        .ToListAsync();

            foreach (var master in billRequisitionMasterModel.DataList)
            {
                var detailsForMaster = matchingDetails.Where(detail => detail.BillRequisitionMasterId == master.BillRequisitionMasterId);
                decimal? total = detailsForMaster.Sum(detail => detail.UnitRate * detail.DemandQty);
                master.TotalAmount = total;
            }

            return billRequisitionMasterModel;
        }

        #endregion

        #region 1.2 BillRequisition Approve circle
        public async Task<BillRequisitionMasterModel> GetBillRequisitionMasterDetailWithApproval(int companyId = 21, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();


            billRequisitionMasterModel = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive && x.BillRequisitionMasterId == billRequisitionMasterId)

                                                               join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                               from t2 in t2_Join.DefaultIfEmpty()
                                                               join t3 in _context.ProductSubCategories on t1.BillRequisitionTypeId equals t3.ProductSubCategoryId into t3_Join
                                                               from t3 in t3_Join.DefaultIfEmpty()
                                                               join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                                               from t4 in t4_Join.DefaultIfEmpty()
                                                               join t5 in _context.BillBoQItems on t1.BOQItemId equals t5.BoQItemId into t5_Join
                                                               from t5 in t5_Join.DefaultIfEmpty()
                                                               join t6 in _context.BoQDivisions on t5.BoQDivisionId equals t6.BoQDivisionId into t6_Join
                                                               from t6 in t6_Join.DefaultIfEmpty()
                                                               join t7 in _context.ProductCategories on t3.ProductCategoryId equals t7.ProductCategoryId into t7_Join
                                                               from t7 in t7_Join.DefaultIfEmpty()
                                                               join t8 in _context.Employees on t1.CreatedBy equals t8.EmployeeId into t8_Join
                                                               from t8 in t8_Join.DefaultIfEmpty()

                                                               select new BillRequisitionMasterModel
                                                               {
                                                                   BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                   BillRequisitionTypeId = t7.ProductCategoryId,
                                                                   BRTypeName = t7.Name,
                                                                   BillRequisitionSubTypeId = t1.BillRequisitionTypeId,
                                                                   BRSubtypeName = t3.Name,
                                                                   ProjectTypeId = t1.ProjectTypeId,
                                                                   ProjectTypeName = t4.Name,
                                                                   BOQItemId = t1.BOQItemId ?? 0,
                                                                   BOQItemName = t5.Name ?? "-",
                                                                   BoQDivisionId = t5.BoQDivisionId ?? 0,
                                                                   BoQDivisionName = t6.Name ?? "-",
                                                                   CostCenterId = t1.CostCenterId,
                                                                   CostCenterName = t2.Name,
                                                                   Description = t1.Description,
                                                                   BRDate = t1.BRDate,
                                                                   BillRequisitionNo = t1.BillRequisitionNo,
                                                                   StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                                                   CompanyFK = t1.CompanyId,
                                                                   CreatedDate = t1.CreateDate,
                                                                   CreatedBy = t1.CreatedBy,
                                                                   EmployeeName = t1.CreatedBy + " - " + t8.Name,

                                                               }).FirstOrDefault());

            billRequisitionMasterModel.DetailList = await Task.Run(() => (from t1 in _context.BillRequisitionDetails.Where(x => x.IsActive && x.BillRequisitionMasterId == billRequisitionMasterId)
                                                                          join t2 in _context.BillRequisitionMasters.Where(x => x.IsActive) on t1.BillRequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                                          from t2 in t2_Join.DefaultIfEmpty()
                                                                          join t3 in _context.Products.Where(x => x.IsActive) on t1.ProductId equals t3.ProductId into t3_Join
                                                                          from t3 in t3_Join.DefaultIfEmpty()
                                                                          join t4 in _context.Units.Where(x => x.IsActive) on t1.UnitId equals t4.UnitId into t4_Join
                                                                          from t4 in t4_Join.DefaultIfEmpty()

                                                                          select new BillRequisitionDetailModel
                                                                          {
                                                                              BillRequisitionDetailId = t1.BillRequisitionDetailId,
                                                                              BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                              ProductId = t1.ProductId,
                                                                              ProductName = t3.ProductName,
                                                                              UnitId = t1.UnitId,
                                                                              UnitName = t4.Name,
                                                                              UnitRate = t1.UnitRate,
                                                                              TotalPrice = t1.UnitRate * t1.DemandQty,
                                                                              DemandQty = t1.DemandQty,
                                                                              ReceivedSoFar = t1.ReceivedSoFar,
                                                                              RemainingQty = t1.RemainingQty,
                                                                              EstimatedQty = t1.EstimatedQty,
                                                                              Remarks = t1.Remarks,
                                                                          }).OrderByDescending(x => x.BillRequisitionDetailId).AsEnumerable());

            billRequisitionMasterModel.TotalAmount = billRequisitionMasterModel.DetailList.Select(x => x.TotalPrice).Sum();

            billRequisitionMasterModel.ApprovalModelList = await Task.Run(() => (from t1 in _context.BillRequisitionApprovals.Where(x => x.IsActive && x.BillRequisitionMasterId == billRequisitionMasterId)
                                                                                 join t2 in _context.BillRequisitionMasters.Where(x => x.IsActive) on t1.BillRequisitionMasterId equals t2.BillRequisitionMasterId into t2_Join
                                                                                 from t2 in t2_Join.DefaultIfEmpty()
                                                                                 join t3 in _context.Employees on t1.EmployeeId equals t3.Id into t3_Join
                                                                                 from t3 in t3_Join.DefaultIfEmpty()
                                                                                 select new BillRequisitionApprovalModel
                                                                                 {
                                                                                     BRApprovalId = t1.BRApprovalId,
                                                                                     BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                                     SignatoryId = t1.SignatoryId,
                                                                                     AprrovalStatusId = t1.AprrovalStatusId,
                                                                                     IsSupremeApproved = t1.IsSupremeApproved,
                                                                                     EmployeeId = t1.EmployeeId,
                                                                                     EmployeeName = t3.EmployeeId,
                                                                                     ApprobalRemarks = t1.Reasons ?? "N/A",
                                                                                     ModifiedDate = t1.ModifiedDate
                                                                                 }).OrderBy(x => x.BRApprovalId).ToList());
            return billRequisitionMasterModel;
        }

        public async Task<long> PMBillRequisitionApproved(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            long result = -1;
            if (billRequisitionMasterModel.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Pending;

            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;
            List<BillRequisitionDetail> details = _context.BillRequisitionDetails.Where(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && c.IsActive == true).ToList();
            if (details?.Count() <= 0) throw new Exception("Sorry! Damage  not found to Receive!");


            List<BillReqApprovalHistory> history = new List<BillReqApprovalHistory>();
            foreach (var item in details)
            {
                history.Add(new BillReqApprovalHistory
                {
                    BillReqApprovalHistoryId = 0,
                    BillRequisitionDetailId = item.BillRequisitionDetailId,
                    BillRequisitionMasterId = item.BillRequisitionMasterId,
                    DemandQty = item.DemandQty,
                    RemainingQty = item.RemainingQty,
                    ReceivedSoFar = item.ReceivedSoFar,
                    UnitRate = item.UnitRate,
                    TotalPrice = item.TotalPrice,
                    CompanyId = item.CompanyId,
                    EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]),
                    CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                    CreateDate = DateTime.Now,
                });
            }

            foreach (var dt in details)
            {
                var obj = billRequisitionMasterModel.DetailDataList.FirstOrDefault(c => c.BillRequisitionDetailId == dt.BillRequisitionDetailId);

                dt.DemandQty = obj.DemandQty;
                dt.TotalPrice = obj.DemandQty * obj.UnitRate;
                dt.UnitRate = obj.UnitRate;
                //dt.Remarks = obj.Remarks;
                dt.ModifiedBy = userName;
                dt.ModifiedDate = DateTime.Now;
            }

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.PM);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = billRequisitionMasterModel.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                _context.BillReqApprovalHistories.AddRange(history);
                await _context.SaveChangesAsync();

                result = billRequisitionMasterModel.BillRequisitionMasterId;
                scope.Commit();
            }
            return result;
        }
        public async Task<int> PMBillRequisitionRejected(BillRequisitionMasterModel model)
        {
            int result = -1;
            if (model.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            //if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == model.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Rejected;
            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == model.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.PM);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = model.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();
                scope.Commit();
                result = billRequisitionMaster.CompanyId;
            }
            return result;
        }
        public async Task<BillRequisitionMasterModel> GetPMBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            var EmpId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            if (EmpId > 0)
            {
                billRequisitionMasterModel.CompanyFK = companyId;
                billRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
                                                             && x.CompanyId == companyId
                                                             && x.StatusId >= (int)EnumBillRequisitionStatus.Submitted)
                                                                            join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                                            from t2 in t2_Join.DefaultIfEmpty()
                                                                            join t3 in _context.ProductSubCategories on t1.BillRequisitionTypeId equals t3.ProductSubCategoryId into t3_Join
                                                                            from t3 in t3_Join.DefaultIfEmpty()
                                                                            join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                                                            from t4 in t4_Join.DefaultIfEmpty()
                                                                            join t5 in _context.CostCenterManagerMaps on t2.CostCenterId equals t5.CostCenterId into t5_Join
                                                                            from t5 in t5_Join.DefaultIfEmpty()
                                                                            join t6 in _context.Employees on t5.ManagerId equals t6.Id into t6_Join
                                                                            from t6 in t6_Join.DefaultIfEmpty()
                                                                            join t03 in _context.ProductCategories on t3.ProductCategoryId equals t03.ProductCategoryId into t03_Join
                                                                            from t03 in t03_Join.DefaultIfEmpty()
                                                                            select new BillRequisitionMasterModel
                                                                            {
                                                                                BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                                BillRequisitionTypeId = t03.ProductCategoryId,
                                                                                BRTypeName = t03.Name,
                                                                                BillRequisitionSubTypeId = t1.BillRequisitionTypeId,
                                                                                BRSubtypeName = t3.Name,
                                                                                BOQItemId = t1.BOQItemId,
                                                                                ProjectTypeId = t1.ProjectTypeId,
                                                                                ProjectTypeName = t4.Name,
                                                                                CostCenterId = t1.CostCenterId,
                                                                                CostCenterName = t2.Name,
                                                                                Description = t1.Description,
                                                                                BRDate = t1.BRDate,
                                                                                BillRequisitionNo = t1.BillRequisitionNo,
                                                                                StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                                                                CompanyFK = t1.CompanyId,
                                                                                CreatedDate = t1.CreateDate,
                                                                                CreatedBy = t1.CreatedBy,
                                                                                EmployeeId = t6.Id,
                                                                                EmployeeStringId = t6.EmployeeId,
                                                                                ApprovalModelList = (from t7 in _context.BillRequisitionApprovals.Where(b => b.BillRequisitionMasterId == t1.BillRequisitionMasterId && b.IsActive)
                                                                                                     join t8 in _context.BillRequisitionMasters on t7.BillRequisitionMasterId equals t8.BillRequisitionMasterId
                                                                                                     select new BillRequisitionApprovalModel
                                                                                                     {
                                                                                                         BRApprovalId = t7.BRApprovalId,
                                                                                                         BillRequisitionMasterId = t8.BillRequisitionMasterId,
                                                                                                         SignatoryId = t7.SignatoryId,
                                                                                                         IsSupremeApproved = t7.IsSupremeApproved,
                                                                                                         AprrovalStatusId = t7.AprrovalStatusId,
                                                                                                     }).OrderBy(m => m.BRApprovalId).ToList(),


                                                                            }).Where(x => x.EmployeeId == EmpId).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

                
                if (vStatus != -1 && vStatus != null)
                {
                    billRequisitionMasterModel.DataList = billRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)vStatus);
                }

                return billRequisitionMasterModel;
            }
            else
            {
                return billRequisitionMasterModel;
            }
            
        }

        #endregion

        #region 1.3 QS BillRequisition Approve circle

        public async Task<long> QSBillRequisitionApproved(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            long result = -1;
            if (billRequisitionMasterModel.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Pending;

            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;
            List<BillRequisitionDetail> details = _context.BillRequisitionDetails.Where(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && c.IsActive == true).ToList();
            if (details?.Count() <= 0) throw new Exception("Sorry! Damage  not found to Receive!");

            List<BillReqApprovalHistory> history = new List<BillReqApprovalHistory>();
            foreach (var item in details)
            {
                history.Add(new BillReqApprovalHistory
                {
                    BillReqApprovalHistoryId = 0,
                    BillRequisitionDetailId = item.BillRequisitionDetailId,
                    BillRequisitionMasterId = item.BillRequisitionMasterId,
                    DemandQty = item.DemandQty,
                    RemainingQty = item.RemainingQty,
                    ReceivedSoFar = item.ReceivedSoFar,
                    UnitRate = item.UnitRate,
                    TotalPrice = item.TotalPrice,
                    CompanyId = item.CompanyId,
                    EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]),
                    CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                    CreateDate = DateTime.Now,
                });
            }

            foreach (var dt in details)
            {
                var obj = billRequisitionMasterModel.DetailDataList.FirstOrDefault(c => c.BillRequisitionDetailId == dt.BillRequisitionDetailId);

                dt.DemandQty = obj.DemandQty;
                dt.TotalPrice = obj.DemandQty * obj.UnitRate;
                dt.UnitRate = obj.UnitRate;
                //dt.Remarks = obj.Remarks;
                dt.ModifiedBy = userName;
                dt.ModifiedDate = DateTime.Now;
            }

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.QS);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = billRequisitionMasterModel.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                _context.BillReqApprovalHistories.AddRange(history);
                await _context.SaveChangesAsync();

                result = billRequisitionMasterModel.BillRequisitionMasterId;
                scope.Commit();
            }
            return result;
        }
        public async Task<int> QSBillRequisitionRejected(BillRequisitionMasterModel model)
        {
            int result = -1;
            if (model.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            //if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == model.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Rejected;
            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == model.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.QS);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = model.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();
                scope.Commit();
                result = billRequisitionMaster.CompanyId;
            }
            return result;
        }
        public async Task<BillRequisitionMasterModel> GetQSBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            //var EmpId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);

            billRequisitionMasterModel.CompanyFK = companyId;
            billRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
                                                         && x.CompanyId == companyId
                                                         && x.BillRequisitionTypeId != (int)EnumBillRequisitionSubType.It
                                                         && x.StatusId >= (int)EnumBillRequisitionStatus.Submitted)
                                                                        join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                                        join t3 in _context.ProductSubCategories on t1.BillRequisitionTypeId equals t3.ProductSubCategoryId into t3_Join
                                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                                        join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                                        join t03 in _context.ProductCategories on t3.ProductCategoryId equals t03.ProductCategoryId into t03_Join
                                                                        from t03 in t03_Join.DefaultIfEmpty()
                                                                        select new BillRequisitionMasterModel
                                                                        {
                                                                            BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                            BillRequisitionTypeId = t03.ProductCategoryId,
                                                                            BRTypeName = t03.Name,
                                                                            BillRequisitionSubTypeId = t1.BillRequisitionTypeId,
                                                                            BRSubtypeName = t3.Name,
                                                                            BOQItemId = t1.BOQItemId,
                                                                            ProjectTypeId = t1.ProjectTypeId,
                                                                            ProjectTypeName = t4.Name,
                                                                            CostCenterId = t1.CostCenterId,
                                                                            CostCenterName = t2.Name,
                                                                            Description = t1.Description,
                                                                            BRDate = t1.BRDate,
                                                                            BillRequisitionNo = t1.BillRequisitionNo,
                                                                            StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                                                            CompanyFK = t1.CompanyId,
                                                                            CreatedDate = t1.CreateDate,
                                                                            CreatedBy = t1.CreatedBy,
                                                                            //EmployeeId = t6.Id,
                                                                            //EmployeeStringId = t6.EmployeeId,
                                                                            ApprovalModelList = (from t7 in _context.BillRequisitionApprovals.Where(b => b.BillRequisitionMasterId == t1.BillRequisitionMasterId && b.IsActive)
                                                                                                 join t8 in _context.BillRequisitionMasters on t7.BillRequisitionMasterId equals t8.BillRequisitionMasterId
                                                                                                 select new BillRequisitionApprovalModel
                                                                                                 {
                                                                                                     BRApprovalId = t7.BRApprovalId,
                                                                                                     BillRequisitionMasterId = t8.BillRequisitionMasterId,
                                                                                                     SignatoryId = t7.SignatoryId,
                                                                                                     IsSupremeApproved = t7.IsSupremeApproved,
                                                                                                     AprrovalStatusId = t7.AprrovalStatusId,
                                                                                                 }).OrderBy(m => m.BRApprovalId).ToList(),


                                                                        }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

            var filteredMasterList = billRequisitionMasterModel.DataList.Where(
            q => q.ApprovalModelList.FirstOrDefault(x => x.SignatoryId == (int)EnumBRequisitionSignatory.PM)?.AprrovalStatusId == (int)EnumBillRequisitionStatus.Approved);
            billRequisitionMasterModel.DataList = filteredMasterList;
            if (vStatus != -1 && vStatus != null)
            {
                billRequisitionMasterModel.DataList = billRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)vStatus);
            }
            return billRequisitionMasterModel;
        }

        #endregion

        #region 1.3.1 ITHead BillRequisition Approve circle

        public async Task<long> ITHeadBillRequisitionApproved(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            long result = -1;
            if (billRequisitionMasterModel.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Pending;

            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;
            List<BillRequisitionDetail> details = _context.BillRequisitionDetails.Where(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && c.IsActive == true).ToList();
            if (details?.Count() <= 0) throw new Exception("Sorry! Damage  not found to Receive!");

            List<BillReqApprovalHistory> history = new List<BillReqApprovalHistory>();
            foreach (var item in details)
            {
                history.Add(new BillReqApprovalHistory
                {
                    BillReqApprovalHistoryId = 0,
                    BillRequisitionDetailId = item.BillRequisitionDetailId,
                    BillRequisitionMasterId = item.BillRequisitionMasterId,
                    DemandQty = item.DemandQty,
                    RemainingQty = item.RemainingQty,
                    ReceivedSoFar = item.ReceivedSoFar,
                    UnitRate = item.UnitRate,
                    TotalPrice = item.TotalPrice,
                    CompanyId = item.CompanyId,
                    EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]),
                    CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                    CreateDate = DateTime.Now,
                });
            }

            foreach (var dt in details)
            {
                var obj = billRequisitionMasterModel.DetailDataList.FirstOrDefault(c => c.BillRequisitionDetailId == dt.BillRequisitionDetailId);

                dt.DemandQty = obj.DemandQty;
                dt.TotalPrice = obj.DemandQty * obj.UnitRate;
                dt.UnitRate = obj.UnitRate;
                //dt.Remarks = obj.Remarks;
                dt.ModifiedBy = userName;
                dt.ModifiedDate = DateTime.Now;
            }

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.QS);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = billRequisitionMasterModel.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                _context.BillReqApprovalHistories.AddRange(history);
                await _context.SaveChangesAsync();

                result = billRequisitionMasterModel.BillRequisitionMasterId;
                scope.Commit();
            }
            return result;
        }
        public async Task<int> ITHeadBillRequisitionRejected(long billRequisitionMasterId)
        {
            int result = -1;
            if (billRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            //if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == billRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Rejected;
            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.QS);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();
                scope.Commit();
                result = billRequisitionMaster.CompanyId;
            }
            return result;
        }

        public async Task<BillRequisitionMasterModel> GetITHeadBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            //var EmpId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);

            billRequisitionMasterModel.CompanyFK = companyId;
            billRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
                                                         && x.CompanyId == companyId
                                                         && x.BillRequisitionTypeId == (int)EnumBillRequisitionSubType.It
                                                         && x.StatusId >= (int)EnumBillRequisitionStatus.Submitted)
                                                                        join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                                        join t3 in _context.ProductSubCategories on t1.BillRequisitionTypeId equals t3.ProductSubCategoryId into t3_Join
                                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                                        join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                                        join t03 in _context.ProductCategories on t3.ProductCategoryId equals t03.ProductCategoryId into t03_Join
                                                                        from t03 in t03_Join.DefaultIfEmpty()
                                                                        select new BillRequisitionMasterModel
                                                                        {
                                                                            BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                            BillRequisitionTypeId = t03.ProductCategoryId,
                                                                            BRTypeName = t03.Name,
                                                                            BillRequisitionSubTypeId = t1.BillRequisitionTypeId,
                                                                            BRSubtypeName = t3.Name,
                                                                            BOQItemId = t1.BOQItemId,
                                                                            ProjectTypeId = t1.ProjectTypeId,
                                                                            ProjectTypeName = t4.Name,
                                                                            CostCenterId = t1.CostCenterId,
                                                                            CostCenterName = t2.Name,
                                                                            Description = t1.Description,
                                                                            BRDate = t1.BRDate,
                                                                            BillRequisitionNo = t1.BillRequisitionNo,
                                                                            StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                                                            CompanyFK = t1.CompanyId,
                                                                            CreatedDate = t1.CreateDate,
                                                                            CreatedBy = t1.CreatedBy,
                                                                            //EmployeeId = t6.Id,
                                                                            //EmployeeStringId = t6.EmployeeId,
                                                                            ApprovalModelList = (from t7 in _context.BillRequisitionApprovals.Where(b => b.BillRequisitionMasterId == t1.BillRequisitionMasterId && b.IsActive)
                                                                                                 join t8 in _context.BillRequisitionMasters on t7.BillRequisitionMasterId equals t8.BillRequisitionMasterId
                                                                                                 select new BillRequisitionApprovalModel
                                                                                                 {
                                                                                                     BRApprovalId = t7.BRApprovalId,
                                                                                                     BillRequisitionMasterId = t8.BillRequisitionMasterId,
                                                                                                     SignatoryId = t7.SignatoryId,
                                                                                                     IsSupremeApproved = t7.IsSupremeApproved,
                                                                                                     AprrovalStatusId = t7.AprrovalStatusId,
                                                                                                 }).OrderBy(m => m.BRApprovalId).ToList(),


                                                                        }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

            var filteredMasterList = billRequisitionMasterModel.DataList.Where(
            q => q.ApprovalModelList.FirstOrDefault(x => x.SignatoryId == (int)EnumBRequisitionSignatory.PM)?.AprrovalStatusId == (int)EnumBillRequisitionStatus.Approved);
            billRequisitionMasterModel.DataList = filteredMasterList;
            if (vStatus != -1 && vStatus != null)
            {
                billRequisitionMasterModel.DataList = billRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)vStatus);
            }
            return billRequisitionMasterModel;
        }

        #endregion

        #region 1.4 PD BillRequisition Approve circle


        public async Task<long> PDBillRequisitionApproved(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            long result = -1;
            if (billRequisitionMasterModel.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Pending;

            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;
            List<BillRequisitionDetail> details = _context.BillRequisitionDetails.Where(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && c.IsActive == true).ToList();
            if (details?.Count() <= 0) throw new Exception("Sorry! Damage  not found to Receive!");

            List<BillReqApprovalHistory> history = new List<BillReqApprovalHistory>();
            foreach (var item in details)
            {
                history.Add(new BillReqApprovalHistory
                {
                    BillReqApprovalHistoryId = 0,
                    BillRequisitionDetailId = item.BillRequisitionDetailId,
                    BillRequisitionMasterId = item.BillRequisitionMasterId,
                    DemandQty = item.DemandQty,
                    RemainingQty = item.RemainingQty,
                    ReceivedSoFar = item.ReceivedSoFar,
                    UnitRate = item.UnitRate,
                    TotalPrice = item.TotalPrice,
                    CompanyId = item.CompanyId,
                    EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]),
                    CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                    CreateDate = DateTime.Now,
                });
            }


            foreach (var dt in details)
            {
                var obj = billRequisitionMasterModel.DetailDataList.FirstOrDefault(c => c.BillRequisitionDetailId == dt.BillRequisitionDetailId);

                dt.DemandQty = obj.DemandQty;
                dt.TotalPrice = obj.DemandQty * obj.UnitRate;
                dt.UnitRate = obj.UnitRate;
                //dt.Remarks = obj.Remarks;
                dt.ModifiedBy = userName;
                dt.ModifiedDate = DateTime.Now;
            }

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.PD);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = billRequisitionMasterModel.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                _context.BillReqApprovalHistories.AddRange(history);
                await _context.SaveChangesAsync();

                result = billRequisitionMasterModel.BillRequisitionMasterId;
                scope.Commit();
            }
            return result;
        }
        public async Task<int> PDBillRequisitionRejected(BillRequisitionMasterModel model)
        {
            int result = -1;
            if (model.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            //if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == model.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Rejected;
            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == model.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.PD);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = model.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();
                scope.Commit();
                result = billRequisitionMaster.CompanyId;
            }
            return result;
        }

        public async Task<BillRequisitionMasterModel> GetPDBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            var EmpId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            if (EmpId <= 0)
            {
                return billRequisitionMasterModel;

            }
            billRequisitionMasterModel.CompanyFK = companyId;
            billRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
                                                         && x.CompanyId == companyId
                                                         && x.StatusId >= (int)EnumBillRequisitionStatus.Submitted)
                                                                        join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                                        join t3 in _context.ProductSubCategories on t1.BillRequisitionTypeId equals t3.ProductSubCategoryId into t3_Join
                                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                                        join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                                        join t03 in _context.ProductCategories on t3.ProductCategoryId equals t03.ProductCategoryId into t03_Join
                                                                        from t03 in t03_Join.DefaultIfEmpty()
                                                                        select new BillRequisitionMasterModel
                                                                        {
                                                                            BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                            BillRequisitionTypeId = t03.ProductCategoryId,
                                                                            BRTypeName = t03.Name,
                                                                            BillRequisitionSubTypeId = t1.BillRequisitionTypeId,
                                                                            BRSubtypeName = t3.Name,
                                                                            BOQItemId = t1.BOQItemId,
                                                                            ProjectTypeId = t1.ProjectTypeId,
                                                                            ProjectTypeName = t4.Name,
                                                                            CostCenterId = t1.CostCenterId,
                                                                            CostCenterName = t2.Name,
                                                                            Description = t1.Description,
                                                                            BRDate = t1.BRDate,
                                                                            BillRequisitionNo = t1.BillRequisitionNo,
                                                                            StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                                                            CompanyFK = t1.CompanyId,
                                                                            CreatedDate = t1.CreateDate,
                                                                            CreatedBy = t1.CreatedBy,
                                                                            //EmployeeId = t6.Id,
                                                                            //EmployeeStringId = t6.EmployeeId,
                                                                            ApprovalModelList = (from t7 in _context.BillRequisitionApprovals.Where(b => b.BillRequisitionMasterId == t1.BillRequisitionMasterId && b.IsActive)
                                                                                                 join t8 in _context.BillRequisitionMasters on t7.BillRequisitionMasterId equals t8.BillRequisitionMasterId
                                                                                                 select new BillRequisitionApprovalModel
                                                                                                 {
                                                                                                     BRApprovalId = t7.BRApprovalId,
                                                                                                     BillRequisitionMasterId = t8.BillRequisitionMasterId,
                                                                                                     SignatoryId = t7.SignatoryId,
                                                                                                     IsSupremeApproved = t7.IsSupremeApproved,
                                                                                                     AprrovalStatusId = t7.AprrovalStatusId,
                                                                                                 }).OrderBy(m => m.BRApprovalId).ToList(),


                                                                        }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

            var filteredMasterList = billRequisitionMasterModel.DataList.Where(
            q => q.ApprovalModelList.FirstOrDefault(x => x.SignatoryId == (int)EnumBRequisitionSignatory.Director)?.AprrovalStatusId == (int)EnumBillRequisitionStatus.Approved);

            billRequisitionMasterModel.DataList = filteredMasterList;
            if (vStatus != -1 && vStatus != null)
            {
                billRequisitionMasterModel.DataList = billRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)vStatus);
            }
            return billRequisitionMasterModel;
        }

        #endregion

        #region 1.5 Director BillRequisition Approve circle
        public async Task<long> DirectorBillRequisitionApproved(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            long result = -1;
            if (billRequisitionMasterModel.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Pending;

            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;
            List<BillRequisitionDetail> details = _context.BillRequisitionDetails.Where(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && c.IsActive == true).ToList();
            if (details?.Count() <= 0) throw new Exception("Sorry! Damage  not found to Receive!");

            List<BillReqApprovalHistory> history = new List<BillReqApprovalHistory>();
            foreach (var item in details)
            {
                history.Add(new BillReqApprovalHistory
                {
                    BillReqApprovalHistoryId = 0,
                    BillRequisitionDetailId = item.BillRequisitionDetailId,
                    BillRequisitionMasterId = item.BillRequisitionMasterId,
                    DemandQty = item.DemandQty,
                    RemainingQty = item.RemainingQty,
                    ReceivedSoFar = item.ReceivedSoFar,
                    UnitRate = item.UnitRate,
                    TotalPrice = item.TotalPrice,
                    CompanyId = item.CompanyId,
                    EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]),
                    CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                    CreateDate = DateTime.Now,
                });
            }

            foreach (var dt in details)
            {
                var obj = billRequisitionMasterModel.DetailDataList.FirstOrDefault(c => c.BillRequisitionDetailId == dt.BillRequisitionDetailId);

                dt.DemandQty = obj.DemandQty;
                dt.TotalPrice = obj.DemandQty * obj.UnitRate;
                dt.UnitRate = obj.UnitRate;
                //dt.Remarks = obj.Remarks;
                dt.ModifiedBy = userName;
                dt.ModifiedDate = DateTime.Now;
            }

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.Director);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = billRequisitionMasterModel.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                _context.BillReqApprovalHistories.AddRange(history);
                await _context.SaveChangesAsync();

                result = billRequisitionMasterModel.BillRequisitionMasterId;
                scope.Commit();
            }
            return result;
        }
        public async Task<int> DirectorBillRequisitionRejected(BillRequisitionMasterModel model)
        {
            int result = -1;
            if (model.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            //if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == model.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Rejected;
            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == model.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.Director);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = model.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();
                scope.Commit();
                result = billRequisitionMaster.CompanyId;
            }
            return result;
        }

        public async Task<BillRequisitionMasterModel> GetDirectorBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            var EmpId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            if (EmpId <= 0)
            {
                return billRequisitionMasterModel;

            }
            billRequisitionMasterModel.CompanyFK = companyId;
            billRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
                                                         && x.CompanyId == companyId
                                                         && x.StatusId >= (int)EnumBillRequisitionStatus.Submitted)
                                                                        join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                                        join t3 in _context.ProductSubCategories on t1.BillRequisitionTypeId equals t3.ProductSubCategoryId into t3_Join
                                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                                        join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                                        join t03 in _context.ProductCategories on t3.ProductCategoryId equals t03.ProductCategoryId into t03_Join
                                                                        from t03 in t03_Join.DefaultIfEmpty()
                                                                        select new BillRequisitionMasterModel
                                                                        {
                                                                            BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                            BillRequisitionTypeId = t03.ProductCategoryId,
                                                                            BRTypeName = t03.Name,
                                                                            BillRequisitionSubTypeId = t1.BillRequisitionTypeId,
                                                                            BRSubtypeName = t3.Name,
                                                                            BOQItemId = t1.BOQItemId,
                                                                            ProjectTypeId = t1.ProjectTypeId,
                                                                            ProjectTypeName = t4.Name,
                                                                            CostCenterId = t1.CostCenterId,
                                                                            CostCenterName = t2.Name,
                                                                            Description = t1.Description,
                                                                            BRDate = t1.BRDate,
                                                                            BillRequisitionNo = t1.BillRequisitionNo,
                                                                            StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                                                            CompanyFK = t1.CompanyId,
                                                                            CreatedDate = t1.CreateDate,
                                                                            CreatedBy = t1.CreatedBy,
                                                                            //EmployeeId = t6.Id,
                                                                            //EmployeeStringId = t6.EmployeeId,
                                                                            ApprovalModelList = (from t7 in _context.BillRequisitionApprovals.Where(b => b.BillRequisitionMasterId == t1.BillRequisitionMasterId && b.IsActive)
                                                                                                 join t8 in _context.BillRequisitionMasters on t7.BillRequisitionMasterId equals t8.BillRequisitionMasterId
                                                                                                 select new BillRequisitionApprovalModel
                                                                                                 {
                                                                                                     BRApprovalId = t7.BRApprovalId,
                                                                                                     BillRequisitionMasterId = t8.BillRequisitionMasterId,
                                                                                                     SignatoryId = t7.SignatoryId,
                                                                                                     IsSupremeApproved = t7.IsSupremeApproved,
                                                                                                     AprrovalStatusId = t7.AprrovalStatusId,
                                                                                                 }).OrderBy(m => m.BRApprovalId).ToList(),


                                                                        }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

            var filteredMasterList = billRequisitionMasterModel.DataList.Where(
            q => q.ApprovalModelList.FirstOrDefault(x => x.SignatoryId == (int)EnumBRequisitionSignatory.QS)?.AprrovalStatusId == (int)EnumBillRequisitionStatus.Approved);

            billRequisitionMasterModel.DataList = filteredMasterList;
            if (vStatus != -1 && vStatus != null)
            {
                billRequisitionMasterModel.DataList = billRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)vStatus);
            }
            return billRequisitionMasterModel;
        }

        #endregion

        #region 1.6 MD BillRequisition Approve circle
        public async Task<long> MDBillRequisitionApproved(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            long result = -1;
            if (billRequisitionMasterModel.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Approved;

            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;
            List<BillRequisitionDetail> details = _context.BillRequisitionDetails.Where(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && c.IsActive == true).ToList();
            if (details?.Count() <= 0) throw new Exception("Sorry! Damage  not found to Receive!");

            List<BillReqApprovalHistory> history = new List<BillReqApprovalHistory>();
            foreach (var item in details)
            {
                history.Add(new BillReqApprovalHistory
                {
                    BillReqApprovalHistoryId = 0,
                    BillRequisitionDetailId = item.BillRequisitionDetailId,
                    BillRequisitionMasterId = item.BillRequisitionMasterId,
                    DemandQty = item.DemandQty,
                    RemainingQty = item.RemainingQty,
                    ReceivedSoFar = item.ReceivedSoFar,
                    UnitRate = item.UnitRate,
                    TotalPrice = item.TotalPrice,
                    CompanyId = item.CompanyId,
                    EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]),
                    CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                    CreateDate = DateTime.Now,
                });
            }


            foreach (var dt in details)
            {
                var obj = billRequisitionMasterModel.DetailDataList.FirstOrDefault(c => c.BillRequisitionDetailId == dt.BillRequisitionDetailId);

                dt.DemandQty = obj.DemandQty;
                dt.TotalPrice = obj.DemandQty * obj.UnitRate;
                dt.UnitRate = obj.UnitRate;
                //dt.Remarks = obj.Remarks;
                dt.ModifiedBy = userName;
                dt.ModifiedDate = DateTime.Now;
            }

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.MD);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
            BRApproval.IsSupremeApproved = true;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = billRequisitionMasterModel.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                _context.BillReqApprovalHistories.AddRange(history);
                await _context.SaveChangesAsync();

                result = billRequisitionMasterModel.BillRequisitionMasterId;
                scope.Commit();
            }
            return result;
        }
        public async Task<int> MDBillRequisitionRejected(BillRequisitionMasterModel model)
        {
            int result = -1;
            if (model.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            //if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == model.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Rejected;
            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == model.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.MD);
            BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
            BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            BRApproval.Reasons = model.CancelReason;
            BRApproval.ModifiedBy = userName;
            BRApproval.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();
                scope.Commit();
                result = billRequisitionMaster.CompanyId;
            }
            return result;
        }

        public async Task<BillRequisitionMasterModel> GetMDBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            var EmpId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            if (EmpId <= 0)
            {
                return billRequisitionMasterModel;

            }
            billRequisitionMasterModel.CompanyFK = companyId;
            billRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
                                              && x.CompanyId == companyId
                                              && x.StatusId >= (int)EnumBillRequisitionStatus.Submitted)
                                                                        join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                                        join t3 in _context.ProductSubCategories on t1.BillRequisitionTypeId equals t3.ProductSubCategoryId into t3_Join
                                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                                        join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                                        join t03 in _context.ProductCategories on t3.ProductCategoryId equals t03.ProductCategoryId into    t03_Join
                                                                        from t03 in t03_Join.DefaultIfEmpty()
                                                                        select new BillRequisitionMasterModel
                                                                        {
                                                                            BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                            BillRequisitionTypeId = t03.ProductCategoryId,
                                                                            BRTypeName = t03.Name,
                                                                            BillRequisitionSubTypeId = t1.BillRequisitionTypeId,
                                                                            BRSubtypeName = t3.Name,
                                                                            BOQItemId = t1.BOQItemId,
                                                                            ProjectTypeId = t1.ProjectTypeId,
                                                                            ProjectTypeName = t4.Name,
                                                                            CostCenterId = t1.CostCenterId,
                                                                            CostCenterName = t2.Name,
                                                                            Description = t1.Description,
                                                                            BRDate = t1.BRDate,
                                                                            BillRequisitionNo = t1.BillRequisitionNo,
                                                                            StatusId = (EnumBillRequisitionStatus)t1.StatusId,
                                                                            CompanyFK = t1.CompanyId,
                                                                            CreatedDate = t1.CreateDate,
                                                                            CreatedBy = t1.CreatedBy,
                                                                            //EmployeeId = t6.Id,
                                                                            //EmployeeStringId = t6.EmployeeId,
                                                                            ApprovalModelList = (from t7 in _context.BillRequisitionApprovals.Where(b => b.BillRequisitionMasterId == t1.BillRequisitionMasterId && b.IsActive)
                                                                                                 join t8 in _context.BillRequisitionMasters on t7.BillRequisitionMasterId equals t8.BillRequisitionMasterId
                                                                                                 select new BillRequisitionApprovalModel
                                                                                                 {
                                                                                                     BRApprovalId = t7.BRApprovalId,
                                                                                                     BillRequisitionMasterId = t8.BillRequisitionMasterId,
                                                                                                     SignatoryId = t7.SignatoryId,
                                                                                                     IsSupremeApproved = t7.IsSupremeApproved,
                                                                                                     AprrovalStatusId = t7.AprrovalStatusId,
                                                                                                 }).OrderBy(m => m.BRApprovalId).ToList(),


                                                                        }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

            //var filteredMasterList = billRequisitionMasterModel.DataList.Where(
            //q => q.ApprovalModelList.FirstOrDefault(x => x.SignatoryId == (int)EnumBRequisitionSignatory.PD)?.AprrovalStatusId == (int)EnumBillRequisitionStatus.Approved);

            //billRequisitionMasterModel.DataList = filteredMasterList;
            if (vStatus != -1 && vStatus != null)
            {
                billRequisitionMasterModel.DataList = billRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)vStatus);
            }

         

            return billRequisitionMasterModel;
        }

        //public string GetRequisitionVoucherStatusMd(long billRequisitionId)
        public async Task<string> GetRequisitionVoucherStatusMd(long billRequisitionId)
        {
            string status = "";

            var requisition = _context.BillRequisitionMasters.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionId);
            var approvedRequisitionAmount = _context.BillRequisitionDetails.Where(x => x.BillRequisitionMasterId == billRequisitionId && x.IsActive).Sum(s => s.UnitRate * s.DemandQty);
            if (requisition !=null)
            {
                var VoucherBRMapMasterList = _context.VoucherBRMapMasters
                .Where(x => x.BillRequsitionMasterId == requisition.BillRequisitionMasterId &&
                            x.IsRequisitionVoucher &&
                            x.IsActive &&
                            (x.ApprovalStatusId == (int)EnumBillRequisitionStatus.Approved ||
                             x.ApprovalStatusId == (int)EnumBillRequisitionStatus.Submitted))
                .ToList();


                var creditAmount = (decimal)0;
                try
                {
                    if (VoucherBRMapMasterList != null && VoucherBRMapMasterList.Count > 0)
                    {
                        var voucherBRMapMasterIds = VoucherBRMapMasterList.Select(s => s.VoucherBRMapMasterId);
                       var voucherBRMapDetails = _context.VoucherBRMapDetails.Where(x => voucherBRMapMasterIds.Contains(x.VoucherBRMapMasterId)).ToList();
                        if (voucherBRMapDetails != null && voucherBRMapDetails.Count > 0)
                        {
                            creditAmount = voucherBRMapDetails.Sum(x => x.CreditAmount);
                        }
                       
                    }

                }
                catch (Exception ex)
                {

                    throw;
                }
                if (creditAmount > 0)
                {
                    if (creditAmount == approvedRequisitionAmount)
                    {
                        status = "Paid";
                    }
                    else if (creditAmount < approvedRequisitionAmount)
                    {
                        status = "Partially Paid";
                    }
                    else if (creditAmount > approvedRequisitionAmount)
                    {
                        status = "Over Paid";
                    }
                    else
                    {
                        status = "Pending";
                    }
                }
            }
            
            //var debitamount = _context.VoucherBRMapDetails.Where(x => VoucherBRMapMasterList.Select(s => s.VoucherBRMapMasterId).Contains(x.VoucherBRMapMasterId)).Sum(l=> l.DebitAmount);
         
           

            return status;
        }

        #endregion

        public List<Product> GetMaterialByBoqId(long boqId)
        {
            var materials = (
                from t1 in _context.BoQItemProductMaps
                    .Where(x => x.BoQItemId == boqId && x.IsActive)
                join t2 in _context.Products
                    .Where(x => x.IsActive) on t1.ProductId equals t2.ProductId
                select new
                {
                    t1.ProductId,
                    t2.ProductName
                }).ToList();

            var productList = materials.Select(x => new Product
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName
            }).ToList();

            return productList;
        }
        public List<Product> GetMaterialByBoqOverhead(int requisitionSubtypeId)
        {
            var materials = (
                from t1 in _context.Products
                    .Where(x => x.ProductSubCategoryId == requisitionSubtypeId && x.IsActive)
                select new
                {
                    t1.ProductId,
                    t1.ProductName
                }).ToList();

            var productList = materials.Select(x => new Product
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName
            }).ToList();

            return productList;
        }

        public async Task<decimal?> ReceivedSoFarTotal(long boqId, long productId)
        {
            decimal? total = 0M;

            if (boqId == 0)
            {
                var getRequisitionByBoqId = await Task.Run(() =>
                {
                    return (from t1 in _context.BillRequisitionMasters
                                .Where(c => c.BOQItemId == null && c.StatusId == (int)EnumBillRequisitionStatus.Approved && c.IsActive == true)
                            join t2 in _context.BillRequisitionDetails
                                .Where(c => c.ProductId == productId && c.IsActive == true) on t1.BillRequisitionMasterId equals t2.BillRequisitionMasterId
                            select new
                            {
                                t2.DemandQty
                            }).ToList();
                });

                foreach (var item in getRequisitionByBoqId)
                {
                    total += item.DemandQty;
                }

                return total;
            }
            else
            {
                var getRequisitionByBoqId = (
                from t1 in _context.BillRequisitionMasters
                    .Where(c => c.BOQItemId == boqId && c.StatusId == (int)EnumBillRequisitionStatus.Approved && c.IsActive == true)
                join t2 in _context.BillRequisitionDetails
                    .Where(c => c.ProductId == productId && c.IsActive == true) on t1.BillRequisitionMasterId equals t2.BillRequisitionMasterId
                select new
                {
                    t2.DemandQty
                }).ToList();

                foreach (var item in getRequisitionByBoqId)
                {
                    total += item.DemandQty;
                }

                return total;
            }

        }

        // approved requisition demand
        public async Task<object> ApprovedRequisitionDemand(long requisitionId, long materialId)
        {
            decimal TotalCr = 0M, TotalDr = 0M, totalReceivedSoFar = 0M;

            var getReqInfo = await _context.BillRequisitionDetails
                .FirstOrDefaultAsync(a => a.BillRequisitionMasterId == requisitionId && a.ProductId == materialId && a.IsActive);

            var getReceivedSoFar = await _context.PurchaseOrders
                            .Where(t1 => t1.BillRequisitionMasterId == requisitionId && t1.IsActive)
                            .Join(_context.PurchaseOrderDetails.Where(c => c.ProductId == materialId), t1 => t1.PurchaseOrderId, t2 => t2.PurchaseOrderId, (t1, t2) => new
                            {
                                RecivedSoFar = t2.PurchaseQty,
                            }).ToListAsync();

            if (getReceivedSoFar != null)
            {
                totalReceivedSoFar = getReceivedSoFar.Sum(x => x.RecivedSoFar);
            }

            if (requisitionId > 0 && materialId > 0)
            {

                var data = (from t1 in _context.VoucherBRMapDetails.Where(c => c.ProductId == materialId && c.IsActive == true)
                            join t2 in _context.VoucherBRMapMasters.Where(c => c.IsActive && c.BillRequsitionMasterId == requisitionId) on t1.VoucherBRMapMasterId equals t2.VoucherBRMapMasterId
                            select new
                            {
                                t1.CreditAmount,
                                t1.DebitAmount
                            }).ToList();
                if (data != null)
                {
                    TotalCr = data.Sum(x => x.CreditAmount);
                    TotalDr = data.Sum(x => x.DebitAmount);
                }

            }

            if (getReqInfo != null)
            {
                return new
                {
                    ApprovedDemand = getReqInfo.DemandQty,
                    UnitPrice = getReqInfo.UnitRate,
                    TotalCredited = TotalCr,
                    TotalDebited = TotalDr,
                    RecivedSoFar = totalReceivedSoFar
                };
            }

            return null;
        }

        // approved requisition no
        public List<object> ApprovedRequisitionList(int companyId)
        {
            var list = new List<object>();
            foreach (var item in _context.BillRequisitionMasters.Where(a => a.StatusId == (int)EnumBillRequisitionStatus.Approved && a.IsActive == true).ToList())
            {
                list.Add(new { Text = item.BillRequisitionNo, Value = item.BillRequisitionMasterId });
            }
            return list;

        }
        // approved material item by requisition id
        public List<Product> ApprovedMaterialList(int companyId, long requisitionId)
        {
            var list = new List<Product>();
            if (requisitionId > 0)
            {
                var materials = (
                    from t1 in _context.BillRequisitionDetails
                        .Where(x => x.BillRequisitionMasterId == requisitionId && x.CompanyId == companyId && x.IsActive)
                    join t2 in _context.Products
                        .Where(x => x.IsActive) on t1.ProductId equals t2.ProductId
                    select new
                    {
                        t1.ProductId,
                        t2.ProductName
                    }).ToList();

                list = materials.Select(x => new Product
                {
                    ProductId = (int)x.ProductId,
                    ProductName = x.ProductName
                }).ToList();

                return list;
            }

            foreach (var item in _context.Products.Where(a => a.IsActive == true).ToList())
            {
                list.Add(new Product() { ProductId = item.ProductId, ProductName = item.ProductName });
            }
            return list;

        }
    }
}
