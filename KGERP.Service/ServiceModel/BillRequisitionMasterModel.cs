﻿using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
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
        public string BillRequisitionNo { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public int BillRequisitionTypeId { get; set; }
        public string BRTypeName { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public EnumBillRequisitionStatus StatusId { get; set; }
        public string StatusName { get { return BaseFunctionalities.GetEnumDescription(this.StatusId); } }
        public CostCenterManagerMapModel CostCenterManagerMapModel { get; set; } = new CostCenterManagerMapModel();
        public IEnumerable<BillRequisitionMasterModel> DataList { get; set; } = new List<BillRequisitionMasterModel>();
        public BillRequisitionDetailModel DetailModel { get; set; } = new BillRequisitionDetailModel();
        public IEnumerable<BillRequisitionDetailModel> DetailList { get; set; } = new List<BillRequisitionDetailModel>();

        public SelectList ProjectList { get; set; } = new SelectList(new List<object>());
        public SelectList RequisitionTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList RequisitionItemList { get; set; } = new SelectList(new List<object>());
    }
    public class BillRequisitionDetailModel : BaseVM
    {
        public long BillRequisitionDetailId { get; set; }
        public long BillRequisitionMasterId { get; set; }
        public int BRItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
