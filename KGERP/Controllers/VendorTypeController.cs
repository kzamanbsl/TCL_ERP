using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System.Collections.Generic;
using System.Web.Mvc;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class VendorTypeController : BaseController
    {
        private readonly IVendorTypeService _service;
        public VendorTypeController(IVendorTypeService vendorTypeService)
        {
            _service = vendorTypeService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(VendorTypeModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                await _service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                await _service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                await _service.Delete(model.ID);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(Index), new { companyId = model.CompanyFK });
        }
    }
}