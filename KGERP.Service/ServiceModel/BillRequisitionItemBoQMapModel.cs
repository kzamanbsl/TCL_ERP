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
        public int BoQItemId { get; set; }
        public int BillRequisitionItemId { get; set; }
        public int EstimateQuantity { get; set; }
        public decimal EstimateAmount { get; set; }

        public List<Product> Products { get; set; }
        public List<BillBoQItem> BillBoQItems { get; set; }
        public List<Accounting_CostCenter> Projects { get; set; }
        public List<Accounting_CostCenterType> ProjectTypes { get; set; }
        public List<BoQItemProductMap> BoQItemProductMaps { get; set; }
    }
}