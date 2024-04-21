using KGERP.Data.Models;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class VendorTypeService : IVendorTypeService
    {
        private readonly ERPEntities _context;
        public VendorTypeService(ERPEntities eRPEntities)
        {
            _context = eRPEntities;
        }

        public async Task<bool> Add(VendorTypeModel model)
        {
            var response = false;

            if (model != null)
            {
                VendorType saveData = new VendorType()
                {
                    Name = model.Name,
                };

                _context.VendorTypes.Add(saveData);

                if (await _context.SaveChangesAsync() > 0)
                {
                    response = true;
                }
            }

            return response;
        }

        public async Task<bool> Edit(VendorTypeModel model)
        {
            var response = false;
            var getVendorType = _context.VendorTypes.FirstOrDefault(x => x.VendorTypeId == model.ID);

            if (getVendorType != null)
            {
                getVendorType.Name = model.Name;

                if (await _context.SaveChangesAsync() > 0)
                {
                    response = true;
                }
            }

            return response;
        }

        public Task<bool> Delete(long id)
        {
            throw new System.NotImplementedException();
        }

        public List<VendorTypeModel> GetVendorTypes()
        {
            IQueryable<Data.Models.VendorType> vendors = _context.VendorTypes.AsQueryable();
            return ObjectConverter<VendorType, VendorTypeModel>.ConvertList(vendors.ToList()).ToList();

        }

        public List<SelectModel> GetVendorTypeSelectModels()
        {
            return _context.VendorTypes.ToList().Select(x => new SelectModel()
            {
                Text = x.Name,
                Value = x.VendorTypeId
            }).ToList();
        }
    }
}
