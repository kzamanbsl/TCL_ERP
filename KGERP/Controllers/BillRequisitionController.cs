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
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;

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

        #region Setting for Building

        // Floor for building
        [HttpGet]
        public async Task<ActionResult> AddNewFloor(int companyId = 0)
        {
            BuildingFloorModel viewModel = new BuildingFloorModel();
            viewModel.CompanyFK = companyId;
            viewModel.BuildingFloorModels = await _service.GetFloorList(companyId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewFloor(BuildingFloorModel model)
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

                return RedirectToAction(nameof(AddNewFloor), new { companyId = model.CompanyFK });
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // Member for building
        [HttpGet]
        public async Task<ActionResult> AddNewMember(int companyId = 0)
        {
            BuildingMemberModel viewModel = new BuildingMemberModel();
            viewModel.CompanyFK = companyId;
            viewModel.BuildingMemberModels = await _service.GetMemberList(companyId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewMember(BuildingMemberModel model)
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

                return RedirectToAction(nameof(AddNewMember), new { companyId = model.CompanyFK });
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        #endregion

        #region All Json Action Method for Requisition

        public JsonResult GetTotalAmountByRequisitionId(long requisitionId)
        {
            decimal result = _service.GetTotalByMasterId(requisitionId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // check BoQ Budget by BoQ item id
        public async Task<JsonResult> CheckBoqBudget(long boqItemId, long materialId)
        {
            bool result = await _service.IsBoqBudgetExistByBoqId(boqItemId, materialId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Check BoQ Number by Division id
        public async Task<JsonResult> CheckBoqNumber(long divisionId, string boqNumber)
        {
            bool result = await _service.IsBoqExistByDivisionId(divisionId, boqNumber);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // get material budget info
        public async Task<JsonResult> GetMaterialBudgetInfo(long projectId = 0, long boqId = 0, long productId = 0)
        {
            decimal EstimateQty = 0;
            decimal UnitRate = 0;
            decimal? ReceivedSoFar = 0;
            decimal? RemainingQty = 0;

            var getData = await _service.BoqMaterialBudget(boqId, productId);
            if (getData != null)
            {
                EstimateQty = (decimal)getData.EstimatedQty;
                UnitRate = (decimal)getData.UnitRate;
                ReceivedSoFar = _service.ReceivedSoFarTotal(projectId, boqId, productId);
                RemainingQty = EstimateQty - ReceivedSoFar;
            }

            return Json(new { EstimateQty = EstimateQty, UnitRate = UnitRate, ReceivedSoFar = ReceivedSoFar, RemainingQty = RemainingQty }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaterialBudgetInfoForOverhead(long projectId = 0, long productId = 0)
        {
            decimal EstimateQty = 0;
            decimal UnitRate = 0;
            decimal? ReceivedSoFar = 0;
            decimal? RemainingQty = 0;
            var boqId = 0;
            ReceivedSoFar = _service.ReceivedSoFarTotal(projectId, boqId, productId);

            return Json(new { EstimateQty = EstimateQty, UnitRate = UnitRate, ReceivedSoFar = ReceivedSoFar, RemainingQty = RemainingQty }, JsonRequestBehavior.AllowGet);
        }

        // Get Unit Info by Id
        public JsonResult GetUnitNameWithId(int id)
        {
            var unitName = "";
            var unitId = 0;
            if (id > 0)
            {
                var unit = _ProductService.GetProductJson().FirstOrDefault(c => c.ProductId == id);
                unitId = unit?.UnitId ?? 0;
                unitName = _configurationService.GetUnitForJson().FirstOrDefault(c => c.UnitId == unitId)?.Name??"";
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
        public JsonResult getBoqMaterialList(long id)
        {
            List<Product> materialList = null;
            if (id > 0)
            {
                materialList = _service.GetMaterialByBoqId(id);
            }

            return Json(materialList, JsonRequestBehavior.AllowGet);
        }

        // Dependent Subcategory List by BoQ and Type
        public JsonResult GetSubCategoryByTypeAndBoQ(long typeId, long boqId)
        {
            List<ProductSubCategory> subCategoryList = null;
            subCategoryList = _service.GetSubcategoryByTypeAndBoq(typeId, boqId);

            return Json(subCategoryList, JsonRequestBehavior.AllowGet);
        }

        // Dependent Subcategory List by BoQ id
        public JsonResult GetSubCategoryByBoqId(long id)
        {
            List<ProductSubCategory> subCategoryList = null;
            if (id > 0)
            {
                subCategoryList = _service.GetSubcategoryByBoq(id);
            }

            return Json(subCategoryList, JsonRequestBehavior.AllowGet);
        }

        // Dependent material list by product sub category id
        public JsonResult GetMaterialListByBoqIdAndSubcategoryId(long boqId, long subtypeId)
        {
            List<Product> materialList = null;
            if (boqId > 0 && subtypeId > 0)
            {
                materialList = _service.GetMaterialByBoqAndSubCategory(boqId, subtypeId);
            }

            return Json(materialList, JsonRequestBehavior.AllowGet);
        }

        // Dependent material list by product sub category id
        public JsonResult GetMaterialListBySubcategoryId(long id)
        {
            List<Product> materialList = null;
            if (id > 0)
            {
                materialList = _service.GetMaterialBySubCategory(id);
            }

            return Json(materialList, JsonRequestBehavior.AllowGet);
        }

        // Dependent BoQ material List for overhead
        public JsonResult getBoqMaterialListForOverHead(int id)
        {
            List<Product> materialList = null;
            if (id > 0)
            {
                materialList = _service.GetMaterialByBoqOverhead(id);
            }

            return Json(materialList, JsonRequestBehavior.AllowGet);
        }

        // Filter with project id
        public async Task<JsonResult> getBoqDivisionList(long id)
        {
            var boqDivisionList = await _service.GetBoqListByProjectId(id);
            return Json(boqDivisionList, JsonRequestBehavior.AllowGet);
        }

        // Filter with boq division id
        public async Task<JsonResult> getBoqItemList(long id)
        {
            var boqItemList = await _service.GetBoqListByDivisionId(id);
            return Json(boqItemList, JsonRequestBehavior.AllowGet);
        }

        // Filter with boq division id
        public async Task<JsonResult> getBoqItemListWithBoqNumber(long id)
        {
            var boqItemList = await _service.GetBoqListByDivisionId(id);

            var boqItemWithId = (from t1 in boqItemList
                                 select new BillRequisitionBoqModel
                                 {
                                     BoQItemId = t1.BoQItemId,
                                     Name = "(" + t1.BoQNumber + ") - " + t1.Name
                                 }).ToList();
            return Json(boqItemWithId, JsonRequestBehavior.AllowGet);
        }

        // get material info
        public async Task<JsonResult> GetMaterialInfo(long requisitionId, long productId)
        {
            decimal approvedDemand = 0M;
            decimal unitPrice = 0M;
            decimal ReceivedSoFar = 0M;
            decimal Remaining = 0M;

            try
            {
                var getData = await _service.ApprovedRequisitionDemand(requisitionId, productId);
                if (getData != null)
                {
                    return Json(getData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Json(new { ApprovedDemand = approvedDemand, ReceivedSoFar = ReceivedSoFar, Remaining = Remaining, UnitPrice = unitPrice }, JsonRequestBehavior.AllowGet);
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
                return View("Error");
            }
        }

        #endregion

        #region Project Manager Assign
        [HttpGet]
        public async Task<ActionResult> CostCenterManagerMap(int companyId)
        {

            var getEmployee = await _service.GetEmployeeList(companyId);
            List<Employee> EmployeeNameWithId = new List<Employee>();
            foreach (var item in getEmployee)
            {
                var data = new Employee()
                {
                    Id = item.Id,
                    Name = item.EmployeeId + " -- " + item.Name
                };
                EmployeeNameWithId.Add(data);
            }

            var viewData = new CostCenterManagerMapModel()
            {
                CompanyFK = companyId,
                Projects = await _service.GetProjectList(companyId),
                Employees = await _service.GetEmployeeList(companyId),
                EmployeesWithId = EmployeeNameWithId,
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
                Projects = await _service.GetProjectList(companyId),
                BoQDivisions = await _service.BoQDivisionList(companyId)
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
                BoQDivisions = await _service.BoQDivisionList(companyId),
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
                BoQDivisions = await _service.BoQDivisionList(companyId),
                BoQItems = _service.GetBillOfQuotationList(),
                BoQMaterials = _ProductService.GetProductJson(),
                BoQItemProductMaps = _service.GetBoQProductMapList(),
                ProjectTypes = await _service.GetCostCenterTypeList(companyId),
                BudgetTypes = _configurationService.GetAllProductCategoryList(companyId),
                BudgetSubtypes = _configurationService.GetAllProductSubCategoryList(companyId)
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

        #region Budget & Estimating Approval
        [HttpGet]
        public ActionResult BudgetAndEstimatingApprovalList(int companyId=0)
        {

            BillRequisitionMasterModel billRequisitionMaster= new BillRequisitionMasterModel();
            billRequisitionMaster.BoQItemProductMaps = _service.GetBoQProductMapList();
            return  View(billRequisitionMaster);  
        }

        [HttpPost]
        public ActionResult BudgetAndEstimatingApprovalList(BillRequisitionMasterModel model)
        {

            BillRequisitionMasterModel billRequisitionMaster= new BillRequisitionMasterModel();

            return  View(billRequisitionMaster);  
        }

        [HttpGet]
        public ActionResult BudgetAndEstimatingApproveSlave(int companyId=0)
        {

            BillRequisitionMasterModel billRequisitionMaster= new BillRequisitionMasterModel();
            
            
            return  View(billRequisitionMaster);  
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
            billRequisitionMasterModel.RequisitionTypeList = new SelectList(_configurationService.GetAllProductCategoryList(companyId), "ProductCategoryId", "Name");
            billRequisitionMasterModel.FloorList = new SelectList(await _service.GetFloorList(companyId), "BuildingFloorId", "Name");
            billRequisitionMasterModel.MemberList = new SelectList(await _service.GetMemberList(companyId), "BuildingMemberId", "Name");
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
            var result = await _service.PMBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(PMBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> PMBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
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
        public async Task<ActionResult> PMReject(BillRequisitionMasterModel model)
        {
            var result = await _service.PMBillRequisitionRejected(model);
            return RedirectToAction(nameof(PMBRApprovalList), new { companyId = result });
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

        #region 1.3 QS BillRequisition Approval Circle
        [HttpGet]
        public async Task<ActionResult> QSBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

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
            var result = await _service.QSBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(QSBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpPost]
        public async Task<ActionResult> QSReject(BillRequisitionMasterModel model)
        {
            var result = await _service.QSBillRequisitionRejected(model);
            return RedirectToAction(nameof(QSBRApprovalList), new { companyId = result });
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

        #region 1.3.1 ITHead BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> ITHeadBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

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
            var result = await _service.ITHeadBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(ITHeadBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpPost]
        public async Task<ActionResult> ITHeadReject(BillRequisitionMasterModel model)
        {
            var result = await _service.PMBillRequisitionRejected(model);
            return RedirectToAction(nameof(ITHeadBRApprovalList), new { companyId = result });
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

        #region 1.3.2 Fuel BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> FuelBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpGet]
        public async Task<ActionResult> FuelBRApproveSlave(int companyId = 0, long billRequisitionMasterId = 0)
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
        public async Task<ActionResult> FuelBRApproveSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var result = await _service.FuelBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(FuelBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpPost]
        public async Task<ActionResult> FuelReject(BillRequisitionMasterModel model)
        {
            var result = await _service.FuelBillRequisitionRejected(model);
            return RedirectToAction(nameof(FuelBRApprovalList), new { companyId = result });
        }

        [HttpGet]
        public async Task<ActionResult> FuelBRApprovalList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetFuelBillRequisitionList(companyId, fromDate, toDate, vStatus);

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
        public ActionResult FuelBRApprovalListSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(FuelBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });

        }

        #endregion

        #region 1.4 Director BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> DirectorBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

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
            var result = await _service.DirectorBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(DirectorBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpPost]
        public async Task<ActionResult> DirectorReject(BillRequisitionMasterModel model)
        {
            var result = await _service.DirectorBillRequisitionRejected(model);
            return RedirectToAction(nameof(DirectorBRApprovalList), new { companyId = result });
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

        #region 1.5 PD BillRequisition Approval Circle

        [HttpGet]
        public async Task<ActionResult> PDBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

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
            var result = await _service.PDBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(PDBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpPost]
        public async Task<ActionResult> PDReject(BillRequisitionMasterModel model)
        {
            var result = await _service.PDBillRequisitionRejected(model);
            return RedirectToAction(nameof(PDBRApprovalList), new { companyId = result });
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

        #region 1.6 MD BillRequisition Approval Circle
        [HttpGet]
        public async Task<ActionResult> MDBRRejectSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);

                billRequisitionMasterModel.VoucherPaymentStatus = await _service.GetRequisitionVoucherStatusMd(billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpGet]
        public async Task<ActionResult> MDBRApproveSlave(int companyId = 0, long billRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();

            if (billRequisitionMasterId > 0)
            {
                billRequisitionMasterModel = await _service.GetBillRequisitionMasterDetailWithApproval(companyId, billRequisitionMasterId);

                //billRequisitionMasterModel.VoucherPaymentStatus = await _service.GetRequisitionVoucherStatusMd(billRequisitionMasterId);
                billRequisitionMasterModel.DetailDataList = billRequisitionMasterModel.DetailList.ToList();
            }
            return View(billRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> MDBRApproveSlave(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            var result = await _service.MDBillRequisitionApproved(billRequisitionMasterModel);
            return RedirectToAction(nameof(MDBRApprovalList), new { companyId = billRequisitionMasterModel.CompanyFK });
        }

        [HttpPost]
        public async Task<ActionResult> MDReject(BillRequisitionMasterModel model)
        {
            var result = await _service.MDBillRequisitionRejected(model);
            return RedirectToAction(nameof(MDBRApprovalList), new { companyId = result });
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