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
using DocumentFormat.OpenXml.Spreadsheet;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class ConsumptionController : BaseController
    {
        private readonly IConsumptionService _service;
        private readonly ConfigurationService _configurationService;
        private readonly IBillRequisitionService _billRequisitionService;

        public ConsumptionController(IConsumptionService consumptionService, ConfigurationService configurationService, IBillRequisitionService billRequisitionService)
        {
            _service = consumptionService;
            _configurationService = configurationService;
            _billRequisitionService = billRequisitionService;
        }

        [HttpGet]
       public async Task<ActionResult> ConsumptionMasterSlave(int companyId)
        {
            ConsumptionModel consumptionModel = new ConsumptionModel();
            //consumptionModel.ProjectTypeList = new SelectList(await _service.GetCostCenterTypeList(companyId), "CostCenterTypeId", "Name");
            //consumptionModel.StatusId = (int)EnumBillRequisitionStatus.Draft;
            consumptionModel.ProjectTypeList = new SelectList(await _billRequisitionService.GetCostCenterTypeList(companyId), "CostCenterTypeId", "Name");
            consumptionModel.BOQDivisionList = new SelectList(_billRequisitionService.BoQDivisionList(), "BoQDivisionId", "Name");
            consumptionModel.BOQItemList =new SelectList  (_billRequisitionService.GetBillOfQuotationList(), "BoQItemId", "BoQNumber") ;
            consumptionModel.MaterialTypeList = new SelectList(_configurationService.GetAllProductCategoryList(companyId));
            consumptionModel.MaterialTypeList = new SelectList(_configurationService.GetAllProductSubCategoryList(companyId));
            return View(consumptionModel);
        }

        [HttpPost]
        public async Task<ActionResult> ConsumptionMasterSlave(ConsumptionModel consumptionModel)
        {
            BillRequisitionMasterModel billRequisition = new BillRequisitionMasterModel();
            //billRequisition.ProjectTypeList = new SelectList(await _service.GetCostCenterTypeList(CompanyInfo.CompanyId), "CostCenterTypeId", "Name");

            return View(billRequisition);
        }

    }
}