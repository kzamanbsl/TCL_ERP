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
        public ActionResult QuotationMasterSlave(int companyId = 0, long QuotationMasterId = 0)
        {
            QuotationMasterModel viewData = new QuotationMasterModel();
            viewData.CompanyFK = companyId;
            viewData.StatusId = (int)EnumQuotationStatus.Draft;
            viewData.RequisitionList = new SelectList(_RequisitionService.ApprovedRequisitionList(companyId), "Value", "Text");
            viewData.DetailModel.MaterialTypeList = new SelectList(_ConfigurationService.GetAllProductCategoryList(companyId), "ProductCategoryId", "Name");
            return View(viewData);
        }
    }
}