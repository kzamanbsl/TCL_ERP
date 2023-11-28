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
        List<dynamic> GetMaterialDetailWithNameAndUnitId(long boqId);
        decimal ReceivedSoFarTotal(int id);

        #region Bill of Quotation and Req. Item Map
        bool Add(BillRequisitionItemBoQMapModel model);
        bool Edit(BillRequisitionItemBoQMapModel model);
        bool Delete(BillRequisitionItemBoQMapModel model);
        List<BoQItemProductMap> GetBoQProductMapList();
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
        Task<BillRequisitionMasterModel> GetBillRequisitionMasterCommonList(int companyId, DateTime? fromDate, DateTime? toDate, int? statusId);
        

        #region 1.2 BillRequisition Approved Circle
        Task<BillRequisitionMasterModel> GetBillRequisitionMasterDetailWithApproval(int companyId, long billRequisitionMasterId);
        Task<long> PMBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<long> PMBillRequisitionRejected(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<BillRequisitionMasterModel> GetPMBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion


        #region 1.3 QS BillRequisition Approved Circle
        Task<long> QSBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<long> QSBillRequisitionRejected(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<BillRequisitionMasterModel> GetQSBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.3.1 ITHead BillRequisition Approved Circle
        Task<long> ITHeadBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<long> ITHeadBillRequisitionRejected(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<BillRequisitionMasterModel> GetITHeadBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.4 QS BillRequisition Approved Circle
        Task<long> PDBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<long> PDBillRequisitionRejected(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<BillRequisitionMasterModel> GetPDBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.5 Director BillRequisition Approved Circle
        Task<long> DirectorBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<long> DirectorBillRequisitionRejected(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<BillRequisitionMasterModel> GetDirectorBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.6 MD BillRequisition Approved Circle
        Task<long> MDBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<long> MDBillRequisitionRejected(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<BillRequisitionMasterModel> GetMDBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #endregion
    }
}
