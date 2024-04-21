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
            return View();
        }
    }
}