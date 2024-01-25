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
    public class ConsumptionService:IConsumptionService
    {
        private readonly ERPEntities _context;
        private readonly ConfigurationService _configurationService;
        public ConsumptionService( ERPEntities erpEntities,ConfigurationService configurationService)
        {
            _context = erpEntities;
            _configurationService = configurationService;
        }

        public Task<bool> CreateConsumption(ConsumptionModel consumption)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteConsumption(ConsumptionModel consumption)
        {
            throw new NotImplementedException();
        }

        public Task<ConsumptionModel> GetConsumptionById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateConsumption(ConsumptionModel consumption)
        {
            throw new NotImplementedException();
        }
    }
}
