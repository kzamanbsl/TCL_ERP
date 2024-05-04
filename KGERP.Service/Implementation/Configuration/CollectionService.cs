using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using KGERP.Data.Models;
using KGERP.Service.Implementation.Accounting;
using KGERP.Service.Implementation.Procurement;
using KGERP.Utility;

namespace KGERP.Service.Implementation.Configuration

{
    public class CollectionService
    {
        private readonly ERPEntities _db;
        public List<VmTransaction> Transaction { get; set; }
        private readonly AccountingService _accountingService;
        public CollectionService(ERPEntities db)
        {
            _db = db;
            _accountingService = new AccountingService(db);
        }



        #region Supplier
        public async Task<VMCommonSupplier> GetSupplier(int companyId)
        {
            VMCommonSupplier vmCommonSupplier = new VMCommonSupplier();
            vmCommonSupplier.CompanyFK = companyId;
            vmCommonSupplier.DataList = await Task.Run(() => (from t1 in _db.Vendors.Where(x => x.CompanyId == companyId)
                                                              join t2 in _db.Countries on t1.CountryId equals t2.CountryId
                                                              where t1.IsActive == true
                                                              select new VMCommonSupplier
                                                              {
                                                                  ID = t1.VendorId,
                                                                  Name = t1.Name,
                                                                  Email = t1.Email,
                                                                  Phone = t1.Phone,
                                                                  Country = t2.CountryName,
                                                                  CompanyFK = t1.CompanyId,
                                                                  Common_CountriesFk = t1.CountryId.Value,
                                                                  ContactPerson = t1.ContactName,
                                                                  Address = t1.Address,
                                                                  Code = t1.Code,
                                                                  CreatedBy = t1.CreatedBy,
                                                                  Remarks = t1.Remarks,
                                                                  VendorTypeId = t1.VendorTypeId,
                                                                  IsForeign = t1.IsForeign
                                                              }).OrderByDescending(x => x.ID).AsEnumerable());


            return vmCommonSupplier;
        }
        public VMCommonSupplier GetSupplierById(int supplierId)
        {
            VMCommonSupplier vmCommonSupplier = new VMCommonSupplier();
            var vandor = _db.Vendors.Find(supplierId);
            vmCommonSupplier.Name = vandor.Name;
            return vmCommonSupplier;
        }
        #endregion


        public class EnumModel
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }


        #region Common Customer

        public VMCommonSupplier GetCommonCustomerByID(int id)
        {
            var v = (from t1 in _db.Vendors.Where(x => x.VendorTypeId == (int)ProviderEnum.Customer && x.VendorId == id)
                     join t2 in _db.SubZones on t1.SubZoneId equals t2.SubZoneId
                     join t3 in _db.Upazilas on t1.UpazilaId equals t3.UpazilaId
                     join t4 in _db.Districts on t3.DistrictId equals t4.DistrictId

                     select new VMCommonSupplier
                     {
                         ID = t1.VendorId,
                         Name = t1.Name,
                         Email = t1.Email,
                         Phone = t1.Phone,
                         CompanyFK = t1.CompanyId,
                         SubZoneId = t1.SubZoneId.Value,
                         ZoneId = t2.ZoneId,
                         Common_DivisionFk = t4.DivisionId,
                         Common_DistrictsFk = t3.DistrictId,
                         Common_UpazilasFk = t3.UpazilaId,
                         ContactPerson = t1.ContactName,
                         Address = t1.Address,
                         Code = t1.Code,
                         CreatedBy = t1.CreatedBy,
                         Remarks = t1.Remarks,
                         IsForeign = t1.IsForeign
                     }).FirstOrDefault();
            return v;
        }
        public async Task<VMPaymentMaster> GetPaymentMasters(int companyId, DateTime? fromDate, DateTime? toDate)
        {
            VMPaymentMaster vmPaymentMaster = new VMPaymentMaster();
            vmPaymentMaster.CompanyFK = companyId;
            vmPaymentMaster.DataList = await Task.Run(() => (from t1 in _db.PaymentMasters
                                                             .Where(x => x.IsActive == true && x.VendorTypeId == (int)ProviderEnum.Customer && x.CompanyId == companyId)
                                                             join t2 in _db.VendorTypes on t1.VendorTypeId equals t2.VendorTypeId
                                                             join t3 in _db.HeadGLs on t1.BankChargeHeadGLId equals t3.Id
                                                             join t4 in _db.HeadGLs on t1.PaymentToHeadGLId equals t4.Id into t4_JOIN
                                                             from t4 in t4_JOIN.DefaultIfEmpty()
                                                             where t1.TransactionDate >= fromDate
                                                             && t1.TransactionDate <= toDate
                                                             select new VMPaymentMaster
                                                             {
                                                                 CompanyFK = t1.CompanyId,
                                                                 PaymentMasterId = t1.PaymentMasterId,
                                                                 VendorId = t1.VendorId,
                                                                 PaymentNo = t1.PaymentNo,
                                                                 BankCharge = t1.BankCharge,
                                                                 BankChargeHeadGLId = t1.BankChargeHeadGLId,
                                                                 TransactionDate = t1.TransactionDate,
                                                                 MoneyReceiptNo = t1.MoneyReceiptNo,
                                                                 MoneyReceiptDate = t1.MoneyReceiptDate,
                                                                 PaymentToHeadGLId = t1.PaymentToHeadGLId,
                                                                 ReferenceNo = t3.Remarks,
                                                                 BankChargeHeadGLName = t3.AccName,
                                                                 PaymentToHeadGLName = t4.AccName
                                                             }).OrderByDescending(x => x.PaymentMasterId).AsEnumerable());
            return vmPaymentMaster;
        }


        public async Task<VMPaymentMaster> GetPaymentMasters(int companyId, int customerId)
        {
            VMPaymentMaster vmPaymentMaster = new VMPaymentMaster();
            vmPaymentMaster.CompanyFK = companyId;
            vmPaymentMaster.DataList = await Task.Run(() => (from t1 in _db.PaymentMasters.Where(x => x.IsActive == true && x.VendorId == customerId && x.CompanyId == companyId)
                                                                 //join t3 in _db.HeadGLs on t1.PaymentToHeadGLId equals t3.Id
                                                             select new VMPaymentMaster
                                                             {
                                                                 CompanyFK = t1.CompanyId,
                                                                 PaymentMasterId = t1.PaymentMasterId,
                                                                 VendorId = t1.VendorId,
                                                                 BankCharge = t1.BankCharge,
                                                                 MoneyReceiptNo = t1.MoneyReceiptNo,
                                                                 MoneyReceiptDate = t1.MoneyReceiptDate,
                                                                 PaymentNo = t1.PaymentNo,
                                                                 ReferenceNo = t1.ReferenceNo,
                                                                 TransactionDate = t1.TransactionDate,
                                                                 TotalAmount = _db.Payments.Where(x => x.PaymentMasterId == t1.PaymentMasterId && x.IsActive).Select(x => x.InAmount).DefaultIfEmpty(0).Sum()
                                                             }).OrderByDescending(x => x.PaymentMasterId).AsEnumerable());
            return vmPaymentMaster;
        }

        #endregion

        public async Task<VMSalesOrder> ProcurementOrderMastersListGetByCustomer(int companyId, int customerId)
        {

            VMSalesOrder vmOrderMaster = new VMSalesOrder();
            vmOrderMaster = (from t1 in _db.Vendors.Where(x => x.VendorId == customerId)
                             select new VMSalesOrder
                             {
                                 CommonCustomerName = t1.Name,
                                 InAmount = _db.Payments.Where(x => x.IsActive == true && x.VendorId == t1.VendorId).Select(x => x.InAmount).DefaultIfEmpty(0).Sum()

                             }).FirstOrDefault();
            vmOrderMaster.CompanyFK = companyId;
            vmOrderMaster.DataList = await Task.Run(() => (from t1 in _db.OrderMasters.Where(x => x.IsActive && x.CompanyId == companyId && x.Status < (int)POStatusEnum.Closed && x.CustomerId == customerId)

                                                           select new VMSalesOrder
                                                           {
                                                               OrderMasterId = t1.OrderMasterId,
                                                               CustomerId = t1.CustomerId.Value,
                                                               CreatedBy = t1.CreatedBy,
                                                               CustomerPaymentMethodEnumFK = t1.PaymentMethod,
                                                               OrderNo = t1.OrderNo,
                                                               OrderDate = t1.OrderDate,
                                                               ExpectedDeliveryDate = t1.ExpectedDeliveryDate,
                                                               Status = t1.Status,
                                                               CompanyFK = t1.CompanyId,
                                                               CourierCharge = t1.CourierCharge,
                                                               TotalAmount = (_db.OrderDetails.Where(x => x.IsActive && x.OrderMasterId == t1.OrderMasterId).Select(x => x.Qty * x.UnitPrice - ((double)x.DiscountAmount)).DefaultIfEmpty(0).Sum()),

                                                           }).OrderByDescending(x => x.OrderMasterId).AsEnumerable());

            return vmOrderMaster;
        }

        public async Task<VMPurchaseOrder> ProcurementPurchaseOrdersListGetBySupplier(int companyId, int supplierId)
        {
            VMPurchaseOrder vmPurchaseOrder = new VMPurchaseOrder();
            vmPurchaseOrder = (from t1 in _db.Vendors.Where(x => x.VendorId == supplierId)
                               select new VMPurchaseOrder
                               {
                                   SupplierName = t1.Name

                               }).FirstOrDefault();
            vmPurchaseOrder.CompanyFK = companyId;
            vmPurchaseOrder.DataList = await Task.Run(() => (from t1 in _db.PurchaseOrders.Where(x => x.IsActive && x.CompanyId == companyId && x.Status < (int)POStatusEnum.Closed && x.SupplierId == supplierId)

                                                             select new VMPurchaseOrder
                                                             {
                                                                 PurchaseOrderId = t1.PurchaseOrderId,
                                                                 Common_SupplierFK = t1.SupplierId.Value,
                                                                 CreatedBy = t1.CreatedBy,
                                                                 SupplierPaymentMethodEnumFK = t1.SupplierPaymentMethodEnumFK,
                                                                 CID = t1.PurchaseOrderNo,
                                                                 OrderDate = t1.PurchaseDate,
                                                                 DeliveryDate = t1.DeliveryDate,
                                                                 DeliveryAddress = t1.DeliveryAddress,

                                                                 CompanyFK = t1.CompanyId,
                                                                 TotalPOValue = _db.PurchaseOrderDetails.Where(x => x.IsActive && x.PurchaseOrderId == t1.PurchaseOrderId).Select(x => x.PurchaseQty * x.PurchaseRate).DefaultIfEmpty(0).Sum(),

                                                                 PayableAmount = (from ts1 in _db.MaterialReceiveDetails
                                                                                  join ts2 in _db.MaterialReceives on ts1.MaterialReceiveId equals ts2.MaterialReceiveId
                                                                                  where ts2.PurchaseOrderId == t1.PurchaseOrderId && ts2.IsActive && ts1.IsActive && !ts1.IsReturn
                                                                                  select ts1.ReceiveQty * ts1.UnitPrice).DefaultIfEmpty(0).Sum(),

                                                                 InAmount = _db.Payments.Where(x => x.VendorId == supplierId && x.PurchaseOrderId == t1.PurchaseOrderId)
                                                                         .Select(x => x.InAmount).DefaultIfEmpty(0).Sum(),

                                                                 OutAmount = _db.Payments.Where(x => x.VendorId == supplierId && x.PurchaseOrderId == t1.PurchaseOrderId)
                                                                     .Select(x => x.OutAmount ?? 0).DefaultIfEmpty(0).Sum()

                                                             }).OrderByDescending(x => x.PurchaseOrderId).AsEnumerable());



            return vmPurchaseOrder;
        }
        public List<object> SubZonesDropDownList(int companyId = 0)
        {
            var list = new List<object>();
            _db.SubZones.Where(x => x.IsActive == true && x.CompanyId == companyId).Select(x => x).ToList()
        .ForEach(x => list.Add(new
        {
            Value = x.SubZoneId,
            Text = x.SalesOfficerName + " -" + x.Name
        }));
            return list;

        }

        public async Task<VMPayment> ProcurementOrderMastersGetByID(int companyId, int paymentMasterId)
        {
            VMPayment vmPayment = new VMPayment();
            if (paymentMasterId > 0)
            {
                vmPayment = await Task.Run(() => (from t2 in _db.PaymentMasters.Where(x => x.IsActive == true && x.PaymentMasterId == paymentMasterId)
                                                  join t3 in _db.HeadGLs on t2.PaymentToHeadGLId equals t3.Id into t3_join
                                                  from t3 in t3_join.DefaultIfEmpty()
                                                  join t4 in _db.HeadGLs on t2.BankChargeHeadGLId equals t4.Id into t4_join
                                                  from t4 in t4_join.DefaultIfEmpty()
                                                  join t5 in _db.VendorTypes on t2.VendorTypeId equals t5.VendorTypeId into t5_join
                                                  from t5 in t5_join.DefaultIfEmpty()



                                                  select new VMPayment
                                                  {
                                                      PaymentToHeadGLId = t2.PaymentToHeadGLId,

                                                      PaymentToHeadGLName = t3 != null ? t3.AccCode + " - " + t3.AccName : "",
                                                      BankChargeHeadGLName = t3 != null ? t4.AccCode + " - " + t4.AccName : "",
                                                      VendorTypeName = t5.Name,

                                                      BankCharge = t2.BankCharge,
                                                      BankChargeHeadGLId = t2.BankChargeHeadGLId,
                                                      IsFinalized = t2.IsFinalized,
                                                      PaymentMasterId = t2.PaymentMasterId,
                                                      PaymentNo = t2.PaymentNo,
                                                      CompanyFK = t2.CompanyId,
                                                      CompanyId = t2.CompanyId,
                                                      TransactionDate = t2.TransactionDate,
                                                      ReferenceNo = t2.ReferenceNo,


                                                  }).FirstOrDefault());
            }
            else
            {
                vmPayment.CompanyFK = companyId;
            }


            vmPayment.DataList = await Task.Run(() => (from t1 in _db.Payments.Where(x => x.IsActive == true && x.CompanyId == companyId && x.PaymentMasterId == paymentMasterId)
                                                       join t2 in _db.PaymentMasters on t1.PaymentMasterId equals t2.PaymentMasterId
                                                       join t3 in _db.OrderMasters on t1.OrderMasterId equals t3.OrderMasterId
                                                       join t0 in _db.Vendors on t1.VendorId equals t0.VendorId
                                                       join t5 in _db.HeadGLs on t1.PaymentFromHeadGLId equals t5.Id

                                                       select new VMPayment
                                                       {
                                                           TransactionDate = t1.TransactionDate,
                                                           MoneyReceiptNo = t1.MoneyReceiptNo,

                                                           InAmount = t1.InAmount,
                                                           CustomerId = t1.VendorId,
                                                           CommonCustomerName = t0.Name,
                                                           CommonCustomerCode = t0.Code,
                                                           OrderNo = t3.OrderNo,
                                                           OrderDate = t3.OrderDate,
                                                           PaymentFromHeadGLName = t5.AccCode + " - " + t5.AccName,
                                                           PaymentFromHeadGLId = t1.PaymentFromHeadGLId,
                                                           CompanyFK = t1.CompanyId,
                                                           CreatedBy = t1.CreatedBy,
                                                           PaymentId = t1.PaymentId,
                                                           ReferenceNo = t1.ReferenceNo
                                                       }).OrderByDescending(x => x.PaymentId).AsEnumerable());


            vmPayment.DataListExpenses = await Task.Run(() => (from t1 in _db.Expenses.Where(x => x.IsActive == true && x.CompanyId == companyId && x.PaymentMasterId == paymentMasterId)
                                                               join t2 in _db.HeadGLs on t1.ExpensesHeadGLId equals t2.Id
                                                               select new VMPayment
                                                               {
                                                                   ID = t1.ExpensesId,
                                                                   ExpensesAmount = t1.Amount,
                                                                   ExpensesHeadGLId = t1.ExpensesHeadGLId,
                                                                   ExpensesHeadGLName = t2.AccName,
                                                                   ExpensessReference = t1.ReferenceNo
                                                               }).OrderByDescending(x => x.ID).AsEnumerable());

            vmPayment.DataListIncome = await Task.Run(() => (from t1 in _db.Incomes.Where(x => x.IsActive == true && x.CompanyId == companyId && x.PaymentMasterId == paymentMasterId)
                                                             join t2 in _db.HeadGLs on t1.IncomeHeadGLId equals t2.Id
                                                             select new VMPayment
                                                             {
                                                                 ID = t1.IncomeId,
                                                                 OthersIncomeAmount = t1.Amount,
                                                                 OthersIncomeHeadGLId = t1.IncomeHeadGLId,
                                                                 OthersIncomeHeadGLName = t2.AccName,
                                                                 IncomeReference = t1.ReferenceNo
                                                             }).OrderByDescending(x => x.ID).AsEnumerable());



            return vmPayment;
        }

        public List<object> OrderMastersDropDownList(int companyId, int customerId)
        {
            var orderMastersList = new List<object>();
            var orderMasters = _db.OrderMasters.Where(a => a.IsActive == true && a.CompanyId == companyId && a.CustomerId == customerId).OrderByDescending(x => x.OrderMasterId).ToList();
            foreach (var x in orderMasters)
            {
                orderMastersList.Add(new { Text = x.OrderNo + " Date: " + x.OrderDate.ToLongDateString(), Value = x.OrderMasterId });
            }
            return orderMastersList;
        }

        public List<object> MaterialReceivedDropDownList(int companyId, int supplierId)
        {
            var workOrderList = (from t1 in _db.MaterialReceives
                                 join t2 in _db.PurchaseOrders on t1.PurchaseOrderId equals t2.PurchaseOrderId
                                 where t1.IsActive && t1.IsSubmitted
                                       && t2.CompanyId == companyId && t2.SupplierId == supplierId
                                 orderby t2.PurchaseDate
                                 select new
                                 {
                                     Value = t1.MaterialReceiveId,
                                     PurchaseOrderNo = t2.PurchaseOrderNo,
                                     PurchaseDate = t2.PurchaseDate
                                 }).ToList();

            var formattedList = workOrderList.Select(item => new
            {
                Value = item.Value,
                Text = $"{item.PurchaseOrderNo} Date: {item.PurchaseDate.Value.ToString("MM/dd/yyyy")}"
            }).ToList<object>();

            return formattedList;
        }




        public List<object> PurchaseOrdersDropDownList(int companyId, int customerId)
        {
            var purchaseOrdersList = new List<object>();
            var orderMasters = _db.PurchaseOrders.Where(a => a.IsActive == true && !a.IsCancel && !a.IsHold && a.CompanyId == companyId && a.SupplierId == customerId).OrderByDescending(x => x.PurchaseOrderId).ToList();
            foreach (var x in orderMasters)
            {
                purchaseOrdersList.Add(new { Text = x.PurchaseOrderNo + " Date: " + x.PurchaseDate.Value.ToLongDateString(), Value = x.PurchaseOrderId });
            }
            return purchaseOrdersList;
        }

        //public async Task<VMPayment> ProcurementPurchaseOrdersGetByID(int companyId, int supplierId, int paymentMasterId)
        //{
        //    VMPayment paymentVm = new VMPayment();

        //    if (paymentMasterId > 0)
        //    {
        //        paymentVm = await Task.Run(() => (from t2 in _db.PaymentMasters.Where(x => x.IsActive == true && x.PaymentMasterId == paymentMasterId)
        //                                          join t1 in _db.Vendors.Where(x => x.VendorId == supplierId && x.IsActive && x.CompanyId == companyId)
        //                                              on t2.VendorId equals t1.VendorId
        //                                          join t3 in _db.HeadGLs on t2.BankChargeHeadGLId equals t3.Id
        //                                          join t4 in _db.HeadGLs on t2.PaymentToHeadGLId equals t4.Id


        //                                          select new VMPayment
        //                                          {
        //                                              ACName = t1.ACName,
        //                                              ACNo = t1.ACNo,
        //                                              BankName = t1.BankId.ToString(),
        //                                              BranchName = t1.BranchId.ToString(),

        //                                              PaymentFromHeadGLId = t2.PaymentToHeadGLId,
        //                                              BankCharge = t2.BankCharge,
        //                                              BankChargeHeadGLId = t2.BankChargeHeadGLId,
        //                                              IsFinalized = t2.IsFinalized,
        //                                              PaymentMasterId = t2.PaymentMasterId,
        //                                              PaymentNo = t2.PaymentNo,
        //                                              CustomerId = t1.VendorId,
        //                                              CompanyId = companyId,
        //                                              CompanyFK = t2.CompanyId,
        //                                              MoneyReceiptNo = t2.MoneyReceiptNo,
        //                                              TransactionDate = t2.TransactionDate,
        //                                              ReferenceNo = t2.ReferenceNo,
        //                                              CommonCustomerName = t1.Name,
        //                                              CommonCustomerCode = t1.Code,
        //                                              PaymentToHeadGLId = t2.PaymentToHeadGLId,
        //                                              PaymentToHeadGLName = t4.AccCode + " - " + t4.AccName,
        //                                              PaymentFromHeadGLName = t3.AccCode + " - " + t3.AccName,


        //                                              PayableAmountDecimal = (from ts1 in _db.MaterialReceiveDetails
        //                                                                      join ts2 in _db.MaterialReceives on ts1.MaterialReceiveId equals ts2.MaterialReceiveId
        //                                                                      join ts3 in _db.PurchaseOrders on ts2.PurchaseOrderId equals ts3.PurchaseOrderId

        //                                                                      where ts3.SupplierId == supplierId && ts2.IsActive && ts1.IsActive && !ts1.IsReturn
        //                                                                      select ts1.ReceiveQty * ts1.UnitPrice).DefaultIfEmpty(0).Sum(),
        //                                              ReturnAmount = (from ts1 in _db.PurchaseReturnDetails
        //                                                              join ts2 in _db.PurchaseReturns.Where(x => x.SupplierId == t1.VendorId && x.CompanyId == t1.CompanyId) on ts1.PurchaseReturnId equals ts2.PurchaseReturnId
        //                                                              where ts1.IsActive && ts2.Active
        //                                                              select ts1.Qty.Value * ts1.Rate.Value).DefaultIfEmpty(0).Sum(),

        //                                              InAmount = _db.Payments.Where(x => x.VendorId == supplierId)
        //                                                               .Select(x => x.InAmount).DefaultIfEmpty(0).Sum()
        //                                          }).FirstOrDefault());
        //    }
        //    else
        //    {
        //        paymentVm = await Task.Run(() => (from t1 in _db.Vendors.Where(x => x.VendorId == supplierId && x.IsActive == true && x.CompanyId == companyId)
        //                                          join t2 in _db.Banks on t1.BankId equals t2.BankId
        //                                          join t3 in _db.BankBranches on t2.BankId equals t3.BankId
        //                                          select new VMPayment
        //                                          {
        //                                              ACName = t1.ACName,
        //                                              ACNo = t1.ACNo,
        //                                              //BankId = t1.BankId,
        //                                              BankName = t2.Name,
        //                                              //BranchId = t1.BranchId,
        //                                              BranchName = t3.Name,
        //                                              CustomerId = t1.VendorId,
        //                                              CompanyId = companyId,

        //                                              CommonCustomerName = t1.Name,
        //                                              CommonCustomerCode = t1.Code,
        //                                              CompanyFK = t1.CompanyId,

        //                                              PayableAmountDecimal = (from ts1 in _db.MaterialReceiveDetails
        //                                                                      join ts2 in _db.MaterialReceives on ts1.MaterialReceiveId equals ts2.MaterialReceiveId
        //                                                                      join ts3 in _db.PurchaseOrders on ts2.PurchaseOrderId equals ts3.PurchaseOrderId

        //                                                                      where ts3.SupplierId == supplierId && ts2.IsActive && ts1.IsActive && !ts1.IsReturn
        //                                                                      select ts1.ReceiveQty * ts1.UnitPrice).DefaultIfEmpty(0).Sum(),

        //                                              ReturnAmount = (from ts1 in _db.PurchaseReturnDetails
        //                                                              join ts2 in _db.PurchaseReturns.Where(x => x.SupplierId == t1.VendorId && x.CompanyId == t1.CompanyId) on ts1.PurchaseReturnId equals ts2.PurchaseReturnId
        //                                                              //where ts1.IsActive && ts2.IsActive
        //                                                              select ts1.Qty.Value * ts1.Rate.Value).DefaultIfEmpty(0).Sum(),

        //                                              InAmount = _db.Payments.Where(x => x.VendorId == supplierId)
        //                                                               .Select(x => x.InAmount).DefaultIfEmpty(0).Sum(),

        //                                              OutAmount = _db.Payments.Where(x => x.VendorId == supplierId)
        //                                                  .Select(x => x.OutAmount).DefaultIfEmpty(0).Sum()

        //                                          }).FirstOrDefault());
        //    }

        //    paymentVm.DataList = await Task.Run(() => (from t1 in _db.Payments.Where(x => x.VendorId == supplierId && x.IsActive && x.CompanyId == companyId && x.PaymentMasterId == paymentMasterId)
        //                                               join t2 in _db.PaymentMasters on t1.PaymentMasterId equals t2.PaymentMasterId
        //                                               join t3 in _db.PurchaseOrders on t1.PurchaseOrderId equals t3.PurchaseOrderId

        //                                               select new VMPayment
        //                                               {
        //                                                   CreatedBy = t1.CreatedBy ?? "",
        //                                                   TransactionDate = t1.TransactionDate,
        //                                                   OrderNo = t3.PurchaseOrderNo ?? "",
        //                                                   OrderDate = t3.PurchaseDate.Value,
        //                                                   CompanyFK = t1.CompanyId,
        //                                                   CompanyId = t1.CompanyId,
        //                                                   OutAmount = t1.OutAmount,
        //                                                   PaymentId = t1.PaymentId,
        //                                                   PaymentToHeadGLId = t1.PaymentFromHeadGLId ?? 0
        //                                               }).OrderByDescending(x => x.PaymentId).AsEnumerable());

        //    return paymentVm;
        //}

        public async Task<VMPayment> ProcurementPurchaseOrdersGetByID(int companyId, int supplierId, int paymentMasterId)
        {
            VMPayment paymentVm = new VMPayment();

            if (paymentMasterId > 0)
            {
                paymentVm = await Task.Run(() =>
                     (from t1 in _db.PaymentMasters.Where(x => x.IsActive && x.PaymentMasterId == paymentMasterId && x.VendorId == supplierId && x.CompanyId == companyId)
                      join t2 in _db.Vendors on t1.VendorId equals t2.VendorId
                      join t3 in _db.HeadGLs on t1.BankChargeHeadGLId equals t3.Id
                      join t4 in _db.HeadGLs on t1.PaymentToHeadGLId equals t4.Id
                      select new VMPayment
                      {
                          ACName = t2.ACName,
                          ACNo = t2.ACNo,
                          BankName = t2.BankId.ToString(),
                          BranchName = t2.BranchId.ToString(),
                          PaymentFromHeadGLId = t1.PaymentToHeadGLId,
                          BankCharge = t1.BankCharge,
                          BankChargeHeadGLId = t1.BankChargeHeadGLId,
                          BankAccountInfoId = t1.BankAccountInfoId ?? 0,
                          IsFinalized = t1.IsFinalized,
                          PaymentMasterId = t1.PaymentMasterId,
                          PaymentNo = t1.PaymentNo,
                          CustomerId = t2.VendorId,
                          CompanyId = companyId,
                          CompanyFK = t1.CompanyId,
                          MoneyReceiptNo = t1.MoneyReceiptNo ?? "N/A",
                          TransactionDate = t1.TransactionDate,
                          ReferenceNo = t1.ReferenceNo,
                          CommonCustomerName = t2.Name,
                          CommonCustomerCode = t2.Code,
                          PaymentToHeadGLId = t1.PaymentToHeadGLId,
                          PaymentToHeadGLName = t4.AccCode + " - " + t4.AccName,
                          PaymentFromHeadGLName = t3.AccCode + " - " + t3.AccName,
                          PayableAmountDecimal = (from ts1 in _db.MaterialReceiveDetails
                                                  join ts2 in _db.MaterialReceives on ts1.MaterialReceiveId equals ts2.MaterialReceiveId
                                                  join ts3 in _db.PurchaseOrders on ts2.PurchaseOrderId equals ts3.PurchaseOrderId
                                                  where ts3.SupplierId == supplierId && ts2.IsActive && ts1.IsActive && !ts1.IsReturn
                                                  select ts1.ReceiveQty * ts1.UnitPrice).DefaultIfEmpty(0).Sum(),

                          ReturnAmount = (from ts1 in _db.PurchaseReturnDetails
                                          join ts2 in _db.PurchaseReturns on ts1.PurchaseReturnId equals ts2.PurchaseReturnId
                                          where ts1.IsActive && ts2.Active && ts2.SupplierId == t2.VendorId && ts2.CompanyId == t2.CompanyId
                                          select ts1.Qty.Value * ts1.Rate.Value).DefaultIfEmpty(0).Sum(),

                          InAmount = _db.Payments.Where(x => x.VendorId == supplierId).Select(x => x.InAmount).DefaultIfEmpty(0).Sum()
                      }).FirstOrDefault());
            }
            else
            {
                paymentVm = await Task.Run(() =>
                    (from t1 in _db.Vendors.Where(x => x.VendorId == supplierId && x.IsActive && x.CompanyId == companyId)
                     join t2 in _db.Banks on t1.BankId equals t2.BankId
                     join t3 in _db.BankBranches on t2.BankId equals t3.BankId
                     select new VMPayment
                     {
                         ACName = t1.ACName,
                         ACNo = t1.ACNo,
                         BankName = t2.Name,
                         BranchName = t3.Name,
                         CustomerId = t1.VendorId,
                         CompanyId = companyId,
                         CommonCustomerName = t1.Name,
                         CommonCustomerCode = t1.Code,
                         CompanyFK = t1.CompanyId,
                         PayableAmountDecimal = (from ts1 in _db.MaterialReceiveDetails
                                                 join ts2 in _db.MaterialReceives on ts1.MaterialReceiveId equals ts2.MaterialReceiveId
                                                 join ts3 in _db.PurchaseOrders on ts2.PurchaseOrderId equals ts3.PurchaseOrderId
                                                 where ts3.SupplierId == supplierId && ts2.IsActive && ts1.IsActive && !ts1.IsReturn
                                                 select ts1.ReceiveQty * ts1.UnitPrice).DefaultIfEmpty(0).Sum(),

                         ReturnAmount = (from ts1 in _db.PurchaseReturnDetails
                                         join ts2 in _db.PurchaseReturns on ts1.PurchaseReturnId equals ts2.PurchaseReturnId
                                         where ts2.SupplierId == t1.VendorId && ts2.CompanyId == t1.CompanyId
                                         select ts1.Qty.Value * ts1.Rate.Value).DefaultIfEmpty(0).Sum(),

                         InAmount = _db.Payments.Where(x => x.VendorId == supplierId).Select(x => x.InAmount).DefaultIfEmpty(0).Sum(),

                         OutAmount = _db.Payments.Where(x => x.VendorId == supplierId).Select(x => x.OutAmount).DefaultIfEmpty(0).Sum()
                     }).FirstOrDefault());
            }

            paymentVm.DataList = await Task.Run(() =>
            (from t1 in _db.Payments
             join t2 in _db.PaymentMasters on t1.PaymentMasterId equals t2.PaymentMasterId into t2_Join
             from t2 in t2_Join.DefaultIfEmpty()
             join t3 in _db.MaterialReceives.Where(x => x.IsActive) on t1.PurchaseOrderId equals t3.PurchaseOrderId into t3_Join
             from t3 in t3_Join.DefaultIfEmpty()
             join t4 in _db.PurchaseOrders.Where(x => x.IsActive) on t3.PurchaseOrderId equals t4.PurchaseOrderId into t4_Join
             from t4 in t4_Join.DefaultIfEmpty()
             where t1.VendorId == supplierId && t1.IsActive && t1.CompanyId == companyId && t1.PaymentMasterId == paymentMasterId
             select new VMPayment
             {
                 CreatedBy = t1.CreatedBy ?? "",
                 TransactionDate = t1.TransactionDate,
                 OrderNo = t4 != null ? t4.PurchaseOrderNo ?? "" : "",
                 OrderDate = t4 != null && t4.PurchaseDate != null ? (DateTime)t4.PurchaseDate : DateTime.MinValue,
                 CompanyFK = t1.CompanyId,
                 CompanyId = t1.CompanyId,
                 OutAmount = t1.OutAmount,
                 PaymentId = t1.PaymentId,
                 PaymentToHeadGLId = t1.PaymentFromHeadGLId ?? 0
             }).OrderByDescending(x => x.PaymentId).ToList());
            paymentVm.CompanyFK = companyId;

            return paymentVm;
        }

        public async Task<int> PaymentMasterAdd(VMPayment vmPayment)
        {
            int result = -1;
            var vendors = _db.Vendors.FirstOrDefault(x => x.VendorId == vmPayment.CustomerId);
            var bankInfo = _db.BankAccountInfoes.FirstOrDefault(x => x.BankAccountInfoId == vmPayment.BankAccountInfoId);

            #region PaymentNo
            int paymentMastersCount = _db.PaymentMasters.Count(x => x.CompanyId == vmPayment.CompanyFK && x.VendorId == vmPayment.CustomerId);

            if (paymentMastersCount == 0)
            {
                paymentMastersCount = 1;
            }
            else
            {
                paymentMastersCount++;
            }

            string paymentNo = "C-" + vendors?.Code + "-" + paymentMastersCount.ToString().PadLeft(4, '0');
            #endregion

            PaymentMaster paymentMaster = new PaymentMaster
            {
                PaymentNo = paymentNo,
                TransactionDate = vmPayment.TransactionDate,
                VendorId = vmPayment.CustomerId.Value,
                ReferenceNo = vmPayment.ReferenceNo,
                IsAdjust = vmPayment.IsAdjust,
                VendorTypeId = vendors.VendorTypeId,
                BankChargeHeadGLId = bankInfo.AccountingHeadId,
                BankCharge = vmPayment.BankCharge,
                BankAccountInfoId = vmPayment.BankAccountInfoId,
                CompanyId = vmPayment.CompanyFK.Value,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                PaymentToHeadGLId = 29833,
                CreatedDate = DateTime.Now,
                IsActive = true
            };
            _db.PaymentMasters.Add(paymentMaster);
            if (await _db.SaveChangesAsync() > 0)
            {
                result = paymentMaster.PaymentMasterId;
            }
            return result;
        }

        public async Task<long> PaymentAdd(VMPayment vmPayment)
        {
            long result = -1;
            var vendor = _db.Vendors.FirstOrDefault(x => x.VendorId == vmPayment.CustomerId);
            var bankInfo = _db.BankAccountInfoes.FirstOrDefault(x => x.BankAccountInfoId == vmPayment.BankAccountInfoId);

            Payment payment = new Payment
            {
                PaymentMasterId = vmPayment.PaymentMasterId,
                InAmount = Convert.ToDecimal(vmPayment.InAmount),
                OutAmount = vmPayment.OutAmount,
                ProductType = "F",
                ReferenceNo = vmPayment.ReferenceNo,
                TransactionDate = vmPayment.TransactionDate.Date,
                MoneyReceiptNo = vmPayment.MoneyReceiptNo,
                VendorId = vmPayment.CustomerId.Value,
                OrderMasterId = vmPayment.OrderMasterId,
                PurchaseOrderId = vmPayment.PurchaseOrderId,
                CompanyId = vmPayment.CompanyFK.Value,
                PaymentFromHeadGLId = bankInfo.AccountingHeadId,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreatedDate = DateTime.Now,
                IsActive = true,
            };

            _db.Payments.Add(payment);

            if (await _db.SaveChangesAsync() > 0)
            {
                result = payment.PaymentId;
            }

            return result;
        }

        public async Task<long> ExpensesAdd(VMPayment vmPayment)
        {
            long result = -1;

            Expense expense = new Expense
            {
                Amount = vmPayment.ExpensesAmount.Value,
                CompanyId = vmPayment.CompanyFK.Value,
                ExpensesHeadGLId = vmPayment.ExpensesHeadGLId,
                PaymentMasterId = vmPayment.PaymentMasterId,
                ReferenceNo = vmPayment.ExpensessReference,
                OutAmount = 0,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreatedDate = DateTime.Now,
                IsActive = true,

            };
            _db.Expenses.Add(expense);
            if (await _db.SaveChangesAsync() > 0)
            {
                result = expense.ExpensesId;
            }


            return result;
        }

        public async Task<long> IncomeAdd(VMPayment vmPayment)
        {
            long result = -1;

            Income income = new Income
            {
                Amount = vmPayment.OthersIncomeAmount.Value,
                CompanyId = vmPayment.CompanyFK.Value,
                IncomeHeadGLId = vmPayment.OthersIncomeHeadGLId,
                PaymentMasterId = vmPayment.PaymentMasterId,
                ReferenceNo = vmPayment.IncomeReference,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreatedDate = DateTime.Now,
                IsActive = true,

            };
            _db.Incomes.Add(income);
            if (await _db.SaveChangesAsync() > 0)
            {
                result = income.IncomeId;
            }


            return result;
        }
        public async Task<int> SubmitCollectionMasters(VMPayment vmPayment)
        {
            var result = -1;
            PaymentMaster model = await _db.PaymentMasters.FindAsync(vmPayment.PaymentMasterId);
            model.IsFinalized = true;

            model.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            model.ModifiedDate = DateTime.Now;

            if (await _db.SaveChangesAsync() > 0)
            {
                result = model.PaymentMasterId;
            }

            if (result > 0 && vmPayment.CompanyFK == (int)CompanyNameEnum.GloriousCropCareLimited)
            {

                vmPayment = await Task.Run(() => ProcurementOrderMastersGetByID(vmPayment.CompanyFK.Value, model.PaymentMasterId)); // , model.VendorId, 

                await _accountingService.CollectionPushGCCL(vmPayment.CompanyFK.Value, vmPayment, (int)GCCLJournalEnum.CreditVoucher);


            }
            else if (result > 0 && vmPayment.CompanyFK == (int)CompanyNameEnum.KrishibidSeedLimited)
            {

                vmPayment = await Task.Run(() => ProcurementOrderMastersGetByID(vmPayment.CompanyFK.Value, model.PaymentMasterId)); // , model.VendorId, 

                await _accountingService.CollectionPushGCCL(vmPayment.CompanyFK.Value, vmPayment, (int)GCCLJournalEnum.CreditVoucher);


            }
            return result;
        }

        public async Task<int> SubmitPaymentMasters(VMPayment vmPayment)
        {
            var result = -1;
            PaymentMaster model = await _db.PaymentMasters.FindAsync(vmPayment.PaymentMasterId);
            model.IsFinalized = true;

            model.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            model.ModifiedDate = DateTime.Now;
            if (await _db.SaveChangesAsync() > 0)
            {
                result = model.PaymentMasterId;
            }
            if (result > 0 && model.CompanyId == (int)CompanyNameEnum.KrishibidSeedLimited)
            {

                vmPayment = await Task.Run(() => ProcurementPurchaseOrdersGetByID(model.CompanyId, model.VendorId, model.PaymentMasterId));

                await _accountingService.PaymentPushGCCL(model.CompanyId, vmPayment, (int)GCCLJournalEnum.DebitVoucher);

            }
            //Deposit Adjust started here
            if (result > 0 && model.IsFinalized == true)
            {
                var paymentDetails = await _db.Payments.Where(x => x.PaymentMasterId == model.PaymentMasterId).ToListAsync();
                var vendorDeposits = await _db.VendorDeposits.Where(x => x.VendorId == model.VendorId && (x.DepositAmount - x.AdjustedAmount) > 0).OrderBy(o => o.VendorDepositId).ToListAsync();

                decimal adjustableAmount = 0;
                if (vendorDeposits != null)
                {
                    adjustableAmount = vendorDeposits.Select(x => x.DepositAmount).Sum() - vendorDeposits.Select(x => x.AdjustedAmount).Sum();
                }
                if (paymentDetails != null && adjustableAmount > 0)
                {
                    foreach (var payment in paymentDetails)
                    {
                        if (adjustableAmount <= 0)
                        {
                            break;
                        }

                        VendorDepositHistory vendorDepositHistory = new VendorDepositHistory();
                        vendorDepositHistory.VendorId = model.VendorId;
                        vendorDepositHistory.VendorTypeId = model.VendorTypeId;
                        // vendorDepositHistory.OrderMasterId = payment.OrderMasterId;
                        vendorDepositHistory.PurchaseOrderId = payment.PurchaseOrderId;
                        if (adjustableAmount >= payment.InAmount)
                        {
                            vendorDepositHistory.OpeningDepositAmount = 0;
                            vendorDepositHistory.VendorDepositId = 0;

                            vendorDepositHistory.PaymentAmount = payment.InAmount;
                            adjustableAmount = adjustableAmount - payment.InAmount;
                        }
                        else
                        {
                            vendorDepositHistory.OpeningDepositAmount = 0;
                            vendorDepositHistory.VendorDepositId = 0;

                            vendorDepositHistory.PaymentAmount = adjustableAmount;
                            adjustableAmount = 0;
                        }

                        vendorDepositHistory.HistoryDate = payment.CreatedDate;
                        vendorDepositHistory.CompanyId = model.CompanyId;
                        vendorDepositHistory.CreateDate = DateTime.Now;
                        vendorDepositHistory.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                        vendorDepositHistory.IsActive = true;

                    }
                }
            }

            return result;
        }

        public async Task<long> SupplierPaymentAdd(VMPurchaseOrder vmPurchaseOrder)
        {
            long result = -1;

            Payment payment = new Payment
            {

                InAmount = Convert.ToDecimal(vmPurchaseOrder.PaidAmount),
                TransactionDate = vmPurchaseOrder.PaymentDate,
                VendorId = vmPurchaseOrder.Common_SupplierFK.Value,
                ReferenceNo = vmPurchaseOrder.Remarks,
                ProductType = "R",


                CompanyId = vmPurchaseOrder.CompanyFK.Value,
                CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                CreatedDate = DateTime.Now,
                IsActive = true
            };
            _db.Payments.Add(payment);
            if (await _db.SaveChangesAsync() > 0)
            {
                result = payment.PaymentId;
            }
            return result;
        }


        public async Task<VmTransaction> GetLedgerInfoBySupplier(VmTransaction vmTransaction)
        {
            List<VmTransaction> tempList = new List<VmTransaction>();
            var vendorOpenings = (from t1 in _db.PurchaseOrders
                                  join t2 in _db.Vendors on t1.SupplierId equals t2.VendorId

                                  where t1.SupplierId == vmTransaction.VendorFK && t1.IsActive == true && t1.Status == (int)POStatusEnum.Submitted && t1.IsOpening
                                  select new VmTransaction
                                  {
                                      PurchaseOrdersId = t1.PurchaseOrderId,
                                      Date = t1.PurchaseDate,
                                      Description = "Opening PO ID : " + t1.PurchaseOrderNo,
                                      Credit = (from ts1 in _db.PurchaseOrderDetails
                                                join ts2 in _db.MaterialReceiveDetails on ts1.PurchaseOrderDetailId equals ts2.PurchaseOrderDetailFk
                                                where ts1.PurchaseOrderId == t1.PurchaseOrderId && ts1.IsActive && ts2.IsActive && !ts2.IsReturn
                                                select ts2.ReceiveQty * ts1.PurchaseRate).DefaultIfEmpty(0).Sum(),//Debit
                                      Debit = 0,
                                      Balance = 0,
                                      FirstCreateDate = t1.CreatedDate
                                  }).Distinct().AsEnumerable();

            var dataList1 = (from t1 in _db.PurchaseOrders
                             join t2 in _db.Vendors on t1.SupplierId equals t2.VendorId

                             where t1.SupplierId == vmTransaction.VendorFK && t1.IsActive == true && t1.Status == (int)POStatusEnum.Submitted && !t1.IsOpening
                             select new VmTransaction
                             {
                                 PurchaseOrdersId = t1.PurchaseOrderId,
                                 Date = t1.PurchaseDate,
                                 Description = "PO ID : " + t1.PurchaseOrderNo,
                                 Credit = (from ts1 in _db.PurchaseOrderDetails
                                           join ts2 in _db.MaterialReceiveDetails on ts1.PurchaseOrderDetailId equals ts2.PurchaseOrderDetailFk
                                           where ts1.PurchaseOrderId == t1.PurchaseOrderId && ts1.IsActive && ts2.IsActive && !ts2.IsReturn
                                           select ts2.ReceiveQty * ts1.PurchaseRate).DefaultIfEmpty(0).Sum(),//Debit
                                 Debit = 0,
                                 Balance = 0,
                                 FirstCreateDate = t1.CreatedDate
                             }).Distinct().AsEnumerable();


            var dataList2 = (from t1 in _db.Payments
                             join t2 in _db.PaymentMasters on t1.PaymentMasterId equals t2.PaymentMasterId into t2_join
                             from t2 in t2_join.DefaultIfEmpty()
                             where t1.VendorId == vmTransaction.VendorFK && t1.IsActive == true
                             select new VmTransaction
                             {
                                 PaymentMasterId = t2.PaymentMasterId,
                                 VendorFK = vmTransaction.VendorFK,
                                 Date = t1.TransactionDate,
                                 Description = "Payment Reference : " + t1.ReferenceNo,
                                 Credit = 0,
                                 Debit = t1.OutAmount ?? 0,
                                 Balance = 0,
                                 FirstCreateDate = t1.CreatedDate
                             }).Distinct().AsEnumerable();

            var dataList = vendorOpenings.Union(dataList1).Union(dataList2).OrderBy(x => x.Date).AsEnumerable();


            var previousBalanceTable = (from t in dataList
                                        where t.Date < vmTransaction.FromDate
                                        select
                                        (
                                            t.Credit - t.Debit
                                        )).ToList();

            var countForId = previousBalanceTable.Count();

            var previousBalance = (previousBalanceTable).DefaultIfEmpty(0).Sum();


            var sortedV = (from t in dataList
                           where t.Date >= vmTransaction.FromDate && t.Date <= vmTransaction.ToDate
                           select new VmTransaction
                           {
                               ID = ++countForId,
                               Date = t.Date,
                               Description = t.Description,
                               Credit = t.Credit,
                               Debit = t.Debit,
                               Balance = 0,
                               PurchaseOrdersId = t.PurchaseOrdersId,
                               PaymentMasterId = t.PaymentMasterId,
                               VendorFK = t.VendorFK
                           }).Distinct().ToList();


            var supplier = await _db.Vendors.FindAsync(vmTransaction.VendorFK);
            var companies = await _db.Companies.FindAsync(vmTransaction.CompanyFK);

            #region Vendor Opening from VendorOpening Table

            // var openingBalance = _db.VendorOpenings.Where(c =>
            //c.VendorId == vmTransaction.VendorFK && c.IsActive == true && c.IsSubmit == true).ToList();

            VmTransaction vmTransition = new VmTransaction();
            vmTransition.Date = vmTransaction.FromDate;
            vmTransition.Name = supplier.Name;
            vmTransition.Description = "Opening Balance";
            vmTransition.Debit = 0;
            //vmTransition.Credit = openingBalance.Sum(c => c.OpeningAmount);
            vmTransition.Balance = previousBalance;
            vmTransition.CompanyAddress = companies.Address;
            vmTransition.CompanyName = companies.Name;
            vmTransition.CompanyPhone = companies.Phone;
            vmTransition.CompanyEmail = companies.Email;
            vmTransition.FromDate = vmTransaction.FromDate;
            vmTransition.ToDate = vmTransaction.ToDate;
            //tempList.Add(vmTransition);
            sortedV.Insert(0, vmTransition);
            #endregion

            foreach (var x in sortedV)
            {
                x.Balance = previousBalance += x.Credit - x.Debit;
                x.Name = supplier.Name;
                x.PurchaseOrdersId = x.PurchaseOrdersId;

                tempList.Add(x);
            }
            vmTransition.DataList = tempList;

            return vmTransition;
        }

        public async Task<VmTransaction> GetLedgerInfoByCustomer(VmTransaction vmTransaction)
        {
            List<VmTransaction> tempList = new List<VmTransaction>();

            var vendorOpenings = (from t1 in _db.OrderMasters
                                  join t2 in _db.Vendors on t1.CustomerId equals t2.VendorId

                                  where t1.CustomerId == vmTransaction.VendorFK && t1.IsActive == true && t1.Status == (int)POStatusEnum.Submitted && t1.IsOpening
                                  select new VmTransaction
                                  {
                                      OrderMasterId = t1.OrderMasterId,
                                      Date = t1.OrderDate,
                                      Description = "Opening Order No : " + t1.OrderNo,
                                      Credit = 0,
                                      Debit = 0,
                                      Balance = 0,
                                      CourierCharge = t1.CourierCharge,
                                      FirstCreateDate = t1.CreateDate
                                  }).Distinct().ToList();
            foreach (var data in vendorOpenings)
            {
                data.Credit = GetOrderValue(data.OrderMasterId) + Convert.ToDecimal(data.CourierCharge);
            }

            var dataList1 = (from t1 in _db.OrderMasters
                             join t2 in _db.Vendors on t1.CustomerId equals t2.VendorId

                             where t1.CustomerId == vmTransaction.VendorFK && t1.IsActive == true && t1.Status == (int)POStatusEnum.Submitted && !t1.IsOpening
                             select new VmTransaction
                             {
                                 OrderMasterId = t1.OrderMasterId,
                                 Date = t1.OrderDate,
                                 Description = "Order No : " + t1.OrderNo,
                                 Credit = 0,
                                 Debit = 0,
                                 Balance = 0,
                                 CourierCharge = t1.CourierCharge,
                                 FirstCreateDate = t1.CreateDate
                             }).Distinct().ToList();

            foreach (var data in dataList1)
            {
                data.Credit = GetOrderValue(data.OrderMasterId) + Convert.ToDecimal(data.CourierCharge);
            }


            var dataList2 = (from t1 in _db.Payments
                             join t2 in _db.PaymentMasters on t1.PaymentMasterId equals t2.PaymentMasterId into t2_join
                             from t2 in t2_join.DefaultIfEmpty()
                             where t1.VendorId == vmTransaction.VendorFK && t1.IsActive == true
                             select new VmTransaction
                             {
                                 //    OrderMasterId = (long)t1.OrderMasterId,
                                 PaymentMasterId = t2.PaymentMasterId,
                                 Date = t1.TransactionDate,
                                 Description = "Payment Reference : " + t1.ReferenceNo,
                                 Credit = 0,
                                 Debit = t1.InAmount,
                                 Balance = 0,
                                 FirstCreateDate = t1.CreatedDate
                             }).Distinct().ToList();

            var dataList3 = (from t1 in _db.SaleReturnDetails
                             join t2 in _db.SaleReturns on t1.SaleReturnId equals t2.SaleReturnId
                             where t2.CustomerId == vmTransaction.VendorFK && t1.IsActive == true
                             select new VmTransaction
                             {
                                 SaleReturnId = t2.SaleReturnId,
                                 Date = t2.ReturnDate,
                                 Description = "Return Reference : " + t2.SaleReturnNo,
                                 Credit = 0,
                                 Debit = 0, //t1.InAmount,
                                 Balance = 0,
                                 FirstCreateDate = t2.ReturnDate
                             }).Distinct().ToList();

            foreach (var data in dataList3)
            {
                data.Debit = GetReturnOrderValue((long)data.SaleReturnId); // Sales Return Id
            }

            var dataList4 = (from t1 in _db.VendorDeposits
                             where t1.VendorId == vmTransaction.VendorFK && t1.IsActive == true && t1.IsSubmit == true
                             select new VmTransaction
                             {
                                 VendorDepositId = t1.VendorDepositId,
                                 Date = t1.DepositDate,
                                 Description = t1.Description,
                                 Credit = 0,
                                 Debit = 0,
                                 Balance = 0,
                                 FirstCreateDate = t1.CreateDate,
                                 VendorDepositAmount = t1.DepositAmount,
                                 VendorAdjustedAmount = t1.AdjustedAmount
                             }).Distinct().ToList();

            foreach (var data in dataList4)
            {
                data.Debit = ((decimal)data.VendorDepositAmount - (decimal)data.VendorAdjustedAmount); // Sales Return Id
            }

            var dataList = vendorOpenings.Union(dataList1).Union(dataList2).Union(dataList3).Union(dataList4).OrderBy(x => x.Date).ToList();


            var previousBalanceTable = (from t in dataList
                                        where t.Date < vmTransaction.FromDate
                                        select
                                        (
                                            t.Credit - t.Debit
                                        )).ToList();

            var countForId = previousBalanceTable.Count();

            var previousBalance = (previousBalanceTable).DefaultIfEmpty(0).Sum();

            var sortedV = (from t in dataList
                           where t.Date >= vmTransaction.FromDate && t.Date <= vmTransaction.ToDate
                           select new VmTransaction
                           {
                               ID = ++countForId,
                               Date = t.Date,
                               Description = t.Description,
                               Credit = t.Credit,
                               Debit = t.Debit,
                               Balance = 0,
                               OrderMasterId = t.OrderMasterId,
                               PaymentMasterId = t.PaymentMasterId,
                               SaleReturnId = t.SaleReturnId,
                               VendorDepositId = t.VendorDepositId,

                           }).Distinct().ToList();


            var supplier = await _db.Vendors.FindAsync(vmTransaction.VendorFK);
            var companies = await _db.Companies.FindAsync(vmTransaction.CompanyFK);

            //var openingBalance = _db.VendorOpenings.Where(c =>
            //    c.VendorId == vmTransaction.VendorFK && c.IsActive == true && c.IsSubmit == true).ToList();
            VmTransaction vmTransition = new VmTransaction();
            vmTransition.Date = vmTransaction.FromDate;
            vmTransition.Name = supplier.Name;
            vmTransition.Description = "Opening Balance";
            vmTransition.Debit = 0;
            //vmTransition.Credit = openingBalance.Sum(c => c.OpeningAmount);
            vmTransition.Balance = previousBalance;
            vmTransition.CompanyAddress = companies.Address;
            vmTransition.CompanyName = companies.Name;
            vmTransition.CompanyPhone = companies.Phone;
            vmTransition.CompanyEmail = companies.Email;
            vmTransition.FromDate = vmTransaction.FromDate;
            vmTransition.ToDate = vmTransaction.ToDate;



            //tempList.Add(vmTransition);
            sortedV.Insert(0, vmTransition);

            foreach (var x in sortedV)
            {
                x.Balance = previousBalance += x.Credit - x.Debit;
                x.Name = supplier.Name;
                x.OrderMasterId = x.OrderMasterId;
                tempList.Add(x);
            }

            vmTransition.DataList = tempList;

            return vmTransition;
        }

        private decimal GetOrderValue(long orderMasterId)
        {
            double orderValue = (from ts1 in _db.OrderDetails
                                 join ts2 in _db.OrderDeliverDetails on ts1.OrderDetailId equals ts2.OrderDetailId
                                 where ts1.OrderMasterId == orderMasterId && ts1.IsActive && ts2.IsActive && !ts2.IsReturn
                                 select (ts2.DeliveredQty * ts1.UnitPrice) - (double)ts1.DiscountAmount).DefaultIfEmpty(0).Sum();
            return Convert.ToDecimal(orderValue);

        }
        private decimal GetReturnOrderValue(long saleReturnId)
        {
            decimal orderValue = (from ts0 in _db.SaleReturnDetails
                                  join ts1 in _db.OrderDeliverDetails on ts0.OrderDeliverDetailsId equals ts1.OrderDeliverDetailId
                                  where ts0.SaleReturnId == saleReturnId && ts1.IsActive && ts0.IsActive
                                  select new
                                  {
                                      ReturnQuantity = ts0.Qty,
                                      UnitPrice = ts1.UnitPrice
                                  }).AsEnumerable().Select(x => x.ReturnQuantity.Value * Convert.ToDecimal(x.UnitPrice)).DefaultIfEmpty(0).Sum();
            return orderValue;
        }


        public IEnumerable<VmCustomerAgeing> CustomerAgeingGet(VmCustomerAgeing vmCustomerAgeing)
        {
            return _db.Database.SqlQuery<VmCustomerAgeing>("[dbo].[GCCLCustomerAgeing] {0},{1},{2},{3}", vmCustomerAgeing.CompanyFK.Value, vmCustomerAgeing.AsOnDate, vmCustomerAgeing.ZoneId ?? 0, vmCustomerAgeing.SubZoneId ?? 0).AsEnumerable();
        }

        public async Task<VMCommonSupplier> GetCustomer(int companyId)
        {
            VMCommonSupplier vmCommonCustomer = new VMCommonSupplier();
            vmCommonCustomer.CompanyFK = companyId;
            vmCommonCustomer.DataList = await Task.Run(() => (from t1 in _db.Vendors.Where(x => x.IsActive == true && x.VendorTypeId == (int)ProviderEnum.Customer && x.CompanyId == companyId)
                                                              join t2 in _db.Upazilas on t1.UpazilaId equals t2.UpazilaId into t2_join
                                                              from t2 in t2_join.DefaultIfEmpty()
                                                              join t3 in _db.Districts on t2.DistrictId equals t3.DistrictId into t3_join
                                                              from t3 in t3_join.DefaultIfEmpty()
                                                              join t4 in _db.Divisions on t3.DivisionId equals t4.DivisionId into t4_join
                                                              from t4 in t4_join.DefaultIfEmpty()
                                                              join t5 in _db.Countries on t1.CountryId equals t5.CountryId into t5_join
                                                              from t5 in t5_join.DefaultIfEmpty()

                                                              select new VMCommonSupplier
                                                              {
                                                                  ID = t1.VendorId,
                                                                  Name = t1.Name,
                                                                  Email = t1.Email,
                                                                  ContactPerson = t1.ContactName,
                                                                  Address = t1.Address,
                                                                  Code = t1.Code,
                                                                  Common_DistrictsFk = t2.DistrictId > 0 ? t2.DistrictId : 0,
                                                                  Common_UpazilasFk = t1.UpazilaId.Value > 0 ? t1.UpazilaId.Value : 0,
                                                                  District = t3.Name,
                                                                  Upazila = t2.Name,
                                                                  Division = t4.Name,
                                                                  Country = t5.CountryName,
                                                                  CreatedBy = t1.CreatedBy,
                                                                  Remarks = t1.Remarks,
                                                                  CompanyFK = t1.CompanyId,
                                                                  Phone = t1.Phone
                                                              }).OrderByDescending(x => x.ID).AsEnumerable());
            return vmCommonCustomer;
        }


        #region Transaction Summary Service
        public async Task<VmTransaction> DealerTransactionSummaryGet(int companyId)
        {
            VmTransaction vmTransaction = new VmTransaction();
            vmTransaction.CompanyFK = companyId;
            var companies = _db.Companies.Find(companyId);
            vmTransaction.CompanyAddress = companies.Address;
            vmTransaction.CompanyName = companies.Name;
            vmTransaction.CompanyEmail = companies.Email;
            vmTransaction.CompanyPhone = companies.Phone;


            var dataList = await Task.Run(() => (from t1 in _db.Vendors.Where(x => x.IsActive == true && x.VendorTypeId == (int)ProviderEnum.Customer && x.CompanyId == companyId)

                                                 select new VmTransaction
                                                 {
                                                     ID = t1.VendorId,
                                                     Name = t1.Name,
                                                     CompanyEmail = t1.Email,

                                                     CompanyAddress = t1.Address,
                                                     Code = t1.Code,
                                                     CompanyPhone = t1.Phone,
                                                     ReturnAmount = 0,
                                                     LastPaymentsDate = _db.Payments.Where(x => x.VendorId == t1.VendorId).OrderByDescending(x => x.TransactionDate).Select(x => x.TransactionDate).FirstOrDefault(),
                                                     LastPaymentsAmount = _db.Payments.Where(x => x.VendorId == t1.VendorId).OrderByDescending(x => x.TransactionDate).Select(x => x.InAmount).FirstOrDefault(),

                                                     CustomerCredit = (from ts1 in _db.OrderDeliverDetails
                                                                       join ts2 in _db.OrderDetails on ts1.OrderDetailId equals ts2.OrderDetailId
                                                                       join ts3 in _db.OrderMasters.Where(x => x.CustomerId == t1.VendorId) on ts2.OrderMasterId equals ts3.OrderMasterId
                                                                       where ts2.IsActive && ts1.IsActive
                                                                       group new { ts1, ts2, ts3 } by new { ts3.CustomerId } into Group
                                                                       select Group.Sum(x => x.ts1.DeliveredQty * x.ts2.UnitPrice) + Group.FirstOrDefault().ts3.CourierCharge).DefaultIfEmpty(0).Sum(),

                                                     Credit = _db.Payments.Where(x => x.VendorId == t1.VendorId)
                                                          .Select(x => x.InAmount).DefaultIfEmpty(0).Sum()
                                                 }).OrderByDescending(x => x.ID).ToList());
            foreach (var data in dataList)
            {
                data.ReturnAmount = GetReturnOrderValueByCustomer(data.ID);
            }

            vmTransaction.DataList = dataList;


            return vmTransaction;
        }

        public async Task<VmTransaction> SupplierTransactionSummaryGet(int companyId)
        {
            VmTransaction vmTransaction = new VmTransaction();
            vmTransaction.CompanyFK = companyId;
            var companies = _db.Companies.Find(companyId);
            vmTransaction.CompanyAddress = companies.Address;
            vmTransaction.CompanyName = companies.Name;
            vmTransaction.CompanyEmail = companies.Email;
            vmTransaction.CompanyPhone = companies.Phone;


            var dataList = await Task.Run(() => (from t1 in _db.Vendors.Where(x => x.IsActive == true && x.VendorTypeId == (int)ProviderEnum.Supplier && x.CompanyId == companyId)

                                                 select new VmTransaction
                                                 {
                                                     ID = t1.VendorId,
                                                     Name = t1.Name,
                                                     CompanyEmail = t1.Email,

                                                     CompanyAddress = t1.Address,
                                                     Code = t1.Code,
                                                     CompanyPhone = t1.Phone,
                                                     ReturnAmount = 0,
                                                     LastPaymentsDate = _db.Payments.Where(x => x.VendorId == t1.VendorId).OrderByDescending(x => x.TransactionDate).Select(x => x.TransactionDate).FirstOrDefault(),
                                                     LastPaymentsAmount = _db.Payments.Where(x => x.VendorId == t1.VendorId).OrderByDescending(x => x.TransactionDate).Select(x => x.InAmount).FirstOrDefault(),

                                                     SupplierCredit = (from ts1 in _db.MaterialReceiveDetails
                                                                       join ts2 in _db.MaterialReceives on ts1.MaterialReceiveId equals ts2.MaterialReceiveId
                                                                       join ts3 in _db.PurchaseOrders.Where(x => x.SupplierId == t1.VendorId) on ts2.PurchaseOrderId equals ts3.PurchaseOrderId
                                                                       where ts2.IsActive && ts1.IsActive

                                                                       select ts1.ReceiveQty * ts1.UnitPrice).DefaultIfEmpty(0).Sum(),

                                                     Credit = _db.Payments.Where(x => x.VendorId == t1.VendorId)
                                                          .Select(x => x.InAmount).DefaultIfEmpty(0).Sum()
                                                 }).OrderByDescending(x => x.ID).ToList());
            foreach (var data in dataList)
            {
                data.SupplierReturnAmount = GetPurchaseReturnValueBySupplier(data.ID);
            }

            vmTransaction.DataList = dataList;


            return vmTransaction;
        }
        #endregion

        private double GetReturnOrderValueByCustomer(int customerId)
        {
            return (from ts0 in _db.SaleReturnDetails
                    join ts1 in _db.OrderDeliverDetails on ts0.OrderDeliverDetailsId equals ts1.OrderDeliverDetailId
                    join ts2 in _db.SaleReturns on ts0.SaleReturnId equals ts2.SaleReturnId

                    where ts2.CustomerId == customerId && ts1.IsActive && ts0.IsActive
                    select new
                    {
                        ReturnQuantity = ts0.Qty,
                        UnitPrice = ts1.UnitPrice
                    }).AsEnumerable().Select(x => Convert.ToDouble(x.ReturnQuantity.Value) * x.UnitPrice).DefaultIfEmpty(0).Sum();
        }

        private decimal GetPurchaseReturnValueBySupplier(int supplier)
        {
            return (from ts0 in _db.MaterialReceiveDetails
                    join ts1 in _db.MaterialReceives on ts0.MaterialReceiveId equals ts1.MaterialReceiveId
                    join ts2 in _db.PurchaseOrders on ts1.PurchaseOrderId equals ts2.PurchaseOrderId

                    where ts2.SupplierId == supplier && ts1.IsActive && ts0.IsActive && ts0.IsReturn
                    select ts0.ReceiveQty * ts0.UnitPrice).DefaultIfEmpty(0).Sum();
        }

    }
}
