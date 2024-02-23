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
        public ActionResult Index(int companyId = 0)
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewChequeRegister(int companyId = 0)
        {
            return View();
        }
    }
}