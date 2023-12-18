using KGERP.Data.Models;
using System;
using System.Collections.Generic;
namespace KG.Core.Services.Configuration
{
    public class VmUserActionLog
    {
        public long UserLogId { get; set; }
        public int ActionType { get; set; }
        public long EmployeeId { get; set; }
        public string EmpUserId { get; set; }
        public string EmployeeName { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime ActionTimeStamp { get; set; }
        public string Details { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public List<VmUserActionLog> DataList { get; set; }
        public virtual Company Company { get; set; }
        public virtual Employee Employee { get; set; }
    }
}