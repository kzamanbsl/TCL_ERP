using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.ServiceModel
{
    public class BillRequisitionMasterModel : BaseVM
    {
        public long BillRequisitionMasterId { get; set; }
        public string BillRequisitionNo { get; set; }
        public int CostCenterId { get; set; }
        public int BillRequisitionTypeId { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public EnumBillRequisitionStatus StatusId { get; set; }
        public string StatusName { get { return BaseFunctionalities.GetEnumDescription(this.StatusId); } }
        public CostCenterManagerMapModel costCenterManagerMapModel { get; set; } = new CostCenterManagerMapModel();
        public IEnumerable<BillRequisitionMasterModel> DataList { get; set; } = new List<BillRequisitionMasterModel>();
        public BillRequisitionDetailModel DetailModel { get; set; } = new BillRequisitionDetailModel();
        public IEnumerable<BillRequisitionDetailModel> DetailList { get; set; } = new List<BillRequisitionDetailModel>();
    }
    public class BillRequisitionDetailModel : BaseVM
    {
        public long BillRequisitionDetailId { get; set; }
        public long BillRequisitionMasterId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
