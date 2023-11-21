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
        public int CostCenterTypeId { get; set; }
        public int CostCenterId { get; set; }
        public decimal BoQAmount { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public List<BillBoQItem> BillBoQItems { get; set; }
        public List<Accounting_CostCenter> Accounting_CostCenters { get; set; }
        public List<Accounting_CostCenterType> Accounting_CostCenterTypes { get; set; }
    }
}