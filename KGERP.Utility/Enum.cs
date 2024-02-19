using System.ComponentModel.DataAnnotations;

namespace KGERP.Utility
{
    public enum EnumWorkOrderFor
    {
        [Display(Name = "General Work Order")]
        General = 1,
        [Display(Name = "Requisition Work Order")]
        Requisition,
        [Display(Name = "Direct Purchase Order")]
        Direct_Purchase
    }

    public enum EnumMaterialQuality
    {
        A = 1,
        B,
        C
    }

    public enum EnumQuotationStatus
    {
        Draft = 1,
        Submitted,
        Accepted
    }

    public enum EnumQuotationFor
    {
        [Display(Name = "General Quotation")]
        General = 1,
        [Display(Name = "Requisition Quotation")]
        Requisition
    }

    public enum EnumVoucherFor
    {
        [Display(Name = "General Voucher")]
        General = 1,
        [Display(Name = "Requisition Voucher")]
        Requisition
    }

    public enum EnumVoucherRequisitionSignatory
    {
        Initiator = 1,
        Checker,
        Approver
    
    }
    public enum EnumBRequisitionSignatory
    {
        Initiator = 1,
        PM,
        QS,
        Director,
        PD,
        MD
    }
    public enum EnumBillRequisitionStatus
    {
        Draft = 1,
        Submitted,
        Pending,
        Approved,
        Rejected,
        Closed
    }

    public enum EnumVoucherPaymentStatus
    {
        Pending = 1,
        Partial,
        Paid
    }

    public enum EnumVoucherApprovalStatus
    {
        Draft = 1,
        Submitted,
        Pending,
        Approved,
        Rejected,
        Closed
    }
    public enum EnumBillRequisitionType
    {
        [Display(Name = "Construction Materials")]
        Materials = 6,
        [Display(Name = "P & M (Paint, Equipment, Machineries and Tools)")]
        PnM,
        [Display(Name = "Power, Water")]
        Power,
        [Display(Name = "Fuel, Oil, Lubricants")]
        Oil,
        [Display(Name = "Formwork")]
        Formwork,
        [Display(Name = "Overhead")]
        Overhead,
        [Display(Name = "Temporary Work")]
        Temporary,
        [Display(Name = "Labor")]
        Labor,
        [Display(Name = "Transportation")]
        Transportation,
        [Display(Name = "Others")]
        Others
    }

    public enum EnumBillRequisitionSubType
    {
        [Display(Name = "IT Equipment")]
        It = 1
    }

    public enum EnumBillReqProjectType
    {
        Agro = 1,
        Bridge,
        Building,
        General,
        Road,
        Runway,
    }
    public enum FLatCompletionStatusEnum
    {
        OnGoing = 1,
        FUllReady = 2
    }
    public enum ProductionResultEnum
    {
        Equal = 1,
        Gain,
        Loss
    }

    public enum IndicatorEnum
    {
        BookingMoney = 1,
        Installment,
        CostHead

    }
    public enum ProductBookingStepsEnum
    {
        Entry = 1,//Dealing officer entry
        Checking = 2,// for Head of sales checking
        Approval = 3,//for company DMD approval
        FinalApproval = 4 //for company  MD approval
    }
    public enum ProductApprovalStatusEnum
    {
        Draft = 1,
        Recheck = 2,
        Approve = 3,
        FinalApprove = 4,
        Reject = 5,
    }
    public enum ReqStatusEnum
    {
        Draft,
        Submitted,
        Closed
    }
    public enum IssueStatusEnum
    {
        Draft,
        Submitted,
        Closed
    }
    public enum POStatusEnum
    {
        Draft,
        Submitted,
        Closed
    }

    public enum ExpenseStatusEnum
    {
        Draft,
        Submitted,
        Approved,
        Closed
    }
    public enum StockTransferStatusEnum
    {
        Draft,
        Submitted,
        Reveived,
        Closed
    }
    public enum SmSStatusEnum
    {

        Draft = 1,
        Pending = 2,
        Failed = 3,
        Cancel = 4,
        Success = 9,
        All = 99,
    }

    public enum POCompletionStatusEnum
    {
        Incomplete = 1,
        Partially_Complete = 2,
        Complete = 3
    }


    public enum TaskTypeEnum
    {
        ERP = 1,
        IT = 2,
        Admin = 3,
        Accounts = 4,
        Engineering = 5,

    }
    public enum RequisitionTypeEnum
    {
        PurchaseRequisition = 1,
        StoreRequisition,

    }

    public enum DepartmentEnum
    {
        AccountsSection = 3, // Data set as on Department table
        SalesMarketingDivision,
        Purchase,
        Store,
        Production,
        FinishStore
    }


    public enum JournalEnum
    {
        BankPayment = 1,
        BankReceive,
        CashPayment,
        CashReceive,
        BillPayment,
        BillReceive,
        ContraVoucher,
        JournalVoucher,
        SalesVoucher,
        PurchaseVoucher,
        ReverseEntry
    }

    public enum GCCLJournalEnum
    {
        SalesVoucher = 9,
        PurchaseVoucher,
        ReverseEntry,
        JournalVoucher,
        ContraVoucher,
        CreditVoucher,
        DebitVoucher,
        CashVoucher,
        ProductionVoucher = 26,
        SalesReturnVoucher = 98

    }

    public enum SeedJournalEnum
    {
        JournalVoucher = 17,
        ContraVoucher = 18,
        CreditVoucher = 19,
        DebitVoucher = 20,
        CashVoucher = 21,
        SalesVoucher = 109,
        PurchaseVoucher = 110,
        AdjustmentEntry = 111,
        ProductionVoucher = 112,
        SalesReturnVoucher = 113
    }

    public enum FeedJournalEnum
    {
        JournalVoucher = 17,
        ContraVoucher = 18,
        CreditVoucher = 19,
        DebitVoucher = 20,
        CashVoucher = 21,
        SalesVoucher = 149, // 109,
        PurchaseVoucher = 110,
        RMAdjustmentEntry = 151, // 111,
        ProductionVoucher = 148,// 112,
        SalesReturnVoucher = 153, // 113,
        ProductConvertVoucher = 152
    }

    public enum ActionEnum
    {
        Add = 1,
        Edit,
        Delete,
        Detech,
        Attech,
        Approve,
        Close,
        UnApprove,
        ReOpen,
        Finalize,
        Acknowledgement
    }

    public enum ProviderEnum
    {
        Supplier = 1,
        Customer,
        RentCompany,
        CustomerAssociates,
        Subcontractor
    }

    public enum PromotionTypeEnum
    {
        FreeProduct = 1,
        PromoAmount = 2
    }

    public enum CustomerTypeEnum
    {
        Customer = 1,
        Retail = 2,
        Corporate = 3,
        Dealer = 4
    }
    public enum SupplierTypeEnum
    {
        Potato = 1,
        WaterMelon,
        Nut,
        BitterGourd,
        Others
    }

    public enum VendorsPaymentMethodEnum
    {
        Cash = 1,
        //Credit,
        //LC
    }


    public enum PaymentMethodEnum
    {
        Cash = 1,
        Bank = 2,
        Adjustment = 3,
        Debit = 10,
    }

    public enum RealStatePaymentMethodEnum
    {
        Cash = 1,
        Bank = 2,
        RemoteDeposit = 3,
        InternalTransfer = 4,
    }

    public enum KgRePaymentMethodEnum
    {
        Cash = 1,
        Bank = 2,
        OnlineBEFTN = 3,
        Mobile = 4,
    }

    public enum CustomerStatusEnum
    {
        // new enum
        AllRounder = 1,
        Beneficiary,
        CashCustomer,
        Defaulter,
        Block,
        LegalAction

        // old enum
        //Unique = 1,
        //Regular,
        //Block,
        //LegalAction

    }

    public enum HrAdminEnum
    {
        Id = 103,

    }

    public enum TicketingStatusEnum
    {
        ToDo = 1,
        InProgress = 2,
        Done = 3,
        Cancel = 4
    }

    public enum ProductStatusEnumGLDL
    {
        Booked = 471,
        Sold,
        Registered,
        UnSold,
        BookingCancelled = 481,
    }

    public enum ProductStatusEnumKPL
    {
        Booked = 1520,
        Sold,
        Registered,
        VacantFlat,
        BookingCancelled,
        LandOwner
    }

    public enum CompanyNameEnum
    {
        KrishibidGroup = 1,
        KrishibidFirmLimited = 4,
        KrishibidMultipurposeCo_operativeSoceityLimited = 5,
        KrishibidPoultryLimited = 6,
        GloriousLandsAndDevelopmentsLimited = 7,
        KrishibidFeedLimited = 8,
        KrishibidPropertiesLimited = 9,
        KrishibidFarmMachineryAndAutomobilesLimited = 10,
        KrishibidSaltLimited = 11,
        KrishibidStockAndSecuritiesLimited = 12,
        KrishiFoundation = 13,
        GloriousOverseasLimited = 14,
        KrishibidBazaarLimited = 16,
        KrishibidSecurityAndServicesLimited = 17,
        KrishibidToursTravelsLimited = 18,
        KrishibidPrintingAndPublicationLimited = 19,
        KrishibidPackagingLimited = 20,
        KrishibidSeedLimited = 21,
        KrishibidFoodAndBeverageLimited = 22,
        KrishibidTradingLimited = 23,
        GloriousCropCareLimited = 24,
        KrishibidFisheriesLimited = 25,
        HumanResourceManagementSystem = 26,
        System = 27,
        KGECOM = 28,
        MymensinghHatcheryAndFeedsLtd = 29,
        AssetManagementSystem = 30,
        TaskManagementSystem = 31,
        GloriousInternationalSchoolAndCollege = 32,
        LandAndLegalDivision = 227,
        KrishibidFillingStationLtd = 308,
        KrishibidMediaCorporationLimited = 309,
        KGBGlobalImpExLtd = 310,
        KGBTradingLimited = 311,
        KrishibidHospitalLtd = 312,
        SonaliOrganicDairyLimited = 650,
        OrganicPoultryLimited = 651,
        NaturalFishFarmingLimited = 652,
        KrishibidSafeFood = 648
    }
    public enum MonthListEnum
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

}
