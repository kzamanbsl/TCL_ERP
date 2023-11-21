using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class BillRequisitionItemBoQMapModel : BaseVM
    {
        public int BoQItemId { get; set; }
        public int BillRequisitionItemId { get; set; }
        public int EstimateQuantity { get; set; }
        public decimal EstimateAmount { get; set; }

        public List<BillBoQItem> BillBoQItems { get; set; }
    }
}