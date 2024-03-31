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
using System.Windows.Media;

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

        #region Quotation Type
        [HttpGet]
        public async Task<ActionResult> QuotationFor(int companyId = 0)
        {
            QuotationForModel viewData = new QuotationForModel();
            viewData.CompanyFK = companyId;
            viewData.QuotationForList = await _Service.GetQuotationForList(companyId);
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> QuotationFor(QuotationForModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                // Add 
                await _Service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                // Edit
                await _Service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                // Delete
                await _Service.Delete(model);
            }
            else
            {
                return View("Error");
            }

            return RedirectToAction(nameof(QuotationFor), new { companyId = model.CompanyFK });
        }
        #endregion

        #region Quotation CRUD
        [HttpGet]
        public async Task<ActionResult> QuotationMasterSlave(int companyId = 0, long quotationMasterId = 0)
        {
            QuotationMasterModel viewData = new QuotationMasterModel();
            if (quotationMasterId == 0)
            {
                viewData.CompanyFK = companyId;
                viewData.StatusId = (int)EnumQuotationStatus.Draft;
                viewData.RequisitionList = new SelectList(_RequisitionService.ApprovedRequisitionList(companyId), "Value", "Text");
                viewData.QuotationForList = new SelectList(await _Service.GetQuotationForList(companyId), "QuotationForId", "Name");
            }
            else
            {
                viewData = await _Service.GetQuotationMasterDetail(companyId, quotationMasterId);
            }
            viewData.DetailModel.MaterialTypeList = new SelectList(_ConfigurationService.GetAllProductCategoryList(companyId), "ProductCategoryId", "Name");
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
                quotationMasterModel.QuotationMasterId = _Service.QuotationDetailEdit(quotationMasterModel);
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
        public async Task<JsonResult> GetQuotationDetailById(long id)
        {
            var data = await _Service.QuotationDetailBbyId(id);
            return Json(data, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public async Task<ActionResult> DeleteQuotationMaster(QuotationMasterModel quotationMasterModel)
        {
            await _Service.QuotationMasterDelete(quotationMasterModel.QuotationMasterId);
            return RedirectToAction(nameof(GetQuotationDetail), new { companyId = quotationMasterModel.CompanyFK });
        }
        #endregion

        #region Quotation List
        [HttpGet]
        public async Task<ActionResult> GetQuotationList(int companyId)
        {
            QuotationMasterModel viewData = new QuotationMasterModel();
            viewData = await _Service.GetQuotationList();
            viewData.CompanyFK = companyId;
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> QuotationListSearch(QuotationMasterModel model)
        {
            QuotationMasterModel viewData = new QuotationMasterModel();
            viewData.CompanyFK = model.CompanyFK;
            viewData = await _Service.GetQuotationListByDate(model);
            TempData["QuotationModel"] = viewData;

            return RedirectToAction(nameof(GetQuotationList), new { companyId = viewData.CompanyFK, fromDate = model.QuotationFromDate, ToDate = model.QuotationToDate });
        }

        [HttpGet]
        public ActionResult GetQuotationDetail(int companyId)
        {
            QuotationMasterModel viewData = new QuotationMasterModel();
            //viewData = await _Service.GetQuotationListFilterByStatus();
            viewData.CompanyFK = companyId;
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> QuotationDetailSearch(QuotationMasterModel model)
        {
            QuotationMasterModel viewData = new QuotationMasterModel();
            viewData.CompanyFK = model.CompanyFK;
            viewData = await _Service.GetQuotationListByDateAndStatus(model);
            TempData["QuotationModel"] = viewData;

            return RedirectToAction(nameof(GetQuotationDetail), new { companyId = viewData.CompanyFK, fromDate = model.QuotationFromDate, ToDate = model.QuotationToDate });
        }

        [HttpPost]
        public async Task<ActionResult> QuotationOpen(QuotationMasterModel model)
        {
            QuotationMasterModel viewData = new QuotationMasterModel();
            viewData.CompanyFK = model.CompanyFK;
            var result = await _Service.OpenQuotationById(model.QuotationMasterId);
            return RedirectToAction(nameof(GetQuotationDetail), new { companyId = viewData.CompanyFK, fromDate = model.QuotationFromDate, ToDate = model.QuotationToDate });
        }

        [HttpPost]
        public async Task<ActionResult> QuotationClose(QuotationMasterModel model)
        {
            QuotationMasterModel viewData = new QuotationMasterModel();
            viewData.CompanyFK = model.CompanyFK;
            var result = await _Service.CloseQuotationById(model.QuotationMasterId);
            return RedirectToAction(nameof(GetQuotationDetail), new { companyId = viewData.CompanyFK, fromDate = model.QuotationFromDate, ToDate = model.QuotationToDate });
        }
        #endregion

        #region Quotation Submit
        [HttpGet]
        public async Task<ActionResult> QuotationSubmit(int companyId, long quotationSubmitId = 0)
        {
            QuotationSubmitModel viewModel = new QuotationSubmitModel();
            viewModel.CompanyFK = companyId;
            viewModel.QuotationForList = new SelectList(await _Service.GetQuotationForList(companyId), "QuotationForId", "Name");
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult QuotationSubmit(QuotationSubmitModel model)
        {
            QuotationSubmitModel viewModel = new QuotationSubmitModel();
            return RedirectToAction(nameof(QuotationSubmit), new { companyId = 21, quotationSubmitId = 3 });
        }

        [HttpGet]
        public JsonResult GetQuotationListFilteredByTypeAndFor(int typeId, long forId)
        {
            var data = _Service.QuotationListByTypeIdAndForId(typeId, forId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Comparative Statement
        [HttpGet]
        public async Task<ActionResult> ComparativeStatement(int companyId)
        {
            QuotationCompareModel viewData = new QuotationCompareModel();
            viewData.CompanyFK = companyId;
            viewData.QuotationList = new SelectList(await _Service.GetQuotationListWithNameAndNo(), "QuotationMasterId", "QuotationNameWitNo");
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> CompareQuotation(QuotationCompareModel model)
        {
            QuotationCompareModel viewData = new QuotationCompareModel();
            viewData = await _Service.GetComparedQuotation(model.QuotationIdOne, model.QuotationIdTwo);
            TempData["CompareModel"] = viewData;
            return RedirectToAction(nameof(ComparativeStatement), new { companyId = 21, QuotationIdOne = model.QuotationIdOne, QuotationIdTwo = model.QuotationIdTwo });
        }
        #endregion
    }
}