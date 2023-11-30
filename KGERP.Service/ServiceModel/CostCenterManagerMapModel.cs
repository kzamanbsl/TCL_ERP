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
        public int ProjectId { get; set; }
        [Required]
        public long EmployeeRowId { get; set; }

        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }

        public List<Employee> Employees { get; set; }
        public List<Accounting_CostCenter> Projects { get; set; }
        public List<CostCenterManagerMapModel> CostCenterManagerMapModels { get; set; }
    }
}