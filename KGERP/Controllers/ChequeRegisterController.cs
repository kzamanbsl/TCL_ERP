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
        public async Task<ActionResult> Index(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.chequeRegisterList = await _Service.GetChequeRegisterList(companyId);
            return View(viewData);
        }

        [HttpGet]
        public async Task<ActionResult> NewChequeRegister(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.ProjectList = new SelectList(await _RequisitionService.GetProjectList(companyId), "CostCenterId", "Name");
            viewData.RequisitionList = new SelectList(_RequisitionService.ApprovedRequisitionList(companyId), "Value", "Text");
            viewData.chequeRegisterList = await _Service.GetChequeRegisterList(companyId);
            return View(viewData);
        }
    }
}