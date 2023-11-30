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
    public class BillRequisitionController : BaseController
    {
        private readonly IBillRequisitionService _service;
        private readonly ConfigurationService _configurationService;
        private readonly ProductService _ProductService;

        public BillRequisitionController(IBillRequisitionService billRequisitionService, ConfigurationService configurationService, ProductService productService)
        {
            _service = billRequisitionService;
            _configurationService = configurationService;
            _ProductService = productService;
        }

        #region All Json Action Method for Requisition

        // get material budget info
        public async Task<JsonResult> GetMaterialBudgetInfo(long boqId, long productId )
        {
            decimal EstimateQty = 0;
            decimal UnitRate = 0;
            decimal ReceivedSoFar = 0;
            decimal RemainingQty = 0;

            var getData = await _service.BoqMaterialBudget(boqId, productId);
            if(getData != null)
            {
                EstimateQty = (decimal) getData.EstimatedQty;
                UnitRate = (decimal)getData.UnitRate;
                ReceivedSoFar = await _service.ReceivedSoFarTotal(boqId, productId);
                RemainingQty = EstimateQty - ReceivedSoFar;
            }

            return Json(new { EstimateQty = EstimateQty, UnitRate = UnitRate, ReceivedSoFar = ReceivedSoFar, RemainingQty = RemainingQty }, JsonRequestBehavior.AllowGet);
        }

        // Get Unit Info by Id
        public async Task<JsonResult> GetUnitNameWithId(int id)
        {
            var unitName = "";
            var unitId = 0;
            if (id > 0)
            {
                unitId = (int)_ProductService.GetProductJson().FirstOrDefault(c => c.ProductId == id).UnitId;
                unitName = _configurationService.GetUnitForJson().FirstOrDefault(c => c.UnitId == unitId).Name;
            }

            var result = new { unitId = unitId, unitName = unitName };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Dependent Project List
        public JsonResult GetProjectList(int id)
        {
            var projectList = _service.GetProjectListByTypeId(id);

            return Json(projectList, JsonRequestBehavior.AllowGet);
        }

        // Dependent BoQ List
        public JsonResult GetBoQList(int id)
        {
            var boQList = _service.GetBillOfQuotationListByProjectId(id);

            return Json(boQList, JsonRequestBehavior.AllowGet);
        }

        // Dependent BoQ material List
        public JsonResult getBoqMaterialList(int id)
        {
            var materialList = _service.GetMaterialDetailWithNameAndUnitId(id);

            return Json(materialList, JsonRequestBehavior.AllowGet);
        }

        // Get Material Info by Product Id
        public async Task<JsonResult> GetBudgetInfoByProductId(int id)
        {
            decimal EstimateQty = 0;
            decimal ReceivedSoFar = 0;
            decimal UnitRate = 0;

            if (id > 0)
            {
                EstimateQty = (_service.GetBoQProductMapList().FirstOrDefault(c => c.ProductId == id).EstimatedQty == null) ? EstimateQty = 0 : (decimal)_service.GetBoQProductMapList().FirstOrDefault(c => c.ProductId == id).EstimatedQty;
                UnitRate = (_service.GetBoQProductMapList().FirstOrDefault(c => c.ProductId == id).UnitRate == null) ? UnitRate = 0 : (decimal)_service.GetBoQProductMapList().FirstOrDefault(c => c.ProductId == id).UnitRate;
                //ReceivedSoFar = _service.ReceivedSoFarTotal(id);
            }

            var result = new { EstimateQty = EstimateQty, UnitRate = UnitRate, ReceivedSoFar = ReceivedSoFar };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Filter with project id
        public JsonResult getBoqDivisionList(long id)
        {
            var boqDivisionList = _service.BoQDivisionList().Where(c => c.ProjectId == id).ToList();
            return Json(boqDivisionList, JsonRequestBehavior.AllowGet);
        }

        // Filter with boq division id
        public JsonResult getBoqItemList(long id)
        {
            var boqItemList = _service.GetBillOfQuotationList().Where(c => c.BoQDivisionId == id).ToList();
            return Json(boqItemList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Project Type

        public async Task<ActionResult> CostCenterType(int companyId)
        {
            var viewData = new CostCenterTypeModel()
            {
                CompanyFK = companyId,
                CostCenterTypes = await _service.GetCostCenterTypeList(companyId)
            };
            return View(viewData);
        }

        [HttpPost]
        public async Task<ActionResult> CostCenterType(CostCenterTypeModel model)
        {
            try
            {
                if (model.ActionEum == ActionEnum.Add)
                {
                    // Add 
                    await _service.Add(model);
                }
                else if (model.ActionEum == ActionEnum.Edit)
                {
                    // Edit
                    await _service.Edit(model);
                }
                else if (model.ActionEum == ActionEnum.Delete)
                {
                    // Delete
                    await _service.Delete(model);
                }
                else
                {
                    return View("Error");
                }

                return RedirectToAction(nameof(CostCenterType), new { companyId = model.CompanyFK });
            }
            catch (Exception ex)
            {
                // Handle exceptions, log the error, or return an error view
                return View("Error");
            }
        }

        #endregion

        #region Project Manager Assign
        [HttpGet]
        public async Task<ActionResult> CostCenterManagerMap(int companyId)
        {
            var viewData = new CostCenterManagerMapModel()
            {
                CompanyFK = companyId,
                Projects = await _service.GetProjectList(companyId),
                Employees = await _service.GetEmployeeList(companyId),
                CostCenterManagerMapModels = await _service.GetCostCenterManagerMapList(companyId),
            };

            return View(viewData);
        }

        [HttpPost]
        public ActionResult CostCenterManagerMap(CostCenterManagerMapModel model)
        {
            try
            {
                if (model.ActionEum == ActionEnum.Add)
                {
                    _service.Add(model);
                }
                else if (model.ActionEum == ActionEnum.Edit)
                {
                    _service.Edit(model);
                }
                else if (model.ActionEum == ActionEnum.Delete)
                {
                    _service.Delete(model);
                }
                else
                {
                    return View("Error");
                }

                return RedirectToAction(nameof(CostCenterManagerMap), new { companyId = model.CompanyFK });
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion

        #region BoQ Unit
        public async Task<JsonResult> SingleCommonUnit(int id)
        {

            VMCommonUnit model = new VMCommonUnit();
            model = await _configurationService.GetSingleCommonUnit(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> CommonUnit(int companyId)
        {

            VMCommonUnit vmCommonUnit = new VMCommonUnit();
            vmCommonUnit = await Task.Run(() => _configurationService.GetUnit(companyId));
            return View(vmCommonUnit);
        }

        [HttpPost]
        public async Task<ActionResult> CommonUnit(VMCommonUnit vmCommonUnit)
        {

            if (vmCommonUnit.ActionEum == ActionEnum.Add)
            {
                if (vmCommonUnit.IsBoQUnit == false)
                {
                    vmCommonUnit.IsBoQUnit = true;
                }
                //Add 
                await _configurationService.UnitAdd(vmCommonUnit);
            }
            else if (vmCommonUnit.ActionEum == ActionEnum.Edit)
            {
                //Edit
                await _configurationService.UnitEdit(vmCommonUnit);
            }
            else if (vmCommonUnit.ActionEum == ActionEnum.Delete)
            {
                //Delete
                await _configurationService.UnitDelete(vmCommonUnit.ID);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(CommonUnit), new { companyId = vmCommonUnit.CompanyFK });
        }

        [HttpPost]
        public async Task<ActionResult> CommonUnitDelete(VMCommonUnit vmCommonUnit)
        {

            if (vmCommonUnit.ActionEum == ActionEnum.Delete)
            {
                //Delete
                await _configurationService.UnitDelete(vmCommonUnit.ID);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(CommonUnit));
        }

        public async Task<ActionResult> CommonUnitGet(int companyId)
        {
            var dataList = await Task.Run(() => _configurationService.CompanyMenusGet(companyId));
            var list = dataList.Select(x => new { Value = x.ID, Text = x.Name }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BoQ Division

        public async Task<ActionResult> BoqDivision(int companyId)
        {
            BoqDivisionModel viewData = new BoqDivisionModel()
            {
                CompanyFK = companyId,
                Projecs = await _service.GetProjectList(companyId),
                BoQDivisions = _service.BoQDivisionList()
            };
            return View(viewData);
        }

        [HttpPost]
        public ActionResult BoqDivision(BoqDivisionModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                _service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                _service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                _service.Delete(model);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(BoqDivision), new { companyId = model.CompanyFK });
        }

        #endregion

        #region Bill of Quotation

        [HttpGet]
        public async Task<ActionResult> BillOfQuotation(int companyId)
        {
            var viewData = new BillRequisitionBoqModel()
            {
                CompanyFK = companyId,
                Projects = await _service.GetProjectList(companyId),
                BoQDivisions = _service.BoQDivisionList(),
                BillBoQItems = _service.GetBillOfQuotationList(),
                BoQUnits = _configurationService.GetUnitForJson()
            };
            return View(viewData);
        }

        [HttpPost]
        public ActionResult BillOfQuotation(BillRequisitionBoqModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                _service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                _service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                _service.Delete(model);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(BillOfQuotation), new { companyId = model.CompanyFK });
        }

        #endregion

        #region Budget & Estimating

        [HttpGet]
        public async Task<ActionResult> BillRequisitionItemBoQMap(int companyId)
        {
            BillRequisitionItemBoQMapModel viewData = new BillRequisitionItemBoQMapModel()
            {
                CompanyFK = companyId,
                Projects = await _service.GetProjectList(companyId),
                BoQDivisions = _service.BoQDivisionList(),
                BoQItems = _service.GetBillOfQuotationList(),
                BoQMaterials = _ProductService.GetProductJson(),
                BoQItemProductMaps = _service.GetBoQProductMapList()
            };

            return View(viewData);
        }

        [HttpPost]
        public ActionResult BillRequisitionItemBoQMap(BillRequisitionItemBoQMapModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                _service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                _service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                _service.Delete(model);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(BillRequisitionItemBoQMap), new { companyId = model.CompanyFK });
        }
        #endregion

        #region Requisition Type

        public ActionResult BillRequisitionType(int companyId)
        {
            var viewData = new BillRequisitionTypeModel()
            {
                CompanyFK = companyId,
                BillRequisitionTypes = _service.GetBillRequisitionTypeList()
            };
            return View(viewData);
        }

        [HttpPost]
        public ActionResult BillRequisitionType(BillRequisitionTypeModel model)
        {
            if (model.ActionEum == ActionEnum.Add)
            {
                //Add 
                _service.Add(model);
            }
            else if (model.ActionEum == ActionEnum.Edit)
            {
                //Edit
                _service.Edit(model);
            }
            else if (model.ActionEum == ActionEnum.Delete)
            {
                //Delete
                _service.Delete(model);
            }
            else
            {
                return View("Error");
            }
            return RedirectToAction(nameof(BillRequisitionType), new { companyId = model.CompanyFK });
        }

        #endregion

        #region 1.1 BillRequisition Basic CRUD Circle

        [HttpGet]
        public async Task<ActionResult> BillRequisitionMasterSlave(int companyId, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId == 0)
            {
                billRequisitionMasterModel.CompanyFK = companyId;
                billRequisitionMasterModel.StatusId = EnumBillRequisitionStatus.Draft;
            }
            else
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, billRequisitionMasterId);
            }
            billRequisitionMasterModel.ProjectTypeList = new SelectList(await _service.GetCostCenterTypeList(companyId), "CostCenterTypeId", "Name");
            billRequisitionMasterModel.ProjectList = new SelectList(await _service.GetProjectList(companyId), "CostCenterId", "Name");
            billRequisitionMasterModel.RequisitionTypeList = new SelectList(_service.GetBillRequisitionTypeList(), "BillRequisitionTypeId", "Name");
            billRequisitionMasterModel.RequisitionItemList = new SelectList(_ProductService.GetProductJson(), "ProductId", "ProductName");
            billRequisitionMasterModel.BOQItemList = new SelectList(_service.GetBillOfQuotationList(), "BoQItemId", "Name");
            billRequisitionMasterModel.UnitList = new SelectList(_configurationService.GetUnitForJson(), "UnitId", "Name");

            return View(billRequisitionMasterModel);

        }

        [HttpPost]
        public async Task<ActionResult> BillRequisitionMasterSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {

            if (billRequisitionMasterModel.ActionEum == ActionEnum.Add)
            {
                if (billRequisitionMasterModel.BillRequisitionMasterId == 0)
                {
                    billRequisitionMasterModel.BillRequisitionMasterId = await _service.BillRequisitionMasterAdd(billRequisitionMasterModel);

                }
                await _service.BillRequisitionDetailAdd(billRequisitionMasterModel);
            }
            else if (billRequisitionMasterModel.ActionEum == ActionEnum.Edit)
            {
                await _service.BillRequisitionDetailEdit(billRequisitionMasterModel);
            }
            return RedirectToAction(nameof(BillRequisitionMasterSlave), new { companyId = billRequisitionMasterModel.CompanyFK, billRequisitionMasterId = billRequisitionMasterModel.BillRequisitionMasterId });
        }

        [HttpPost]
        public async Task<ActionResult> SubmitBillRequisitionMaster(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            billRequisitionMasterModel.BillRequisitionMasterId = await _service.SubmitBillRequisitionMaster(billRequisitionMasterModel.BillRequisitionMasterId);
            return RedirectToAction(nameof(BillRequisitionMasterSlave), new { companyId = billRequisitionMasterModel.CompanyFK, BillRequisitionMasterId = billRequisitionMasterModel.BillRequisitionMasterId });
        }

        [HttpPost]
        public async Task<ActionResult> BillRequisitionMasterEdit(BillRequisitionMasterModel model)
        {
            if (model.ActionEum == ActionEnum.Edit)
            {
                await _service.BillRequisitionMasterEdit(model);
            }
            return RedirectToAction(nameof(BillRequisitionMasterList), new { companyId = model.CompanyFK });
        }

        [HttpPost]
        public async Task<JsonResult> GetBillRequisitionMasterById(long BillRequisitionMasterId)
        {
            var model = await _service.GetBillRequisitionMasterById(BillRequisitionMasterId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> SingleBillRequisitionDetails(long id)
        {
            var model = await _service.GetSingleBillRequisitionDetails(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteBillRequisitionDetailById(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.ActionEum == ActionEnum.Delete)
            {
                billRequisitionMasterModel.DetailModel.BillRequisitionDetailId = await _service.BillRequisitionDetailDelete(billRequisitionMasterModel.DetailModel.BillRequisitionDetailId);
            }
            return RedirectToAction(nameof(BillRequisitionMasterSlave), new { companyId = billRequisitionMasterModel.CompanyFK, BillRequisitionMasterId = billRequisitionMasterModel.BillRequisitionMasterId });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteBillRequisitionMasterById(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.ActionEum == ActionEnum.Delete)
            {
                billRequisitionMasterModel.BillRequisitionMasterId = await _service.BillRequisitionMasterDelete(billRequisitionMasterModel.BillRequisitionMasterId);
            }
            return RedirectToAction(nameof(BillRequisitionMasterList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> BillRequisitionMasterList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetBillRequisitionMasterList(companyId, fromDate, toDate, vStatus);

            billRequisitionMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            billRequisitionMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            if (vStatus == null)
            {
                vStatus = -1;
            }
            billRequisitionMasterModel.StatusId = (EnumBillRequisitionStatus)vStatus;
            //BillRequisitionMasterModel.ZoneList = new SelectList(procurementService.ZonesDropDownList(companyId), "Value", "Text");
            //BillRequisitionMasterModel.StockInfos = _stockInfoService.GetStockInfoSelectModels(companyId);
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public ActionResult BillRequisitionMasterSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(BillRequisitionMasterList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });
        }

        #region Common Bill Requisition List


        [HttpGet]
        public async Task<ActionResult> BillRequisitionMasterCommonSlave(int companyId, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId == 0)
            {
                billRequisitionMasterModel.CompanyFK = companyId;
                billRequisitionMasterModel.StatusId = EnumBillRequisitionStatus.Draft;
            }
            else
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, billRequisitionMasterId);
            }

            return View(billRequisitionMasterModel);

        }

        [HttpGet]
        public async Task<ActionResult> BillRequisitionMasterCommonList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetBillRequisitionMasterCommonList(companyId, fromDate, toDate, vStatus);

            billRequisitionMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            billRequisitionMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            if (vStatus == null)
            {
                vStatus = -1;
            }
            billRequisitionMasterModel.StatusId = (EnumBillRequisitionStatus)vStatus;
            //BillRequisitionMasterModel.ZoneList = new SelectList(procurementService.ZonesDropDownList(companyId), "Value", "Text");
            //BillRequisitionMasterModel.StockInfos = _stockInfoService.GetStockInfoSelectModels(companyId);
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public ActionResult BillRequisitionMasterCommonSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(BillRequisitionMasterCommonList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });

        }

        #endregion

        #endregion

        #region 1.2 PM BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> PMBRApproveSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> PMBRApproveSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var resutl = await _service.PMBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(PMBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> PMBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> PMBRRejectSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var result = await _service.PMBillRequisitionRejected(billRequisitionMasterModel);
            return RedirectToAction(nameof(PMBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> PMBRApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetPMBillRequisitionList(companyId, fromDate, toDate, vStatus);

            billRequisitionMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            billRequisitionMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            if (vStatus == null)
            {
                vStatus = -1;
            }
            billRequisitionMasterModel.StatusId = (EnumBillRequisitionStatus)vStatus;

            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public ActionResult PMBRApprovalListSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(PMBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });

        }

        #endregion

        #region 1.3  QS  BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> QSBRApproveSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> QSBRApproveSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var resutl = await _service.QSBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(QSBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> QSBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> QSBRRejectSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var result = await _service.QSBillRequisitionRejected(billRequisitionMasterModel);
            return RedirectToAction(nameof(QSBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> QSBRApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetQSBillRequisitionList(companyId, fromDate, toDate, vStatus);

            billRequisitionMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            billRequisitionMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            if (vStatus == null)
            {
                vStatus = -1;
            }
            billRequisitionMasterModel.StatusId = (EnumBillRequisitionStatus)vStatus;

            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public ActionResult QSBRApprovalListSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(QSBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });

        }

        #endregion

        #region 1.3.1  ITHead  BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> ITHeadBRApproveSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> ITHeadBRApproveSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var resutl = await _service.ITHeadBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(ITHeadBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> ITHeadBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> ITHeadBRRejectSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var result = await _service.ITHeadBillRequisitionRejected(billRequisitionMasterModel);
            return RedirectToAction(nameof(ITHeadBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> ITHeadBRApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetITHeadBillRequisitionList(companyId, fromDate, toDate, vStatus);

            billRequisitionMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            billRequisitionMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            if (vStatus == null)
            {
                vStatus = -1;
            }
            billRequisitionMasterModel.StatusId = (EnumBillRequisitionStatus)vStatus;

            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public ActionResult ITHeadBRApprovalListSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(ITHeadBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });

        }

        #endregion

        #region 1.4  Director  BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> DirectorBRApproveSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> DirectorBRApproveSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var resutl = await _service.DirectorBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(DirectorBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> DirectorBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> DirectorBRRejectSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var result = await _service.DirectorBillRequisitionRejected(billRequisitionMasterModel);
            return RedirectToAction(nameof(DirectorBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> DirectorBRApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetDirectorBillRequisitionList(companyId, fromDate, toDate, vStatus);

            billRequisitionMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            billRequisitionMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            if (vStatus == null)
            {
                vStatus = -1;
            }
            billRequisitionMasterModel.StatusId = (EnumBillRequisitionStatus)vStatus;

            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public ActionResult DirectorBRApprovalListSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(DirectorBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });

        }

        #endregion

        #region 1.5  PD  BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> PDBRApproveSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> PDBRApproveSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var resutl = await _service.PDBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(PDBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> PDBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> PDBRRejectSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var result = await _service.PDBillRequisitionRejected(billRequisitionMasterModel);
            return RedirectToAction(nameof(PDBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> PDBRApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetPDBillRequisitionList(companyId, fromDate, toDate, vStatus);

            billRequisitionMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            billRequisitionMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            if (vStatus == null)
            {
                vStatus = -1;
            }
            billRequisitionMasterModel.StatusId = (EnumBillRequisitionStatus)vStatus;

            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public ActionResult PDBRApprovalListSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(PDBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });

        }

        #endregion

        #region 1.6  MD  BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> MDBRApproveSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> MDBRApproveSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var resutl = await _service.MDBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(MDBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> MDBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> MDBRRejectSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var result = await _service.MDBillRequisitionRejected(billRequisitionMasterModel);
            return RedirectToAction(nameof(MDBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> MDBRApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetMDBillRequisitionList(companyId, fromDate, toDate, vStatus);

            billRequisitionMasterModel.StrFromDate = fromDate.Value.ToString("yyyy-MM-dd");
            billRequisitionMasterModel.StrToDate = toDate.Value.ToString("yyyy-MM-dd");
            if (vStatus == null)
            {
                vStatus = -1;
            }
            billRequisitionMasterModel.StatusId = (EnumBillRequisitionStatus)vStatus;

            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public ActionResult MDBRApprovalListSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(MDBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });

        }

        #endregion

    }
}