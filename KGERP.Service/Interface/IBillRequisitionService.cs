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
        #region Employee
        Task<List<Employee>> GetEmployeeList(int companyId);
        #endregion

        #region Project
        Task<List<Accounting_CostCenter>> GetProjectList(int companyId);
        List<Accounting_CostCenter> GetProjectListByTypeId(int id);
        #endregion

        #region Project Type
        Task<bool> Add(CostCenterTypeModel model);
        Task<bool> Edit(CostCenterTypeModel model);
        Task<bool> Delete(CostCenterTypeModel model);
        Task<List<Accounting_CostCenterType>> GetCostCenterTypeList(int companyId);
        #endregion

        #region Project Manager Assign
        bool Add(CostCenterManagerMapModel model);
        bool Edit(CostCenterManagerMapModel model);
        bool Delete(CostCenterManagerMapModel model);
        Task<List<CostCenterManagerMapModel>> GetCostCenterManagerMapList(int companyId);
        #endregion

        #region BoQ Division
        bool Add(BoqDivisionModel model);
        bool Edit(BoqDivisionModel model);
        bool Delete(BoqDivisionModel model);
        List<BoQDivision> BoQDivisionList();
        #endregion

        #region Bill of Quotation
        bool Add(BillRequisitionBoqModel model);
        bool Edit(BillRequisitionBoqModel model);
        bool Delete(BillRequisitionBoqModel model);
        List<BillBoQItem> GetBillOfQuotationList();
        List<BillBoQItem> GetBillOfQuotationListByProjectId(int id);
        #endregion

        #region Budget & Estimating
        bool Add(BillRequisitionItemBoQMapModel model);
        bool Edit(BillRequisitionItemBoQMapModel model);
        bool Delete(BillRequisitionItemBoQMapModel model);
        List<BoQItemProductMap> GetBoQProductMapList();
        #endregion
        
        #region Requisition Type
        bool Add(BillRequisitionTypeModel model);
        bool Edit(BillRequisitionTypeModel model);
        bool Delete(BillRequisitionTypeModel model);
        List<BillRequisitionType> GetBillRequisitionTypeList();
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
        Task<BillRequisitionMasterModel> GetBillRequisitionMasterCommonList(int companyId, DateTime? fromDate, DateTime? toDate, int? statusId);
        

        #region 1.2 BillRequisition Approved Circle
        Task<BillRequisitionMasterModel> GetBillRequisitionMasterDetailWithApproval(int companyId, long billRequisitionMasterId);
        Task<long> PMBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> PMBillRequisitionRejected(long billRequisitionMasterId);
        Task<BillRequisitionMasterModel> GetPMBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion


        #region 1.3 QS BillRequisition Approved Circle
        Task<long> QSBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> QSBillRequisitionRejected(long billRequisitionMasterId);
        Task<BillRequisitionMasterModel> GetQSBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.3.1 ITHead BillRequisition Approved Circle
        Task<long> ITHeadBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> ITHeadBillRequisitionRejected(long billRequisitionMasterId);
        Task<BillRequisitionMasterModel> GetITHeadBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.4 PD BillRequisition Approved Circle
        Task<long> PDBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> PDBillRequisitionRejected(long billRequisitionMasterId);
        Task<BillRequisitionMasterModel> GetPDBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.5 Director BillRequisition Approved Circle
        Task<long> DirectorBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> DirectorBillRequisitionRejected(long billRequisitionMasterId);
        Task<BillRequisitionMasterModel> GetDirectorBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.6 MD BillRequisition Approved Circle
        Task<long> MDBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> MDBillRequisitionRejected(long billRequisitionMasterId);
        Task<BillRequisitionMasterModel> GetMDBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #endregion

        List<Product> GetMaterialByBoqId(long boqId);
        Task<decimal?> ReceivedSoFarTotal(long boqId, long productId);

        Task<BoQItemProductMap> BoqMaterialBudget(long boqId, long productId);
    }
}
