﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using KGERP.Data.Models;
using KGERP.Service.ServiceModel.RealState;
using KGERP.Utility;
using Newtonsoft.Json;

namespace KGERP.Service.Implementation.Configuration
{
    public abstract class BaseVM
    {

        public int ID { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public ActionEnum ActionEum { get { return (ActionEnum)this.ActionId; } }
        public int ActionId { get; set; } = 1;
        public JournalEnum JournalEnum { get { return (JournalEnum)this.JournalType; } }
        public int JournalType { get; set; } = (int)JournalEnum.JournalVoucher;
        public bool IsActive { get; set; } = true;
        public int? CompanyFK { get; set; }
        public string Remarks { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        //public string Message { get; set; }
        //public bool HasError { get; set; }

    }
    public class VMCompany : BaseVM
    {
        public int CompanyId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int OrderNo { get; set; }
        public int? CompanyType { get; set; }
        public string MushokNo { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int? LayerNo { get; set; }
        public string CompanyLogo { get; set; }
        public bool IsCompany { get; set; }
        public IEnumerable<VMCompany> DataList { get; set; }
        public HttpPostedFileBase CompanyLogoUpload { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }
        public string Param { get; set; }

    }

    public class VMUserMenuAssignment : BaseVM
    {
        //public bool IsAllowed { get; set; }
        public string MenuName { get; set; }
        public int MenuID { get; set; }
        public string SubmenuName { get; set; }
        public int SubMenuID { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string Method { get; set; }
        public string CompanyName { get; set; }
        public long CompanyUserMenusId { get; set; }
        public SelectList CompanyList { get; set; } = new SelectList(new List<object>());
        public IEnumerable<VMUserMenuAssignment> DataList { get; set; }
        public int MenuPriority { get; set; }
    }

    public class VMUserMenu : BaseVM
    {
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int? LayerNo { get; set; }
        public string ShortName { get; set; }
        public int accounting_CostCenterTypeId { get; set; }
        public string CostCenterTypeName { get; set; }
        public string ProjectLocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public IEnumerable<VMUserMenu> DataList { get; set; }
        public List<Accounting_CostCenterType> accounting_CostCenterTypes { get; set; }
        public SelectList CompanyList { get; set; } = new SelectList(new List<object>());
    }

    public class VMUserSubMenu : BaseVM
    {
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string UserMenuName { get; set; }
        public int User_MenuFk { get; set; }
        public int Priority { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int? LayerNo { get; set; }
        public string ShortName { get; set; }
        public string Param { get; set; }
        public SelectList UserMenuList { get; set; } = new SelectList(new List<object>());
        public SelectList CompanyList { get; set; } = new SelectList(new List<object>());

        public string DisplayMessage { get; set; }

        public IEnumerable<VMUserSubMenu> DataList { get; set; }

    }

    public class VMPOTremsAndConditions : BaseVM
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public IEnumerable<VMPOTremsAndConditions> DataList { get; set; }
    }

    public class VMCommonZone : BaseVM
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> Rate { get; set; }

        public string ZoneIncharge { get; set; }
        public string SalesOfficerName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string MobileOffice { get; set; }
        public string MobilePersonal { get; set; }
        public int HeadGLId { get; set; }
        public Nullable<long> EmployeeId { get; set; }

        public IEnumerable<VMCommonZone> DataList { get; set; }
        public List<SelectModel> EmployeeList { get; set; } = new List<SelectModel>();
    }
    public class VMCommonArea : BaseVM
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string SalesOfficerName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string MobileOffice { get; set; }
        public string MobilePersonal { get; set; }

        public SelectList ZoneList { get; set; } = new SelectList(new List<object>());
        public List<SelectModel> EmployeeList { get; set; } = new List<SelectModel>();
        public List<SelectModel> RegionList { get; set; } = new List<SelectModel>();

        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<int> RegionId { get; set; }
        public string RegionName { get; set; }

        public IEnumerable<VMCommonArea> DataList { get; set; }


    }
    public class VMCommonSubZone : BaseVM
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string SalesOfficerName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string MobileOffice { get; set; }
        public string MobilePersonal { get; set; }

        public SelectList ZoneList { get; set; } = new SelectList(new List<object>());
        public List<SelectModel> EmployeeList { get; set; } = new List<SelectModel>();
        public List<SelectModel> RegionList { get; set; } = new List<SelectModel>();
        public List<SelectModel> AreaList { get; set; } = new List<SelectModel>();

        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<int> RegionId { get; set; }
        public string RegionName { get; set; }
        public Nullable<int> AreaId { get; set; }
        public string AreaName { get; set; }

        public IEnumerable<VMCommonSubZone> DataList { get; set; }


    }

    public class VMCommonRegion : BaseVM
    {

        public string Name { get; set; }
        public string Code { get; set; }
        public string RegionIncharge { get; set; }
        public string SalesOfficerName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string MobileOffice { get; set; }
        public string MobilePersonal { get; set; }
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<int> RegionId { get; set; }
        public string RegionName { get; set; }

        public SelectList ZoneList { get; set; } = new SelectList(new List<object>());
        public List<SelectModel> EmployeeList { get; set; } = new List<SelectModel>();

        public IEnumerable<VMCommonRegion> DataList { get; set; }


    }

    public class VMCommonSize : BaseVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLock { get; set; }
        public IEnumerable<VMCommonSize> DataList { get; set; }


    }

    public class VMCommonUnit : BaseVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLock { get; set; }
        public bool IsBoQUnit { get; set; } = false;

        public IEnumerable<VMCommonUnit> DataList { get; set; }
    }

    public class VMCommonSupplier : BaseVM
    {


        public string Name { get; set; }
        public int VendorReferenceId { get; set; }
        public int ZoneId { get; set; }
        public int SubZoneId { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public int? RegionId { get; set; }
        public HttpPostedFileBase file { get; set; }
        public string ImageFileUrl { get; set; }
        public long ImageDocId { get; set; }
        public int Common_UpazilasFk { get; set; }
        public int Common_DistrictsFk { get; set; }
        public int Common_DivisionFk { get; set; }
        public int Common_CountriesFk { get; set; }

        public string Upazila { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Country { get; set; }
        public string Fax { get; set; }
        public int CustomerTypeFk { get; set; }
        public int SupplierTypeFk { get; set; }
        public int VendorTypeId { get; set; }
        public string VendorTypeName { get; set; }
        public bool BiltoBilCreditPeriod { get; set; }
        public int CreditPeriod { get; set; }
        public decimal SecurityAmount { get; set; }
        public int? CustomerStatus { get; set; } = 1;
        public string Propietor { get; set; }
        public string PaymentType { get; set; }
        public string NomineeName { get; set; }
        public string NomineePhone { get; set; }
        public string BusinessAddress { get; set; }
        public string NomineeNID { get; set; }
        public string NomineeRelation { get; set; }


        public SelectList UpazilasList { get; set; } = new SelectList(new List<object>());
        public SelectList ZoneList { get; set; } = new SelectList(new List<object>());
        public SelectList RegionList { get; set; } = new SelectList(new List<object>());
        public SelectList CountryList { get; set; } = new SelectList(new List<object>());
        public SelectList DistrictList { get; set; } = new SelectList(new List<object>());
        public SelectList DivisionList { get; set; } = new SelectList(new List<object>());
        public SelectList TerritoryList { get; set; } = new SelectList(new List<object>());
        public SelectList AreaList { get; set; } = new SelectList(new List<object>());
        public SelectList PaymentTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList NomineeRelationList { get; set; } = new SelectList(new List<object>());


        public CustomerTypeEnum CustomerType { get { return (CustomerTypeEnum)this.CustomerTypeFk; } }// = SupplierPaymentMethodEnum.Cash;
        public SupplierTypeEnum SupplierType { get { return (SupplierTypeEnum)this.SupplierTypeFk; } }// = SupplierPaymentMethodEnum.Cash;
        public string CustomerTypeName { get { return BaseFunctionalities.GetEnumDescription(CustomerType); } }
        public SelectList CustomerTypeList { get { return new SelectList(BaseFunctionalities.GetEnumList<CustomerTypeEnum>(), "Value", "Text"); } }
        //public SelectList SupplierTypeList { get { return new SelectList(BaseFunctionalities.GetEnumList<SupplierTypeEnum>(), "Value", "Text"); } }


        public CustomerStatusEnum CustomerStatusEnum { get { return (CustomerStatusEnum)this.CustomerStatus; } }
        public string CustomerStatusName { get { return BaseFunctionalities.GetEnumDescription(CustomerStatusEnum); } }
        public SelectList CustomerStatusEnumList { get { return new SelectList(BaseFunctionalities.GetEnumList<CustomerStatusEnum>(), "Value", "Text"); } }
        public string ContactPerson { get; set; }

        [Required]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Phone Number Must be 11 digit")]
        public string Phone { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsForeign { get; set; }
        public string ZoneName { get; set; }
        public string ZoneIncharge { get; set; }
        public string NID { get; set; }
        public decimal? CreditLimit { get; set; }

        public string ACName { get; set; }
        public string ACNo { get; set; }
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public IEnumerable<VMCommonSupplier> DataList { get; set; }
        public List<SelectModel> CList { get; set; }
        public List<object> HeadList { get; set; }
        public long? HeadGLId { get; set; }
        public int HId { get; set; }
        public long CGId { get; set; }
        public string FileNo { get; set; }
        public string BookingNo { get; set; }
        public string ProductName { get; set; }
        public string RegionName { get; set; }
        public string SubZoneName { get; set; }
        public string TradeLicenseNumber { get; set; }
        public string BankRoutingNumber { get; set; }
        public string BIN { get; set; }
        public string TIN { get; set; }
        public int SupplierTypeId { get; set; }
        public string SupplierTypeName { get; set; }
        public SelectList SupplierTypeList { get; set; } = new SelectList(new List<object>());
        public SelectList BankList { get; set; } = new SelectList(new List<object>());
        public SelectList BankBranchList { get; set; } = new SelectList(new List<object>());
    }

    public class VMCommonSupplierProduct : VMCommonProduct
    {
        public string SupplierName { get; set; }
        public int Common_SupplierFk { get; set; }

        public string Thana { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Country { get; set; }
        public string Fax { get; set; }
        public bool BiltoBilCreditPeriod { get; set; }
        public int CreditPeriod { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // public List<string> Common_ProductListFK { get; set; }

        // public MultiSelectList MultiProductList { get; set; } = new MultiSelectList(new List<object>());
        public IEnumerable<VMCommonSupplierProduct> DataListSupplierProduct { get; set; }

    }

    public class VMCommonSupplierContact : VMCommonSupplier
    {
        public int Common_SupplierFk { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public string ContactName { get; set; }
        public string Designations { get; set; }

        public IEnumerable<VMCommonSupplierContact> SupplierContactDataList { get; set; }

    }

    public class VMCommonProductCategory : BaseVM
    {
        public string Name { get; set; }
        public decimal? CashCommission { get; set; }

        public string Description { get; set; }

        public string ProductType { get; set; }
        public string Code { get; set; }
        public bool IsCrm { get; set; }
        //public EnumAssetIntegration Asset { get; set; } = EnumAssetIntegration.Inventory;
        public bool IsBudget { get; set; }
        public bool Asset { get; set; }
        public bool Income { get; set; }
        public bool Equity { get; set; }
        public bool Expense { get; set; }
        public int Head2AssetId { get; set; }
        public SelectList Head2AssetList { get; set; } = new SelectList(new List<object>());
        public int Head2EquityId { get; set; }
        public SelectList Head2EquityList { get; set; } = new SelectList(new List<object>());
        public int Head2IncomeId { get; set; }
        public SelectList Head2IncomeList { get; set; } = new SelectList(new List<object>());
        public int Head2ExpenseId { get; set; }
        public SelectList Head2ExpenseList { get; set; } = new SelectList(new List<object>());

        public int Head3AssetId { get; set; }
        public SelectList Head3AssetList { get; set; } = new SelectList(new List<object>());
        public int Head3EquityId { get; set; }
        public SelectList Head3EquityList { get; set; } = new SelectList(new List<object>());
        public int Head3IncomeId { get; set; }
        public SelectList Head3IncomeList { get; set; } = new SelectList(new List<object>());
        public int Head3ExpenseId { get; set; }
        public SelectList Head3ExpenseList { get; set; } = new SelectList(new List<object>());

        public IEnumerable<VMCommonProductCategory> DataList { get; set; }
    }

    public class VMIncentive : BaseVM
    {

        public int IncentiveId { get; set; }
        public string IncentiveType { get; set; }
        public int CompanyId { get; set; }
        public DateTime? MDate { get; set; }

        public IEnumerable<VMIncentive> DataList { get; set; }
    }

    public class VMIncentiveDetails : VMIncentive
    {
        public int IncentiveDetailId { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? MaxQty { get; set; }
        public decimal? Rate { get; set; }

        public IEnumerable<VMIncentiveDetails> DataListDetails { get; set; }
    }

    public class VMCommonProductSubCategory : BaseVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Common_ProductCategoryFk { get; set; }
        public string CategoryName { get; set; }
        public string ProductType { get; set; }
        public bool IsLock { get; set; }
        public decimal? BaseCommissionRate { get; set; }
        public List<SelectModelType> ProductCategoryList { get; set; }
        public SelectList ProductCategoryLists { get; set; } = new SelectList(new List<object>());
        public IEnumerable<VMCommonProductSubCategory> DataList { get; set; }


    }

    public partial class VMFinishProductBOM : VMCommonProduct
    {
        public int FProductFK { get; set; }
        public string ORDStyle { get; set; }

        public int CompanyId { get; set; }
        public int? StatusIs { get; set; }
        public int RProductFK { get; set; }
        public decimal RequiredQuantity { get; set; }
        public double Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public double FinishUnitPrice { get; set; }
        public double FinishTotalPrice { get; set; }


        public decimal RProcessLoss { get; set; }
        public string RawProductName { get; set; }
        public string FinishProductName { get; set; }
        public string SupplierName { get; set; }
        public long OrderDetailId { get; set; }


        public IEnumerable<VMFinishProductBOM> DataListProductBOM { get; set; }
        public SelectList RawProductNameList { get; set; } = new SelectList(new List<object>());
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string ContactPerson { get; set; }
        public long OrderMasterId { get; set; }
        public string OrderNo { get; set; }
        public int Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerPaymentMethodEnumFK { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string CommonCustomerName { get; set; }
        public string CustomerTypeFk { get; set; }
        public string CustomerId { get; set; }
        public string CourierCharge { get; set; }
        public string FinalDestination { get; set; }
        public string CourierNo { get; set; }
        public int SupplierId { get; set; }

    }

    public class VMRealStateProduct : VMCommonProduct
    {
        public int? FacingId { get; set; }
        public FlatProperties FlatProp { get; set; } = new FlatProperties();
        public PlotProperties PlotProp { get; set; }
        public List<SelectModelType> FacingDropDown { get; set; }
    }

    public class VMrealStateProductsForList
    {
        public int CompanyId { get; set; }
        public string ProductType { get; set; }
        public string CompanyName { get; set; }
        public int ID { get; set; }
        public ActionEnum ActionEum { get { return (ActionEnum)this.ActionId; } }
        public int ActionId { get; set; } = 1;
        public List<realStateProducts> DataList { get; set; } = new List<realStateProducts>();
        public realStateProducts realStateProducts { get; set; } = new realStateProducts();
        public List<SelectModelType> GetList { get; set; }
    }

    public class realStateProducts
    {
        public int ID { get; set; }
        public string ProductType { get; set; }
        public string Code { get; set; }

        public int? FacingId { get; set; }
        public string FacingTitle { get; set; }
        public string LandFacingTitle { get; set; }
        public FlatProperties FlatProp
        {
            get
            {
                var d = JsonConvert.DeserializeObject<FlatProperties>(this.JsonData);
                if (d == null)
                {
                    return new FlatProperties();
                }
                else
                {
                    return d;
                }
            }
        }
        public PlotProperties PlotProp
        {
            get
            {
                var d = JsonConvert.DeserializeObject<PlotProperties>(this.JsonData);
                if (d == null)
                {
                    return new PlotProperties();
                }
                else
                {
                    return d;
                }
            }
        }
        public int Common_ProductInBinSlaveFk { get; set; }
        public string Name { get; set; }
        public double? PackSize { get; set; }
        public string ShortName { get; set; }
        public string Remarks { get; set; }
        public int? Common_ProductCategoryFk { get; set; }
        public int? Common_ProductSubCategoryFk { get; set; }
        public int? Common_ProductFk { get; set; }
        public int WareHouse_POReceivingFk { get; set; }
        public int WareHouse_POReceivingSlaveFk { get; set; }
        public int? Common_UnitFk { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DiscountExpiryDate { get; set; }
        public decimal ProcessLoss { get; set; }
        public decimal CostingPrice { get; set; }
        public string UnitName { get; set; }
        public decimal MRPPrice { get; set; }
        public decimal TPPrice { get; set; }
        [Required]
        [Range(1, 99999999999999, ErrorMessage = "Unit Price is Required!! not allow zero or negative .")]
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PreviousStock { get; set; }
        public decimal CurrentStock { get; set; }
        public string Image { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }
        public int Status { get; set; }
        public bool IsLock { get; set; }
        public string JsonData { get; set; }
        public List<SelectModelType> GetList { get; set; }
    }

    public class VMCommonProduct : BaseVM
    {
        [Display(Name = "Code")]
        public string ProductCode { get; set; }
        public int Common_ProductInBinSlaveFk { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? Common_ProductCategoryFk { get; set; }
        public int? Common_ProductSubCategoryFk { get; set; }
        public int? Common_ProductFk { get; set; }

        public int WareHouse_POReceivingFk { get; set; }
        public int WareHouse_POReceivingSlaveFk { get; set; }
        public int? Common_UnitFk { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DiscountExpiryDate { get; set; }
        public decimal ProcessLoss { get; set; }
        public decimal CostingPrice { get; set; }
        public Nullable<int> PackId { get; set; }
        public Nullable<decimal> DieSize { get; set; }
        public Nullable<double> PackSize { get; set; }
        public string PackName { get; set; }
        public string UnitName { get; set; }
        public decimal MRPPrice { get; set; }
        public decimal TPPrice { get; set; }

        [Required]
        [Range(1, 99999999999999, ErrorMessage = "Unit Price is Required!! not allow zero or nagetive .")]
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PreviousStock { get; set; }
        public decimal CurrentStock { get; set; }

        public string Image { get; set; }

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }

        //public IFormFile ImageFile { get; set; }

        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }

        public bool IsLock { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal? CreditSalePrice { get; set; }
        public decimal DamageQuantity { get; set; }
        public double DeliveredQty { get; set; }
        public decimal RawConsumeQuantity { get; set; }
        public string Description { get; set; }


        public decimal LaborCost { get; set; }
        public decimal ManufacturingOverhead { get; set; }
        public decimal TransportationOverhead { get; set; }
        public decimal OthersCost { get; set; }
        public string OthersCostNote { get; set; }

        public int ProductCategoryId { get; set; }
        public string ProductSubCategoryId { get; set; }

        public IEnumerable<VMCommonProduct> DataList { get; set; }
        public List<SelectModelType> GetProductCategoryList { get; set; }
        public SelectList ProductCategoryList { get; set; } = new SelectList(new List<object>());
        public SelectList ProductList { get; set; } = new SelectList(new List<object>());
        public SelectList WareHousePOReceivingList { get; set; } = new SelectList(new List<object>());


        public SelectList ProductSubCategoryList { get; set; } = new SelectList(new List<object>());
        public SelectList UnitList { get; set; } = new SelectList(new List<object>());
        public SelectList StatusList { get; set; } = new SelectList(new List<object>());

        public string ProductType { get; set; }
        public int Status { get; set; }
        public decimal? ReturnQuntity { get; set; }

        public decimal? FormulaQty { get; set; }
    }


    public class VMCommonCustomer : BaseVM
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string MemberShipNo { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public string Sex { get; set; }

        public string CustomerLoyalityPoint { get; set; }
        public string CustomerTypeName { get; set; }
        public int CustomerTypeEnumFk { get; set; }

        //public int Common_ThanaFk { get; set; }
        //public int Common_CountriesFk { get; set; }
        //public int Common_DistrictsFk { get; set; }
        //public int Common_DivisionFk { get; set; }

        //public string Division { get; set; }
        //public string Thana { get; set; }
        //public string District { get; set; }
        //public string Country { get; set; }

        //public SelectList CountryList { get; set; } = new SelectList(new List<object>());
        //public SelectList DivisionList { get; set; } = new SelectList(new List<object>());
        //public SelectList DistrictList { get; set; } = new SelectList(new List<object>());
        //public SelectList ThanaList { get; set; } = new SelectList(new List<object>());

        public IEnumerable<VMCommonCustomer> DataList { get; set; }


    }

    public class VMCommonCustomerContact : VMCommonCustomer
    {
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public string ContactName { get; set; }
        public int Common_CustomerFk { get; set; }
        public IEnumerable<VMCommonCustomerContact> CustomerContactDataList { get; set; }

    }

    public class VMCommonShop : BaseVM
    {
        public string Name { get; set; }



        public int Common_ThanaFk { get; set; }

        public int Common_CountriesFk { get; set; }
        public int Common_DistrictsFk { get; set; }
        public int Common_DivisionFk { get; set; }
        public int ShopTypeEnumFk { get; set; }



        public string Thana { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string Country { get; set; }


        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public bool OwnDeliveryService { get; set; }
        public string ServiceStartTime { get; set; }
        public string ServiceEndTime { get; set; }
        public string Description { get; set; }

        public SelectList ThanaList { get; set; } = new SelectList(new List<object>());
        public SelectList DistrictList { get; set; } = new SelectList(new List<object>());
        public SelectList DivisionList { get; set; } = new SelectList(new List<object>());
        public SelectList CountryList { get; set; } = new SelectList(new List<object>());

        //public ShopTypeEnum STypeEnum { get { return (ShopTypeEnum)this.ShopTypeEnumFk; } }
        //public string ShopTypeEnumName { get { return BaseFunctionalities.GetEnumDescription(STypeEnum); } }
        //public SelectList ShopTypeList { get { return new SelectList(BaseFunctionalities.GetEnumList<ShopTypeEnum>(), "Value", "Text"); } }


        public string TradeLicenceNumber { get; set; }
        public DateTime TradeLicenceExpireDate { get; set; }
        public string TradeLicenceUrl { get; set; }

        public IEnumerable<VMCommonShop> DataList { get; set; }
    }

    public class VMCommonShopOwner : VMCommonShop
    {
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public string ContactName { get; set; }
        public IEnumerable<VMCommonShopOwner> CommonShopOwnerDataList { get; set; }
    }

    public class VMCommonShopDeliveryService : VMCommonShop
    {
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public string ContactName { get; set; }
        public IEnumerable<VMCommonShopDeliveryService> ShopDeliveryServiceDataList { get; set; }
    }

    public class VMCommonShopCounter : BaseVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SelectList ShopList { get; set; } = new SelectList(new List<object>());
        public string ShopName { get; set; }
        public IEnumerable<VMCommonShopCounter> DataList { get; set; }
        public VMCommonShop VMCommonShop { get; set; }

    }

    public class VMCommonBin : BaseVM
    {
        public string UnitName { get; set; }
        public string ShopName { get; set; }
        public int RackNo { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public IEnumerable<VMCommonBin> DataList { get; set; }
        public SelectList ShopList { get; set; } = new SelectList(new List<object>());

    }

    public class VMCommonBinSlave : VMCommonBin
    {

        public int Common_BinFk { get; set; }
        public string CID { get; set; }
        public string Dimension { get; set; }
        public List<string> ProductNameList { get; set; }
        public string ProductName { get; set; }
        public VMCommonProduct VMCommonProduct { get; set; }
        public int Common_ProductInBinSlaveFk { get; set; }
        public int Common_ProductFK { get; set; }
        public IEnumerable<VMCommonBinSlave> DataListSlave { get; set; }
        public List<VMCommonBinSlave> DataSlaveToList { get; set; }
        public MultiSelectList ProductList { get; set; } = new MultiSelectList(new List<object>());

    }

    public class VMCommonCountries : BaseVM
    {
        public string Name { get; set; }
        public string BnName { get; set; }
        public string Url { get; set; }

    }

    public class VMCommonDivisions : BaseVM
    {
        public string Name { get; set; }
        public string BnName { get; set; }
        public string Url { get; set; }

        public int Common_CountriesFk { get; set; }
        public IEnumerable<VMCommonDivisions> DataList { get; set; }


    }

    public class VMCommonDistricts : BaseVM
    {
        public string Name { get; set; }
        public string BnName { get; set; }
        public string Url { get; set; }
        public int Common_DivisionsFk { get; set; }
        public string DivisionsName { get; set; }

        public string ShorName { get; set; }
        public IEnumerable<VMCommonDistricts> DataList { get; set; }
        public SelectList DivisionList { get; set; } = new SelectList(new List<object>());


    }

    public class VMCommonThana : BaseVM
    {
        public string Name { get; set; }
        public string BnName { get; set; }
        public string Url { get; set; }
        public int Common_DistrictsFk { get; set; }
        public string DistictName { get; set; }
        public int Common_DivisionsFk { get; set; }
        public string DivisionsName { get; set; }
        public string ShorName { get; set; }
        public IEnumerable<VMCommonThana> DataList { get; set; }

        public List<District> DistrictList { get; set; }

    }

    public partial class VMAccountingSignatory : BaseVM
    {

        public int SignatoryId { get; set; }
        public string SignatoryName { get; set; }
        public string SignatoryType { get; set; }
        public int OrderBy { get; set; }
        public int Priority { get; set; }
        public IEnumerable<VMAccountingSignatory> DataList { get; set; }
        public SelectList CompanyList { get; set; } = new SelectList(new List<object>());
    }

    public partial class VMPackagingPurchaseRequisition : BaseVM
    {
        public decimal TotalQty { get; set; }
        public long IssueMasterId { get; set; }
        public decimal? PriviousIssueQty { get; set; }
        public decimal? RemainingQuantity { get; set; }
        public long IssueDetailsId { get; set; }
        public decimal IssueQty { get; set; }
        public string OrderNo { get; set; }
        public string IssueNo { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public long OrderMasterId { get; set; }
        public int OrderDetailsId { get; set; }
        public int RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string Description { get; set; }
        public string RequisitionStatus { get; set; }
        public int CompanyId { get; set; }
        public int RequisitionType { get; set; }
        public int FromRequisitionId { get; set; }
        public int ToRequisitionId { get; set; }
        //DetailsItem
        public int RequisitionItemId { get; set; }
        public decimal? Qty { get; set; }
        public string RequisitionItemStatus { get; set; }
        public string IssueBy { get; set; }
        public DateTime IssueDate { get; set; }
        public string RawProductName { get; set; }
        public IEnumerable<VMPackagingPurchaseRequisition> DataList { get; set; }
        public List<VMPackagingPurchaseRequisition> DataListPro { get; set; }
    }

    #region Bank
    public class VMCommonBank : BaseVM
    {

        public string Name { get; set; }
        public IEnumerable<VMCommonBank> DataList { get; set; }
    }

    public class VMCommonBankBranch : BaseVM
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public SelectList BankList { get; set; } = new SelectList(new List<object>());
        public int BankId { get; set; }
        public string BankName { get; set; }
        public IEnumerable<VMCommonBankBranch> DataList { get; set; }

    }

    public class VMCommonActChequeInfo : BaseVM
    {
        public string AccountNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/YYYY}")]
        public DateTime ChequeDate { get; set; }
        public string PayTo { get; set; }
        public decimal Amount { get; set; }
        public SelectList BankList { get; set; } = new SelectList(new List<object>());
        public SelectList BankBranchList { get; set; } = new SelectList(new List<object>());

        public SelectList SignatoryList { get; set; } = new SelectList(new List<object>());

        public int BankId { get; set; }
        public string BankName { get; set; }
        public int BankBranchId { get; set; }

        public string BankBranchName { get; set; }
        public int SignatoryId { get; set; }

        public string SignatoryName { get; set; }

        public IEnumerable<VMCommonActChequeInfo> DataList { get; set; }

    }
    #endregion
}