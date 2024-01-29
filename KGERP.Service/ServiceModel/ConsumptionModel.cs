using KGERP.Service.Implementation.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class ConsumptionModel: BaseVM
    {
        public long ConsumptionMasterId { get; set; }
        public long ConsumptionDetailsId { get; set; }
        public long StoreId { get; set; }
        public int ProjectTypeId { get; set; }
        public String ProjectTypeName { get; set; }
        public int CostCenterId { get; set; }
        public String CostCenterName { get; set; }
        public int BoQDivisionId { get; set; }
        public String BoQDivisionName { get; set; }
        public int BOQItemId { get; set; }
        public String BOQItemName { get; set; }
        public int StatusId { get; set; }
        public String Description { get; set; }
        public Nullable<long> DivisionId { get; set; }
        public Nullable<long> BoqItemId { get; set; }
        public long ProductSubtypeId { get; set; }
        public DateTime ConsumptionDate { get; set; }
        public long ProductId { get; set; }
        
        public ConsumptionDetailModel DetailModel { get; set; }
        public List<ConsumptionDetailModel> DetailList { get; set; }
        public SelectList ProjectTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList ProjectList { get; set; } = new SelectList(new List<object>());
        public SelectList StoreList { get; set; } = new SelectList(new List<object>());

        public SelectList BOQDivisionList { get; set; } = new SelectList(new List<object>());
        public SelectList BOQItemList { get; set; } = new SelectList(new List<object>());
        public SelectList MaterialStoreList { get; set; } = new SelectList(new List<object>());
        public SelectList MaterialTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList MaterialSubTypeList { get; set; } = new SelectList(new List<object>());
    }

    public class ConsumptionDetailModel
    {
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> ConsumedQty { get; set; }
        public Nullable<decimal> StoredQty { get; set; }
        public Nullable<decimal> RemainingQty { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public long? ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitId{ get; set; }
        public string UnitName { get; set; }
        public decimal UnitRate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
