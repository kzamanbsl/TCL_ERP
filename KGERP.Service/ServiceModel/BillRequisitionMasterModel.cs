using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class BillRequisitionMasterModel : BaseVM
    {
        public long BillRequisitionMasterId { get; set; }
        public DateTime BRDate { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public int? BOQItemId { get; set; }
        public string BOQItemName { get; set; }
        public string BillRequisitionNo { get; set; }
        public int CostCenterId { get; set; }
        public long BoQDivisionId { get; set; }
        public string BoQDivisionName { get; set; }
        public string CostCenterName { get; set; }
        public int BillRequisitionTypeId { get; set; }
        public string BRTypeName { get; set; }
        public int BillRequisitionSubTypeId { get; set; }
        public string BRSubtypeName { get; set; }
        public string Description { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string StrFromDate { get; set; }
        public string StrToDate { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeStringId { get; set; }
        public bool IsPMApproved { get; set; }
        public bool IsQSApproved { get; set; }
        public bool IsDirectorApproved { get; set; }
        public bool IsMDApproved { get; set; }
        public string CancelReason { get; set; }
        public string VoucherPaymentStatus { get; set; }
        public string EmployeeName { get; set; }
        public bool PaymentStatus { get; set; }

        public EnumVoucherPaymentStatus VoucherPaymentStatusId { get; set; }
        public string VoucherPaymentStatusName { get { return BaseFunctionalities.GetEnumDescription(this.VoucherPaymentStatusId); } }

        public EnumBillRequisitionStatus StatusId { get; set; }
        public string StatusName { get { return BaseFunctionalities.GetEnumDescription(this.StatusId); } }

        public CostCenterManagerMapModel CostCenterManagerMapModel { get; set; } = new CostCenterManagerMapModel();
        public IEnumerable<BillRequisitionMasterModel> DataList { get; set; } = new List<BillRequisitionMasterModel>();
        public List<BillRequisitionDetailModel> DetailDataList { get; set; } = new List<BillRequisitionDetailModel>();
        public BillRequisitionDetailModel DetailModel { get; set; } = new BillRequisitionDetailModel();
        public IEnumerable<BillRequisitionDetailModel> DetailList { get; set; } = new List<BillRequisitionDetailModel>();
        public BillRequisitionApprovalModel ApprovalModel { get; set; } = new BillRequisitionApprovalModel();
        public List<BillRequisitionApprovalModel> ApprovalModelList { get; set; } = new List<BillRequisitionApprovalModel>();
        public SelectList ProjectList { get; set; } = new SelectList(new List<object>());
        public SelectList RequisitionTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList RequisitionSubTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList BOQDivisionList { get; set; } = new SelectList(new List<object>());
        public SelectList BOQItemList { get; set; } = new SelectList(new List<object>());
        public SelectList ProjectTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList RequisitionItemList { get; set; } = new SelectList(new List<object>());
        public SelectList UnitList { get; set; } = new SelectList(new List<object>());
        public SelectList EnumBRStatusList { get { return new SelectList(BaseFunctionalities.GetEnumList<EnumBillRequisitionStatus>(), "Value", "Text"); } }
        public SelectList EnumBRSignatoryList { get { return new SelectList(BaseFunctionalities.GetEnumList<EnumBRequisitionSignatory>(), "Value", "Text"); } }
        public SelectList FloorList { get; set; } = new SelectList(new List<object>());
        public SelectList MemberList { get; set; } = new SelectList(new List<object>());
    }

    public class BillRequisitionDetailModel : BaseVM
    {
        public long BillRequisitionDetailId { get; set; }
        public long BillRequisitionMasterId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal? EstimatedQty { get; set; }
        public decimal? DemandQty { get; set; }
        public decimal? UnitRate { get; set; }
        public decimal? TotalPrice { get; set; }
        public Nullable<decimal> ReceivedSoFar { get; set; }
        public Nullable<decimal> RemainingQty { get; set; }
        public int MemberId { get; set; }
        public int FloorId { get; set; }
        public string Ward { get; set; }
        public string DPP { get; set; }
        public string Chainage { get; set; }
        public string Pier { get; set; }
        public int CompanyId { get; set; }

        public long BoqDivisionId { get; set; }
        public string BoqDivisionName { get; set; }
        public long? BoqItemId { get; set; }
        public string BoqItemName { get; set; }
        public string BoqNumber { get; set; }
        public long? RequisitionSubtypeId { get; set; }
        public string RequisitionSubtypeName { get; set; }
    }

    public partial class BillRequisitionApprovalModel
    {
        public long BRApprovalId { get; set; }
        public long BillRequisitionMasterId { get; set; }
        public int AprrovalStatusId { get; set; }
        public string AprrovalStatusName { get { return BaseFunctionalities.GetEnumDescription((EnumBillRequisitionStatus)AprrovalStatusId); } }
        public Nullable<long> EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int SignatoryId { get; set; }
        public string SignatoryName { get { return BaseFunctionalities.GetEnumDescription((EnumBRequisitionSignatory)SignatoryId); } }

        public int PriorityNo { get; set; }
        public bool IsSupremeApproved { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public string ApprobalRemarks { get; set; }
        public string VoucherPaymentStatus { get; set; }
    }

}
