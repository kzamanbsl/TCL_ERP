using KGERP.Data.Models;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class BillRequisitionService : IBillRequisitionService
    {
        private readonly ERPEntities _context;
        public BillRequisitionService(ERPEntities context)
        {
            _context = context;
        }
        public int GetRequisitionNo()
        {
            int requisitionId = 0;
            //var value = _context.Requisitions.OrderByDescending(x => x.RequisitionId).FirstOrDefault();
            //if (value != null)
            //{
            //    requisitionId = value.RequisitionId + 1;
            //}
            //else
            //{
            //    requisitionId = requisitionId + 1;
            //}
            return requisitionId;
        }
    }
}
