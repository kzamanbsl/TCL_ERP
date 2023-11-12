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
                catch (Exception err)
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

                    findBillRequisitionItem.BillRequisitionItemId = model.BillRequisitionItemId;
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
                catch (Exception err)
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
                catch (Exception err)
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
                catch (Exception err)
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

                    findBillRequisitionType.BillRequisitionTypeId = model.BillRequisitionTypeId;
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
                catch (Exception err)
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
                catch (Exception err)
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
                catch (Exception err)
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
                catch (Exception err)
                {
                    return false;
                }
            }
            return false;
        }

        public bool Delete(int id)
        {
            if (id > 0 || id != null)
            {
                try
                {
                    var findCostCenterManagerMap = _context.CostCenterManagerMaps.FirstOrDefault(c => c.CostCenterManagerMapId == id);

                    findCostCenterManagerMap.IsActive = false;
                    findCostCenterManagerMap.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    findCostCenterManagerMap.ModifiedDate = DateTime.Now;
                    var count = _context.SaveChanges();

                    if (count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception err)
                {
                    return false;
                }
            }
            return false;
        }

        #endregion
    }
}
