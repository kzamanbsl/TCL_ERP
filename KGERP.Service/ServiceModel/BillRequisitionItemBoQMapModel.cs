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
        public List<decimal?> EstimatedQtyList { get; set; } = new List<decimal?>();
        public decimal EstimatedAmount { get; set; }
        public decimal UnitRate { get; set; }
        public List<decimal?> UnitRateList { get; set; }= new List<decimal?>();
        public int ProjectId { get; set; }
        public string ProjectTypeName { get; set; }
        public decimal ProjectTotalValue { get; set; }
        public decimal TotalAmountBudget { get; set; }
        public decimal TotalGrossMargin { get; set; }
        public decimal TotalGrossMarginPercentage { get; set; }
        public long? BoQDivisionId { get; set; }
        public List<BillRequisitionBoqModel> BoQItems { get; set; } = new List<BillRequisitionBoqModel>();
        public List<Product> BoQMaterials { get; set; } = new List<Product>();
        public List<BoqDivisionModel> BoQDivisions { get; set; } = new List<BoqDivisionModel>();
        public List<Accounting_CostCenter> Projects { get; set; } = new List<Accounting_CostCenter>();
        public List<BillRequisitionItemBoQMapModel> BoQItemProductMaps { get; set; }
        public List<BillRequisitionItemBoQMapModel> BoQMapVmList { get; set; }

        public string ProjectName { get; set; }
        public string DivisionName { get; set; }
        public string BoqName { get; set; }
        public string BoqNumber { get; set; }
        public string MaterialName { get; set; }
        public int? ApprovalStatus { get; set; }

        public int ProjectTypeId { get; set; }
        public List<Accounting_CostCenterType> ProjectTypes { get; set; } = new List<Accounting_CostCenterType>();
        public int BudgetTypeId { get; set; }
        public string MaterialTypeName { get; set; }
        public List<ProductCategory> BudgetTypes { get; set; } = new List<ProductCategory>();

        public int BudgetSubtypeId { get; set; }
        public string MaterialSubtypeName { get; set; }
        public List<ProductSubCategory> BudgetSubtypes { get; set; } = new List<ProductSubCategory>();
        public EnumBudgetAndEstimatingApprovalStatus? BNEApprovalStatus { get; set; }
        public SelectList EnumBNEStatusList { get { return new SelectList(BaseFunctionalities.GetEnumList<EnumBudgetAndEstimatingApprovalStatus>(), "Value", "Text"); } }
        public List<object> BoqDivisionsSelectList { get; set; } = new List<object>();
        public List<object> BoqItemSelectList { get; set; } = new List<object>();
    }
}