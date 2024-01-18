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
using System.Web.UI.WebControls;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class ConsumptionController : BaseController
    {
        private readonly IBillRequisitionService _service;
        private readonly ConfigurationService _configurationService;
        private readonly ProductService _ProductService;

        public ConsumptionController(IBillRequisitionService billRequisitionService, ConfigurationService configurationService, ProductService productService)
        {
            _service = billRequisitionService;
            _configurationService = configurationService;
            _ProductService = productService;
        }

       public ActionResult ConsumptionMasterSlave()
        {
            BillRequisitionMasterModel billRequisition = new BillRequisitionMasterModel();

            return View(billRequisition);
        }

    }
}