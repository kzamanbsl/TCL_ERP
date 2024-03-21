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
        Task<QuotationMasterModel> GetQuotationList(DateTime? fromDate, DateTime? toDate);
        #endregion

        #region Quotation Detail
        Task<long> QuotationDetailAdd(QuotationMasterModel model);
        long QuotationDetailEdit(QuotationMasterModel model);
        Task<QuotationDetailModel> QuotationDetailBbyId(long id);
        Task<long> QuotationDetailDelete(long id);
        #endregion
    }
}
