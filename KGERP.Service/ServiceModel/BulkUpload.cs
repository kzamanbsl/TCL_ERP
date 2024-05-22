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
        public int RequisitionStatus { get; set; }
        public SelectList RequisitionStatusList { get { return new SelectList(BaseFunctionalities.GetEnumList<EnumBillRequisitionStatus>(), "Value", "Text"); } }
    }
}
