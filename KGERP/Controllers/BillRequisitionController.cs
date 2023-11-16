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

namespace KGERP.Controllers
{
    [SessionExpire]
    public class BillRequisitionController : BaseController
    {
        private readonly IBillRequisitionService _service;
        private readonly ConfigurationService _configurationService;

        public BillRequisitionController(IBillRequisitionService billRequisitionService)
        {
            _service = billRequisitionService;
        }

        #region BoQ Item

        [HttpGet]
        public ActionResult BillOfQuotation(int companyId)
        {
            var viewData = new BillRequisitionBoqModel()
            {
                CompanyFK = companyId,
                BillBoQItems = _service.GetBillOfQuotationList()
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

        #region Bill Requisition Item

        public ActionResult BillRequisitionItem(int companyId)
        {
            var viewData = new BillRequisitionItemModel()
            {
                CompanyFK = companyId,
                Units = _service.GetUnitList(companyId),
                BillRequisitionItems = _service.GetBillRequisitionItemList()
            };
            return View(viewData);
        }

        [HttpPost]
        public ActionResult BillRequisitionItem(BillRequisitionItemModel model)
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
            return RedirectToAction(nameof(BillRequisitionItem), new { companyId = model.CompanyFK });
        }

        #endregion

        #region Bill Requisition Type

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

        #region Cost Center Type

        public ActionResult CostCenterType(int companyId)
        {
            var viewData = new CostCenterTypeModel()
            {
                CompanyFK = companyId,
                CostCenterTypes = _service.GetCostCenterTypeList()
            };
            return View(viewData);
        }

        [HttpPost]
        public ActionResult CostCenterType(CostCenterTypeModel model)
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
            return RedirectToAction(nameof(CostCenterType), new { companyId = model.CompanyFK });
        }

        #endregion

        #region Cost Center Manager Map

        [HttpGet]
        public ActionResult CostCenterManagerMap(int companyId)
        {
            var viewData = new CostCenterManagerMapModel()
            {
                CompanyFK = companyId,
                Projects = _service.GetProjectList(),
                Employees = _service.GetEmployeeList(),
                CostCenterManagerMaps = _service.GetCostCenterManagerMapList(),
            };
            return View(viewData);
        }

        [HttpPost]
        public ActionResult CostCenterManagerMap(CostCenterManagerMapModel model)
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
            return RedirectToAction(nameof(CostCenterManagerMap), new { companyId = model.CompanyFK });
        }

        #endregion

        #region 1.1 BillRequisition Basic CRUD Circle

         
        [HttpGet]
        public async Task<ActionResult> BillRequisitionMasterSlave(int companyId = 0, long billRequisitionMasterId = 0)
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
            billRequisitionMasterModel.ProjectTypeList = new SelectList(_service.GetCostCenterTypeList().Where(x=>x.CompanyId== companyId).ToList(), "CostCenterTypeId", "Name");
            billRequisitionMasterModel.ProjectList = new SelectList(_service.GetProjectList().Where(x=>x.CompanyId== companyId).ToList(), "CostCenterId", "Name");
            billRequisitionMasterModel.RequisitionTypeList = new SelectList(_service.GetBillRequisitionTypeList(), "BillRequisitionTypeId", "Name");
            billRequisitionMasterModel.RequisitionItemList = new SelectList(_service.GetBillRequisitionItemList(), "BillRequisitionItemId", "Name");
            billRequisitionMasterModel.BOQItemList = new SelectList(_service.GetBillOfQuotationList(), "BoQItemId", "Name");
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

        #endregion

        #region 1.2  BillRequisition Received Circle

        [HttpGet]
        public async Task<ActionResult> PMBRReceivedSlave(int companyId = 0, long BillRequisitionMasterId = 0)
        {
            BillRequisitionMasterModel BillRequisitionMasterModel = new BillRequisitionMasterModel();

            if (BillRequisitionMasterId > 0)
            {
                BillRequisitionMasterModel = await _service.GetBillRequisitionMasterDetail(companyId, BillRequisitionMasterId);
                BillRequisitionMasterModel.DetailDataList = BillRequisitionMasterModel.DetailList.ToList();
            }

            return View(BillRequisitionMasterModel);
        }

        [HttpPost]
        public async Task<ActionResult> PMBRReceivedSlave(BillRequisitionMasterModel BillRequisitionMasterModel)
        {
            var resutl = await _service.PMBillRequisitionReceived(BillRequisitionMasterModel);
            return RedirectToAction(nameof(PMBRReceivedList), new { companyId = BillRequisitionMasterModel.CompanyFK });
        }

        [HttpGet]
        public async Task<ActionResult> PMBRReceivedList(int companyId, DateTime? fromDate, DateTime? toDate, int? vStatus)
        {
            if (!fromDate.HasValue) fromDate = DateTime.Now.AddMonths(-2);
            if (!toDate.HasValue) toDate = DateTime.Now;

            BillRequisitionMasterModel billRequisitionMasterModel = new BillRequisitionMasterModel();
            billRequisitionMasterModel = await _service.GetPMBillRequisitionMasterReceivedList(companyId, fromDate, toDate, vStatus);

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
        public ActionResult PMBillRequisitionMasterReceivedSearch(BillRequisitionMasterModel billRequisitionMasterModel)
        {
            if (billRequisitionMasterModel.CompanyFK > 0)
            {
                Session["CompanyId"] = billRequisitionMasterModel.CompanyFK;
            }

            billRequisitionMasterModel.FromDate = Convert.ToDateTime(billRequisitionMasterModel.StrFromDate);
            billRequisitionMasterModel.ToDate = Convert.ToDateTime(billRequisitionMasterModel.StrToDate);
            return RedirectToAction(nameof(PMBRReceivedList), new { companyId = billRequisitionMasterModel.CompanyFK, fromDate = billRequisitionMasterModel.FromDate, toDate = billRequisitionMasterModel.ToDate, vStatus = (int)billRequisitionMasterModel.StatusId });

        }

        #endregion


    }
}