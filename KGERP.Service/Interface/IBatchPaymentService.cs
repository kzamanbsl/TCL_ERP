using KGERP.Service.ServiceModel;
using System;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public interface IBatchPaymentService
    {
        #region Customer Batch Collection
        Task<int> CustomerBatchPaymentMasterAdd(BatchPaymentMasterModel model);
        Task<int> CustomerBatchPaymentDetailAdd(BatchPaymentMasterModel model);
        Task<BatchPaymentMasterModel> GetCustomerBatchPaymentDetail(int companyId, int batchPaymentMasterId);
        Task<BatchPaymentDetailModel> GetCustomerBatchPaymentDetailById(int id);
        Task<int> CustomerBatchPaymentDetailEdit(BatchPaymentMasterModel model);
        Task<int> SubmitCustomerBatchPayment(int productionMasterId);
        Task<int> CustomerBatchPaymentDetailDeleteById(int batchPaymentDetailId);
        Task<BatchPaymentMasterModel> GetCustomerBatchPaymentList(int companyId, DateTime? fromDate, DateTime? toDate);
        #endregion


        #region Supplier Batch Payment

        // work on need

        #endregion
    }
}
