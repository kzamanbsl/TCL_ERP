﻿using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public interface IQuotationService
    {
        #region Quotation Type
        Task<bool> Add(QuotationForModel model);
        Task<bool> Edit(QuotationForModel model);
        Task<bool> Delete(QuotationForModel model);
        Task<List<QuotationForModel>> GetQuotationTypeList(int companyId);
        #endregion

        #region Quotation Master
        Task<long> QuotationMasterAdd(QuotationMasterModel model);
        Task<QuotationMasterModel> GetQuotationMasterDetail(int companyId, long quotationMasterId);
        Task<long> SubmitQuotationMaster(long quotationMasterId);
        Task<QuotationMasterModel> GetQuotationList();
        Task<QuotationMasterModel> GetQuotationListFilterByStatus();
        Task<QuotationMasterModel> GetQuotationListByStatusId(int id);
        Task<List<QuotationMasterModel>> GetQuotationListWithNameAndNo();
        Task<QuotationMasterModel> GetQuotationListByDate(QuotationMasterModel model);
        Task<QuotationMasterModel> GetQuotationListByDateAndStatus(QuotationMasterModel model);
        Task<bool> QuotationMasterDelete(long id);
        Task<bool> CloseQuotationById(long id);
        Task<bool> OpenQuotationById(long id);
        #endregion

        #region Quotation Detail
        Task<long> QuotationDetailAdd(QuotationMasterModel model);
        long QuotationDetailEdit(QuotationMasterModel model);
        Task<QuotationDetailModel> QuotationDetailBbyId(long id);
        Task<long> QuotationDetailDelete(long id);
        #endregion

        #region Comparatative Statement
        Task<QuotationCompareModel> GetComparedQuotation(long quotationIdOne, long quotationIdTwo);
        #endregion
    }
}
