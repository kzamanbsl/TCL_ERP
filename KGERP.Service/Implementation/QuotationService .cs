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

        public Task<long> QuotationMasterAdd(QuotationMasterModel model)
        {
            throw new NotImplementedException();
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
