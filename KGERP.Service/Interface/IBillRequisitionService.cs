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

        #region Bill Requisition Item
        bool Add(BillRequisitionItemModel model);
        bool Edit(BillRequisitionItemModel model);
        bool Delete(BillRequisitionItemModel model);
        List<BillRequisitionItem> GetBillRequisitionItemList();
        #endregion

        #region Bill Requisition Type
        bool Add(BillRequisitionTypeModel model);
        bool Edit(BillRequisitionTypeModel model);
        bool Delete(BillRequisitionTypeModel model);
        List<BillRequisitionType> GetBillRequisitionTypeList();
        #endregion

        #region Cost Center Manager Map
        bool Add(CostCenterManagerMapModel model);
        bool Edit(CostCenterManagerMapModel model);
        bool Delete(CostCenterManagerMapModel model);
        List<Employee> GetEmployeeList();
        List<Accounting_CostCenter> GetProjectList();
        List<CostCenterManagerMap> GetCostCenterManagerMapList();
        #endregion
    }
}
