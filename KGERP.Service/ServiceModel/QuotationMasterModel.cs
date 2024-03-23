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
        public long QuotationMasterId { get; set; }
        public string QuotationNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public int QuotationFor { get; set; }
        public string QuotationName { get; set; }
        public string QuotationNameWitNo { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int StatusId { get; set; }
        public long RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public string Description { get; set; }
        public string EmployeeName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime QuotationFromDate { get; set; }
        public DateTime QuotationToDate { get; set; }
        public SelectList QuotationForList { get; set; } = new SelectList(Enum.GetValues(typeof(EnumQuotationFor)).Cast<EnumQuotationFor>().Select(e => new SelectListItem { Text = e.ToString(), Value = ((int)e).ToString() }), "Value", "Text");
        public SelectList RequisitionList { get; set; } = new SelectList(new List<object>());
        public IEnumerable<QuotationMasterModel> DataList { get; set; } = new List<QuotationMasterModel>();
        public List<QuotationDetailModel> DetailDataList { get; set; } = new List<QuotationDetailModel>();
        public QuotationDetailModel DetailModel { get; set; } = new QuotationDetailModel();
        public IEnumerable<QuotationDetailModel> DetailList { get; set; } = new List<QuotationDetailModel>();
    }

    public class QuotationDetailModel : BaseVM
    {
        public long QuotationDetailId { get; set; }
        public long QuotationMasterId { get; set; }
        public long MaterialId { get; set; }
        public string MaterialName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public int MaterialQualityId { get; set; }
        public int MaterialTypeId { get; set; }
        public string MaterialTypeName { get; set; }
        public int MaterialSubtypeId { get; set; }
        public string MaterialSubtypeName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string EmployeeName { get; set; }
        public SelectList MaterialTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList MaterialSubtypeList { get; set; } = new SelectList(new List<object>());
        public SelectList MaterialList { get; set; } = new SelectList(new List<object>());
        public SelectList MaterialQualityList { get; set; } = new SelectList(Enum.GetValues(typeof(EnumMaterialQuality)).Cast<EnumMaterialQuality>().Select(e => new SelectListItem { Text = e.ToString(), Value = ((int)e).ToString() }), "Value", "Text");
    }

    public partial class QuotationCompareModel : BaseVM
    {
        public long QuotationDetailId { get; set; }
        public long QuotationMasterId { get; set; }
        public long QuotationIdOne { get; set; }
        public long QuotationIdTwo { get; set; }
        public string[] HeadingName { get; set; }
        public SelectList QuotationList { get; set; } = new SelectList(new List<object>());
        public List<QuotationMasterModel> QuotationMasterModel { get; set; } = new List<QuotationMasterModel>();
    }
}
