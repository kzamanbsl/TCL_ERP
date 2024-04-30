﻿using KGERP.Data.Models;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KGERP.Service.Implementation.Warehouse;
using System.Web.Mvc;

namespace KGERP.Service.Interface
{
    public interface IMaterialReceiveService : IDisposable
    {
        Task<MaterialReceiveModel> GetMaterialReceivedList(int companyId, DateTime? fromDate, DateTime? toDate);
        Task<MaterialReceiveModel> GetMaterialIssuePendingList(int companyId, DateTime? fromDate, DateTime? toDate);
        Task<List<VMMaterialRcvList>> GetMaterialRcvList(int companyId, DateTime? fromDate, DateTime? toDate);
        Task<GCCLMaterialRecieveVm> GCCLMaterialRcvList(int companyId, DateTime? fromDate, DateTime? toDate);
        Task<KFMALMaterialRecieveVm> KFMALMaterialRcvList(int companyId, DateTime? fromDate, DateTime? toDate);
        List<MaterialReceiveModel> GetMaterialReceives(int companyId, string searchDate, string searchText, string type);
        VMWarehousePOReceivingSlave GetMaterialReceive(int companyId, long materialReceiveId);
        Task<long> SaveMaterialReceive(VMWarehousePOReceivingSlave vmPOReceivingSlave);
        List<MaterialReceiveModel> GetMaterialIssuePendingList(int companyId, string searchDate, string searchText);
       Task<SelectList> GetMaterialReceiveListByBoqItem(int companyId, int? projectId, int? BoqItem);
        Task<object> GetMaterialReceiveInfoByProductIdAndBoqItemId(int? BoqItem,int? ProductId);
        Task<object> GetReceivedWorkOrderNo(string Prefix);
        bool MaterialReceiveIssue(MaterialReceiveModel materialReceive);
        MaterialReceiveModel GetMaterialReceiveIssue(long id);
        IList<MaterialReceiveDetailModel> GetMaterialReceiveDetailIssue(long id);
        bool MaterialIssueCancel(VMWarehousePOReceivingSlave VMReceivingSlave);
        MaterialReceiveModel GetMaterialReceiveEdit(long id);
        bool MaterialReceiveEdit(MaterialReceiveModel materialReceive);
        //Task<long> SubmitMaterialIssue(MaterialReceiveModel model);

        //StoreModel GetStore(long id);
        //bool SaveStore(long id, StoreModel store);
        //List<SoreProductQty> GetStoreProductQty(int companyId);
        //List<SoreProductQty> GetRMStoreProductQty();
        //List<SoreProductQty> GetEcomProductStore();
        //StoreModel GetOpenningStore(long id);
        //bool StoreUpdateAfterProduction(StoreModel store, List<RequisitionItemModel> requistionItems);
        //bool SaveRMStore(long storeId, StoreModel store);
    }
}
