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
    
    public partial class Education
    {
        public int EducationId { get; set; }
        public Nullable<long> Id { get; set; }
        public Nullable<int> ExaminationId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public Nullable<int> InstituteId { get; set; }
        public string PassingYear { get; set; }
        public string RollNo { get; set; }
        public string RegNo { get; set; }
        public string Result { get; set; }
        public string Remarks { get; set; }
        public string CertificateName { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual DropDownItem DropDownItem { get; set; }
        public virtual DropDownItem DropDownItem1 { get; set; }
        public virtual DropDownItem DropDownItem2 { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
