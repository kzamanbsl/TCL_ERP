﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KGERP.Data.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ERPEntities : DbContext
    {
        public ERPEntities()
            : base("name=ERPEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminPolicy> AdminPolicies { get; set; }
        public virtual DbSet<AdminSetUp> AdminSetUps { get; set; }
        public virtual DbSet<AdvanceSalary> AdvanceSalaries { get; set; }
        public virtual DbSet<AllowancePolicy> AllowancePolicies { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<AssetAssign> AssetAssigns { get; set; }
        public virtual DbSet<AssetCategory> AssetCategories { get; set; }
        public virtual DbSet<AssetFileAttach> AssetFileAttaches { get; set; }
        public virtual DbSet<AssetLocation> AssetLocations { get; set; }
        public virtual DbSet<AssetStatu> AssetStatus { get; set; }
        public virtual DbSet<AssetSubLocation> AssetSubLocations { get; set; }
        public virtual DbSet<AssetType> AssetTypes { get; set; }
        public virtual DbSet<AttendancePolicy> AttendancePolicies { get; set; }
        public virtual DbSet<Attendence> Attendences { get; set; }
        public virtual DbSet<AttendenceApproveApplication> AttendenceApproveApplications { get; set; }
        public virtual DbSet<AttendenceHistory> AttendenceHistories { get; set; }
        public virtual DbSet<Bag> Bags { get; set; }
        public virtual DbSet<BoardOfDirector> BoardOfDirectors { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<CaseComment> CaseComments { get; set; }
        public virtual DbSet<CaseHistory> CaseHistories { get; set; }
        public virtual DbSet<clients_BasicInfo> clients_BasicInfo { get; set; }
        public virtual DbSet<ClientsInfo> ClientsInfoes { get; set; }
        public virtual DbSet<ClientsInfo_Del> ClientsInfo_Del { get; set; }
        public virtual DbSet<Colour> Colours { get; set; }
        public virtual DbSet<CompanyVoucher> CompanyVouchers { get; set; }
        public virtual DbSet<ComplainManagement> ComplainManagements { get; set; }
        public virtual DbSet<ComplainType> ComplainTypes { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CreditRecover> CreditRecovers { get; set; }
        public virtual DbSet<CreditRecoverDetail> CreditRecoverDetails { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DisputedList> DisputedLists { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DropDownItem> DropDownItems { get; set; }
        public virtual DbSet<DropDownType> DropDownTypes { get; set; }
        public virtual DbSet<ECMember> ECMembers { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public virtual DbSet<EmployeeHierarkey> EmployeeHierarkeys { get; set; }
        public virtual DbSet<EmployeeLoan> EmployeeLoans { get; set; }
        public virtual DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<GradeDetail> GradeDetails { get; set; }
        public virtual DbSet<HolidayCategory> HolidayCategories { get; set; }
        public virtual DbSet<KgAsset> KgAssets { get; set; }
        public virtual DbSet<KGPFData> KGPFDatas { get; set; }
        public virtual DbSet<KGREComment> KGREComments { get; set; }
        public virtual DbSet<KGRECostSetup> KGRECostSetups { get; set; }
        public virtual DbSet<KGREInstallment> KGREInstallments { get; set; }
        public virtual DbSet<KGREPlot> KGREPlots { get; set; }
        public virtual DbSet<KGREPlotBooking> KGREPlotBookings { get; set; }
        public virtual DbSet<KGREProject> KGREProjects { get; set; }
        public virtual DbSet<KGREProjectDetail> KGREProjectDetails { get; set; }
        public virtual DbSet<KTTLComment> KTTLComments { get; set; }
        public virtual DbSet<KttlCustomer> KttlCustomers { get; set; }
        public virtual DbSet<KTTLHistory> KTTLHistories { get; set; }
        public virtual DbSet<KttlService> KttlServices { get; set; }
        public virtual DbSet<LandNLegal> LandNLegals { get; set; }
        public virtual DbSet<LandOwner> LandOwners { get; set; }
        public virtual DbSet<LandReceiver> LandReceivers { get; set; }
        public virtual DbSet<LandUser> LandUsers { get; set; }
        public virtual DbSet<Leave> Leaves { get; set; }
        public virtual DbSet<LeaveApplication> LeaveApplications { get; set; }
        public virtual DbSet<LeaveApplicationDetail> LeaveApplicationDetails { get; set; }
        public virtual DbSet<LeaveCategory> LeaveCategories { get; set; }
        public virtual DbSet<LoanCollection> LoanCollections { get; set; }
        public virtual DbSet<LoanType> LoanTypes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MonthlyTarget> MonthlyTargets { get; set; }
        public virtual DbSet<PaymentMode> PaymentModes { get; set; }
        public virtual DbSet<PfData> PfDatas { get; set; }
        public virtual DbSet<PloatInfoSetup> PloatInfoSetups { get; set; }
        public virtual DbSet<PlotBooking> PlotBookings { get; set; }
        public virtual DbSet<POTremsAndCondition> POTremsAndConditions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ReportingPerson> ReportingPersons { get; set; }
        public virtual DbSet<ShareHolder> ShareHolders { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Upazila> Upazilas { get; set; }
        public virtual DbSet<UrlInfo> UrlInfoes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Work> Works { get; set; }
        public virtual DbSet<WorkAssign> WorkAssigns { get; set; }
        public virtual DbSet<WorkAssignFile> WorkAssignFiles { get; set; }
        public virtual DbSet<WorkMember> WorkMembers { get; set; }
        public virtual DbSet<WorkQA> WorkQAs { get; set; }
        public virtual DbSet<WorkQAFile> WorkQAFiles { get; set; }
        public virtual DbSet<WorkState> WorkStates { get; set; }
        public virtual DbSet<YearlyHoliday> YearlyHolidays { get; set; }
        public virtual DbSet<DemandItemDetail> DemandItemDetails { get; set; }
        public virtual DbSet<EmiDetail> EmiDetails { get; set; }
        public virtual DbSet<Farmer> Farmers { get; set; }
        public virtual DbSet<Incentive> Incentives { get; set; }
        public virtual DbSet<IncentiveDetail> IncentiveDetails { get; set; }
        public virtual DbSet<IngredientStandard> IngredientStandards { get; set; }
        public virtual DbSet<IngredientStandardDetail> IngredientStandardDetails { get; set; }
        public virtual DbSet<IssueDetailInfo> IssueDetailInfoes { get; set; }
        public virtual DbSet<IssueMasterInfo> IssueMasterInfoes { get; set; }
        public virtual DbSet<PFormulaDetail> PFormulaDetails { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<ProductFormula> ProductFormulas { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }
        public virtual DbSet<ProductStore> ProductStores { get; set; }
        public virtual DbSet<PurchaseOrderTemplate> PurchaseOrderTemplates { get; set; }
        public virtual DbSet<UpazilaAssign> UpazilaAssigns { get; set; }
        public virtual DbSet<VendorOffer> VendorOffers { get; set; }
        public virtual DbSet<AttendanceType> AttendanceTypes { get; set; }
        public virtual DbSet<EmployeeLeaveDetail> EmployeeLeaveDetails { get; set; }
        public virtual DbSet<ProcessAttenendance> ProcessAttenendances { get; set; }
        public virtual DbSet<ProcessLeave> ProcessLeaves { get; set; }
        public virtual DbSet<TempFeedVoucherDetail> TempFeedVoucherDetails { get; set; }
        public virtual DbSet<ConvertedProductDetail> ConvertedProductDetails { get; set; }
        public virtual DbSet<FormulaHistory> FormulaHistories { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<Prod_ReferenceSlave> Prod_ReferenceSlave { get; set; }
        public virtual DbSet<FinishProductBOM> FinishProductBOMs { get; set; }
        public virtual DbSet<KGREPayment> KGREPayments { get; set; }
        public virtual DbSet<PromotionType> PromotionTypes { get; set; }
        public virtual DbSet<PromtionalOffer> PromtionalOffers { get; set; }
        public virtual DbSet<PromtionalOfferDetail> PromtionalOfferDetails { get; set; }
        public virtual DbSet<Prod_ReferenceSlaveConsumption> Prod_ReferenceSlaveConsumption { get; set; }
        public virtual DbSet<Prod_Reference> Prod_Reference { get; set; }
        public virtual DbSet<VoucherType> VoucherTypes { get; set; }
        public virtual DbSet<BankBranch> BankBranches { get; set; }
        public virtual DbSet<Accounting_ChequeInfo> Accounting_ChequeInfo { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<RentProduction> RentProductions { get; set; }
        public virtual DbSet<RentProductionDetail> RentProductionDetails { get; set; }
        public virtual DbSet<StockTransferDetail> StockTransferDetails { get; set; }
        public virtual DbSet<DemandItem> DemandItems { get; set; }
        public virtual DbSet<Ticketing> Ticketings { get; set; }
        public virtual DbSet<vwDemandForSaleInvoice> vwDemandForSaleInvoices { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<vw_Attendence> vw_Attendence { get; set; }
        public virtual DbSet<vwAllHead> vwAllHeads { get; set; }
        public virtual DbSet<vwProductPrice> vwProductPrices { get; set; }
        public virtual DbSet<vwKGRECustomer> vwKGRECustomers { get; set; }
        public virtual DbSet<VoucherMap> VoucherMaps { get; set; }
        public virtual DbSet<SmsType> SmsTypes { get; set; }
        public virtual DbSet<vwSMSList> vwSMSLists { get; set; }
        public virtual DbSet<CompanyUserMenu> CompanyUserMenus { get; set; }
        public virtual DbSet<SMSScheduleLog> SMSScheduleLogs { get; set; }
        public virtual DbSet<Requisition> Requisitions { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Demand> Demands { get; set; }
        public virtual DbSet<LCInfo> LCInfoes { get; set; }
        public virtual DbSet<ErpSMS> ErpSMS { get; set; }
        public virtual DbSet<SupplierProduct> SupplierProduct { get; set; }
        public virtual DbSet<vwSeedMaterialRcvList> vwSeedMaterialRcvLists { get; set; }
        public virtual DbSet<BookingApprovalHistory> BookingApprovalHistories { get; set; }
        public virtual DbSet<BookingCostHead> BookingCostHeads { get; set; }
        public virtual DbSet<FileCatagory> FileCatagories { get; set; }
        public virtual DbSet<ServerConfig> ServerConfigs { get; set; }
        public virtual DbSet<KGRECustomerInfo> KGRECustomerInfoes { get; set; }
        public virtual DbSet<FileArchive> FileArchives { get; set; }
        public virtual DbSet<vwFTPFileInfo> vwFTPFileInfoes { get; set; }
        public virtual DbSet<AssetTrackingFinal> AssetTrackingFinals { get; set; }
        public virtual DbSet<PRM_Relation> PRM_Relation { get; set; }
        public virtual DbSet<NomineePercentageMapping> NomineePercentageMappings { get; set; }
        public virtual DbSet<CustomerNomineeInfo> CustomerNomineeInfoes { get; set; }
        public virtual DbSet<StockAdjust> StockAdjusts { get; set; }
        public virtual DbSet<CustomerBookingFileMapping> CustomerBookingFileMappings { get; set; }
        public virtual DbSet<BookingInstallmentType> BookingInstallmentTypes { get; set; }
        public virtual DbSet<CustomerGroupInfo> CustomerGroupInfoes { get; set; }
        public virtual DbSet<vwTeamInfoList> vwTeamInfoLists { get; set; }
        public virtual DbSet<vwTeamLeaderList> vwTeamLeaderLists { get; set; }
        public virtual DbSet<FacingInfo> FacingInfoes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<BookingInstallmentSchedule> BookingInstallmentSchedules { get; set; }
        public virtual DbSet<MaterialReceiveDetail> MaterialReceiveDetails { get; set; }
        public virtual DbSet<MoneyReceiptType> MoneyReceiptTypes { get; set; }
        public virtual DbSet<MoneyReceiptDetail> MoneyReceiptDetails { get; set; }
        public virtual DbSet<StoreDetail> StoreDetails { get; set; }
        public virtual DbSet<CrmPromotionalOffer> CrmPromotionalOffers { get; set; }
        public virtual DbSet<CrmChoiceArea> CrmChoiceAreas { get; set; }
        public virtual DbSet<CrmSourceMedia> CrmSourceMedias { get; set; }
        public virtual DbSet<CrmServiceStatu> CrmServiceStatus { get; set; }
        public virtual DbSet<CompanyMenu> CompanyMenus { get; set; }
        public virtual DbSet<FileAttachment> FileAttachments { get; set; }
        public virtual DbSet<KGRECustomer> KGRECustomers { get; set; }
        public virtual DbSet<CustomerGroupMapping> CustomerGroupMappings { get; set; }
        public virtual DbSet<BookingCostMapping> BookingCostMappings { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<KGREHistory> KGREHistories { get; set; }
        public virtual DbSet<CrmUploadHistory> CrmUploadHistories { get; set; }
        public virtual DbSet<ProductBookingInfo> ProductBookingInfoes { get; set; }
        public virtual DbSet<Head1> Head1 { get; set; }
        public virtual DbSet<Head2> Head2 { get; set; }
        public virtual DbSet<Head3> Head3 { get; set; }
        public virtual DbSet<Head4> Head4 { get; set; }
        public virtual DbSet<Head5> Head5 { get; set; }
        public virtual DbSet<HeadGL> HeadGLs { get; set; }
        public virtual DbSet<CrmSchedule> CrmSchedules { get; set; }
        public virtual DbSet<ErpLogInfo> ErpLogInfoes { get; set; }
        public virtual DbSet<VendorDeed> VendorDeeds { get; set; }
        public virtual DbSet<ConvertedProduct> ConvertedProducts { get; set; }
        public virtual DbSet<StockAdjustDetail> StockAdjustDetails { get; set; }
        public virtual DbSet<TeamInfo> TeamInfoes { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductSubCategory> ProductSubCategories { get; set; }
        public virtual DbSet<RequisitionItemDetail> RequisitionItemDetails { get; set; }
        public virtual DbSet<RequisitionItem> RequisitionItems { get; set; }
        public virtual DbSet<CompanySubMenu> CompanySubMenus { get; set; }
        public virtual DbSet<MoneyReceipt> MoneyReceipts { get; set; }
        public virtual DbSet<Accounting_Signatory> Accounting_Signatory { get; set; }
        public virtual DbSet<ReportApproval> ReportApprovals { get; set; }
        public virtual DbSet<ReportApprovalDetail> ReportApprovalDetails { get; set; }
        public virtual DbSet<ReportCategory> ReportCategories { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<OfficerAssign> OfficerAssigns { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<FeedMay2022Opening> FeedMay2022Opening { get; set; }
        public virtual DbSet<Attendance_Shadow> Attendance_Shadow { get; set; }
        public virtual DbSet<AttendenceApproveApplication_Shadow> AttendenceApproveApplication_Shadow { get; set; }
        public virtual DbSet<Employee_Audit> Employee_Audit { get; set; }
        public virtual DbSet<Employee_Shadow> Employee_Shadow { get; set; }
        public virtual DbSet<LeaveApplication_Shadow> LeaveApplication_Shadow { get; set; }
        public virtual DbSet<P> PS { get; set; }
        public virtual DbSet<StockTransfer> StockTransfers { get; set; }
        public virtual DbSet<ManagerProductMap> ManagerProductMaps { get; set; }
        public virtual DbSet<ExpenseMaster> ExpenseMasters { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<SaleSetting> SaleSettings { get; set; }
        public virtual DbSet<StockInfo> StockInfoes { get; set; }
        public virtual DbSet<PurchaseReturn> PurchaseReturns { get; set; }
        public virtual DbSet<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
        public virtual DbSet<ProductionMaster> ProductionMasters { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ProductionStage> ProductionStages { get; set; }
        public virtual DbSet<VendorOpening> VendorOpenings { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<SubZone> SubZones { get; set; }
        public virtual DbSet<OrderDeliver> OrderDelivers { get; set; }
        public virtual DbSet<OrderDeliverDetail> OrderDeliverDetails { get; set; }
        public virtual DbSet<OrderDeliveryPreview> OrderDeliveryPreviews { get; set; }
        public virtual DbSet<EMI> EMIs { get; set; }
        public virtual DbSet<SaleReturn> SaleReturns { get; set; }
        public virtual DbSet<SaleReturnDetail> SaleReturnDetails { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderMaster> OrderMasters { get; set; }
        public virtual DbSet<ProductionDetail> ProductionDetails { get; set; }
        public virtual DbSet<MaterialReceive> MaterialReceives { get; set; }
        public virtual DbSet<SalaryInformation> SalaryInformations { get; set; }
        public virtual DbSet<VendorDeposit> VendorDeposits { get; set; }
        public virtual DbSet<VendorDepositHistory> VendorDepositHistories { get; set; }
        public virtual DbSet<BatchPaymentDetail> BatchPaymentDetails { get; set; }
        public virtual DbSet<BatchPaymentMaster> BatchPaymentMasters { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<BillRequisitionType> BillRequisitionTypes { get; set; }
        public virtual DbSet<CostCenterManagerMap> CostCenterManagerMaps { get; set; }
        public virtual DbSet<Accounting_CostCenterType> Accounting_CostCenterType { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<BillRequisitionVoucherMap> BillRequisitionVoucherMaps { get; set; }
        public virtual DbSet<BillReqApprovalHistory> BillReqApprovalHistories { get; set; }
        public virtual DbSet<BoQDivision> BoQDivisions { get; set; }
        public virtual DbSet<VoucherDetail> VoucherDetails { get; set; }
        public virtual DbSet<VoucherBRMapDetail> VoucherBRMapDetails { get; set; }
        public virtual DbSet<VoucherBRMapMaster> VoucherBRMapMasters { get; set; }
        public virtual DbSet<VoucherBRMapMasterApproval> VoucherBRMapMasterApprovals { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual DbSet<BuildingFloor> BuildingFloors { get; set; }
        public virtual DbSet<BuildingMember> BuildingMembers { get; set; }
        public virtual DbSet<ConsumptionDetail> ConsumptionDetails { get; set; }
        public virtual DbSet<ConsumptionMaster> ConsumptionMasters { get; set; }
        public virtual DbSet<BillRequisitionDetail> BillRequisitionDetails { get; set; }
        public virtual DbSet<Accounting_CostCenter> Accounting_CostCenter { get; set; }
        public virtual DbSet<ChequeBook> ChequeBooks { get; set; }
        public virtual DbSet<BillRequisitionMaster> BillRequisitionMasters { get; set; }
        public virtual DbSet<BillRequisitionApproval> BillRequisitionApprovals { get; set; }
        public virtual DbSet<BoqBNEApprovalHistroy> BoqBNEApprovalHistroys { get; set; }
        public virtual DbSet<BoQItemProductMap> BoQItemProductMaps { get; set; }
        public virtual DbSet<QuotationDetail> QuotationDetails { get; set; }
        public virtual DbSet<QuotationFor> QuotationFors { get; set; }
        public virtual DbSet<QuotationMaster> QuotationMasters { get; set; }
        public virtual DbSet<QuotationSubmitDetail> QuotationSubmitDetails { get; set; }
        public virtual DbSet<QuotationSubmitMaster> QuotationSubmitMasters { get; set; }
        public virtual DbSet<BillBoQItem> BillBoQItems { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<EmployeeOperation> EmployeeOperations { get; set; }
        public virtual DbSet<VendorType> VendorTypes { get; set; }
        public virtual DbSet<BillingGeneratedMap> BillingGeneratedMaps { get; set; }
        public virtual DbSet<BankAccountInfo> BankAccountInfoes { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<PaymentMaster> PaymentMasters { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<ChequeRegister> ChequeRegisters { get; set; }
        public virtual DbSet<VoucherPaymentChequeHistory> VoucherPaymentChequeHistories { get; set; }
    
        public virtual ObjectResult<GetEmployeeListForTeam_Result> GetEmployeeListForTeam(Nullable<int> companyId)
        {
            var companyIdParameter = companyId.HasValue ?
                new ObjectParameter("CompanyId", companyId) :
                new ObjectParameter("CompanyId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetEmployeeListForTeam_Result>("GetEmployeeListForTeam", companyIdParameter);
        }
    }
}
