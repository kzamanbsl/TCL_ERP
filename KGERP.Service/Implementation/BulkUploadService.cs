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
using KGERP.Utility;

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
                        var values = line.Split(',');

                        var materialCategory = new VMCommonProductCategory();
                        if (Enum.TryParse<EnumAssetIntegration>(values[1], out var asset))
                        {
                            materialCategory.Asset = asset;
                        }
                        else
                        {
                            materialCategory.Asset = EnumAssetIntegration.Inventory;
                        }

                        materialCategory.Name = values[0];
                        materialCategory.Income = (values[2].ToUpper() == "NO" ? Convert.ToBoolean(false) : values[2].ToUpper() == "YES" ? Convert.ToBoolean(true) : false);
                        materialCategory.Expense = (values[3].ToUpper() == "NO" ? Convert.ToBoolean(false) : values[3].ToUpper() == "YES" ? Convert.ToBoolean(true) : false);
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

