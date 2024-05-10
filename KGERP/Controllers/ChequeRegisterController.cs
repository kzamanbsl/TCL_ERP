using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class ChequeRegisterController : BaseController
    {
        private readonly IChequeRegisterService _Service;
        private readonly IBillRequisitionService _RequisitionService;
        private readonly ConfigurationService _ConfigurationService;

        public ChequeRegisterController(IChequeRegisterService chequeRegisterService, IBillRequisitionService requisitionService, ConfigurationService configurationService)
        {
            _Service = chequeRegisterService;
            _RequisitionService = requisitionService;
            _ConfigurationService = configurationService;
        }

        #region Bank A/C Information

        public async Task<ActionResult> BankAccountInformation(int companyId)
        {
            BankAccountInfoModel viewData = new BankAccountInfoModel();
            viewData.CompanyFK = companyId;
            viewData.BankList = new SelectList(_ConfigurationService.CommonBanksDropDownList(companyId), "Value", "Text");
            viewData.BankAccountInfoList = await _Service.GetBankAccountInfoList(companyId);
            return View(viewData);
        }

        [HttpPost]
        public ActionResult BankAccountInformation(BankAccountInfoModel model)
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
            return RedirectToAction(nameof(BankAccountInformation), new { companyId = model.CompanyFK });
        }

        [HttpGet]
        public async Task<JsonResult> BankAccountInfoById(long bankAccountInfoId)
        {
            var data = await _Service.GetBankAccountInfoById(bankAccountInfoId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cheque Book Register

        public async Task<ActionResult> NewChequeBook(int companyId)
        {
            ChequeBookModel viewData = new ChequeBookModel();
            viewData.CompanyFK = companyId;
            viewData.BankList = new SelectList(_ConfigurationService.CommonBanksDropDownList(companyId), "Value", "Text");
            viewData.ChequeBookList = await _Service.GetChequeBookList(companyId);
            return View(viewData);
        }

        [HttpPost]
        public ActionResult NewChequeBook(ChequeBookModel model)
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
            return RedirectToAction(nameof(NewChequeBook), new { companyId = model.CompanyFK });
        }

        [HttpGet]
        public async Task<JsonResult> ChequeBookById(long chequeBookId)
        {
            var data = await _Service.GetChequeBookById(chequeBookId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBranchBybankId(int bankId)
        {
            var data = _ConfigurationService.GetBankBranchesById(bankId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAccountNoByBankBranchId(int bankId, int bankBranchId)
        {
            var data = _ConfigurationService.GetBankAccountInfoByBankBranchId(bankId, bankBranchId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetChequeBookListByAccountInfo(int bankAccountInfoId)
        {
            var data = await _Service.GetChequeBookListByAccountInfo(bankAccountInfoId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetChequeBookInfoById(long chequeBookId)
        {
            var data = await _Service.GetChequeBookInfo(chequeBookId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cheque Register

        [HttpGet]
        public async Task<ActionResult> NewChequeRegister(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.BankList = new SelectList(_ConfigurationService.CommonBanksDropDownList(companyId), "Value", "Text");
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
        public async Task<JsonResult> ChequeRegisterById(long chequeRegisterId)
        {
            var data = await _Service.GetChequeRegisterById(chequeRegisterId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ChequeRegisterReport(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.ProjectList = new SelectList(await _RequisitionService.GetProjectList(companyId), "CostCenterId", "Name");
            return View(viewData);
        }

        #endregion

        #region Cheque Genearte

        [HttpGet]
        public async Task<ActionResult> ChequeGenerate(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.ChequeRegisterList = await _Service.GetChequeRegisterList(companyId);
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> ChequeRegisterSearchForGenearte(ChequeRegisterModel model)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = model.CompanyFK;
            viewData.ChequeRegisterList = await _Service.GetChequeRegisterListByDate(model);
            TempData["ChequeRegisterModel"] = viewData;

            return RedirectToAction(nameof(ChequeGenerate), new { companyId = model.CompanyFK, fromDate = model.StrFromDate, ToDate = model.StrToDate });
        }

        [HttpGet]
        public async Task<JsonResult> ChequePdfGenerate(long chequeRegisterId, long chequeBookId, int chequeNo)
        {
            bool response = false;
            if (chequeRegisterId > 0)
            {
                response = await _Service.MakePdf(chequeRegisterId, chequeBookId, chequeNo);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckBookByACInfo(long chequeRegisterId)
        {
            object response = new { };
            if (chequeRegisterId > 0)
            {
                response = _Service.GetCheckBookByACInfo(chequeRegisterId);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cheque Printing

        [HttpGet]
        public async Task<ActionResult> ChequePrinting(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.ChequeRegisterList = await _Service.GetGeneratedChequeList(companyId);
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> ChequeRegisterSearchForPrint(ChequeRegisterModel model)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = model.CompanyFK;
            viewData.ChequeRegisterList = await _Service.GetChequeRegisterListByDate(model);
            TempData["ChequeRegisterModel"] = viewData;

            return RedirectToAction(nameof(ChequePrinting), new { companyId = model.CompanyFK, fromDate = model.StrFromDate, ToDate = model.StrToDate });
        }

        [HttpGet]
        public async Task<JsonResult> ChequePrintCount(long chequeRegisterId)
        {
            bool response = false;
            if (chequeRegisterId > 0)
            {
                response = await _Service.PrintCount(chequeRegisterId);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cheque Sing

        [HttpGet]
        public async Task<ActionResult> ChequeSigning(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.ChequeRegisterList = await _Service.GetGeneratedChequeList(companyId);
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> ChequeRegisterSearchForSign(ChequeRegisterModel model)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = model.CompanyFK;
            viewData.ChequeRegisterList = await _Service.GetChequeRegisterListByDate(model);
            TempData["ChequeRegisterModel"] = viewData;

            return RedirectToAction(nameof(ChequeSigning), new { companyId = model.CompanyFK, fromDate = model.StrFromDate, ToDate = model.StrToDate });
        }

        [HttpGet]
        public async Task<JsonResult> ChequeSign(long chequeRegisterId)
        {
            bool response = false;
            if (chequeRegisterId > 0)
            {
                response = await _Service.ChequeSignRequest(chequeRegisterId);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ChequeCancel(long chequeRegisterId)
        {
            bool response = false;
            if (chequeRegisterId > 0)
            {
                response = await _Service.ChequeCancelRequest(chequeRegisterId);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cheque Cancel Request

        [HttpGet]
        public ActionResult ChequeCancelRequest(int companyId = 0)
        {
            ChequeRegisterModel viewData = new ChequeRegisterModel();
            viewData.CompanyFK = companyId;
            viewData.ChequeNumberList = new SelectList(_Service.ChequePageNo(), "Value", "Text");
            viewData.ChequeRegisterList = _Service.CanceledChequeRegisterList();
            return View(viewData);
        }

        [HttpPost]
        public ActionResult ChequeCancelRequest(ChequeRegisterModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                var data = _Service.SendRequest(model);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(ChequeCancelRequest), new { companyId = model.CompanyFK });
        }

        [HttpGet]
        public JsonResult IsSignOrNot(long chequeRegisterId)
        {
            var response = _Service.CheckIsSignOrNot(chequeRegisterId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CancelationInfo(long chequeRegisterId)
        {
            var response = _Service.ChequeCancelationInfo(chequeRegisterId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Other Json

        [HttpGet]
        public JsonResult RequisitionListWithFilter(int projectId)
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

        #endregion
    }
}