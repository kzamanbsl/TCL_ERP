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

namespace KGERP.Controllers
{
    [SessionExpire]
    public class BillRequisitionController : BaseController
    {
        private readonly IBillRequisitionService _service;
        public BillRequisitionController(IBillRequisitionService billRequisitionService)
        {
            _service = billRequisitionService;
        }

        #region Bill Requisition Item
        public ActionResult BillRequisitionItem(int companyId = 21)
        {
            //var viewData = new BillRequisitionTypeModel()
            //{
            //    BillRequisitionItems = _service.GetBillRequisitionItemList()
            //};
            return View();
        }
        #endregion

        #region Bill Requisition Type
        public ActionResult BillRequisitionType(int companyId = 21)
        {
            var viewData = new BillRequisitionTypeModel()
            {
                BillRequisitionTypes = _service.GetBillRequisitionTypeList()
            };
            return View(viewData);
        }

        [HttpPost]
        public ActionResult BillRequisitionType(BillRequisitionTypeModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                _service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                _service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                _service.Delete(model);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(BillRequisitionType), new { companyId = model.CompanyFK });
        }
        #endregion

        #region Cost Center Manager Map

        [HttpGet]
        public ActionResult CostCenterManagerMap(int companyId = 21)
        {
            var viewData = new CostCenterManagerMapModel()
            {
                Projects = _service.GetProjectList(),
                Employees = _service.GetEmployeeList(),
                CostCenterManagerMaps = _service.GetCostCenterManagerMapList(),
            };
            return View(viewData);
        }

        [HttpPost]
        public ActionResult CostCenterManagerMap(CostCenterManagerMapModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                _service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                _service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                _service.Delete(model.CostCenterManagerMapId);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(CostCenterManagerMap), new { companyId = model.CompanyFK });
        }

        #endregion

        #region  BillRequisition Circle


        [HttpGet]
        public async Task<ActionResult> BillRequisitionMasterSlave(int companyId = 0, long BillRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (BillRequisitionMasterId == 0)
            {
                billRequisitionMasterModel.CompanyFK = companyId;
                billRequisitionMasterModel.StatusId = EnumBillRequisitionStatus.Draft;
            }
            //else
            //{
            //    billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, BillRequisitionMasterId);

            //}
            //billRequisitionMasterModel.ZoneList = new SelectList(procurementService.ZonesDropDownList(companyId), "Value", "Text");
            //billRequisitionMasterModel.DamageTypeList = new SelectList(configurationService.DamageTypeDropDownList(companyId), "Value", "Text");
            //billRequisitionMasterModel.StockInfos = _stockInfoService.GetStockInfoSelectModels(companyId);
            return View(billRequisitionMasterModel);
        }


        #endregion

    }
}