using KGERP.Data.Models;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public interface IBillRequisitionService
    {
        int GetRequisitionNo();
        bool Add(CostCenterManagerMapModel model);
        List<CostCenterManagerMap> GetCostCenterManagerMapList();
        List<Project> GetProjectList();
        List<Employee> GetEmployeeList();
    }
}
