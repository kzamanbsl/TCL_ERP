using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KGERP.Service.Implementation;
using KGERP.Service.Implementation.FTP;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using Microsoft.AspNetCore.Hosting.Server;

namespace KGERP.Controllers
{
    [SessionExpire]
    public class BulkUploadController : Controller
    {
        private BulkUploadService _service;

        public BulkUploadController(BulkUploadService bulkUploadService)
        {
            _service = bulkUploadService;
        }

        [HttpGet]
        public ActionResult MaterialCategoryUpload(int companyId = 0, bool? result = null)
        {
            BulkUpload viewModel = new BulkUpload();
            viewModel.CompanyId = companyId;
            ViewBag.Result = result;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult MaterialCategoryUpload(BulkUpload model)
        {
            bool response = false;
            if (model.FormFile != null)
            {
                response = _service.ProductCategoryUpload(model);
            }
            ViewBag.Result = response;
            return RedirectToAction(nameof(MaterialCategoryUpload), new { companyId = model.CompanyId, result = response });
        }

        [HttpGet]
        public ActionResult RequisitionArchive(int companyId = 0, long status = 0)
        {
            BulkUpload viewModel = new BulkUpload
            {
                CompanyId = companyId
            };

            if (status > 0)
            {
                try
                {
                    var requisitionList = _service.RequisitionIdList(status);
                    string tempDir = Server.MapPath("~/TempReports");
                    Directory.CreateDirectory(tempDir);

                    List<string> pdfFilePaths = new List<string>();

                    foreach (var requisition in requisitionList)
                    {
                        string pdfFileName = $"{requisition.BillRequisitionNo}.pdf";
                        string pdfFilePath = Path.Combine(tempDir, pdfFileName);

                        bool isSuccess = DownloadPdf((int)requisition.BillRequisitionMasterId, pdfFilePath);
                        if (isSuccess)
                        {
                            pdfFilePaths.Add(pdfFilePath);
                        }
                        else
                        {
                            // Handle the case where the PDF download failed
                            return Content($"Failed to download PDF for requisition ID: {requisition.BillRequisitionMasterId}");
                        }
                    }

                    string requisitionStatus = Enum.GetName(typeof(EnumBillRequisitionStatus), status);
                    string zipFileName = $"{DateTime.Now.ToString("ddMMyyyy_HHmm")}_{requisitionStatus}_Report.zip";
                    string zipFilePath = Path.Combine(tempDir, zipFileName);
                    ZipFiles(pdfFilePaths, zipFilePath);

                    foreach (string pdfFilePath in pdfFilePaths)
                    {
                        System.IO.File.Delete(pdfFilePath);
                    }

                    return File(zipFilePath, "application/zip", zipFileName);
                }
                catch (Exception ex)
                {
                    return Content($"An error occurred: {ex.Message}");
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RequisitionArchive(BulkUpload model)
        {
            return RedirectToAction(nameof(RequisitionArchive), new { companyId = model.CompanyId, status = model.RequisitionStatus });
        }

        private bool DownloadPdf(int requisitionId, string filePath)
        {
            string reportUrl = $"{Request.Url.Scheme}://{Request.Url.Host}:{Request.Url.Port}/Report/TCLBillRequisiontReport?companyId=21&billRequisitionMasterId={requisitionId}";

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/pdf");
                    byte[] pdfData = client.DownloadData(reportUrl);

                    if (pdfData != null && pdfData.Length > 0)
                    {
                        System.IO.File.WriteAllBytes(filePath, pdfData);
                        return true;
                    }
                    else
                    {
                        // Log that the download failed because the data was empty
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
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