using KGERP.Service.Implementation.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class ConsumptionModel : BaseVM
    {
        public long ConsumptionMasterId { get; set; }
        public long ConsumptionDetailsId { get; set; }
        public DateTime ConsumptionDate { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public long StockInfoId { get; set; }
        public string StoreName { get; set; }
        public long BoQDivisionId { get; set; }
        public string BoQDivisionName { get; set; }

        public int BOQItemId { get; set; }
        public string BOQItemName { get; set; }
        public long DivisionId { get; set; }
        public int? StatusId { get; set; }
        public decimal? TotalAmount { get; set; }
        public ConsumptionDetailModel DetailModel { get; set; } = new ConsumptionDetailModel();
        public List<ConsumptionDetailModel> DetailList { get; set; } = new List<ConsumptionDetailModel>();

        public SelectList BOQItemList { get; set; } = new SelectList(new List<object>());
        public SelectList BOQDivisionList { get; set; } = new SelectList(new List<object>());
        public SelectList StockInfoList { get; set; } = new SelectList(new List<object>());
        public SelectList ProjectList { get; set; } = new SelectList(new List<object>());
        public SelectList ProjectTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList MaterialTypeList { get; set; } = new SelectList(new List<object>());
        //public SelectList MaterialSubTypeList { get; set; } = new SelectList(new List<object>());
    }

    public class ConsumptionDetailModel
    {
        public long ConsumptionMasterId { get; set; }
        public long ConsumptionDetailsId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductSubTypeId { get; set; }
        public string ProductSubTypeName { get; set; }
        public decimal? StoredQty { get; set; }
        public decimal? TotalProductReceived { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? RemainingQty { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? ConsumedQty { get; set; }
    }
}
