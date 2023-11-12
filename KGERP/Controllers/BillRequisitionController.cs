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
        private readonly IBillRequisitionService _billRequisitionService;
        public BillRequisitionController(IBillRequisitionService billRequisitionService)
        {
            _billRequisitionService = billRequisitionService;
        }

        #region Cost Center Manager Map

        [HttpGet]
        public ActionResult CostCenterManagerMap(int companyId = 21)
        {
            var viewData = new CostCenterManagerMapModel()
            {
                Projects = _billRequisitionService.GetProjectList(),
                Employees = _billRequisitionService.GetEmployeeList(),
                CostCenterManagerMaps = _billRequisitionService.GetCostCenterManagerMapList(),
            };
            return View(viewData);
        }
   
        [HttpPost]
        public ActionResult CostCenterManagerMap(CostCenterManagerMapModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                _billRequisitionService.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                _billRequisitionService.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                _billRequisitionService.Delete(model.CostCenterManagerMapId);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(CostCenterManagerMap), new { companyId = model.CompanyFK });
        }

        #endregion
    }

}