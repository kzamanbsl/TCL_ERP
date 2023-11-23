using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class BillRequisitionItemBoQMapModel : BaseVM
    {
        public long BoQItemProductMapId { get; set; }
        public int ProjectId { get; set; }
        public int BoQItemId { get; set; }
        public int MeterialId { get; set; }
        public int EstimateQuantity { get; set; }
        public decimal EstimateAmount { get; set; }

        public List<Product> MaterialList { get; set; }
        public List<BillBoQItem> BoQItemList { get; set; }
        public List<Accounting_CostCenter> ProjectList { get; set; }
        public List<BoQItemProductMap> BoQItemMapList { get; set; }
    }
}