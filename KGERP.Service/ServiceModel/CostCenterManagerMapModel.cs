using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KGERP.Service.ServiceModel
{
    public class CostCenterManagerMapModel : BaseVM
    {
        public int CostCenterManagerMapId { get; set; }

        [Required]
        [Display(Name = "Select Project")]
        public int ProjectId { get; set; }
        public string ProjecName { get; set; }

        [Required]
        [Display(Name = "Select Manager")]
        public long EmployeeRowId { get; set; }
        public string EmployeeId { get; set; }

        public List<Employee> Employees { get; set; }
        public List<Accounting_CostCenter> Projects { get; set; }
        public List<CostCenterManagerMap> CostCenterManagerMaps { get; set; }
    }
}