using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using KGERP.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

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
        [SessionExpire]
        [HttpGet]
        public ActionResult CostCenterManagerMap(int companyId = 0)
        {
            var viewData = new CostCenterManagerMapModel()
            {
                Projects = _billRequisitionService.GetProjectList(),
                Employees = _billRequisitionService.GetEmployeeList(),
                CostCenterManagerMaps = _billRequisitionService.GetCostCenterManagerMapList(),
            };
            return View(viewData);
        }

        [SessionExpire]
        [HttpPost]
        public ActionResult CostCenterManagerMap(CostCenterManagerMapModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _billRequisitionService.Add(model);

                if (result)
                {
                    TempData["Message"] = "Project manager assigned successfully!";
                    return RedirectToAction("CostCenterManagerMap");
                }

                TempData["Message"] = "Something problem inside me! Please try again.";
                return View(model);
            }

            return View(model);
        }
        #endregion
    }

}