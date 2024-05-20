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
        public ActionResult MaterialCategoryUpload(int companyId = 0)
        {
            BulkUpload viewModel = new BulkUpload();
            viewModel.CompanyId = companyId;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult MaterialCategoryUpload(BulkUpload model)
        {
            if (model.FormFile != null)
            {
                var response = _service.ProductCategoryUploadAsync(model);
            }
            return RedirectToAction(nameof(MaterialCategoryUpload), new { companyId = 21 });
        }
    }
}