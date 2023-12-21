using KGERP.Data.Models;
using KGERP.Service.Implementation;
using KGERP.Service.Implementation.Accounting;
using KGERP.Service.Implementation.Procurement;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using KGERP.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class VouchersController : BaseController
    {
        private readonly IVoucherTypeService _voucherTypeService;
        private readonly IVoucherService _voucherService;
        private readonly IAccountHeadService _accountHeadService;
        private readonly AccountingService _accountingService;
        private readonly IBillRequisitionService _billrequisitionService;


        public VouchersController(ERPEntities db, IAccountHeadService accountHeadService, IVoucherTypeService voucherTypeService, IVoucherService voucherService, IBillRequisitionService billrequisitionService)
        {
            this._voucherTypeService = voucherTypeService;
            this._voucherService = voucherService;
            this._accountHeadService = accountHeadService;
            _accountingService = new AccountingService(db);
             _billrequisitionService = billrequisitionService;
        }


        [HttpGet]
        [SessionExpire]
        public async Task<ActionResult> Index(int companyId, DateTime? fromDate, DateTime? toDate, bool? vStatus, int? voucherTypeId)
        {
            if (companyId > 0)
            {
                Session["CompanyId"] = companyId;
            }
            if (fromDate == null)
            {
                fromDate = DateTime.Now.AddMonths(-2);
            }

            if (toDate == null)
            {
                toDate = DateTime.Now;
            }
            if (vStatus == null)
            {
                vStatus = true;
            }

            VoucherModel voucherModel = new VoucherModel();
            voucherModel = await _voucherService.GetVouchersList(companyId, fromDate, toDate, vStatus, voucherTypeId);
            voucherModel.VoucherTypesList = new SelectList(_accountingService.VoucherTypesDownList(companyId), "Value", "Text");
            voucherModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            voucherModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            voucherModel.IsSubmit = vStatus;
            return View(voucherModel);
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> Index(VoucherModel voucherModel)
        {
            if (voucherModel.CompanyId > 0)
            {
                Session["CompanyId"] = voucherModel.CompanyId;
            }

            voucherModel.FromDate = Convert.ToDateTime(voucherModel.StrFromDate);
            voucherModel.ToDate = Convert.ToDateTime(voucherModel.StrToDate);


            return RedirectToAction(nameof(Index), new { companyId = voucherModel.CompanyId, fromDate = voucherModel.FromDate, toDate = voucherModel.ToDate, vStatus = voucherModel.IsSubmit, voucherTypeId = voucherModel.VmVoucherTypeId ?? 0 });
        }

        [HttpGet]
        [SessionExpire]
        public async Task<ActionResult> StockVoucherIndex(int companyId)
        {
            if (companyId > 0)
            {
                Session["CompanyId"] = companyId;
            }

            VoucherModel voucherModel = new VoucherModel();
            voucherModel = await _voucherService.GetStockVouchersList(companyId);
            voucherModel.VoucherTypesList = new SelectList(_accountingService.VoucherTypesDownList(companyId), "Value", "Text");

            return View(voucherModel);
        }

        [HttpGet]
        [SessionExpire]
        public ActionResult Create(int id, int companyId)
        {
            VoucherViewModel vm = new VoucherViewModel();
            Session["CompanyId"] = companyId;
            vm.Voucher = _voucherService.GetVoucher(companyId, id);
            vm.Voucher.CompanyId = companyId;
            vm.VoucherTypes = _voucherTypeService.GetVoucherTypeSelectModels(companyId);
            vm.CostCenters = _voucherTypeService.GetAccountingCostCenter(companyId);
            return View(vm);
        }

        [HttpGet]
        [SessionExpire]
        public async Task<ActionResult> GetAllVoucher(int companyId, int? voucherTypeId, DateTime? fromDate, DateTime? toDate)
        {

            if (companyId > 0)
            {
                Session["CompanyId"] = companyId;
            }
            if (fromDate == null)
            {
                fromDate = DateTime.Now.AddDays(-2);
            }
            if (toDate == null)
            {
                toDate = DateTime.Now;
            }

            VoucherViewModel voucherModel = new VoucherViewModel();
            //    voucherModel.Voucher = await voucherService.GetAllVouchersList(companyId,voucherTypeId, fromDate, toDate);
            voucherModel.Voucher.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            voucherModel.Voucher.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            voucherModel.VoucherTypes = _voucherTypeService.GetVoucherTypeSelectModels(companyId);
            voucherModel.Voucher.VoucherTypeId = voucherTypeId;
            return View(voucherModel);
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> GetAllVoucher(VoucherViewModel voucherModel)
        {
            if (voucherModel.Voucher.CompanyId > 0)
            {
                Session["CompanyId"] = voucherModel.Voucher.CompanyId;
            }
            voucherModel.Voucher.FromDate = Convert.ToDateTime(voucherModel.Voucher.StrFromDate);
            voucherModel.Voucher.ToDate = Convert.ToDateTime(voucherModel.Voucher.StrToDate);

            return RedirectToAction(nameof(Index), new
            {
                companyId = voucherModel.Voucher.CompanyId,
                voucherTypeId = voucherModel.Voucher.VoucherTypeId,
                fromDate = voucherModel.Voucher.FromDate,
                toDate = voucherModel.Voucher.ToDate
            });
        }

        //[HttpPost]
        //[SessionExpire]
        //[ValidateAntiForgeryToken]

        //public ActionResult CreateTempVoucher(VoucherViewModel vm)
        //{
        //    vm.Voucher = voucherService.CreateTempVoucher(vm.Voucher);

        //    return PartialView("~/Views/Vouchers/_voucherDetailGrid.cshtml", vm.Voucher);
        //}


        // POST: GLTables/Create  

        [HttpPost]
        [SessionExpire]
        public ActionResult CreateVoucher(VoucherViewModel vm)
        {
            string message = string.Empty;
            vm.Voucher.VoucherDetails = vm.VoucherDetails;
            bool result = _voucherService.SaveVoucher(vm.Voucher, out message);
            if (result)
            {
                TempData["message"] = message;
                return RedirectToAction("Index", new { companyId = vm.Voucher.CompanyId });
            }
            TempData["message"] = message;
            return RedirectToAction("Create", new { id = 0, companyId = vm.Voucher.CompanyId });
        }

        //[HttpPost]
        //[SessionExpire]
        //public ActionResult RemoveVoucherItem(long id)
        //{

        //    VoucherModel Voucher = voucherService.RemoveVoucherItem(id);

        //    return PartialView("~/Views/Vouchers/_voucherDetailGrid.cshtml", Voucher);
        //}

        //[SessionExpire]
        //[HttpGet]
        public JsonResult GetVoucherNo(int voucherTypeId, int companyId, DateTime voucherDate)
        {
            ModelState.Clear();
            var voucherNo = _voucherService.GetVoucherNo(voucherTypeId, companyId, voucherDate);
            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [SessionExpire]
        public JsonResult VoucherNoAutoComplete(string prefix)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var vouchers = _voucherService.GetVoucherNoAutoComplete(prefix, companyId);
            return Json(vouchers);
        }

        [HttpGet]
        public async Task<ActionResult> ManageBankOrCash(int companyId = 0, int voucherId = 0)
        {
            VMJournalSlave vmJournalSlave = new VMJournalSlave();

            if (voucherId == 0)
            {
                vmJournalSlave = await Task.Run(() => _accountingService.GetCompaniesDetails(companyId));

            }
            else if (voucherId > 0)
            {
                vmJournalSlave = await Task.Run(() => _accountingService.GetVoucherDetails(companyId, voucherId));
            }
            vmJournalSlave.CostCenterList = new SelectList(_accountingService.CostCenterDropDownList(companyId), "Value", "Text");
            vmJournalSlave.VoucherTypesList = new SelectList(_accountingService.VoucherTypesDownList(companyId), "Value", "Text");
            if (companyId == (int)CompanyNameEnum.GloriousCropCareLimited ||
                companyId == (int)CompanyNameEnum.KrishibidPrintingAndPublicationLimited ||
                companyId == (int)CompanyNameEnum.KrishibidPackagingLimited)
            {
                vmJournalSlave.BankOrCashParantList = new SelectList(_accountingService.GCCLCashAndBankDropDownList(companyId), "Value", "Text");

            }
            if (companyId == (int)CompanyNameEnum.KrishibidSeedLimited)
            {
                vmJournalSlave.BankOrCashParantList = new SelectList(_accountingService.SeedCashAndBankDropDownList(companyId), "Value", "Text");

            }

            return View(vmJournalSlave);
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> ManageBankOrCash(VMJournalSlave vmJournalSlave)
        {

            if (vmJournalSlave.ActionEum == ActionEnum.Add)
            {
                if (vmJournalSlave.VoucherId == 0)
                {
                    vmJournalSlave.IsStock = false;
                    // it is important / dont delete
                    var voucherNo = _voucherService.GetVoucherNo(vmJournalSlave.VoucherTypeId, vmJournalSlave.CompanyFK.Value, vmJournalSlave.Date.Value);
                    vmJournalSlave.VoucherNo = voucherNo;
                    vmJournalSlave.VoucherId = await _accountingService.VoucherAdd(vmJournalSlave);

                }
                await _accountingService.VoucherDetailAdd(vmJournalSlave);
            }
            else if (vmJournalSlave.ActionEum == ActionEnum.Edit)
            {
                //Edit
                await _accountingService.VoucherDetailsEdit(vmJournalSlave);
            }
            else if (vmJournalSlave.ActionEum == ActionEnum.Delete)
            {
                //Delete
                await _accountingService.VoucherDetailsDelete(vmJournalSlave.VoucherDetailId.Value);
            }

            return RedirectToAction(nameof(ManageBankOrCash), new { companyId = vmJournalSlave.CompanyFK, voucherId = vmJournalSlave.VoucherId });
        }


        #region Requisition Voucher Basic

        [HttpGet]
        [SessionExpire]
        public async Task<ActionResult> RequisitionVoucherList(int companyId, DateTime? fromDate, DateTime? toDate, bool? vStatus, int? voucherTypeId)
        {
            if (companyId > 0)
            {
                Session["CompanyId"] = companyId;
            }
            if (fromDate == null)
            {
                fromDate = DateTime.Now.AddMonths(-2);
            }

            if (toDate == null)
            {
                toDate = DateTime.Now;
            }
            if (vStatus == null)
            {
                vStatus = true;
            }

            VoucherModel voucherModel = new VoucherModel();
            voucherModel = await _voucherService.GetRequisitionVouchersList(companyId, fromDate, toDate, vStatus, voucherTypeId);
            voucherModel.VoucherTypesList = new SelectList(_accountingService.VoucherTypesDownList(companyId), "Value", "Text");
            voucherModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            voucherModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            voucherModel.IsSubmit = vStatus;
            return View(voucherModel);
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> RequisitionVoucherList(VoucherModel voucherModel)
        {
            if (voucherModel.CompanyId > 0)
            {
                Session["CompanyId"] = voucherModel.CompanyId;
            }

            voucherModel.FromDate = Convert.ToDateTime(voucherModel.StrFromDate);
            voucherModel.ToDate = Convert.ToDateTime(voucherModel.StrToDate);


            return RedirectToAction(nameof(RequisitionVoucherList), new { companyId = voucherModel.CompanyId, fromDate = voucherModel.FromDate, toDate = voucherModel.ToDate, vStatus = voucherModel.IsSubmit, voucherTypeId = voucherModel.VmVoucherTypeId ?? 0 });
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> DeleteRequisitionVoucher(VoucherModel voucherModel)
        {
            if (voucherModel.VoucherId > 0)
            {
                await _accountingService.RequisitionVoucherDelete(voucherModel);

            }

            return RedirectToAction(nameof(RequisitionVoucherList), new { companyId = voucherModel.CompanyId });
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> UndoSubmitRequisitionVoucher(VoucherModel voucherModel)
        {
            if (voucherModel.VoucherId > 0)
            {
                await _accountingService.RequisitionVoucherUndoSubmit(voucherModel);
            }
            return RedirectToAction(nameof(RequisitionVoucherList), new { companyId = voucherModel.CompanyId });
        }


        //RequisitionVoucherEntry Master Details

        [HttpGet]
        public async Task<ActionResult> RequisitionVoucherEntry(int companyId = 0, int voucherId = 0)
        {
            VMJournalSlave vmJournalSlave = new VMJournalSlave();

            if (voucherId == 0)
            {
                vmJournalSlave = await Task.Run(() => _accountingService.GetCompaniesDetails(companyId));

            }
            else if (voucherId > 0)
            {
                vmJournalSlave = await Task.Run(() => _accountingService.GetVoucherRequisitionMapDetails(companyId, voucherId));
            }
            vmJournalSlave.CostCenterList = new SelectList(_accountingService.CostCenterDropDownList(companyId), "Value", "Text");
            vmJournalSlave.VoucherTypesList = new SelectList(_accountingService.VoucherTypesDownList(companyId), "Value", "Text");
            if (companyId == (int)CompanyNameEnum.GloriousCropCareLimited ||
                companyId == (int)CompanyNameEnum.KrishibidPrintingAndPublicationLimited ||
                companyId == (int)CompanyNameEnum.KrishibidPackagingLimited)
            {
                vmJournalSlave.BankOrCashParantList = new SelectList(_accountingService.GCCLCashAndBankDropDownList(companyId), "Value", "Text");

            }
            if (companyId == (int)CompanyNameEnum.KrishibidSeedLimited)
            {
                long requisitionId = 0;
                vmJournalSlave.BankOrCashParantList = new SelectList(_accountingService.SeedCashAndBankDropDownList(companyId), "Value", "Text");
                vmJournalSlave.Requisitions = new SelectList(_billrequisitionService.ApprovedRequisitionList(companyId), "Value", "Text");
                vmJournalSlave.MaterialItemList = new SelectList(_billrequisitionService.ApprovedMaterialList(companyId, requisitionId), "ProductId", "ProductName");

            }

            return View(vmJournalSlave);
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> RequisitionVoucherEntry(VMJournalSlave vmJournalSlave)
        {

            if (vmJournalSlave.ActionEum == ActionEnum.Add)
            {
                if (vmJournalSlave.VoucherId == 0)
                {
                    vmJournalSlave.IsStock = false;
                    // it is important / dont delete
                    var voucherNo = _voucherService.GetVoucherNo(vmJournalSlave.VoucherTypeId, vmJournalSlave.CompanyFK.Value, vmJournalSlave.Date.Value);
                    vmJournalSlave.VoucherNo = voucherNo;
                    vmJournalSlave.VoucherId = await _accountingService.VoucherRequisitionMapAdd(vmJournalSlave);

                }
                await _accountingService.VoucherDetailRequisitionMapAdd(vmJournalSlave);
            }
            else if (vmJournalSlave.ActionEum == ActionEnum.Edit)
            {
                //Edit
                await _accountingService.VoucherDetailsRequisitionMapEdit(vmJournalSlave);
            }
            else if (vmJournalSlave.ActionEum == ActionEnum.Delete)
            {
                //Delete
                await _accountingService.VoucherDetailsRequisitionMapDelete(vmJournalSlave.VoucherDetailId.Value);
            }

            return RedirectToAction(nameof(RequisitionVoucherEntry), new { companyId = vmJournalSlave.CompanyFK, voucherId = vmJournalSlave.VoucherId });
        }

        #endregion



        #region Requisition Voucher Approval 

        [HttpGet]
        [SessionExpire]
        public async Task<ActionResult> RequisitionVoucherApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, bool? vStatus, int? voucherTypeId)
        {
            if (companyId > 0)
            {
                Session["CompanyId"] = companyId;
            }
            if (fromDate == null)
            {
                fromDate = DateTime.Now.AddMonths(-2);
            }

            if (toDate == null)
            {
                toDate = DateTime.Now;
            }
            if (vStatus == null)
            {
                vStatus = true;
            }

            VoucherModel voucherModel = new VoucherModel();
            voucherModel = await _voucherService.GetRequisitionVouchersList(companyId, fromDate, toDate, vStatus, voucherTypeId);
            voucherModel.VoucherTypesList = new SelectList(_accountingService.VoucherTypesDownList(companyId), "Value", "Text");
            voucherModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            voucherModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            voucherModel.IsApproved = vStatus;
            return View(voucherModel);
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> RequisitionVoucherApprovalList(VoucherModel voucherModel)
        {
            if (voucherModel.CompanyId > 0)
            {
                Session["CompanyId"] = voucherModel.CompanyId;
            }

            voucherModel.FromDate = Convert.ToDateTime(voucherModel.StrFromDate);
            voucherModel.ToDate = Convert.ToDateTime(voucherModel.StrToDate);


            return RedirectToAction(nameof(RequisitionVoucherApprovalList), new { companyId = voucherModel.CompanyId, fromDate = voucherModel.FromDate, toDate = voucherModel.ToDate, vStatus = voucherModel.IsApproved, voucherTypeId = voucherModel.VmVoucherTypeId ?? 0 });
        }


        [HttpGet]
        public async Task<ActionResult> RequisitionVoucherApproval(int companyId = 0, int voucherId = 0)
        {
            VMJournalSlave vmJournalSlave = new VMJournalSlave();

            if (voucherId == 0)
            {
                vmJournalSlave = await Task.Run(() => _accountingService.GetCompaniesDetails(companyId));

            }
            else if (voucherId > 0)
            {
                vmJournalSlave = await Task.Run(() => _accountingService.GetVoucherRequisitionMapDetails(companyId, voucherId));
            }
            vmJournalSlave.CostCenterList = new SelectList(_accountingService.CostCenterDropDownList(companyId), "Value", "Text");
            vmJournalSlave.VoucherTypesList = new SelectList(_accountingService.VoucherTypesDownList(companyId), "Value", "Text");
            if (companyId == (int)CompanyNameEnum.GloriousCropCareLimited ||
                companyId == (int)CompanyNameEnum.KrishibidPrintingAndPublicationLimited ||
                companyId == (int)CompanyNameEnum.KrishibidPackagingLimited)
            {
                vmJournalSlave.BankOrCashParantList = new SelectList(_accountingService.GCCLCashAndBankDropDownList(companyId), "Value", "Text");

            }
            if (companyId == (int)CompanyNameEnum.KrishibidSeedLimited)
            {
                long requisitionId = 0;
                vmJournalSlave.BankOrCashParantList = new SelectList(_accountingService.SeedCashAndBankDropDownList(companyId), "Value", "Text");
                vmJournalSlave.Requisitions = new SelectList(_billrequisitionService.ApprovedRequisitionList(companyId), "Value", "Text");
                vmJournalSlave.MaterialItemList = new SelectList(_billrequisitionService.ApprovedMaterialList(companyId, requisitionId), "ProductId", "ProductName");

            }

            return View(vmJournalSlave);
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> RequisitionVoucherApproval(VMJournalSlave vmJournalSlave)
        {

            
            if (vmJournalSlave.ActionEum == ActionEnum.Approve)
            {
                //Delete
                await _accountingService.VoucherDetailsRequisitionMapDelete(vmJournalSlave.VoucherDetailId.Value);
            }

            return RedirectToAction(nameof(RequisitionVoucherApproval), new { companyId = vmJournalSlave.CompanyFK, voucherId = vmJournalSlave.VoucherId });
        }


        #endregion

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> DeleteVoucher(VoucherModel voucherModel)
        {
            if (voucherModel.VoucherId > 0)
            {
                await _accountingService.VoucherDelete(voucherModel);

            }

            return RedirectToAction(nameof(Index), new { companyId = voucherModel.CompanyId });
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> UndoSubmitVoucher(VoucherModel voucherModel)
        {
            if (voucherModel.VoucherId > 0)
            {
                await _accountingService.VoucherUndoSubmit(voucherModel);
            }
            return RedirectToAction(nameof(Index), new { companyId = voucherModel.CompanyId });
        }


        [HttpGet]
        public async Task<ActionResult> ManageStock(int companyId = 0, int voucherId = 0)
        {
            VMJournalSlave vmJournalSlave = new VMJournalSlave();

            if (voucherId == 0)
            {
                vmJournalSlave.CompanyFK = companyId;
            }
            else if (voucherId > 0)
            {
                vmJournalSlave = await Task.Run(() => _accountingService.GetStockVoucherDetails(companyId, voucherId));
            }
            vmJournalSlave.CostCenterList = new SelectList(_accountingService.CostCenterDropDownList(companyId), "Value", "Text");
            vmJournalSlave.VoucherTypesList = new SelectList(_accountingService.VoucherTypesDownList(companyId), "Value", "Text");

            vmJournalSlave.BankOrCashParantList = new SelectList(_accountingService.StockDropDownList(companyId), "Value", "Text");


            return View(vmJournalSlave);
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> ManageStock(VMJournalSlave vmJournalSlave)
        {

            if (vmJournalSlave.ActionEum == ActionEnum.Add)
            {
                if (vmJournalSlave.VoucherId == 0)
                {
                    vmJournalSlave.IsStock = true;
                    vmJournalSlave.VoucherId = await _accountingService.VoucherAdd(vmJournalSlave);

                }
                await _accountingService.VoucherDetailAdd(vmJournalSlave);
            }
            else if (vmJournalSlave.ActionEum == ActionEnum.Edit)
            {
                //Delete
                await _accountingService.VoucherDetailsEdit(vmJournalSlave);
            }
            return RedirectToAction(nameof(ManageStock), new { companyId = vmJournalSlave.CompanyFK, voucherId = vmJournalSlave.VoucherId });
        }

        public async Task<ActionResult> Head5Get(int companyId, int parentId)
        {

            var headGLModel = await Task.Run(() => _accountingService.Head5Get(companyId, parentId));
            var list = headGLModel.Select(x => new { Value = x.Id, Text = x.AccName }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> HeadGLGet(int companyId, int parentId)
        {

            var headGLModel = await Task.Run(() => _accountingService.HeadGLGet(companyId, parentId));
            var list = headGLModel.Select(x => new { Value = x.Id, Text = x.AccName }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> HeadGLByHead5ParentIdGet(int companyId, int parentId)
        {
            var headGLModel = await Task.Run(() => _accountingService.HeadGLByHeadGLParentIdGet(companyId, parentId));
            var list = headGLModel.Select(x => new { Value = x.Id, Text = x.AccName }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);

            //if (companyId == (int)CompanyName.KrishibidSeedLimited)
            //{
            //    var headGLModel = await Task.Run(() => _accountingService.HeadGLByHead5ParentIdGet(companyId, parentId));
            //    var list = headGLModel.Select(x => new { Value = x.Id, Text = x.AccName }).ToList();
            //    return Json(list, JsonRequestBehavior.AllowGet);

            //}
            //else if (companyId == (int)CompanyName.GloriousCropCareLimited 
            //    || companyId == (int)CompanyName.KrishibidPrintingAndPublicationLimited
            //    || companyId == (int)CompanyName.KrishibidPackagingLimited
            //    || companyId == (int)CompanyName.KrishibidFirmLimited
            //    || companyId == (int)CompanyName.KrishibidPoultryLimited
            //    || companyId == (int)CompanyName.KrishibidSeedLimited)
            //{

            //}

        }

        public async Task<ActionResult> StockByHead5ParentIdGet(int companyId, int parentId)
        {
            if (companyId == (int)CompanyNameEnum.NaturalFishFarmingLimited
                || companyId == (int)CompanyNameEnum.OrganicPoultryLimited
                || companyId == (int)CompanyNameEnum.SonaliOrganicDairyLimited
                || companyId == (int)CompanyNameEnum.KrishibidPrintingAndPublicationLimited
                || companyId == (int)CompanyNameEnum.KrishibidFoodAndBeverageLimited
                || companyId == (int)CompanyNameEnum.KrishibidPackagingLimited
                || companyId == (int)CompanyNameEnum.KrishibidBazaarLimited
                || companyId == (int)CompanyNameEnum.KrishibidPoultryLimited
                || companyId == (int)CompanyNameEnum.KrishibidFisheriesLimited
                || companyId == (int)CompanyNameEnum.KrishibidTradingLimited
                || companyId == (int)CompanyNameEnum.KrishibidSafeFood
                
                )
            {
                var headGLModel = await Task.Run(() => _accountingService.HeadGLGet(companyId, parentId));
                var list = headGLModel.Select(x => new { Value = x.Id, Text = x.AccName }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);

            }

            return null;
        }

        public async Task<ActionResult> AutoInsertVoucherDetails(int voucherId, int virtualHeadId, string virtualHeadParticular)
        {
            long voucherDetailsId = await Task.Run(() => _accountingService.AutoInsertVoucherDetails(voucherId, virtualHeadId, virtualHeadParticular));
            return Json(voucherDetailsId, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AutoInsertStockVoucherDetails(int companyId, int voucherId)
        {
            long voucherDetailsId = 0;
            if (companyId == (int)CompanyNameEnum.NaturalFishFarmingLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.NFFLAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.OrganicPoultryLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.OPLAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.SonaliOrganicDairyLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.SODLAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.KrishibidPrintingAndPublicationLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.PrintingAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.KrishibidFoodAndBeverageLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.FBLAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.KrishibidPackagingLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.PackagingAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.KrishibidBazaarLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.KBLAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.KrishibidFisheriesLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.FishariseAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.KrishibidPoultryLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.PoultryAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.KrishibidTradingLimited)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.TradingAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            if (companyId == (int)CompanyNameEnum.KrishibidSafeFood)
            {
                voucherDetailsId = await Task.Run(() => _accountingService.SafeFoodAutoInsertStockVoucherDetails(companyId, voucherId));
            }
            return Json(voucherDetailsId, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateVoucherStatus(int voucherId)
        {
            long voucherDetailsId = await Task.Run(() => _accountingService.UpdateVoucherStatus(voucherId));
            return Json(voucherDetailsId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutoCompleteHeadGLGet(string prefix, int companyId)
        {
            var products = _accountingService.GetAutoCompleteHeadGL(prefix, companyId);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutoCompleteExpenseHeadGL(string prefix, int companyId)
        {
            var products = _accountingService.GetAutoCompleteExpenseHeadGL(prefix, companyId);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSingleVoucherDetails(int voucherDetailId)
        {
            var model = await _accountingService.GetSingleVoucherDetails(voucherDetailId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSingleVoucher(int voucherId)
        {
            var model = await _accountingService.GetSingleVoucher(voucherId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSinglevoucherTypes(int voucherTypesId)
        {
            var model = await _accountingService.GetSingleVoucherTypes(voucherTypesId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }



    }
}
