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
        Task<List<ChequeRegisterModel>> GetChequeRegisterList(int companyId);
        Task<ChequeRegisterModel> GetChequeRegisterById(long chequeRegisterId);
        Task<bool> Add(ChequeRegisterModel model);
        Task<bool> Edit(ChequeRegisterModel model);
        Task<bool> Delete(ChequeRegisterModel model);
        Task<bool> ChequeSign(long chequeRegisterId);
    }
}
