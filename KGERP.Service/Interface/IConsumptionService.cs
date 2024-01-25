using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public  interface IConsumptionService
    {
         Task<ConsumptionModel> GetConsumptionById(int Id);
         Task<bool> CreateConsumption(ConsumptionModel consumption);
         Task<bool> UpdateConsumption(ConsumptionModel consumption);
         Task<bool> DeleteConsumption(ConsumptionModel consumption);

    }
}
