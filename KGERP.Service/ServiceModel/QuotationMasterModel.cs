using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class QuotationMasterModel : BaseVM
    {
        public long QuotationMasterId{ get; set; }
        public string QuotationNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public string QuotationFor { get; set; }
        public string QuotationName { get; set; }
        public long RequisitionMasterId { get; set; }
        public string Description { get; set; }
        public string EmployeeName { get; set; }
        public IEnumerable<QuotationMasterModel> DataList { get; set; } = new List<QuotationMasterModel>();
        public List<QuotationDetailModel> DetailDataList { get; set; } = new List<QuotationDetailModel>();
        public QuotationDetailModel DetailModel { get; set; } = new QuotationDetailModel();
        public IEnumerable<QuotationDetailModel> DetailList { get; set; } = new List<QuotationDetailModel>();
    }

    public class QuotationDetailModel : BaseVM
    {
        public long QuotationDetailId { get; set; }
        public long QuotationMasterId { get; set; }
        public string QuotationNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public string QuotationFor { get; set; }
        public long RequisitionMasterId { get; set; }
        public string QuotationName { get; set; }
        public string EmployeeName { get; set; }
    }

    public partial class QuotationCompareModel
    {
        public long QuotationDetailId { get; set; }
        public long QuotationMasterId { get; set; }
    }
}
