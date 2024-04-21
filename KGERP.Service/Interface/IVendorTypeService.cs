using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public interface IVendorTypeService
    {
        Task<bool> Add(VendorTypeModel model);
        Task<bool> Edit(VendorTypeModel model);
        Task<bool> Delete(long id);
        List<VendorTypeModel> GetVendorTypes();
        List<SelectModel> GetVendorTypeSelectModels();
    }
}
