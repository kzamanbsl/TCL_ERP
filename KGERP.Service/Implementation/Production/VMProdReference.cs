﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KGERP.Service.Implementation.Configuration;

namespace KGERP.Service.Implementation.Production
{

    public partial class VMProdReference : BaseVM
    {
        public int ProdReferenceId { get; set; }
        public string ManagerReferenceName { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime ReferenceDate { get; set; }
        public bool IsSubmitted { get; set; }

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public decimal TotalRawConsumedAmount { get; set; }
        public decimal TotalFactoryExpensessAmount { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string StrFromDate { get; set; }
        public string StrToDate { get; set; }

        public IEnumerable<VMProdReference> DataList { get; set; }

    }
    public partial class VMProdReferenceSlave : VMProdReference
    {
        public int ProdReferenceSlaveID { get; set; }
        public int FProductId { get; set; }
        public int RProductId { get; set; }
        public decimal RawConsumeQuantity { get; set; }
        public int? FactoryExpensesHeadGLId { get; set; }
        public int? AdvanceHeadGLId { get; set; }
        public string AdvanceHeadGLName { get; set; }

        public decimal Quantity { get; set; }
        public decimal QuantityOver { get; set; }
        public decimal QuantityLess { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal PriviousProcessQuantity { get; set; }
        public decimal ExcessQuantity { get; set; }
        public string ChallanNo { get; set; }

        public int MaterialReceiveFk { get; set; }
        public IEnumerable<VMProdReferenceSlave> DataListSlave { get; set; }

        public IEnumerable<VMProdReferenceSlave> RawDataListSlave { get; set; }
        public IEnumerable<VMProdReferenceSlave> FinishDataListSlave { get; set; }
        public int? AccountingHeadId { get; set; }
        public string ProductName { get; set; }
        public string FactoryExpecsesHeadName { get; set; }

        public string RawProductName { get; set; }

        public string UnitName { get; set; }
        public bool MakeFinishInventory { get; set; }
        public decimal CostingPrice { get; set; }
        public decimal FectoryExpensesAmount { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal PurchasePrice { get; set; }
        public SelectList FactoryExpensesList { get; set; } = new SelectList(new List<object>());
        public SelectList AdvanceHeadList { get; set; } = new SelectList(new List<object>());

        public List<VMProdReferenceSlaveConsumption> RowProductConsumeList { get; set; }
        public List<VMProdReferenceSlave> DataToList { get; set; }

    }
    public partial class VMProdReferenceSlaveConsumption : BaseVM
    {
        public int ProdReferenceSlaveConsumptionID { get; set; }
        public int ProdReferenceSlaveID { get; set; }
        public int RProductId { get; set; }
        public string RProductName { get; set; }
        public string RSubCategoryName { get; set; }
        public string RCategoryName { get; set; }

        public string UnitName { get; set; }

        public decimal TotalConsumeQuantity { get; set; }
        public decimal UnitPrice { get; set; }


    }


    public partial class AccountList
    {
        public int? AccountingHeadId { get; set; }     

        public decimal Value { get; set; }

      

    }
}
