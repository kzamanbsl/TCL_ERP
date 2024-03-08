using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public interface IChequeRegisterService
    {
        Task<bool> Add(ChequeRegisterModel model);
        Task<bool> Edit(ChequeRegisterModel model);
        Task<bool> Delete(ChequeRegisterModel model);
        Task<bool> ChequeSign(long chequeRegisterId);
        Task<ChequeRegisterModel> GetChequeRegisterById(long chequeRegisterId);
        Task<List<ChequeRegisterModel>> GetChequeRegisterList(int companyId);
        Task<List<ChequeRegisterModel>> GetSignedChequeList(int companyId);
        Task<List<ChequeRegisterModel>> GetChequeRegisterListByDate(ChequeRegisterModel model);
    }
}
