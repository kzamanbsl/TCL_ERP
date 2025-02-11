﻿using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc.Routing.Constraints;

namespace KGERP.Service.Interface
{
    public interface IBillRequisitionService
    {
        #region Setting for building
        Task<bool> Add(BuildingFloorModel model);
        Task<bool> Edit(BuildingFloorModel model);
        Task<bool> Delete(BuildingFloorModel model);
        Task<List<BuildingFloorModel>> GetFloorList(int companyId);
        #endregion

        #region Setting for member
        Task<bool> Add(BuildingMemberModel model);
        Task<bool> Edit(BuildingMemberModel model);
        Task<bool> Delete(BuildingMemberModel model);
        Task<List<BuildingMemberModel>> GetMemberList(int companyId);
        #endregion

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
        Task<List<BoqDivisionModel>> BoQDivisionList(long companyId);
        Task<List<BoqDivisionModel>> GetBoqListByProjectId(long projectId);
        #endregion

        #region Bill of Quotation
        bool Add(BillRequisitionBoqModel model);
        bool Edit(BillRequisitionBoqModel model);
        bool Delete(BillRequisitionBoqModel model);
        List<BillRequisitionBoqModel> GetBillOfQuotationList();
        Task<List<BillRequisitionBoqModel>> GetBoqListByDivisionId(long id);
        List<BillBoQItem> GetBillOfQuotationListByProjectId(int id);
        Task<bool> IsBoqExistByDivisionId(long divisionId, string boqNumber);
        #endregion

        #region Budget & Estimating
        bool Add(BillRequisitionItemBoQMapModel model);
        bool Edit(BillRequisitionItemBoQMapModel model);
        bool Delete(BillRequisitionItemBoQMapModel model);
        //List<BoQItemProductMap> GetBoQProductMapList();
        List<BillRequisitionItemBoQMapModel> GetBoQProductMapList();
        Task<bool> IsBoqBudgetExistByBoqId(long boqItemId, long materialId);
        Task<BillRequisitionMasterModel> GetBoqAndBudgetDetailWithApproval(int companyId = 21, int boqMapId = 0);
        Task<BillRequisitionItemBoQMapModel> FilteredBudgetAndEstimatingApprovalList(int projectId = 0, long? boqDivisionId = 0,int? BoqItemId=0, EnumBudgetAndEstimatingApprovalStatus approvalStatus=default );
        Task<long> BoqAndEstimatingItemApproved(BillRequisitionItemBoQMapModel masterModel);
        Task<long> BoqAndEstimatingRevisedItemApproved(BillRequisitionItemBoQMapModel masterModel);

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
        Task<int> PMBillRequisitionRejected(BillRequisitionMasterModel model);
        Task<BillRequisitionMasterModel> GetPMBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion


        #region 1.3 QS BillRequisition Approved Circle
        Task<long> QSBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> QSBillRequisitionRejected(BillRequisitionMasterModel model);
        Task<BillRequisitionMasterModel> GetQSBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.3.1 ITHead BillRequisition Approved Circle
        Task<long> ITHeadBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> ITHeadBillRequisitionRejected(BillRequisitionMasterModel model);
        Task<BillRequisitionMasterModel> GetITHeadBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.3.2 Fuel BillRequisition Approved Circle
        Task<long> FuelBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> FuelBillRequisitionRejected(BillRequisitionMasterModel model);
        Task<BillRequisitionMasterModel> GetFuelBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.4 PD BillRequisition Approved Circle
        Task<long> PDBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> PDBillRequisitionRejected(BillRequisitionMasterModel model);
        Task<BillRequisitionMasterModel> GetPDBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.5 Director BillRequisition Approved Circle
        Task<long> DirectorBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> DirectorBillRequisitionRejected(BillRequisitionMasterModel model);
        Task<BillRequisitionMasterModel> GetDirectorBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        #endregion

        #region 1.6 MD BillRequisition Approved Circle
        Task<long> MDBillRequisitionApproved(BillRequisitionMasterModel BillRequisitionMasterModel);
        Task<int> MDBillRequisitionRejected(BillRequisitionMasterModel model);
        Task<BillRequisitionMasterModel> GetMDBillRequisitionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus);
        Task<string> GetRequisitionVoucherStatusMd(long billRequisitionId);
        #endregion

        #endregion

        List<Product> GetMaterialByBoqAndSubCategory(long boqId, long subtypeId);
        List<Product> GetMaterialBySubCategory(long id);
        List<ProductSubCategory> GetSubcategoryByTypeAndBoq(long typeId, long boqId);
        List<ProductSubCategory> GetSubcategoryByBoq(long id);
        List<Product> GetMaterialByBoqId(long boqId);
        List<Product> GetMaterialByBoqOverhead(int requisitionSubtypeId);
        decimal ReceivedSoFarTotal(long projectId, long boqId, long productId);
        decimal GetTotalByMasterId(long requisitionId);
        BoQItemProductMap BoqMaterialBudget(long boqId, long productId);
        Task<object> ApprovedRequisitionDemand(long requisitionId, long materialId);
        List<object> ApprovedRequisitionList(int companyId);
        List<object> FilteredApprovedRequisitionList(int projectId);
        List<Product> ApprovedMaterialList(int companyId, long requisitionId);
    }
}
