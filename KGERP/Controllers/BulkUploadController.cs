using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Web.Mvc;
using KGERP.Service.Implementation;
using KGERP.Service.ServiceModel;
using KGERP.Utility;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class BulkUploadController : Controller
    {
        private readonly string _admin = "Administrator";
        private readonly string _password = "Gocorona!9";
        private BulkUploadService _service;

        public BulkUploadController(BulkUploadService bulkUploadService)
        {
            _service = bulkUploadService;
        }

        [HttpGet]
        public ActionResult MaterialBulkUpload(int companyId = 0, bool? result = null)
        {
            BulkUpload viewModel = new BulkUpload();
            viewModel.CompanyId = companyId;
            ViewBag.Result = result;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult MaterialBulkUpload(BulkUpload model)
        {
            bool response = false;
            if (model.FormFile != null)
            {
                if (model.UploadType == (int)EnumMaterialBulkUploadType.Category)
                {
                    response = _service.ProductCategoryUpload(model);
                }

                if (model.UploadType == (int)EnumMaterialBulkUploadType.Subcategory)
                {
                    //response = _service.ProductCategoryUpload(model);
                }

                if (model.UploadType == (int)EnumMaterialBulkUploadType.Material)
                {
                    //response = _service.ProductCategoryUpload(model);
                }
            }
            ViewBag.Result = response;
            return RedirectToAction(nameof(MaterialBulkUpload), new { companyId = model.CompanyId, result = response });
        }

        [HttpGet]
        public ActionResult RequisitionArchive(int companyId = 0, int? status = null)
        {
            BulkUpload viewModel = new BulkUpload
            {
                CompanyId = companyId,
                Status = status
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RequisitionArchive(BulkUpload model)
        {
            int status = 0;
            int companyId = model.CompanyId;
            long requisitionId = model.RequisitionStatus;

            if (requisitionId > 0)
            {
                try
                {
                    var requisitionList = _service.RequisitionIdList(requisitionId);
                    string tempDir = Server.MapPath("~/TempReports");
                    Directory.CreateDirectory(tempDir);

                    List<string> pdfFilePaths = new List<string>();

                    foreach (var requisition in requisitionList)
                    {
                        string pdfFileName = $"{requisition.BillRequisitionNo}.pdf";
                        string pdfFilePath = Path.Combine(tempDir, pdfFileName);

                        bool isSuccess = DownloadPdf(companyId, requisition.BillRequisitionMasterId, pdfFilePath);
                        if (isSuccess)
                        {
                            pdfFilePaths.Add(pdfFilePath);
                        }
                        else
                        {
                            return Content($"Failed to download PDF for requisition ID: {requisition.BillRequisitionNo}");
                        }
                    }

                    string requisitionStatus = Enum.GetName(typeof(EnumBillRequisitionStatus), requisitionId);
                    string zipFileName = $"{DateTime.Now.ToString("ddMMyyyy_HHmm")}_{requisitionStatus}_Report.zip";
                    string zipFilePath = Path.Combine(tempDir, zipFileName);
                    ZipFiles(pdfFilePaths, zipFilePath);

                    foreach (string pdfFilePath in pdfFilePaths)
                    {
                        System.IO.File.Delete(pdfFilePath);
                    }

                    status = 1;
                    TempData["ZipFileName"] = zipFileName;
                    return RedirectToAction(nameof(RequisitionArchive), new { companyId = model.CompanyId, status = status });
                }
                catch (Exception ex)
                {
                    //return Content($"An error occurred: {ex.Message}");
                    return RedirectToAction(nameof(RequisitionArchive), new { companyId = model.CompanyId, status = status });
                }
            }
            return RedirectToAction(nameof(RequisitionArchive), new { companyId = model.CompanyId, status = status });
        }

        private bool DownloadPdf(int companyId, long requisitionId, string filePath)
        {
            try
            {
                NetworkCredential authentication = new NetworkCredential(_admin, _password);
                using (WebClient client = new WebClient())
                {
                    client.Credentials = authentication;

                    var reportName = CompanyInfo.ReportPrefix + "BillRequisition";
                    string reportUrl = string.Format(
                        "http://192.168.0.7/ReportServer_SQLEXPRESS/?%2fErpReport/{0}&rs:Command=Render&rs:Format=PDF&CompanyId={1}&BillRequisitionMasterId={2}",
                        reportName, companyId, requisitionId
                    );

                    byte[] pdfData = client.DownloadData(reportUrl);

                    if (pdfData != null && pdfData.Length > 0)
                    {
                        System.IO.File.WriteAllBytes(filePath, pdfData);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ZipFiles(List<string> filePaths, string zipFilePath)
        {
            using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    foreach (string filePath in filePaths)
                    {
                        string entryName = Path.GetFileName(filePath);

                        ZipArchiveEntry entry = archive.CreateEntry(entryName);

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (Stream entryStream = entry.Open())
                            {
                                fileStream.CopyTo(entryStream);
                            }
                        }
                    }
                }
            }
        }
    }
}