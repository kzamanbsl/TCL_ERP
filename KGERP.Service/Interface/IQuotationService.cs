using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public interface IQuotationService
    {
        #region Quotation Master
        Task<long> QuotationMasterAdd(QuotationMasterModel model);
        Task<QuotationMasterModel> GetQuotationMasterDetail(int companyId, long quotationMasterId);
        Task<long> SubmitQuotationMaster(long quotationMasterId);
        #endregion

        #region Quotation Detail
        Task<long> QuotationDetailAdd(QuotationMasterModel model);
        Task<long> QuotationDetailEdit(QuotationMasterModel model);
        Task<long> QuotationDetailDelete(long id);
        #endregion
    }
}
