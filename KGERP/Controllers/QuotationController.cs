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
        private readonly IQuotationService _service;
        private readonly ConfigurationService _configurationService;
        private readonly ProductService _ProductService;

        public QuotationController(IQuotationService quotationService, ConfigurationService configurationService, ProductService productService)
        {
            _service = quotationService;
            _configurationService = configurationService;
            _ProductService = productService;
        }

        [HttpGet]
        public ActionResult QuotationMasterSlave(int companyId = 0, long QuotationMasterId = 0)
        {
            return View();
        }
    }
}