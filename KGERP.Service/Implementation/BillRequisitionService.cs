using KGERP.Data.Models;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public List<Project> GetProjectList()
        {
            List<Project> projects = new List<Project>();
            var getProject = _context.Accounting_CostCenter.Where(c => c.IsActive == true).ToList();
            foreach ( var project in getProject )
            {
                var data = new Project()
                {
                    ProjectId = project.CostCenterId,
                    ProjectName = project.Name
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
            var getCostCenterManagerMaps = _context.CostCenterManagerMaps.Where(c=>c.IsActive == true).ToList();
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
            if(model != null)
            {
                try
                {
                    CostCenterManagerMap data = new CostCenterManagerMap()
                    {
                        CostCenterId = model.ProjectId,
                        ManagerId = model.EmployeeRowId,
                        //CompanyId = model.CompanyId,
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
    }
}
