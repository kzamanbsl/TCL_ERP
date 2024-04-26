using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Implementation.Warehouse;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public interface IChequeRegisterService
    {
        #region Bank Account Info

        Task<bool> Add(BankAccountInfoModel model);
        Task<bool> Edit(BankAccountInfoModel model);
        Task<bool> Delete(BankAccountInfoModel model);
        Task<BankAccountInfoModel> GetBankAccountInfoById(long bankAccountInfoId);
        Task<List<BankAccountInfoModel>> GetBankAccountInfoList(int companyId);

        #endregion

        #region Cheque Book Register

        Task<bool> Add(ChequeBookModel model);
        Task<bool> Edit(ChequeBookModel model);
        Task<bool> Delete(ChequeBookModel model);
        Task<ChequeBookModel> GetChequeBookById(long chequeBookId);
        Task<object> GetChequeBookInfo(long chequeBookId);
        Task<List<ChequeBookModel>> GetChequeBookList(int companyId);
        Task<List<ChequeBookModel>> GetChequeBookListByAccountInfo(int bankAccountInfoId);

        #endregion

        #region Cheque Register

        Task<bool> Add(ChequeRegisterModel model);
        Task<bool> Edit(ChequeRegisterModel model);
        Task<bool> Delete(ChequeRegisterModel model);
        Task<List<ChequeRegisterModel>> GetChequeRegisterListByDate(ChequeRegisterModel model);
        Task<ChequeRegisterModel> GetChequeRegisterById(long chequeRegisterId);
        Task<bool> ChequeSign(long chequeRegisterId);
        Task<bool> MakePdf(long chequeRegisterId);
        Task<bool> PrintCount(long chequeRegisterId);
        Task<List<ChequeRegisterModel>> GetChequeRegisterList(int companyId);
        Task<List<ChequeRegisterModel>> GetSignedChequeList(int companyId);
        Task<List<ChequeRegisterModel>> GetGeneratedChequeList(int companyId);
        List<object> RegisteredRequisitionList(int projectId);

        #endregion

        Task<string> GetPayeeNameBySupplierId(int supplierId);
    }
}
