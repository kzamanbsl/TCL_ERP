﻿using KGERP.Service.Implementation.Configuration;
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
    public class ChequeRegisterController : BaseController
    {
        private readonly IChequeRegisterService _Service;
        private readonly IBillRequisitionService _RequisitionService;

        public ChequeRegisterController(IChequeRegisterService chequeRegisterService, IBillRequisitionService requisitionService)
        {
            _Service = chequeRegisterService;
            _RequisitionService = requisitionService;
        }

        [HttpGet]
        public async Task<ActionResult> NewChequeRegister(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.ProjectList = new SelectList(await _RequisitionService.GetProjectList(companyId), "CostCenterId", "Name");
            viewData.RequisitionList = new SelectList(_RequisitionService.ApprovedRequisitionList(companyId), "Value", "Text");
            viewData.ChequeRegisterList = await _Service.GetChequeRegisterList(companyId);
            return View(viewData);
        }

        [HttpPost]
        public ActionResult NewChequeRegister(ChequeRegisterModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                _Service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                _Service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                _Service.Delete(model);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(NewChequeRegister), new { companyId = model.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> ChequeSigning(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.ChequeRegisterList = await _Service.GetChequeRegisterList(companyId);
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> ChequeRegisterSearch(ChequeRegisterModel model)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = model.CompanyFK;
            viewData.ChequeRegisterList = await _Service.GetChequeRegisterListByDate(model);
            TempData["ChequeRegisterModel"] = viewData;

            return RedirectToAction(nameof(ChequeSigning), new { companyId = model.CompanyFK, fromDate = model.StrFromDate, ToDate = model.StrToDate});
        }

        [HttpGet]
        public async Task<ActionResult> ChequeRegisterReport(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.ProjectList = new SelectList(await _RequisitionService.GetProjectList(companyId), "CostCenterId", "Name");
            viewData.RequisitionList = new SelectList(_RequisitionService.ApprovedRequisitionList(companyId), "Value", "Text");
            return View(viewData);
        }

        [HttpGet]
        public async Task<JsonResult> MakeSignToCheque(long chequeRegisterId)
        {
            bool response = false;
            if (chequeRegisterId > 0)
            {
                response = await _Service.ChequeSign(chequeRegisterId);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ChequeRegisterById(long chequeRegisterId)
        {
            var data = await _Service.GetChequeRegisterById(chequeRegisterId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}