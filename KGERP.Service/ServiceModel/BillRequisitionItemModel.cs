using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;

namespace KGERP.Service.ServiceModel
{
    public class BillRequisitionItemModel : BaseVM
    {
        public int BillRequisitionItemId { get; set; }
        public int UnitId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public List<VMCommonUnit> Units { get; set; }
        public List<BillRequisitionItem> BillRequisitionItems { get; set; }
    }
}