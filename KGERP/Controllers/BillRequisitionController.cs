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

        [SessionExpire]
        [HttpGet]
        public ActionResult Index(int companyId = 21)
        {
            return View();
        }

        #region Cost Center Manager Map
            [SessionExpire]
            [HttpGet]
            public ActionResult CostCenterManagerMap(int companyId = 21)
            {
                return View();
            }

            [SessionExpire]
            [HttpPost]
            public ActionResult CostCenterManagerMap(CostCenterManagerMapModel costCenterManagerMapModel)
            {
                return View();
            }
        #endregion
    }

}