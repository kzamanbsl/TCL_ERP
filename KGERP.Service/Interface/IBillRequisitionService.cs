using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public interface IBillRequisitionService
    {
        int GetRequisitionNo();

        #region Bill of Quotation
        bool Add(BillRequisitionBoqModel model);
        bool Edit(BillRequisitionBoqModel model);
        bool Delete(BillRequisitionBoqModel model);
        List<BillBoQItem> GetBillOfQuotationList();
        #endregion

        #region Bill Requisition Item
        bool Add(BillRequisitionItemModel model);
        bool Edit(BillRequisitionItemModel model);
        bool Delete(BillRequisitionItemModel model);
        List<BillRequisitionItem> GetBillRequisitionItemList();
        List<VMCommonUnit> GetUnitList(int companyId);
        #endregion

        #region Bill Requisition Type
        bool Add(BillRequisitionTypeModel model);
        bool Edit(BillRequisitionTypeModel model);
        bool Delete(BillRequisitionTypeModel model);
        List<BillRequisitionType> GetBillRequisitionTypeList();
        #endregion

        #region Cost Center Type
        bool Add(CostCenterTypeModel model);
        bool Edit(CostCenterTypeModel model);
        bool Delete(CostCenterTypeModel model);
        List<Accounting_CostCenterType> GetCostCenterTypeList();
        #endregion

        #region Cost Center Manager Map
        bool Add(CostCenterManagerMapModel model);
        bool Edit(CostCenterManagerMapModel model);
        bool Delete(CostCenterManagerMapModel model);
        List<Employee> GetEmployeeList();
        List<Accounting_CostCenter> GetProjectList();
        List<Accounting_CostCenter> GetProjectListByTypeId(int id);
        List<CostCenterManagerMap> GetCostCenterManagerMapList();
        #endregion

        #region BillRequisition Master Detail
        Task<BillRequisitionMasterModel> GetBillRequisitionMasterDetail(int companyId, long billRequisitionMasterId);
        Task<long> BillRequisitionMasterAdd(BillRequisitionMasterModel model);
        Task<long> BillRequisitionDetailAdd(BillRequisitionMasterModel model);
        Task<long> BillRequisitionDetailEdit(BillRequisitionMasterModel model);
        Task<long> SubmitBillRequisitionMaster(long? id = 0);
        Task<long> BillRequisitionMasterEdit(BillRequisitionMasterModel model);
        Task<BillRequisitionMasterModel> GetBillRequisitionMasterById(long billRequisitionMaster);
        Task<long> BillRequisitionDetailDelete(long id);
        Task<long> BillRequisitionMasterDelete(long id);
        Task<BillRequisitionDetailModel> GetSingleBillRequisitionDetails(long id);
        Task<BillRequisitionMasterModel> GetBillRequisitionMasterList(int companyId, DateTime? fromDate, DateTime? toDate, int? statusId);


        #region 1.2 BillRequisition Received Circle
        Task<long> PMBillRequisitionReceived(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<BillRequisitionMasterModel> GetPMBillRequisitionMasterReceivedList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion
        #endregion
    }
}
