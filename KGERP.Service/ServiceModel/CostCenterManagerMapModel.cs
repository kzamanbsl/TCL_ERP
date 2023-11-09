using KGERP.Data.Models;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KGERP.Service.ServiceModel
{
    public class CostCenterManagerMapModel
    {
        public int CostCenterManagerMapId { get; set; }
        public int ProjectId { get; set; }
        public string EmployeeId { get; set; }
        public List<Project> Projects { get; set; }
        public List<Employee> Employees { get; set; }
    }
}