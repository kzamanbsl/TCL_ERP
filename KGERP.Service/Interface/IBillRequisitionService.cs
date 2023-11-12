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
        bool Edit(CostCenterManagerMapModel model);
        bool Delete(int id);
        List<Employee> GetEmployeeList();
        List<Accounting_CostCenter> GetProjectList();
        List<CostCenterManagerMap> GetCostCenterManagerMapList();
    }
}
