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
        public int MaterialItemId { get; set; }
        public decimal EstimatedQty { get; set; }
        public decimal EstimatedAmount { get; set; }
        public decimal UnitRate { get; set; }
        public int ProjectId { get; set; }
        public long BoQDivisionId { get; set; }
        public List<BillBoQItem> BoQItems { get; set; }
        public List<Product> BoQMaterials { get; set; }
        public List<BoQDivision> BoQDivisions { get; set; }
        public List<Accounting_CostCenter> Projects { get; set; }
        public List<BillRequisitionItemBoQMapModel> BoQItemProductMaps { get; set; }

        public string ProjectName { get; set; }
        public string DivisionName { get; set; }
        public string BoqName { get; set; }
        public string BoqNumber { get; set; }
        public string MaterialName { get; set; }
    }
}