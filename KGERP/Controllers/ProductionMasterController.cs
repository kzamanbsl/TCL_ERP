using System;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System.Threading.Tasks;
using System.Web.Mvc;
using KGERP.Service.Implementation.Procurement;
using KGERP.Service.Implementation.Accounting;
using KGERP.Service.Implementation;
using KGERP.Data.Models;
using System.Web.Services.Description;
using DocumentFormat.OpenXml.EMMA;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class ProductionMasterController : Controller
    {
        private readonly IUnitService unitService;
        private readonly IProductService productService;
        private readonly IProductSubCategoryService productSubCategoryService;
        private readonly IProductCategoryService productCategoryService;
        private readonly AccountingService _accountingService;
        private readonly IProductionMasterService productionMasterService;
        public ProductionMasterController(IProductionMasterService productionMasterService, AccountingService accountingService, IUnitService unitService, IProductService productService, IProductCategoryService productCategoryService, IProductSubCategoryService productSubCategoryService)
        {
            this.productCategoryService = productCategoryService;
            this.productSubCategoryService = productSubCategoryService;
            this.productionMasterService = productionMasterService;
            this.unitService = unitService;
            _accountingService = accountingService;
        }

        [HttpGet]
        public async Task<ActionResult> ProductionMasterSlave(int companyId = 0, int productionMasterId = 0, string productType = "R")
        {
            ProductionMasterModel model = new ProductionMasterModel();
            model.IsSubmitted = false;
            model = await Task.Run(() => productionMasterService.ProductionDetailsGet(companyId, productionMasterId));
            model.CompanyId = companyId;
            model.CompanyFK = companyId;
            model.ProductionDate = DateTime.Now;
            model.ProductCategories = productCategoryService.GetProductCategorySelectModelByCompany(companyId, productType);
            model.Units = unitService.GetUnitSelectModels(companyId);
            model.ProductionStatusList = productionMasterService.GetProductionStatusList(companyId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ProductionMasterSlave(ProductionMasterModel productionMasterModel)
        {

            if (productionMasterModel.ActionEum == ActionEnum.Add)
            {
                if (productionMasterModel.ProductionMasterId == 0)
                {
                    productionMasterModel.ProductionMasterId = await productionMasterService.ProductionAdd(productionMasterModel);

                }
                await productionMasterService.ProductionDetailAdd(productionMasterModel);
            }
            else if (productionMasterModel.ActionEum == ActionEnum.Edit)
            {
                //Delete
                await productionMasterService.ProductionDetailEdit(productionMasterModel);
            }
            else if (productionMasterModel.ActionEum == ActionEnum.Finalize)
            {
                //Delete
                var stockInfoIdVal = Convert.ToInt32(Session["StockInfoId"]);
                await productionMasterService.SubmitProductionMastersFromSlave(productionMasterModel.ProductionMasterId, stockInfoIdVal);
            }
            return RedirectToAction(nameof(ProductionMasterSlave), new { companyId = productionMasterModel.CompanyId, productionMasterId = productionMasterModel.ProductionMasterId });
        }

        //[HttpPost]
        //public async Task<ActionResult> SubmitProductionMastersFromSlave(ProductionMasterModel productionMasterModel)
        //{
        //    productionMasterModel.ProductionMasterId = await productionMasterService.SubmitProductionMastersFromSlave(productionMasterModel.ProductionMasterId);
        //    return RedirectToAction(nameof(ProductionMasterSlave), "ProductionMaster", new { companyId = productionMasterModel.CompanyId, productionMasterId = productionMasterModel.ProductionMasterId });
        //}

        [HttpGet]
        public async Task<ActionResult> ProductionMasterSlaveProcessing(int companyId = 0, int productionMasterId = 0, string productType = "R")
        {
            ProductionMasterModel model = new ProductionMasterModel();
            model.IsSubmitted = false;
            
            model = await Task.Run(() => productionMasterService.ProductionDetailsGet(companyId, productionMasterId));
            //if(model.ProductionStatusId == 0)
            //{
            //    model.ProductionStatusId = 1;
            //}
            model.CompanyId = companyId;
            model.CompanyFK = companyId;
            model.ProductionDate = DateTime.Now;
            model.ProductCategories = productCategoryService.GetProductCategorySelectModelByCompany(companyId, productType);
            model.Units = unitService.GetUnitSelectModels(companyId);
            model.ProductionStatusList = productionMasterService.GetProductionStatusList(companyId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ProductionMasterSlaveProcessing(ProductionMasterModel productionMasterModel)
        {
            if (productionMasterModel.ActionEum == ActionEnum.Add &&  productionMasterModel.ProductionMasterId == 0)
            {
                if (productionMasterModel.ProductionMasterId == 0)
                {
                    productionMasterModel.ProductionMasterId = await productionMasterService.ProductionAddProcessing(productionMasterModel);
                    await productionMasterService.ProductionDetailProcessingAdd(productionMasterModel);
                }
                
            }
            else if (productionMasterModel.ActionEum == ActionEnum.Edit)
            {
                //Delete
                await productionMasterService.ProductionDetailEdit(productionMasterModel);
            }
            else if (productionMasterModel.ActionEum == ActionEnum.Add && productionMasterModel.ProductionMasterId > 0)
            {
                var stockInfoIdVal = Convert.ToInt32(Session["StockInfoId"]);
                await productionMasterService.SubmitProductionMastersFromSlaveProcessing(productionMasterModel, stockInfoIdVal);
            }
            return RedirectToAction(nameof(ProductionMasterSlaveProcessing), new { companyId = productionMasterModel.CompanyId, productionMasterId = productionMasterModel.ProductionMasterId });
        }






        #region production master other actions

        [HttpPost]
        public async Task<JsonResult> GetSingleProductionDetailById(long id)
        {
            var model = await productionMasterService.GetSingleProductionDetailById(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductionStatusById(long id)
        {
            var model = await productionMasterService.GetSingleProductionStatusById(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> ProductionList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);

            if (!toDate.HasValue) toDate = DateTime.Now;
            ProductionMasterModel productionMasterModel = new ProductionMasterModel();
            productionMasterModel = await productionMasterService.GetProductionList(companyId, fromDate, toDate, vStatus);
            //productionMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            //productionMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            productionMasterModel.FromDate = fromDate;
            productionMasterModel.ToDate = toDate;
            productionMasterModel.ProductionStatusId = vStatus ?? -1;
            productionMasterModel.UserId = System.Web.HttpContext.Current.User.Identity.Name;
            productionMasterModel.ProductionStatusList = productionMasterService.GetProductionStatusList(companyId);
            return View(productionMasterModel);
        }
        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> ProductionList(ProductionMasterModel productionMasterModel)
        {
            if (productionMasterModel.CompanyId > 0)
            {
                Session["CompanyId"] = productionMasterModel.CompanyId;
            }
            productionMasterModel.ProductionDate = Convert.ToDateTime(productionMasterModel.ProductionDate);
            return RedirectToAction(nameof(ProductionList), new { companyId = productionMasterModel.CompanyId, fromDate = productionMasterModel.FromDate, toDate = productionMasterModel.ToDate, vStatus = productionMasterModel.ProductionStatusId });
        }


        //processing list ProductionProcessingList
        [HttpGet]
        public async Task<ActionResult> ProductionProcessingList(int companyId, DateTime? fromDate, DateTime? toDate)
        {
            if (!fromDate.HasValue) { fromDate = DateTime.Now.AddMonths(-2); }
            if (!toDate.HasValue) { toDate = DateTime.Now; }
            ProductionMasterModel productionMasterModel = new ProductionMasterModel();
            productionMasterModel = await productionMasterService.GetProductionProcessingList(companyId, fromDate, toDate);
            productionMasterModel.FromDate = fromDate;
            productionMasterModel.ToDate = toDate;
            productionMasterModel.UserId = System.Web.HttpContext.Current.User.Identity.Name;
            return View(productionMasterModel);
        }
        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> ProductionProcessingList(ProductionMasterModel productionMasterModel)
        {
            if (productionMasterModel.CompanyId > 0)
            {
                Session["CompanyId"] = productionMasterModel.CompanyId;
            }
            productionMasterModel.ProductionDate = Convert.ToDateTime(productionMasterModel.ProductionDate);
            return RedirectToAction(nameof(ProductionProcessingList), new { companyId = productionMasterModel.CompanyId, fromDate = productionMasterModel.FromDate, toDate = productionMasterModel.ToDate });
        }






        [HttpPost]
        public async Task<ActionResult> DeleteProductionDetailId(ProductionMasterModel productionMasterModel)
        {
            if (productionMasterModel.ActionEum == ActionEnum.Delete)
            {
                productionMasterModel.ProductionMasterId = await productionMasterService.ProductionDetailDeleteById(productionMasterModel.productionDetailModel.ProductionDetailsId);
            }
            return RedirectToAction(nameof(ProductionMasterSlave), new { companyId = productionMasterModel.CompanyId, productionMasterId = productionMasterModel.ProductionMasterId });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProductionDetailProcessingId(ProductionMasterModel productionMasterModel)
        {
            if (productionMasterModel.ActionEum == ActionEnum.Delete)
            {
                productionMasterModel.ProductionMasterId = await productionMasterService.ProductionDetailDeleteById(productionMasterModel.productionDetailModel.ProductionDetailsId);
            }
            return RedirectToAction(nameof(ProductionMasterSlaveProcessing), new { companyId = productionMasterModel.CompanyId, productionMasterId = productionMasterModel.ProductionMasterId });
        }

        #endregion
    }

}