﻿using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class OrderMasterController : BaseController
    {
        private readonly IOrderMasterService orderMasterService;
        private readonly IEmployeeService employeeService;
        private readonly IVendorService vendorService;
        private readonly IProductCategoryService productCategoryService;
        private readonly IProductSubCategoryService productSubCategoryService;
        private readonly IProductService productService;
        private readonly IStockInfoService stockInfoService;
        public OrderMasterController(IEmployeeService employeeService, IOrderMasterService orderMasterService, IVendorService vendorService,
           IProductCategoryService productCategoryService, IProductSubCategoryService productSubCategoryService,
           IProductService productService, IStockInfoService stockInfoService)
        {
            this.orderMasterService = orderMasterService;
            this.employeeService = employeeService;
            this.vendorService = vendorService;
            this.productCategoryService = productCategoryService;
            this.productSubCategoryService = productSubCategoryService;
            this.productService = productService;
            this.stockInfoService = stockInfoService;
        }

        [SessionExpire]
        [HttpGet]
        public async Task<ActionResult> Index(int companyId, string productType, DateTime? fromDate, DateTime? toDate)
        {
            if (companyId > 0)
            {
                Session["CompanyId"] = companyId;
            }
            if (fromDate == null)
            {
                fromDate = DateTime.Now.AddMonths(-1);
            }

            if (toDate == null)
            {
                toDate = DateTime.Now;
            }
            OrderMasterModel orderMasterModels = new OrderMasterModel();
            //if (string.IsNullOrEmpty(productType)) productType = "F";
            
            orderMasterModels =await orderMasterService.GetOrderMasters(companyId, fromDate, toDate, productType);

            orderMasterModels.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            orderMasterModels.StrToDate = toDate.Value.ToString("yyyy-MM-dd");

            return View(orderMasterModels);
        }
        [HttpPost]
        [SessionExpire]
        public async Task<ActionResult> Index(OrderMasterModel model)
        {
            if (model.CompanyId > 0)
            {
                Session["CompanyId"] = model.CompanyId;
            }
            model.CompanyId = model.CompanyId;
            model.FromDate = Convert.ToDateTime(model.StrFromDate);
            model.ToDate = Convert.ToDateTime(model.StrToDate);
            model.ProductType = model.ProductType;
            return RedirectToAction(nameof(Index), new { companyId = model.CompanyId, productType = model.ProductType, fromDate = model.FromDate, toDate = model.ToDate });
        }

        [SessionExpire]
        [HttpGet]
        public ActionResult Create(int orderMasterId, string productType)
        {
            OrderMasterModel model = new OrderMasterModel();
            model = orderMasterService.GetOrderMaster(orderMasterId);
            model.ProductType = productType;
            model.MarketingOfficers = new List<SelectModel>();
            model.OrderLocations = stockInfoService.GetStockInfoSelectModels(model.CompanyId);
            if (model.CompanyId == (int)CompanyNameEnum.KrishibidFarmMachineryAndAutomobilesLimited)
            {
                return View("CreateOrEditForKFMAL", model);
            }
            else
            {
                if (productType.Equals("R"))
                {
                    return View("RMCreate", model);
                }
                return View(model);
            }
        }



        [SessionExpire]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderMasterModel model)
        {
            bool status = false;
            string message;
            status = orderMasterService.SaveOrder(model.OrderMasterId, model, out message);
            if (status)
            {
                TempData["successMessage"] = "Order generated successfully.";
            }
            else
            {
                TempData["successMessage"] = message;
            }
            return RedirectToAction("Index", new { companyId = model.CompanyId, productType = model.ProductType });


        }

        [SessionExpire]
        [HttpGet]
        public ActionResult Edit(int orderMasterId, string productType)
        {
            OrderMasterModel model = new OrderMasterModel();
            model = orderMasterService.GetOrderMaster(orderMasterId);
            VendorModel customer = vendorService.GetVendor(model.CustomerId);
            model.ProductType = productType;
            model.Customer = customer.Name;
            model.CustomerAddress = customer.Address;
            model.CustomerPhone = customer.Phone;
            EmployeeModel employee = employeeService.GetEmployee(model.SalePersonId ?? 0);

            model.MarketingOfficers = new List<SelectModel>() { new SelectModel() { Text = employee.Name, Value = employee.Id } };
            model.OrderLocations = stockInfoService.GetStockInfoSelectModels(model.CompanyId);
            model.Products = productService.GetProductSelectModelsByCompanyAndProductType(model.CompanyId, productType);
            if (model.CompanyId == (int)CompanyNameEnum.KrishibidFarmMachineryAndAutomobilesLimited)
            {
                return View("CreateOrEditForKFMAL", model);
            }
            else
            {
                if (productType.Equals("R"))
                {
                    return View("RMCreate", model);
                }
                return View(model);
            }

        }

        [SessionExpire]
        [HttpGet]
        public JsonResult GetCustomers()
        {
            List<SelectModel> customers = vendorService.GetCustomerSelectModel(Convert.ToInt32(Session["CompanyId"]), (int)ProviderEnum.Customer);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(customers, Formatting.Indented, jss);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [SessionExpire]
        [HttpGet]
        public JsonResult GetProductCategories(string type)
        {
            List<SelectModel> productCategories = productCategoryService.GetProductCategorySelectModelByCompany(Convert.ToInt32(Session["CompanyId"]), type);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(productCategories, Formatting.Indented, jss);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [SessionExpire]
        [HttpGet]
        public JsonResult GetProductSubCategories(int productCategoryId)
        {
            List<SelectModel> productSubCategories = productSubCategoryService.GetProductSubCategorySelectModelsByProductCategory(productCategoryId);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(productSubCategories, Formatting.Indented, jss);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [SessionExpire]
        [HttpGet]
        public JsonResult GetProducts(int productSubCategoryId)
        {
            List<SelectModel> products = productService.GetProductSelectModelsByProductSubCategory(productSubCategoryId);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(products, Formatting.Indented, jss);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        [SessionExpire]
        [HttpGet]
        public JsonResult GetCustomerInfo(int id)
        {
            var custInfo = orderMasterService.GetCustomerInfo(id);
            return Json(custInfo, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        [HttpGet]
        public JsonResult GetProductUnitPrice(int id)
        {
            var productInfo = orderMasterService.GetProductUnitPrice(id);
            return Json(productInfo, JsonRequestBehavior.AllowGet);
        }


        [SessionExpire]
        [HttpGet]
        public JsonResult GetProductUnitPriceCustomerWise(int id, int vendorId)
        {
            var productInfo = orderMasterService.GetProductUnitPriceCustomerWise(id, vendorId);
            return Json(productInfo, JsonRequestBehavior.AllowGet);
        }


        [SessionExpire]
        [HttpGet]
        public JsonResult GetProductAvgPurchasePrice(int id)
        {
            var avgprice = orderMasterService.GetProductAvgPurchasePrice(id);
            return Json(avgprice, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        [HttpGet]
        public ActionResult EditOrder(long id)
        {
            var data = orderMasterService.GetOrderInforForEdit(id);
            return View(data);
        }

        [SessionExpire]
        [HttpGet]
        public JsonResult GetOrderDetails(long orderMasterId)
        {
            var orderDetails = orderMasterService.GetOrderDetailInforForEdit(orderMasterId);
            return Json(orderDetails, JsonRequestBehavior.AllowGet);
        }



        [SessionExpire]
        [HttpGet]
        public async Task<ActionResult> KgEcomIndex(int companyId, string productType, DateTime? fromDate, DateTime? toDate)
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
            OrderMasterModel orderMasterModels = new OrderMasterModel();
            productType = "F";
            orderMasterModels = await orderMasterService.GetOrderMasters(companyId, fromDate, toDate, productType);

            orderMasterModels.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            orderMasterModels.StrToDate = toDate.Value.ToString("yyyy-MM-dd");

            return View(orderMasterModels);
          
        }



        [SessionExpire]
        [HttpGet]
        public ActionResult DeleteOrder(long orderMasterId, string productType)
        {
            bool result = orderMasterService.DeleteOrder(orderMasterId);
            if (result)
            {
                TempData["successMessage"] = "Order Deleted Successfully";
                return RedirectToAction("Index", new { companyId = Session["CompanyId"], productType });
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetNewOrderNo(int stockInfoId, string productType)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string orderNo = orderMasterService.GetNewOrderNo(companyId, stockInfoId, productType);
            return Json(orderNo, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult DeleteOrderDetail(long orderMasterId, long orderDetailId)
        {
            string productType = string.Empty;
            bool result = orderMasterService.DeleteOrderDetail(orderDetailId, out productType);
            return RedirectToAction("Edit", new { orderMasterId, productType });
        }

        [SessionExpire]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderEdit(OrderMasterModel model)
        {
            bool status = false;
            string message;
            status = orderMasterService.UpdateOrder(model, out message);
            if (status)
            {
                TempData["successMessage"] = "Order updated successfully.";
            }
            else
            {
                TempData["successMessage"] = message;
            }
            return RedirectToAction("Index", new { companyId = model.CompanyId, productType = model.ProductType });
        }

        public ActionResult Support(int companyId)
        {
            Session["CompanyId"] = companyId;
            return View();
        }


        public ActionResult SupportDeleteOrder(string orderNo)
        {
            TempData["successMessage"] = "Order not deleted";
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            bool result = orderMasterService.SupportDeleteOrderByOrderNo(companyId, orderNo);
            if (result)
            {
                TempData["successMessage"] = "Order deleted successfully";
            }
            return RedirectToAction("Support", new { companyId });
        }

    }
}