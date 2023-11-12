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

        [Required]
        [Display(Name = "Select Manager")]
        public long EmployeeRowId { get; set; }
        public string EmployeeId { get; set; }

        public List<CostCenterManagerMap> CostCenterManagerMaps { get; set; }
        public List<Project> Projects { get; set; }
        public List<Employee> Employees { get; set; }
    }
}