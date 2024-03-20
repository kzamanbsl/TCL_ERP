using KGERP.Data.CustomModel;
using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class QuotationService : IQuotationService
    {
        private readonly ERPEntities _context;
        private readonly ConfigurationService _configurationService;
        public QuotationService(ERPEntities context, ConfigurationService configurationService)
        {
            _context = context;
            _configurationService = configurationService;
        }

        public Task<QuotationMasterModel> GetQuotationMasterDetail(int companyId, long quotationMasterId)
        {
            throw new NotImplementedException();
        }

        public async Task<long> QuotationMasterAdd(QuotationMasterModel model)
        {
            long result = -1;

            if (model.StatusId == 0)
            {
                model.StatusId = (int)EnumQuotationStatus.Draft;
            }

            QuotationMaster quotationMaster = new QuotationMaster
            {
                QutationDate = model.QuotationDate,
                QuotationNo = GetUniqueQuotationNo(),
                QuotationName = model.QuotationName,
                //SupplierId = model.SupplierId,
                QuotationForId = model.QuotationFor,
                BillRequisitionMasterId = model.RequisitionId,
                Description = model.Description,
                StatusId = (int)model.StatusId,
                //CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                //CreateDate = DateTime.Now,
                //IsActive = true
            };

            _context.QuotationMasters.Add(quotationMaster);

            if (await _context.SaveChangesAsync() > 0)
            {
                result = quotationMaster.QuotationMasterId;
            }
            return result;

            #region Generate Unique Requisition Number With Last Id

            // Generate Unique Quotation Number
            string GetUniqueQuotationNo()
            {
                int getLastRowId = _context.BillRequisitionMasters.Where(c => c.BRDate == model.QuotationDate).Count();

                string setZeroBeforeLastId(int lastRowId, int length)
                {
                    string totalDigit = "";

                    for (int i = (length - lastRowId.ToString().Length); 0 < i; i--)
                    {
                        totalDigit += "0";
                    }
                    return totalDigit + lastRowId.ToString();
                }

                string generatedNumber = $"QUO-{model.QuotationDate:yyMMdd}-{setZeroBeforeLastId(++getLastRowId, 4)}";
                return generatedNumber;
            }

            #endregion
        }

        public Task<long> QuotationDetailAdd(QuotationMasterModel model)
        {
            throw new NotImplementedException();
        }

        public Task<long> QuotationDetailEdit(QuotationMasterModel model)
        {
            throw new NotImplementedException();
        }
    }
}
