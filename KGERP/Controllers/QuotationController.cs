using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Implementation.Procurement;
using KGERP.Service.Implementation;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using KGERP.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Description;
using DocumentFormat.OpenXml.EMMA;
using System.Linq;
using Remotion.Data.Linq;
using Ninject.Activation;
using KGERP.Data.CustomModel;
using KGERP.Data.Models;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class QuotationController : BaseController
    {
        private readonly IQuotationService _Service;
        private readonly ConfigurationService _ConfigurationService;
        private readonly ProductService _ProductService;
        private readonly IBillRequisitionService _RequisitionService;

        public QuotationController(IQuotationService quotationService, ConfigurationService configurationService, ProductService productService, IBillRequisitionService requisitionService)
        {
            _Service = quotationService;
            _ConfigurationService = configurationService;
            _ProductService = productService;
            _RequisitionService = requisitionService;
        }

        [HttpGet]
        public async Task<ActionResult> QuotationMasterSlave(int companyId = 0, long quotationMasterId = 0)
        {
            QuotationMasterModel viewData = new QuotationMasterModel();
            if (quotationMasterId == 0)
            {
                viewData.CompanyFK = companyId;
                viewData.StatusId = (int)EnumQuotationStatus.Draft;
                viewData.RequisitionList = new SelectList(_RequisitionService.ApprovedRequisitionList(companyId), "Value", "Text");
                viewData.DetailModel.MaterialTypeList = new SelectList(_ConfigurationService.GetAllProductCategoryList(companyId), "ProductCategoryId", "Name");
            }
            else
            {
                viewData = await _Service.GetQuotationMasterDetail(companyId, quotationMasterId);
            }
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> QuotationMasterSlave(QuotationMasterModel quotationMasterModel)
        {
            if (quotationMasterModel.ActionEum == ActionEnum.Add)
            {
                if (quotationMasterModel.QuotationMasterId == 0)
                {
                    quotationMasterModel.QuotationMasterId = await _Service.QuotationMasterAdd(quotationMasterModel);
                }
                await _Service.QuotationDetailAdd(quotationMasterModel);
            }
            else if (quotationMasterModel.ActionEum == ActionEnum.Edit)
            {
                await _Service.QuotationDetailEdit(quotationMasterModel);
            }
            return RedirectToAction(nameof(QuotationMasterSlave), new { companyId = quotationMasterModel.CompanyFK, quotationMasterId = quotationMasterModel.QuotationMasterId });
        }

        [HttpPost]
        public async Task<ActionResult> SubmitQuotationMasterSlave(QuotationMasterModel quotationMasterModel)
        {
            quotationMasterModel.QuotationMasterId = await _Service.SubmitQuotationMaster(quotationMasterModel.QuotationMasterId);
            return RedirectToAction(nameof(QuotationMasterSlave), new { companyId = quotationMasterModel.CompanyFK, quotationMasterId = quotationMasterModel.QuotationMasterId });
        }

        [HttpGet]
        public async Task<JsonResult> QuotationDetailById(long id)
        {
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteQuotationDetail(QuotationMasterModel quotationMasterModel)
        {
            if (quotationMasterModel.ActionEum == ActionEnum.Delete)
            {
                quotationMasterModel.DetailModel.QuotationDetailId = await _Service.QuotationDetailDelete(quotationMasterModel.DetailModel.QuotationDetailId);
            }
            return RedirectToAction(nameof(QuotationMasterSlave), new { companyId = quotationMasterModel.CompanyFK, quotationMasterId = quotationMasterModel.QuotationMasterId });
        }
    }
}