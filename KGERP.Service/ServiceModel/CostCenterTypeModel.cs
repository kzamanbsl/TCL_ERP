using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KGERP.Service.ServiceModel
{
    public class CostCenterTypeModel : BaseVM
    {
        public int CostCenterTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public List<Accounting_CostCenterType> CostCenterTypes { get; set; }
    }
}