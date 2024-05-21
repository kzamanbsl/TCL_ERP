using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using KGERP.Service.Implementation;
using KGERP.Service.Implementation.FTP;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class BulkUploadController : Controller
    {
        private BulkUploadService _service;
        public BulkUploadController(BulkUploadService bulkUploadService)
        {
            this._service = bulkUploadService;
        }

        [HttpGet]
        public ActionResult MaterialCategoryUpload(int companyId = 0, bool? result = null)
        {
            BulkUpload viewModel = new BulkUpload();
            viewModel.CompanyId = companyId;
            ViewBag.Result = result;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult MaterialCategoryUpload(BulkUpload model)
        {
            bool response = false;
            if (model.FormFile != null)
            {
                response = _service.ProductCategoryUpload(model);
            }
            ViewBag.Result = response;
            return RedirectToAction(nameof(MaterialCategoryUpload), new { companyId = model.CompanyId, result = response });
        }
    }
}