﻿using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System.Collections.Generic;
using System.Web.Mvc;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class WorkStateController : BaseController
    {
        private readonly IWorkStateService workStateService;
        public WorkStateController(IWorkStateService workStateService)
        {
            this.workStateService = workStateService;
        }
        [HttpGet]
        [SessionExpire]
        public ActionResult Index()
        {
            List<WorkStateModel> models = workStateService.GetWorkStates();
            return View(models);
        }

        [HttpGet]
        [SessionExpire]
        public ActionResult CreateOrEdit(int id)
        {
            WorkStateModel model = workStateService.GetWorkState(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult CreateOrEdit(WorkStateModel model)
        {
            bool result = false;
            if (model.WorkStateId <= 0)
            {
                result = workStateService.SaveWorkState(0, model);
            }
            else
            {
                result = workStateService.SaveWorkState(model.WorkStateId, model);
            }

            if (result)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}