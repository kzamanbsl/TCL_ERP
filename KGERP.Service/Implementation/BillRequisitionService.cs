using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.IdentityModel.Tokens;
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

        #region Bill of Quotation

        public List<BillBoQItem> GetBillOfQuotationList()
        {
            List<BillBoQItem> billBoQItems = new List<BillBoQItem>();
            var getBillBoQItems = _context.BillBoQItems.Where(c => c.IsActive == true).ToList();
            foreach (var item in getBillBoQItems)
            {
                var data = new BillBoQItem()
                {
                    BoQItemId = item.BoQItemId,
                    Name = item.Name,
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
                        Name = model.Name,
                        Description = model.Description,
                        CompanyId = (int)model.CompanyFK,
                        IsActive = true,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CreateDate = DateTime.Now
                    };
                    _context.BillBoQItems.Add(data);
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

        public bool Edit(BillRequisitionBoqModel model)
        {
            if (model != null)
            {
                try
                {
                    var findBillRequisitionBoQ = _context.BillBoQItems.FirstOrDefault(c => c.BoQItemId == model.BoQItemId);

                    findBillRequisitionBoQ.Name = model.Name;
                    findBillRequisitionBoQ.Description = model.Description;
                    findBillRequisitionBoQ.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBillRequisitionBoQ.ModifiedDate = DateTime.Now;
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

        public bool Delete(BillRequisitionBoqModel model)
        {
            if (model.BoQItemId > 0 || model.BoQItemId != null)
            {
                try
                {
                    var findBillRequisitionBoQ = _context.BillBoQItems.FirstOrDefault(c => c.BoQItemId == model.BoQItemId);

                    findBillRequisitionBoQ.IsActive = false;
                    findBillRequisitionBoQ.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findBillRequisitionBoQ.ModifiedDate = DateTime.Now;
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
                    Description = item.Description,
                    UnitId = item.UnitId,
                    BoQItemId = item.BoQItemId
                };
                billRequisitionItems.Add(data);
            }
            
            return billRequisitionItems;
        }

        public List<VMCommonUnit> GetUnitList(int companyId)
        {
            List<VMCommonUnit> commonUnits = new List<VMCommonUnit>();
            var getCommonUnits = _context.Units.Where(c => c.CompanyId == companyId && c.IsActive == true).ToList();
            foreach (var item in getCommonUnits)
            {
                var data = new VMCommonUnit()
                {
                    ID = item.UnitId,
                    Name = item.Name,
                };
                commonUnits.Add(data);
            }

            return commonUnits;
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
                        CompanyId = (int)model.CompanyFK,
                        UnitId = model.UnitId,
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
                    var findBillRequisitionItem = _context.BillRequisitionItems.FirstOrDefault(c => c.BillRequisitionItemId == model.ID);

                    findBillRequisitionItem.Name = model.Name;
                    findBillRequisitionItem.Description = model.Description;
                    findBillRequisitionItem.UnitId = model.UnitId;
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

        #region Bill Requisition Type

        public List<Accounting_CostCenterType> GetCostCenterTypeList()
        {
            List<Accounting_CostCenterType> costCenterTypes = new List<Accounting_CostCenterType>();
            var getCostCenterTypes = _context.Accounting_CostCenterType.Where(c => c.CompanyId == 21 && c.IsActive == true).ToList();
            foreach (var item in getCostCenterTypes)
            {
                var data = new Accounting_CostCenterType()
                {
                    CostCenterTypeId = item.CostCenterTypeId,
                    Name = item.Name,
                };
                costCenterTypes.Add(data);
            }
            return costCenterTypes;
        }

        public bool Add(CostCenterTypeModel model)
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
                        CreatedDate = DateTime.Now
                    };
                    _context.Accounting_CostCenterType.Add(data);
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

        public bool Edit(CostCenterTypeModel model)
        {
            if (model != null)
            {
                try
                {
                    var findCostCenterType = _context.Accounting_CostCenterType.FirstOrDefault(c => c.CostCenterTypeId == model.ID);

                    findCostCenterType.Name = model.Name;
                    findCostCenterType.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findCostCenterType.ModifiedDate = DateTime.Now.ToString();
                    var count = _context.SaveChanges();
                    model.CompanyFK = findCostCenterType.CompanyId;
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

        public bool Delete(CostCenterTypeModel model)
        {
            if (model.CostCenterTypeId > 0 || model.CostCenterTypeId != null)
            {
                try
                {
                    var findCostCenterType = _context.Accounting_CostCenterType.FirstOrDefault(c => c.CostCenterTypeId == model.CostCenterTypeId);

                    findCostCenterType.IsActive = false;
                    findCostCenterType.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findCostCenterType.ModifiedDate = DateTime.Now.ToString();
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
            var getProjects = _context.Accounting_CostCenter.Where(c => c.CompanyId == 21 && c.IsActive == true).ToList();
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
                        CompanyId = (int)model.CompanyFK,
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
        public async Task<BillRequisitionMasterModel> GetBillRequisitionMasterDetail(int companyId = 21, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();


            billRequisitionMasterModel = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive && x.BillRequisitionMasterId == billRequisitionMasterId)

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
                                                                   ProjectTypeId =t1.ProjectTypeId,
                                                                   ProjectTypeName = t4.Name,
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
                                                                          join t3 in _context.BillRequisitionItems.Where(x => x.IsActive) on t1.BillRequisitionItemId equals t3.BillRequisitionItemId into t3_Join
                                                                          from t3 in t3_Join.DefaultIfEmpty()
                                                                          join t4 in _context.Units.Where(x => x.IsActive) on t1.UnitId equals t4.UnitId into t4_Join
                                                                          from t4 in t4_Join.DefaultIfEmpty()

                                                                          select new BillRequisitionDetailModel
                                                                          {
                                                                              BillRequisitionDetailId = t1.BillRequisitionDetailId,
                                                                              BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                                                              BillRequisitionItemId = t1.BillRequisitionItemId,
                                                                              ItemName = t3.Name,
                                                                              UnitId = t1.UnitId,
                                                                              UnitName = t4.Name,
                                                                              UnitRate = t1.UnitRate,
                                                                              TotalPrice = t1.TotalPrice,
                                                                              DemandQty = t1.DemandQty,
                                                                              ReceivedSoFar = t1.ReceivedSoFar,
                                                                              RemainingQty = t1.RemainingQty,
                                                                              EstimatedQty = t1.EstimatedQty,
                                                                              Remarks = t1.Remarks,
                                                                          }).OrderByDescending(x => x.BillRequisitionDetailId).AsEnumerable());


            return billRequisitionMasterModel;
        }

        public async Task<long> BillRequisitionMasterAdd(BillRequisitionMasterModel model)
        {
            long result = -1;

            if(model.StatusId == 0 || model.StatusId == null)
            {
                model.StatusId = EnumBillRequisitionStatus.Draft;
            }

            try
            {
                BillRequisitionMaster billRequisitionMaster = new BillRequisitionMaster
                {
                    BillRequisitionMasterId = model.BillRequisitionMasterId,
                    BRDate = model.BRDate,
                    BillRequisitionTypeId = model.BillRequisitionTypeId,
                    BOQItemId = model.BOQItemId,
                    ProjectTypeId = model.ProjectTypeId,
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
                    BillRequisitionItemId = model.DetailModel.BillRequisitionItemId,
                    UnitRate = model.DetailModel.UnitRate,
                    DemandQty = model.DetailModel.DemandQty,
                    UnitId = model.DetailModel.UnitId,
                    ReceivedSoFar = model.DetailModel.ReceivedSoFar,
                    RemainingQty = model.DetailModel.RemainingQty,
                    EstimatedQty = model.DetailModel.EstimatedQty,
                    Floor = model.DetailModel.Floor,
                    Ward = model.DetailModel.Ward,
                    DPP = model.DetailModel.DPP,
                    Chainage = model.DetailModel.Chainage,
                    Remarks = model.DetailModel.Remarks,
                    CompanyId = (int)model.CompanyFK,
                    CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
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
            catch(Exception error)
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
            demageDetail.BillRequisitionItemId = model.DetailModel.BillRequisitionItemId;
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


            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Submitted;
            billRequisitionMaster.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();

                BillRequisitionMasterModel model = new BillRequisitionMasterModel();
                List<BillRequisitionApproval> billRequisitionApprovalList = new List<BillRequisitionApproval>();
                int priority = 0;
                foreach (var item in model.EnumBRSignatoryList)
                {

                    BillRequisitionApproval billRequisitionApproval = new BillRequisitionApproval();
                    billRequisitionApproval.BillRequisitionMasterId = billRequisitionMaster.BillRequisitionMasterId;
                    billRequisitionApproval.CompanyId = billRequisitionMaster.CompanyId;
                    billRequisitionApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);

                    billRequisitionApproval.SignatoryId = Convert.ToInt16(item.Value);

                    if (billRequisitionApproval.SignatoryId == 1)
                    {
                        billRequisitionApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
                    }
                    else
                    {
                        billRequisitionApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Pending;
                    }
                    priority = priority + 1;
                    billRequisitionApproval.PriorityNo = priority;
                    billRequisitionApproval.IsActive = true;
                    billRequisitionApproval.IsSupremeApproved = false;

                    billRequisitionApproval.CreateDate = DateTime.Now;
                    billRequisitionApproval.CreatedBy = billRequisitionMaster.CreatedBy;
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
                                          join t3 in _context.BillRequisitionItems.Where(x => x.IsActive) on t1.BillRequisitionItemId equals t3.BillRequisitionItemId into t3_Join
                                          from t3 in t3_Join.DefaultIfEmpty()

                                          select new BillRequisitionDetailModel
                                          {
                                              BillRequisitionDetailId = t1.BillRequisitionDetailId,
                                              BillRequisitionMasterId = t1.BillRequisitionMasterId,
                                              BillRequisitionItemId = t1.BillRequisitionItemId,
                                              ItemName = t3.Name,
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
                                                                        }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

            if (statusId != -1 && statusId != null)
            {
                BillRequisitionMasterModel.DataList = BillRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)statusId);
            }
            return BillRequisitionMasterModel;
        }

        #endregion
      
        #region 1.2 BillRequisition Approve circle

        public async Task<long> PMBillRequisitionApproved(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            long result = -1;
            if (billRequisitionMasterModel.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Approved;

            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();
                var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.PM);

                BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Approved;
                BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
                BRApproval.ModifiedBy = userName;
                BRApproval.ModifiedDate = DateTime.Now;

                result = billRequisitionMasterModel.BillRequisitionMasterId;
                scope.Commit();
            }
            return result;
        } 
        public async Task<long> PMBillRequisitionRejected(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            long result = -1;
            if (billRequisitionMasterModel.BillRequisitionMasterId <= 0) throw new Exception("Sorry! BillRequisition not found to Receive!");
            if (billRequisitionMasterModel.DetailDataList.Count() <= 0) throw new Exception("Sorry! BillRequisition  Detail not found to Receive!");

            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            BillRequisitionMaster billRequisitionMaster = _context.BillRequisitionMasters.FirstOrDefault(c => c.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId);
            billRequisitionMaster.StatusId = (int)EnumBillRequisitionStatus.Rejected;

            billRequisitionMaster.ModifiedBy = userName;
            billRequisitionMaster.ModifiedDate = DateTime.Now;

            using (var scope = _context.Database.BeginTransaction())
            {
                await _context.SaveChangesAsync();
                var BRApproval = _context.BillRequisitionApprovals.FirstOrDefault(x => x.BillRequisitionMasterId == billRequisitionMasterModel.BillRequisitionMasterId && x.SignatoryId == (int)EnumBRequisitionSignatory.PM);

                BRApproval.AprrovalStatusId = (int)EnumBillRequisitionStatus.Rejected;
                BRApproval.EmployeeId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
                BRApproval.ModifiedBy = userName;
                BRApproval.ModifiedDate = DateTime.Now;

                result = billRequisitionMasterModel.BillRequisitionMasterId;
                scope.Commit();
            }
            return result;
        }
        public async Task<BillRequisitionMasterModel> GetPMBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            long EmpId = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);
            billRequisitionMasterModel.CompanyFK = companyId;
            billRequisitionMasterModel.DataList = await Task.Run(() => (from t1 in _context.BillRequisitionMasters.Where(x => x.IsActive
                                                         && x.CompanyId == companyId
                                                         && x.StatusId >= (int)EnumBillRequisitionStatus.Submitted)
                                                                        join t2 in _context.Accounting_CostCenter on t1.CostCenterId equals t2.CostCenterId into t2_Join
                                                                        from t2 in t2_Join.DefaultIfEmpty()
                                                                        join t3 in _context.BillRequisitionTypes on t1.BillRequisitionTypeId equals t3.BillRequisitionTypeId into t3_Join
                                                                        from t3 in t3_Join.DefaultIfEmpty()
                                                                        join t4 in _context.Accounting_CostCenterType on t1.ProjectTypeId equals t4.CostCenterTypeId into t4_Join
                                                                        from t4 in t4_Join.DefaultIfEmpty()
                                                                        join t5 in _context.CostCenterManagerMaps on t2.CostCenterId equals t5.CostCenterId into t5_Join
                                                                        from t5 in t5_Join.DefaultIfEmpty()
                                                                        join t6 in _context.Employees on t5.ManagerId equals t6.Id into t6_Join
                                                                        from t6 in t6_Join.DefaultIfEmpty()
                                                                        join t7 in _context.BillRequisitionApprovals on t1.BillRequisitionMasterId equals t7.BillRequisitionMasterId into t7_Join
                                                                        from t7 in t7_Join.DefaultIfEmpty()
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
                                                                            EmployeeId = t6.Id,
                                                                            EmployeeStringId = t6.EmployeeId,
                                                                        }).OrderByDescending(x => x.BillRequisitionMasterId).AsEnumerable());

            if (EmpId > 0)
            {
                billRequisitionMasterModel.DataList = billRequisitionMasterModel.DataList.Where(q =>  q.EmployeeId == EmpId);
            }
            if (vStatus != -1 && vStatus != null)
            {
                billRequisitionMasterModel.DataList = billRequisitionMasterModel.DataList.Where(q => q.StatusId == (EnumBillRequisitionStatus)vStatus);
            }
            return billRequisitionMasterModel;
        }

        #endregion

    }
}
