//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KGERP.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LeaveApplicationDetail
    {
        public long LeaveApplicationDetailId { get; set; }
        public Nullable<long> LeaveApplicationId { get; set; }
        public Nullable<System.DateTime> LeaveDate { get; set; }
        public string LeaveYear { get; set; }
    
        public virtual LeaveApplication LeaveApplication { get; set; }
    }
}
