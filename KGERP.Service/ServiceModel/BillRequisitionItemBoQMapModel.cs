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
        public long BoQItemProductMapId { get; set; }
        public int? BoQItemId { get; set; }
        public int MaterialItemId { get; set; }
        public decimal EstimatedQty { get; set; }
        public decimal EstimatedAmount { get; set; }
        public decimal UnitRate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectTypeName { get; set; }
        public long? BoQDivisionId { get; set; }
        public List<BillRequisitionBoqModel> BoQItems { get; set; }
        public List<Product> BoQMaterials { get; set; }
        public List<BoqDivisionModel> BoQDivisions { get; set; }
        public List<Accounting_CostCenter> Projects { get; set; }
        public List<BillRequisitionItemBoQMapModel> BoQItemProductMaps { get; set; }

        public string ProjectName { get; set; }
        public string DivisionName { get; set; }
        public string BoqName { get; set; }
        public string BoqNumber { get; set; }
        public string MaterialName { get; set; }
        public bool? IsApproved { get; set; }

        public int ProjectTypeId { get; set; }
        public List<Accounting_CostCenterType> ProjectTypes { get; set; }

        public int BudgetTypeId { get; set; }
        public string MaterialTypeName { get; set; }
        public List<ProductCategory> BudgetTypes { get; set; }

        public int BudgetSubtypeId { get; set; }
        public string MaterialSubtypeName { get; set; }
        public List<ProductSubCategory> BudgetSubtypes { get; set; }
        public BudgetAndEstimatingApprovalStatus BNEStatusId { get; set; }
        public SelectList EnumBNEStatusList { get { return new SelectList(BaseFunctionalities.GetEnumList<BudgetAndEstimatingApprovalStatus>(), "Value", "Text"); } }

    }
}