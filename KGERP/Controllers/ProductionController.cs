﻿using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KGERP.Service.Implementation.Production;
using KGERP.Utility;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class ProductionController : Controller
    {
        private HttpContext httpContext;
        private readonly ProductionService _service;


        public ProductionController(ProductionService productionService)
        {
            _service = productionService;
        }
        public JsonResult GetAutoCompleteSupplierGet(string prefix, int companyId)
        {
            var products = _service.GetAutoCompleteSupplier(prefix, companyId);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        #region Production Process
        [HttpGet]
        public async Task<ActionResult> ProdReferenceSlaveReport(int companyId = 0, int prodReferenceId = 0)
        {
            VMProdReferenceSlave vmProdReferenceSlave = new VMProdReferenceSlave();
            if (prodReferenceId > 0)
            {
                vmProdReferenceSlave = await Task.Run(() => _service.ProdReferenceSlaveGet(companyId, prodReferenceId));

            }
            return View(vmProdReferenceSlave);
        }
        [HttpGet]
        public async Task<ActionResult> ProdReferenceSlave(int companyId = 0, int prodReferenceId = 0)
        {
            VMProdReferenceSlave vmPurchaseOrderSlave = new VMProdReferenceSlave();

            if (prodReferenceId == 0)
            {
                vmPurchaseOrderSlave.CompanyFK = companyId;

            }
            else if (prodReferenceId > 0)
            {
                vmPurchaseOrderSlave = await Task.Run(() => _service.ProdReferenceSlaveGet(companyId, prodReferenceId));

            }

            return View(vmPurchaseOrderSlave);
        }
        [HttpPost]
        public async Task<ActionResult> ProdReferenceSlave(VMProdReferenceSlave vmProdReferenceSlave)
        {

            if (vmProdReferenceSlave.ActionEum == ActionEnum.Add)
            {
                if (vmProdReferenceSlave.ProdReferenceId == 0)
                {
                    vmProdReferenceSlave.ProdReferenceId = await _service.Prod_ReferenceAdd(vmProdReferenceSlave);
                }
                await _service.ProdReferenceSlaveAdd(vmProdReferenceSlave);
            }
            //else if (vmProdReferenceSlave.ActionEum == ActionEnum.Edit)
            //{
            //    //Delete
            //    await _service.ProcurementPurchaseOrderSlaveEdit(vmProdReferenceSlave);
            //}
            return RedirectToAction(nameof(ProdReferenceSlave), new { companyId = vmProdReferenceSlave.CompanyFK, prodReferenceId = vmProdReferenceSlave.ProdReferenceId });
        }
        public JsonResult AutoCompleteMaterialReceivesGet(int companyId, string prefix)
        {
            var products = _service.GetAutoCompleteMaterialReceives(companyId, prefix);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ProdReferenceSlaveByChallan(int companyId = 0, int prodReferenceId = 0)
        {
            VMProdReferenceSlave vmPurchaseOrderSlave = new VMProdReferenceSlave();

            if (prodReferenceId == 0)
            {
                vmPurchaseOrderSlave.CompanyFK = companyId;

            }
            else if (prodReferenceId > 0)
            {
                vmPurchaseOrderSlave = await Task.Run(() => _service.ProdReferenceSlaveGet(companyId, prodReferenceId));

            }

            return View(vmPurchaseOrderSlave);
        }
        [HttpPost]
        public async Task<ActionResult> ProdReferenceSlaveByChallan(VMProdReferenceSlave vmProdReferenceSlave)
        {

            if (vmProdReferenceSlave.ActionEum == ActionEnum.Add)
            {
                if (vmProdReferenceSlave.ProdReferenceId == 0)
                {
                    vmProdReferenceSlave.ProdReferenceId = await _service.Prod_ReferenceAdd(vmProdReferenceSlave);
                }
                await _service.ProdReferenceSlaveByChallanAdd(vmProdReferenceSlave);
            }
            else if (vmProdReferenceSlave.ActionEum == ActionEnum.Edit)
            {
                //Delete
                await _service.ProdReferenceSlaveEdit(vmProdReferenceSlave);
            }
            return RedirectToAction(nameof(ProdReferenceSlaveByChallan), new { companyId = vmProdReferenceSlave.CompanyFK, prodReferenceId = vmProdReferenceSlave.ProdReferenceId });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProcurementPurchaseOrderSlave(VMProdReferenceSlave vmProdReferenceSlave)
        {
            if (vmProdReferenceSlave.ActionEum == ActionEnum.Delete)
            {
                //Delete
                vmProdReferenceSlave.ProdReferenceId = await _service.Prod_ReferenceSlaveDelete(vmProdReferenceSlave.ProdReferenceSlaveID);
            }
            return RedirectToAction(nameof(ProdReferenceSlave), new { companyId = vmProdReferenceSlave.CompanyFK, purchaseOrderId = vmProdReferenceSlave.ProdReferenceId });
        }

        [SessionExpire]
        [HttpGet]
        public async Task<ActionResult> ProdReferenceList(int companyId, DateTime? fromDate, DateTime? toDate)
        {
            if (companyId > 0)
            {  Session["CompanyId"] = companyId; }
            if (fromDate == null)
            { fromDate = DateTime.Now.AddMonths(-2);}
            if (toDate == null)
            { toDate = DateTime.Now;}

            VMProdReference vmPaymentMaster = new VMProdReference();
            vmPaymentMaster = await Task.Run(() => _service.ProdReferenceListGet(companyId, fromDate, toDate));
            vmPaymentMaster.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            vmPaymentMaster.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            return View(vmPaymentMaster);
        }

        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> ProdReferenceList(VMProdReference model)
        {
            if (model.CompanyFK > 0)
            {
                Session["CompanyId"] = model.CompanyFK;
            }
            model.FromDate = Convert.ToDateTime(model.StrFromDate);
            model.ToDate = Convert.ToDateTime(model.StrToDate);
            return RedirectToAction(nameof(ProdReferenceList), new { companyId = model.CompanyFK, fromDate = model.FromDate, toDate = model.ToDate });
        }
        //[HttpGet]
        //public async Task<ActionResult> ProdReferenceList(int companyId)
        //{
        //    VMProdReference vmProdReference = new VMProdReference();
        //    vmProdReference = await _service.ProdReferenceListGet(companyId);

        //    return View(vmProdReference);
        //}
        [HttpPost]
        public async Task<ActionResult> ProdReferenceListList(VMProdReference vmProdReference)
        {
            if (vmProdReference.ActionEum == ActionEnum.Edit)
            {
                await _service.Prod_ReferenceEdit(vmProdReference);
            }
            return RedirectToAction(nameof(ProdReferenceListList), new { companyId = vmProdReference.CompanyFK });
        }

        public async Task<JsonResult> SingleProdReferenceSlave(int id)
        {
            var model = await _service.GetSingleProdReferenceSlave(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SingleProdReferenceSlaveConsumption(int id)
        {
            var model = await _service.GetSingleProd_ReferenceSlaveConsumption(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> SingleProdReferenceSlaveExpensessConsumption(int id)
        {
            var model = await _service.GetSingleProdReferenceSlaveExpansessConsumption(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> SingleProdReference(int id)
        {
            VMProdReference model = new VMProdReference();
            model = await _service.GetSingleProdReference(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitProdReference(VMProdReference vmProdReference)
        {
            vmProdReference.ProdReferenceId = await _service.ProdReferenceSubmit(vmProdReference.ProdReferenceId);
            return RedirectToAction(nameof(ProdReferenceListList), new { companyId = vmProdReference.CompanyFK });
        }
        [HttpPost]
        public async Task<ActionResult> SubmitProdReferenceFromSlave(VMProdReferenceSlave vmProdReferenceSlave)
        {
            vmProdReferenceSlave.ProdReferenceId = await _service.ProdReferenceSubmit(vmProdReferenceSlave.ProdReferenceId);
            return RedirectToAction(nameof(ProdReferenceSlave), "Procurement", new { companyId = vmProdReferenceSlave.CompanyFK, prodReferenceId = vmProdReferenceSlave.ProdReferenceId });
        }


        [HttpPost]
        public async Task<ActionResult> DeleteProdReference(VMProdReference vmProdReference)
        {
            if (vmProdReference.ActionEum == ActionEnum.Delete)
            {
                //Delete
                vmProdReference.ProdReferenceId = await _service.ProdReferenceDelete(vmProdReference.ProdReferenceId);
            }
            return RedirectToAction(nameof(ProdReferenceListList), new { companyId = vmProdReference.CompanyFK });
        }

        #endregion

        public ActionResult GetMaterialReceiveDetails(int materialReceiveId)
        {
            var model = new VMProdReferenceSlave();
            if (materialReceiveId > 0)
            {
                model.DataToList = _service.GetMaterialReceiveDetailsData(materialReceiveId);
            }
            return PartialView("_MaterialReceiveDetails", model);
        }



        public async Task<ActionResult> GCCLProdReferenceSlave(int companyId = 0, int prodReferenceId = 0)
        {
            VMProdReferenceSlave vmProdReferenceSlave = new VMProdReferenceSlave();

            if (prodReferenceId == 0)
            {
                vmProdReferenceSlave.CompanyFK = companyId;

            }
            else if (prodReferenceId > 0)
            {
                vmProdReferenceSlave = await Task.Run(() => _service.GCCLProdReferenceSlaveGet(companyId, prodReferenceId));

            }
            if (companyId == (int)CompanyNameEnum.GloriousCropCareLimited)
            {

                vmProdReferenceSlave.FactoryExpensesList = new SelectList(_service.GCCLLCFactoryExpanceHeadGLList(companyId), "Value", "Text");
                vmProdReferenceSlave.AdvanceHeadList = new SelectList(_service.GCCLAdvanceHeadGLList(companyId), "Value", "Text");

            }
            return View(vmProdReferenceSlave);
        }
        [HttpPost]
        public async Task<ActionResult> GCCLProdReferenceSlave(VMProdReferenceSlave vmProdReferenceSlave)
        {

            if (vmProdReferenceSlave.ActionEum == ActionEnum.Add)
            {
                if (vmProdReferenceSlave.ProdReferenceId == 0)
                {
                    vmProdReferenceSlave.ProdReferenceId = await _service.Prod_ReferenceAdd(vmProdReferenceSlave);
                }
                if (vmProdReferenceSlave.RProductId > 0)
                {
                    await _service.GCCLProd_ReferenceSlaveConsumptionAdd(vmProdReferenceSlave);
                }
                if (vmProdReferenceSlave.FactoryExpensesHeadGLId > 0)
                {
                    await _service.GCCLProdReferenceFactoryExpensesAdd(vmProdReferenceSlave);
                }
                if (vmProdReferenceSlave.FProductId > 0)
                {
                    await _service.GCCLProdReferenceSlaveAdd(vmProdReferenceSlave);
                }
            }
            else if (vmProdReferenceSlave.ActionEum == ActionEnum.Edit)
            {
                //Delete
                if (vmProdReferenceSlave.RProductId > 0 && vmProdReferenceSlave.ID > 0)
                {
                    await _service.ProdReferenceSlaveRawConsumptionEdit(vmProdReferenceSlave);
                }
                if (vmProdReferenceSlave.FactoryExpensesHeadGLId > 0 && vmProdReferenceSlave.ID > 0)
                {
                    await _service.ProdReferenceSlaveFactoryConsumptionEdit(vmProdReferenceSlave);
                }
                if (vmProdReferenceSlave.FProductId > 0 && vmProdReferenceSlave.ProdReferenceSlaveID > 0)
                {
                    await _service.ProdReferenceSlaveEdit(vmProdReferenceSlave);
                }                
            }
            else if (vmProdReferenceSlave.ActionEum == ActionEnum.Delete)
            {
                //Delete
                if (vmProdReferenceSlave.ID > 0)
                {
                    await _service.DeleteProdReferenceSlaveConsumption(vmProdReferenceSlave);
                }
                if (vmProdReferenceSlave.ProdReferenceSlaveID > 0)
                {
                    await _service.DeleteProdReferenceSlave(vmProdReferenceSlave);
                }

            }
            else if (vmProdReferenceSlave.ActionEum == ActionEnum.Finalize)
            {
                await _service.SubmitProdReference(vmProdReferenceSlave);


            }
            return RedirectToAction(nameof(GCCLProdReferenceSlave), new { companyId = vmProdReferenceSlave.CompanyFK, prodReferenceId = vmProdReferenceSlave.ProdReferenceId });
        }

    }
}