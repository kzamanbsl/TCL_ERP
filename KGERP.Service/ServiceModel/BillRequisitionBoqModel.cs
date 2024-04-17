using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class BillRequisitionBoqModel : BaseVM
    {
        public int BoQItemId { get; set; }
        public string BoQNumber { get; set; }
        public string Name { get; set; }
        public decimal BoqQuantity { get; set; }
        public string Description { get; set; }
        public int BoqUnitId { get; set; }
        public long BoQDivisionId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string BoqDivisionName { get; set; }
        public string BoqUnitName { get; set; }
        public decimal Amount { get; set; }
        public List<Unit> BoQUnits { get; set; }
        public List<BillRequisitionBoqModel> BillBoQItems { get; set; }
        public List<BoqDivisionModel> BoQDivisions { get; set; }
        public List<Accounting_CostCenter> Projects { get; set; }
    }
}