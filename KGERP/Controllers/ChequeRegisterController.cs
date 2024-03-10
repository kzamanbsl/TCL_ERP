using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        [HttpGet]
        public async Task<ActionResult> ChequePrinting(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.ChequeRegisterList = await _Service.GetSignedChequeList(companyId);
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

        [HttpGet]
        public JsonResult  RequisitionListWithFilter(int projectId)
        {
            var data = _RequisitionService.FilteredApprovedRequisitionList(projectId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RegisteredRequisitionListByProjectId(int projectId)
        {
            var data = _Service.RegisteredRequisitionList(projectId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetSupplierPayeeName(int supplierId)
        {
            var data = await _Service.GetPayeeNameBySupplierId(supplierId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Add Cheque Book

        public async Task<ActionResult> NewChequeBook(int companyId = 0)
        {
            ChequeBookModel viewData = new ChequeBookModel();
            viewData.CompanyFK = companyId;
            return View(viewData);
        }

        [HttpPost]
        public ActionResult NewChequeBook(ChequeBookModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                //_Service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                //_Service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                //_Service.Delete(model);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(NewChequeRegister), new { companyId = model.CompanyFK });
        }

        #endregion
    }
}