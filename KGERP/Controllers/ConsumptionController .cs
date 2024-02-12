using KGERP.Data.Models;
using KGERP.Service.Implementation;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Implementation.Procurement;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class ConsumptionController : BaseController
    {
        private readonly IConsumptionService _service;
        private readonly ConfigurationService _configurationService;
        private readonly IBillRequisitionService _billRequisitionService;
        private readonly IStockInfoService _stockInfoService;

        public ConsumptionController(IConsumptionService consumptionService, ConfigurationService configurationService, IBillRequisitionService billRequisitionService, IStockInfoService stockInfoService)
        {
            _service = consumptionService;
            _configurationService = configurationService;
            _billRequisitionService = billRequisitionService;
            _stockInfoService = stockInfoService;
        }

        [HttpGet]
        public async Task<ActionResult> ConsumptionMasterSlave(int companyId, int consumptionMasterId = 0)
        {
            //'BillRequisitionTypeId'
            ConsumptionModel consumptionModel = new ConsumptionModel();
            if (consumptionMasterId > 0)
            {
                consumptionModel = await _service.GetConsumptionMasterDetail(consumptionMasterId);
            }
            else
            {

            }


            //consumptionModel.ProjectTypeList = new SelectList(await _service.GetCostCenterTypeList(companyId), "CostCenterTypeId", "Name");
            //consumptionModel.StatusId = (int)EnumBillRequisitionStatus.Draft;
            consumptionModel.ProjectTypeList = new SelectList(await _billRequisitionService.GetCostCenterTypeList(companyId), "CostCenterTypeId", "Name");
            consumptionModel.BOQDivisionList = new SelectList(await _billRequisitionService.BoQDivisionList(companyId), "BoQDivisionId", "Name");
            consumptionModel.BOQItemList = new SelectList(_billRequisitionService.GetBillOfQuotationList(), "BoQItemId", "BoQNumber");
            consumptionModel.MaterialTypeList = new SelectList(_configurationService.GetAllProductCategoryList(companyId));
            //consumptionModel.MaterialSubTypeList = new SelectList(_configurationService.GetAllProductSubCategoryList(companyId));
            consumptionModel.ProjectList = new SelectList(_configurationService.GetAllProductSubCategoryList(companyId));
            consumptionModel.StockInfoList = new SelectList(_stockInfoService.GetStockInfoSelectModels(companyId), "Value","Text");
            //consumptionModel.ActionId = (int)ActionEnum.Add;
            return View(consumptionModel);

        }

        [HttpPost]
        public async Task<ActionResult> ConsumptionMasterSlave(ConsumptionModel consumptionModel)
        {
            BillRequisitionMasterModel billRequisition = new BillRequisitionMasterModel();
            if (consumptionModel.ActionId == (int)ActionEnum.Add)
            {
                if (consumptionModel.ConsumptionMasterId == 0)
                {
                    consumptionModel.ConsumptionMasterId = (int)await _service.CreateConsumptionMaster(consumptionModel);
                }
                await _service.CreateConsumptionDetail(consumptionModel);
            }
            if (consumptionModel.ActionId == (int)ActionEnum.Edit)
            {

                await _service.UpdateConsumptionDetail(consumptionModel);
            }

            return RedirectToAction(nameof(ConsumptionMasterSlave), new { companyId = CompanyInfo.CompanyId, consumptionMasterId = consumptionModel.ConsumptionMasterId });
        }
    }
}