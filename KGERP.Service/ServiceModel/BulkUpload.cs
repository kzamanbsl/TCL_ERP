using System.Web;

namespace KGERP.Service.ServiceModel
{
    public class BulkUpload
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public HttpPostedFileBase FormFile { get; set; }
    }
}
