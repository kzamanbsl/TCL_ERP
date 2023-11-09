using System;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System.Threading.Tasks;
using System.Web.Mvc;
using KGERP.Service.Implementation.Procurement;
using KGERP.Service.Implementation.Accounting;
using KGERP.Service.Implementation.Configuration;
using DocumentFormat.OpenXml.EMMA;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class BatchPaymentController : Controller
    {
        private readonly AccountingService _accountingService;
        private readonly IBatchPaymentService batchPaymentService;
        private readonly ProcurementService procurementService;
        private readonly IVendorService vendorService;
        private readonly CollectionService collectionService;

        public BatchPaymentController(AccountingService accountingService, IBatchPaymentService batchPaymentService, ProcurementService procurementService, IVendorService vendorService, CollectionService collectionService)
        {
            _accountingService = accountingService;
            this.batchPaymentService = batchPaymentService;
            this.procurementService = procurementService;
            this.vendorService = vendorService;
            this.collectionService = collectionService;
        }

        #region Customer Batch Collection

        [HttpGet]
        public async Task<ActionResult> CustomerBatchPaymentMasterSlave(int companyId = 0, int batchPaymentMasterId = 0)
        {
            BatchPaymentMasterModel model = new BatchPaymentMasterModel();
            model = await Task.Run(() => batchPaymentService.GetCustomerBatchPaymentDetail(companyId, batchPaymentMasterId));
            model.batchPaymentDetailModel.SubZoneList = new SelectList(collectionService.SubZonesDropDownList(companyId), "Value", "Text");
            model.BankOrCashParantList = new SelectList(_accountingService.SeedCashAndBankDropDownList(companyId), "Value", "Text");
            model.CompanyId = companyId;
            model.CompanyFK = companyId;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CustomerBatchPaymentMasterSlave(BatchPaymentMasterModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {

                if (model.BatchPaymentMasterId == 0)
                {

                    model.BatchPaymentMasterId = await batchPaymentService.CustomerBatchPaymentMasterAdd(model);

                }
                await batchPaymentService.CustomerBatchPaymentDetailAdd(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                await batchPaymentService.CustomerBatchPaymentDetailEdit(model);
            }
            else if (model.ActionEum == ActionEnum.Finalize)
            {
                await batchPaymentService.SubmitCustomerBatchPayment(model.BatchPaymentMasterId);
            }
            return RedirectToAction(nameof(CustomerBatchPaymentMasterSlave), new { companyId = model.CompanyId, batchPaymentMasterId = model.BatchPaymentMasterId });
        }

        [HttpPost]
        public async Task<JsonResult> GetCustomerBatchPaymentDetailById(int id)
        {
            var model = await batchPaymentService.GetCustomerBatchPaymentDetailById(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCustomerBatchPaymentDetailById(BatchPaymentMasterModel batchPaymentMasterModel)
        {
            if (batchPaymentMasterModel.ActionEum == ActionEnum.Delete)
            {
                batchPaymentMasterModel.BatchPaymentMasterId = await batchPaymentService.CustomerBatchPaymentDetailDeleteById(batchPaymentMasterModel.batchPaymentDetailModel.BatchPaymentDetailId);
            }
            return RedirectToAction(nameof(CustomerBatchPaymentMasterSlave), new { companyId = batchPaymentMasterModel.CompanyId, batchPaymentMasterId = batchPaymentMasterModel.BatchPaymentMasterId });
        }

        [HttpGet]
        public async Task<ActionResult> CustomerBatchPaymentList(int companyId, DateTime? fromDate, DateTime? toDate)
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
            BatchPaymentMasterModel batchPaymentMasterModel = new BatchPaymentMasterModel();
            batchPaymentMasterModel = await batchPaymentService.GetCustomerBatchPaymentList(companyId, fromDate, toDate);
            batchPaymentMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            batchPaymentMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            batchPaymentMasterModel.UserId = System.Web.HttpContext.Current.User.Identity.Name;
            return View(batchPaymentMasterModel);
        }

        [HttpPost]
        [SessionExpire]
        public ActionResult CustomerBatchPaymentList(BatchPaymentMasterModel batchPaymentMasterModel)
        {
            if (batchPaymentMasterModel.CompanyId > 0)
            {
                Session["CompanyId"] = batchPaymentMasterModel.CompanyId;
            }
            batchPaymentMasterModel.FromDate = Convert.ToDateTime(batchPaymentMasterModel.StrFromDate);
            batchPaymentMasterModel.ToDate = Convert.ToDateTime(batchPaymentMasterModel.StrToDate);
            batchPaymentMasterModel.TransactionDate = Convert.ToDateTime(batchPaymentMasterModel.TransactionDate);
            return RedirectToAction(nameof(CustomerBatchPaymentList), new { companyId = batchPaymentMasterModel.CompanyId, fromDate = batchPaymentMasterModel.FromDate, toDate = batchPaymentMasterModel.ToDate });
        }


        #endregion


        #region Supplier Batch Payment

        // work on need

        #endregion
    }

}