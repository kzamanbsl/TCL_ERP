using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class ConsumptionService : IConsumptionService
    {
        private readonly ERPEntities _context;
        private readonly ConfigurationService _configurationService;
        public ConsumptionService(ERPEntities erpEntities, ConfigurationService configurationService)
        {
            _context = erpEntities;
            _configurationService = configurationService;
        }

        public async Task<long> CreateConsumptionMaster(ConsumptionModel consumption)
        {
            var result = -1;
            if (consumption is null)
            {
                return await Task.Run(() => result);
            }

            ConsumptionMaster consumptionMaster = new ConsumptionMaster()
            {

                BoqItemId = consumption.BoqItemId,
                StoreId = consumption.StoreId,
                DivisionId = consumption.DivisionId,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreatedOn = DateTime.Now
            };
            try
            {
                var count = _context.SaveChanges();
                if (count > 0)
                {
                    return consumptionMaster.ConsumptionMasterId;
                }
            }
            catch (Exception e)
            {
                return result;
            }


            return result;
        }
        public async Task<bool> CreateConsumptionDetail(ConsumptionModel consumption)
        {
           if (consumption is null|| consumption.ConsumptionMasterId<=0)
            {
                return await Task.Run(() => false);
            }



            try
            {
                ConsumptionDetail consumptionDetail = new ConsumptionDetail()
                {
                    ConsumptionMasterId = consumption.ConsumptionMasterId,
                    ProductSubtypeId = consumption.ProductSubtypeId,
                    ProductId = consumption.ProductId,
                    UnitPrice = consumption.DetailModel.UnitPrice,
                    ConsumedQty = consumption.DetailModel.ConsumedQty,
                    RemainingQty = consumption.DetailModel.RemainingQty,
                    TotalAmount = (consumption.DetailModel.UnitPrice * consumption.DetailModel.ConsumedQty) ?? 0,
                    CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                    CreatedOn = DateTime.Now
                };
                _context.ConsumptionDetails.Add(consumptionDetail);
                _context.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);

            }

            return false;
        }
        public Task<bool> DeleteConsumption(ConsumptionModel consumption)
        {
            throw new NotImplementedException();
        }

        public Task<ConsumptionModel> GetConsumptionById(int Id)
        {
            throw new NotImplementedException();
        }

        public async  Task<bool> UpdateConsumptionDetail(ConsumptionModel consumption)
        {
            if (consumption is null || consumption.ConsumptionMasterId <= 0)
            {
                return await Task.Run(() => false);
            }
            var detail= _context.ConsumptionDetails.FirstOrDefault(c=> c.ConsumptionMasterId== consumption.ConsumptionMasterId && c.ConsumptionDetailsId==consumption.ConsumptionDetailsId);
            if (detail is null)
            {
                return await Task.Run(() => false);
            }


            try
            {

                detail.ProductSubtypeId = consumption.ProductSubtypeId;
                detail.ProductId = consumption.ProductId;
                detail.UnitPrice= consumption.DetailModel.UnitPrice;
                detail.ConsumedQty = consumption.DetailModel.ConsumedQty;
                detail.StoredQty=consumption.DetailModel?.StoredQty;
                detail.RemainingQty=consumption.DetailModel?.RemainingQty;
                detail.TotalAmount=consumption.DetailModel?.TotalAmount;
                detail.ModifiedBy=System.Web.HttpContext.Current.User.Identity.Name;
                detail.ModifiedOn = DateTime.Now;
                var count = _context.SaveChanges();
                if (count > 0)
                {
                    return true;
                }
            }
            catch(Exception e)
            {

            }


            return false;
        }

    }
}
