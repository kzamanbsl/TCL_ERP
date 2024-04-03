using KGERP.Data.CustomModel;
using KGERP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.ServiceModel
{
    public class BillingGeneratedMapModel
    {
        
            public int BillingMapId { get; set; }
            public string BillGeneratedNo { get; set; }
            public long MaterialReceiveId { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            public Nullable<bool> IsActive { get; set; }

            
        }
}
