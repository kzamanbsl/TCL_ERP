using KGERP.Utility;
using System.Web;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class BulkUpload
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public HttpPostedFileBase FormFile { get; set; }
        public long RequisitionStatus { get; set; }
        public int? Status { get; set; }
        public int UploadType { get; set; }
        public SelectList UploadTypeList { get { return new SelectList(BaseFunctionalities.GetEnumList<EnumMaterialBulkUploadType>(), "Value", "Text"); } }
        public SelectList RequisitionStatusList { get { return new SelectList(BaseFunctionalities.GetEnumList<EnumBillRequisitionStatus>(), "Value", "Text"); } }
    }
}
