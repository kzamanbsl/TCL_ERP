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
            BulkUpload viewModel = new BulkUpload();
            viewModel.CompanyId = companyId;
            if (status > 0)
            {
                try
                {
                    var requisitionIds = _service.RequisitionIdList(status);
                    string tempDir = Server.MapPath("~/TempReports");
                    Directory.CreateDirectory(tempDir);

                    List<string> pdfFilePaths = new List<string>();

                    foreach (int requisitionId in requisitionIds)
                    {
                        string pdfFileName = $"ChequeRegisterReport_{requisitionId}.pdf";
                        string pdfFilePath = Path.Combine(tempDir, pdfFileName);

                        DownloadPdf(requisitionId, pdfFilePath);

                        pdfFilePaths.Add(pdfFilePath);
                    }

                    string zipFilePath = Path.Combine(tempDir, "TempReports.zip");
                    ZipFiles(pdfFilePaths, zipFilePath);

                    foreach (string pdfFilePath in pdfFilePaths)
                    {
                        System.IO.File.Delete(pdfFilePath);
                    }

                    return File(zipFilePath, "application/zip", "RequisitionReports.zip");
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

        private void DownloadPdf(int requisitionId, string filePath)
        {
            string reportUrl = $"http://localhost:60768/Report/TCLBillRequisiontReport?companyId=21&billRequisitionMasterId={requisitionId}";

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(reportUrl, filePath);
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

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
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