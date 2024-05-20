using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.ServiceModel;

namespace KGERP.Service.Implementation
{
    public class BulkUploadService
    {
        private readonly ERPEntities _context;
        private readonly ConfigurationService _configurationService;

        public BulkUploadService(ERPEntities database, ConfigurationService configurationService)
        {
            _context = database;
            _configurationService = configurationService;
        }

        public bool ProductCategoryUpload(BulkUpload model)
        {
            if (model == null || model.FormFile == null || model.FormFile.ContentLength == 0)
            {
                return false;
            }

            try
            {
                using (var reader = new StreamReader(model.FormFile.InputStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var materialCategory = new VMCommonProductCategory();
                        materialCategory.Name = line;
                        materialCategory.ProductType = "R";
                        materialCategory.CompanyFK = model.CompanyId;
                        var result = _configurationService.ProductFinishCategoryAdd(materialCategory);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message.ToString();
                return false;
            }
        }


    }
}

