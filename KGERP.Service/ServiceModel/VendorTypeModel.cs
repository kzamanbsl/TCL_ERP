using KGERP.Service.Implementation.Configuration;
using System.Collections.Generic;

namespace KGERP.Service.ServiceModel
{
    public class VendorTypeModel : BaseVM
    {
        public int VendorTypeId { get; set; }
        public string Name { get; set; }
        public List<VendorTypeModel> DataList { get; set; }
    }
}
