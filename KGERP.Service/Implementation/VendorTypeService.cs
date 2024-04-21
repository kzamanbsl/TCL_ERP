using KGERP.Data.Models;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
                    CreatedBy = HttpContext.Current.User.Identity.Name,
                    CreatedOn = DateTime.Now,
                    IsActive = true
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
                getVendorType.ModifiedBy = HttpContext.Current.User.Identity.Name;
                getVendorType.ModifiedOn = DateTime.Now;

                if (await _context.SaveChangesAsync() > 0)
                {
                    response = true;
                }
            }

            return response;
        }

        public async Task<bool> Delete(long id)
        {
            var response = false;
            var getVendorType = _context.VendorTypes.FirstOrDefault(x => x.VendorTypeId == id);

            if (getVendorType != null)
            {
                getVendorType.IsActive = false;
                getVendorType.ModifiedBy = HttpContext.Current.User.Identity.Name;
                getVendorType.ModifiedOn = DateTime.Now;

                if (await _context.SaveChangesAsync() > 0)
                {
                    response = true;
                }
            }

            return response;
        }

        public List<VendorTypeModel> GetVendorTypes()
        {
            var response = (from t1 in _context.VendorTypes
                            where t1.IsActive
                            select new VendorTypeModel
                            {
                                VendorTypeId = t1.VendorTypeId,
                                Name = t1.Name
                            }).ToList();
            return response;
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
